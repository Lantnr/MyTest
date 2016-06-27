using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Duplicate
{
    /// <summary>
    /// 名塔Vo
    /// </summary>
    public class ShotVo : BaseVo
    {
        /// <summary>名次</summary>
        public int ranking { get; set; }

        /// <summary>玩家名字 </summary>
        public string player_name { get; set; }        

        /// <summary>闯关总分最高分数</summary>
        public int score_highest { get; set; }

        /// <summary>玩家自身武将id</summary>
        public double role_id { get; set; }
    }
}
