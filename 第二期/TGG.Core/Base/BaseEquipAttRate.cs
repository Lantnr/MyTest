using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 属性条数几率
    /// </summary>
    //[Serializable]
    public class BaseEquipAttRate : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseEquipAttRate CloneEntity()
        {
            return Clone() as BaseEquipAttRate;
        }

        #endregion

        /// <summary></summary>
        public int id { get; set; }

        /// <summary>几率（%）</summary>
        public int rate { get; set; }

    }
}
