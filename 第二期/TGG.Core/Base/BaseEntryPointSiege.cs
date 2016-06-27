using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 美浓攻略传送点基表
    /// </summary>
    //[Serializable]
    public class BaseEntryPointSiege : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseEntryPointSiege CloneEntity()
        {
            return Clone() as BaseEntryPointSiege;
        }

        #endregion

        /// <summary> id </summary>
        public Int64 id { get; set; }

        /// <summary> 坐标 </summary>
        public string coorPoint { get; set; }

        /// <summary> 传送坐标 </summary>
        public string coorPoint2 { get; set; }

        /// <summary> 阵营 </summary>
        public int camp { get; set; }
    }
}
