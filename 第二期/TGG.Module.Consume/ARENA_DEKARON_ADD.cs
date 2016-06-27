using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    public class ARENA_DEKARON_ADD : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        /// <summary> 增加挑战次数 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_DEKARON_ADD", "增加挑战次数");
#endif
            var user = session.Player.User.CloneEntity();
            var vip = session.Player.Vip.CloneEntity();

            var base_vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == vip.vip_level);//验证VIP
            if (base_vip.arenaBuy <= vip.arena_buy) return CommonHelper.ErrorResult(ResultType.ARENA_BUY_COUNT_LOCK);

            var arena = tg_arena.FindByUserId(user.id);
            if (arena.buy_count <= 0) arena.buy_count = 1;
            else arena.buy_count += 1;

            var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "23002");
            if (baserule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var temp = baserule.value;
            temp = temp.Replace("n", (vip.arena_buy + 1).ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);

            int money = Convert.ToInt32(express);
            if (money < 0) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            user.gold = user.gold - money;
            if (user.gold < 0) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_GOLD_ERROR);

            arena.count += 1;
            vip.arena_buy += 1;
            arena.Update();
            user.Update();
            vip.Update();
            log.GoldInsertLog(money, user.id, (int)ModuleNumber.ARENA, (int)ArenaCommand.ARENA_DEKARON_ADD); //记录金钱消耗

            session.Player.Vip = vip;
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
# if DEBUG
            XTrace.WriteLine("{0}:{1}  {2} {3} ", "挑战剩余次数", arena.count, "消耗:", user.gold);
#endif
            return new ASObject(BuildData((int)ResultType.SUCCESS, arena.totalCount, arena.count, vip.arena_buy));
        }

        /// <summary>组装数据</summary>
        private Dictionary<String, Object> BuildData(int result, int total, int count, int buycount)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"total",total },
                {"count",count },
                 {"buycount",buycount }
            };
            return dic;
        }
    }
}
