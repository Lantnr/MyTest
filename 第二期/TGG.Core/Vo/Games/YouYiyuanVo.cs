using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Games
{
    /// <summary>
    /// 游艺园VO
    /// </summary>
    public class YouYiyuanVo : BaseVo
    {
        /// <summary>游戏类型</summary>
        public int type { get; set; }

        /// <summary>剩余次数</summary>
        public int num { get; set; }

        /// <summary>最高关数</summary>
        public int passMax { get; set; }
    }
}
