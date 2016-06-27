using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class FamilyListVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 族徽id
        /// </summary>
        public int clanbadge { get; set; }

        /// <summary>
        /// 家族名字
        /// </summary>
        public String familyName { get; set; }

        /// <summary>
        /// 家族等级
        /// </summary>
        public int familyLevel { get; set; }

        /// <summary>
        /// 族长名字
        /// </summary>
        public String chairmanName { get; set; }

        /// <summary>
        /// 现有族员
        /// </summary>
        public int memberValue { get; set; }

        /// <summary>
        /// 是否申请中		0:未申请		1:申请中
        /// </summary>
        public int state { get; set; }
    }
}
