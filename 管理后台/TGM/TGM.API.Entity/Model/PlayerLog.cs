using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 玩家日志
    /// </summary>
    public class PlayerLog : BaseEntity
    {
        /// <summary>编号id</summary>
        public Int64 id { get; set; }

        /// <summary>模块号</summary>
        public Int32 module_number { get; set; }

        /// <summary>模块号名称</summary>
        public String module_name { get; set; }

        /// <summary>指令号</summary>
        public Int32 command_number { get; set; }

        /// <summary>指令号名称</summary>
        public String command_name { get; set; }

        /// <summary>变动类型</summary>
        public Int32 changes_type { get; set; }

        /// <summary>资源类型</summary>
        public Int32 resources_type { get; set; }

        /// <summary>资源名称</summary>
        public String resources_name { get; set; }

        /// <summary>变动数量</summary>
        public Int64 count { get; set; }

        /// <summary>剩余资源</summary>
        public Int64 surplus { get; set; }

        /// <summary>时间</summary>
        public String time { get; set; }
    }
}
