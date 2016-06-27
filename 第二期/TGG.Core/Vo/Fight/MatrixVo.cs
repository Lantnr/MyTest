using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Fight
{
    /// <summary>
    /// 阵Vo
    /// </summary>
    public class MatrixVo : BaseVo
    {
        /// <summary>主键id</summary>
        public double id { get; set; }

        /// <summary>印Vo</summary>
        public YinVo yinVo { get; set; }

        /// <summary>阵法位置1武将表编号</summary>
        public Decimal matrix1_rid { get; set; }

        /// <summary>阵法位置2武将编号</summary>
        public Decimal matrix2_rid { get; set; }

        /// <summary>阵法位置3武将编号</summary>
        public Decimal matrix3_rid { get; set; }

        /// <summary>阵法位置4武将编号</summary>
        public Decimal matrix4_rid { get; set; }

        /// <summary>阵法位置5武将编号</summary>
        public Decimal matrix5_rid { get; set; }
    }
}
