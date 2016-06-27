using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Activity
{
    public class ActivityOpenVo : BaseVo
    {
        /// <summary>
        /// 活动开启状态 0：未开启 1：开启
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 功能开放表id
        /// </summary>
        public int openId { get; set; }
    }
}
