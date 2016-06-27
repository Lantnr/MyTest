using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 购买的议价次数
    /// </summary>
    public class BUSINESS_BUY_BARGAIN : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_BUY_BARGAIN", "购买的议价次数");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var count = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value);

            return Logic(count, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(Int32 count, TGGSession session)
        {
            var player = session.Player.CloneEntity();

            if (player.Vip.bargain <= 0 || player.Vip.bargain < count)
                return CommonHelper.ErrorResult((int)ResultType.BUSINESS_VIP_COUNR_ERROR);

            player.Vip.bargain -= count;
            player.UserExtend.bargain_count -= count;

            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3013"); //获取基表数据
            if (baserule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var cost = Convert.ToInt32(baserule.value) * count;

            if (player.User.gold < cost) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_GOLD_ERROR);
            player.User.gold -= cost;


            //数据库操作
            //using (var scope = new TransactionScope())//事务
            try
            {
                player.Vip.Save();
                player.UserExtend.Save();
                player.User.Save();

            }
            catch { return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR); }

            session.Player = player;
            //元宝更新推送
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            var _count = (new Share.Business()).GetBargainCount(player);

            return BuildData((int)ResultType.SUCCESS, _count, session.Player.Vip.bargain);
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, int count, int bargain)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"count",count},
                {"bargain",bargain},
            };
            return new ASObject(dic);
        }
    }
}
