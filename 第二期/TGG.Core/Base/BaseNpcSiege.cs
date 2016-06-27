using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 美浓攻略大将，破坏点，本丸表基表
    /// </summary>
    //[Serializable]
    public class BaseNpcSiege : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcSiege CloneEntity()
        {
            return Clone() as BaseNpcSiege;
        }

        #endregion

        /// <summary> id </summary>
        public Int64 id { get; set; }

        /// <summary> 类型 </summary>
        public int type { get; set; }

        /// <summary> 坐标 </summary>
        public string coorPoint { get; set; }

        /// <summary> 战斗部队id </summary>
        public int armyId { get; set; }

        /// <summary> 总HP </summary>
        public int totalHp { get; set; }

        /// <summary> 阵营 </summary>
        public int camp { get; set; }

    }
}
