using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Arena
{
    /// <summary>
    /// 竞技场VO
    /// </summary>
    public class ArenaVo : BaseVo
    {
        /// <summary>主键</summary>
        public double id { get; set; }

        /// <summary> 排名 </summary>
        public int ranking { get; set; }

        /// <summary> 挑战总次数 </summary>
        public int totalCount { get; set; }

        /// <summary> 剩余次数 </summary>
        public int count { get; set; }

        /// <summary> 购买次数 </summary>
        public int buyCount { get; set; }

        /// <summary> 已清除冷却CD次数 </summary>
        public int hasCoolTime { get; set; }

        /// <summary> 挑战剩余时间 </summary>
        public decimal time { get; set; }

        /// <summary> 连胜次数 </summary>
        public int winCount { get; set; }

        /// <summary> 是否可以挑战 </ summary>
        public bool isChallage { get; set; }

        /// <summary> 战报集合 </summary>
        public List<ReportVo> report { get; set; }

        /// <summary> 可挑战玩家集合 </summary>
        public List<DekaronPlayerVo> dekaronPlayer { get; set; }

    }
}
