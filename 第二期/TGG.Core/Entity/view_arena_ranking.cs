using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Arena_Ranking</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_arena_ranking", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_arena_ranking : Iview_arena_ranking
    {
        #region 属性
        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(1, "player_camp", "", null, "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
        }

        private Int32 _player_influence;
        /// <summary></summary>
        [DisplayName("Influence")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "player_influence", "", null, "int", 10, 0, false)]
        public virtual Int32 player_influence
        {
            get { return _player_influence; }
            set { if (OnPropertyChanging(__.player_influence, value)) { _player_influence = value; OnPropertyChanged(__.player_influence); } }
        }

        private Int32 _player_position;
        /// <summary></summary>
        [DisplayName("Position")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "player_position", "", null, "int", 10, 0, false)]
        public virtual Int32 player_position
        {
            get { return _player_position; }
            set { if (OnPropertyChanging(__.player_position, value)) { _player_position = value; OnPropertyChanged(__.player_position); } }
        }

        private Int32 _player_vocation;
        /// <summary></summary>
        [DisplayName("Vocation")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "player_vocation", "", null, "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _player_sex;
        /// <summary></summary>
        [DisplayName("Sex")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "player_sex", "", null, "int", 10, 0, false)]
        public virtual Int32 player_sex
        {
            get { return _player_sex; }
            set { if (OnPropertyChanging(__.player_sex, value)) { _player_sex = value; OnPropertyChanged(__.player_sex); } }
        }

        private Int32 _role_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "role_id", "", null, "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private String _player_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "player_name", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private Int32 _fame;
        /// <summary></summary>
        [DisplayName("Fame")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "fame", "", null, "int", 10, 0, false)]
        public virtual Int32 fame
        {
            get { return _fame; }
            set { if (OnPropertyChanging(__.fame, value)) { _fame = value; OnPropertyChanged(__.fame); } }
        }

        private Int32 _power;
        /// <summary></summary>
        [DisplayName("Power")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "power", "", null, "int", 10, 0, false)]
        public virtual Int32 power
        {
            get { return _power; }
            set { if (OnPropertyChanging(__.power, value)) { _power = value; OnPropertyChanged(__.power); } }
        }

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "role_level", "", null, "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
        }

        private Int32 _role_state;
        /// <summary></summary>
        [DisplayName("State")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "role_state", "", null, "int", 10, 0, false)]
        public virtual Int32 role_state
        {
            get { return _role_state; }
            set { if (OnPropertyChanging(__.role_state, value)) { _role_state = value; OnPropertyChanged(__.role_state); } }
        }

        private Int32 _role_exp;
        /// <summary></summary>
        [DisplayName("Exp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "role_exp", "", null, "int", 10, 0, false)]
        public virtual Int32 role_exp
        {
            get { return _role_exp; }
            set { if (OnPropertyChanging(__.role_exp, value)) { _role_exp = value; OnPropertyChanged(__.role_exp); } }
        }

        private Int32 _role_honor;
        /// <summary></summary>
        [DisplayName("Honor")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "role_honor", "", null, "int", 10, 0, false)]
        public virtual Int32 role_honor
        {
            get { return _role_honor; }
            set { if (OnPropertyChanging(__.role_honor, value)) { _role_honor = value; OnPropertyChanged(__.role_honor); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(15, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _ranking;
        /// <summary></summary>
        [DisplayName("Ranking")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "ranking", "", null, "int", 10, 0, false)]
        public virtual Int32 ranking
        {
            get { return _ranking; }
            set { if (OnPropertyChanging(__.ranking, value)) { _ranking = value; OnPropertyChanged(__.ranking); } }
        }

        private Int32 _totalCount;
        /// <summary></summary>
        [DisplayName("TotalCount")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "totalCount", "", null, "int", 10, 0, false)]
        public virtual Int32 totalCount
        {
            get { return _totalCount; }
            set { if (OnPropertyChanging(__.totalCount, value)) { _totalCount = value; OnPropertyChanged(__.totalCount); } }
        }

        private Int32 _count;
        /// <summary></summary>
        [DisplayName("Count")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "count", "", null, "int", 10, 0, false)]
        public virtual Int32 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
        }

        private Int64 _time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(19, "time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _winCount;
        /// <summary></summary>
        [DisplayName("WinCount")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(20, "winCount", "", null, "int", 10, 0, false)]
        public virtual Int32 winCount
        {
            get { return _winCount; }
            set { if (OnPropertyChanging(__.winCount, value)) { _winCount = value; OnPropertyChanged(__.winCount); } }
        }

        private Int32 _remove_cooling;
        /// <summary></summary>
        [DisplayName("Cooling")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(21, "remove_cooling", "", null, "int", 10, 0, false)]
        public virtual Int32 remove_cooling
        {
            get { return _remove_cooling; }
            set { if (OnPropertyChanging(__.remove_cooling, value)) { _remove_cooling = value; OnPropertyChanged(__.remove_cooling); } }
        }

        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID2")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(22, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _buy_count;
        /// <summary></summary>
        [DisplayName("Count1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(23, "buy_count", "", null, "int", 10, 0, false)]
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
                    case __.player_camp : return _player_camp;
                    case __.player_influence : return _player_influence;
                    case __.player_position : return _player_position;
                    case __.player_vocation : return _player_vocation;
                    case __.player_sex : return _player_sex;
                    case __.role_id : return _role_id;
                    case __.player_name : return _player_name;
                    case __.fame : return _fame;
                    case __.power : return _power;
                    case __.role_level : return _role_level;
                    case __.role_state : return _role_state;
                    case __.role_exp : return _role_exp;
                    case __.role_honor : return _role_honor;
                    case __.role_identity : return _role_identity;
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
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.player_influence : _player_influence = Convert.ToInt32(value); break;
                    case __.player_position : _player_position = Convert.ToInt32(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.player_sex : _player_sex = Convert.ToInt32(value); break;
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.fame : _fame = Convert.ToInt32(value); break;
                    case __.power : _power = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.role_state : _role_state = Convert.ToInt32(value); break;
                    case __.role_exp : _role_exp = Convert.ToInt32(value); break;
                    case __.role_honor : _role_honor = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
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
        /// <summary>取得Arena_Ranking字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary></summary>
            public static readonly Field player_influence = FindByName(__.player_influence);

            ///<summary></summary>
            public static readonly Field player_position = FindByName(__.player_position);

            ///<summary></summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary></summary>
            public static readonly Field player_sex = FindByName(__.player_sex);

            ///<summary></summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field fame = FindByName(__.fame);

            ///<summary></summary>
            public static readonly Field power = FindByName(__.power);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field role_state = FindByName(__.role_state);

            ///<summary></summary>
            public static readonly Field role_exp = FindByName(__.role_exp);

            ///<summary></summary>
            public static readonly Field role_honor = FindByName(__.role_honor);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field ranking = FindByName(__.ranking);

            ///<summary></summary>
            public static readonly Field totalCount = FindByName(__.totalCount);

            ///<summary></summary>
            public static readonly Field count = FindByName(__.count);

            ///<summary></summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary></summary>
            public static readonly Field winCount = FindByName(__.winCount);

            ///<summary></summary>
            public static readonly Field remove_cooling = FindByName(__.remove_cooling);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field buy_count = FindByName(__.buy_count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Arena_Ranking字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String player_camp = "player_camp";

            ///<summary></summary>
            public const String player_influence = "player_influence";

            ///<summary></summary>
            public const String player_position = "player_position";

            ///<summary></summary>
            public const String player_vocation = "player_vocation";

            ///<summary></summary>
            public const String player_sex = "player_sex";

            ///<summary></summary>
            public const String role_id = "role_id";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String fame = "fame";

            ///<summary></summary>
            public const String power = "power";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String role_state = "role_state";

            ///<summary></summary>
            public const String role_exp = "role_exp";

            ///<summary></summary>
            public const String role_honor = "role_honor";

            ///<summary></summary>
            public const String role_identity = "role_identity";

            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String ranking = "ranking";

            ///<summary></summary>
            public const String totalCount = "totalCount";

            ///<summary></summary>
            public const String count = "count";

            ///<summary></summary>
            public const String time = "time";

            ///<summary></summary>
            public const String winCount = "winCount";

            ///<summary></summary>
            public const String remove_cooling = "remove_cooling";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String buy_count = "buy_count";

        }
        #endregion
    }

    /// <summary>Arena_Ranking接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_arena_ranking
    {
        #region 属性
        /// <summary></summary>
        Int32 player_camp { get; set; }

        /// <summary></summary>
        Int32 player_influence { get; set; }

        /// <summary></summary>
        Int32 player_position { get; set; }

        /// <summary></summary>
        Int32 player_vocation { get; set; }

        /// <summary></summary>
        Int32 player_sex { get; set; }

        /// <summary></summary>
        Int32 role_id { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 fame { get; set; }

        /// <summary></summary>
        Int32 power { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int32 role_state { get; set; }

        /// <summary></summary>
        Int32 role_exp { get; set; }

        /// <summary></summary>
        Int32 role_honor { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }

        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int32 ranking { get; set; }

        /// <summary></summary>
        Int32 totalCount { get; set; }

        /// <summary></summary>
        Int32 count { get; set; }

        /// <summary></summary>
        Int64 time { get; set; }

        /// <summary></summary>
        Int32 winCount { get; set; }

        /// <summary></summary>
        Int32 remove_cooling { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
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