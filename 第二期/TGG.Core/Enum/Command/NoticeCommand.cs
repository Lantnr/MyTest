using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 公告指令枚举
    /// </summary>
    public enum NoticeCommand
    {
        /// <summary>
        /// 系统公告
        /// 前端传递数据:
        /// 服务器响应数据:
        /// id:[int]基表Id
        /// content:[string] 内容
        /// </summary>
        SYSTEM_NOTICE = 1,
    }
}
