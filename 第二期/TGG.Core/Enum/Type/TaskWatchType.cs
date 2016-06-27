using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 守护结果type
    /// </summary>
    public enum TaskWatchType
    {
        /// <summary>失败</summary>
        LOSE = 0,

        /// <summary>成功</summary>
        WIN = 1,

        /// <summary>完成任务</summary>
        FINISH = 2,
    }
}
