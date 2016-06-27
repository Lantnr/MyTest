using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 刺杀任务结果枚举
    /// </summary>
    public enum TaskKillType
    {
        /// <summary>未挑战</summary>
        NOT_FIGHT = 0,

        /// <summary>成功</summary>
        WIN = 1,

        /// <summary>失败</summary>
        LOSE = 2,
    }
}
