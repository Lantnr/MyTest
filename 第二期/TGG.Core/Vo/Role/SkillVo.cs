using System;
namespace TGG.Core.Vo.Role
{
    /// <summary>
    /// 技能Vo
    /// </summary>
    [Serializable]
    public class SkillVo : BaseVo
    {
        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>基础数据编号</summary>
        public double baseId { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>技能到达时间</summary>
        public double skill_time { get; set; }
    }
}
