using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>邮件信息表</summary>
    [Serializable]
    [DataObject]
    [Description("邮件信息表")]
    [BindIndex("PK_tg_messages", true, "id")]
    [BindTable("tg_messages", Description = "邮件信息表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_messages : Itg_messages
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

        private Int64 _receive_id;
        /// <summary>接收玩家id</summary>
        [DisplayName("接收玩家id")]
        [Description("接收玩家id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "receive_id", "接收玩家id", "0", "bigint", 19, 0, false)]
        public virtual Int64 receive_id
        {
            get { return _receive_id; }
            set { if (OnPropertyChanging(__.receive_id, value)) { _receive_id = value; OnPropertyChanged(__.receive_id); } }
        }

        private Int64 _send_id;
        /// <summary>发送玩家id</summary>
        [DisplayName("发送玩家id")]
        [Description("发送玩家id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "send_id", "发送玩家id", "0", "bigint", 19, 0, false)]
        public virtual Int64 send_id
        {
            get { return _send_id; }
            set { if (OnPropertyChanging(__.send_id, value)) { _send_id = value; OnPropertyChanged(__.send_id); } }
        }

        private Int32 _type;
        /// <summary>消息类型 0:玩家邮件 1:系统邮件</summary>
        [DisplayName("消息类型0:玩家邮件1:系统邮件")]
        [Description("消息类型 0:玩家邮件 1:系统邮件")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "type", "消息类型 0:玩家邮件 1:系统邮件", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private String _title;
        /// <summary>标题</summary>
        [DisplayName("标题")]
        [Description("标题")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "title", "标题", null, "nvarchar(10)", 0, 0, true)]
        public virtual String title
        {
            get { return _title; }
            set { if (OnPropertyChanging(__.title, value)) { _title = value; OnPropertyChanged(__.title); } }
        }

        private String _contents;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(6, "contents", "内容", null, "nvarchar(200)", 0, 0, true)]
        public virtual String contents
        {
            get { return _contents; }
            set { if (OnPropertyChanging(__.contents, value)) { _contents = value; OnPropertyChanged(__.contents); } }
        }

        private Int32 _isread;
        /// <summary>是否已读</summary>
        [DisplayName("是否已读")]
        [Description("是否已读")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "isread", "是否已读", "0", "int", 10, 0, false)]
        public virtual Int32 isread
        {
            get { return _isread; }
            set { if (OnPropertyChanging(__.isread, value)) { _isread = value; OnPropertyChanged(__.isread); } }
        }

        private Int32 _isattachment;
        /// <summary>是否有附件</summary>
        [DisplayName("是否有附件")]
        [Description("是否有附件")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "isattachment", "是否有附件", "0", "int", 10, 0, false)]
        public virtual Int32 isattachment
        {
            get { return _isattachment; }
            set { if (OnPropertyChanging(__.isattachment, value)) { _isattachment = value; OnPropertyChanged(__.isattachment); } }
        }

        private String _attachment;
        /// <summary>附件</summary>
        [DisplayName("附件")]
        [Description("附件")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(9, "attachment", "附件", null, "nvarchar(200)", 0, 0, true)]
        public virtual String attachment
        {
            get { return _attachment; }
            set { if (OnPropertyChanging(__.attachment, value)) { _attachment = value; OnPropertyChanged(__.attachment); } }
        }

        private Int64 _create_time;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "create_time", "创建时间", null, "bigint", 19, 0, false)]
        public virtual Int64 create_time
        {
            get { return _create_time; }
            set { if (OnPropertyChanging(__.create_time, value)) { _create_time = value; OnPropertyChanged(__.create_time); } }
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
                    case __.receive_id : return _receive_id;
                    case __.send_id : return _send_id;
                    case __.type : return _type;
                    case __.title : return _title;
                    case __.contents : return _contents;
                    case __.isread : return _isread;
                    case __.isattachment : return _isattachment;
                    case __.attachment : return _attachment;
                    case __.create_time : return _create_time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.receive_id : _receive_id = Convert.ToInt64(value); break;
                    case __.send_id : _send_id = Convert.ToInt64(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.title : _title = Convert.ToString(value); break;
                    case __.contents : _contents = Convert.ToString(value); break;
                    case __.isread : _isread = Convert.ToInt32(value); break;
                    case __.isattachment : _isattachment = Convert.ToInt32(value); break;
                    case __.attachment : _attachment = Convert.ToString(value); break;
                    case __.create_time : _create_time = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得邮件信息表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>接收玩家id</summary>
            public static readonly Field receive_id = FindByName(__.receive_id);

            ///<summary>发送玩家id</summary>
            public static readonly Field send_id = FindByName(__.send_id);

            ///<summary>消息类型 0:玩家邮件 1:系统邮件</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>标题</summary>
            public static readonly Field title = FindByName(__.title);

            ///<summary>内容</summary>
            public static readonly Field contents = FindByName(__.contents);

            ///<summary>是否已读</summary>
            public static readonly Field isread = FindByName(__.isread);

            ///<summary>是否有附件</summary>
            public static readonly Field isattachment = FindByName(__.isattachment);

            ///<summary>附件</summary>
            public static readonly Field attachment = FindByName(__.attachment);

            ///<summary>创建时间</summary>
            public static readonly Field create_time = FindByName(__.create_time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得邮件信息表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>接收玩家id</summary>
            public const String receive_id = "receive_id";

            ///<summary>发送玩家id</summary>
            public const String send_id = "send_id";

            ///<summary>消息类型 0:玩家邮件 1:系统邮件</summary>
            public const String type = "type";

            ///<summary>标题</summary>
            public const String title = "title";

            ///<summary>内容</summary>
            public const String contents = "contents";

            ///<summary>是否已读</summary>
            public const String isread = "isread";

            ///<summary>是否有附件</summary>
            public const String isattachment = "isattachment";

            ///<summary>附件</summary>
            public const String attachment = "attachment";

            ///<summary>创建时间</summary>
            public const String create_time = "create_time";

        }
        #endregion
    }

    /// <summary>邮件信息表接口</summary>
    public partial interface Itg_messages
    {
        #region 属性
        /// <summary>编号</summary>
        Int64 id { get; set; }

        /// <summary>接收玩家id</summary>
        Int64 receive_id { get; set; }

        /// <summary>发送玩家id</summary>
        Int64 send_id { get; set; }

        /// <summary>消息类型 0:玩家邮件 1:系统邮件</summary>
        Int32 type { get; set; }

        /// <summary>标题</summary>
        String title { get; set; }

        /// <summary>内容</summary>
        String contents { get; set; }

        /// <summary>是否已读</summary>
        Int32 isread { get; set; }

        /// <summary>是否有附件</summary>
        Int32 isattachment { get; set; }

        /// <summary>附件</summary>
        String attachment { get; set; }

        /// <summary>创建时间</summary>
        Int64 create_time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}