using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Module.War.Service.Fight
{

    #region 枚举
    /// <summary>
    /// 战斗武将类型
    /// </summary>
    public enum WarFightRoleType
    {
        //武将类型 0:武将 1:备大将 2:本丸  3：城门 4:伏兵
        武将 = 0,
        备大将 = 1,
        本丸 = 2,
        城门 = 3,
        伏兵 = 4
    }

    /// <summary>
    /// 合战技能触发条件
    /// </summary>
    public enum WarFightCondition
    {
        // 装备时 
        Equip = 1,
        //攻击时
        Attack = 2,
        //雨天
        Rain = 3,
        // 被攻击时
        Attacked = 4,
        //第一次战斗时
        FirstAttack = 5,
        //伏兵第一次攻击时
        DarkRoleFirstAttack = 6,
        //五常字相同时
        FiveSame = 7,
        //行动回合时气力值满值
        QiLi = 8,
        // 被合战秘技攻击时
        SkillAttack = 9,
        //被合战奥义攻击时
        FancyAttack = 10,
        //兵力值低于一定数值时
        BloodLess = 11,
        //攻击比自己魅力低的部队时
        CharmLessMe = 12,
        //攻击比自己武力低的部队时
        ForceLessMe = 13,
        //攻击比自己智谋低的部队时
        BrainLessMe = 14,
        //处于高坡地形上时
        AreaSlope = 15,
        //处于河滩地形上时
        AreaRiver = 16,
        //处于沼泽地形上时
        AreaSwamp = 17,
        //处于密林地形上时
        AreaForest = 18,
        //内政技能
        OtherUse = 101,
    }



    /// <summary>
    /// 技能类型
    /// </summary>
    public enum WarFightSkillType
    {
        /// <summary> 合战秘技</summary>
        Skill = 1,

        /// <summary> 合战奥义</summary>
        Katha = 2,

        /// <summary> 武将特性</summary>
        Character = 3,

        /// <summary> 忍者众技能</summary>
        NinjaSkill = 4,

        /// <summary> 忍者众奥义</summary>
        NinjaMystery = 5,

        /// <summary> 地形</summary>
        Area = 6,
        /// <summary> 陷阱</summary>
        Trap = 7,

    }

    /// <summary>
    ///效果对象类型
    /// </summary>
    public enum EffectFaceTo
    {
        //  2：敌方 1：本方
        Me = 1,
        Rival = 1,
    }

    public enum WeatherType
    {
        /// <summary>晴天</summary>
        Sun = 0,
        /// <summary>雨天</summary>
        Rain = 1,
    }

    public enum WarFightTypes
    {
        /// <summary>
        /// 玩家合战
        /// </summary>
        User = 1,

        /// <summary>
        /// npc战斗
        /// </summary>
        NPC = 2,

    }
    #endregion

}
