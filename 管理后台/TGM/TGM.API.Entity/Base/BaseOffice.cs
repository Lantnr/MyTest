using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    public class BaseOffice : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseOffice CloneEntity()
        {
            return Clone() as BaseOffice;
        }

        #endregion
        /// <summary> id </summary>
        public int id { get; set; }

        /// <summary>官位</summary>
        public string officeLevel { get; set; }

        /// <summary>官位名</summary>
        public string officeName { get; set; }

        /// <summary> 需要贡献度 </summary>
        public int contribution { get; set; }

        /// <summary> 需要金钱 </summary>
        public int money { get; set; }

        /// <summary> 占有度 </summary>
        public int total_own { get; set; }

    }
}
