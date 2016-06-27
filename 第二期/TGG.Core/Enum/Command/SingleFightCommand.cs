using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// SingleFightCommand一将讨相关命令
    /// </summary>
    public enum SingleFightCommand
    {
        /// <summary>
        /// 一将讨挑战
        /// 前端传递数据:
        /// id:[int] 一将讨基表Id
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        SINGLE_FIGHT_NPC = 1,
    }
}
