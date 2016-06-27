using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;

namespace TGG.Share.NewFight
{
    /// <summary> 战斗实体 </summary>
    public class FightEntity
    {
        /// <summary> 己方是否胜利 </summary>
        public bool Iswin { get; set; }

        /// <summary> 玩家剩余血量 </summary>
        public Int64 PlayHp { get; set; }

        /// <summary> Boss剩余血量 Boss专用</summary>
        public Int64 BoosHp { get; set; }

        /// <summary> 己方对敌方造成的伤害 </summary>
        public Int64 Hurt { get; set; }

        /// <summary> 己方战斗Vo </summary>
        public FightVo Ofight { get; set; }

        /// <summary> 敌方战斗Vo </summary>
        public FightVo Rfight { get; set; }

        /// <summary> 出手总次数 </summary>
        public int ShotCount { get; set; }

        /// <summary> 战斗处理结果 </summary>
        public ResultType Result { get; set; }

    }

    /// <summary> 战斗全局实体 </summary>
    public class FightGlobalEntity
    {
        /// <summary> buff集合 包含双方buff 实体 </summary>
        public List<BuffEntity> buff { get; set; }

        /// <summary> 是否初始攻击方 </summary>
        public bool isAttack { get; set; }

        /// <summary> 当前攻击的战斗用户实体 </summary>
        public FightUserEntity attack { get; set; }

        /// <summary> 当前防守的战斗用户实体 </summary>
        public FightUserEntity defense { get; set; }

        /// <summary> 是否计算出胜负 </summary>
        public bool isResult { get; set; }

        /// <summary> 是否己方胜利 </summary>
        public bool isAttackWin { get; set; }
    }

    /// <summary> 战斗用户实体 </summary>
    public class FightUserEntity
    {
        public FightUserEntity()
        {
            user = new tg_user();
            yin = new YinEntity();
            attackRole = new RoleEntity();
            personal = new tg_fight_personal();
            roleEntity = new List<RoleEntity>();
        }
        /// <summary> 是否对手 </summary>
        public bool isRaval { get; set; }

        /// <summary> 是否为初始攻击方 </summary>
        public bool isAttack { get; set; }

        /// <summary> 用户实体 </summary>
        public tg_user user { get; set; }

        /// <summary> 印实体 </summary>
        public YinEntity yin { get; set; }

        /// <summary> 个人战阵形实体 </summary>
        public tg_fight_personal personal { get; set; }

        /// <summary> 武将实体集合 </summary>
        public List<RoleEntity> roleEntity { get; set; }

        /// <summary> 当前攻击武将 </summary>
        public RoleEntity attackRole { get; set; }

        /// <summary> 当前防守武将 </summary>
        public RoleEntity defenseRole { get; set; }

        /// <summary> 加成系数 </summary>
        public double xiShu { get; set; }

    }

    /// <summary> 战斗武将实体 </summary>
    public class RoleEntity : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public RoleEntity CloneEntity()
        {
            return Clone() as RoleEntity;
        }

        #endregion

        public RoleEntity()
        {
            mystery = new SkillEntity();
            cheatCode = new SkillEntity();
            art_ninja_mystery = new SkillEntity();
            art_ninja_cheat_code1 = new SkillEntity();
            art_ninja_cheat_code2 = new SkillEntity();
            art_ninja_cheat_code3 = new SkillEntity();
        }
        /// <summary>武将主键</summary>
        public Int64 id { get; set; }

        /// <summary>基础 id </summary>
        public int baseId { get; set; }

        /// <summary> 怪物类型  0人物  1怪物 </summary>
        public int monsterType { get; set; }

        /// <summary> 奥义</summary>
        public SkillEntity mystery { get; set; }

        /// <summary>秘技</summary>
        public SkillEntity cheatCode { get; set; }

        /// <summary>忍者众秘技1</summary>
        public SkillEntity art_ninja_cheat_code1 { get; set; }

        /// <summary>忍者众秘技2</summary>
        public SkillEntity art_ninja_cheat_code2 { get; set; }

