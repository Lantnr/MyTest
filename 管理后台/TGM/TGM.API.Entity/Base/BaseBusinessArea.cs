using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Base
{
    /// <summary>
    /// 跑商商圈基表信息
    /// </summary>
    public class BaseBusinessArea
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseBusinessArea CloneEntity()
        {
            return Clone() as BaseBusinessArea;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>商圈名称</summary>
        public string name { get; set; }
    }
}
