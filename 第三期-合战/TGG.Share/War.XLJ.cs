using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Global;

namespace TGG.Share
{
    public partial class War : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 军需品

        /// <summary>加载全局货物价格</summary>
        public void RefreshWarGoods()
        {
            var _city = Variable.BASE_WARCITY.Where(m => m.type != 2).ToList();
            var _res = Variable.BASE_CITYRESOURCE;

            var city_count = _city.Count();
            for (var i = 0; i < city_count; i++)
            {
                var goods = new GlobalWarGoods { city_id = _city[i].id };
                foreach (var res in _res)
                {
                    var newgoods = goods.CloneEntity();
                    newgoods.goods_id = res.id;
                    //货物价格表中得到浮动的数据
                    var price = Variable.BASE_CITYRESOURCE.FirstOrDefault(q => q.id == res.id);

                    //随机生成货物买、卖的价格
                    if (price != null)
                    {
                        newgoods.count = price.count;
                        newgoods.goods_buy_price = RNG.NextDouble(price.min_buy, price.max_buy, 1);
                    }
                    var key = string.Format("{0}_{1}", newgoods.city_id, newgoods.goods_id);
                    Variable.WARGOODS.AddOrUpdate(key, newgoods, (k, v) => newgoods);
                }
            }
#if DEBUG
            Console.WriteLine();
#endif

        }

        #endregion
    }
}
