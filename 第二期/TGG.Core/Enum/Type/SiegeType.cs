using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 美浓攻略类型
    /// </summary>
    public enum SiegeType
    {
        /// <summary> 云梯上限 </summary>
        YUNTI_LIMIT = 1,

        /// <summary> 云梯制作几率 </summary>
        YUNTI_ODDS = 2,

        /// <summary> 破坏城门 </summary>
        GATE_HURT = 3,

        /// <summary> 破坏城门几率 </summary>
        GATE_ODDS = 4,
    }
}
