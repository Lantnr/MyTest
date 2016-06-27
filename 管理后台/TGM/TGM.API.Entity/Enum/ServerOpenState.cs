using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Enum
{
    /// <summary>
    /// 服务器状态枚举类型
    /// </summary>
    public enum ServerOpenState
    {
        未启服 = 0,
        停服 = 1,
        测试 = 2,
        启服 = 3,
    }
}
