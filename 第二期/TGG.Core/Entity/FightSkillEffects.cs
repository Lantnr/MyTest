namespace TGG.Core.Entity
{
    /// <summary> 战斗使用的战斗技能效果实体 </summary>
    public class FightSkillEffects
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
