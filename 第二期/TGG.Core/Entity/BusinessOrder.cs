using System;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Global;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 跑商货物锁定订单
    /// </summary>
    [Serializable]
    public class BusinessOrder : ICloneable
    {
        /// <summary>马车主键ID</summary>
        public Int64 car_mainid { get; set; }

        /// <summary>货物ID</summary>
        public int goods_id { get; set; }

        /// <summary>货物基表ID</summary>
        public Int64 goods_main_id { get; set; }

        /// <summary>町基表ID</summary>
        public int ting_base_id { get; set; }

        /// <summary>购买/卖出数量</summary>
        public int count { get; set; }

        /// <summary>货物买入最大数量 </summary>
        public int buy_count_max { get; set; }

        /// <summary>货物卖出最大数量 </summary>
        public int sell_count_max { get; set; }

        /// <summary>货物买入最终价格 </summary>
        public int buy_price_ok { get; set; }

        /// <summary>货物卖出最终价格 </summary>
        public int sell_price_ok { get; set; }

        /// <summary>当前町基表ID</summary>
        public int current_ting_base_id { get; set; }

        /// <summary>职业</summary>
        public int vocation { get; set; }

        /// <summary>货物买入讲价最终总价 </summary>
        public Int64 buy_bargain { get; set; }

        /// <summary>货物卖出讲价最终总价 </summary>
        public Int64 sell_bargain { get; set; }

        /// <summary>是否讲价</summary>
        public bool isbargain { get; set; }

        /// <summary>是否免税</summary>
        public bool istaxes { get; set; }

        /// <summary>获取购买货物总价</summary>
        public Int64 GetBuy()
        {
            return isbargain ? buy_bargain : GetTotalBuy();
        }

        /// <summary>获取卖出货物总价</summary>
        public Int64 GetSell()
        {
            return isbargain ? sell_bargain : GetTotalSell();
        }

        /// <summary>购买货物总价</summary>
        public Int64 GetTotalBuy()
        {
            //货物总价=总价+税收
            Int64 total = buy_price_ok * count;
            Int64 taxes = 0;
            if (!istaxes) taxes = GetTaxes(total);
            return total + taxes;
        }

        /// <summary>卖出货物总价</summary>
        public Int64 GetTotalSell()
        {
            //货物总价=总价-税收
            Int64 total = sell_price_ok * count;
            Int64 taxes = 0;
            if (!istaxes) taxes = GetTaxes(total);
            return total - taxes;
        }

        /// <summary>获取税收</summary>
        /// <param name="total">货物总价</param>
        private Int64 GetTaxes(Int64 total)
        {
            //最新根据职业基表读取数据扣税
            var voc_tax = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == vocation);
            if (voc_tax == null) return 0;
            var temp = voc_tax.business;
            var taxes = total * temp;
            return Convert.ToInt64(Math.Ceiling(taxes));
        }


        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BusinessOrder CloneEntity()
        {
            return Clone() as BusinessOrder;
        }

        #endregion
    }
}
