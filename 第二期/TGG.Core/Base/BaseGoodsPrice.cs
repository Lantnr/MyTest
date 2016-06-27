using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 货物价格基表
    /// </summary>
    //[Serializable]
    public class BaseGoodsPrice : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseGoodsPrice CloneEntity()
        {
            return Clone() as BaseGoodsPrice;
        }

        #endregion

        /// <summary>编号</summary>
        public int id { get; set; }

        /// <summary>商圈id</summary>
        public int areaId { get; set; }

        /// <summary>货物id</summary>
        public int goodsId { get; set; }

        /// <summary>最低卖出价格</summary>
        public int min_sell { get; set; }

        /// <summary>最高卖出价格</summary>
        public int max_sell { get; set; }

        /// <summary>最低买入价格</summary>
        public int min_buy { get; set; }

        /// <summary>最高买入价格</summary>
        public int max_buy { get; set; }

        ///// <summary>最低价格</summary>
        //public int min { get; set; }

        ///// <summary>最高价格</summary>
        //public int max { get; set; }
    }
}
