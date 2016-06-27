using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Ranking_Honor</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_ranking_honor", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_ranking_honor : Iview_ranking_honor
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

        private Int32 _role_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "role_id", "", null, "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
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

        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "player_camp", "", null, "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
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

        private Int64 _rid;
        /// <summary></summary>
        [DisplayName("Rid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "rid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _role_honor;
        /// <summary></summary>
        [DisplayName("Honor1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "role_honor", "", null, "int", 10, 0, false)]
        public virtual Int32 role_honor
        {
            get { return _role_honor; }
            set { if (OnPropertyChanging(__.role_honor, value)) { _role_honor = value; OnPropertyChanged(__.role_honor); } }
        }

        private Int32 _honor;
        /// <summary></summary>
        [DisplayName("Honor")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "honor", "", null, "int", 10, 0, false)]
        public virtual Int32 honor
        {
            get { return _honor; }
            set { if (OnPropertyChanging(__.honor, value)) { _honor = value; OnPropertyChanged(__.honor); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
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
                    case __.role_id : return _role_id;
                    case __.player_influence : return _player_influence;
                    case __.player_camp : return _player_camp;
                    case __.role_level : return _role_level;
                    case __.rid : return _rid;
                    case __.role_honor : return _role_honor;
                    case __.honor : return _honor;
                    case __.role_identity : return _role_identity;
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
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.player_influence : _player_influence = Convert.ToInt32(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.role_honor : _role_honor = Convert.ToInt32(value); break;
                    case __.honor : _honor = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Ranking_Honor字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field ranks = FindByName(__.ranks);

            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary></summary>
            public static readonly Field player_influence = FindByName(__.player_influence);

            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary></summary>
            public static readonly Field role_honor = FindByName(__.role_honor);

            ///<summary></summary>
            public static readonly Field honor = FindByName(__.honor);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Ranking_Honor字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String ranks = "ranks";

            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String role_id = "role_id";

            ///<summary></summary>
            public const String player_influence = "player_influence";

            ///<summary></summary>
            public const String player_camp = "player_camp";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String rid = "rid";

            ///<summary></summary>
            public const String role_honor = "role_honor";

            ///<summary></summary>
            public const String honor = "honor";

            ///<summary></summary>
            public const String role_identity = "role_identity";

        }
        #endregion
    }

    /// <summary>Ranking_Honor接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_ranking_honor
    {
        #region 属性
        /// <summary></summary>
        Int64 ranks { get; set; }

        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 role_id { get; set; }

        /// <summary></summary>
        Int32 player_influence { get; set; }

        /// <summary></summary>
        Int32 player_camp { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int64 rid { get; set; }

        /// <summary></summary>
        Int32 role_honor { get; set; }

        /// <summary></summary>
        Int32 honor { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}