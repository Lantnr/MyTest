using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary> 武将组合基表 </summary>
    //[Serializable]
    public class BaseRoleGroup : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleGroup CloneEntity()
        {
            return Clone() as BaseRoleGroup;
        }

        #endregion

        /// <summary> id </summary>
        public int id { get; set; }

        /// <summary>武将1</summary>
        public int role1 { get; set; }

        /// <summary>武将2</summary>
        public int role2 { get; set; }

        /// <summary>武将3</summary>
        public int role3 { get; set; }

        /// <summary>武将4</summary>
        public int role4 { get; set; }

        /// <summary>技能效果</summary>
        public string effects { get; set; }
    }
}
