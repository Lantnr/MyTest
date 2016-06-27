using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class FamilyApplyVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 玩家编号
        /// </summary>
        public double userid { get; set; }

        /// <summary>
        /// 玩家名字
        /// </summary>
		public string userName{ get; set; }
		
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public double time { get; set; }
    }
}
