using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 印系统基表
    /// </summary>
    //[Serializable]
    public class BaseYin : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseYin CloneEntity()
        {
            return Clone() as BaseYin;
        }

        #endregion

        /// <summary> 印基表编号 </summary>
        public int id { get; set; }

        /// <summary>开启等级</summary>
        public int level { get; set; }

        /// <summary>印计数</summary>
        public int yinCount { get; set; }

        /// <summary>等级上限</summary>
        public int levelLimit { get; set; }

        /// <summary>效果范围(1=单人 2=全体）</summary>
        public int effectRange { get; set; }

        /// <summary>目标类型（1=本方 2=敌方）</summary>
        public int targetType { get; set; }

    }
}
