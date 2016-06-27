using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Guide
{
    /// <summary>
    /// 大名令任务信息Vo
    /// </summary>
    public class DaMingLingVo : BaseVo
    {
        /// <summary>主键id</summary>
        public double id { get; set; }

        /// <summary>基表id</summary>
        public int baseid { get; set; }

        /// <summary>任务状态  0:未领取	1:可领取	2:已领取</summary>
        public int state { get; set; }

        /// <summary>任务进度值</summary>
        public int degree { get; set; }
    }
}
