using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Arena
{
    /// <summary>
    /// 竞技场-战报Vo
    /// </summary>
    public class ReportVo:BaseVo
    {
        /// <summary> id </summary>
        public double id { get; set; }

        /// <summary> 玩家昵称 </summary>
        public string playerName { get; set; }

        /// <summary> 时间 </summary>
        public string time { get; set; }

        /// <summary> 类型 0:攻击 1:被攻击 </summary>
        public int type { get; set; }

        /// <summary> 是否胜利 </summary>
        public bool isWin { get; set; }
    }
}
