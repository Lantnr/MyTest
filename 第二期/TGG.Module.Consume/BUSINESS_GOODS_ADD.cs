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
using TGG.Core.Vo.Business;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 补充货物
    /// </summary>
    public class BUSINESS_GOODS_ADD : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_GOODS_ADD", "补充货物");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var id = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);
            return Logic(id, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(Int32 id, TGGSession session)
        {
            var player = session.Player.CloneEntity();

            if (player.Vip.buy <= 0) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_GOODS_BUY_ERROR);

            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3014"); //获取基表数据
            if (baserule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var goods = tg_goods_item.FindByid(id);
            if (goods == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            var cost = Convert.ToInt32(baserule.value);
            player.User.gold -= cost;

            player.Vip.buy -= 1;

            player.User.Save();
            player.Vip.Save();

            session.Player = player;
            //元宝更新推送
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            goods.number = goods.number_max;
            goods.Save();

            return BuildData((int)ResultType.SUCCESS, goods);
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, tg_goods_item goods)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"goods",ConvertBusinessGoodsVo(goods)}
            };
            return new ASObject(dic);
        }

        /// <summary> tg_goods_item 转换 BusinessGoodsVo</summary>
        /// <param name="goods">tg_goods_item</param>
        private BusinessGoodsVo ConvertBusinessGoodsVo(tg_goods_item goods)
        {
            var bg = Variable.GOODS.FirstOrDefault(m => m.goods_id == goods.goods_id && m.ting_id == goods.ting_id);
            return new BusinessGoodsVo()
            {
                id = goods.id,
                baseId = goods.goods_id,
                count = goods.number,
                priceBuy = bg.goods_buy_price,
                priceSell = bg.goods_sell_price,
            };
        }
    }
}
