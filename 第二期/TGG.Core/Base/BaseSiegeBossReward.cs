using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// Boss伤害奖励表
    /// </summary>
    //[Serializable]
    public class BaseSiegeBossReward : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseSiegeBossReward CloneEntity()
        {
            return Clone() as BaseSiegeBossReward;
        }

        #endregion

        /// <summary> id </summary>
        public int id { get; set; }

        /// <summary>个人伤害累加 </summary>
        public int hrut { get; set; }

        /// <summary>奖励声望 </summary>
        public int fameReward { get; set; }

    }
}
