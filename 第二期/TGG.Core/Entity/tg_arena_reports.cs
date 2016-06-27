using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Arena_Reports</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindIndex("PK_tg_arena_reports", true, "id")]
    [BindTable("tg_arena_reports", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_arena_reports : Itg_arena_reports
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键Id</summary>
        [DisplayName("主键Id")]
        [Description("主键Id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键Id", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _other_user_id;
        /// <summary>对手Id</summary>
        [DisplayName("对手Id")]
        [Description("对手Id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "other_user_id", "对手Id", "0", "bigint", 19, 0, false)]
        public virtual Int64 other_user_id
        {
            get { return _other_user_id; }
            set { if (OnPropertyChanging(__.other_user_id, value)) { _other_user_id = value; OnPropertyChanged(__.other_user_id); } }
        }

        private Int32 _type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "type", "类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Boolean _isWin;
        /// <summary>是否胜利</summary>
        [DisplayName("是否胜利")]
        [Description("是否胜利")]
        [DataObjectField(false, false, false, 1)]
        [BindColumn(4, "isWin", "是否胜利", "1", "bit", 0, 0, false)]
        public virtual Boolean isWin
        {
            get { return _isWin; }
            set { if (OnPropertyChanging(__.isWin, value)) { _isWin = value; OnPropertyChanged(__.isWin); } }
        }

        private Byte[] _history;
        /// <summary>历史战斗过程</summary>
        [DisplayName("历史战斗过程")]
        [Description("历史战斗过程")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(5, "history", "历史战斗过程", null, "image", 0, 0, false)]
        public virtual Byte[] history
        {
            get { return _history; }
            set { if (OnPropertyChanging(__.history, value)) { _history = value; OnPropertyChanged(__.history); } }
        }

        private Int64 _time;
        /// <summary>战斗时间</summary>
        [DisplayName("战斗时间")]
        [Description("战斗时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "time", "战斗时间", null, "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int64 _user_id;
        /// <summary>用户Id</summary>
        [DisplayName("用户Id")]
        [Description("用户Id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "user_id", "用户Id", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
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
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Arena_Reports字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键Id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>对手Id</summary>
            public static readonly Field other_user_id = FindByName(__.other_user_id);

            ///<summary>类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>是否胜利</summary>
            public static readonly Field isWin = FindByName(__.isWin);

            ///<summary>历史战斗过程</summary>
            public static readonly Field history = FindByName(__.history);

            ///<summary>战斗时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>用户Id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Arena_Reports字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键Id</summary>
            public const String id = "id";

            ///<summary>对手Id</summary>
            public const String other_user_id = "other_user_id";

            ///<summary>类型</summary>
            public const String type = "type";

            ///<summary>是否胜利</summary>
            public const String isWin = "isWin";

            ///<summary>历史战斗过程</summary>
            public const String history = "history";

            ///<summary>战斗时间</summary>
            public const String time = "time";

            ///<summary>用户Id</summary>
            public const String user_id = "user_id";

        }
        #endregion
    }

    /// <summary>Arena_Reports接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_arena_reports
    {
        #region 属性
        /// <summary>主键Id</summary>
        Int64 id { get; set; }

        /// <summary>对手Id</summary>
        Int64 other_user_id { get; set; }

        /// <summary>类型</summary>
        Int32 type { get; set; }

        /// <summary>是否胜利</summary>
        Boolean isWin { get; set; }

        /// <summary>历史战斗过程</summary>
        Byte[] history { get; set; }

        /// <summary>战斗时间</summary>
        Int64 time { get; set; }

        /// <summary>用户Id</summary>
        Int64 user_id { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}