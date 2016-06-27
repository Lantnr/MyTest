using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 武将信息基表
    /// arlen 2014-08-19 最新更新 
    /// </summary>
    //[Serializable]
    public class BaseRoleInfo : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleInfo CloneEntity()
        {
            return Clone() as BaseRoleInfo;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>职业武将</summary>
        public int jobType { get; set; }

        /// <summary>名称</summary>
        public string name { get; set; }

        /// <summary>品质</summary>
        public int grade { get; set; }

        /// <summary>统帅</summary>
        public int captain { get; set; }

        /// <summary>武力</summary>
        public int force { get; set; }

        /// <summary>智谋</summary>
        public int brains { get; set; }

        /// <summary>魅力</summary>
        public int charm { get; set; }

        /// <summary>政务</summary>
        public int govern { get; set; }

        /// <summary>体力</summary>
        public int power { get; set; }

        /// <summary>生命</summary>
        public int life { get; set; }

    }
}
