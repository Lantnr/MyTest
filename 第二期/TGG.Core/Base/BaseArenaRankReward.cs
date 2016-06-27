using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary> 竞技场排名奖励基表 </summary>
    //[Serializable]
    public class BaseArenaRankReward : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseArenaRankReward CloneEntity()
        {
            return Clone() as BaseArenaRankReward;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>名次</summary>
        public int ranKing { get; set; }

        /// <summary>金钱</summary>
        public int money { get; set; }

        /// <summary>声望</summary>
        public int fame { get; set; }
    }
}
