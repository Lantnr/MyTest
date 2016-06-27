using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Prison
{
    /// <summary>
    /// 监狱留言vo
    /// </summary>
    public class PrisonMessageVo : BaseVo
    {

        /// <summary>玩家名称</summary>
        public string name { get; set; }

        /// <summary> 留言内容 </summary>
        /// 
        public string content { get; set; }

        /// <summary>日期 Y-M-D H:M:S </summary>
        public string date { get; set; }
    }
}
