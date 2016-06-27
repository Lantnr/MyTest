using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 町基表
    /// </summary>
    //[Serializable]
    public class BaseTing : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTing CloneEntity()
        {
            return Clone() as BaseTing;
        }

        #endregion

        /// <summary>城市基表编号</summary>
        public int id { get; set; }

        /// <summary>坐标</summary>
        public string coorPoint { get; set; }

        /// <summary>商圈id</summary>
        public int areaId { get; set; }

        /// <summary>调查费用</summary>
        public int lookCost { get; set; }

        /// <summary>产出货物id集合</summary>
        public string goods { get; set; }
    }
}
