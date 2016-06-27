using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.RoleTrain
{
    /// <summary>
    /// TrainVo 武将修炼vo
    /// </summary>
    public class TrainVo : BaseVo
    {
        /// <summary>编号</summary> 
        public double id { get; set; }

        /// <summary>是否修行 </summary>
        public int state { get; set; }

        /// <summary> 修炼武将属性类型 </summary>        
        public int type { get; set; }

        /// <summary>修行等级</summary>
        public int lv { get; set; }

        /// <summary>结束时间 </summary>
        public double time { get; set; }
    }
}
