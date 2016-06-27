using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 战斗类型
    /// </summary>
    public enum FightType
    {
        /// <summary> NPC战斗部队表 </summary>
        NPC_FIGHT_ARMY = 1,

        /// <summary>一将讨怪物表</summary>
        SINGLE_FIGHT = 2,

        /// <summary> 点将怪物表 </summary>
        NPC_MONSTER = 3,

        /// <summary> 副本-爬塔 </summary>
        DUPLICATE_SHARP = 4,

        /// <summary> 美浓活动 </summary>
        SIEGE = 5,

        /// <summary> 一夜墨俣 </summary>
        BUILDING = 6,

        /// <summary> 连续战斗 </summary>
        CONTINUOUS=7,

        /// <summary> 玩家单向战斗 </summary>
        ONE_SIDE = 10,

        /// <summary> 玩家双向战斗 </summary>
        BOTH_SIDES = 11,
    }
}
