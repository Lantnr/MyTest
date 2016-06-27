using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>监狱留言表</summary>
    [Serializable]
    [DataObject]
    [Description("监狱留言表")]
    [BindIndex("PK_tg_prison_messages", true, "id")]
    [BindTable("tg_prison_messages", Description = "监狱留言表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_prison_messages : Itg_prison_messages
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键id", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "用户id", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private String _message;
        /// <summary>留言信息</summary>
        [DisplayName("留言信息")]
        [Description("留言信息")]
        [DataObjectField(false, false, false, 80)]
        [BindColumn(3, "message", "留言信息", null, "nvarchar(80)", 0, 0, true)]
        public virtual String message
        {
            get { return _message; }
            set { if (OnPropertyChanging(__.message, value)) { _message = value; OnPropertyChanged(__.message); } }
        }

        private Int64 _writetime;
        /// <summary>留言时间</summary>
        [DisplayName("留言时间")]
        [Description("留言时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "writetime", "留言时间", null, "bigint", 19, 0, false)]
        public virtual Int64 writetime
        {
            get { return _writetime; }
            set { if (OnPropertyChanging(__.writetime, value)) { _writetime = value; OnPropertyChanged(__.writetime); } }
        }

        private String _play_name;
        /// <summary>玩家角色名</summary>
        [DisplayName("玩家角色名")]
        [Description("玩家角色名")]
        [DataObjectField(false, false, false, 16)]
        [BindColumn(5, "play_name", "玩家角色名", "0", "nvarchar(16)", 0, 0, true)]
        public virtual String play_name
        {
            get { return _play_name; }
            set { if (OnPropertyChanging(__.play_name, value)) { _play_name = value; OnPropertyChanged(__.play_name); } }
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
                    case __.message : return _message;
                    case __.writetime : return _writetime;
                    case __.play_name : return _play_name;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.message : _message = Convert.ToString(value); break;
                    case __.writetime : _writetime = Convert.ToInt64(value); break;
                    case __.play_name : _play_name = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得监狱留言表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>留言信息</summary>
            public static readonly Field message = FindByName(__.message);

            ///<summary>留言时间</summary>
            public static readonly Field writetime = FindByName(__.writetime);

            ///<summary>玩家角色名</summary>
            public static readonly Field play_name = FindByName(__.play_name);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得监狱留言表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>用户id</summary>
            public const String user_id = "user_id";

            ///<summary>留言信息</summary>
            public const String message = "message";

            ///<summary>留言时间</summary>
            public const String writetime = "writetime";

            ///<summary>玩家角色名</summary>
            public const String play_name = "play_name";

        }
        #endregion
    }

    /// <summary>监狱留言表接口</summary>
    public partial interface Itg_prison_messages
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>用户id</summary>
        Int64 user_id { get; set; }

        /// <summary>留言信息</summary>
        String message { get; set; }

        /// <summary>留言时间</summary>
        Int64 writetime { get; set; }

        /// <summary>玩家角色名</summary>
        String play_name { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}