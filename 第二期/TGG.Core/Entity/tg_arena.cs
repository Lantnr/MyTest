using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Arena</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindIndex("PK_tg_arena", true, "id")]
    [BindTable("tg_arena", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_arena : Itg_arena
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

        private Int32 _ranking;
        /// <summary>排名</summary>
        [DisplayName("排名")]
        [Description("排名")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "ranking", "排名", "0", "int", 10, 0, false)]
        public virtual Int32 ranking
        {
            get { return _ranking; }
            set { if (OnPropertyChanging(__.ranking, value)) { _ranking = value; OnPropertyChanged(__.ranking); } }
        }

        private Int32 _totalCount;
        /// <summary>挑战总次数</summary>
        [DisplayName("挑战总次数")]
        [Description("挑战总次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "totalCount", "挑战总次数", "0", "int", 10, 0, false)]
        public virtual Int32 totalCount
        {
            get { return _totalCount; }
            set { if (OnPropertyChanging(__.totalCount, value)) { _totalCount = value; OnPropertyChanged(__.totalCount); } }
        }

        private Int32 _count;
        /// <summary>剩余次数</summary>
        [DisplayName("剩余次数")]
        [Description("剩余次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "count", "剩余次数", "0", "int", 10, 0, false)]
        public virtual Int32 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
        }

        private Int64 _time;
        /// <summary>挑战剩余时间</summary>
        [DisplayName("挑战剩余时间")]
        [Description("挑战剩余时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "time", "挑战剩余时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _winCount;
        /// <summary>连胜次数</summary>
        [DisplayName("连胜次数")]
        [Description("连胜次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "winCount", "连胜次数", "0", "int", 10, 0, false)]
        public virtual Int32 winCount
        {
            get { return _winCount; }
            set { if (OnPropertyChanging(__.winCount, value)) { _winCount = value; OnPropertyChanged(__.winCount); } }
        }

        private Int32 _remove_cooling;
        /// <summary>清除冷却次数</summary>
        [DisplayName("清除冷却次数")]
        [Description("清除冷却次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "remove_cooling", "清除冷却次数", "0", "int", 10, 0, false)]
        public virtual Int32 remove_cooling
        {
            get { return _remove_cooling; }
            set { if (OnPropertyChanging(__.remove_cooling, value)) { _remove_cooling = value; OnPropertyChanged(__.remove_cooling); } }
        }

        private Int64 _user_id;
        /// <summary>用户Id</summary>
        [DisplayName("用户Id")]
        [Description("用户Id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "user_id", "用户Id", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _buy_count;
        /// <summary>购买次数</summary>
        [DisplayName("购买次数")]
        [Description("购买次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "buy_count", "购买次数", "0", "int", 10, 0, false)]
        public virtual Int32 buy_count
        {
            get { return _buy_count; }
            set { if (OnPropertyChanging(__.buy_count, value)) { _buy_count = value; OnPropertyChanged(__.buy_count); } }
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
                    case __.ranking : return _ranking;
                    case __.totalCount : return _totalCount;
                    case __.count : return _count;
                    case __.time : return _time;
                    case __.winCount : return _winCount;
                    case __.remove_cooling : return _remove_cooling;
                    case __.user_id : return _user_id;
                    case __.buy_count : return _buy_count;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.ranking : _ranking = Convert.ToInt32(value); break;
                    case __.totalCount : _totalCount = Convert.ToInt32(value); break;
                    case __.count : _count = Convert.ToInt32(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.winCount : _winCount = Convert.ToInt32(value); break;
                    case __.remove_cooling : _remove_cooling = Convert.ToInt32(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.buy_count : _buy_count = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Arena字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>排名</summary>
            public static readonly Field ranking = FindByName(__.ranking);

            ///<summary>挑战总次数</summary>
            public static readonly Field totalCount = FindByName(__.totalCount);

            ///<summary>剩余次数</summary>
            public static readonly Field count = FindByName(__.count);

            ///<summary>挑战剩余时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>连胜次数</summary>
            public static readonly Field winCount = FindByName(__.winCount);

            ///<summary>清除冷却次数</summary>
            public static readonly Field remove_cooling = FindByName(__.remove_cooling);

            ///<summary>用户Id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>购买次数</summary>
            public static readonly Field buy_count = FindByName(__.buy_count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Arena字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>排名</summary>
            public const String ranking = "ranking";

            ///<summary>挑战总次数</summary>
            public const String totalCount = "totalCount";

            ///<summary>剩余次数</summary>
            public const String count = "count";

            ///<summary>挑战剩余时间</summary>
            public const String time = "time";

            ///<summary>连胜次数</summary>
            public const String winCount = "winCount";

            ///<summary>清除冷却次数</summary>
            public const String remove_cooling = "remove_cooling";

            ///<summary>用户Id</summary>
            public const String user_id = "user_id";

            ///<summary>购买次数</summary>
            public const String buy_count = "buy_count";

        }
        #endregion
    }

    /// <summary>Arena接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_arena
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>排名</summary>
        Int32 ranking { get; set; }

        /// <summary>挑战总次数</summary>
        Int32 totalCount { get; set; }

        /// <summary>剩余次数</summary>
        Int32 count { get; set; }

        /// <summary>挑战剩余时间</summary>
        Int64 time { get; set; }

        /// <summary>连胜次数</summary>
        Int32 winCount { get; set; }

        /// <summary>清除冷却次数</summary>
        Int32 remove_cooling { get; set; }

        /// <summary>用户Id</summary>
        Int64 user_id { get; set; }

        /// <summary>购买次数</summary>
        Int32 buy_count { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}