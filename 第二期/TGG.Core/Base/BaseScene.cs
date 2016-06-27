using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 场景基表
    /// </summary>
    //[Serializable]
    public class BaseScene : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseScene CloneEntity()
        {
            return Clone() as BaseScene;
        }

        #endregion

        /// <summary>场景id</summary>
        public int id { get; set; }

        /// <summary>默认出生点id</summary>
        public int bornPoint { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public int enabled { get; set; }
    }
}
