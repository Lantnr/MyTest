using System;
namespace TGG.Core.Base
{
    /// <summary>
    /// 部件基表
    /// </summary>
    //[Serializable]
    public class BasePart : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BasePart CloneEntity()
        {
            return Clone() as BasePart;
        }

        #endregion

        /// <summary>部件编号</summary>
        public int id { get; set; }

        /// <summary>名称</summary>
        public string name { get; set; }

        /// <summary>速度</summary>
        public int speed { get; set; }

        /// <summary>vip</summary>
        public int vip { get; set; }

    }
}
