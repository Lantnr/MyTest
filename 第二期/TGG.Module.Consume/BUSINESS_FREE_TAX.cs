using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 免税
    /// </summary>
    public class BUSINESS_FREE_TAX : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_FREE_TAX", "免税");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            return Logic(session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(TGGSession session)
        {
            var player = session.Player.CloneEntity();

            var base_vip = Variable.BASE_VIP.FirstOrDefault(q => q.level == player.Vip.vip_level); //获取基表数据
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3016"); //获取基表数据
            if (base_vip == null || baserule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            //VIP 达到6级开放
            if (base_vip.freeTax == 0)
                return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_VIP_ERROR);

            var cost = Convert.ToInt32(baserule.value);
            player.User.gold -= cost;

            player.Order.istaxes = true;

            player.User.Save();
            session.Player = player;
            //元宝更新推送
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            return new ASObject(BuildData((int)ResultType.SUCCESS));

        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return new ASObject(dic);
        }
    }
}
