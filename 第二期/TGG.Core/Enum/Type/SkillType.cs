using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 技能类型枚举
    /// </summary>
    public enum SkillType
    {
        /// <summary> 奥义 </summary>
        MYSTERY = 1,

        /// <summary> 秘技 </summary>
        CHEATCODE = 2,

        /// <summary> 印 </summary>
        YIN = 3,

        /// <summary> 其他默认 </summary>
        COMMON = 1000,

        /// <summary> 套卡 </summary>
        CARD = 9999,
    }
}
