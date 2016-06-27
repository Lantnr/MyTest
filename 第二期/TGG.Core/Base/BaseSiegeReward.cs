using System;

namespace TGG.Core.Base
{
    /// <summary> 美浓活动奖励基表数据 </summary>
    //[Serializable]
    public class BaseSiegeReward : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseSiegeReward CloneEntity()
        {
            return Clone() as BaseSiegeReward;
        }

        #endregion

        /// <summary> id </summary>
        public int id { get; set; }

        /// <summary>活动结果 </summary>
        public int type { get; set; }

        /// <summary>玩家声望区间 </summary>
        public int fame { get; set; }

        /// <summary>金钱奖励 </summary>
        public int money { get; set; }

        /// <summary>奖励声望 </summary>
        public int fameReward { get; set; }
    }
}
