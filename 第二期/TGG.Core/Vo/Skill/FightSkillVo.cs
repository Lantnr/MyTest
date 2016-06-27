namespace TGG.Core.Vo.Skill
{
    public class FightSkillVo : BaseVo
    {
        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>基础数据编号</summary>
        public double baseId { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>升级达到时间</summary>
        public double costTimer { get; set; }

        /// <summary>流派</summary>
        public int genre { get; set; }

        /// <summary>技能学习状态	0:未学	1:已学	2:可学	3:正在学</summary>
        public int state { get; set; }


    }
}
