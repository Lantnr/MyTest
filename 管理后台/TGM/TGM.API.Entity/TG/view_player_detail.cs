using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Player_Detail</summary>
    /// <remarks></remarks>
    [DataObject]
    [Description("")]
    [BindTable("view_player_detail", Description = "", ConnName = "TGG", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_player_detail : Iview_player_detail
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

        private String _user_code;
        /// <summary></summary>
        [DisplayName("Code")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(2, "user_code", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
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

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "role_level", "", null, "int", 10, 0, false)]
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
        [BindColumn(5, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Int64 _coin;
        /// <summary></summary>
        [DisplayName("Coin")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "coin", "", null, "bigint", 19, 0, false)]
        public virtual Int64 coin
        {
            get { return _coin; }
            set { if (OnPropertyChanging(__.coin, value)) { _coin = value; OnPropertyChanged(__.coin); } }
        }

        private Int32 _gold;
        /// <summary></summary>
        [DisplayName("Gold")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "gold", "", null, "int", 10, 0, false)]
        public virtual Int32 gold
        {
            get { return _gold; }
            set { if (OnPropertyChanging(__.gold, value)) { _gold = value; OnPropertyChanged(__.gold); } }
        }

        private Int32 _vip_level;
        /// <summary></summary>
        [DisplayName("Level1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "vip_level", "", null, "int", 10, 0, false)]
        public virtual Int32 vip_level
        {
            get { return _vip_level; }
            set { if (OnPropertyChanging(__.vip_level, value)) { _vip_level = value; OnPropertyChanged(__.vip_level); } }
        }

        private Int32 _vip_gold;
        /// <summary></summary>
        [DisplayName("Gold1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "vip_gold", "", null, "int", 10, 0, false)]
        public virtual Int32 vip_gold
        {
            get { return _vip_gold; }
            set { if (OnPropertyChanging(__.vip_gold, value)) { _vip_gold = value; OnPropertyChanged(__.vip_gold); } }
        }

        private Int64 _login_time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "login_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 login_time
        {
            get { return _login_time; }
            set { if (OnPropertyChanging(__.login_time, value)) { _login_time = value; OnPropertyChanged(__.login_time); } }
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
                    case __.user_code : return _user_code;
                    case __.player_name : return _player_name;
                    case __.role_level : return _role_level;
                    case __.role_identity : return _role_identity;
                    case __.coin : return _coin;
                    case __.gold : return _gold;
                    case __.vip_level : return _vip_level;
                    case __.vip_gold : return _vip_gold;
                    case __.login_time : return _login_time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_code : _user_code = Convert.ToString(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    case __.coin : _coin = Convert.ToInt64(value); break;
                    case __.gold : _gold = Convert.ToInt32(value); break;
                    case __.vip_level : _vip_level = Convert.ToInt32(value); break;
                    case __.vip_gold : _vip_gold = Convert.ToInt32(value); break;
                    case __.login_time : _login_time = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Player_Detail字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary></summary>
            public static readonly Field coin = FindByName(__.coin);

            ///<summary></summary>
            public static readonly Field gold = FindByName(__.gold);

            ///<summary></summary>
            public static readonly Field vip_level = FindByName(__.vip_level);

            ///<summary></summary>
            public static readonly Field vip_gold = FindByName(__.vip_gold);

            ///<summary></summary>
            public static readonly Field login_time = FindByName(__.login_time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Player_Detail字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String user_code = "user_code";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String role_identity = "role_identity";

            ///<summary></summary>
            public const String coin = "coin";

            ///<summary></summary>
            public const String gold = "gold";

            ///<summary></summary>
            public const String vip_level = "vip_level";

            ///<summary></summary>
            public const String vip_gold = "vip_gold";

            ///<summary></summary>
            public const String login_time = "login_time";

        }
        #endregion
    }

    /// <summary>Player_Detail接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_player_detail
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        String user_code { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }

        /// <summary></summary>
        Int64 coin { get; set; }

        /// <summary></summary>
        Int32 gold { get; set; }

        /// <summary></summary>
        Int32 vip_level { get; set; }

        /// <summary></summary>
        Int32 vip_gold { get; set; }

        /// <summary></summary>
        Int64 login_time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}