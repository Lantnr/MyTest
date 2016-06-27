using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 武将修行状态类型
    /// </summary>
    public enum RoleTrainStatusType
    {
        /// <summary>
        /// 空闲
        /// </summary>
        FREE = 0,
        /// <summary>
        /// 未修行
        /// </summary>
        STOP = 1,
        /// <summary>
        /// 修行中
        /// </summary>
        TRAINING = 2,      
    }
}
