using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡发放记录
    /// </summary>
    public class ServerCodeLog : BaseEntity
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>平台名称</summary>
        public String platform_name { get; set; }

        /// <summary>服务器名称</summary>
        public String server_name { get; set; }

        /// <summary>序号</summary>
        public String kind { get; set; }

        /// <summary>发放福利卡类型</summary>
        public String type { get; set; }

        /// <summary>发放时间</summary>
        public String time { get; set; }
    }
}
