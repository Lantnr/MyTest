using System;
namespace TGG.Core.Base
{
    /// <summary>
    /// 货物基表
    /// </summary>
    //[Serializable]
    public class BaseGoods : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseGoods CloneEntity()
        {
            return Clone() as BaseGoods;
        }

        #endregion

        /// <summary>货物编号</summary>
        public int id { get; set; }

        /// <summary>库存总数量</summary>
        public int sum { get; set; }

    }
}