        /// <summary>忍者众秘技3</summary>
        public SkillEntity art_ninja_cheat_code3 { get; set; }

        /// <summary>忍者众奥义</summary>
        public SkillEntity art_ninja_mystery { get; set; }

        /// <summary>伤害 </summary>
        public Int64 damage { get; set; }

        /// <summary> 生命 </summary>
        public Int64 hp { get; set; }

        /// <summary> 当前战斗初始生命 </summary>
        public Int64 HP { get; set; }

        // <summary> 初始血量 </summary>
        public Int64 initHp { get; set; }

        /// <summary>攻击</summary>
        public Double attack { get; set; }

        /// <summary>防御 </summary>
        public Double defense { get; set; }

        /// <summary> 增伤 </summary>
        public Double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary> 会心几率 </summary>
        public Double critProbability { get; set; }

        /// <summary> 会心加成  </summary>
        public Double critAddition { get; set; }

        /// <summary> 闪避几率 </summary>
        public Double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public Double mystery_probability { get; set; }

        /// <summary> 无视闪避几率 </summary>
        public Double IgnoreDuck { get; set; }

        /// <summary> 气力值 </summary>
        public int angerCount { get; set; }

        /// <summary> 等级</summary>
        public int lv { get; set; }

        /// <summary> 初始位置编号 </summary>
        public int initweizhi { get; set; }

        /// <summary> 滚动后位置编号 </summary>
        public int weizhi { get; set; }

        /// <summary> 用户id</summary>
        public Int64 user_id { get; set; }
    }

    /// <summary> buff实体 </summary>
    public class BuffEntity
    {
        /// <summary> 武将主键Id </summary>
        public Int64 rid { get; set; }

        /// <summary> buff类型 0:新 1:旧 2:永久 </summary>
        public int bufftype { get; set; }

        /// <summary> buff效果类型 </summary>
        public FightingSkillType type { get; set; }

        /// <summary> buff是否有效 </summary>
        public bool IsOk { get; set; }

        /// <summary> buff过期回合 </summary>
        public int count { get; set; }

        /// <summary> buff数值 </summary>
        public double buffValue { get; set; }
    }

    /// <summary> 技能实体 </summary>
    public class SkillEntity
    {
        public SkillEntity()
        {
            skill = new tg_role_fight_skill();
            skillEffect = new List<SkillEffects>();
        }

        /// <summary> 触发条件 </summary>
        public int condition { get; set; }

        /// <summary> 个人战技能 </summary>
        public tg_role_fight_skill skill { get; set; }

        /// <summary> 所需气力 </summary>
        public int energy { get; set; }

        /// <summary> 技能效果 </summary>
        public List<SkillEffects> skillEffect { get; set; }

        /// <summary> 是否立即攻击 1:是 0:否 </summary>
        public int isQuickAttack { get; set; }

        /// <summary> 攻击范围 1:单体 2:全体 </summary>
        public int attackRange { get; set; }
    }

    /// <summary> 印实体 </summary>
    public class YinEntity
    {
        public YinEntity()
        {
            yin = new tg_fight_yin();
            yinEffect = new List<SkillEffects>();
        }

        /// <summary> 个人战技能 </summary>
        public tg_fight_yin yin { get; set; }

        /// <summary> 当前印数 </summary>
        public int count { get; set; }

        /// <summary> 所需印数 </summary>
        public int yinCount { get; set; }

        /// <summary> 印效果集合 </summary>
        public List<SkillEffects> yinEffect { get; set; }
    }

    /// <summary> 战斗使用的战斗技能效果实体 </summary>
    public class SkillEffects
    {
        /// <summary>技能效果类型</summary>
        public int type { get; set; }

        /// <summary>技能效果目标 (1=本方 2=敌方)</summary>
        public int target { get; set; }

        /// <summary>技能效果范围 (1=单体 2=全体)</summary>
        public int range { get; set; }

        /// <summary>技能效果回合数</summary>
        public int round { get; set; }

        /// <summary>技能效果值</summary>
        public double values { get; set; }

        ///<summary>技能效果几率</summary>
        public double robabilityValues { get; set; }
    }

}
