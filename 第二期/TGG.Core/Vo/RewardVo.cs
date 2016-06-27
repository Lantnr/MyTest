using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo
{
    /// <summary>奖励Vo</summary>
    [Serializable]
    public class RewardVo : BaseVo
    {
        /// <summary>
        /// 物品类型 GoodsType 
        /// </summary>
        public int goodsType { get; set; }

        /// <summary>
        /// 数值物品类型当前值
        /// </summary>
        public double value { get; set; }

        /// <summary>
        /// 对于非数值物品类型，为增加的物品 vo 数组
        /// </summary>
        public List<ASObject> increases { get; set; }

        /// <summary>
        /// 对于非数值物品类型，为更新的物品 vo 数组 
        /// </summary>
        public List<ASObject> decreases { get; set; }

        /// <summary>
        /// 对于非数值物品类型，为减少的物品 vo 数组 
        /// </summary>
        public List<double> deleteArray { get; set; }
    }
}
