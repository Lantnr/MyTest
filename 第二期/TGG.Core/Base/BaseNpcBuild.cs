using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseNpcBuild : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcBuild CloneEntity()
        {
            return Clone() as BaseNpcBuild;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>类型：1.大将 2.城池</summary>
        public int type { get; set; }

        /// <summary>战斗部队id</summary>
        public int armyId { get; set; }

        /// <summary>城池初始耐久</summary>
        public int baseHp { get; set; }

        /// <summary>总耐久</summary>
        public int totalHp { get; set; }

        /// <summary>阵营</summary>
        public int camp { get; set; }

        /// <summary>坐标</summary>
        public string coorPoint { get; set; }

    }
}
