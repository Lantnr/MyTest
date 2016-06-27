using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary> 战斗结果枚举类型 </summary>
    public enum FightResultType
    {
        /// <summary> 胜利 </summary>
        WIN = 1,
        /// <summary> 失败 </summary>
        LOSE = 0,

        /// <summary> 对手错误 </summary>
        RIVAL_ERROR = -140003,

        /// <summary> 战斗类型错误 </summary>
        FIGHT_TYPE_ERROR = -14004,

        /// <summary> 战斗出错错误 </summary>
        FIGHT_ERROR = -14005,

        /// <summary> 拉取对手阵形错误 </summary>
        RIVAL_PERSONAL_ERROR = -14006,

        /// <summary> 对手武将ID获取错误 </summary>
        RIVAL_ID_ERROR = -14007,

        /// <summary> NPC基表错误 </summary>
        NPC_BASE_ERROR = -14008,

        /// <summary> 初始值 </summary>
        DEFAULT = -1,
    }
}
