using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡类型信息
    /// </summary>
    public class GoodsType : BaseEntity
    {
        /// <summary>编号</summary>
        public Int32 id { get; set; }

        /// <summary>类型枚举ID</summary>
        public Int32 type_id { get; set; }

        /// <summary>类型名称</summary>
        public String name { get; set; }
    }
}
