using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 偷窃
    /// </summary>
    public class TRAIN_HOME_NPC_STEAL
    {
        private static TRAIN_HOME_NPC_STEAL _objInstance;

        /// <summary>TRAIN_HOME_NPC_STEAL单体模式</summary>
        public static TRAIN_HOME_NPC_STEAL GetInstance()
        {
            return _objInstance ?? (_objInstance = new TRAIN_HOME_NPC_STEAL());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_NPC_STEAL", "偷窃");
#endif
                var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());     //偷窃npc主键id
                var player = session.Player.CloneEntity();

                var check = CheckResult(player);
                if (check != (int)ResultType.SUCCESS) return Error(check);  //验证信息

                var npc = tg_train_home.GetNpcById(npcid);
                if (npc == null) return Error((int)ResultType.TRAIN_HOME_GET_ERROR);
                if (npc.npc_state == (int)FightResultType.WIN) return Error((int)ResultType.TRAIN_ROLE_NO_STEAL);   //验证武将是否能偷窃
                if (npc.is_steal == (int)TrainHomeStealType.STEAL_YES) return Error((int)ResultType.TRAIN_HOME_STEAL_YES);

                var basenpc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == npc.npc_id);
                if (basenpc == null || string.IsNullOrEmpty(basenpc.equip))
                    return Error((int)ResultType.BASE_TABLE_ERROR);   //判断npc基表信息

                if (!(new Share.RoleTrain()).PowerOperate(player.Role.Kind)) return Error((int)ResultType.BASE_ROLE_POWER_ERROR);
                return StealNpc(session, player.UserExtend, basenpc, npc);       //处理偷窃npc信息
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>偷窃逻辑处理</summary>
        private ASObject StealNpc(TGGSession session, tg_user_extend extend, BaseNpcMonster basenpc, tg_train_home npc)
        {
            var userid = session.Player.User.id;
            var nprob = GetProbability(session.Player.Role.LifeSkill.sub_ninjitsu, session.Player.Role.LifeSkill.sub_ninjitsu_level);  //忍术加成概率
            var bprob = GetBrainProb(session.Player.Role);  //智谋加成概率
            var equip = Common.GetInstance().RandomEquip(userid, basenpc, nprob, bprob);

            if (equip != null)   //偷窃成功
            {
                npc.is_steal = (int)TrainHomeStealType.STEAL_YES;
                if (!tg_train_home.UpdateNpc(npc)) return Error((int)ResultType.DATABASE_ERROR);
                AcquireEquip(session, equip);   //获得装备信息处理

                var logdata = string.Format("{0}_{1}", "EquipSteal", equip.base_id);     //记录偷窃获得装备信息
                (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_STEAL, logdata);
                log.BagInsertLog(equip, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_STEAL, 0);   //记录日志
            }
            else
            {
                extend.steal_fail_count++;
                extend.Update();
                session.Player.UserExtend = extend;
                if (IsEnterPrison(extend.steal_fail_count)) { new Share.Prison().PutInPrison(userid); }  //推送进入监狱
            }
            (new Share.DaMing()).CheckDaMing(userid, (int)DaMingType.武将宅偷窃);
            return new ASObject(Common.GetInstance().NpcStealData((int)ResultType.SUCCESS, npc.is_steal));
        }

        /// <summary>验证信息</summary>
        private int CheckResult(Player player)
        {
            var myscene = Scene.GetSceneInfo((int)ModuleNumber.PRISON, player.User.id);
            if (myscene != null) return ((int)ResultType.TRAIN_HOME_IN_PRISON);   //验证是否在监狱中

            if (player.Bag.BagIsFull || player.Bag.Surplus == 0)
                return ((int)ResultType.BAG_ISFULL_ERROR);   //验证背包已满
            if (!IsLearnTouTian(player.Role.Kind.id))
                return ((int)ResultType.TRAIN_HOME_TOUTIAN_UNLEARN);     //验证是否学习了偷天术

            return (int)ResultType.SUCCESS;
        }

        /// <summary>验证是否学习了偷天术</summary>
        private bool IsLearnTouTian(Int64 rid)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17012");
            if (rule == null || string.IsNullOrEmpty(rule.value)) return false;
            var skillid = Convert.ToInt32(rule.value);
            return tg_role_fight_skill.GetFightSkill(rid, skillid);
        }

        /// <summary>忍术等级获得的偷窃加成概率</summary>
        private int GetProbability(int ninjaid, int ninjalevel)
        {
            var probability = 0;
            var steal = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == ninjaid && m.level == ninjalevel);
            if (steal == null) return probability;
            if (string.IsNullOrEmpty(steal.effect)) return probability;
            if (!steal.effect.Contains("_")) return probability;
            var addtion = steal.effect.Split("_").ToList();
            var value = addtion[3];
            probability = probability + Convert.ToInt32(value);
            return probability;
        }

        /// <summary>智谋加成概率值</summary>
        private int GetBrainProb(RoleItem role)
        {
            var prob = 0;
            var list = Variable.BASE_STEAL_PROB;
            if (!list.Any()) return prob;
            var first = list.First();
            var myvalue = (new Share.TGTask()).GetLifeSkillType(role, first.skillOrAtt);
            var data = list.FirstOrDefault(q => q.value >= myvalue);
            if (data == null) return prob;
            prob = data.prob;
            return prob;
        }

        /// <summary>判断是否进监狱</summary>
        private bool IsEnterPrison(int failcount)
        {
            var condition = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17009");
            if (condition == null) return false;
            var count = Convert.ToInt32(condition.value);
            return failcount >= count;
        }

        /// <summary>偷窃获得装备</summary>
        private void AcquireEquip(TGGSession session, tg_bag equip)
        {
            var bag = session.Player.Bag;
            bag.Surplus = bag.Surplus - 1;
            if (bag.Surplus == 0) bag.BagIsFull = true;
            if (equip.Insert() <= 0) return;
            (new Share.User()).REWARDS_API(session.Player.User.id, BuildReward(equip));
        }

        /// <summary>组装奖励信息</summary>
        private List<RewardVo> BuildReward(tg_bag equip)
        {
            var aso = new List<ASObject>(); { aso.Add(AMFConvert.ToASObject(EntityToVo.ToEquipVo(equip))); }
            var reward = new List<RewardVo>();
            var vo = new RewardVo()
            {
                goodsType = (int)GoodsType.TYPE_EQUIP,
                increases = aso,
            };
            reward.Add(vo);
            return reward;
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BulidData(error));
        }
    }
}
