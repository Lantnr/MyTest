using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseFamilyLog : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFamilyLog CloneEntity()
        {
            return Clone() as BaseFamilyLog;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>类型</summary>
        public int type { get; set; }

        /// <summary>描述</summary>
        public string prestige { get; set; }

    }
}
