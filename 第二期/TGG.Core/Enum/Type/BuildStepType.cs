using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 一夜墨俣活动步骤
    /// </summary>
    public enum BuildStepType
    {
        /// <summary> 收集木材</summary>
        GET_WOOD = 1,

        /// <summary> 收集火把</summary>
        GET_TORCH = 2,

        /// <summary> 收集木材</summary>
        MAKE_BUILD = 3,

        /// <summary> 筑城</summary>
        BUILD = 4,

        /// <summary> 放火</summary>
        FIRE = 5,
    }
}
