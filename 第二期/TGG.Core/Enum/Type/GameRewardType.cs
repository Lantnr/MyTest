using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 游艺园奖励领取枚举
    /// </summary>
    public enum  GameRewardType
    {
        /// <summary>
        /// 未领取
        /// </summary>
        TYPE_UNREWARD = 0,

        /// <summary>
        /// 可领取
        /// </summary>
        TYPE_CANREWARD = 1,

        /// <summary>
        /// 已领取
        /// </summary>
        TYPE_REWARDED = 2,
    }
}
