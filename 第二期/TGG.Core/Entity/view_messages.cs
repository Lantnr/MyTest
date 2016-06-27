using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Messages</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_messages", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_messages : Iview_messages
    {
        #region 属性
        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(1, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _receive_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "receive_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 receive_id
        {
            get { return _receive_id; }
            set { if (OnPropertyChanging(__.receive_id, value)) { _receive_id = value; OnPropertyChanged(__.receive_id); } }
        }

        private Int64 _send_id;
        /// <summary></summary>
        [DisplayName("ID2")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "send_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 send_id
        {
            get { return _send_id; }
            set { if (OnPropertyChanging(__.send_id, value)) { _send_id = value; OnPropertyChanged(__.send_id); } }
        }

        private Int32 _type;
        /// <summary></summary>
        [DisplayName("Type")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "type", "", null, "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private String _title;
        /// <summary></summary>
        [DisplayName("Title")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "title", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String title
        {
            get { return _title; }
            set { if (OnPropertyChanging(__.title, value)) { _title = value; OnPropertyChanged(__.title); } }
        }

        private String _contents;
        /// <summary></summary>
        [DisplayName("Contents")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(6, "contents", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String contents
        {
            get { return _contents; }
            set { if (OnPropertyChanging(__.contents, value)) { _contents = value; OnPropertyChanged(__.contents); } }
        }

        private Int32 _isread;
        /// <summary></summary>
        [DisplayName("IsRead")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "isread", "", null, "int", 10, 0, false)]
        public virtual Int32 isread
        {
            get { return _isread; }
            set { if (OnPropertyChanging(__.isread, value)) { _isread = value; OnPropertyChanged(__.isread); } }
        }

        private Int32 _isattachment;
        /// <summary></summary>
        [DisplayName("IsAttachment")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "isattachment", "", null, "int", 10, 0, false)]
        public virtual Int32 isattachment
        {
            get { return _isattachment; }
            set { if (OnPropertyChanging(__.isattachment, value)) { _isattachment = value; OnPropertyChanged(__.isattachment); } }
        }

        private String _attachment;
        /// <summary></summary>
        [DisplayName("Attachment")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(9, "attachment", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String attachment
        {
            get { return _attachment; }
            set { if (OnPropertyChanging(__.attachment, value)) { _attachment = value; OnPropertyChanged(__.attachment); } }
        }

        private Int64 _create_time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "create_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 create_time
        {
            get { return _create_time; }
            set { if (OnPropertyChanging(__.create_time, value)) { _create_time = value; OnPropertyChanged(__.create_time); } }
        }

        private String _player_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "player_name", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
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
                    case __.player_name : return _player_name;
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
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Messages字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field receive_id = FindByName(__.receive_id);

            ///<summary></summary>
            public static readonly Field send_id = FindByName(__.send_id);

            ///<summary></summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary></summary>
            public static readonly Field title = FindByName(__.title);

            ///<summary></summary>
            public static readonly Field contents = FindByName(__.contents);

            ///<summary></summary>
            public static readonly Field isread = FindByName(__.isread);

            ///<summary></summary>
            public static readonly Field isattachment = FindByName(__.isattachment);

            ///<summary></summary>
            public static readonly Field attachment = FindByName(__.attachment);

            ///<summary></summary>
            public static readonly Field create_time = FindByName(__.create_time);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Messages字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String receive_id = "receive_id";

            ///<summary></summary>
            public const String send_id = "send_id";

            ///<summary></summary>
            public const String type = "type";

            ///<summary></summary>
            public const String title = "title";

            ///<summary></summary>
            public const String contents = "contents";

            ///<summary></summary>
            public const String isread = "isread";

            ///<summary></summary>
            public const String isattachment = "isattachment";

            ///<summary></summary>
            public const String attachment = "attachment";

            ///<summary></summary>
            public const String create_time = "create_time";

            ///<summary></summary>
            public const String player_name = "player_name";

        }
        #endregion
    }

    /// <summary>Messages接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_messages
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 receive_id { get; set; }

        /// <summary></summary>
        Int64 send_id { get; set; }

        /// <summary></summary>
        Int32 type { get; set; }

        /// <summary></summary>
        String title { get; set; }

        /// <summary></summary>
        String contents { get; set; }

        /// <summary></summary>
        Int32 isread { get; set; }

        /// <summary></summary>
        Int32 isattachment { get; set; }

        /// <summary></summary>
        String attachment { get; set; }

        /// <summary></summary>
        Int64 create_time { get; set; }

        /// <summary></summary>
        String player_name { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}