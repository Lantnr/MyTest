using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 买/卖
    /// </summary>
    public class BUSINESS_GOODS_BUY_ENTER
    {
        private static BUSINESS_GOODS_BUY_ENTER ObjInstance;

        /// <summary>BUSINESS_GOODS_BUY_ENTER单体模式</summary>
        public static BUSINESS_GOODS_BUY_ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_GOODS_BUY_ENTER());
        }

        /// <summary>买/卖</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine(string.Format("{0}:{1}", "BUSINESS_GOODS_BUY_ENTER", "买/卖"));
#endif
            var user = session.Player.User.CloneEntity();
            var tingid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);//町基础 id
            var type = data.FirstOrDefault(q => q.Key == "type").Value.ToString();//0:购买,1:出售

            session.Player.Order.current_ting_base_id = tingid;

            if (type == "0")
                return BuyType(tingid, user.id);
            if (type != "1")
                return Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR);

            var carid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "carId").Value);//马车 id
            _tingBaseId = tingid;
            return SellType(carid, session.Player.User.player_vocation);
        }

        /// <summary>购买类型货物一览</summary>
        private ASObject BuyType(int tingid, Int64 userid)
        {
            var list = tg_goods_item.GetEntityList(userid, tingid);
            var temp = list.GroupBy(m => m.goods_id);
            var result = new List<tg_goods_item>();
            foreach (var item in temp)
            {
                if (item.Count() > 1)
                {
                    var _entity = item.FirstOrDefault();
                    result.Add(_entity);
                    var _l = item.Where(m => m.id != _entity.id);
                    foreach (var k in _l)
                    {
                        k.Delete();
                    }
                }
                else
                    result.Add(item.FirstOrDefault());
            }

            return BuildData((int)ResultType.SUCCESS, result);
        }

        /// <summary>出售类型货物一览</summary>
        private ASObject SellType(int carid, int vocation)
        {
            var list = tg_goods_business.GetListEntityByCid(carid); //马车上的货物
            return BuildData((int)ResultType.SUCCESS, list, vocation);
        }

        private int _tingBaseId; //町基础id

        /// <summary>组装数据</summary>
        /// <param name="goods">町的货物</param>
        private ASObject BuildData(int result, IEnumerable<tg_goods_business> goods, int vocation)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "goods", ConvertListAsObject(goods, vocation) } };
            return new ASObject(dic);
        }

        /// <summary>组装数据</summary>
        /// <param name="goods">町的货物</param>
        private ASObject BuildData(int result, IEnumerable<tg_goods_item> goods)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "goods", ConvertListAsObject(goods) } };
            return new ASObject(dic);
        }

        /// <summary>List[tg_goods_business] 集合转换 List[ASObject]</summary>
        private List<ASObject> ConvertListAsObject(IEnumerable<tg_goods_business> list, int vocation)
        {
            //return list.Select(item =>
            //    AMFConvert.ToASObject(ConvertBusinessGoodsVo(item))).ToList();
            return list.Select(item => AMFConvert.ToASObject(ConvertBusinessGoodsVo(item, vocation))).ToList();
        }

        /// <summary> List[tg_goods_item] 集合转换 List[ASObject] </summary>
        private List<ASObject> ConvertListAsObject(IEnumerable<tg_goods_item> list)
        {
            return list.Select(item => AMFConvert.ToASObject(ConvertBusinessGoodsVo(item))).ToList();
        }

        /// <summary>tg_goods_business 转换 BusinessGoodsVo</summary>
        /// <param name="goods">tg_goods_business</param>
        private BusinessGoodsVo ConvertBusinessGoodsVo(tg_goods_business goods, int vocation)
        {
            var ting_goods = Variable.GOODS.Where(m => m.ting_id == _tingBaseId).ToList();
            var tg_id = ting_goods.FirstOrDefault(m => m.goods_id == goods.goods_id);
            //var bg = tg_id.CloneEntity();
            //var ting_base_goods = Variable.GOODS.Where(m => m.ting_id == goods.ting_id).ToList();
            //var _bg = ting_base_goods.FirstOrDefault(m => m.goods_id == goods.goods_id);
            try
            {
                if (tg_id == null) return new BusinessGoodsVo();
                var bg = tg_id.CloneEntity();
                var model = new BusinessGoodsVo()
                {
                    id = goods.id,
                    baseId = (int)goods.goods_id,
                    count = goods.goods_number,
                    priceBuy = goods.price,
                };
                if (bg.goods_buy_price != 0)
                    model.priceSell = bg.goods_buy_price / 2;
                else
                    model.priceSell = bg.goods_sell_price;
                //model.priceSell -= Common.GetInstance().RuleData(vocation, model.priceSell);
                return model;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new BusinessGoodsVo();
            }
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
