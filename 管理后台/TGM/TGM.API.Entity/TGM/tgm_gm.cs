using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>GM管理表</summary>
    [Serializable]
    [DataObject]
    [Description("GM管理表")]
    [BindTable("tgm_gm", Description = "GM管理表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_gm : Itgm_gm
    {
        #region 属性
        private Int32 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "主键id", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _pid;
        /// <summary>平台id</summary>
        [DisplayName("平台id")]
        [Description("平台id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "平台id", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private Int32 _sid;
        /// <summary>服务器id</summary>
        [DisplayName("服务器id")]
        [Description("服务器id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "sid", "服务器id", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private Int64 _limit_time;
        /// <summary>限制时间</summary>
        [DisplayName("限制时间")]
        [Description("限制时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "limit_time", "限制时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 limit_time
        {
            get { return _limit_time; }
            set { if (OnPropertyChanging(__.limit_time, value)) { _limit_time = value; OnPropertyChanged(__.limit_time); } }
        }

        private Int32 _state;
        /// <summary>账号状态 0：已解禁 1：冻结 2：封号</summary>
        [DisplayName("账号状态0：已解禁1：冻结2：封号")]
        [Description("账号状态 0：已解禁 1：冻结 2：封号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "state", "账号状态 0：已解禁 1：冻结 2：封号", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int64 _player_id;
        /// <summary>玩家id</summary>
        [DisplayName("玩家id")]
        [Description("玩家id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "player_id", "玩家id", "0", "bigint", 19, 0, false)]
        public virtual Int64 player_id
        {
            get { return _player_id; }
            set { if (OnPropertyChanging(__.player_id, value)) { _player_id = value; OnPropertyChanged(__.player_id); } }
        }

        private String _player_code;
        /// <summary></summary>
        [DisplayName("Code")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(7, "player_code", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String player_code
        {
            get { return _player_code; }
            set { if (OnPropertyChanging(__.player_code, value)) { _player_code = value; OnPropertyChanged(__.player_code); } }
        }

        private String _player_name;
        /// <summary>玩家名称</summary>
        [DisplayName("玩家名称")]
        [Description("玩家名称")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "player_name", "玩家名称", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private String _platform_name;
        /// <summary>平台名称</summary>
        [DisplayName("平台名称")]
        [Description("平台名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(9, "platform_name", "平台名称", null, "nvarchar(100)", 0, 0, true)]
        public virtual String platform_name
        {
            get { return _platform_name; }
            set { if (OnPropertyChanging(__.platform_name, value)) { _platform_name = value; OnPropertyChanged(__.platform_name); } }
        }

        private String _server_name;
        /// <summary>服务器名称</summary>
        [DisplayName("服务器名称")]
        [Description("服务器名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(10, "server_name", "服务器名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String server_name
        {
            get { return _server_name; }
            set { if (OnPropertyChanging(__.server_name, value)) { _server_name = value; OnPropertyChanged(__.server_name); } }
        }

        private String _describe;
        /// <summary>操作原因</summary>
        [DisplayName("操作原因")]
        [Description("操作原因")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(11, "describe", "操作原因", null, "varchar(100)", 0, 0, false)]
        public virtual String describe
        {
            get { return _describe; }
            set { if (OnPropertyChanging(__.describe, value)) { _describe = value; OnPropertyChanged(__.describe); } }
        }

        private Int64 _createtime;
        /// <summary>操作时间</summary>
        [DisplayName("操作时间")]
        [Description("操作时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(12, "createtime", "操作时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
        }

        private String _operate;
        /// <summary>操作员</summary>
        [DisplayName("操作员")]
        [Description("操作员")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "operate", "操作员", "0", "nvarchar(10)", 0, 0, true)]
        public virtual String operate
        {
            get { return _operate; }
            set { if (OnPropertyChanging(__.operate, value)) { _operate = value; OnPropertyChanged(__.operate); } }
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
                    case __.id: return _id;
                    case __.pid: return _pid;
                    case __.sid: return _sid;
                    case __.limit_time: return _limit_time;
                    case __.state: return _state;
                    case __.player_id: return _player_id;
                    case __.player_code: return _player_code;
                    case __.player_name: return _player_name;
                    case __.platform_name: return _platform_name;
                    case __.server_name: return _server_name;
                    case __.describe: return _describe;
                    case __.createtime: return _createtime;
                    case __.operate: return _operate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt32(value); break;
                    case __.pid: _pid = Convert.ToInt32(value); break;
                    case __.sid: _sid = Convert.ToInt32(value); break;
                    case __.limit_time: _limit_time = Convert.ToInt64(value); break;
                    case __.state: _state = Convert.ToInt32(value); break;
                    case __.player_id: _player_id = Convert.ToInt64(value); break;
                    case __.player_code: _player_code = Convert.ToString(value); break;
                    case __.player_name: _player_name = Convert.ToString(value); break;
                    case __.platform_name: _platform_name = Convert.ToString(value); break;
                    case __.server_name: _server_name = Convert.ToString(value); break;
                    case __.describe: _describe = Convert.ToString(value); break;
                    case __.createtime: _createtime = Convert.ToInt64(value); break;
                    case __.operate: _operate = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得GM管理表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>平台id</summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>服务器id</summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary>限制时间</summary>
            public static readonly Field limit_time = FindByName(__.limit_time);

            ///<summary>账号状态 0：已解禁 1：冻结 2：封号</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>玩家id</summary>
            public static readonly Field player_id = FindByName(__.player_id);

            ///<summary></summary>
            public static readonly Field player_code = FindByName(__.player_code);

            ///<summary>玩家名称</summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary>平台名称</summary>
            public static readonly Field platform_name = FindByName(__.platform_name);

            ///<summary>服务器名称</summary>
            public static readonly Field server_name = FindByName(__.server_name);

            ///<summary>操作原因</summary>
            public static readonly Field describe = FindByName(__.describe);

            ///<summary>操作时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            ///<summary>操作员</summary>
            public static readonly Field operate = FindByName(__.operate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得GM管理表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>平台id</summary>
            public const String pid = "pid";

            ///<summary>服务器id</summary>
            public const String sid = "sid";

            ///<summary>限制时间</summary>
            public const String limit_time = "limit_time";

            ///<summary>账号状态 0：已解禁 1：冻结 2：封号</summary>
            public const String state = "state";

            ///<summary>玩家id</summary>
            public const String player_id = "player_id";

            ///<summary></summary>
            public const String player_code = "player_code";

            ///<summary>玩家名称</summary>
            public const String player_name = "player_name";

            ///<summary>平台名称</summary>
            public const String platform_name = "platform_name";

            ///<summary>服务器名称</summary>
            public const String server_name = "server_name";

            ///<summary>操作原因</summary>
            public const String describe = "describe";

            ///<summary>操作时间</summary>
            public const String createtime = "createtime";

            ///<summary>操作员</summary>
            public const String operate = "operate";

        }
        #endregion
    }

    /// <summary>GM管理表接口</summary>
    public partial interface Itgm_gm
    {
        #region 属性
        /// <summary>主键id</summary>
        Int32 id { get; set; }

        /// <summary>平台id</summary>
        Int32 pid { get; set; }

        /// <summary>服务器id</summary>
        Int32 sid { get; set; }

        /// <summary>限制时间</summary>
        Int64 limit_time { get; set; }

        /// <summary>账号状态 0：已解禁 1：冻结 2：封号</summary>
        Int32 state { get; set; }

        /// <summary>玩家id</summary>
        Int64 player_id { get; set; }

        /// <summary></summary>
        String player_code { get; set; }

        /// <summary>玩家名称</summary>
        String player_name { get; set; }

        /// <summary>平台名称</summary>
        String platform_name { get; set; }

        /// <summary>服务器名称</summary>
        String server_name { get; set; }

        /// <summary>操作原因</summary>
        String describe { get; set; }

        /// <summary>操作时间</summary>
        Int64 createtime { get; set; }

        /// <summary>操作员</summary>
        String operate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}