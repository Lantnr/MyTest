using System;
namespace TGG.Core.Base
{
    /// <summary>
    /// 装备基表
    /// </summary>
    //[Serializable]
    public class BaseEquip : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseEquip CloneEntity()
        {
            return Clone() as BaseEquip;
        }

        #endregion

        #region

        /// <summary>道具编号</summary>
        public Int32 id { get; set; }

        /// <summary>品质</summary>
        public Int32 grade { get; set; }

        /// <summary>卖出价</summary>
        public Int32 sellPrice { get; set; }

        /// <summary>买入价</summary>
        public Int32 buyPrice { get; set; }

        /// <summary>物品子类型</summary>
        public Int32 typeSub { get; set; }

        /// <summary>物品小类型</summary>
        public Int32 typeSmall { get; set; }

        /// <summary>使用等级</summary>
        public Int32 useLevel { get; set; }

        /// <summary>统率</summary>
        public Int32 captain { get; set; }

        /// <summary>武力</summary>
        public Int32 force { get; set; }

        /// <summary>智谋</summary>
        public Int32 brains { get; set; }

        /// <summary>政务</summary>
        public Int32 govern { get; set; }

        /// <summary>魅力</summary>
        public Int32 charm { get; set; }

        /// <summary>攻击力</summary>
        public Int32 attack { get; set; }

        /// <summary>防御力</summary>
        public Int32 defense { get; set; }

        /// <summary>生命值</summary>
        public Int32 life { get; set; }

        /// <summary>增伤</summary>
        public Double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary>获取时是否公告</summary>
        public Int32 notice { get; set; }

        #endregion
    }
}
