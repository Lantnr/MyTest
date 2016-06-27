using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>武将表</summary>
    [Serializable]
    [DataObject]
    [Description("武将表")]
    [BindIndex("PK__tg_role__3213E83F0361F627", true, "id")]
    [BindTable("tg_role", Description = "武将表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_role : Itg_role
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

        private Int32 _role_id;
        /// <summary>武将基表编号</summary>
        [DisplayName("武将基表编号")]
        [Description("武将基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "role_id", "武将基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private Int32 _power;
        /// <summary>体力</summary>
        [DisplayName("体力")]
        [Description("体力")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "power", "体力", "0", "int", 10, 0, false)]
        public virtual Int32 power
        {
            get { return _power; }
            set { if (OnPropertyChanging(__.power, value)) { _power = value; OnPropertyChanged(__.power); } }
        }

        private Int32 _role_genre;
        /// <summary>武将默认流派</summary>
        [DisplayName("武将默认流派")]
        [Description("武将默认流派")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "role_genre", "武将默认流派", "0", "int", 10, 0, false)]
        public virtual Int32 role_genre
        {
            get { return _role_genre; }
            set { if (OnPropertyChanging(__.role_genre, value)) { _role_genre = value; OnPropertyChanged(__.role_genre); } }
        }

        private Int32 _role_ninja;
        /// <summary>武将默认忍者冢</summary>
        [DisplayName("武将默认忍者冢")]
        [Description("武将默认忍者冢")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "role_ninja", "武将默认忍者冢", "0", "int", 10, 0, false)]
        public virtual Int32 role_ninja
        {
            get { return _role_ninja; }
            set { if (OnPropertyChanging(__.role_ninja, value)) { _role_ninja = value; OnPropertyChanged(__.role_ninja); } }
        }

        private Int32 _role_level;
        /// <summary>武将等级</summary>
        [DisplayName("武将等级")]
        [Description("武将等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "role_level", "武将等级", "1", "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
        }

        private Int32 _role_state;
        /// <summary>武将状态</summary>
        [DisplayName("武将状态")]
        [Description("武将状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "role_state", "武将状态", "0", "int", 10, 0, false)]
        public virtual Int32 role_state
        {
            get { return _role_state; }
            set { if (OnPropertyChanging(__.role_state, value)) { _role_state = value; OnPropertyChanged(__.role_state); } }
        }

        private Int32 _role_exp;
        /// <summary>武将经验</summary>
        [DisplayName("武将经验")]
        [Description("武将经验")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "role_exp", "武将经验", "0", "int", 10, 0, false)]
        public virtual Int32 role_exp
        {
            get { return _role_exp; }
            set { if (OnPropertyChanging(__.role_exp, value)) { _role_exp = value; OnPropertyChanged(__.role_exp); } }
        }

        private Int32 _role_honor;
        /// <summary>武将功勋</summary>
        [DisplayName("武将功勋")]
        [Description("武将功勋")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "role_honor", "武将功勋", "0", "int", 10, 0, false)]
        public virtual Int32 role_honor
        {
            get { return _role_honor; }
            set { if (OnPropertyChanging(__.role_honor, value)) { _role_honor = value; OnPropertyChanged(__.role_honor); } }
        }

        private Int32 _role_identity;
        /// <summary>身份</summary>
        [DisplayName("身份")]
        [Description("身份")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "role_identity", "身份", "0", "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Double _base_captain;
        /// <summary>统帅(初始值)</summary>
        [DisplayName("统帅初始值")]
        [Description("统帅(初始值)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(12, "base_captain", "统帅(初始值)", "0", "float", 53, 0, false)]
        public virtual Double base_captain
        {
            get { return _base_captain; }
            set { if (OnPropertyChanging(__.base_captain, value)) { _base_captain = value; OnPropertyChanged(__.base_captain); } }
        }

        private Double _base_force;
        /// <summary>武力(初始值)</summary>
        [DisplayName("武力初始值")]
        [Description("武力(初始值)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(13, "base_force", "武力(初始值)", "0", "float", 53, 0, false)]
        public virtual Double base_force
        {
            get { return _base_force; }
            set { if (OnPropertyChanging(__.base_force, value)) { _base_force = value; OnPropertyChanged(__.base_force); } }
        }

        private Double _base_brains;
        /// <summary>智谋(初始值)</summary>
        [DisplayName("智谋初始值")]
        [Description("智谋(初始值)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(14, "base_brains", "智谋(初始值)", "0", "float", 53, 0, false)]
        public virtual Double base_brains
        {
            get { return _base_brains; }
            set { if (OnPropertyChanging(__.base_brains, value)) { _base_brains = value; OnPropertyChanged(__.base_brains); } }
        }

        private Double _base_charm;
        /// <summary>魅力(初始值)</summary>
        [DisplayName("魅力初始值")]
        [Description("魅力(初始值)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(15, "base_charm", "魅力(初始值)", "0", "float", 53, 0, false)]
        public virtual Double base_charm
        {
            get { return _base_charm; }
            set { if (OnPropertyChanging(__.base_charm, value)) { _base_charm = value; OnPropertyChanged(__.base_charm); } }
        }

        private Double _base_govern;
        /// <summary>政务(初始值)</summary>
        [DisplayName("政务初始值")]
        [Description("政务(初始值)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(16, "base_govern", "政务(初始值)", "0", "float", 53, 0, false)]
        public virtual Double base_govern
        {
            get { return _base_govern; }
            set { if (OnPropertyChanging(__.base_govern, value)) { _base_govern = value; OnPropertyChanged(__.base_govern); } }
        }

        private Int32 _att_points;
        /// <summary>武将剩余属性点</summary>
        [DisplayName("武将剩余属性点")]
        [Description("武将剩余属性点")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "att_points", "武将剩余属性点", "0", "int", 10, 0, false)]
        public virtual Int32 att_points
        {
            get { return _att_points; }
            set { if (OnPropertyChanging(__.att_points, value)) { _att_points = value; OnPropertyChanged(__.att_points); } }
        }

        private Int32 _att_life;
        /// <summary>生命</summary>
        [DisplayName("生命")]
        [Description("生命")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "att_life", "生命", "0", "int", 10, 0, false)]
        public virtual Int32 att_life
        {
            get { return _att_life; }
            set { if (OnPropertyChanging(__.att_life, value)) { _att_life = value; OnPropertyChanged(__.att_life); } }
        }

        private Double _att_attack;
        /// <summary>攻击</summary>
        [DisplayName("攻击")]
        [Description("攻击")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(19, "att_attack", "攻击", "0", "float", 53, 0, false)]
        public virtual Double att_attack
        {
            get { return _att_attack; }
            set { if (OnPropertyChanging(__.att_attack, value)) { _att_attack = value; OnPropertyChanged(__.att_attack); } }
        }

        private Double _att_defense;
        /// <summary>防御</summary>
        [DisplayName("防御")]
        [Description("防御")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(20, "att_defense", "防御", "0", "float", 53, 0, false)]
        public virtual Double att_defense
        {
            get { return _att_defense; }
            set { if (OnPropertyChanging(__.att_defense, value)) { _att_defense = value; OnPropertyChanged(__.att_defense); } }
        }

        private Double _att_sub_hurtIncrease;
        /// <summary>增伤</summary>
        [DisplayName("增伤")]
        [Description("增伤")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(21, "att_sub_hurtIncrease", "增伤", "0", "float", 53, 0, false)]
        public virtual Double att_sub_hurtIncrease
        {
            get { return _att_sub_hurtIncrease; }
            set { if (OnPropertyChanging(__.att_sub_hurtIncrease, value)) { _att_sub_hurtIncrease = value; OnPropertyChanged(__.att_sub_hurtIncrease); } }
        }

        private Double _att_sub_hurtReduce;
        /// <summary>减伤</summary>
        [DisplayName("减伤")]
        [Description("减伤")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(22, "att_sub_hurtReduce", "减伤", "0", "float", 53, 0, false)]
        public virtual Double att_sub_hurtReduce
        {
            get { return _att_sub_hurtReduce; }
            set { if (OnPropertyChanging(__.att_sub_hurtReduce, value)) { _att_sub_hurtReduce = value; OnPropertyChanged(__.att_sub_hurtReduce); } }
        }

        private Double _att_crit_probability;
        /// <summary>会心几率</summary>
        [DisplayName("会心几率")]
        [Description("会心几率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(23, "att_crit_probability", "会心几率", "0", "float", 53, 0, false)]
        public virtual Double att_crit_probability
        {
            get { return _att_crit_probability; }
            set { if (OnPropertyChanging(__.att_crit_probability, value)) { _att_crit_probability = value; OnPropertyChanged(__.att_crit_probability); } }
        }

        private Double _att_crit_addition;
        /// <summary>会心效果</summary>
        [DisplayName("会心效果")]
        [Description("会心效果")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(24, "att_crit_addition", "会心效果", "0", "float", 53, 0, false)]
        public virtual Double att_crit_addition
        {
            get { return _att_crit_addition; }
            set { if (OnPropertyChanging(__.att_crit_addition, value)) { _att_crit_addition = value; OnPropertyChanged(__.att_crit_addition); } }
        }

        private Double _att_mystery_probability;
        /// <summary>奥义触发几率</summary>
        [DisplayName("奥义触发几率")]
        [Description("奥义触发几率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(25, "att_mystery_probability", "奥义触发几率", "0", "float", 53, 0, false)]
        public virtual Double att_mystery_probability
        {
            get { return _att_mystery_probability; }
            set { if (OnPropertyChanging(__.att_mystery_probability, value)) { _att_mystery_probability = value; OnPropertyChanged(__.att_mystery_probability); } }
        }

        private Double _att_dodge_probability;
        /// <summary>闪避几率</summary>
        [DisplayName("闪避几率")]
        [Description("闪避几率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(26, "att_dodge_probability", "闪避几率", "0", "float", 53, 0, false)]
        public virtual Double att_dodge_probability
        {
            get { return _att_dodge_probability; }
            set { if (OnPropertyChanging(__.att_dodge_probability, value)) { _att_dodge_probability = value; OnPropertyChanged(__.att_dodge_probability); } }
        }

        private Int64 _art_cheat_code;
        /// <summary>秘技</summary>
        [DisplayName("秘技")]
        [Description("秘技")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(27, "art_cheat_code", "秘技", "0", "bigint", 19, 0, false)]
        public virtual Int64 art_cheat_code
        {
            get { return _art_cheat_code; }
            set { if (OnPropertyChanging(__.art_cheat_code, value)) { _art_cheat_code = value; OnPropertyChanged(__.art_cheat_code); } }
        }

        private Int64 _art_mystery;
        /// <summary>奥义</summary>
        [DisplayName("奥义")]
        [Description("奥义")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(28, "art_mystery", "奥义", "0", "bigint", 19, 0, false)]
        public virtual Int64 art_mystery
        {
            get { return _art_mystery; }
            set { if (OnPropertyChanging(__.art_mystery, value)) { _art_mystery = value; OnPropertyChanged(__.art_mystery); } }
        }

        private Int64 _equip_weapon;
        /// <summary>武器(对应装备表主键编号)</summary>
        [DisplayName("武器对应装备表主键编号")]
        [Description("武器(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(29, "equip_weapon", "武器(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_weapon
        {
            get { return _equip_weapon; }
            set { if (OnPropertyChanging(__.equip_weapon, value)) { _equip_weapon = value; OnPropertyChanged(__.equip_weapon); } }
        }

        private Int64 _equip_barbarian;
        /// <summary>南蛮物(对应装备表主键编号)</summary>
        [DisplayName("南蛮物对应装备表主键编号")]
        [Description("南蛮物(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(30, "equip_barbarian", "南蛮物(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_barbarian
        {
            get { return _equip_barbarian; }
            set { if (OnPropertyChanging(__.equip_barbarian, value)) { _equip_barbarian = value; OnPropertyChanged(__.equip_barbarian); } }
        }

        private Int64 _equip_mounts;
        /// <summary>坐骑(对应装备表主键编号)</summary>
        [DisplayName("坐骑对应装备表主键编号")]
        [Description("坐骑(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(31, "equip_mounts", "坐骑(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_mounts
        {
            get { return _equip_mounts; }
            set { if (OnPropertyChanging(__.equip_mounts, value)) { _equip_mounts = value; OnPropertyChanged(__.equip_mounts); } }
        }

        private Int64 _equip_armor;
        /// <summary>铠甲(对应装备表主键编号)</summary>
        [DisplayName("铠甲对应装备表主键编号")]
        [Description("铠甲(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(32, "equip_armor", "铠甲(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_armor
        {
            get { return _equip_armor; }
            set { if (OnPropertyChanging(__.equip_armor, value)) { _equip_armor = value; OnPropertyChanged(__.equip_armor); } }
        }

        private Int64 _equip_gem;
        /// <summary>宝石(对应装备表主键编号)</summary>
        [DisplayName("宝石对应装备表主键编号")]
        [Description("宝石(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(33, "equip_gem", "宝石(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_gem
        {
            get { return _equip_gem; }
            set { if (OnPropertyChanging(__.equip_gem, value)) { _equip_gem = value; OnPropertyChanged(__.equip_gem); } }
        }

        private Int64 _equip_tea;
        /// <summary>茶器(对应装备表主键编号)</summary>
        [DisplayName("茶器对应装备表主键编号")]
        [Description("茶器(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(34, "equip_tea", "茶器(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_tea
        {
            get { return _equip_tea; }
            set { if (OnPropertyChanging(__.equip_tea, value)) { _equip_tea = value; OnPropertyChanged(__.equip_tea); } }
        }

        private Int64 _equip_craft;
        /// <summary>艺术品(对应装备表主键编号)</summary>
        [DisplayName("艺术品对应装备表主键编号")]
        [Description("艺术品(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(35, "equip_craft", "艺术品(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_craft
        {
            get { return _equip_craft; }
            set { if (OnPropertyChanging(__.equip_craft, value)) { _equip_craft = value; OnPropertyChanged(__.equip_craft); } }
        }

        private Int64 _equip_book;
        /// <summary>书籍(对应装备表主键编号)</summary>
        [DisplayName("书籍对应装备表主键编号")]
        [Description("书籍(对应装备表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(36, "equip_book", "书籍(对应装备表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 equip_book
        {
            get { return _equip_book; }
            set { if (OnPropertyChanging(__.equip_book, value)) { _equip_book = value; OnPropertyChanged(__.equip_book); } }
        }

        private Double _base_captain_life;
        /// <summary>统帅(生活技能加点)</summary>
        [DisplayName("统帅生活技能加点")]
        [Description("统帅(生活技能加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(37, "base_captain_life", "统帅(生活技能加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_life
        {
            get { return _base_captain_life; }
            set { if (OnPropertyChanging(__.base_captain_life, value)) { _base_captain_life = value; OnPropertyChanged(__.base_captain_life); } }
        }

        private Double _base_captain_train;
        /// <summary>统帅(锻炼加点)</summary>
        [DisplayName("统帅锻炼加点")]
        [Description("统帅(锻炼加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(38, "base_captain_train", "统帅(锻炼加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_train
        {
            get { return _base_captain_train; }
            set { if (OnPropertyChanging(__.base_captain_train, value)) { _base_captain_train = value; OnPropertyChanged(__.base_captain_train); } }
        }

        private Double _base_captain_level;
        /// <summary>统帅(升级加点)</summary>
        [DisplayName("统帅升级加点")]
        [Description("统帅(升级加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(39, "base_captain_level", "统帅(升级加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_level
        {
            get { return _base_captain_level; }
            set { if (OnPropertyChanging(__.base_captain_level, value)) { _base_captain_level = value; OnPropertyChanged(__.base_captain_level); } }
        }

        private Double _base_captain_spirit;
        /// <summary>统帅(铸魂加点)</summary>
        [DisplayName("统帅铸魂加点")]
        [Description("统帅(铸魂加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(40, "base_captain_spirit", "统帅(铸魂加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_spirit
        {
            get { return _base_captain_spirit; }
            set { if (OnPropertyChanging(__.base_captain_spirit, value)) { _base_captain_spirit = value; OnPropertyChanged(__.base_captain_spirit); } }
        }

        private Double _base_captain_equip;
        /// <summary>统帅(装备加点)</summary>
        [DisplayName("统帅装备加点")]
        [Description("统帅(装备加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(41, "base_captain_equip", "统帅(装备加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_equip
        {
            get { return _base_captain_equip; }
            set { if (OnPropertyChanging(__.base_captain_equip, value)) { _base_captain_equip = value; OnPropertyChanged(__.base_captain_equip); } }
        }

        private Double _base_captain_title;
        /// <summary>统帅(称号加点)</summary>
        [DisplayName("统帅称号加点")]
        [Description("统帅(称号加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(42, "base_captain_title", "统帅(称号加点)", "0", "float", 53, 0, false)]
        public virtual Double base_captain_title
        {
            get { return _base_captain_title; }
            set { if (OnPropertyChanging(__.base_captain_title, value)) { _base_captain_title = value; OnPropertyChanged(__.base_captain_title); } }
        }

        private Double _base_force_life;
        /// <summary>武力(生活技能加点)</summary>
        [DisplayName("武力生活技能加点")]
        [Description("武力(生活技能加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(43, "base_force_life", "武力(生活技能加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_life
        {
            get { return _base_force_life; }
            set { if (OnPropertyChanging(__.base_force_life, value)) { _base_force_life = value; OnPropertyChanged(__.base_force_life); } }
        }

        private Double _base_force_train;
        /// <summary>武力(锻炼加点)</summary>
        [DisplayName("武力锻炼加点")]
        [Description("武力(锻炼加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(44, "base_force_train", "武力(锻炼加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_train
        {
            get { return _base_force_train; }
            set { if (OnPropertyChanging(__.base_force_train, value)) { _base_force_train = value; OnPropertyChanged(__.base_force_train); } }
        }

        private Double _base_force_level;
        /// <summary>武力(升级加点)</summary>
        [DisplayName("武力升级加点")]
        [Description("武力(升级加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(45, "base_force_level", "武力(升级加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_level
        {
            get { return _base_force_level; }
            set { if (OnPropertyChanging(__.base_force_level, value)) { _base_force_level = value; OnPropertyChanged(__.base_force_level); } }
        }

        private Double _base_force_spirit;
        /// <summary>武力(铸魂加点)</summary>
        [DisplayName("武力铸魂加点")]
        [Description("武力(铸魂加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(46, "base_force_spirit", "武力(铸魂加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_spirit
        {
            get { return _base_force_spirit; }
            set { if (OnPropertyChanging(__.base_force_spirit, value)) { _base_force_spirit = value; OnPropertyChanged(__.base_force_spirit); } }
        }

        private Double _base_force_equip;
        /// <summary>武力(装备加点)</summary>
        [DisplayName("武力装备加点")]
        [Description("武力(装备加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(47, "base_force_equip", "武力(装备加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_equip
        {
            get { return _base_force_equip; }
            set { if (OnPropertyChanging(__.base_force_equip, value)) { _base_force_equip = value; OnPropertyChanged(__.base_force_equip); } }
        }

        private Double _base_force_title;
        /// <summary>武力(称号加点)</summary>
        [DisplayName("武力称号加点")]
        [Description("武力(称号加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(48, "base_force_title", "武力(称号加点)", "0", "float", 53, 0, false)]
        public virtual Double base_force_title
        {
            get { return _base_force_title; }
            set { if (OnPropertyChanging(__.base_force_title, value)) { _base_force_title = value; OnPropertyChanged(__.base_force_title); } }
        }

        private Double _base_brains_life;
        /// <summary>智谋(生活技能加点)</summary>
        [DisplayName("智谋生活技能加点")]
        [Description("智谋(生活技能加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(49, "base_brains_life", "智谋(生活技能加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_life
        {
            get { return _base_brains_life; }
            set { if (OnPropertyChanging(__.base_brains_life, value)) { _base_brains_life = value; OnPropertyChanged(__.base_brains_life); } }
        }

        private Double _base_brains_train;
        /// <summary>智谋(锻炼加点)</summary>
        [DisplayName("智谋锻炼加点")]
        [Description("智谋(锻炼加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(50, "base_brains_train", "智谋(锻炼加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_train
        {
            get { return _base_brains_train; }
            set { if (OnPropertyChanging(__.base_brains_train, value)) { _base_brains_train = value; OnPropertyChanged(__.base_brains_train); } }
        }

        private Double _base_brains_level;
        /// <summary>智谋(升级加点)</summary>
        [DisplayName("智谋升级加点")]
        [Description("智谋(升级加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(51, "base_brains_level", "智谋(升级加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_level
        {
            get { return _base_brains_level; }
            set { if (OnPropertyChanging(__.base_brains_level, value)) { _base_brains_level = value; OnPropertyChanged(__.base_brains_level); } }
        }

        private Double _base_brains_spirit;
        /// <summary>智谋(铸魂加点)</summary>
        [DisplayName("智谋铸魂加点")]
        [Description("智谋(铸魂加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(52, "base_brains_spirit", "智谋(铸魂加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_spirit
        {
            get { return _base_brains_spirit; }
            set { if (OnPropertyChanging(__.base_brains_spirit, value)) { _base_brains_spirit = value; OnPropertyChanged(__.base_brains_spirit); } }
        }

        private Double _base_brains_equip;
        /// <summary>智谋(装备加点)</summary>
        [DisplayName("智谋装备加点")]
        [Description("智谋(装备加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(53, "base_brains_equip", "智谋(装备加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_equip
        {
            get { return _base_brains_equip; }
            set { if (OnPropertyChanging(__.base_brains_equip, value)) { _base_brains_equip = value; OnPropertyChanged(__.base_brains_equip); } }
        }

        private Double _base_brains_title;
        /// <summary>智谋(称号加点)</summary>
        [DisplayName("智谋称号加点")]
        [Description("智谋(称号加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(54, "base_brains_title", "智谋(称号加点)", "0", "float", 53, 0, false)]
        public virtual Double base_brains_title
        {
            get { return _base_brains_title; }
            set { if (OnPropertyChanging(__.base_brains_title, value)) { _base_brains_title = value; OnPropertyChanged(__.base_brains_title); } }
        }

        private Double _base_charm_life;
        /// <summary>魅力(生活技能加点)</summary>
        [DisplayName("魅力生活技能加点")]
        [Description("魅力(生活技能加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(55, "base_charm_life", "魅力(生活技能加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_life
        {
            get { return _base_charm_life; }
            set { if (OnPropertyChanging(__.base_charm_life, value)) { _base_charm_life = value; OnPropertyChanged(__.base_charm_life); } }
        }

        private Double _base_charm_train;
        /// <summary>魅力(锻炼加点)</summary>
        [DisplayName("魅力锻炼加点")]
        [Description("魅力(锻炼加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(56, "base_charm_train", "魅力(锻炼加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_train
        {
            get { return _base_charm_train; }
            set { if (OnPropertyChanging(__.base_charm_train, value)) { _base_charm_train = value; OnPropertyChanged(__.base_charm_train); } }
        }

        private Double _base_charm_level;
        /// <summary>魅力(升级加点)</summary>
        [DisplayName("魅力升级加点")]
        [Description("魅力(升级加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(57, "base_charm_level", "魅力(升级加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_level
        {
            get { return _base_charm_level; }
            set { if (OnPropertyChanging(__.base_charm_level, value)) { _base_charm_level = value; OnPropertyChanged(__.base_charm_level); } }
        }

        private Double _base_charm_spirit;
        /// <summary>魅力(铸魂加点)</summary>
        [DisplayName("魅力铸魂加点")]
        [Description("魅力(铸魂加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(58, "base_charm_spirit", "魅力(铸魂加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_spirit
        {
            get { return _base_charm_spirit; }
            set { if (OnPropertyChanging(__.base_charm_spirit, value)) { _base_charm_spirit = value; OnPropertyChanged(__.base_charm_spirit); } }
        }

        private Double _base_charm_equip;
        /// <summary>魅力(装备加点)</summary>
        [DisplayName("魅力装备加点")]
        [Description("魅力(装备加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(59, "base_charm_equip", "魅力(装备加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_equip
        {
            get { return _base_charm_equip; }
            set { if (OnPropertyChanging(__.base_charm_equip, value)) { _base_charm_equip = value; OnPropertyChanged(__.base_charm_equip); } }
        }

        private Double _base_charm_title;
        /// <summary>魅力(称号加点)</summary>
        [DisplayName("魅力称号加点")]
        [Description("魅力(称号加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(60, "base_charm_title", "魅力(称号加点)", "0", "float", 53, 0, false)]
        public virtual Double base_charm_title
        {
            get { return _base_charm_title; }
            set { if (OnPropertyChanging(__.base_charm_title, value)) { _base_charm_title = value; OnPropertyChanged(__.base_charm_title); } }
        }

        private Double _base_govern_life;
        /// <summary>政务(生活技能加点)</summary>
        [DisplayName("政务生活技能加点")]
        [Description("政务(生活技能加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(61, "base_govern_life", "政务(生活技能加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_life
        {
            get { return _base_govern_life; }
            set { if (OnPropertyChanging(__.base_govern_life, value)) { _base_govern_life = value; OnPropertyChanged(__.base_govern_life); } }
        }

        private Double _base_govern_train;
        /// <summary>政务(锻炼加点)</summary>
        [DisplayName("政务锻炼加点")]
        [Description("政务(锻炼加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(62, "base_govern_train", "政务(锻炼加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_train
        {
            get { return _base_govern_train; }
            set { if (OnPropertyChanging(__.base_govern_train, value)) { _base_govern_train = value; OnPropertyChanged(__.base_govern_train); } }
        }

        private Double _base_govern_level;
        /// <summary>政务(升级加点)</summary>
        [DisplayName("政务升级加点")]
        [Description("政务(升级加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(63, "base_govern_level", "政务(升级加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_level
        {
            get { return _base_govern_level; }
            set { if (OnPropertyChanging(__.base_govern_level, value)) { _base_govern_level = value; OnPropertyChanged(__.base_govern_level); } }
        }

        private Double _base_govern_spirit;
        /// <summary>政务(铸魂加点)</summary>
        [DisplayName("政务铸魂加点")]
        [Description("政务(铸魂加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(64, "base_govern_spirit", "政务(铸魂加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_spirit
        {
            get { return _base_govern_spirit; }
            set { if (OnPropertyChanging(__.base_govern_spirit, value)) { _base_govern_spirit = value; OnPropertyChanged(__.base_govern_spirit); } }
        }

        private Double _base_govern_equip;
        /// <summary>政务(装备加点)</summary>
        [DisplayName("政务装备加点")]
        [Description("政务(装备加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(65, "base_govern_equip", "政务(装备加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_equip
        {
            get { return _base_govern_equip; }
            set { if (OnPropertyChanging(__.base_govern_equip, value)) { _base_govern_equip = value; OnPropertyChanged(__.base_govern_equip); } }
        }

        private Double _base_govern_title;
        /// <summary>政务(称号加点)</summary>
        [DisplayName("政务称号加点")]
        [Description("政务(称号加点)")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(66, "base_govern_title", "政务(称号加点)", "0", "float", 53, 0, false)]
        public virtual Double base_govern_title
        {
            get { return _base_govern_title; }
            set { if (OnPropertyChanging(__.base_govern_title, value)) { _base_govern_title = value; OnPropertyChanged(__.base_govern_title); } }
        }

        private Int64 _title_sword;
        /// <summary>剑称号(对应家臣称号表主键编号)</summary>
        [DisplayName("剑称号对应家臣称号表主键编号")]
        [Description("剑称号(对应家臣称号表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(67, "title_sword", "剑称号(对应家臣称号表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 title_sword
        {
            get { return _title_sword; }
            set { if (OnPropertyChanging(__.title_sword, value)) { _title_sword = value; OnPropertyChanged(__.title_sword); } }
        }

        private Int64 _title_gun;
        /// <summary>枪称号(对应家臣称号表主键编号)</summary>
        [DisplayName("枪称号对应家臣称号表主键编号")]
        [Description("枪称号(对应家臣称号表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(68, "title_gun", "枪称号(对应家臣称号表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 title_gun
        {
            get { return _title_gun; }
            set { if (OnPropertyChanging(__.title_gun, value)) { _title_gun = value; OnPropertyChanged(__.title_gun); } }
        }

        private Int64 _title_tea;
        /// <summary>茶道称号(对应家臣称号表主键编号)</summary>
        [DisplayName("茶道称号对应家臣称号表主键编号")]
        [Description("茶道称号(对应家臣称号表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(69, "title_tea", "茶道称号(对应家臣称号表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 title_tea
        {
            get { return _title_tea; }
            set { if (OnPropertyChanging(__.title_tea, value)) { _title_tea = value; OnPropertyChanged(__.title_tea); } }
        }

        private Int64 _title_eloquence;
        /// <summary>讲价称号(对应家臣称号表主键编号)</summary>
        [DisplayName("讲价称号对应家臣称号表主键编号")]
        [Description("讲价称号(对应家臣称号表主键编号)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(70, "title_eloquence", "讲价称号(对应家臣称号表主键编号)", "0", "bigint", 19, 0, false)]
        public virtual Int64 title_eloquence
        {
            get { return _title_eloquence; }
            set { if (OnPropertyChanging(__.title_eloquence, value)) { _title_eloquence = value; OnPropertyChanged(__.title_eloquence); } }
        }

        private Int32 _buff_power;
        /// <summary>体力buff</summary>
        [DisplayName("体力buff")]
        [Description("体力buff")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(71, "buff_power", "体力buff", "0", "int", 10, 0, false)]
        public virtual Int32 buff_power
        {
            get { return _buff_power; }
            set { if (OnPropertyChanging(__.buff_power, value)) { _buff_power = value; OnPropertyChanged(__.buff_power); } }
        }

        private Int32 _total_honor;
        /// <summary>总功勋值</summary>
        [DisplayName("总功勋值")]
        [Description("总功勋值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(72, "total_honor", "总功勋值", "0", "int", 10, 0, false)]
        public virtual Int32 total_honor
        {
            get { return _total_honor; }
            set { if (OnPropertyChanging(__.total_honor, value)) { _total_honor = value; OnPropertyChanged(__.total_honor); } }
        }

        private Int32 _total_exp;
        /// <summary></summary>
        [DisplayName("Exp1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(73, "total_exp", "", "0", "int", 10, 0, false)]
        public virtual Int32 total_exp
        {
            get { return _total_exp; }
            set { if (OnPropertyChanging(__.total_exp, value)) { _total_exp = value; OnPropertyChanged(__.total_exp); } }
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
                    case __.role_id : return _role_id;
                    case __.power : return _power;
                    case __.role_genre : return _role_genre;
                    case __.role_ninja : return _role_ninja;
                    case __.role_level : return _role_level;
                    case __.role_state : return _role_state;
                    case __.role_exp : return _role_exp;
                    case __.role_honor : return _role_honor;
                    case __.role_identity : return _role_identity;
                    case __.base_captain : return _base_captain;
                    case __.base_force : return _base_force;
                    case __.base_brains : return _base_brains;
                    case __.base_charm : return _base_charm;
                    case __.base_govern : return _base_govern;
                    case __.att_points : return _att_points;
                    case __.att_life : return _att_life;
                    case __.att_attack : return _att_attack;
                    case __.att_defense : return _att_defense;
                    case __.att_sub_hurtIncrease : return _att_sub_hurtIncrease;
                    case __.att_sub_hurtReduce : return _att_sub_hurtReduce;
                    case __.att_crit_probability : return _att_crit_probability;
                    case __.att_crit_addition : return _att_crit_addition;
                    case __.att_mystery_probability : return _att_mystery_probability;
                    case __.att_dodge_probability : return _att_dodge_probability;
                    case __.art_cheat_code : return _art_cheat_code;
                    case __.art_mystery : return _art_mystery;
                    case __.equip_weapon : return _equip_weapon;
                    case __.equip_barbarian : return _equip_barbarian;
                    case __.equip_mounts : return _equip_mounts;
                    case __.equip_armor : return _equip_armor;
                    case __.equip_gem : return _equip_gem;
                    case __.equip_tea : return _equip_tea;
                    case __.equip_craft : return _equip_craft;
                    case __.equip_book : return _equip_book;
                    case __.base_captain_life : return _base_captain_life;
                    case __.base_captain_train : return _base_captain_train;
                    case __.base_captain_level : return _base_captain_level;
                    case __.base_captain_spirit : return _base_captain_spirit;
                    case __.base_captain_equip : return _base_captain_equip;
                    case __.base_captain_title : return _base_captain_title;
                    case __.base_force_life : return _base_force_life;
                    case __.base_force_train : return _base_force_train;
                    case __.base_force_level : return _base_force_level;
                    case __.base_force_spirit : return _base_force_spirit;
                    case __.base_force_equip : return _base_force_equip;
                    case __.base_force_title : return _base_force_title;
                    case __.base_brains_life : return _base_brains_life;
                    case __.base_brains_train : return _base_brains_train;
                    case __.base_brains_level : return _base_brains_level;
                    case __.base_brains_spirit : return _base_brains_spirit;
                    case __.base_brains_equip : return _base_brains_equip;
                    case __.base_brains_title : return _base_brains_title;
                    case __.base_charm_life : return _base_charm_life;
                    case __.base_charm_train : return _base_charm_train;
                    case __.base_charm_level : return _base_charm_level;
                    case __.base_charm_spirit : return _base_charm_spirit;
                    case __.base_charm_equip : return _base_charm_equip;
                    case __.base_charm_title : return _base_charm_title;
                    case __.base_govern_life : return _base_govern_life;
                    case __.base_govern_train : return _base_govern_train;
                    case __.base_govern_level : return _base_govern_level;
                    case __.base_govern_spirit : return _base_govern_spirit;
                    case __.base_govern_equip : return _base_govern_equip;
                    case __.base_govern_title : return _base_govern_title;
                    case __.title_sword : return _title_sword;
                    case __.title_gun : return _title_gun;
                    case __.title_tea : return _title_tea;
                    case __.title_eloquence : return _title_eloquence;
                    case __.buff_power : return _buff_power;
                    case __.total_honor : return _total_honor;
                    case __.total_exp : return _total_exp;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.power : _power = Convert.ToInt32(value); break;
                    case __.role_genre : _role_genre = Convert.ToInt32(value); break;
                    case __.role_ninja : _role_ninja = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.role_state : _role_state = Convert.ToInt32(value); break;
                    case __.role_exp : _role_exp = Convert.ToInt32(value); break;
                    case __.role_honor : _role_honor = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    case __.base_captain : _base_captain = Convert.ToDouble(value); break;
                    case __.base_force : _base_force = Convert.ToDouble(value); break;
                    case __.base_brains : _base_brains = Convert.ToDouble(value); break;
                    case __.base_charm : _base_charm = Convert.ToDouble(value); break;
                    case __.base_govern : _base_govern = Convert.ToDouble(value); break;
                    case __.att_points : _att_points = Convert.ToInt32(value); break;
                    case __.att_life : _att_life = Convert.ToInt32(value); break;
                    case __.att_attack : _att_attack = Convert.ToDouble(value); break;
                    case __.att_defense : _att_defense = Convert.ToDouble(value); break;
                    case __.att_sub_hurtIncrease : _att_sub_hurtIncrease = Convert.ToDouble(value); break;
                    case __.att_sub_hurtReduce : _att_sub_hurtReduce = Convert.ToDouble(value); break;
                    case __.att_crit_probability : _att_crit_probability = Convert.ToDouble(value); break;
                    case __.att_crit_addition : _att_crit_addition = Convert.ToDouble(value); break;
                    case __.att_mystery_probability : _att_mystery_probability = Convert.ToDouble(value); break;
                    case __.att_dodge_probability : _att_dodge_probability = Convert.ToDouble(value); break;
                    case __.art_cheat_code : _art_cheat_code = Convert.ToInt64(value); break;
                    case __.art_mystery : _art_mystery = Convert.ToInt64(value); break;
                    case __.equip_weapon : _equip_weapon = Convert.ToInt64(value); break;
                    case __.equip_barbarian : _equip_barbarian = Convert.ToInt64(value); break;
                    case __.equip_mounts : _equip_mounts = Convert.ToInt64(value); break;
                    case __.equip_armor : _equip_armor = Convert.ToInt64(value); break;
                    case __.equip_gem : _equip_gem = Convert.ToInt64(value); break;
                    case __.equip_tea : _equip_tea = Convert.ToInt64(value); break;
                    case __.equip_craft : _equip_craft = Convert.ToInt64(value); break;
                    case __.equip_book : _equip_book = Convert.ToInt64(value); break;
                    case __.base_captain_life : _base_captain_life = Convert.ToDouble(value); break;
                    case __.base_captain_train : _base_captain_train = Convert.ToDouble(value); break;
                    case __.base_captain_level : _base_captain_level = Convert.ToDouble(value); break;
                    case __.base_captain_spirit : _base_captain_spirit = Convert.ToDouble(value); break;
                    case __.base_captain_equip : _base_captain_equip = Convert.ToDouble(value); break;
                    case __.base_captain_title : _base_captain_title = Convert.ToDouble(value); break;
                    case __.base_force_life : _base_force_life = Convert.ToDouble(value); break;
                    case __.base_force_train : _base_force_train = Convert.ToDouble(value); break;
                    case __.base_force_level : _base_force_level = Convert.ToDouble(value); break;
                    case __.base_force_spirit : _base_force_spirit = Convert.ToDouble(value); break;
                    case __.base_force_equip : _base_force_equip = Convert.ToDouble(value); break;
                    case __.base_force_title : _base_force_title = Convert.ToDouble(value); break;
                    case __.base_brains_life : _base_brains_life = Convert.ToDouble(value); break;
                    case __.base_brains_train : _base_brains_train = Convert.ToDouble(value); break;
                    case __.base_brains_level : _base_brains_level = Convert.ToDouble(value); break;
                    case __.base_brains_spirit : _base_brains_spirit = Convert.ToDouble(value); break;
                    case __.base_brains_equip : _base_brains_equip = Convert.ToDouble(value); break;
                    case __.base_brains_title : _base_brains_title = Convert.ToDouble(value); break;
                    case __.base_charm_life : _base_charm_life = Convert.ToDouble(value); break;
                    case __.base_charm_train : _base_charm_train = Convert.ToDouble(value); break;
                    case __.base_charm_level : _base_charm_level = Convert.ToDouble(value); break;
                    case __.base_charm_spirit : _base_charm_spirit = Convert.ToDouble(value); break;
                    case __.base_charm_equip : _base_charm_equip = Convert.ToDouble(value); break;
                    case __.base_charm_title : _base_charm_title = Convert.ToDouble(value); break;
                    case __.base_govern_life : _base_govern_life = Convert.ToDouble(value); break;
                    case __.base_govern_train : _base_govern_train = Convert.ToDouble(value); break;
                    case __.base_govern_level : _base_govern_level = Convert.ToDouble(value); break;
                    case __.base_govern_spirit : _base_govern_spirit = Convert.ToDouble(value); break;
                    case __.base_govern_equip : _base_govern_equip = Convert.ToDouble(value); break;
                    case __.base_govern_title : _base_govern_title = Convert.ToDouble(value); break;
                    case __.title_sword : _title_sword = Convert.ToInt64(value); break;
                    case __.title_gun : _title_gun = Convert.ToInt64(value); break;
                    case __.title_tea : _title_tea = Convert.ToInt64(value); break;
                    case __.title_eloquence : _title_eloquence = Convert.ToInt64(value); break;
                    case __.buff_power : _buff_power = Convert.ToInt32(value); break;
                    case __.total_honor : _total_honor = Convert.ToInt32(value); break;
                    case __.total_exp : _total_exp = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得武将表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>武将基表编号</summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary>体力</summary>
            public static readonly Field power = FindByName(__.power);

            ///<summary>武将默认流派</summary>
            public static readonly Field role_genre = FindByName(__.role_genre);

            ///<summary>武将默认忍者冢</summary>
            public static readonly Field role_ninja = FindByName(__.role_ninja);

            ///<summary>武将等级</summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary>武将状态</summary>
            public static readonly Field role_state = FindByName(__.role_state);

            ///<summary>武将经验</summary>
            public static readonly Field role_exp = FindByName(__.role_exp);

            ///<summary>武将功勋</summary>
            public static readonly Field role_honor = FindByName(__.role_honor);

            ///<summary>身份</summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary>统帅(初始值)</summary>
            public static readonly Field base_captain = FindByName(__.base_captain);

            ///<summary>武力(初始值)</summary>
            public static readonly Field base_force = FindByName(__.base_force);

            ///<summary>智谋(初始值)</summary>
            public static readonly Field base_brains = FindByName(__.base_brains);

            ///<summary>魅力(初始值)</summary>
            public static readonly Field base_charm = FindByName(__.base_charm);

            ///<summary>政务(初始值)</summary>
            public static readonly Field base_govern = FindByName(__.base_govern);

            ///<summary>武将剩余属性点</summary>
            public static readonly Field att_points = FindByName(__.att_points);

            ///<summary>生命</summary>
            public static readonly Field att_life = FindByName(__.att_life);

            ///<summary>攻击</summary>
            public static readonly Field att_attack = FindByName(__.att_attack);

            ///<summary>防御</summary>
            public static readonly Field att_defense = FindByName(__.att_defense);

            ///<summary>增伤</summary>
            public static readonly Field att_sub_hurtIncrease = FindByName(__.att_sub_hurtIncrease);

            ///<summary>减伤</summary>
            public static readonly Field att_sub_hurtReduce = FindByName(__.att_sub_hurtReduce);

            ///<summary>会心几率</summary>
            public static readonly Field att_crit_probability = FindByName(__.att_crit_probability);

            ///<summary>会心效果</summary>
            public static readonly Field att_crit_addition = FindByName(__.att_crit_addition);

            ///<summary>奥义触发几率</summary>
            public static readonly Field att_mystery_probability = FindByName(__.att_mystery_probability);

            ///<summary>闪避几率</summary>
            public static readonly Field att_dodge_probability = FindByName(__.att_dodge_probability);

            ///<summary>秘技</summary>
            public static readonly Field art_cheat_code = FindByName(__.art_cheat_code);

            ///<summary>奥义</summary>
            public static readonly Field art_mystery = FindByName(__.art_mystery);

            ///<summary>武器(对应装备表主键编号)</summary>
            public static readonly Field equip_weapon = FindByName(__.equip_weapon);

            ///<summary>南蛮物(对应装备表主键编号)</summary>
            public static readonly Field equip_barbarian = FindByName(__.equip_barbarian);

            ///<summary>坐骑(对应装备表主键编号)</summary>
            public static readonly Field equip_mounts = FindByName(__.equip_mounts);

            ///<summary>铠甲(对应装备表主键编号)</summary>
            public static readonly Field equip_armor = FindByName(__.equip_armor);

            ///<summary>宝石(对应装备表主键编号)</summary>
            public static readonly Field equip_gem = FindByName(__.equip_gem);

            ///<summary>茶器(对应装备表主键编号)</summary>
            public static readonly Field equip_tea = FindByName(__.equip_tea);

            ///<summary>艺术品(对应装备表主键编号)</summary>
            public static readonly Field equip_craft = FindByName(__.equip_craft);

            ///<summary>书籍(对应装备表主键编号)</summary>
            public static readonly Field equip_book = FindByName(__.equip_book);

            ///<summary>统帅(生活技能加点)</summary>
            public static readonly Field base_captain_life = FindByName(__.base_captain_life);

            ///<summary>统帅(锻炼加点)</summary>
            public static readonly Field base_captain_train = FindByName(__.base_captain_train);

            ///<summary>统帅(升级加点)</summary>
            public static readonly Field base_captain_level = FindByName(__.base_captain_level);

            ///<summary>统帅(铸魂加点)</summary>
            public static readonly Field base_captain_spirit = FindByName(__.base_captain_spirit);

            ///<summary>统帅(装备加点)</summary>
            public static readonly Field base_captain_equip = FindByName(__.base_captain_equip);

            ///<summary>统帅(称号加点)</summary>
            public static readonly Field base_captain_title = FindByName(__.base_captain_title);

            ///<summary>武力(生活技能加点)</summary>
            public static readonly Field base_force_life = FindByName(__.base_force_life);

            ///<summary>武力(锻炼加点)</summary>
            public static readonly Field base_force_train = FindByName(__.base_force_train);

            ///<summary>武力(升级加点)</summary>
            public static readonly Field base_force_level = FindByName(__.base_force_level);

            ///<summary>武力(铸魂加点)</summary>
            public static readonly Field base_force_spirit = FindByName(__.base_force_spirit);

            ///<summary>武力(装备加点)</summary>
            public static readonly Field base_force_equip = FindByName(__.base_force_equip);

            ///<summary>武力(称号加点)</summary>
            public static readonly Field base_force_title = FindByName(__.base_force_title);

            ///<summary>智谋(生活技能加点)</summary>
            public static readonly Field base_brains_life = FindByName(__.base_brains_life);

            ///<summary>智谋(锻炼加点)</summary>
            public static readonly Field base_brains_train = FindByName(__.base_brains_train);

            ///<summary>智谋(升级加点)</summary>
            public static readonly Field base_brains_level = FindByName(__.base_brains_level);

            ///<summary>智谋(铸魂加点)</summary>
            public static readonly Field base_brains_spirit = FindByName(__.base_brains_spirit);

            ///<summary>智谋(装备加点)</summary>
            public static readonly Field base_brains_equip = FindByName(__.base_brains_equip);

            ///<summary>智谋(称号加点)</summary>
            public static readonly Field base_brains_title = FindByName(__.base_brains_title);

            ///<summary>魅力(生活技能加点)</summary>
            public static readonly Field base_charm_life = FindByName(__.base_charm_life);

            ///<summary>魅力(锻炼加点)</summary>
            public static readonly Field base_charm_train = FindByName(__.base_charm_train);

            ///<summary>魅力(升级加点)</summary>
            public static readonly Field base_charm_level = FindByName(__.base_charm_level);

            ///<summary>魅力(铸魂加点)</summary>
            public static readonly Field base_charm_spirit = FindByName(__.base_charm_spirit);

            ///<summary>魅力(装备加点)</summary>
            public static readonly Field base_charm_equip = FindByName(__.base_charm_equip);

            ///<summary>魅力(称号加点)</summary>
            public static readonly Field base_charm_title = FindByName(__.base_charm_title);

            ///<summary>政务(生活技能加点)</summary>
            public static readonly Field base_govern_life = FindByName(__.base_govern_life);

            ///<summary>政务(锻炼加点)</summary>
            public static readonly Field base_govern_train = FindByName(__.base_govern_train);

            ///<summary>政务(升级加点)</summary>
            public static readonly Field base_govern_level = FindByName(__.base_govern_level);

            ///<summary>政务(铸魂加点)</summary>
            public static readonly Field base_govern_spirit = FindByName(__.base_govern_spirit);

            ///<summary>政务(装备加点)</summary>
            public static readonly Field base_govern_equip = FindByName(__.base_govern_equip);

            ///<summary>政务(称号加点)</summary>
            public static readonly Field base_govern_title = FindByName(__.base_govern_title);

            ///<summary>剑称号(对应家臣称号表主键编号)</summary>
            public static readonly Field title_sword = FindByName(__.title_sword);

            ///<summary>枪称号(对应家臣称号表主键编号)</summary>
            public static readonly Field title_gun = FindByName(__.title_gun);

            ///<summary>茶道称号(对应家臣称号表主键编号)</summary>
            public static readonly Field title_tea = FindByName(__.title_tea);

            ///<summary>讲价称号(对应家臣称号表主键编号)</summary>
            public static readonly Field title_eloquence = FindByName(__.title_eloquence);

            ///<summary>体力buff</summary>
            public static readonly Field buff_power = FindByName(__.buff_power);

            ///<summary>总功勋值</summary>
            public static readonly Field total_honor = FindByName(__.total_honor);

            ///<summary></summary>
            public static readonly Field total_exp = FindByName(__.total_exp);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得武将表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>武将基表编号</summary>
            public const String role_id = "role_id";

            ///<summary>体力</summary>
            public const String power = "power";

            ///<summary>武将默认流派</summary>
            public const String role_genre = "role_genre";

            ///<summary>武将默认忍者冢</summary>
            public const String role_ninja = "role_ninja";

            ///<summary>武将等级</summary>
            public const String role_level = "role_level";

            ///<summary>武将状态</summary>
            public const String role_state = "role_state";

            ///<summary>武将经验</summary>
            public const String role_exp = "role_exp";

            ///<summary>武将功勋</summary>
            public const String role_honor = "role_honor";

            ///<summary>身份</summary>
            public const String role_identity = "role_identity";

            ///<summary>统帅(初始值)</summary>
            public const String base_captain = "base_captain";

            ///<summary>武力(初始值)</summary>
            public const String base_force = "base_force";

            ///<summary>智谋(初始值)</summary>
            public const String base_brains = "base_brains";

            ///<summary>魅力(初始值)</summary>
            public const String base_charm = "base_charm";

            ///<summary>政务(初始值)</summary>
            public const String base_govern = "base_govern";

            ///<summary>武将剩余属性点</summary>
            public const String att_points = "att_points";

            ///<summary>生命</summary>
            public const String att_life = "att_life";

            ///<summary>攻击</summary>
            public const String att_attack = "att_attack";

            ///<summary>防御</summary>
            public const String att_defense = "att_defense";

            ///<summary>增伤</summary>
            public const String att_sub_hurtIncrease = "att_sub_hurtIncrease";

            ///<summary>减伤</summary>
            public const String att_sub_hurtReduce = "att_sub_hurtReduce";

            ///<summary>会心几率</summary>
            public const String att_crit_probability = "att_crit_probability";

            ///<summary>会心效果</summary>
            public const String att_crit_addition = "att_crit_addition";

            ///<summary>奥义触发几率</summary>
            public const String att_mystery_probability = "att_mystery_probability";

            ///<summary>闪避几率</summary>
            public const String att_dodge_probability = "att_dodge_probability";

            ///<summary>秘技</summary>
            public const String art_cheat_code = "art_cheat_code";

            ///<summary>奥义</summary>
            public const String art_mystery = "art_mystery";

            ///<summary>武器(对应装备表主键编号)</summary>
            public const String equip_weapon = "equip_weapon";

            ///<summary>南蛮物(对应装备表主键编号)</summary>
            public const String equip_barbarian = "equip_barbarian";

            ///<summary>坐骑(对应装备表主键编号)</summary>
            public const String equip_mounts = "equip_mounts";

            ///<summary>铠甲(对应装备表主键编号)</summary>
            public const String equip_armor = "equip_armor";

            ///<summary>宝石(对应装备表主键编号)</summary>
            public const String equip_gem = "equip_gem";

            ///<summary>茶器(对应装备表主键编号)</summary>
            public const String equip_tea = "equip_tea";

            ///<summary>艺术品(对应装备表主键编号)</summary>
            public const String equip_craft = "equip_craft";

            ///<summary>书籍(对应装备表主键编号)</summary>
            public const String equip_book = "equip_book";

            ///<summary>统帅(生活技能加点)</summary>
            public const String base_captain_life = "base_captain_life";

            ///<summary>统帅(锻炼加点)</summary>
            public const String base_captain_train = "base_captain_train";

            ///<summary>统帅(升级加点)</summary>
            public const String base_captain_level = "base_captain_level";

            ///<summary>统帅(铸魂加点)</summary>
            public const String base_captain_spirit = "base_captain_spirit";

            ///<summary>统帅(装备加点)</summary>
            public const String base_captain_equip = "base_captain_equip";

            ///<summary>统帅(称号加点)</summary>
            public const String base_captain_title = "base_captain_title";

            ///<summary>武力(生活技能加点)</summary>
            public const String base_force_life = "base_force_life";

            ///<summary>武力(锻炼加点)</summary>
            public const String base_force_train = "base_force_train";

            ///<summary>武力(升级加点)</summary>
            public const String base_force_level = "base_force_level";

            ///<summary>武力(铸魂加点)</summary>
            public const String base_force_spirit = "base_force_spirit";

            ///<summary>武力(装备加点)</summary>
            public const String base_force_equip = "base_force_equip";

            ///<summary>武力(称号加点)</summary>
            public const String base_force_title = "base_force_title";

            ///<summary>智谋(生活技能加点)</summary>
            public const String base_brains_life = "base_brains_life";

            ///<summary>智谋(锻炼加点)</summary>
            public const String base_brains_train = "base_brains_train";

            ///<summary>智谋(升级加点)</summary>
            public const String base_brains_level = "base_brains_level";

            ///<summary>智谋(铸魂加点)</summary>
            public const String base_brains_spirit = "base_brains_spirit";

            ///<summary>智谋(装备加点)</summary>
            public const String base_brains_equip = "base_brains_equip";

            ///<summary>智谋(称号加点)</summary>
            public const String base_brains_title = "base_brains_title";

            ///<summary>魅力(生活技能加点)</summary>
            public const String base_charm_life = "base_charm_life";

            ///<summary>魅力(锻炼加点)</summary>
            public const String base_charm_train = "base_charm_train";

            ///<summary>魅力(升级加点)</summary>
            public const String base_charm_level = "base_charm_level";

            ///<summary>魅力(铸魂加点)</summary>
            public const String base_charm_spirit = "base_charm_spirit";

            ///<summary>魅力(装备加点)</summary>
            public const String base_charm_equip = "base_charm_equip";

            ///<summary>魅力(称号加点)</summary>
            public const String base_charm_title = "base_charm_title";

            ///<summary>政务(生活技能加点)</summary>
            public const String base_govern_life = "base_govern_life";

            ///<summary>政务(锻炼加点)</summary>
            public const String base_govern_train = "base_govern_train";

            ///<summary>政务(升级加点)</summary>
            public const String base_govern_level = "base_govern_level";

            ///<summary>政务(铸魂加点)</summary>
            public const String base_govern_spirit = "base_govern_spirit";

            ///<summary>政务(装备加点)</summary>
            public const String base_govern_equip = "base_govern_equip";

            ///<summary>政务(称号加点)</summary>
            public const String base_govern_title = "base_govern_title";

            ///<summary>剑称号(对应家臣称号表主键编号)</summary>
            public const String title_sword = "title_sword";

            ///<summary>枪称号(对应家臣称号表主键编号)</summary>
            public const String title_gun = "title_gun";

            ///<summary>茶道称号(对应家臣称号表主键编号)</summary>
            public const String title_tea = "title_tea";

            ///<summary>讲价称号(对应家臣称号表主键编号)</summary>
            public const String title_eloquence = "title_eloquence";

            ///<summary>体力buff</summary>
            public const String buff_power = "buff_power";

            ///<summary>总功勋值</summary>
            public const String total_honor = "total_honor";

            ///<summary></summary>
            public const String total_exp = "total_exp";

        }
        #endregion
    }

    /// <summary>武将表接口</summary>
    public partial interface Itg_role
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>武将基表编号</summary>
        Int32 role_id { get; set; }

        /// <summary>体力</summary>
        Int32 power { get; set; }

        /// <summary>武将默认流派</summary>
        Int32 role_genre { get; set; }

        /// <summary>武将默认忍者冢</summary>
        Int32 role_ninja { get; set; }

        /// <summary>武将等级</summary>
        Int32 role_level { get; set; }

        /// <summary>武将状态</summary>
        Int32 role_state { get; set; }

        /// <summary>武将经验</summary>
        Int32 role_exp { get; set; }

        /// <summary>武将功勋</summary>
        Int32 role_honor { get; set; }

        /// <summary>身份</summary>
        Int32 role_identity { get; set; }

        /// <summary>统帅(初始值)</summary>
        Double base_captain { get; set; }

        /// <summary>武力(初始值)</summary>
        Double base_force { get; set; }

        /// <summary>智谋(初始值)</summary>
        Double base_brains { get; set; }

        /// <summary>魅力(初始值)</summary>
        Double base_charm { get; set; }

        /// <summary>政务(初始值)</summary>
        Double base_govern { get; set; }

        /// <summary>武将剩余属性点</summary>
        Int32 att_points { get; set; }

        /// <summary>生命</summary>
        Int32 att_life { get; set; }

        /// <summary>攻击</summary>
        Double att_attack { get; set; }

        /// <summary>防御</summary>
        Double att_defense { get; set; }

        /// <summary>增伤</summary>
        Double att_sub_hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        Double att_sub_hurtReduce { get; set; }

        /// <summary>会心几率</summary>
        Double att_crit_probability { get; set; }

        /// <summary>会心效果</summary>
        Double att_crit_addition { get; set; }

        /// <summary>奥义触发几率</summary>
        Double att_mystery_probability { get; set; }

        /// <summary>闪避几率</summary>
        Double att_dodge_probability { get; set; }

        /// <summary>秘技</summary>
        Int64 art_cheat_code { get; set; }

        /// <summary>奥义</summary>
        Int64 art_mystery { get; set; }

        /// <summary>武器(对应装备表主键编号)</summary>
        Int64 equip_weapon { get; set; }

        /// <summary>南蛮物(对应装备表主键编号)</summary>
        Int64 equip_barbarian { get; set; }

        /// <summary>坐骑(对应装备表主键编号)</summary>
        Int64 equip_mounts { get; set; }

        /// <summary>铠甲(对应装备表主键编号)</summary>
        Int64 equip_armor { get; set; }

        /// <summary>宝石(对应装备表主键编号)</summary>
        Int64 equip_gem { get; set; }

        /// <summary>茶器(对应装备表主键编号)</summary>
        Int64 equip_tea { get; set; }

        /// <summary>艺术品(对应装备表主键编号)</summary>
        Int64 equip_craft { get; set; }

        /// <summary>书籍(对应装备表主键编号)</summary>
        Int64 equip_book { get; set; }

        /// <summary>统帅(生活技能加点)</summary>
        Double base_captain_life { get; set; }

        /// <summary>统帅(锻炼加点)</summary>
        Double base_captain_train { get; set; }

        /// <summary>统帅(升级加点)</summary>
        Double base_captain_level { get; set; }

        /// <summary>统帅(铸魂加点)</summary>
        Double base_captain_spirit { get; set; }

        /// <summary>统帅(装备加点)</summary>
        Double base_captain_equip { get; set; }

        /// <summary>统帅(称号加点)</summary>
        Double base_captain_title { get; set; }

        /// <summary>武力(生活技能加点)</summary>
        Double base_force_life { get; set; }

        /// <summary>武力(锻炼加点)</summary>
        Double base_force_train { get; set; }

        /// <summary>武力(升级加点)</summary>
        Double base_force_level { get; set; }

        /// <summary>武力(铸魂加点)</summary>
        Double base_force_spirit { get; set; }

        /// <summary>武力(装备加点)</summary>
        Double base_force_equip { get; set; }

        /// <summary>武力(称号加点)</summary>
        Double base_force_title { get; set; }

        /// <summary>智谋(生活技能加点)</summary>
        Double base_brains_life { get; set; }

        /// <summary>智谋(锻炼加点)</summary>
        Double base_brains_train { get; set; }

        /// <summary>智谋(升级加点)</summary>
        Double base_brains_level { get; set; }

        /// <summary>智谋(铸魂加点)</summary>
        Double base_brains_spirit { get; set; }

        /// <summary>智谋(装备加点)</summary>
        Double base_brains_equip { get; set; }

        /// <summary>智谋(称号加点)</summary>
        Double base_brains_title { get; set; }

        /// <summary>魅力(生活技能加点)</summary>
        Double base_charm_life { get; set; }

        /// <summary>魅力(锻炼加点)</summary>
        Double base_charm_train { get; set; }

        /// <summary>魅力(升级加点)</summary>
        Double base_charm_level { get; set; }

        /// <summary>魅力(铸魂加点)</summary>
        Double base_charm_spirit { get; set; }

        /// <summary>魅力(装备加点)</summary>
        Double base_charm_equip { get; set; }

        /// <summary>魅力(称号加点)</summary>
        Double base_charm_title { get; set; }

        /// <summary>政务(生活技能加点)</summary>
        Double base_govern_life { get; set; }

        /// <summary>政务(锻炼加点)</summary>
        Double base_govern_train { get; set; }

        /// <summary>政务(升级加点)</summary>
        Double base_govern_level { get; set; }

        /// <summary>政务(铸魂加点)</summary>
        Double base_govern_spirit { get; set; }

        /// <summary>政务(装备加点)</summary>
        Double base_govern_equip { get; set; }

        /// <summary>政务(称号加点)</summary>
        Double base_govern_title { get; set; }

        /// <summary>剑称号(对应家臣称号表主键编号)</summary>
        Int64 title_sword { get; set; }

        /// <summary>枪称号(对应家臣称号表主键编号)</summary>
        Int64 title_gun { get; set; }

        /// <summary>茶道称号(对应家臣称号表主键编号)</summary>
        Int64 title_tea { get; set; }

        /// <summary>讲价称号(对应家臣称号表主键编号)</summary>
        Int64 title_eloquence { get; set; }

        /// <summary>体力buff</summary>
        Int32 buff_power { get; set; }

        /// <summary>总功勋值</summary>
        Int32 total_honor { get; set; }

        /// <summary></summary>
        Int32 total_exp { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}