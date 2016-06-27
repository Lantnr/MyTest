using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 战斗技能加速
    /// </summary>
    public class SKILL_FIGHT_ACCELERATE
    {
        private static SKILL_FIGHT_ACCELERATE _objInstance;

        /// <summary> SKILL_FIGHT_ACCELERATE单体模式 </summary>
        public static SKILL_FIGHT_ACCELERATE GetInstance()
        {
            return _objInstance ?? (_objInstance = new SKILL_FIGHT_ACCELERATE());
        }

        /// <summary> 战斗技能加速</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_FIGHT_ACCELERATE", "战斗技能加速");
#endif
            var sid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            if (sid == 0) return Error((int)ResultType.FRONT_DATA_ERROR);
            var player = session.Player.CloneEntity();

            var skill = tg_role_fight_skill.GetBySkillId(sid);
            if (skill == null) return Error((int)ResultType.SKILL_GET_ERROR);     //验证技能信息是否为空

            var current = Common.GetInstance().CurrentTime();
            if (skill.skill_time <= current || skill.skill_state == (int)SkillLearnType.LEARNED)
                return Error((int)ResultType.SKILL_LEARN_UP_ERROR);      //验证技能已学习或已经升级

            var cost = Common.GetInstance().Consume(skill.skill_time);
            var gold = player.User.gold;
            if (player.User.gold < cost) return Error((int)ResultType.BASE_PLAYER_GOLD_ERROR);     //验证玩家元宝信息
            player.User.gold -= cost;
            player.User.Update();
            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, player.User);
            log.GoldInsertLog(cost, player.User.id, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_ACCELERATE);  //玩家消费金币记录

            CheckSkillLearnOrUp(skill, player.User.id);     //根据等级打断线程并更新加速完成的技能信息

            //记录元宝花费日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, cost, player.User.gold);
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Use, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_ACCELERATE, logdata);

            var rolevo = (new Share.Role()).BuildRole(skill.rid);
            return new ASObject(Common.GetInstance().DataBuild((int)ResultType.SUCCESS, rolevo));
        }

        /// <summary>
        /// 验证加速类型是学习还是升级 打断线程
        /// </summary>
        /// <param name="skill"></param>
        /// /// <param name="userid"></param>
        private void CheckSkillLearnOrUp(tg_role_fight_skill skill, Int64 userid)
        {
            if (skill.skill_level < 1)    //学习技能
            {
                var key = string.Format("{0}_{1}_{2}_{3}", (int)CDType.FightSkillLearn, userid, skill.rid, skill.skill_id);   //为打断线程加入全局变量cd(学习线程)
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            }
            else
            {
                var key = string.Format("{0}_{1}_{2}", (int)CDType.FightSkillUp, userid, skill.id);     //为打断线程加入全局变量cd(升级线程)
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            }
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().DataBuild(error, null));
        }
    }
}
