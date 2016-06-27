using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Arena_Report</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_arena_report", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_arena_report : Iview_arena_report
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

        private Int64 _other_user_id;
        /// <summary></summary>
        [DisplayName("User_ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "other_user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 other_user_id
        {
            get { return _other_user_id; }
            set { if (OnPropertyChanging(__.other_user_id, value)) { _other_user_id = value; OnPropertyChanged(__.other_user_id); } }
        }

        private Int32 _type;
        /// <summary></summary>
        [DisplayName("Type")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "type", "", null, "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Boolean _isWin;
        /// <summary></summary>
        [DisplayName("IsWin")]
        [Description("")]
        [DataObjectField(false, false, false, 1)]
        [BindColumn(4, "isWin", "", null, "bit", 0, 0, false)]
        public virtual Boolean isWin
        {
            get { return _isWin; }
            set { if (OnPropertyChanging(__.isWin, value)) { _isWin = value; OnPropertyChanged(__.isWin); } }
        }

        private Byte[] _history;
        /// <summary></summary>
        [DisplayName("History")]
        [Description("")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(5, "history", "", null, "image", 0, 0, false)]
        public virtual Byte[] history
        {
            get { return _history; }
            set { if (OnPropertyChanging(__.history, value)) { _history = value; OnPropertyChanged(__.history); } }
        }

        private Int64 _time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private String _player_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "player_name", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private Int32 _player_sex;
        /// <summary></summary>
        [DisplayName("Sex")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "player_sex", "", null, "int", 10, 0, false)]
        public virtual Int32 player_sex
        {
            get { return _player_sex; }
            set { if (OnPropertyChanging(__.player_sex, value)) { _player_sex = value; OnPropertyChanged(__.player_sex); } }
        }

        private Int32 _player_vocation;
        /// <summary></summary>
        [DisplayName("Vocation")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "player_vocation", "", null, "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "role_level", "", null, "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
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
                    case __.other_user_id : return _other_user_id;
                    case __.type : return _type;
                    case __.isWin : return _isWin;
                    case __.history : return _history;
                    case __.time : return _time;
                    case __.user_id : return _user_id;
                    case __.player_name : return _player_name;
                    case __.player_sex : return _player_sex;
                    case __.player_vocation : return _player_vocation;
                    case __.role_level : return _role_level;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.other_user_id : _other_user_id = Convert.ToInt64(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.isWin : _isWin = Convert.ToBoolean(value); break;
                    case __.history : _history = (Byte[])value; break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.player_sex : _player_sex = Convert.ToInt32(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Arena_Report字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field other_user_id = FindByName(__.other_user_id);

            ///<summary></summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary></summary>
            public static readonly Field isWin = FindByName(__.isWin);

            ///<summary></summary>
            public static readonly Field history = FindByName(__.history);

            ///<summary></summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field player_sex = FindByName(__.player_sex);

            ///<summary></summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Arena_Report字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String other_user_id = "other_user_id";

            ///<summary></summary>
            public const String type = "type";

            ///<summary></summary>
            public const String isWin = "isWin";

            ///<summary></summary>
            public const String history = "history";

            ///<summary></summary>
            public const String time = "time";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String player_sex = "player_sex";

            ///<summary></summary>
            public const String player_vocation = "player_vocation";

            ///<summary></summary>
            public const String role_level = "role_level";

        }
        #endregion
    }

    /// <summary>Arena_Report接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_arena_report
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 other_user_id { get; set; }

        /// <summary></summary>
        Int32 type { get; set; }

        /// <summary></summary>
        Boolean isWin { get; set; }

        /// <summary></summary>
        Byte[] history { get; set; }

        /// <summary></summary>
        Int64 time { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 player_sex { get; set; }

        /// <summary></summary>
        Int32 player_vocation { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}