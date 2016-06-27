using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class InviteReceiveVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
		public double id { get; set; }
		  /// <summary>
        /// 家族id
        /// </summary>
		public int familyId{ get; set; }
		
        /// <summary>
        /// 家族名字
        /// </summary>
		public string familyName{ get; set; }

        /// <summary>
        /// 邀请人名字
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 邀请人id
        /// </summary>
        public int userId { get; set; }

    }
}
