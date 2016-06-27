using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 功能开放表
    /// </summary>
    //[Serializable]
    public class BaseModuleOpen : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseModuleOpen CloneEntity()
        {
            return Clone() as BaseModuleOpen;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>开放等级</summary>
        public int level { get; set; }

        /// <summary>身份要求</summary>
        public int identity { get; set; }

        /// <summary>是否手动激活</summary>
        public int manual { get; set; }

    }
}
