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
using TGG.Core.Vo.Fight;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 挑战
    /// </summary>
    public class TRAIN_HOME_NPC_FIGHT
    {
        private static TRAIN_HOME_NPC_FIGHT _objInstance;

        /// <summary>TRAIN_HOME_NPC_FIGHT单体模式</summary>
        public static TRAIN_HOME_NPC_FIGHT GetInstance()
        {
            return _objInstance ?? (_objInstance = new TRAIN_HOME_NPC_FIGHT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_NPC_FIGHT", "挑战");
#endif
                var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());
                var userid = session.Player.User.id;
                var npc = tg_train_home.GetNpcById(npcid);
                if (npc == null) return Error((int)ResultType.TRAIN_HOME_GET_ERROR);    //验证武将宅数据库npc信息
                if (npc.npc_state == (int)FightResultType.WIN) return Error((int)ResultType.TRAIN_HOME_FIGHT_YES);     //验证武将是否已经挑战

                if (!CheckCount(session.Player.UserExtend.fight_count))
                    return Error((int)ResultType.TRAIN_HOME_FIGHT_LACK);   //验证剩余挑战次数
                if (!CheckPower(session.Player.Role.Kind)) return Error((int)ResultType.BASE_ROLE_POWER_ERROR);

                var fight = NpcChallenge(userid, npc.npc_id, FightType.NPC_MONSTER, npc.npc_type);      //战斗结果
                if (fight == null) return Error((int)ResultType.FIGHT_ERROR);    //验证战斗是否出错

                var basenpc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == npc.npc_id);
                if (basenpc == null) return Error((int)ResultType.BASE_TABLE_ERROR);
                if (basenpc.limit == (int)TrainHomeLimitType.JUST_TEA) return Error((int)ResultType.TRAIN_HOME_NO_CHALLENGE);   //验证武将是否限制不能挑战

                return ChallengeResult(session, basenpc, npc, fight);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>得到战斗vo</summary>
        private FightVo NpcChallenge(Int64 userid, int npcid, FightType type, int lv)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, npcid, type, 0, true, false, false, false, lv);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS ? null : fight.Ofight;
        }

        /// <summary>对战结果处理</summary>
        private ASObject ChallengeResult(TGGSession session, BaseNpcMonster npcinfo, tg_train_home npc, FightVo fight)
        {
            var isprop = false;
            var exp = npcinfo.experience;
            var equips = npcinfo.equip;
            var probability = npcinfo.probability;
            var reward = new List<RewardVo>();
            var propreward = new List<RewardVo>();

            if (fight.isWin)
            {
                PowerUpdate(fight.isWin, session.Player.Role.Kind); //体力更新
                reward = BuildReward(exp);
                RewardExp(session, exp);       //推送经验奖励
                UpdateExt(session.Player.User.id);

                npc.npc_state = (int)FightResultType.WIN; //更新npc状态为已战胜
                npc.Update();
                var newequip = Common.GetInstance().RandomEquip(session.Player.User.id, equips, probability);    //是否获得装备
                isprop = newequip != null;
                if (newequip != null)
                {
                    Variable.TempProp.AddOrUpdate(session.Player.User.id, new List<tg_bag>() { newequip }, (m, n) => n);
                    var listaso = new List<ASObject> { AMFConvert.ToASObject(EntityToVo.ToEquipVo(newequip)) };
                    propreward.Add(new RewardVo
                    {
                        goodsType = (int)GoodsType.TYPE_EQUIP,
                        increases = listaso,
                    });
                }
            }
            FightSend(fight, isprop, reward, session.Player.User.id, propreward); //发送战斗协议

            (new Share.DaMing()).CheckDaMing(session.Player.User.id, (int)DaMingType.武将宅挑战);
            return new ASObject(Common.GetInstance().NpcChallengeData((int)ResultType.SUCCESS, fight.isWin ? (int)FightResultType.WIN : (int)FightResultType.LOSE));
        }

        /// <summary>获得经验奖励</summary>
        private void RewardExp(TGGSession session, int experience)
        {
            var role = session.Player.Role.Kind.CloneEntity();
            var otherroles = tg_role.GetWarRoles(session.Player.User.id);
            otherroles.Add(role);
            foreach (var otherrole in otherroles)
            {
                var exp = otherrole.role_exp;
                var level = otherrole.role_level;

                var userid = session.Player.User.id;
                var count = experience;
                if (otherrole.role_state == (int)RoleStateType.PROTAGONIST)
                {
                    new Share.Upgrade().UserLvUpdate(userid, count, otherrole);  //用户是否升级
                    RecordLog(userid, exp, count, level, otherrole);   //记录日志
                    continue;
                }
                new Share.Upgrade().RoleLvUpdate(session.Player.User.id, count, otherrole);
                RecordLog(userid, exp, count, level, otherrole);   //记录日志
            }
            session.Player.Role.Kind = role;
        }

        /// <summary>更新extend 已挑战次数</summary>
        private void UpdateExt(Int64 userid)
        {
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var ext = session.Player.UserExtend.CloneEntity();
            ext.fight_count++;
            ext.Update();
            session.Player.UserExtend = ext;
        }

        /// <summary> 组装奖励 </summary>
        private List<RewardVo> BuildReward(int exp)
        {
            var reward = new List<RewardVo> { new RewardVo { goodsType = (int)GoodsType.TYPE_EXP, value = exp } };
            return reward;
        }

        /// <summary>
        /// 战斗协议发送
        /// </summary>
        private void FightSend(FightVo fight, Boolean isprop, List<RewardVo> reward, Int64 userid, List<RewardVo> proRewardVos)
        {
            fight.rewards = reward;
            fight.propReward = proRewardVos;
            fight.haveProp = isprop ? 1 : 0;
            (new Share.Fight.Fight()).SendProtocol(userid, fight);
        }

        /// <summary>验证体力是否足够</summary>
        private bool CheckPower(tg_role role)
        {
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            return totalpower >= power;
        }

        /// <summary>验证挑战次数</summary>
        private bool CheckCount(int ucount)
        {
            var f = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17020");
            if (f == null) return false;
            return ucount < Convert.ToInt32(f.value);
        }

        /// <summary>
        /// 主角体力更新
        /// </summary>
        /// <param name="iswin">战斗结果</param>
        /// <param name="role"></param>
        private void PowerUpdate(bool iswin, tg_role role)
        {
            if (!iswin) return;
            var power = RuleConvert.GetCostPower();
            new Share.Role().PowerUpdateAndSend(role, power, role.user_id);
        }

        /// <summary>记录经验变动日志</summary>
        private void RecordLog(Int64 userid, int exp, int count, int level, tg_role role)
        {
            var logdata = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", "Exp", level, exp, count, role.role_level, role.role_exp, role.id);     //记录获得经验变动信息
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_FIGHT, logdata);
        }

        /// <summary>返回错误信息</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BuilData(result));
        }
    }
}
