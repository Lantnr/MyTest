using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 攻大将
    /// </summary>
    public class ATTACK_BOSS
    {
        private static ATTACK_BOSS _objInstance;

        /// <summary>ATTACK_BOSS单体模式</summary>
        public static ATTACK_BOSS GetInstance()
        {
            return _objInstance ?? (_objInstance = new ATTACK_BOSS());
        }

        /// <summary> 攻大将</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var playerdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);
            var rivalcamp = user.player_camp == (int)CampType.East ? (int)CampType.West : (int)CampType.East;//对手阵营
            var boss = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == rivalcamp);//对手BOSS数据
            var result = IsData(playerdata, boss);
            if (result != ResultType.SUCCESS) return new ASObject(Common.GetInstance().BuildData((int)result));

            var npcboss = Variable.BASE_NPCSIEGE.FirstOrDefault(m => m.type == (int)SiegeNpcType.BOSS && m.camp == rivalcamp);
            if (npcboss == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));//验证基表数据

            Int64 hrut = 0;
            var vo = GetFightResult(user.id, npcboss.armyId, boss.BossLife, ref hrut);  //调用战斗
            if (vo == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_ERROR));//验证战斗

            boss = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == rivalcamp);//对手BOSS数据
            lock (this)
            {
                boss.BossLife -= hrut;
                if (boss.BossLife <= 0) playerdata.fame += Variable.Activity.Siege.BaseData.WinFame;
                playerdata.hurt += hrut;
                playerdata.count -= Variable.Activity.Siege.BaseData.BossLadder;
                PUSH_OURS_HP.GetInstance().CommandStart(user.player_camp, rivalcamp, (int)SiegeNpcType.BOSS, Convert.ToInt32(boss.BossLife), 0);
            }


            var times = Variable.Activity.Siege.BaseData.AShotTime;
            int count = vo.moves.Sum(item => item.Count());
            if (count < 5) times += 1000;
            var time = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000);
            playerdata.bosstime = time + (count * times);
            return new ASObject(BuildData((int)ResultType.SUCCESS, playerdata.count, vo));
        }

        /// <summary> 数据验证 </summary>
        /// <param name="playerdata">要验证的玩家活动数据</param>
        /// <param name="boss">boss</param>
        private ResultType IsData(SiegePlayer playerdata, CampCondition boss)
        {
            if (playerdata.count < Variable.Activity.Siege.BaseData.BossLadder) return ResultType.SIEGE_YUNTI_ERROR;//验证云梯
            var time = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000);
            if (playerdata.bosstime > time) return ResultType.ARENA_TIME_ERROR;             //验证间隔
            if (boss == null) return ResultType.BASE_TABLE_ERROR;                     //验证全局Boss数据
            if (boss.GateLife > 0 || boss.BossLife <= 0) return ResultType.SIEGE_BOSS_ERROR;//验证城门是否破坏
            return ResultType.SUCCESS;
        }

        /// <summary> 获取战斗结果 </summary>
        private FightVo GetFightResult(Int64 userid, Int64 rivalid, Int64 life, ref Int64 hrut)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, rivalid, FightType.SIEGE, life, true, true);
            new Share.Fight.Fight().Dispose();
            if (fight.Result != ResultType.SUCCESS) return null;
            hrut = fight.Hurt;
            return fight.Ofight;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, int count, FightVo vo)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"ladder", count},
                {"fight", vo},
            };
            return dic;
        }
    }
}
