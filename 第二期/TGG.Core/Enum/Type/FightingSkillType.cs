using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 战斗技能类型
    /// </summary>
    public enum FightingSkillType
    {
        /// <summary> 增加攻击力 </summary>
        INCREASE_ATTACK = 1,

        /// <summary> 降低攻击力 </summary>
        REDUCE_ATTACK = 2,

        /// <summary> 增加防御力 </summary>
        INCREASE_DEFENSE = 3,

        /// <summary> 降低防御力/ </summary>
        REDUCE_DEFENSE = 4,

        /// <summary> 会心几率 </summary>
        KMOWING_PROBABILITY = 5,

        /// <summary> 增加会心效果 </summary>
        INCREASE_KMOWING = 6,

        /// <summary> 闪避率 </summary>
        DUCK_PROBABILITY = 7,

        /// <summary> 无视闪避几率 </summary>
        IGNORE_DUCK_PROBABILITY = 8,

        /// <summary> 增加伤害百分比 </summary>
        INCREASES_DAMAGE_PERCENTAGE = 9,

        /// <summary> 降低伤害百分比 </summary>
        REDUCE_DAMAGE_PERCENTAGE = 10,

        /// <summary> 眩晕 </summary>
        DIZZINESS = 11,

        /// <summary> 封印 </summary>
        SEAL = 12,

        /// <summary> 增加气力 </summary>
        INCREASES_STRENGTH = 13,

        /// <summary> 减少气力 </summary>
        REDUCE_STRENGTH = 14,

        /// <summary> 增加印计数 </summary>
        INCREASES_YINCOUNT = 15,

        /// <summary> 减少印计数 </summary>
        REDUCE__YINCOUNT = 16,

        /// <summary> 增加奥义触发几率 </summary>
        INCREASES_MYSTERY_PROBABILITY = 17,




        #region 扩展

        /// <summary> 暴击 </summary>
        CRIT = 30,

        /// <summary> 闪避 </summary>
        DODGE = 31,
       
        #endregion

    }
}
