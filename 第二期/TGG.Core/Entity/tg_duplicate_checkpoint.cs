using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>利塔副本</summary>
    [Serializable]
    [DataObject]
    [Description("利塔副本")]
    [BindIndex("PK_tg_duplicate_checkpoint", true, "id")]
    [BindTable("tg_duplicate_checkpoint", Description = "利塔副本", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_duplicate_checkpoint : Itg_duplicate_checkpoint
    {
        #region 属性
        private Int64 _id;
        /// <summary>自动编号</summary>
        [DisplayName("自动编号")]
        [Description("自动编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "自动编号", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>玩家ID</summary>
        [DisplayName("玩家ID")]
        [Description("玩家ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "玩家ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _site;
        /// <summary>通关到第几层</summary>
        [DisplayName("通关到第几层")]
        [Description("通关到第几层")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "site", "通关到第几层", "0", "int", 10, 0, false)]
        public virtual Int32 site
        {
            get { return _site; }
            set { if (OnPropertyChanging(__.site, value)) { _site = value; OnPropertyChanged(__.site); } }
        }

        private Int32 _tower_site;
        /// <summary>塔主层</summary>
        [DisplayName("塔主层")]
        [Description("塔主层")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "tower_site", "塔主层", "0", "int", 10, 0, false)]
        public virtual Int32 tower_site
        {
            get { return _tower_site; }
            set { if (OnPropertyChanging(__.tower_site, value)) { _tower_site = value; OnPropertyChanged(__.tower_site); } }
        }

        private String _ninjutsu_star;
        /// <summary>忍术游戏星星位置：0：失败星星；1：胜利星星</summary>
        [DisplayName("忍术游戏星星位置：0：失败星星；1：胜利星星")]
        [Description("忍术游戏星星位置：0：失败星星；1：胜利星星")]
        [DataObjectField(false, false, false, 20)]
        [BindColumn(5, "ninjutsu_star", "忍术游戏星星位置：0：失败星星；1：胜利星星", null, "nvarchar(20)", 0, 0, true)]
        public virtual String ninjutsu_star
        {
            get { return _ninjutsu_star; }
            set { if (OnPropertyChanging(__.ninjutsu_star, value)) { _ninjutsu_star = value; OnPropertyChanged(__.ninjutsu_star); } }
        }

        private String _calculate_star;
        /// <summary>算术游戏星星位置：0：失败星星；1：胜利星星</summary>
        [DisplayName("算术游戏星星位置：0：失败星星；1：胜利星星")]
        [Description("算术游戏星星位置：0：失败星星；1：胜利星星")]
        [DataObjectField(false, false, false, 20)]
        [BindColumn(6, "calculate_star", "算术游戏星星位置：0：失败星星；1：胜利星星", null, "nvarchar(20)", 0, 0, true)]
        public virtual String calculate_star
        {
            get { return _calculate_star; }
            set { if (OnPropertyChanging(__.calculate_star, value)) { _calculate_star = value; OnPropertyChanged(__.calculate_star); } }
        }

        private Int32 _state;
        /// <summary>通关状态</summary>
        [DisplayName("通关状态")]
        [Description("通关状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "state", "通关状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private String _npcids;
        /// <summary>怪物id集合</summary>
        [DisplayName("怪物id集合")]
        [Description("怪物id集合")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(8, "npcids", "怪物id集合", null, "nvarchar(50)", 0, 0, true)]
        public virtual String npcids
        {
            get { return _npcids; }
            set { if (OnPropertyChanging(__.npcids, value)) { _npcids = value; OnPropertyChanged(__.npcids); } }
        }

        private Int32 _npc_tea;
        /// <summary>npc茶席值</summary>
        [DisplayName("npc茶席值")]
        [Description("npc茶席值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "npc_tea", "npc茶席值", "0", "int", 10, 0, false)]
        public virtual Int32 npc_tea
        {
            get { return _npc_tea; }
            set { if (OnPropertyChanging(__.npc_tea, value)) { _npc_tea = value; OnPropertyChanged(__.npc_tea); } }
        }

        private Int32 _user_tea;
        /// <summary>用户茶席值</summary>
        [DisplayName("用户茶席值")]
        [Description("用户茶席值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "user_tea", "用户茶席值", "0", "int", 10, 0, false)]
        public virtual Int32 user_tea
        {
            get { return _user_tea; }
            set { if (OnPropertyChanging(__.user_tea, value)) { _user_tea = value; OnPropertyChanged(__.user_tea); } }
        }

        private Int32 _dekaron;
        /// <summary>挑战守护者状态</summary>
        [DisplayName("挑战守护者状态")]
        [Description("挑战守护者状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "dekaron", "挑战守护者状态", "0", "int", 10, 0, false)]
        public virtual Int32 dekaron
        {
            get { return _dekaron; }
            set { if (OnPropertyChanging(__.dekaron, value)) { _dekaron = value; OnPropertyChanged(__.dekaron); } }
        }

        private Int32 _blood;
        /// <summary>主角临时血量</summary>
        [DisplayName("主角临时血量")]
        [Description("主角临时血量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "blood", "主角临时血量", "0", "int", 10, 0, false)]
        public virtual Int32 blood
        {
            get { return _blood; }
            set { if (OnPropertyChanging(__.blood, value)) { _blood = value; OnPropertyChanged(__.blood); } }
        }

        private Int32 _user_blood;
        /// <summary>玩家气血值</summary>
        [DisplayName("玩家气血值")]
        [Description("玩家气血值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "user_blood", "玩家气血值", "0", "int", 10, 0, false)]
        public virtual Int32 user_blood
        {
            get { return _user_blood; }
            set { if (OnPropertyChanging(__.user_blood, value)) { _user_blood = value; OnPropertyChanged(__.user_blood); } }
        }

        private Int32 _npc_blood;
        /// <summary>npc气血值</summary>
        [DisplayName("npc气血值")]
        [Description("npc气血值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "npc_blood", "npc气血值", "0", "int", 10, 0, false)]
        public virtual Int32 npc_blood
        {
            get { return _npc_blood; }
            set { if (OnPropertyChanging(__.npc_blood, value)) { _npc_blood = value; OnPropertyChanged(__.npc_blood); } }
        }

        private String _select_position;
        /// <summary>记录翻开的牌</summary>
        [DisplayName("记录翻开的牌")]
        [Description("记录翻开的牌")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(15, "select_position", "记录翻开的牌", null, "varchar(100)", 0, 0, false)]
        public virtual String select_position
        {
            get { return _select_position; }
            set { if (OnPropertyChanging(__.select_position, value)) { _select_position = value; OnPropertyChanged(__.select_position); } }
        }

        private String _all_cards;
        /// <summary>已随机好的牌</summary>
        [DisplayName("已随机好的牌")]
        [Description("已随机好的牌")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(16, "all_cards", "已随机好的牌", null, "varchar(100)", 0, 0, false)]
        public virtual String all_cards
        {
            get { return _all_cards; }
            set { if (OnPropertyChanging(__.all_cards, value)) { _all_cards = value; OnPropertyChanged(__.all_cards); } }
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
                    case __.id: return _id;
                    case __.user_id: return _user_id;
                    case __.site: return _site;
                    case __.tower_site: return _tower_site;
                    case __.ninjutsu_star: return _ninjutsu_star;
                    case __.calculate_star: return _calculate_star;
                    case __.state: return _state;
                    case __.npcids: return _npcids;
                    case __.npc_tea: return _npc_tea;
                    case __.user_tea: return _user_tea;
                    case __.dekaron: return _dekaron;
                    case __.blood: return _blood;
                    case __.user_blood: return _user_blood;
                    case __.npc_blood: return _npc_blood;
                    case __.select_position: return _select_position;
                    case __.all_cards: return _all_cards;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.user_id: _user_id = Convert.ToInt64(value); break;
                    case __.site: _site = Convert.ToInt32(value); break;
                    case __.tower_site: _tower_site = Convert.ToInt32(value); break;
                    case __.ninjutsu_star: _ninjutsu_star = Convert.ToString(value); break;
                    case __.calculate_star: _calculate_star = Convert.ToString(value); break;
                    case __.state: _state = Convert.ToInt32(value); break;
                    case __.npcids: _npcids = Convert.ToString(value); break;
                    case __.npc_tea: _npc_tea = Convert.ToInt32(value); break;
                    case __.user_tea: _user_tea = Convert.ToInt32(value); break;
                    case __.dekaron: _dekaron = Convert.ToInt32(value); break;
                    case __.blood: _blood = Convert.ToInt32(value); break;
                    case __.user_blood: _user_blood = Convert.ToInt32(value); break;
                    case __.npc_blood: _npc_blood = Convert.ToInt32(value); break;
                    case __.select_position: _select_position = Convert.ToString(value); break;
                    case __.all_cards: _all_cards = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得利塔副本字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>自动编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>通关到第几层</summary>
            public static readonly Field site = FindByName(__.site);

            ///<summary>塔主层</summary>
            public static readonly Field tower_site = FindByName(__.tower_site);

            ///<summary>忍术游戏星星位置：0：失败星星；1：胜利星星</summary>
            public static readonly Field ninjutsu_star = FindByName(__.ninjutsu_star);

            ///<summary>算术游戏星星位置：0：失败星星；1：胜利星星</summary>
            public static readonly Field calculate_star = FindByName(__.calculate_star);

            ///<summary>通关状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>怪物id集合</summary>
            public static readonly Field npcids = FindByName(__.npcids);

            ///<summary>npc茶席值</summary>
            public static readonly Field npc_tea = FindByName(__.npc_tea);

            ///<summary>用户茶席值</summary>
            public static readonly Field user_tea = FindByName(__.user_tea);

            ///<summary>挑战守护者状态</summary>
            public static readonly Field dekaron = FindByName(__.dekaron);

            ///<summary>主角临时血量</summary>
            public static readonly Field blood = FindByName(__.blood);

            ///<summary>玩家气血值</summary>
            public static readonly Field user_blood = FindByName(__.user_blood);

            ///<summary>npc气血值</summary>
            public static readonly Field npc_blood = FindByName(__.npc_blood);

            ///<summary>记录翻开的牌</summary>
            public static readonly Field select_position = FindByName(__.select_position);

            ///<summary>已随机好的牌</summary>
            public static readonly Field all_cards = FindByName(__.all_cards);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得利塔副本字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>自动编号</summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>通关到第几层</summary>
            public const String site = "site";

            ///<summary>塔主层</summary>
            public const String tower_site = "tower_site";

            ///<summary>忍术游戏星星位置：0：失败星星；1：胜利星星</summary>
            public const String ninjutsu_star = "ninjutsu_star";

            ///<summary>算术游戏星星位置：0：失败星星；1：胜利星星</summary>
            public const String calculate_star = "calculate_star";

            ///<summary>通关状态</summary>
            public const String state = "state";

            ///<summary>怪物id集合</summary>
            public const String npcids = "npcids";

            ///<summary>npc茶席值</summary>
            public const String npc_tea = "npc_tea";

            ///<summary>用户茶席值</summary>
            public const String user_tea = "user_tea";

            ///<summary>挑战守护者状态</summary>
            public const String dekaron = "dekaron";

            ///<summary>主角临时血量</summary>
            public const String blood = "blood";

            ///<summary>玩家气血值</summary>
            public const String user_blood = "user_blood";

            ///<summary>npc气血值</summary>
            public const String npc_blood = "npc_blood";

            ///<summary>记录翻开的牌</summary>
            public const String select_position = "select_position";

            ///<summary>已随机好的牌</summary>
            public const String all_cards = "all_cards";

        }
        #endregion
    }

    /// <summary>利塔副本接口</summary>
    public partial interface Itg_duplicate_checkpoint
    {
        #region 属性
        /// <summary>自动编号</summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>通关到第几层</summary>
        Int32 site { get; set; }

        /// <summary>塔主层</summary>
        Int32 tower_site { get; set; }

        /// <summary>忍术游戏星星位置：0：失败星星；1：胜利星星</summary>
        String ninjutsu_star { get; set; }

        /// <summary>算术游戏星星位置：0：失败星星；1：胜利星星</summary>
        String calculate_star { get; set; }

        /// <summary>通关状态</summary>
        Int32 state { get; set; }

        /// <summary>怪物id集合</summary>
        String npcids { get; set; }

        /// <summary>npc茶席值</summary>
        Int32 npc_tea { get; set; }

        /// <summary>用户茶席值</summary>
        Int32 user_tea { get; set; }

        /// <summary>挑战守护者状态</summary>
        Int32 dekaron { get; set; }

        /// <summary>主角临时血量</summary>
        Int32 blood { get; set; }

        /// <summary>玩家气血值</summary>
        Int32 user_blood { get; set; }

        /// <summary>npc气血值</summary>
        Int32 npc_blood { get; set; }

        /// <summary>记录翻开的牌</summary>
        String select_position { get; set; }

        /// <summary>已随机好的牌</summary>
        String all_cards { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}