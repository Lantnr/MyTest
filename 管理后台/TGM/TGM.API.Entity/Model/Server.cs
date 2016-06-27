using System;
using System.ComponentModel.DataAnnotations;

namespace TGM.API.Entity.Model
{
    /// <summary>服务器列表</summary> 
    public class Server : BaseEntity
    {
        #region 属性
        /// <summary>编号</summary>
        public Int32 id { get; set; }

        /// <summary>平台编号</summary>
        public Int32 pid { get; set; }

        /// <summary>平台名称</summary>
        public String platform_name { get; set; }

        /// <summary>服务器名称</summary>
        public String name { get; set; }

        /// <summary>启服策略端口</summary>
        public Int32 port_policy { get; set; }

        /// <summary>启服服务端口</summary>
        public Int32 port_server { get; set; }

        /// <summary>游戏ip</summary>
        public String ip { get; set; }

        /// <summary>数据库连接字符串</summary>
        public String connect_string { get; set; }

        /// <summary>创建时间</summary>
        public Int64 createtime { get; set; }

        /// <summary>登陆网站</summary>
        public String tg_route { get; set; }

        /// <summary>支付连接</summary>
        public String tg_pay { get; set; }

        /// <summary>游戏访问域名</summary>
        public String game_domain { get; set; }

        /// <summary>游戏支付路径</summary>
        public String game_pay { get; set; }

        /// <summary>开服状态(0:未启服 1:停服 2:测试 3:启服)</summary>
        public Int32 server_state { get; set; }

        /// <summary>状态说明</summary>
        public String state_name { get; set; }

        /// <summary>启服时间</summary>
        public String server_open { get; set; }

        /// <summary>测试路径</summary>
        public String test_url { get; set; }

        #endregion


    }
}