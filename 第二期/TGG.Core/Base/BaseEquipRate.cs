using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 装备品质几率
    /// </summary>
    //[Serializable]
    public class BaseEquipRate : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseEquipRate CloneEntity()
        {
            return Clone() as BaseEquipRate;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>品质</summary>
        public int grade { get; set; }

        /// <summary>几率(%)</summary>
        public int rate { get; set; }

    }
}
