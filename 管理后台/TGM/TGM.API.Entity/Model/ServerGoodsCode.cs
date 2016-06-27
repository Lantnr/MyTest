using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡激活码分页信息
    /// </summary>
    public class ServerGoodsCode : BaseEntity
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>服务器名称</summary>
        public String server_name { get; set; }

        /// <summary>平台名称</summary>
        public String platform_name { get; set; }

        /// <summary>福利卡类型</summary>
        public String type { get; set; }

        /// <summary>激活码</summary>
        public String card_key { get; set; }

        /// <summary>生成批次</summary>
        public String kind { get; set; }
    }
}
