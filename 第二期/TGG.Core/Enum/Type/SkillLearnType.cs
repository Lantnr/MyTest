using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 生活技能学习状态类型
    /// </summary>
    public enum SkillLearnType
    {
        /// <summary>未学</summary>
        NOSCHOOL = 0,       

        /// <summary>已学</summary>
        LEARNED = 1,

        /// <summary>可学</summary>
        TOLEARN = 2,

        /// <summary>正在学</summary>
        STUDYING = 3,        
    }
}
