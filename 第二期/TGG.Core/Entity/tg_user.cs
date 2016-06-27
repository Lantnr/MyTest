using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家信息表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家信息表")]
    [BindTable("tg_user", Description = "玩家信息表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_user : Itg_user
    {
        #region 属性
        private Int64 _id;
        /// <summary>玩家ID</summary>
        [DisplayName("玩家ID")]
        [Description("玩家ID")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "玩家ID", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private String _user_code;
        /// <summary>关联代码</summary>
        [DisplayName("关联代码")]
        [Description("关联代码")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(2, "user_code", "关联代码", null, "nvarchar(200)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
        }

        private String _player_name;
        /// <summary>玩家名称</summary>
        [DisplayName("玩家名称")]
        [Description("玩家名称")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "player_name", "玩家名称", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private Int32 _role_id;
        /// <summary>武将基表编号</summary>
        [DisplayName("武将基表编号")]
        [Description("武将基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "role_id", "武将基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private Int32 _player_sex;
        /// <summary>玩家性别</summary>
        [DisplayName("玩家性别")]
        [Description("玩家性别")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "player_sex", "玩家性别", "0", "int", 10, 0, false)]
        public virtual Int32 player_sex
        {
            get { return _player_sex; }
            set { if (OnPropertyChanging(__.player_sex, value)) { _player_sex = value; OnPropertyChanged(__.player_sex); } }
        }

        private Int32 _player_vocation;
        /// <summary>玩家职业</summary>
        [DisplayName("玩家职业")]
        [Description("玩家职业")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "player_vocation", "玩家职业", "0", "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _player_position;
        /// <summary>官位</summary>
        [DisplayName("官位")]
        [Description("官位")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "player_position", "官位", "0", "int", 10, 0, false)]
        public virtual Int32 player_position
        {
            get { return _player_position; }
            set { if (OnPropertyChanging(__.player_position, value)) { _player_position = value; OnPropertyChanged(__.player_position); } }
        }

        private Int32 _player_camp;
        /// <summary>阵营</summary>
        [DisplayName("阵营")]
        [Description("阵营")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "player_camp", "阵营", "0", "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
        }

        private Int32 _player_influence;
        /// <summary>玩家势力</summary>
        [DisplayName("玩家势力")]
        [Description("玩家势力")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "player_influence", "玩家势力", "0", "int", 10, 0, false)]
        public virtual Int32 player_influence
        {
            get { return _player_influence; }
            set { if (OnPropertyChanging(__.player_influence, value)) { _player_influence = value; OnPropertyChanged(__.player_influence); } }
        }

        private Int32 _birthplace;
        /// <summary>出生地</summary>
        [DisplayName("出生地")]
        [Description("出生地")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "birthplace", "出生地", "0", "int", 10, 0, false)]
        public virtual Int32 birthplace
        {
            get { return _birthplace; }
            set { if (OnPropertyChanging(__.birthplace, value)) { _birthplace = value; OnPropertyChanged(__.birthplace); } }
        }

        private Int32 _spirit;
        /// <summary>魂</summary>
        [DisplayName("魂")]
        [Description("魂")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "spirit", "魂", "0", "int", 10, 0, false)]
        public virtual Int32 spirit
        {
            get { return _spirit; }
            set { if (OnPropertyChanging(__.spirit, value)) { _spirit = value; OnPropertyChanged(__.spirit); } }
        }

        private Int32 _fame;
        /// <summary>声望</summary>
        [DisplayName("声望")]
        [Description("声望")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "fame", "声望", "0", "int", 10, 0, false)]
        public virtual Int32 fame
        {
            get { return _fame; }
            set { if (OnPropertyChanging(__.fame, value)) { _fame = value; OnPropertyChanged(__.fame); } }
        }

        private Int64 _coin;
        /// <summary>铜币</summary>
        [DisplayName("铜币")]
        [Description("铜币")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(13, "coin", "铜币", "0", "bigint", 19, 0, false)]
        public virtual Int64 coin
        {
            get { return _coin; }
            set { if (OnPropertyChanging(__.coin, value)) { _coin = value; OnPropertyChanged(__.coin); } }
        }

        private Int32 _gold;
        /// <summary>金币</summary>
        [DisplayName("金币")]
        [Description("金币")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "gold", "金币", "0", "int", 10, 0, false)]
        public virtual Int32 gold
        {
            get { return _gold; }
            set { if (OnPropertyChanging(__.gold, value)) { _gold = value; OnPropertyChanged(__.gold); } }
        }

        private Int32 _rmb;
        /// <summary>内币</summary>
        [DisplayName("内币")]
        [Description("内币")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "rmb", "内币", "0", "int", 10, 0, false)]
        public virtual Int32 rmb
        {
            get { return _rmb; }
            set { if (OnPropertyChanging(__.rmb, value)) { _rmb = value; OnPropertyChanged(__.rmb); } }
        }

        private Int32 _coupon;
        /// <summary>礼券</summary>
        [DisplayName("礼券")]
        [Description("礼券")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "coupon", "礼券", "0", "int", 10, 0, false)]
        public virtual Int32 coupon
        {
            get { return _coupon; }
            set { if (OnPropertyChanging(__.coupon, value)) { _coupon = value; OnPropertyChanged(__.coupon); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(17, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
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
                    case __.role_id : return _role_id;
                    case __.player_sex : return _player_sex;
                    case __.player_vocation : return _player_vocation;
                    case __.player_position : return _player_position;
                    case __.player_camp : return _player_camp;
                    case __.player_influence : return _player_influence;
                    case __.birthplace : return _birthplace;
                    case __.spirit : return _spirit;
                    case __.fame : return _fame;
                    case __.coin : return _coin;
                    case __.gold : return _gold;
                    case __.rmb : return _rmb;
                    case __.coupon : return _coupon;
                    case __.createtime : return _createtime;
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
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.player_sex : _player_sex = Convert.ToInt32(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.player_position : _player_position = Convert.ToInt32(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.player_influence : _player_influence = Convert.ToInt32(value); break;
                    case __.birthplace : _birthplace = Convert.ToInt32(value); break;
                    case __.spirit : _spirit = Convert.ToInt32(value); break;
                    case __.fame : _fame = Convert.ToInt32(value); break;
                    case __.coin : _coin = Convert.ToInt64(value); break;
                    case __.gold : _gold = Convert.ToInt32(value); break;
                    case __.rmb : _rmb = Convert.ToInt32(value); break;
                    case __.coupon : _coupon = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家信息表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>玩家ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>关联代码</summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary>玩家名称</summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary>武将基表编号</summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary>玩家性别</summary>
            public static readonly Field player_sex = FindByName(__.player_sex);

            ///<summary>玩家职业</summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary>官位</summary>
            public static readonly Field player_position = FindByName(__.player_position);

            ///<summary>阵营</summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary>玩家势力</summary>
            public static readonly Field player_influence = FindByName(__.player_influence);

            ///<summary>出生地</summary>
            public static readonly Field birthplace = FindByName(__.birthplace);

            ///<summary>魂</summary>
            public static readonly Field spirit = FindByName(__.spirit);

            ///<summary>声望</summary>
            public static readonly Field fame = FindByName(__.fame);

            ///<summary>铜币</summary>
            public static readonly Field coin = FindByName(__.coin);

            ///<summary>金币</summary>
            public static readonly Field gold = FindByName(__.gold);

            ///<summary>内币</summary>
            public static readonly Field rmb = FindByName(__.rmb);

            ///<summary>礼券</summary>
            public static readonly Field coupon = FindByName(__.coupon);

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家信息表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>玩家ID</summary>
            public const String id = "id";

            ///<summary>关联代码</summary>
            public const String user_code = "user_code";

            ///<summary>玩家名称</summary>
            public const String player_name = "player_name";

            ///<summary>武将基表编号</summary>
            public const String role_id = "role_id";

            ///<summary>玩家性别</summary>
            public const String player_sex = "player_sex";

            ///<summary>玩家职业</summary>
            public const String player_vocation = "player_vocation";

            ///<summary>官位</summary>
            public const String player_position = "player_position";

            ///<summary>阵营</summary>
            public const String player_camp = "player_camp";

            ///<summary>玩家势力</summary>
            public const String player_influence = "player_influence";

            ///<summary>出生地</summary>
            public const String birthplace = "birthplace";

            ///<summary>魂</summary>
            public const String spirit = "spirit";

            ///<summary>声望</summary>
            public const String fame = "fame";

            ///<summary>铜币</summary>
            public const String coin = "coin";

            ///<summary>金币</summary>
            public const String gold = "gold";

            ///<summary>内币</summary>
            public const String rmb = "rmb";

            ///<summary>礼券</summary>
            public const String coupon = "coupon";

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>玩家信息表接口</summary>
    public partial interface Itg_user
    {
        #region 属性
        /// <summary>玩家ID</summary>
        Int64 id { get; set; }

        /// <summary>关联代码</summary>
        String user_code { get; set; }

        /// <summary>玩家名称</summary>
        String player_name { get; set; }

        /// <summary>武将基表编号</summary>
        Int32 role_id { get; set; }

        /// <summary>玩家性别</summary>
        Int32 player_sex { get; set; }

        /// <summary>玩家职业</summary>
        Int32 player_vocation { get; set; }

        /// <summary>官位</summary>
        Int32 player_position { get; set; }

        /// <summary>阵营</summary>
        Int32 player_camp { get; set; }

        /// <summary>玩家势力</summary>
        Int32 player_influence { get; set; }

        /// <summary>出生地</summary>
        Int32 birthplace { get; set; }

        /// <summary>魂</summary>
        Int32 spirit { get; set; }

        /// <summary>声望</summary>
        Int32 fame { get; set; }

        /// <summary>铜币</summary>
        Int64 coin { get; set; }

        /// <summary>金币</summary>
        Int32 gold { get; set; }

        /// <summary>内币</summary>
        Int32 rmb { get; set; }

        /// <summary>礼券</summary>
        Int32 coupon { get; set; }

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}