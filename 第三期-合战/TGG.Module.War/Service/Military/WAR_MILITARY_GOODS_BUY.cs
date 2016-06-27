using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 购买军需品
    /// arlen
    /// </summary>
    public class WAR_MILITARY_GOODS_BUY : IDisposable
    {
        //private static WAR_MILITARY_GOODS_BUY _objInstance;

        ///// <summary>WAR_MILITARY_GOODS_BUY单体模式</summary>
        //public static WAR_MILITARY_GOODS_BUY GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_GOODS_BUY());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 查看开始指令处理 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_MILITARY_ENTER", "进入军需品功能");
#endif
            if (!data.ContainsKey("id") || !data.ContainsKey("count")) return null;
            var goods_id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value); //据点id
            var goods_count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value); //据点id

            if (goods_count <= 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            var station = session.Player.War.PlayerInCityId;//1000024;
            var user = session.Player.User;

            var goods = Variable.WARGOODS.Values.FirstOrDefault(m => m.city_id == station && m.goods_id == goods_id);
            if (goods == null) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            //判断资源消耗
            if (goods_count > goods.count) return CommonHelper.ErrorResult((int)ResultType.WAR_GOODS_COUNT_ERROR);

            var city = session.Player.War.City.CloneEntity();
            if (!IsMax(city, goods_id, goods_count)) return CommonHelper.ErrorResult((int)ResultType.WAR_RES_MAX_ERROR);
            var res_cost = Math.Round(goods_count * goods.goods_buy_price, 2);

            res_cost = Common.GetInstance().GetFunds(res_cost, user.player_influence, goods_id);
            var temp = city.res_funds - res_cost;
            if (temp < 0) return CommonHelper.ErrorResult((int)ResultType.WAR_RES_FUNDS_ERROR);
            city.res_funds = temp;
            city = SetResouce(city, goods_id, goods_count);
            session.Player.War.City = city;
            //资源更新推送
            tg_war_city.UpdateByResouce(city);
            (new Share.War()).SendCity(city.base_id, city.user_id);
            //数量更新
            var _c = goods.count - goods_count;
            goods.count = _c;
            var key = string.Format("{0}_{1}", goods.city_id, goods.goods_id);
            Variable.WARGOODS.AddOrUpdate(key, goods, (k, v) => goods);

            return new ASObject(BuildData((int)ResultType.SUCCESS, goods));
        }

        private Dictionary<String, Object> BuildData(int result, GlobalWarGoods goods)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "goods",EntityToVo.ToWarGoodsVo(goods) }, 
            };
            return dic;
        }

        /// <summary>资源设置</summary>
        private tg_war_city SetResouce(tg_war_city model, int type, int count)
        {
            switch (type)
            {
                case (int)WarResourseType.兵粮: { model.res_foods += count; break; }
                case (int)WarResourseType.马匹: { model.res_horse += count; break; }
                case (int)WarResourseType.铁炮: { model.res_gun += count; break; }
                case (int)WarResourseType.薙刀: { model.res_razor += count; break; }
                case (int)WarResourseType.苦无: { model.res_kuwu += count; break; }
            }
            return model;
        }

        /// <summary>判断是否大于最大值</summary>
        private bool IsMax(tg_war_city model, int type, int count)
        {
            var result = false;
            var cs = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == model.size);
            if (cs == null) return false;
            var temp = 0;
            switch (type)
            {
                case (int)WarResourseType.兵粮:
                    {
                        temp = model.res_foods + count;
                        result = temp <= cs.foods; break;
                    }
                case (int)WarResourseType.马匹:
                case (int)WarResourseType.铁炮:
                case (int)WarResourseType.薙刀:
                case (int)WarResourseType.苦无:
                    {
                        temp = model.res_horse + model.res_gun + model.res_razor + model.res_kuwu + count;
                        result = temp <= cs.goods; break;
                    }
            }
            return result;
        }
    }
}
