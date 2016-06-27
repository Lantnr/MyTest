using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 领取每日完成游戏次数的奖励
    /// </summary>
    public class GAMES_RECEIVE
    {
        private static GAMES_RECEIVE _objInstance;

        /// <summary>
        /// GAMES_RECEIVE单体模式
        /// </summary>
        public static GAMES_RECEIVE GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_RECEIVE());
        }

        /// <summary>领取每日完成游戏次数的奖励</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_RECEIVE", "领取每日完成游戏次数的奖励");
#endif
            var ex = session.Player.UserExtend.CloneEntity();

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30004");   //每日完成度次数
            if (rule == null) return Result((int)ResultType.BASE_TABLE_ERROR);

            if (ex.game_finish_count < Convert.ToInt32(rule.value))    //完成度未达到
                return Result((int)ResultType.GAME_REWARD_UNFINISH);

            if (ex.game_receive != (int)GameRewardType.TYPE_CANREWARD)   //未达到领奖条件
                return Result((int)ResultType.GAME_REWARD_ERROE);

            if (ex.game_receive == (int)GameRewardType.TYPE_REWARDED)  //奖励已领取
                return Result((int)ResultType.GAME_REWARD_RECEIVED);

            if (!Reward(ex.user_id)) return Result((int)ResultType.REWARD_FALSE);   //验证奖励领取信息

            ex.game_receive = (int)GameRewardType.TYPE_REWARDED;
            if (!tg_user_extend.GetUpdate(ex)) return Result((int)ResultType.DATABASE_ERROR);
            session.Player.UserExtend = ex;

            return Result((int)ResultType.SUCCESS);
        }

        /// <summary>领取奖励信息</summary>
        private bool Reward(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            var reward = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30003");
            if (reward == null || string.IsNullOrEmpty(reward.value)) return false;
            return (new Share.Reward()).GetReward(reward.value, userid);
        }

        /// <summary>返回数据</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
