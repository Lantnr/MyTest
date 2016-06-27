using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>用户报表</summary>
    [DataObject]
    [Description("用户报表")]
    [BindIndex("PK__report_d__3213E83F0CBAE877", true, "id")]
    [BindTable("report_day", Description = "用户报表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class report_day : Ireport_day
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

        private Int32 _online;
        /// <summary>在线人数</summary>
        [DisplayName("在线人数")]
        [Description("在线人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "online", "在线人数", "0", "int", 10, 0, false)]
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
        [BindColumn(3, "offline", "离线人数", "0", "int", 10, 0, false)]
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
        [BindColumn(4, "register", "今日注册人数", "0", "int", 10, 0, false)]
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
        [BindColumn(5, "register_total", "注册总人数", "0", "int", 10, 0, false)]
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
        [BindColumn(6, "taday_online", "今日最高在线人数", "0", "int", 10, 0, false)]
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
        [BindColumn(7, "history_online", "历史最高在线人数", "0", "int", 10, 0, false)]
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
        [BindColumn(8, "taday_login", "今日登陆人数", "0", "int", 10, 0, false)]
        public virtual Int32 taday_login
        {
            get { return _taday_login; }
            set { if (OnPropertyChanging(__.taday_login, value)) { _taday_login = value; OnPropertyChanged(__.taday_login); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
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
                    case __.online : return _online;
                    case __.offline : return _offline;
                    case __.register : return _register;
                    case __.register_total : return _register_total;
                    case __.taday_online : return _taday_online;
                    case __.history_online : return _history_online;
                    case __.taday_login : return _taday_login;
                    case __.createtime : return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.online : _online = Convert.ToInt32(value); break;
                    case __.offline : _offline = Convert.ToInt32(value); break;
                    case __.register : _register = Convert.ToInt32(value); break;
                    case __.register_total : _register_total = Convert.ToInt32(value); break;
                    case __.taday_online : _taday_online = Convert.ToInt32(value); break;
                    case __.history_online : _history_online = Convert.ToInt32(value); break;
                    case __.taday_login : _taday_login = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户报表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

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

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户报表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

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

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>用户报表接口</summary>
    public partial interface Ireport_day
    {
        #region 属性
        /// <summary>编号</summary>
        Int64 id { get; set; }

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

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}