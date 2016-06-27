using System;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 指令选项
    /// </summary>
    [Serializable]
    public class ComandItem : ICloneable
    {
        /// <summary>
        /// 模块号
        /// </summary>
        public int moduleNumber { get; set; }

        /// <summary>
        /// 指令号
        /// </summary>
        public int commandNumber { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime currentTime { get; set; }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public Int64 userid { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public ComandItem CloneEntity()
        {
            return Clone() as ComandItem;
        }

        #endregion
    }
}
