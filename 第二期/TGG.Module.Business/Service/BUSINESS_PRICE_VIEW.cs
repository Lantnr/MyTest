using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;
using TGG.Core.Common.Util;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 市价一览
    /// </summary>
    public class BUSINESS_PRICE_VIEW
    {
        private static BUSINESS_PRICE_VIEW objInstance;

        /// <summary>BUSINESS_PRICE_VIEW单体模式</summary>
        public static BUSINESS_PRICE_VIEW getInstance()
        {
            return objInstance ?? (objInstance = new BUSINESS_PRICE_VIEW());
        }

        /// <summary>市价一览</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "BUSINESS_PRICE_VIEW", "市价一览");
#endif
                var dic = new Dictionary<string, object>();
                var player = session.Player.User.CloneEntity();
                var list = tg_user_ting.GetEntityByUserIdAndState(player.id, (int)CityVisitType.VISIT);
                var tingids = list.Select(m => m.ting_id).ToList();
                var time = Variable.GRT;

                foreach (var item in tingids)
                {
                    var goods = tg_goods_item.GetEntityByTingIdAndUserId(item, player.id);
                    var list_goods = Variable.GOODS.Where(m => m.ting_id == item).ToList();
                    var goodid = goods.Select(m => m.goods_id).ToList();
                    var glogoods = new List<GlobalGoods>();
                    list_goods.ForEach(m =>
                    {
                        if (!goodid.Contains(m.goods_id))
                        {
                            glogoods.Add(m);
                        }
                    }

                        );
                    var golbalgood = ConverBusinessGoods(glogoods);
                    if (dic.Keys.Contains(item.ToString()))
                        dic[item.ToString()] = ConverBusinessGoodsVos(goods, golbalgood);
                    else
                        dic.Add(item.ToString(), ConverBusinessGoodsVos(goods, golbalgood));
                }
                return new ASObject(BuildData((int)ResultType.SUCCESS, time, dic));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>数据组装</summary>
        private Dictionary<string, object> BuildData(int result, double time, Dictionary<string, object> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"time", time},
                {"goods", list.Count > 0 ? list : null}
            };
            return dic;
        }

        public List<BusinessGoodsVo> ConverBusinessGoodsVos(IEnumerable<tg_goods_item> list, IEnumerable<BusinessGoodsVo> lists)
        {
            var list_good = list.Select(ConvertBusinessGoodsVo).ToList();
            list_good.AddRange(lists);
            return list_good;
        }

        /// <summary>tg_goods_item 转换 BusinessGoodsVo</summary>
        /// <param name="goods">tg_goods_item</param>
        public BusinessGoodsVo ConvertBusinessGoodsVo(tg_goods_item goods)
        {
            var bg = Variable.GOODS.FirstOrDefault(m => m.goods_id == goods.goods_id && m.ting_id == goods.ting_id);
            var sellprice = 0;
            sellprice = GetHalfPrice(bg.goods_sell_price);
            return new BusinessGoodsVo()
            {
                id = goods.id,
                baseId = goods.goods_id,
                count = goods.number,
                priceBuy = bg.goods_buy_price,
                priceSell = sellprice,
            };
        }

        public List<BusinessGoodsVo> ConverBusinessGoods(IEnumerable<GlobalGoods> list)
        {
            return list.Select(ConvertBusinessGoodsVo).ToList();
        }

        /// <summary>GlobalGoods 转换 BusinessGoodsVo</summary>
        /// <param name="goods">GlobalGoods</param>
        public BusinessGoodsVo ConvertBusinessGoodsVo(GlobalGoods goods)
        {
            return new BusinessGoodsVo()
            {
                id = goods.goods_id,
                baseId = goods.goods_id,
                count = 0,
                priceBuy = 0,
                priceSell = goods.goods_sell_price,
            };
        }

        /// <summary>获取货物减半价格</summary>
        public int GetHalfPrice(int sellprice)
        {
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3007"); //获取基表数据
            if (baserule == null) return 0;
            var temp = baserule.value;
            temp = temp.Replace("price", sellprice.ToString("0.00"));
            var _halfprice = CommonHelper.EvalExpress(temp);
            var halfprice = Convert.ToInt32(_halfprice);
            return halfprice;
        }
    }
}
