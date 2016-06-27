using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseBuyPower : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseBuyPower CloneEntity()
        {
            return Clone() as BaseBuyPower;
        }

        #endregion

        /// <summary>购买次数 </summary>
        public int id { get; set; }

        /// <summary>购买体力数 </summary>
        public int power { get; set; }

        /// <summary>花费元宝 </summary>
        public int gold { get; set; }

    }
}
