using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Business
{
    /// <summary>
    /// BusinessGoodsVo 货物vo
    /// </summary>
    public class BusinessGoodsVo : BaseVo
    {
        /// <summary>主键</summary>
        public double id { get; set; }

        /// <summary>基础id</summary>
        public int baseId { get; set; }

        /// <summary> 购买价格 </summary>
        public double priceBuy { get; set; }

        /// <summary> 出售价格 </summary>
        public double priceSell { get; set; }

        /// <summary>货物数量</summary>
        public int count { get; set; }



    }
}
