using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    public class ARENA_REMOVE_COOLING : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        /// <summary> 清除冷却 </summary>
        private ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_REMOVE_COOLING", "清除冷却");
#endif
            var user = session.Player.User.CloneEntity();
            var vip = session.Player.Vip.CloneEntity();
            var base_vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == vip.vip_level);

            var arena = tg_arena.FindByUserId(user.id);

            decimal timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var time = arena.time - timeStamp;
            if (time <= 0) return CommonHelper.ErrorResult(ResultType.ARENA_TIME_ERROR);

            if (vip.arena_cd < base_vip.arenaCD) return VipRemove_Cooling(arena, vip, session);

            var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "23003");
            if (baserule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var temp = baserule.value;
            var min = time / 60000;
            temp = temp.Replace("minute", min.ToString("0.00"));

            var express = CommonHelper.EvalExpress(temp);
            int money = Convert.ToInt32(express);
            user.gold = user.gold - money;
            if (user.gold < 0) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_GOLD_ERROR);

            arena.time = 0;
            arena.Update(); user.Update();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
            log.GoldInsertLog(money, user.id, (int)ModuleNumber.ARENA, (int)ArenaCommand.ARENA_REMOVE_COOLING);//金钱消耗记录
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_REMOVE_COOLING", "清除冷却成功");
#endif
            return new ASObject(BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> VIP清除冷却 </summary>
        private ASObject VipRemove_Cooling(tg_arena arena, tg_user_vip vip, TGGSession session)
        {
            vip.arena_cd += 1;
            arena.time = 0;
            arena.Update();
            vip.Update();
            session.Player.Vip = vip;
            return new ASObject(BuildData((int)ResultType.SUCCESS));
        }

        /// <summary>组装数据</summary>
        private Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return dic;
        }
    }
}
