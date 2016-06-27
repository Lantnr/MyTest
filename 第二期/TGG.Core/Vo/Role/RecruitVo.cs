using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Role
{
    /// <summary>
    /// 武将招募Vo
    /// </summary>
    public class RecruitVo : BaseVo
    {
        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>基表武将id</summary>
        public int baseId { get; set; }

        /// <summary>位置</summary>
        public int position { get; set; }

        /// <summary>卡牌状态 0:未招募 1:已招募</summary>
        public int state { get; set; }
    }
}
