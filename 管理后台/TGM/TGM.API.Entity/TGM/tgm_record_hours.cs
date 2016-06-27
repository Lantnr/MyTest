using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>游戏小时统计记录</summary>
    [DataObject]
    [Description("游戏小时统计记录")]
    [BindIndex("PK__tgm_reco__3213E83F09A971A2", true, "id")]
    [BindTable("tgm_record_hours", Description = "游戏小时统计记录", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_record_hours : Itgm_record_hours
    {
        #region 属性
        private Int64 _id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "编号", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _pid;
        /// <summary></summary>
        [DisplayName("Pid")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private Int32 _sid;
        /// <summary></summary>
        [DisplayName("Sid")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "sid", "", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private String _server_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "server_name", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String server_name
        {
            get { return _server_name; }
            set { if (OnPropertyChanging(__.server_name, value)) { _server_name = value; OnPropertyChanged(__.server_name); } }
        }

        private Int32 _online;
        /// <summary>在线人数</summary>
        [DisplayName("在线人数")]
        [Description("在线人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "online", "在线人数", "0", "int", 10, 0, false)]
        public virtual Int32 online
        {
            get { return _online; }
            set { if (OnPropertyChanging(__.online, value)) { _online = value; OnPropertyChanged(__.online); } }
        }

        private Int32 _offline;
        /// <summary>离线人数</summary>
        [DisplayName("离线人数")]
        [Description("离线人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "offline", "离线人数", "0", "int", 10, 0, false)]
        public virtual Int32 offline
        {
            get { return _offline; }
            set { if (OnPropertyChanging(__.offline, value)) { _offline = value; OnPropertyChanged(__.offline); } }
        }

        private Int32 _register;
        /// <summary>今日注册人数</summary>
        [DisplayName("今日注册人数")]
        [Description("今日注册人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "register", "今日注册人数", "0", "int", 10, 0, false)]
        public virtual Int32 register
        {
            get { return _register; }
            set { if (OnPropertyChanging(__.register, value)) { _register = value; OnPropertyChanged(__.register); } }
        }

        private Int32 _register_total;
        /// <summary>注册总人数</summary>
        [DisplayName("注册总人数")]
        [Description("注册总人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "register_total", "注册总人数", "0", "int", 10, 0, false)]
        public virtual Int32 register_total
        {
            get { return _register_total; }
            set { if (OnPropertyChanging(__.register_total, value)) { _register_total = value; OnPropertyChanged(__.register_total); } }
        }

        private Int32 _taday_online;
        /// <summary>今日最高在线人数</summary>
        [DisplayName("今日最高在线人数")]
        [Description("今日最高在线人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "taday_online", "今日最高在线人数", "0", "int", 10, 0, false)]
        public virtual Int32 taday_online
        {
            get { return _taday_online; }
            set { if (OnPropertyChanging(__.taday_online, value)) { _taday_online = value; OnPropertyChanged(__.taday_online); } }
        }

        private Int32 _history_online;
        /// <summary>历史最高在线人数</summary>
        [DisplayName("历史最高在线人数")]
        [Description("历史最高在线人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "history_online", "历史最高在线人数", "0", "int", 10, 0, false)]
        public virtual Int32 history_online
        {
            get { return _history_online; }
            set { if (OnPropertyChanging(__.history_online, value)) { _history_online = value; OnPropertyChanged(__.history_online); } }
        }

        private Int32 _taday_login;
        /// <summary>今日登陆人数</summary>
        [DisplayName("今日登陆人数")]
        [Description("今日登陆人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "taday_login", "今日登陆人数", "0", "int", 10, 0, false)]
        public virtual Int32 taday_login
        {
            get { return _taday_login; }
            set { if (OnPropertyChanging(__.taday_login, value)) { _taday_login = value; OnPropertyChanged(__.taday_login); } }
        }

        private Int32 _pay_number;
        /// <summary>今日充值人数</summary>
        [DisplayName("今日充值人数")]
        [Description("今日充值人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "pay_number", "今日充值人数", "0", "int", 10, 0, false)]
        public virtual Int32 pay_number
        {
            get { return _pay_number; }
            set { if (OnPropertyChanging(__.pay_number, value)) { _pay_number = value; OnPropertyChanged(__.pay_number); } }
        }

        private Int32 _pay_count;
        /// <summary>今日充值次数</summary>
        [DisplayName("今日充值次数")]
        [Description("今日充值次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "pay_count", "今日充值次数", "0", "int", 10, 0, false)]
        public virtual Int32 pay_count
        {
            get { return _pay_count; }
            set { if (OnPropertyChanging(__.pay_count, value)) { _pay_count = value; OnPropertyChanged(__.pay_count); } }
        }

        private Int32 _pay_taday;
        /// <summary>今日充值</summary>
        [DisplayName("今日充值")]
        [Description("今日充值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "pay_taday", "今日充值", "0", "int", 10, 0, false)]
        public virtual Int32 pay_taday
        {
            get { return _pay_taday; }
            set { if (OnPropertyChanging(__.pay_taday, value)) { _pay_taday = value; OnPropertyChanged(__.pay_taday); } }
        }

        private Int32 _pay_total;
        /// <summary>总充值</summary>
        [DisplayName("总充值")]
        [Description("总充值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "pay_total", "总充值", "0", "int", 10, 0, false)]
        public virtual Int32 pay_total
        {
            get { return _pay_total; }
            set { if (OnPropertyChanging(__.pay_total, value)) { _pay_total = value; OnPropertyChanged(__.pay_total); } }
        }

        private Int32 _pay_month;
        /// <summary>月充值</summary>
        [DisplayName("月充值")]
        [Description("月充值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "pay_month", "月充值", "0", "int", 10, 0, false)]
        public virtual Int32 pay_month
        {
            get { return _pay_month; }
            set { if (OnPropertyChanging(__.pay_month, value)) { _pay_month = value; OnPropertyChanged(__.pay_month); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(17, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
        }

        private Int32 _taday_cost;
        /// <summary>今日元宝消耗</summary>
        [DisplayName("今日元宝消耗")]
        [Description("今日元宝消耗")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "taday_cost", "今日元宝消耗", "0", "int", 10, 0, false)]
        public virtual Int32 taday_cost
        {
            get { return _taday_cost; }
            set { if (OnPropertyChanging(__.taday_cost, value)) { _taday_cost = value; OnPropertyChanged(__.taday_cost); } }
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
                    case __.sid : return _sid;
                    case __.server_name : return _server_name;
                    case __.online : return _online;
                    case __.offline : return _offline;
                    case __.register : return _register;
                    case __.register_total : return _register_total;
                    case __.taday_online : return _taday_online;
                    case __.history_online : return _history_online;
                    case __.taday_login : return _taday_login;
                    case __.pay_number : return _pay_number;
                    case __.pay_count : return _pay_count;
                    case __.pay_taday : return _pay_taday;
                    case __.pay_total : return _pay_total;
                    case __.pay_month : return _pay_month;
                    case __.createtime : return _createtime;
                    case __.taday_cost : return _taday_cost;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.pid : _pid = Convert.ToInt32(value); break;
                    case __.sid : _sid = Convert.ToInt32(value); break;
                    case __.server_name : _server_name = Convert.ToString(value); break;
                    case __.online : _online = Convert.ToInt32(value); break;
                    case __.offline : _offline = Convert.ToInt32(value); break;
                    case __.register : _register = Convert.ToInt32(value); break;
                    case __.register_total : _register_total = Convert.ToInt32(value); break;
                    case __.taday_online : _taday_online = Convert.ToInt32(value); break;
                    case __.history_online : _history_online = Convert.ToInt32(value); break;
                    case __.taday_login : _taday_login = Convert.ToInt32(value); break;
                    case __.pay_number : _pay_number = Convert.ToInt32(value); break;
                    case __.pay_count : _pay_count = Convert.ToInt32(value); break;
                    case __.pay_taday : _pay_taday = Convert.ToInt32(value); break;
                    case __.pay_total : _pay_total = Convert.ToInt32(value); break;
                    case __.pay_month : _pay_month = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    case __.taday_cost : _taday_cost = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得游戏小时统计记录字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary></summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary></summary>
            public static readonly Field server_name = FindByName(__.server_name);

            ///<summary>在线人数</summary>
            public static readonly Field online = FindByName(__.online);

            ///<summary>离线人数</summary>
            public static readonly Field offline = FindByName(__.offline);

            ///<summary>今日注册人数</summary>
            public static readonly Field register = FindByName(__.register);

            ///<summary>注册总人数</summary>
            public static readonly Field register_total = FindByName(__.register_total);

            ///<summary>今日最高在线人数</summary>
            public static readonly Field taday_online = FindByName(__.taday_online);

            ///<summary>历史最高在线人数</summary>
            public static readonly Field history_online = FindByName(__.history_online);

            ///<summary>今日登陆人数</summary>
            public static readonly Field taday_login = FindByName(__.taday_login);

            ///<summary>今日充值人数</summary>
            public static readonly Field pay_number = FindByName(__.pay_number);

            ///<summary>今日充值次数</summary>
            public static readonly Field pay_count = FindByName(__.pay_count);

            ///<summary>今日充值</summary>
            public static readonly Field pay_taday = FindByName(__.pay_taday);

            ///<summary>总充值</summary>
            public static readonly Field pay_total = FindByName(__.pay_total);

            ///<summary>月充值</summary>
            public static readonly Field pay_month = FindByName(__.pay_month);

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            ///<summary>今日元宝消耗</summary>
            public static readonly Field taday_cost = FindByName(__.taday_cost);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得游戏小时统计记录字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary></summary>
            public const String pid = "pid";

            ///<summary></summary>
            public const String sid = "sid";

            ///<summary></summary>
            public const String server_name = "server_name";

            ///<summary>在线人数</summary>
            public const String online = "online";

            ///<summary>离线人数</summary>
            public const String offline = "offline";

            ///<summary>今日注册人数</summary>
            public const String register = "register";

            ///<summary>注册总人数</summary>
            public const String register_total = "register_total";

            ///<summary>今日最高在线人数</summary>
            public const String taday_online = "taday_online";

            ///<summary>历史最高在线人数</summary>
            public const String history_online = "history_online";

            ///<summary>今日登陆人数</summary>
            public const String taday_login = "taday_login";

            ///<summary>今日充值人数</summary>
            public const String pay_number = "pay_number";

            ///<summary>今日充值次数</summary>
            public const String pay_count = "pay_count";

            ///<summary>今日充值</summary>
            public const String pay_taday = "pay_taday";

            ///<summary>总充值</summary>
            public const String pay_total = "pay_total";

            ///<summary>月充值</summary>
            public const String pay_month = "pay_month";

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

            ///<summary>今日元宝消耗</summary>
            public const String taday_cost = "taday_cost";

        }
        #endregion
    }

    /// <summary>游戏小时统计记录接口</summary>
    public partial interface Itgm_record_hours
    {
        #region 属性
        /// <summary>编号</summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int32 pid { get; set; }

        /// <summary></summary>
        Int32 sid { get; set; }

        /// <summary></summary>
        String server_name { get; set; }

        /// <summary>在线人数</summary>
        Int32 online { get; set; }

        /// <summary>离线人数</summary>
        Int32 offline { get; set; }

        /// <summary>今日注册人数</summary>
        Int32 register { get; set; }

        /// <summary>注册总人数</summary>
        Int32 register_total { get; set; }

        /// <summary>今日最高在线人数</summary>
        Int32 taday_online { get; set; }

        /// <summary>历史最高在线人数</summary>
        Int32 history_online { get; set; }

        /// <summary>今日登陆人数</summary>
        Int32 taday_login { get; set; }

        /// <summary>今日充值人数</summary>
        Int32 pay_number { get; set; }

        /// <summary>今日充值次数</summary>
        Int32 pay_count { get; set; }

        /// <summary>今日充值</summary>
        Int32 pay_taday { get; set; }

        /// <summary>总充值</summary>
        Int32 pay_total { get; set; }

        /// <summary>月充值</summary>
        Int32 pay_month { get; set; }

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }

        /// <summary>今日元宝消耗</summary>
        Int32 taday_cost { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}