using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Skill
{
    /// <summary>
    /// LifeSkillVo 生活技能vo
    /// </summary>
    public class LifeSkillVo : BaseVo
    {
        /// <summary>编号</summary> 
        public double id { get; set; }

        /// <summary>武将主键id</summary>
        public int role_id { get; set; }

        /// <summary>基础数据编号</summary>        
        public double baseId { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>升级到达时间</summary>
        public double costTimer { get; set; }

        /// <summary>熟练度</summary>
        public int progress { get; set; }

        /// <summary>技能状态	0:未学 1:已学 2:可学 3:正在学</summary>	
        public int state { get; set; }
    }
}
