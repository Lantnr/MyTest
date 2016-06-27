using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 公告状态
    /// </summary>
    public enum NoticeStateType
    {
        /// <summary> 等待 </summary>
        WAIT = 0,

        /// <summary> 运行中 </summary>
        RUN = 1,

        /// <summary> 过时 </summary>
        OBSOLETE = -1,
    }
}
