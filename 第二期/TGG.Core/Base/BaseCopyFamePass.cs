using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 打气球名塔关卡基表
    /// </summary>
    //[Serializable]
    public class BaseCopyFamePass : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseCopyFamePass CloneEntity()
        {
            return Clone() as BaseCopyFamePass;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>关卡</summary>
        public int type { get; set; }

        /// <summary>通关分数</summary>
        public int score { get; set; }

    }
}
