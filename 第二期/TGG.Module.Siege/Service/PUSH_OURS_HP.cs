using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 推送我方攻城hp数据
    /// </summary>
    public class PUSH_OURS_HP
    {
        private static PUSH_OURS_HP _objInstance;

        /// <summary>PUSH_OURS_HP单体模式</summary>
        public static PUSH_OURS_HP GetInstance()
        {
            return _objInstance ?? (_objInstance = new PUSH_OURS_HP());
        }

        /// <summary> 推送我方攻城hp数据 </summary>
        /// <param name="camp">己方阵营</param>
        /// <param name="rivalcamp">对方阵营</param>
        /// <param name="type">NPC 类型</param>
        /// <param name="life">剩余血量</param>
        /// <param name="fame">声望</param>
        public void CommandStart(int camp, int rivalcamp, int type, int life, int fame)
        {
            if (life < 0) life = 0;
            if (type == (int)SiegeNpcType.BOSS && life <= 0)  //验证是否大将死亡
            {
                SendBossRewardAndHp(camp, rivalcamp, type, life);
                return;
            }

            foreach (var item in Variable.Activity.Siege.PlayerData.Where(item => Common.GetInstance().IsActivities(item.user_id)))
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var sp = m as SiegePlayer;
                    if (sp == null) return;
                    SendBossHp(sp.user_id, rivalcamp, type, life, sp.fame);
                    token.Cancel();
                }, item, token.Token);
            }

            if (type != (int)SiegeNpcType.BASE) return;
            if (life <= 0) PUSH_END.GetInstance().CommandStart(rivalcamp == (int)CampType.East ? SiegeResultType.EAST_LOSE : SiegeResultType.EAST_WIN);
        }

        /// <summary> Boss伤害奖励计算并Send </summary>
        /// <param name="camp">己方阵营</param>
        /// <param name="rivalcamp">对方阵营</param>
        /// <param name="type">类型</param>
        /// <param name="life">Boss血量</param>
        private void SendBossRewardAndHp(int camp, int rivalcamp, int type, int life)
        {
            foreach (var item in Variable.Activity.ScenePlayer.Keys)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var key = Convert.ToString(m);
                    if (!Variable.Activity.ScenePlayer.ContainsKey(key)) return;
                    var userid = Variable.Activity.ScenePlayer[key].user_id;
                    var playdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(q => q.user_id == userid);
                    if (playdata == null) return;
                    if (playdata.player_camp == camp)
                    {
                        var reward = GetBaseSiegeBossReward(playdata.hurt);
                        if (reward == null) return;
                        playdata.fame += reward.fameReward;
                    }
                    SendBossHp(userid, rivalcamp, type, life, playdata.fame);
                    token.Cancel();
                }, item, token.Token);
            }
        }

        /// <summary> 根据伤害获取符合的奖励数据 </summary>
        /// <param name="hurt">伤害值</param>
        private BaseSiegeBossReward GetBaseSiegeBossReward(Int64 hurt)
        {
            return Variable.BASE_SIEGEBOSSREWARD.Where(m => m.hrut <= hurt)
                           .OrderByDescending(m => m.hrut)
                           .FirstOrDefault();
        }

        /// <summary> Send boss血量给玩家 </summary>
        /// <param name="userid">要Send的玩家Id</param>
        /// <param name="camp">boss的阵营</param>
        /// <param name="type">boss的类型</param>
        /// <param name="life">boss血量</param>
        /// <param name="fame">玩家当前声望</param>
        private void SendBossHp(Int64 userid, int camp, int type, int life, int fame)
        {
            if (!Common.GetInstance().IsActivities(userid)) return;
            var aso = new ASObject(BuildData(camp == (int)CampType.East, type, life, fame));
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            Common.GetInstance().TrainingSiegeEndSend(session, aso, (int)SiegeCommand.PUSH_OURS_HP);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(bool flag, int type, int hp, int fame)
        {
            var dic = new Dictionary<string, object>
            {
                {"flag", flag},
                {"type", type},
                {"hp", hp},
                {"fame", fame},
                
            };
            return dic;
        }
    }
}
