using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{

    /// <summary>
    /// 偷窃加成概率
    /// </summary>
    //[Serializable]
    public class BaseStealProb : ICloneable
    {

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseStealProb CloneEntity()
        {
            return Clone() as BaseStealProb;
        }

        #endregion

        /// <summary>加成概率</summary>
        public int prob { get; set; }

        /// <summary>技能或者属性</summary>
        public string skillOrAtt { get; set; }

        /// <summary>能力值</summary>
        public int value { get; set; }
    }
}
