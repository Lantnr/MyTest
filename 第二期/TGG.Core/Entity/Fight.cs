using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;

namespace TGG.Core.Entity
{
    /// <summary> 战斗实体 </summary>
    public class Fight
    {
        /// <summary> 己方是否胜利 </summary>
        public bool Iswin { get; set; }

        /// <summary> 玩家剩余血量 </summary>
        public Int64 PlayHp { get; set; }

        /// <summary> Boss剩余血量 Boss专用</summary>
        public Int64 BoosHp { get; set; }

        /// <summary> 己方对敌方造成的伤害 </summary>
        public Int64 Hurt { get; set; }

        /// <summary> 己方战斗Vo </summary>
        public FightVo Ofight { get; set; }

        /// <summary> 敌方战斗Vo </summary>
        public FightVo Rfight { get; set; }

        /// <summary> 出手总次数 </summary>
        public int ShotCount { get; set; }

        /// <summary> 战斗处理结果 </summary>
        public ResultType Result { get; set; }

    }
}
