using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Ranking_Game</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_ranking_game", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_ranking_game : Iview_ranking_game
    {
        #region 属性
        private Int64 _ranks;
        /// <summary></summary>
        [DisplayName("Ranks")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(1, "ranks", "", null, "bigint", 19, 0, false)]
        public virtual Int64 ranks
        {
            get { return _ranks; }
            set { if (OnPropertyChanging(__.ranks, value)) { _ranks = value; OnPropertyChanged(__.ranks); } }
        }

        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private String _player_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "player_name", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "player_camp", "", null, "int", 10, 0, false)]
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
        [BindColumn(5, "player_influence", "", null, "int", 10, 0, false)]
        public virtual Int32 player_influence
        {
            get { return _player_influence; }
            set { if (OnPropertyChanging(__.player_influence, value)) { _player_influence = value; OnPropertyChanged(__.player_influence); } }
        }

        private Int64 _rid;
        /// <summary></summary>
        [DisplayName("Rid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "rid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "role_level", "", null, "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Int32 _tea_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "tea_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 tea_max_pass
        {
            get { return _tea_max_pass; }
            set { if (OnPropertyChanging(__.tea_max_pass, value)) { _tea_max_pass = value; OnPropertyChanged(__.tea_max_pass); } }
        }

        private Int32 _ninjutsu_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass1")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "ninjutsu_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 ninjutsu_max_pass
        {
            get { return _ninjutsu_max_pass; }
            set { if (OnPropertyChanging(__.ninjutsu_max_pass, value)) { _ninjutsu_max_pass = value; OnPropertyChanged(__.ninjutsu_max_pass); } }
        }

        private Int32 _calculate_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass2")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "calculate_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 calculate_max_pass
        {
            get { return _calculate_max_pass; }
            set { if (OnPropertyChanging(__.calculate_max_pass, value)) { _calculate_max_pass = value; OnPropertyChanged(__.calculate_max_pass); } }
        }

        private Int32 _eloquence_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass3")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "eloquence_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 eloquence_max_pass
        {
            get { return _eloquence_max_pass; }
            set { if (OnPropertyChanging(__.eloquence_max_pass, value)) { _eloquence_max_pass = value; OnPropertyChanged(__.eloquence_max_pass); } }
        }

        private Int32 _spirit_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass4")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "spirit_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 spirit_max_pass
        {
            get { return _spirit_max_pass; }
            set { if (OnPropertyChanging(__.spirit_max_pass, value)) { _spirit_max_pass = value; OnPropertyChanged(__.spirit_max_pass); } }
        }

        private Int32 _week_max_pass;
        /// <summary></summary>
        [DisplayName("Max_Pass5")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(14, "week_max_pass", "", null, "int", 10, 0, false)]
        public virtual Int32 week_max_pass
        {
            get { return _week_max_pass; }
            set { if (OnPropertyChanging(__.week_max_pass, value)) { _week_max_pass = value; OnPropertyChanged(__.week_max_pass); } }
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
                    case __.ranks : return _ranks;
                    case __.id : return _id;
                    case __.player_name : return _player_name;
                    case __.player_camp : return _player_camp;
                    case __.player_influence : return _player_influence;
                    case __.rid : return _rid;
                    case __.role_level : return _role_level;
                    case __.role_identity : return _role_identity;
                    case __.tea_max_pass : return _tea_max_pass;
                    case __.ninjutsu_max_pass : return _ninjutsu_max_pass;
                    case __.calculate_max_pass : return _calculate_max_pass;
                    case __.eloquence_max_pass : return _eloquence_max_pass;
                    case __.spirit_max_pass : return _spirit_max_pass;
                    case __.week_max_pass : return _week_max_pass;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ranks : _ranks = Convert.ToInt64(value); break;
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.player_influence : _player_influence = Convert.ToInt32(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    case __.tea_max_pass : _tea_max_pass = Convert.ToInt32(value); break;
                    case __.ninjutsu_max_pass : _ninjutsu_max_pass = Convert.ToInt32(value); break;
                    case __.calculate_max_pass : _calculate_max_pass = Convert.ToInt32(value); break;
                    case __.eloquence_max_pass : _eloquence_max_pass = Convert.ToInt32(value); break;
                    case __.spirit_max_pass : _spirit_max_pass = Convert.ToInt32(value); break;
                    case __.week_max_pass : _week_max_pass = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Ranking_Game字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field ranks = FindByName(__.ranks);

            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary></summary>
            public static readonly Field player_influence = FindByName(__.player_influence);

            ///<summary></summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary></summary>
            public static readonly Field tea_max_pass = FindByName(__.tea_max_pass);

            ///<summary></summary>
            public static readonly Field ninjutsu_max_pass = FindByName(__.ninjutsu_max_pass);

            ///<summary></summary>
            public static readonly Field calculate_max_pass = FindByName(__.calculate_max_pass);

            ///<summary></summary>
            public static readonly Field eloquence_max_pass = FindByName(__.eloquence_max_pass);

            ///<summary></summary>
            public static readonly Field spirit_max_pass = FindByName(__.spirit_max_pass);

            ///<summary></summary>
            public static readonly Field week_max_pass = FindByName(__.week_max_pass);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Ranking_Game字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String ranks = "ranks";

            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String player_camp = "player_camp";

            ///<summary></summary>
            public const String player_influence = "player_influence";

            ///<summary></summary>
            public const String rid = "rid";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String role_identity = "role_identity";

            ///<summary></summary>
            public const String tea_max_pass = "tea_max_pass";

            ///<summary></summary>
            public const String ninjutsu_max_pass = "ninjutsu_max_pass";

            ///<summary></summary>
            public const String calculate_max_pass = "calculate_max_pass";

            ///<summary></summary>
            public const String eloquence_max_pass = "eloquence_max_pass";

            ///<summary></summary>
            public const String spirit_max_pass = "spirit_max_pass";

            ///<summary></summary>
            public const String week_max_pass = "week_max_pass";

        }
        #endregion
    }

    /// <summary>Ranking_Game接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_ranking_game
    {
        #region 属性
        /// <summary></summary>
        Int64 ranks { get; set; }

        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 player_camp { get; set; }

        /// <summary></summary>
        Int32 player_influence { get; set; }

        /// <summary></summary>
        Int64 rid { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }

        /// <summary></summary>
        Int32 tea_max_pass { get; set; }

        /// <summary></summary>
        Int32 ninjutsu_max_pass { get; set; }

        /// <summary></summary>
        Int32 calculate_max_pass { get; set; }

        /// <summary></summary>
        Int32 eloquence_max_pass { get; set; }

        /// <summary></summary>
        Int32 spirit_max_pass { get; set; }

        /// <summary></summary>
        Int32 week_max_pass { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}