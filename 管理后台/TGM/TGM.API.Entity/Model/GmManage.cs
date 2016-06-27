using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class GmManage : BaseEntity
    {
        /// <summary>主键id</summary>
        public int id { get; set; }

        /// <summary>平台id</summary>
        public int pid { get; set; }

        /// <summary>服务器id</summary>
        public int sid { get; set; }

        /// <summary>玩家Id</summary>
        public Int64 player_id { get; set; }

        /// <summary>玩家账号</summary>
        public String player_code { get; set; }

        /// <summary>操作类型</summary>
        public int state { get; set; }

        /// <summary>限制时间</summary>
        public Int64 limit_time { get; set; }

        /// <summary>玩家名称</summary>
        public String player_name { get; set; }

        /// <summary>平台名称</summary>
        public String platform_name { get; set; }

        /// <summary>服务器名称</summary>
        public String server_name { get; set; }

        /// <summary>描述</summary>
        public string describe { get; set; }

        /// <summary>创建时间</summary>
        public DateTime createtime { get; set; }

        /// <summary>操作员</summary>
        public String operate { get; set; }
    }
}
