using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家VIP表</summary>
    [DataObject]
    [Description("玩家VIP表")]
    [BindIndex("PK__tg_user___3213E83F503BEA1C", true, "id")]
    [BindTable("tg_user_vip", Description = "玩家VIP表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_vip : Itg_user_vip
    {
        #region 属性
        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _vip_level;
        /// <summary>VIP等级</summary>
        [DisplayName("VIP等级")]
        [Description("VIP等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "vip_level", "VIP等级", "0", "int", 10, 0, false)]
        public virtual Int32 vip_level
        {
            get { return _vip_level; }
            set { if (OnPropertyChanging(__.vip_level, value)) { _vip_level = value; OnPropertyChanged(__.vip_level); } }
        }

        private Int32 _vip_gold;
        /// <summary>充值元宝</summary>
        [DisplayName("充值元宝")]
        [Description("充值元宝")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "vip_gold", "充值元宝", "0", "int", 10, 0, false)]
        public virtual Int32 vip_gold
        {
            get { return _vip_gold; }
            set { if (OnPropertyChanging(__.vip_gold, value)) { _vip_gold = value; OnPropertyChanged(__.vip_gold); } }
        }

        private Int32 _power;
        /// <summary>购买体力</summary>
        [DisplayName("购买体力")]
        [Description("购买体力")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "power", "购买体力", "0", "int", 10, 0, false)]
        public virtual Int32 power
        {
            get { return _power; }
            set { if (OnPropertyChanging(__.power, value)) { _power = value; OnPropertyChanged(__.power); } }
        }

        private Int32 _area;
        /// <summary>可开启商圈数</summary>
        [DisplayName("可开启商圈数")]
        [Description("可开启商圈数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "area", "可开启商圈数", "0", "int", 10, 0, false)]
        public virtual Int32 area
        {
            get { return _area; }
            set { if (OnPropertyChanging(__.area, value)) { _area = value; OnPropertyChanged(__.area); } }
        }

        private Int32 _car;
        /// <summary>增加跑商队列</summary>
        [DisplayName("增加跑商队列")]
        [Description("增加跑商队列")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "car", "增加跑商队列", "0", "int", 10, 0, false)]
        public virtual Int32 car
        {
            get { return _car; }
            set { if (OnPropertyChanging(__.car, value)) { _car = value; OnPropertyChanged(__.car); } }
        }

        private Int32 _fight;
        /// <summary>跳过战斗</summary>
        [DisplayName("跳过战斗")]
        [Description("跳过战斗")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "fight", "跳过战斗", "0", "int", 10, 0, false)]
        public virtual Int32 fight
        {
            get { return _fight; }
            set { if (OnPropertyChanging(__.fight, value)) { _fight = value; OnPropertyChanged(__.fight); } }
        }

        private Int32 _bargain;
        /// <summary>恢复讲价次数</summary>
        [DisplayName("恢复讲价次数")]
        [Description("恢复讲价次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "bargain", "恢复讲价次数", "0", "int", 10, 0, false)]
        public virtual Int32 bargain
        {
            get { return _bargain; }
            set { if (OnPropertyChanging(__.bargain, value)) { _bargain = value; OnPropertyChanged(__.bargain); } }
        }

        private Int32 _buy;
        /// <summary>恢复购买量</summary>
        [DisplayName("恢复购买量")]
        [Description("恢复购买量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "buy", "恢复购买量", "0", "int", 10, 0, false)]
        public virtual Int32 buy
        {
            get { return _buy; }
            set { if (OnPropertyChanging(__.buy, value)) { _buy = value; OnPropertyChanged(__.buy); } }
        }

        private Int32 _arena_buy;
        /// <summary>竞技场挑战次数购买</summary>
        [DisplayName("竞技场挑战次数购买")]
        [Description("竞技场挑战次数购买")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "arena_buy", "竞技场挑战次数购买", "0", "int", 10, 0, false)]
        public virtual Int32 arena_buy
        {
            get { return _arena_buy; }
            set { if (OnPropertyChanging(__.arena_buy, value)) { _arena_buy = value; OnPropertyChanged(__.arena_buy); } }
        }

        private Int32 _arena_cd;
        /// <summary>竞技场冷却CD免费消除次数</summary>
        [DisplayName("竞技场冷却CD免费消除次数")]
        [Description("竞技场冷却CD免费消除次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "arena_cd", "竞技场冷却CD免费消除次数", "0", "int", 10, 0, false)]
        public virtual Int32 arena_cd
        {
            get { return _arena_cd; }
            set { if (OnPropertyChanging(__.arena_cd, value)) { _arena_cd = value; OnPropertyChanged(__.arena_cd); } }
        }

        private Int32 _train_bar;
        /// <summary>武将修行栏个数</summary>
        [DisplayName("武将修行栏个数")]
        [Description("武将修行栏个数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "train_bar", "武将修行栏个数", "0", "int", 10, 0, false)]
        public virtual Int32 train_bar
        {
            get { return _train_bar; }
            set { if (OnPropertyChanging(__.train_bar, value)) { _train_bar = value; OnPropertyChanged(__.train_bar); } }
        }

        private Int32 _train_home;
        /// <summary>武将宅刷新次数</summary>
        [DisplayName("武将宅刷新次数")]
        [Description("武将宅刷新次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "train_home", "武将宅刷新次数", "0", "int", 10, 0, false)]
        public virtual Int32 train_home
        {
            get { return _train_home; }
            set { if (OnPropertyChanging(__.train_home, value)) { _train_home = value; OnPropertyChanged(__.train_home); } }
        }

        private Int32 _tax_count;
        /// <summary>使用的免税次数</summary>
        [DisplayName("使用的免税次数")]
        [Description("使用的免税次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "tax_count", "使用的免税次数", "0", "int", 10, 0, false)]
        public virtual Int32 tax_count
        {
            get { return _tax_count; }
            set { if (OnPropertyChanging(__.tax_count, value)) { _tax_count = value; OnPropertyChanged(__.tax_count); } }
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
                    case __.vip_level : return _vip_level;
                    case __.vip_gold : return _vip_gold;
                    case __.power : return _power;
                    case __.area : return _area;
                    case __.car : return _car;
                    case __.fight : return _fight;
                    case __.bargain : return _bargain;
                    case __.buy : return _buy;
                    case __.arena_buy : return _arena_buy;
                    case __.arena_cd : return _arena_cd;
                    case __.train_bar : return _train_bar;
                    case __.train_home : return _train_home;
                    case __.tax_count : return _tax_count;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.vip_level : _vip_level = Convert.ToInt32(value); break;
                    case __.vip_gold : _vip_gold = Convert.ToInt32(value); break;
                    case __.power : _power = Convert.ToInt32(value); break;
                    case __.area : _area = Convert.ToInt32(value); break;
                    case __.car : _car = Convert.ToInt32(value); break;
                    case __.fight : _fight = Convert.ToInt32(value); break;
                    case __.bargain : _bargain = Convert.ToInt32(value); break;
                    case __.buy : _buy = Convert.ToInt32(value); break;
                    case __.arena_buy : _arena_buy = Convert.ToInt32(value); break;
                    case __.arena_cd : _arena_cd = Convert.ToInt32(value); break;
                    case __.train_bar : _train_bar = Convert.ToInt32(value); break;
                    case __.train_home : _train_home = Convert.ToInt32(value); break;
                    case __.tax_count : _tax_count = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家VIP表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>VIP等级</summary>
            public static readonly Field vip_level = FindByName(__.vip_level);

            ///<summary>充值元宝</summary>
            public static readonly Field vip_gold = FindByName(__.vip_gold);

            ///<summary>购买体力</summary>
            public static readonly Field power = FindByName(__.power);

            ///<summary>可开启商圈数</summary>
            public static readonly Field area = FindByName(__.area);

            ///<summary>增加跑商队列</summary>
            public static readonly Field car = FindByName(__.car);

            ///<summary>跳过战斗</summary>
            public static readonly Field fight = FindByName(__.fight);

            ///<summary>恢复讲价次数</summary>
            public static readonly Field bargain = FindByName(__.bargain);

            ///<summary>恢复购买量</summary>
            public static readonly Field buy = FindByName(__.buy);

            ///<summary>竞技场挑战次数购买</summary>
            public static readonly Field arena_buy = FindByName(__.arena_buy);

            ///<summary>竞技场冷却CD免费消除次数</summary>
            public static readonly Field arena_cd = FindByName(__.arena_cd);

            ///<summary>武将修行栏个数</summary>
            public static readonly Field train_bar = FindByName(__.train_bar);

            ///<summary>武将宅刷新次数</summary>
            public static readonly Field train_home = FindByName(__.train_home);

            ///<summary>使用的免税次数</summary>
            public static readonly Field tax_count = FindByName(__.tax_count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家VIP表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>VIP等级</summary>
            public const String vip_level = "vip_level";

            ///<summary>充值元宝</summary>
            public const String vip_gold = "vip_gold";

            ///<summary>购买体力</summary>
            public const String power = "power";

            ///<summary>可开启商圈数</summary>
            public const String area = "area";

            ///<summary>增加跑商队列</summary>
            public const String car = "car";

            ///<summary>跳过战斗</summary>
            public const String fight = "fight";

            ///<summary>恢复讲价次数</summary>
            public const String bargain = "bargain";

            ///<summary>恢复购买量</summary>
            public const String buy = "buy";

            ///<summary>竞技场挑战次数购买</summary>
            public const String arena_buy = "arena_buy";

            ///<summary>竞技场冷却CD免费消除次数</summary>
            public const String arena_cd = "arena_cd";

            ///<summary>武将修行栏个数</summary>
            public const String train_bar = "train_bar";

            ///<summary>武将宅刷新次数</summary>
            public const String train_home = "train_home";

            ///<summary>使用的免税次数</summary>
            public const String tax_count = "tax_count";

        }
        #endregion
    }

    /// <summary>玩家VIP表接口</summary>
    public partial interface Itg_user_vip
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>VIP等级</summary>
        Int32 vip_level { get; set; }

        /// <summary>充值元宝</summary>
        Int32 vip_gold { get; set; }

        /// <summary>购买体力</summary>
        Int32 power { get; set; }

        /// <summary>可开启商圈数</summary>
        Int32 area { get; set; }

        /// <summary>增加跑商队列</summary>
        Int32 car { get; set; }

        /// <summary>跳过战斗</summary>
        Int32 fight { get; set; }

        /// <summary>恢复讲价次数</summary>
        Int32 bargain { get; set; }

        /// <summary>恢复购买量</summary>
        Int32 buy { get; set; }

        /// <summary>竞技场挑战次数购买</summary>
        Int32 arena_buy { get; set; }

        /// <summary>竞技场冷却CD免费消除次数</summary>
        Int32 arena_cd { get; set; }

        /// <summary>武将修行栏个数</summary>
        Int32 train_bar { get; set; }

        /// <summary>武将宅刷新次数</summary>
        Int32 train_home { get; set; }

        /// <summary>使用的免税次数</summary>
        Int32 tax_count { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}