using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家登陆日志表</summary>
    [DataObject]
    [Description("玩家登陆日志表")]
    [BindIndex("PK__tg_user___3213E83F7F60ED59", true, "id")]
    [BindTable("tg_user_login_log", Description = "玩家登陆日志表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_login_log : Itg_user_login_log
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

        private Int64 _user_id;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private String _login_ip;
        /// <summary>登陆IP</summary>
        [DisplayName("登陆IP")]
        [Description("登陆IP")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "login_ip", "登陆IP", "127.0.0.1", "nvarchar(50)", 0, 0, true)]
        public virtual String login_ip
        {
            get { return _login_ip; }
            set { if (OnPropertyChanging(__.login_ip, value)) { _login_ip = value; OnPropertyChanged(__.login_ip); } }
        }

        private Int32 _login_state;
        /// <summary>登陆状态</summary>
        [DisplayName("登陆状态")]
        [Description("登陆状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "login_state", "登陆状态", "0", "int", 10, 0, false)]
        public virtual Int32 login_state
        {
            get { return _login_state; }
            set { if (OnPropertyChanging(__.login_state, value)) { _login_state = value; OnPropertyChanged(__.login_state); } }
        }

        private Int64 _login_time;
        /// <summary>上线时间</summary>
        [DisplayName("上线时间")]
        [Description("上线时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "login_time", "上线时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 login_time
        {
            get { return _login_time; }
            set { if (OnPropertyChanging(__.login_time, value)) { _login_time = value; OnPropertyChanged(__.login_time); } }
        }

        private Int64 _logout_time;
        /// <summary>下线时间</summary>
        [DisplayName("下线时间")]
        [Description("下线时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "logout_time", "下线时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 logout_time
        {
            get { return _logout_time; }
            set { if (OnPropertyChanging(__.logout_time, value)) { _logout_time = value; OnPropertyChanged(__.logout_time); } }
        }

        private Int32 _login_count_day;
        /// <summary>每日登录次数</summary>
        [DisplayName("每日登录次数")]
        [Description("每日登录次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "login_count_day", "每日登录次数", "0", "int", 10, 0, false)]
        public virtual Int32 login_count_day
        {
            get { return _login_count_day; }
            set { if (OnPropertyChanging(__.login_count_day, value)) { _login_count_day = value; OnPropertyChanged(__.login_count_day); } }
        }

        private Int32 _login_count_total;
        /// <summary>登陆总次数</summary>
        [DisplayName("登陆总次数")]
        [Description("登陆总次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "login_count_total", "登陆总次数", "0", "int", 10, 0, false)]
        public virtual Int32 login_count_total
        {
            get { return _login_count_total; }
            set { if (OnPropertyChanging(__.login_count_total, value)) { _login_count_total = value; OnPropertyChanged(__.login_count_total); } }
        }

        private Int64 _login_time_longer_day;
        /// <summary>每日在线时长</summary>
        [DisplayName("每日在线时长")]
        [Description("每日在线时长")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "login_time_longer_day", "每日在线时长", "0", "bigint", 19, 0, false)]
        public virtual Int64 login_time_longer_day
        {
            get { return _login_time_longer_day; }
            set { if (OnPropertyChanging(__.login_time_longer_day, value)) { _login_time_longer_day = value; OnPropertyChanged(__.login_time_longer_day); } }
        }

        private Int64 _login_time_longer_total;
        /// <summary>总在线时长</summary>
        [DisplayName("总在线时长")]
        [Description("总在线时长")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "login_time_longer_total", "总在线时长", "0", "bigint", 19, 0, false)]
        public virtual Int64 login_time_longer_total
        {
            get { return _login_time_longer_total; }
            set { if (OnPropertyChanging(__.login_time_longer_total, value)) { _login_time_longer_total = value; OnPropertyChanged(__.login_time_longer_total); } }
        }

        private Int64 _login_open_time;
        /// <summary>防沉迷到达时间</summary>
        [DisplayName("防沉迷到达时间")]
        [Description("防沉迷到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "login_open_time", "防沉迷到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 login_open_time
        {
            get { return _login_open_time; }
            set { if (OnPropertyChanging(__.login_open_time, value)) { _login_open_time = value; OnPropertyChanged(__.login_open_time); } }
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
                    case __.user_id : return _user_id;
                    case __.login_ip : return _login_ip;
                    case __.login_state : return _login_state;
                    case __.login_time : return _login_time;
                    case __.logout_time : return _logout_time;
                    case __.login_count_day : return _login_count_day;
                    case __.login_count_total : return _login_count_total;
                    case __.login_time_longer_day : return _login_time_longer_day;
                    case __.login_time_longer_total : return _login_time_longer_total;
                    case __.login_open_time : return _login_open_time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.login_ip : _login_ip = Convert.ToString(value); break;
                    case __.login_state : _login_state = Convert.ToInt32(value); break;
                    case __.login_time : _login_time = Convert.ToInt64(value); break;
                    case __.logout_time : _logout_time = Convert.ToInt64(value); break;
                    case __.login_count_day : _login_count_day = Convert.ToInt32(value); break;
                    case __.login_count_total : _login_count_total = Convert.ToInt32(value); break;
                    case __.login_time_longer_day : _login_time_longer_day = Convert.ToInt64(value); break;
                    case __.login_time_longer_total : _login_time_longer_total = Convert.ToInt64(value); break;
                    case __.login_open_time : _login_open_time = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家登陆日志表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>登陆IP</summary>
            public static readonly Field login_ip = FindByName(__.login_ip);

            ///<summary>登陆状态</summary>
            public static readonly Field login_state = FindByName(__.login_state);

            ///<summary>上线时间</summary>
            public static readonly Field login_time = FindByName(__.login_time);

            ///<summary>下线时间</summary>
            public static readonly Field logout_time = FindByName(__.logout_time);

            ///<summary>每日登录次数</summary>
            public static readonly Field login_count_day = FindByName(__.login_count_day);

            ///<summary>登陆总次数</summary>
            public static readonly Field login_count_total = FindByName(__.login_count_total);

            ///<summary>每日在线时长</summary>
            public static readonly Field login_time_longer_day = FindByName(__.login_time_longer_day);

            ///<summary>总在线时长</summary>
            public static readonly Field login_time_longer_total = FindByName(__.login_time_longer_total);

            ///<summary>防沉迷到达时间</summary>
            public static readonly Field login_open_time = FindByName(__.login_open_time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家登陆日志表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>登陆IP</summary>
            public const String login_ip = "login_ip";

            ///<summary>登陆状态</summary>
            public const String login_state = "login_state";

            ///<summary>上线时间</summary>
            public const String login_time = "login_time";

            ///<summary>下线时间</summary>
            public const String logout_time = "logout_time";

            ///<summary>每日登录次数</summary>
            public const String login_count_day = "login_count_day";

            ///<summary>登陆总次数</summary>
            public const String login_count_total = "login_count_total";

            ///<summary>每日在线时长</summary>
            public const String login_time_longer_day = "login_time_longer_day";

            ///<summary>总在线时长</summary>
            public const String login_time_longer_total = "login_time_longer_total";

            ///<summary>防沉迷到达时间</summary>
            public const String login_open_time = "login_open_time";

        }
        #endregion
    }

    /// <summary>玩家登陆日志表接口</summary>
    public partial interface Itg_user_login_log
    {
        #region 属性
        /// <summary>编号</summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>登陆IP</summary>
        String login_ip { get; set; }

        /// <summary>登陆状态</summary>
        Int32 login_state { get; set; }

        /// <summary>上线时间</summary>
        Int64 login_time { get; set; }

        /// <summary>下线时间</summary>
        Int64 logout_time { get; set; }

        /// <summary>每日登录次数</summary>
        Int32 login_count_day { get; set; }

        /// <summary>登陆总次数</summary>
        Int32 login_count_total { get; set; }

        /// <summary>每日在线时长</summary>
        Int64 login_time_longer_day { get; set; }

        /// <summary>总在线时长</summary>
        Int64 login_time_longer_total { get; set; }

        /// <summary>防沉迷到达时间</summary>
        Int64 login_open_time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}