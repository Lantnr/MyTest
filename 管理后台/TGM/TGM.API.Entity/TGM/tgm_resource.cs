using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>资源管理表</summary>
    [Serializable]
    [DataObject]
    [Description("资源管理表")]
    [BindTable("tgm_resource", Description = "资源管理表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_resource : Itgm_resource
    {
        #region 属性
        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private String _name;
        /// <summary>礼包名称</summary>
        [DisplayName("礼包名称")]
        [Description("礼包名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "name", "礼包名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
        }

        private Int64 _pid;
        /// <summary>平台id</summary>
        [DisplayName("平台id")]
        [Description("平台id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "pid", "平台id", "0", "bigint", 19, 0, false)]
        public virtual Int64 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private Int64 _sid;
        /// <summary>服务器id</summary>
        [DisplayName("服务器id")]
        [Description("服务器id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "sid", "服务器id", "0", "bigint", 19, 0, false)]
        public virtual Int64 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private String _player_name;
        /// <summary>玩家名字</summary>
        [DisplayName("玩家名字")]
        [Description("玩家名字")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "player_name", "玩家名字", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private String _user_code;
        /// <summary>玩家账号</summary>
        [DisplayName("玩家账号")]
        [Description("玩家账号")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(6, "user_code", "玩家账号", null, "nvarchar(200)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
        }

        private Int64 _time;
        /// <summary>申请时间</summary>
        [DisplayName("申请时间")]
        [Description("申请时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "time", "申请时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _type;
        /// <summary>申请类型</summary>
        [DisplayName("申请类型")]
        [Description("申请类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "type", "申请类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _state;
        /// <summary>申请状态</summary>
        [DisplayName("申请状态")]
        [Description("申请状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "state", "申请状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private String _content;
        /// <summary>申请原因</summary>
        [DisplayName("申请原因")]
        [Description("申请原因")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(10, "content", "申请原因", null, "varchar(500)", 0, 0, false)]
        public virtual String content
        {
            get { return _content; }
            set { if (OnPropertyChanging(__.content, value)) { _content = value; OnPropertyChanged(__.content); } }
        }

        private String _operation;
        /// <summary>操作人名字</summary>
        [DisplayName("操作人名字")]
        [Description("操作人名字")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "operation", "操作人名字", null, "nvarchar(10)", 0, 0, true)]
        public virtual String operation
        {
            get { return _operation; }
            set { if (OnPropertyChanging(__.operation, value)) { _operation = value; OnPropertyChanged(__.operation); } }
        }

        private String _attachment;
        /// <summary>附件</summary>
        [DisplayName("附件")]
        [Description("附件")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(12, "attachment", "附件", "0", "nvarchar(200)", 0, 0, true)]
        public virtual String attachment
        {
            get { return _attachment; }
            set { if (OnPropertyChanging(__.attachment, value)) { _attachment = value; OnPropertyChanged(__.attachment); } }
        }

        private String _message;
        /// <summary>邮件内容</summary>
        [DisplayName("邮件内容")]
        [Description("邮件内容")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(13, "message", "邮件内容", null, "varchar(500)", 0, 0, false)]
        public virtual String message
        {
            get { return _message; }
            set { if (OnPropertyChanging(__.message, value)) { _message = value; OnPropertyChanged(__.message); } }
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
                    case __.name : return _name;
                    case __.pid : return _pid;
                    case __.sid : return _sid;
                    case __.player_name : return _player_name;
                    case __.user_code : return _user_code;
                    case __.time : return _time;
                    case __.type : return _type;
                    case __.state : return _state;
                    case __.content : return _content;
                    case __.operation : return _operation;
                    case __.attachment : return _attachment;
                    case __.message : return _message;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.name : _name = Convert.ToString(value); break;
                    case __.pid : _pid = Convert.ToInt64(value); break;
                    case __.sid : _sid = Convert.ToInt64(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.user_code : _user_code = Convert.ToString(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.content : _content = Convert.ToString(value); break;
                    case __.operation : _operation = Convert.ToString(value); break;
                    case __.attachment : _attachment = Convert.ToString(value); break;
                    case __.message : _message = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得资源管理表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>礼包名称</summary>
            public static readonly Field name = FindByName(__.name);

            ///<summary>平台id</summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>服务器id</summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary>玩家名字</summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary>玩家账号</summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary>申请时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>申请类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>申请状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>申请原因</summary>
            public static readonly Field content = FindByName(__.content);

            ///<summary>操作人名字</summary>
            public static readonly Field operation = FindByName(__.operation);

            ///<summary>附件</summary>
            public static readonly Field attachment = FindByName(__.attachment);

            ///<summary>邮件内容</summary>
            public static readonly Field message = FindByName(__.message);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得资源管理表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>礼包名称</summary>
            public const String name = "name";

            ///<summary>平台id</summary>
            public const String pid = "pid";

            ///<summary>服务器id</summary>
            public const String sid = "sid";

            ///<summary>玩家名字</summary>
            public const String player_name = "player_name";

            ///<summary>玩家账号</summary>
            public const String user_code = "user_code";

            ///<summary>申请时间</summary>
            public const String time = "time";

            ///<summary>申请类型</summary>
            public const String type = "type";

            ///<summary>申请状态</summary>
            public const String state = "state";

            ///<summary>申请原因</summary>
            public const String content = "content";

            ///<summary>操作人名字</summary>
            public const String operation = "operation";

            ///<summary>附件</summary>
            public const String attachment = "attachment";

            ///<summary>邮件内容</summary>
            public const String message = "message";

        }
        #endregion
    }

    /// <summary>资源管理表接口</summary>
    public partial interface Itgm_resource
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>礼包名称</summary>
        String name { get; set; }

        /// <summary>平台id</summary>
        Int64 pid { get; set; }

        /// <summary>服务器id</summary>
        Int64 sid { get; set; }

        /// <summary>玩家名字</summary>
        String player_name { get; set; }

        /// <summary>玩家账号</summary>
        String user_code { get; set; }

        /// <summary>申请时间</summary>
        Int64 time { get; set; }

        /// <summary>申请类型</summary>
        Int32 type { get; set; }

        /// <summary>申请状态</summary>
        Int32 state { get; set; }

        /// <summary>申请原因</summary>
        String content { get; set; }

        /// <summary>操作人名字</summary>
        String operation { get; set; }

        /// <summary>附件</summary>
        String attachment { get; set; }

        /// <summary>邮件内容</summary>
        String message { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}