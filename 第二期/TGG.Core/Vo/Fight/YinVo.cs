using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Fight
{
    /// <summary> 印Vo</summary>
    [Serializable]
    public class YinVo : BaseVo
    {
        /// <summary>主键id</summary>
        public double id { get; set; }

        /// <summary>印基表编号</summary>
        public int baseid { get; set; }

        /// <summary>印等级</summary>
        public int level { get; set; }

        /// <summary>状态 0:未使用 1:使用中</summary>
        public int state { get; set; }
    }
}
