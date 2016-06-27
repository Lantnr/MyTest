using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    ///价格锁定
    /// </summary>
    public class BUSINESS_PRICE_LOCK
    {
        private static BUSINESS_PRICE_LOCK ObjInstance;

        /// <summary>BUSINESS_PRICE_LOCK单体模式</summary>
        public static BUSINESS_PRICE_LOCK GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_PRICE_LOCK());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_PRICE_LOCK", "价格锁定");
#endif
            var price = 0;
            var carmainid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "carId").Value);
            var goodsmainid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "goodsId").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);

            //验证商圈
            if (!(new Share.Business()).IsArea(session.Player.Order.current_ting_base_id, session.Player.BusinessArea))
            {
                return CommonHelper.ErrorResult(ResultType.BUSINESS_TIME_OPEN_ERROR);
            }


            var goodsid = GetGoodsId(type, goodsmainid, session);
            if (goodsid == 0) return CommonHelper.ErrorResult(ResultType.BUSINESS_GOODS_LACK);

            var businessgoods = tg_goods_business.GetByGoodsidAndUserid(session.Player.User.id, goodsid, carmainid);

            //全局货物查询
            var goodsinfo = Variable.GOODS.FirstOrDefault(q => q.ting_id == session.Player.Order.ting_base_id
                && q.goods_id == goodsid);
            if (goodsinfo != null && ((type == 0 && goodsinfo.goods_buy_price == 0) || goodsinfo.goods_sell_price == 0))
                return new ASObject(BuildData(null, 0, 0, (int)ResultType.UNKNOW_ERROR, session.Player.Vip.bargain));
            var buyprice = type == 0 ? goodsinfo.goods_buy_price : businessgoods.price;
            var sell = IsSameGoods(session.Player.Order.current_ting_base_id, goodsinfo.goods_id)
                ? goodsinfo.goods_buy_price
                : goodsinfo.goods_sell_price;


            session.Player.Order.vocation = session.Player.User.player_vocation;
            GetOrder((int)goodsid, goodsmainid, session.Player.Order, sell, carmainid, buyprice); //session中的订单赋值

            if (type == 1)
            {
                CheckGoodsPrice(session.Player.Order); //卖货物时町中已有货物价格减半
                price = session.Player.Order.sell_price_ok;
            }
            else
            {
                price = session.Player.Order.buy_price_ok;
            }
            var order = session.Player.Order.CloneEntity();

            var count = (new Share.Business()).GetBargainCount(session.Player); //剩余讲价次数
            var goodsvo = BuildGoodsVo(order, goodsmainid, type);
            return new ASObject(BuildData(goodsvo, count, price, (int)ResultType.SUCCESS, session.Player.Vip.bargain));
        }

        /// <summary>验证町中是否已有改货物。已有的货物价格减半</summary>
        public void CheckGoodsPrice(BusinessOrder order)
        {
            var base_ting = Variable.BASE_TING.FirstOrDefault(q => q.id == order.ting_base_id);
            if (base_ting == null) return;
            var listgoods = base_ting.goods.Split(',').ToList();
            if (!listgoods.Contains(order.goods_id.ToString())) return;
            //order.sell_price = Convert.ToInt32(order.sell_price * 0.5);
            order.sell_price_ok = Convert.ToInt32(order.sell_price_ok * 0.5);
        }

        /// <summary>当前町是否有相同货物</summary>
        /// <param name="ting">町</param>
        /// <param name="goodsid">货物id</param>
        private bool IsSameGoods(int ting, int goodsid)
        {
            return Variable.BASE_TING.Count(m => m.id == ting && m.goods.Contains(goodsid.ToString())) > 0;
        }

        /// <summary>获取买卖货物id</summary>
        private Int64 GetGoodsId(int type, Int64 mainid, TGGSession session)
        {
            if (type == 1) //卖
            {
                var goodsbusiness = tg_goods_business.FindByid(mainid);
                if (goodsbusiness == null) return 0;
                session.Player.Order.sell_count_max = goodsbusiness.goods_number;
                return goodsbusiness.goods_id;
            }
            var tinggoods = tg_goods_item.FindByid(mainid);
            if (tinggoods == null) return 0;
            session.Player.Order.buy_count_max = tinggoods.number;
            return tinggoods.goods_id;
        }

        /// <summary>组装订单数据</summary>
        private void GetOrder(int goodsid, Int64 goodsmainid, BusinessOrder order, int sellprice, Int64 carmainid, int buyprice)
        {
            order.car_mainid = carmainid;
            order.goods_main_id = goodsmainid;
            order.goods_id = goodsid;
            order.sell_price_ok = sellprice;
            order.buy_price_ok = buyprice;
            order.count = 0;
            order.isbargain = false;
            order.buy_bargain = 0;
            order.sell_bargain = 0;
        }

        /// <summary>组装数据</summary>
        private Dictionary<string, object> BuildData(BusinessGoodsVo goodsvo, int count, int price, int result, int bargain)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",result },
                {"goods",goodsvo },
                {"count",count},
                {"price",price},
                {"bargain",bargain},
            };
            return dic;
        }

        /// <summary>组装货物vo</summary>
        private BusinessGoodsVo BuildGoodsVo(BusinessOrder order, double id, int type)
        {
            return new BusinessGoodsVo()
            {
                id = id,
                baseId = order.goods_id,
                priceBuy = order.buy_price_ok,
                priceSell = order.sell_price_ok,
                count = type == 1 ? order.sell_count_max : order.buy_count_max
            };
        }
    }
}
