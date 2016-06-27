using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Arena
{
    /// <summary>
    /// 竞技场-排名Vo
    /// </summary>
    public class RankingVo:BaseVo
    {
        /// <summary> 排名 </summary>
        public int ranking { get; set; }

        /// <summary> 玩家昵称 </summary>
        public string playerName { get; set; }

        /// <summary> 玩家职业 </summary>
        public int vocation { get; set; }
    }
}
