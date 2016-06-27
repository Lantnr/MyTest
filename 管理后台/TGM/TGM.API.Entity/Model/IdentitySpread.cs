using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>身份分布信息列表</summary>
    public class IdentitySpread : BaseEntity
    {
        /// <summary>编号id</summary>
        public Int32 id { get; set; }

        /// <summary>第一阶身份人数</summary>
        public Int32 identity1_count { get; set; }

        /// <summary>第二阶身份人数</summary>
        public Int32 identity2_count { get; set; }

        /// <summary>第三阶身份人数</summary>
        public Int32 identity3_count { get; set; }

        /// <summary>第四阶身份人数</summary>
        public Int32 identity4_count { get; set; }

        /// <summary>第五阶身份人数</summary>
        public Int32 identity5_count { get; set; }

        /// <summary>第六阶身份人数</summary>
        public Int32 identity6_count { get; set; }

        /// <summary>第七阶身份人数</summary>
        public Int32 identity7_count { get; set; }

        /// <summary>时间</summary>
        public String createtime { get; set; }
    }
}
