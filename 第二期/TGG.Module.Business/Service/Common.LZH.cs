using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// LZH 跑商公共方法
    /// </summary>
    public partial class Common
    {
        /// <summary>进入町组装数据</summary>
        public Dictionary<String, Object> EnterTingBuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result}
            };
            return dic;
        }

        

        /// <summary>市价情报数据组装</summary>
        public Dictionary<String, Object> BuildIds(int result, List<int> ids)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"id", ids}
            };
            return dic;
        }

        /// <summary>转为 Entity货物</summary>
        public tg_goods_item ConvertBaseGood(BaseGoods good, int tingid, Int64 userid)
        {
            return new tg_goods_item
            {
                user_id = userid,
                ting_id = tingid,
                goods_id = good.id,
                number = good.sum,
                number_max = good.sum,
            };
        }


        /// <summary>进入町公共方法</summary>
        public ASObject EnterTing(Int64 userid, int tingid)
        {
            var ting_enter = tg_user_ting.GetEntityByUserIdTingId(userid, tingid);

            if (ting_enter == null || ting_enter.id == 0)
            {
                var base_ting = Variable.BASE_TING.FirstOrDefault(m => m.id == tingid);
                if (base_ting == null)
                    return new ASObject(EnterTingBuildData((int)ResultType.BASE_TABLE_ERROR));

                var ting = new tg_user_ting
                {
                    user_id = userid,
                    ting_id = tingid,
                    state = (int)CityVisitType.VISIT,
                    area_id = base_ting.areaId
                };
                ting.Insert();
                InsertGoods(ting.ting_id, ting.user_id); //插入tg_goods_item货物信息
            }
            else
            {
                var num = GetBaseGoodsNum(ting_enter.ting_id);
                //这个町还没有货物 插入货物
                if (tg_goods_item.GetFindByTingIdUserId(ting_enter.ting_id, ting_enter.user_id) < num)
                    InsertGoods(ting_enter.ting_id, userid);
                if (ting_enter.state == (int)CityVisitType.VISIT)       //町状态验证
                    return new ASObject(EnterTingBuildData((int)ResultType.SUCCESS));

                ting_enter.state = (int)CityVisitType.VISIT;            //更新町为已访问状态
                ting_enter.Update();
            }
            return new ASObject(EnterTingBuildData((int)ResultType.SUCCESS));
        }

        /// <summary>货物数量</summary>
        public int GetBaseGoodsNum(int tingid)
        {
            var goods_string = GetBaseGoods(tingid);
            return string.IsNullOrEmpty(goods_string) ? 0 : GetBaseGoodsList(goods_string).Count;
        }

        /// <summary>获取货物id集合</summary>
        public List<int> GetBaseGoodsList(string goods)
        {
            var list = goods.Split(',').ToList();
            return list.Select(m => Convert.ToInt32(m)).ToList();
        }

        /// <summary>查询当前町基表货物信息</summary>
        public string GetBaseGoods(int tingid)
        {
            var baseting = Variable.BASE_TING.FirstOrDefault(m => m.id == tingid);
            if (baseting == null || baseting.goods == null) return null;
            return baseting.goods;

        }
        /// <summary>添加货物信息</summary>
        public void InsertGoods(int tingid, Int64 userid)
        {
            var goods_string = GetBaseGoods(tingid);
            if (string.IsNullOrEmpty(goods_string)) return;

            var listid = GetBaseGoodsList(goods_string);   //查询当前町基表货物信息 
            var _list = tg_goods_item.GetFindByGoodsIds(userid, tingid, goods_string).ToList();

            foreach (var id in listid)
            {
                var _bid = Convert.ToInt32(id);
                if (_list.Any())
                {
                    var temp = _list.Count(m => m.goods_id == _bid);
                    if (temp > 0) continue;
                }
                var basegood = Variable.BASE_GOODS.FirstOrDefault(m => m.id == _bid);
                var model = ConvertBaseGood(basegood, tingid, userid);
                model.Insert();
            }
        }
    }
}
