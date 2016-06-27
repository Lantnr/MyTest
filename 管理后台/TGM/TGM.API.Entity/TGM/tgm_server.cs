using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>启服表</summary>
    [DataObject]
    [Description("启服表")]
    [BindIndex("PK__tgm_serv__3213E83F451F3D2B", true, "id")]
    [BindTable("tgm_server", Description = "启服表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_server : Itgm_server
    {
        #region 属性
        private Int32 _id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "编号", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _pid;
        /// <summary>管理员编号</summary>
        [DisplayName("管理员编号")]
        [Description("管理员编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "管理员编号", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private String _name;
        /// <summary>启服名称</summary>
        [DisplayName("启服名称")]
        [Description("启服名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "name", "启服名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
        }

        private Int32 _port_policy;
        /// <summary>启服策略端口</summary>
        [DisplayName("启服策略端口")]
        [Description("启服策略端口")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "port_policy", "启服策略端口", "10000", "int", 10, 0, false)]
        public virtual Int32 port_policy
        {
            get { return _port_policy; }
            set { if (OnPropertyChanging(__.port_policy, value)) { _port_policy = value; OnPropertyChanged(__.port_policy); } }
        }

        private Int32 _port_server;
        /// <summary>启服服务端口</summary>
        [DisplayName("启服服务端口")]
        [Description("启服服务端口")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "port_server", "启服服务端口", "20000", "int", 10, 0, false)]
        public virtual Int32 port_server
        {
            get { return _port_server; }
            set { if (OnPropertyChanging(__.port_server, value)) { _port_server = value; OnPropertyChanged(__.port_server); } }
        }

        private String _ip;
        /// <summary></summary>
        [DisplayName("ip")]
        [Description("")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(6, "ip", "", "127.0.0.1", "nvarchar(50)", 0, 0, true)]
        public virtual String ip
        {
            get { return _ip; }
            set { if (OnPropertyChanging(__.ip, value)) { _ip = value; OnPropertyChanged(__.ip); } }
        }

        private String _connect_string;
        /// <summary>数据库连接字符串</summary>
        [DisplayName("数据库连接字符串")]
        [Description("数据库连接字符串")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(7, "connect_string", "数据库连接字符串", null, "nvarchar(500)", 0, 0, true)]
        public virtual String connect_string
        {
            get { return _connect_string; }
            set { if (OnPropertyChanging(__.connect_string, value)) { _connect_string = value; OnPropertyChanged(__.connect_string); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
        }

        private String _tg_route;
        /// <summary>登陆网站</summary>
        [DisplayName("登陆网站")]
        [Description("登陆网站")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(9, "tg_route", "登陆网站", null, "nvarchar(500)", 0, 0, true)]
        public virtual String tg_route
        {
            get { return _tg_route; }
            set { if (OnPropertyChanging(__.tg_route, value)) { _tg_route = value; OnPropertyChanged(__.tg_route); } }
        }

        private String _tg_pay;
        /// <summary>支付连接</summary>
        [DisplayName("支付连接")]
        [Description("支付连接")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(10, "tg_pay", "支付连接", null, "nvarchar(500)", 0, 0, true)]
        public virtual String tg_pay
        {
            get { return _tg_pay; }
            set { if (OnPropertyChanging(__.tg_pay, value)) { _tg_pay = value; OnPropertyChanged(__.tg_pay); } }
        }

        private String _game_domain;
        /// <summary>访问域名</summary>
        [DisplayName("访问域名")]
        [Description("访问域名")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(11, "game_domain", "访问域名", null, "nvarchar(500)", 0, 0, true)]
        public virtual String game_domain
        {
            get { return _game_domain; }
            set { if (OnPropertyChanging(__.game_domain, value)) { _game_domain = value; OnPropertyChanged(__.game_domain); } }
        }

        private String _game_pay;
        /// <summary>游戏支付路径</summary>
        [DisplayName("游戏支付路径")]
        [Description("游戏支付路径")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(12, "game_pay", "游戏支付路径", null, "nvarchar(500)", 0, 0, true)]
        public virtual String game_pay
        {
            get { return _game_pay; }
            set { if (OnPropertyChanging(__.game_pay, value)) { _game_pay = value; OnPropertyChanged(__.game_pay); } }
        }

        private Int32 _server_state;
        /// <summary>开服状态(0:未启服 1:停服 2:测试 3:启服)</summary>
        [DisplayName("开服状态0:未启服1:停服2:测试3:启服")]
        [Description("开服状态(0:未启服 1:停服 2:测试 3:启服)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "server_state", "开服状态(0:未启服 1:停服 2:测试 3:启服)", "0", "int", 10, 0, false)]
        public virtual Int32 server_state
        {
            get { return _server_state; }
            set { if (OnPropertyChanging(__.server_state, value)) { _server_state = value; OnPropertyChanged(__.server_state); } }
        }

        private DateTime _server_open;
        /// <summary>启服时间</summary>
        [DisplayName("启服时间")]
        [Description("启服时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(14, "server_open", "启服时间", "getdate()", "datetime", 3, 0, false)]
        public virtual DateTime server_open
        {
            get { return _server_open; }
            set { if (OnPropertyChanging(__.server_open, value)) { _server_open = value; OnPropertyChanged(__.server_open); } }
        }

        private String _test_url;
        /// <summary>测试路径</summary>
        [DisplayName("测试路径")]
        [Description("测试路径")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(15, "test_url", "测试路径", null, "nvarchar(500)", 0, 0, true)]
        public virtual String test_url
        {
            get { return _test_url; }
            set { if (OnPropertyChanging(__.test_url, value)) { _test_url = value; OnPropertyChanged(__.test_url); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.id : return _id;
                    case __.pid : return _pid;
                    case __.name : return _name;
                    case __.port_policy : return _port_policy;
                    case __.port_server : return _port_server;
                    case __.ip : return _ip;
                    case __.connect_string : return _connect_string;
                    case __.createtime : return _createtime;
                    case __.tg_route : return _tg_route;
                    case __.tg_pay : return _tg_pay;
                    case __.game_domain : return _game_domain;
                    case __.game_pay : return _game_pay;
                    case __.server_state : return _server_state;
                    case __.server_open : return _server_open;
                    case __.test_url : return _test_url;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.pid : _pid = Convert.ToInt32(value); break;
                    case __.name : _name = Convert.ToString(value); break;
                    case __.port_policy : _port_policy = Convert.ToInt32(value); break;
                    case __.port_server : _port_server = Convert.ToInt32(value); break;
                    case __.ip : _ip = Convert.ToString(value); break;
                    case __.connect_string : _connect_string = Convert.ToString(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    case __.tg_route : _tg_route = Convert.ToString(value); break;
                    case __.tg_pay : _tg_pay = Convert.ToString(value); break;
                    case __.game_domain : _game_domain = Convert.ToString(value); break;
                    case __.game_pay : _game_pay = Convert.ToString(value); break;
                    case __.server_state : _server_state = Convert.ToInt32(value); break;
                    case __.server_open : _server_open = Convert.ToDateTime(value); break;
                    case __.test_url : _test_url = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得启服表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>管理员编号</summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>启服名称</summary>
            public static readonly Field name = FindByName(__.name);

            ///<summary>启服策略端口</summary>
            public static readonly Field port_policy = FindByName(__.port_policy);

            ///<summary>启服服务端口</summary>
            public static readonly Field port_server = FindByName(__.port_server);

            ///<summary></summary>
            public static readonly Field ip = FindByName(__.ip);

            ///<summary>数据库连接字符串</summary>
            public static readonly Field connect_string = FindByName(__.connect_string);

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            ///<summary>登陆网站</summary>
            public static readonly Field tg_route = FindByName(__.tg_route);

            ///<summary>支付连接</summary>
            public static readonly Field tg_pay = FindByName(__.tg_pay);

            ///<summary>访问域名</summary>
            public static readonly Field game_domain = FindByName(__.game_domain);

            ///<summary>游戏支付路径</summary>
            public static readonly Field game_pay = FindByName(__.game_pay);

            ///<summary>开服状态(0:未启服 1:停服 2:测试 3:启服)</summary>
            public static readonly Field server_state = FindByName(__.server_state);

            ///<summary>启服时间</summary>
            public static readonly Field server_open = FindByName(__.server_open);

            ///<summary>测试路径</summary>
            public static readonly Field test_url = FindByName(__.test_url);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得启服表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>管理员编号</summary>
            public const String pid = "pid";

            ///<summary>启服名称</summary>
            public const String name = "name";

            ///<summary>启服策略端口</summary>
            public const String port_policy = "port_policy";

            ///<summary>启服服务端口</summary>
            public const String port_server = "port_server";

            ///<summary></summary>
            public const String ip = "ip";

            ///<summary>数据库连接字符串</summary>
            public const String connect_string = "connect_string";

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

            ///<summary>登陆网站</summary>
            public const String tg_route = "tg_route";

            ///<summary>支付连接</summary>
            public const String tg_pay = "tg_pay";

            ///<summary>访问域名</summary>
            public const String game_domain = "game_domain";

            ///<summary>游戏支付路径</summary>
            public const String game_pay = "game_pay";

            ///<summary>开服状态(0:未启服 1:停服 2:测试 3:启服)</summary>
            public const String server_state = "server_state";

            ///<summary>启服时间</summary>
            public const String server_open = "server_open";

            ///<summary>测试路径</summary>
            public const String test_url = "test_url";

        }
        #endregion
    }

    /// <summary>启服表接口</summary>
    public partial interface Itgm_server
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 id { get; set; }

        /// <summary>管理员编号</summary>
        Int32 pid { get; set; }

        /// <summary>启服名称</summary>
        String name { get; set; }

        /// <summary>启服策略端口</summary>
        Int32 port_policy { get; set; }

        /// <summary>启服服务端口</summary>
        Int32 port_server { get; set; }

        /// <summary></summary>
        String ip { get; set; }

        /// <summary>数据库连接字符串</summary>
        String connect_string { get; set; }

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }

        /// <summary>登陆网站</summary>
        String tg_route { get; set; }

        /// <summary>支付连接</summary>
        String tg_pay { get; set; }

        /// <summary>访问域名</summary>
        String game_domain { get; set; }

        /// <summary>游戏支付路径</summary>
        String game_pay { get; set; }

        /// <summary>开服状态(0:未启服 1:停服 2:测试 3:启服)</summary>
        Int32 server_state { get; set; }

        /// <summary>启服时间</summary>
        DateTime server_open { get; set; }

        /// <summary>测试路径</summary>
        String test_url { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}