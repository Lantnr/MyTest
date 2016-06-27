using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary> 推送攻击城门结果</summary>
    public class PUSH_GATE_RESULT
    {
        public static PUSH_GATE_RESULT ObjInstance;

        /// <summary>PUSH_GATE_RESULT单体模式</summary>
        public static PUSH_GATE_RESULT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PUSH_GATE_RESULT());
        }

        /// <summary> 推送攻击城门结果</summary>
        public void CommandStart(TGGSession session)
        {
            var user = session.Player.User;
            var role = session.Player.Role.Kind;
            var skill = session.Player.Role.LifeSkill;

            var playerdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);     //拉取活动数据
            
            var flag = (new RandomSingle()).IsTrue(GetBaseProbability(skill)); //是否破坏成功
            if (!flag) Common.GetInstance().TrainingSiegeEndSend(session, new ASObject(BuildData(false, playerdata.count)), (int)SiegeCommand.PUSH_GATE_RESULT);//失败推送
            else
            {
                var rivalcamp = playerdata.player_camp == (int)CampType.East ? (int)CampType.West : (int)CampType.East;   //对方阵营
                var boss = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == rivalcamp);
                if (boss == null) return;
                playerdata.count -= Variable.Activity.Siege.BaseData.GateLadder;   //扣除云梯
                var count = GetBaseHurt(role);  //获取伤害
                lock (this) { boss.GateLife -= count; }

                Common.GetInstance().TrainingSiegeEndSend(session, new ASObject(BuildData(true, playerdata.count)), (int)SiegeCommand.PUSH_GATE_RESULT);//成功推送
                PUSH_OURS_HP.GetInstance().CommandStart(user.player_camp, rivalcamp, (int)SiegeNpcType.GATE, boss.GateLife, 0);
            }
        }

        /// <summary> 获取攻击城门的几率 </summary>
        /// <param name="skill">生活技能</param>
        /// <returns>几率</returns>
        private int GetBaseProbability(tg_role_life_skill skill)
        {
            var list = Common.GetInstance().GetBaseSieges((int)SiegeType.GATE_ODDS);
            if (!list.Any()) return 0;
            var basesiege = list.FirstOrDefault();
            if (basesiege == null) return 0;
            var level = Common.GetInstance().GetLifeLevel(basesiege.skillType, skill);
            var bl = list.FirstOrDefault(m => m.level <= level);
            return bl == null ? 0 : bl.probability;
        }

        /// <summary> 获取对城门的伤害 </summary>
        /// <param name="role">武将信息</param>
        /// <returns>伤害值</returns>
        private int GetBaseHurt(tg_role role)
        {
            var list = Common.GetInstance().GetBaseSieges((int)SiegeType.GATE_HURT);
            if (!list.Any()) return 0;
            var basesiege = list.FirstOrDefault();
            if (basesiege == null) return 0;
            var values = tg_role.GetSingleTotal(Common.GetInstance().ConverType(basesiege.ributeType), role);//获取主角武将属性
            var bl = list.Where(m => m.ributeValues <= values).OrderByDescending(m => m.ributeValues).FirstOrDefault() ??
                     list.OrderBy(m => m.id).FirstOrDefault();
            return bl == null ? 0 : bl.count;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(bool istrue, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"isSuccess", istrue},
                {"ladder", count},
            };
            return dic;
        }
    }
}
