using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 家臣称号信息基表
    /// </summary>
    //[Serializable]
    public class BaseRoleTitle : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleTitle CloneEntity()
        {
            return Clone() as BaseRoleTitle;
        }

        #endregion

        /// <summary> id</summary>
        public int id { get; set; }

        /// <summary>称号类型</summary>
        public int type { get; set; }

        /// <summary>称号属性加成</summary>
        public string attAddition { get; set; }

        /// <summary>获取途径</summary>
        public int methods { get; set; }

        /// <summary>所需次数</summary>
        public int count { get; set; }
    }
}
