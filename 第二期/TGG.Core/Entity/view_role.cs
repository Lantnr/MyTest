using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Role</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_role", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_role : Iview_role
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

        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _role_id;
        /// <summary></summary>
        [DisplayName("ID2")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "role_id", "", null, "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private Int32 _power;
        /// <summary></summary>
        [DisplayName("Power")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "power", "", null, "int", 10, 0, false)]
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
        [BindColumn(5, "role_level", "", null, "int", 10, 0, false)]
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
        [BindColumn(6, "role_state", "", null, "int", 10, 0, false)]
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
        [BindColumn(7, "role_exp", "", null, "int", 10, 0, false)]
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
        [BindColumn(8, "role_honor", "", null, "int", 10, 0, false)]
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
        [BindColumn(9, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Double _base_captain;
        /// <summary></summary>
        [DisplayName("Captain")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(10, "base_captain", "", null, "float", 53, 0, false)]
        public virtual Double base_captain
        {
            get { return _base_captain; }
            set { if (OnPropertyChanging(__.base_captain, value)) { _base_captain = value; OnPropertyChanged(__.base_captain); } }
        }

        private Double _base_force;
        /// <summary></summary>
        [DisplayName("Force")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(11, "base_force", "", null, "float", 53, 0, false)]
        public virtual Double base_force
        {
            get { return _base_force; }
            set { if (OnPropertyChanging(__.base_force, value)) { _base_force = value; OnPropertyChanged(__.base_force); } }
        }

        private Double _base_brains;
        /// <summary></summary>
        [DisplayName("Brains")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(12, "base_brains", "", null, "float", 53, 0, false)]
        public virtual Double base_brains
        {
            get { return _base_brains; }
            set { if (OnPropertyChanging(__.base_brains, value)) { _base_brains = value; OnPropertyChanged(__.base_brains); } }
        }

        private Double _base_charm;
        /// <summary></summary>
        [DisplayName("Charm")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(13, "base_charm", "", null, "float", 53, 0, false)]
        public virtual Double base_charm
        {
            get { return _base_charm; }
            set { if (OnPropertyChanging(__.base_charm, value)) { _base_charm = value; OnPropertyChanged(__.base_charm); } }
        }

        private Double _base_govern;
        /// <summary></summary>
        [DisplayName("Govern")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(14, "base_govern", "", null, "float", 53, 0, false)]
        public virtual Double base_govern
        {
            get { return _base_govern; }
            set { if (OnPropertyChanging(__.base_govern, value)) { _base_govern = value; OnPropertyChanged(__.base_govern); } }
        }

        private Int32 _att_life;
        /// <summary></summary>
        [DisplayName("Life")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "att_life", "", null, "int", 10, 0, false)]
        public virtual Int32 att_life
        {
            get { return _att_life; }
            set { if (OnPropertyChanging(__.att_life, value)) { _att_life = value; OnPropertyChanged(__.att_life); } }
        }

        private Double _att_attack;
        /// <summary></summary>
        [DisplayName("Attack")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(16, "att_attack", "", null, "float", 53, 0, false)]
        public virtual Double att_attack
        {
            get { return _att_attack; }
            set { if (OnPropertyChanging(__.att_attack, value)) { _att_attack = value; OnPropertyChanged(__.att_attack); } }
        }

        private Double _att_defense;
        /// <summary></summary>
        [DisplayName("Defense")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(17, "att_defense", "", null, "float", 53, 0, false)]
        public virtual Double att_defense
        {
            get { return _att_defense; }
            set { if (OnPropertyChanging(__.att_defense, value)) { _att_defense = value; OnPropertyChanged(__.att_defense); } }
        }

        private Double _att_sub_hurtIncrease;
        /// <summary></summary>
        [DisplayName("Sub_HurtIncrease")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(18, "att_sub_hurtIncrease", "", null, "float", 53, 0, false)]
        public virtual Double att_sub_hurtIncrease
        {
            get { return _att_sub_hurtIncrease; }
            set { if (OnPropertyChanging(__.att_sub_hurtIncrease, value)) { _att_sub_hurtIncrease = value; OnPropertyChanged(__.att_sub_hurtIncrease); } }
        }

        private Double _att_sub_hurtReduce;
        /// <summary></summary>
        [DisplayName("Sub_HurtReduce")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(19, "att_sub_hurtReduce", "", null, "float", 53, 0, false)]
        public virtual Double att_sub_hurtReduce
        {
            get { return _att_sub_hurtReduce; }
            set { if (OnPropertyChanging(__.att_sub_hurtReduce, value)) { _att_sub_hurtReduce = value; OnPropertyChanged(__.att_sub_hurtReduce); } }
        }

        private Int64 _art_mystery;
        /// <summary></summary>
        [DisplayName("Mystery")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(20, "art_mystery", "", null, "bigint", 19, 0, false)]
        public virtual Int64 art_mystery
        {
            get { return _art_mystery; }
            set { if (OnPropertyChanging(__.art_mystery, value)) { _art_mystery = value; OnPropertyChanged(__.art_mystery); } }
        }

        private Int64 _art_cheat_code;
        /// <summary></summary>
        [DisplayName("Cheat_Code")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(21, "art_cheat_code", "", null, "bigint", 19, 0, false)]
        public virtual Int64 art_cheat_code
        {
            get { return _art_cheat_code; }
            set { if (OnPropertyChanging(__.art_cheat_code, value)) { _art_cheat_code = value; OnPropertyChanged(__.art_cheat_code); } }
        }

        private Int64 _equip_weapon;
        /// <summary></summary>
        [DisplayName("Weapon")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(22, "equip_weapon", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_weapon
        {
            get { return _equip_weapon; }
            set { if (OnPropertyChanging(__.equip_weapon, value)) { _equip_weapon = value; OnPropertyChanged(__.equip_weapon); } }
        }

        private Int64 _equip_barbarian;
        /// <summary></summary>
        [DisplayName("Barbarian")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(23, "equip_barbarian", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_barbarian
        {
            get { return _equip_barbarian; }
            set { if (OnPropertyChanging(__.equip_barbarian, value)) { _equip_barbarian = value; OnPropertyChanged(__.equip_barbarian); } }
        }

        private Int64 _equip_mounts;
        /// <summary></summary>
        [DisplayName("Mounts")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(24, "equip_mounts", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_mounts
        {
            get { return _equip_mounts; }
            set { if (OnPropertyChanging(__.equip_mounts, value)) { _equip_mounts = value; OnPropertyChanged(__.equip_mounts); } }
        }

        private Int64 _equip_armor;
        /// <summary></summary>
        [DisplayName("Armor")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(25, "equip_armor", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_armor
        {
            get { return _equip_armor; }
            set { if (OnPropertyChanging(__.equip_armor, value)) { _equip_armor = value; OnPropertyChanged(__.equip_armor); } }
        }

        private Int64 _equip_gem;
        /// <summary></summary>
        [DisplayName("Gem")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(26, "equip_gem", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_gem
        {
            get { return _equip_gem; }
            set { if (OnPropertyChanging(__.equip_gem, value)) { _equip_gem = value; OnPropertyChanged(__.equip_gem); } }
        }

        private Int64 _equip_craft;
        /// <summary></summary>
        [DisplayName("Craft")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(27, "equip_craft", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_craft
        {
            get { return _equip_craft; }
            set { if (OnPropertyChanging(__.equip_craft, value)) { _equip_craft = value; OnPropertyChanged(__.equip_craft); } }
        }

        private Int64 _equip_tea;
        /// <summary></summary>
        [DisplayName("Tea")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(28, "equip_tea", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_tea
        {
            get { return _equip_tea; }
            set { if (OnPropertyChanging(__.equip_tea, value)) { _equip_tea = value; OnPropertyChanged(__.equip_tea); } }
        }

        private Int64 _equip_book;
        /// <summary></summary>
        [DisplayName("Book")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(29, "equip_book", "", null, "bigint", 19, 0, false)]
        public virtual Int64 equip_book
        {
            get { return _equip_book; }
            set { if (OnPropertyChanging(__.equip_book, value)) { _equip_book = value; OnPropertyChanged(__.equip_book); } }
        }

        private Int32 _att_points;
        /// <summary></summary>
        [DisplayName("Points")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(30, "att_points", "", null, "int", 10, 0, false)]
        public virtual Int32 att_points
        {
            get { return _att_points; }
            set { if (OnPropertyChanging(__.att_points, value)) { _att_points = value; OnPropertyChanged(__.att_points); } }
        }

        private Int64 _lid;
        /// <summary></summary>
        [DisplayName("Lid")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(31, "lid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 lid
        {
            get { return _lid; }
            set { if (OnPropertyChanging(__.lid, value)) { _lid = value; OnPropertyChanged(__.lid); } }
        }

        private Int32 _sub_tea;
        /// <summary></summary>
        [DisplayName("Tea1")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(32, "sub_tea", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tea
        {
            get { return _sub_tea; }
            set { if (OnPropertyChanging(__.sub_tea, value)) { _sub_tea = value; OnPropertyChanged(__.sub_tea); } }
        }

        private Int32 _sub_calculate;
        /// <summary></summary>
        [DisplayName("Calculate")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(33, "sub_calculate", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_calculate
        {
            get { return _sub_calculate; }
            set { if (OnPropertyChanging(__.sub_calculate, value)) { _sub_calculate = value; OnPropertyChanged(__.sub_calculate); } }
        }

        private Int32 _sub_build;
        /// <summary></summary>
        [DisplayName("Build")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(34, "sub_build", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_build
        {
            get { return _sub_build; }
            set { if (OnPropertyChanging(__.sub_build, value)) { _sub_build = value; OnPropertyChanged(__.sub_build); } }
        }

        private Int32 _sub_eloquence;
        /// <summary></summary>
        [DisplayName("Eloquence")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(35, "sub_eloquence", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_eloquence
        {
            get { return _sub_eloquence; }
            set { if (OnPropertyChanging(__.sub_eloquence, value)) { _sub_eloquence = value; OnPropertyChanged(__.sub_eloquence); } }
        }

        private Int32 _sub_equestrian;
        /// <summary></summary>
        [DisplayName("Equestrian")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(36, "sub_equestrian", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_equestrian
        {
            get { return _sub_equestrian; }
            set { if (OnPropertyChanging(__.sub_equestrian, value)) { _sub_equestrian = value; OnPropertyChanged(__.sub_equestrian); } }
        }

        private Int32 _sub_reclaimed;
        /// <summary></summary>
        [DisplayName("Reclaimed")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(37, "sub_reclaimed", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed
        {
            get { return _sub_reclaimed; }
            set { if (OnPropertyChanging(__.sub_reclaimed, value)) { _sub_reclaimed = value; OnPropertyChanged(__.sub_reclaimed); } }
        }

        private Int32 _sub_ashigaru;
        /// <summary></summary>
        [DisplayName("Ashigaru")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(38, "sub_ashigaru", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru
        {
            get { return _sub_ashigaru; }
            set { if (OnPropertyChanging(__.sub_ashigaru, value)) { _sub_ashigaru = value; OnPropertyChanged(__.sub_ashigaru); } }
        }

        private Int32 _sub_artillery;
        /// <summary></summary>
        [DisplayName("Artillery")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(39, "sub_artillery", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_artillery
        {
            get { return _sub_artillery; }
            set { if (OnPropertyChanging(__.sub_artillery, value)) { _sub_artillery = value; OnPropertyChanged(__.sub_artillery); } }
        }

        private Int32 _sub_mine;
        /// <summary></summary>
        [DisplayName("Mine")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(40, "sub_mine", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_mine
        {
            get { return _sub_mine; }
            set { if (OnPropertyChanging(__.sub_mine, value)) { _sub_mine = value; OnPropertyChanged(__.sub_mine); } }
        }

        private Int32 _sub_craft;
        /// <summary></summary>
        [DisplayName("Craft1")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(41, "sub_craft", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_craft
        {
            get { return _sub_craft; }
            set { if (OnPropertyChanging(__.sub_craft, value)) { _sub_craft = value; OnPropertyChanged(__.sub_craft); } }
        }

        private Int32 _sub_archer;
        /// <summary></summary>
        [DisplayName("Archer")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(42, "sub_archer", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_archer
        {
            get { return _sub_archer; }
            set { if (OnPropertyChanging(__.sub_archer, value)) { _sub_archer = value; OnPropertyChanged(__.sub_archer); } }
        }

        private Int32 _sub_etiquette;
        /// <summary></summary>
        [DisplayName("Etiquette")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(43, "sub_etiquette", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_etiquette
        {
            get { return _sub_etiquette; }
            set { if (OnPropertyChanging(__.sub_etiquette, value)) { _sub_etiquette = value; OnPropertyChanged(__.sub_etiquette); } }
        }

        private Int32 _sub_martial;
        /// <summary></summary>
        [DisplayName("Martial")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(44, "sub_martial", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_martial
        {
            get { return _sub_martial; }
            set { if (OnPropertyChanging(__.sub_martial, value)) { _sub_martial = value; OnPropertyChanged(__.sub_martial); } }
        }

        private Int32 _sub_tactical;
        /// <summary></summary>
        [DisplayName("Tactical")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(45, "sub_tactical", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tactical
        {
            get { return _sub_tactical; }
            set { if (OnPropertyChanging(__.sub_tactical, value)) { _sub_tactical = value; OnPropertyChanged(__.sub_tactical); } }
        }

        private Int32 _sub_medical;
        /// <summary></summary>
        [DisplayName("Medical")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(46, "sub_medical", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_medical
        {
            get { return _sub_medical; }
            set { if (OnPropertyChanging(__.sub_medical, value)) { _sub_medical = value; OnPropertyChanged(__.sub_medical); } }
        }

        private Int32 _sub_ninjitsu;
        /// <summary></summary>
        [DisplayName("Ninjitsu")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(47, "sub_ninjitsu", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu
        {
            get { return _sub_ninjitsu; }
            set { if (OnPropertyChanging(__.sub_ninjitsu, value)) { _sub_ninjitsu = value; OnPropertyChanged(__.sub_ninjitsu); } }
        }

        private Int32 _sub_tea_progress;
        /// <summary></summary>
        [DisplayName("Tea_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(48, "sub_tea_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tea_progress
        {
            get { return _sub_tea_progress; }
            set { if (OnPropertyChanging(__.sub_tea_progress, value)) { _sub_tea_progress = value; OnPropertyChanged(__.sub_tea_progress); } }
        }

        private Int32 _sub_calculate_progress;
        /// <summary></summary>
        [DisplayName("Calculate_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(49, "sub_calculate_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_calculate_progress
        {
            get { return _sub_calculate_progress; }
            set { if (OnPropertyChanging(__.sub_calculate_progress, value)) { _sub_calculate_progress = value; OnPropertyChanged(__.sub_calculate_progress); } }
        }

        private Int32 _sub_build_progress;
        /// <summary></summary>
        [DisplayName("Build_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(50, "sub_build_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_build_progress
        {
            get { return _sub_build_progress; }
            set { if (OnPropertyChanging(__.sub_build_progress, value)) { _sub_build_progress = value; OnPropertyChanged(__.sub_build_progress); } }
        }

        private Int32 _sub_eloquence_progress;
        /// <summary></summary>
        [DisplayName("Eloquence_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(51, "sub_eloquence_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_progress
        {
            get { return _sub_eloquence_progress; }
            set { if (OnPropertyChanging(__.sub_eloquence_progress, value)) { _sub_eloquence_progress = value; OnPropertyChanged(__.sub_eloquence_progress); } }
        }

        private Int32 _sub_equestrian_progress;
        /// <summary></summary>
        [DisplayName("Equestrian_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(52, "sub_equestrian_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_progress
        {
            get { return _sub_equestrian_progress; }
            set { if (OnPropertyChanging(__.sub_equestrian_progress, value)) { _sub_equestrian_progress = value; OnPropertyChanged(__.sub_equestrian_progress); } }
        }

        private Int32 _sub_reclaimed_progress;
        /// <summary></summary>
        [DisplayName("Reclaimed_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(53, "sub_reclaimed_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_progress
        {
            get { return _sub_reclaimed_progress; }
            set { if (OnPropertyChanging(__.sub_reclaimed_progress, value)) { _sub_reclaimed_progress = value; OnPropertyChanged(__.sub_reclaimed_progress); } }
        }

        private Int32 _sub_ashigaru_progress;
        /// <summary></summary>
        [DisplayName("Ashigaru_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(54, "sub_ashigaru_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_progress
        {
            get { return _sub_ashigaru_progress; }
            set { if (OnPropertyChanging(__.sub_ashigaru_progress, value)) { _sub_ashigaru_progress = value; OnPropertyChanged(__.sub_ashigaru_progress); } }
        }

        private Int32 _sub_artillery_progress;
        /// <summary></summary>
        [DisplayName("Artillery_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(55, "sub_artillery_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_artillery_progress
        {
            get { return _sub_artillery_progress; }
            set { if (OnPropertyChanging(__.sub_artillery_progress, value)) { _sub_artillery_progress = value; OnPropertyChanged(__.sub_artillery_progress); } }
        }

        private Int32 _sub_mine_progress;
        /// <summary></summary>
        [DisplayName("Mine_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(56, "sub_mine_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_mine_progress
        {
            get { return _sub_mine_progress; }
            set { if (OnPropertyChanging(__.sub_mine_progress, value)) { _sub_mine_progress = value; OnPropertyChanged(__.sub_mine_progress); } }
        }

        private Int32 _sub_craft_progress;
        /// <summary></summary>
        [DisplayName("Craft_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(57, "sub_craft_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_craft_progress
        {
            get { return _sub_craft_progress; }
            set { if (OnPropertyChanging(__.sub_craft_progress, value)) { _sub_craft_progress = value; OnPropertyChanged(__.sub_craft_progress); } }
        }

        private Int32 _sub_archer_progress;
        /// <summary></summary>
        [DisplayName("Archer_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(58, "sub_archer_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_archer_progress
        {
            get { return _sub_archer_progress; }
            set { if (OnPropertyChanging(__.sub_archer_progress, value)) { _sub_archer_progress = value; OnPropertyChanged(__.sub_archer_progress); } }
        }

        private Int32 _sub_etiquette_progress;
        /// <summary></summary>
        [DisplayName("Etiquette_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(59, "sub_etiquette_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_progress
        {
            get { return _sub_etiquette_progress; }
            set { if (OnPropertyChanging(__.sub_etiquette_progress, value)) { _sub_etiquette_progress = value; OnPropertyChanged(__.sub_etiquette_progress); } }
        }

        private Int32 _sub_martial_progress;
        /// <summary></summary>
        [DisplayName("Martial_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(60, "sub_martial_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_martial_progress
        {
            get { return _sub_martial_progress; }
            set { if (OnPropertyChanging(__.sub_martial_progress, value)) { _sub_martial_progress = value; OnPropertyChanged(__.sub_martial_progress); } }
        }

        private Int32 _sub_tactical_progress;
        /// <summary></summary>
        [DisplayName("Tactical_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(61, "sub_tactical_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tactical_progress
        {
            get { return _sub_tactical_progress; }
            set { if (OnPropertyChanging(__.sub_tactical_progress, value)) { _sub_tactical_progress = value; OnPropertyChanged(__.sub_tactical_progress); } }
        }

        private Int32 _sub_medical_progress;
        /// <summary></summary>
        [DisplayName("Medical_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(62, "sub_medical_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_medical_progress
        {
            get { return _sub_medical_progress; }
            set { if (OnPropertyChanging(__.sub_medical_progress, value)) { _sub_medical_progress = value; OnPropertyChanged(__.sub_medical_progress); } }
        }

        private Int32 _sub_ninjitsu_progress;
        /// <summary></summary>
        [DisplayName("Ninjitsu_Progress")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(63, "sub_ninjitsu_progress", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_progress
        {
            get { return _sub_ninjitsu_progress; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_progress, value)) { _sub_ninjitsu_progress = value; OnPropertyChanged(__.sub_ninjitsu_progress); } }
        }

        private Int64 _sub_tea_time;
        /// <summary></summary>
        [DisplayName("Tea_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(64, "sub_tea_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_tea_time
        {
            get { return _sub_tea_time; }
            set { if (OnPropertyChanging(__.sub_tea_time, value)) { _sub_tea_time = value; OnPropertyChanged(__.sub_tea_time); } }
        }

        private Int64 _sub_calculate_time;
        /// <summary></summary>
        [DisplayName("Calculate_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(65, "sub_calculate_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_calculate_time
        {
            get { return _sub_calculate_time; }
            set { if (OnPropertyChanging(__.sub_calculate_time, value)) { _sub_calculate_time = value; OnPropertyChanged(__.sub_calculate_time); } }
        }

        private Int64 _sub_build_time;
        /// <summary></summary>
        [DisplayName("Build_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(66, "sub_build_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_build_time
        {
            get { return _sub_build_time; }
            set { if (OnPropertyChanging(__.sub_build_time, value)) { _sub_build_time = value; OnPropertyChanged(__.sub_build_time); } }
        }

        private Int64 _sub_eloquence_time;
        /// <summary></summary>
        [DisplayName("Eloquence_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(67, "sub_eloquence_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_eloquence_time
        {
            get { return _sub_eloquence_time; }
            set { if (OnPropertyChanging(__.sub_eloquence_time, value)) { _sub_eloquence_time = value; OnPropertyChanged(__.sub_eloquence_time); } }
        }

        private Int64 _sub_equestrian_time;
        /// <summary></summary>
        [DisplayName("Equestrian_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(68, "sub_equestrian_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_equestrian_time
        {
            get { return _sub_equestrian_time; }
            set { if (OnPropertyChanging(__.sub_equestrian_time, value)) { _sub_equestrian_time = value; OnPropertyChanged(__.sub_equestrian_time); } }
        }

        private Int64 _sub_reclaimed_time;
        /// <summary></summary>
        [DisplayName("Reclaimed_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(69, "sub_reclaimed_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_reclaimed_time
        {
            get { return _sub_reclaimed_time; }
            set { if (OnPropertyChanging(__.sub_reclaimed_time, value)) { _sub_reclaimed_time = value; OnPropertyChanged(__.sub_reclaimed_time); } }
        }

        private Int64 _sub_ashigaru_time;
        /// <summary></summary>
        [DisplayName("Ashigaru_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(70, "sub_ashigaru_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_ashigaru_time
        {
            get { return _sub_ashigaru_time; }
            set { if (OnPropertyChanging(__.sub_ashigaru_time, value)) { _sub_ashigaru_time = value; OnPropertyChanged(__.sub_ashigaru_time); } }
        }

        private Int64 _sub_artillery_time;
        /// <summary></summary>
        [DisplayName("Artillery_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(71, "sub_artillery_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_artillery_time
        {
            get { return _sub_artillery_time; }
            set { if (OnPropertyChanging(__.sub_artillery_time, value)) { _sub_artillery_time = value; OnPropertyChanged(__.sub_artillery_time); } }
        }

        private Int64 _sub_mine_time;
        /// <summary></summary>
        [DisplayName("Mine_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(72, "sub_mine_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_mine_time
        {
            get { return _sub_mine_time; }
            set { if (OnPropertyChanging(__.sub_mine_time, value)) { _sub_mine_time = value; OnPropertyChanged(__.sub_mine_time); } }
        }

        private Int64 _sub_craft_time;
        /// <summary></summary>
        [DisplayName("Craft_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(73, "sub_craft_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_craft_time
        {
            get { return _sub_craft_time; }
            set { if (OnPropertyChanging(__.sub_craft_time, value)) { _sub_craft_time = value; OnPropertyChanged(__.sub_craft_time); } }
        }

        private Int64 _sub_archer_time;
        /// <summary></summary>
        [DisplayName("Archer_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(74, "sub_archer_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_archer_time
        {
            get { return _sub_archer_time; }
            set { if (OnPropertyChanging(__.sub_archer_time, value)) { _sub_archer_time = value; OnPropertyChanged(__.sub_archer_time); } }
        }

        private Int64 _sub_etiquette_time;
        /// <summary></summary>
        [DisplayName("Etiquette_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(75, "sub_etiquette_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_etiquette_time
        {
            get { return _sub_etiquette_time; }
            set { if (OnPropertyChanging(__.sub_etiquette_time, value)) { _sub_etiquette_time = value; OnPropertyChanged(__.sub_etiquette_time); } }
        }

        private Int64 _sub_martial_time;
        /// <summary></summary>
        [DisplayName("Martial_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(76, "sub_martial_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_martial_time
        {
            get { return _sub_martial_time; }
            set { if (OnPropertyChanging(__.sub_martial_time, value)) { _sub_martial_time = value; OnPropertyChanged(__.sub_martial_time); } }
        }

        private Int64 _sub_tactical_time;
        /// <summary></summary>
        [DisplayName("Tactical_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(77, "sub_tactical_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_tactical_time
        {
            get { return _sub_tactical_time; }
            set { if (OnPropertyChanging(__.sub_tactical_time, value)) { _sub_tactical_time = value; OnPropertyChanged(__.sub_tactical_time); } }
        }

        private Int64 _sub_medical_time;
        /// <summary></summary>
        [DisplayName("Medical_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(78, "sub_medical_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_medical_time
        {
            get { return _sub_medical_time; }
            set { if (OnPropertyChanging(__.sub_medical_time, value)) { _sub_medical_time = value; OnPropertyChanged(__.sub_medical_time); } }
        }

        private Int64 _sub_ninjitsu_time;
        /// <summary></summary>
        [DisplayName("Ninjitsu_Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(79, "sub_ninjitsu_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 sub_ninjitsu_time
        {
            get { return _sub_ninjitsu_time; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_time, value)) { _sub_ninjitsu_time = value; OnPropertyChanged(__.sub_ninjitsu_time); } }
        }

        private Int32 _sub_tea_level;
        /// <summary></summary>
        [DisplayName("Tea_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(80, "sub_tea_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tea_level
        {
            get { return _sub_tea_level; }
            set { if (OnPropertyChanging(__.sub_tea_level, value)) { _sub_tea_level = value; OnPropertyChanged(__.sub_tea_level); } }
        }

        private Int32 _sub_calculate_level;
        /// <summary></summary>
        [DisplayName("Calculate_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(81, "sub_calculate_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_calculate_level
        {
            get { return _sub_calculate_level; }
            set { if (OnPropertyChanging(__.sub_calculate_level, value)) { _sub_calculate_level = value; OnPropertyChanged(__.sub_calculate_level); } }
        }

        private Int32 _sub_build_level;
        /// <summary></summary>
        [DisplayName("Build_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(82, "sub_build_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_build_level
        {
            get { return _sub_build_level; }
            set { if (OnPropertyChanging(__.sub_build_level, value)) { _sub_build_level = value; OnPropertyChanged(__.sub_build_level); } }
        }

        private Int32 _sub_eloquence_level;
        /// <summary></summary>
        [DisplayName("Eloquence_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(83, "sub_eloquence_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_level
        {
            get { return _sub_eloquence_level; }
            set { if (OnPropertyChanging(__.sub_eloquence_level, value)) { _sub_eloquence_level = value; OnPropertyChanged(__.sub_eloquence_level); } }
        }

        private Int32 _sub_equestrian_level;
        /// <summary></summary>
        [DisplayName("Equestrian_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(84, "sub_equestrian_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_level
        {
            get { return _sub_equestrian_level; }
            set { if (OnPropertyChanging(__.sub_equestrian_level, value)) { _sub_equestrian_level = value; OnPropertyChanged(__.sub_equestrian_level); } }
        }

        private Int32 _sub_reclaimed_level;
        /// <summary></summary>
        [DisplayName("Reclaimed_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(85, "sub_reclaimed_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_level
        {
            get { return _sub_reclaimed_level; }
            set { if (OnPropertyChanging(__.sub_reclaimed_level, value)) { _sub_reclaimed_level = value; OnPropertyChanged(__.sub_reclaimed_level); } }
        }

        private Int32 _sub_ashigaru_level;
        /// <summary></summary>
        [DisplayName("Ashigaru_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(86, "sub_ashigaru_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_level
        {
            get { return _sub_ashigaru_level; }
            set { if (OnPropertyChanging(__.sub_ashigaru_level, value)) { _sub_ashigaru_level = value; OnPropertyChanged(__.sub_ashigaru_level); } }
        }

        private Int32 _sub_artillery_level;
        /// <summary></summary>
        [DisplayName("Artillery_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(87, "sub_artillery_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_artillery_level
        {
            get { return _sub_artillery_level; }
            set { if (OnPropertyChanging(__.sub_artillery_level, value)) { _sub_artillery_level = value; OnPropertyChanged(__.sub_artillery_level); } }
        }

        private Int32 _sub_mine_level;
        /// <summary></summary>
        [DisplayName("Mine_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(88, "sub_mine_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_mine_level
        {
            get { return _sub_mine_level; }
            set { if (OnPropertyChanging(__.sub_mine_level, value)) { _sub_mine_level = value; OnPropertyChanged(__.sub_mine_level); } }
        }

        private Int32 _sub_craft_level;
        /// <summary></summary>
        [DisplayName("Craft_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(89, "sub_craft_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_craft_level
        {
            get { return _sub_craft_level; }
            set { if (OnPropertyChanging(__.sub_craft_level, value)) { _sub_craft_level = value; OnPropertyChanged(__.sub_craft_level); } }
        }

        private Int32 _sub_archer_level;
        /// <summary></summary>
        [DisplayName("Archer_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(90, "sub_archer_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_archer_level
        {
            get { return _sub_archer_level; }
            set { if (OnPropertyChanging(__.sub_archer_level, value)) { _sub_archer_level = value; OnPropertyChanged(__.sub_archer_level); } }
        }

        private Int32 _sub_etiquette_level;
        /// <summary></summary>
        [DisplayName("Etiquette_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(91, "sub_etiquette_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_level
        {
            get { return _sub_etiquette_level; }
            set { if (OnPropertyChanging(__.sub_etiquette_level, value)) { _sub_etiquette_level = value; OnPropertyChanged(__.sub_etiquette_level); } }
        }

        private Int32 _sub_martial_level;
        /// <summary></summary>
        [DisplayName("Martial_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(92, "sub_martial_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_martial_level
        {
            get { return _sub_martial_level; }
            set { if (OnPropertyChanging(__.sub_martial_level, value)) { _sub_martial_level = value; OnPropertyChanged(__.sub_martial_level); } }
        }

        private Int32 _sub_tactical_level;
        /// <summary></summary>
        [DisplayName("Tactical_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(93, "sub_tactical_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tactical_level
        {
            get { return _sub_tactical_level; }
            set { if (OnPropertyChanging(__.sub_tactical_level, value)) { _sub_tactical_level = value; OnPropertyChanged(__.sub_tactical_level); } }
        }

        private Int32 _sub_medical_level;
        /// <summary></summary>
        [DisplayName("Medical_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(94, "sub_medical_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_medical_level
        {
            get { return _sub_medical_level; }
            set { if (OnPropertyChanging(__.sub_medical_level, value)) { _sub_medical_level = value; OnPropertyChanged(__.sub_medical_level); } }
        }

        private Int32 _sub_ninjitsu_level;
        /// <summary></summary>
        [DisplayName("Ninjitsu_Level")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(95, "sub_ninjitsu_level", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_level
        {
            get { return _sub_ninjitsu_level; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_level, value)) { _sub_ninjitsu_level = value; OnPropertyChanged(__.sub_ninjitsu_level); } }
        }

        private Int32 _sub_tea_state;
        /// <summary></summary>
        [DisplayName("Tea_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(96, "sub_tea_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tea_state
        {
            get { return _sub_tea_state; }
            set { if (OnPropertyChanging(__.sub_tea_state, value)) { _sub_tea_state = value; OnPropertyChanged(__.sub_tea_state); } }
        }

        private Int32 _sub_calculate_state;
        /// <summary></summary>
        [DisplayName("Calculate_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(97, "sub_calculate_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_calculate_state
        {
            get { return _sub_calculate_state; }
            set { if (OnPropertyChanging(__.sub_calculate_state, value)) { _sub_calculate_state = value; OnPropertyChanged(__.sub_calculate_state); } }
        }

        private Int32 _sub_build_state;
        /// <summary></summary>
        [DisplayName("Build_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(98, "sub_build_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_build_state
        {
            get { return _sub_build_state; }
            set { if (OnPropertyChanging(__.sub_build_state, value)) { _sub_build_state = value; OnPropertyChanged(__.sub_build_state); } }
        }

        private Int32 _sub_eloquence_state;
        /// <summary></summary>
        [DisplayName("Eloquence_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(99, "sub_eloquence_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_state
        {
            get { return _sub_eloquence_state; }
            set { if (OnPropertyChanging(__.sub_eloquence_state, value)) { _sub_eloquence_state = value; OnPropertyChanged(__.sub_eloquence_state); } }
        }

        private Int32 _sub_equestrian_state;
        /// <summary></summary>
        [DisplayName("Equestrian_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(100, "sub_equestrian_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_state
        {
            get { return _sub_equestrian_state; }
            set { if (OnPropertyChanging(__.sub_equestrian_state, value)) { _sub_equestrian_state = value; OnPropertyChanged(__.sub_equestrian_state); } }
        }

        private Int32 _sub_reclaimed_state;
        /// <summary></summary>
        [DisplayName("Reclaimed_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(101, "sub_reclaimed_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_state
        {
            get { return _sub_reclaimed_state; }
            set { if (OnPropertyChanging(__.sub_reclaimed_state, value)) { _sub_reclaimed_state = value; OnPropertyChanged(__.sub_reclaimed_state); } }
        }

        private Int32 _sub_ashigaru_state;
        /// <summary></summary>
        [DisplayName("Ashigaru_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(102, "sub_ashigaru_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_state
        {
            get { return _sub_ashigaru_state; }
            set { if (OnPropertyChanging(__.sub_ashigaru_state, value)) { _sub_ashigaru_state = value; OnPropertyChanged(__.sub_ashigaru_state); } }
        }

        private Int32 _sub_artillery_state;
        /// <summary></summary>
        [DisplayName("Artillery_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(103, "sub_artillery_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_artillery_state
        {
            get { return _sub_artillery_state; }
            set { if (OnPropertyChanging(__.sub_artillery_state, value)) { _sub_artillery_state = value; OnPropertyChanged(__.sub_artillery_state); } }
        }

        private Int32 _sub_mine_state;
        /// <summary></summary>
        [DisplayName("Mine_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(104, "sub_mine_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_mine_state
        {
            get { return _sub_mine_state; }
            set { if (OnPropertyChanging(__.sub_mine_state, value)) { _sub_mine_state = value; OnPropertyChanged(__.sub_mine_state); } }
        }

        private Int32 _sub_craft_state;
        /// <summary></summary>
        [DisplayName("Craft_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(105, "sub_craft_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_craft_state
        {
            get { return _sub_craft_state; }
            set { if (OnPropertyChanging(__.sub_craft_state, value)) { _sub_craft_state = value; OnPropertyChanged(__.sub_craft_state); } }
        }

        private Int32 _sub_archer_state;
        /// <summary></summary>
        [DisplayName("Archer_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(106, "sub_archer_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_archer_state
        {
            get { return _sub_archer_state; }
            set { if (OnPropertyChanging(__.sub_archer_state, value)) { _sub_archer_state = value; OnPropertyChanged(__.sub_archer_state); } }
        }

        private Int32 _sub_etiquette_state;
        /// <summary></summary>
        [DisplayName("Etiquette_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(107, "sub_etiquette_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_state
        {
            get { return _sub_etiquette_state; }
            set { if (OnPropertyChanging(__.sub_etiquette_state, value)) { _sub_etiquette_state = value; OnPropertyChanged(__.sub_etiquette_state); } }
        }

        private Int32 _sub_martial_state;
        /// <summary></summary>
        [DisplayName("Martial_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(108, "sub_martial_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_martial_state
        {
            get { return _sub_martial_state; }
            set { if (OnPropertyChanging(__.sub_martial_state, value)) { _sub_martial_state = value; OnPropertyChanged(__.sub_martial_state); } }
        }

        private Int32 _sub_tactical_state;
        /// <summary></summary>
        [DisplayName("Tactical_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(109, "sub_tactical_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_tactical_state
        {
            get { return _sub_tactical_state; }
            set { if (OnPropertyChanging(__.sub_tactical_state, value)) { _sub_tactical_state = value; OnPropertyChanged(__.sub_tactical_state); } }
        }

        private Int32 _sub_medical_state;
        /// <summary></summary>
        [DisplayName("Medical_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(110, "sub_medical_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_medical_state
        {
            get { return _sub_medical_state; }
            set { if (OnPropertyChanging(__.sub_medical_state, value)) { _sub_medical_state = value; OnPropertyChanged(__.sub_medical_state); } }
        }

        private Int32 _sub_ninjitsu_state;
        /// <summary></summary>
        [DisplayName("Ninjitsu_State")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(111, "sub_ninjitsu_state", "", null, "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_state
        {
            get { return _sub_ninjitsu_state; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_state, value)) { _sub_ninjitsu_state = value; OnPropertyChanged(__.sub_ninjitsu_state); } }
        }

        private Int64 _fid;
        /// <summary></summary>
        [DisplayName("Fid")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(112, "fid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 fid
        {
            get { return _fid; }
            set { if (OnPropertyChanging(__.fid, value)) { _fid = value; OnPropertyChanged(__.fid); } }
        }

        private Int32 _skill_id;
        /// <summary></summary>
        [DisplayName("ID3")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(113, "skill_id", "", null, "int", 10, 0, false)]
        public virtual Int32 skill_id
        {
            get { return _skill_id; }
            set { if (OnPropertyChanging(__.skill_id, value)) { _skill_id = value; OnPropertyChanged(__.skill_id); } }
        }

        private Int32 _skill_type;
        /// <summary></summary>
        [DisplayName("Type")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(114, "skill_type", "", null, "int", 10, 0, false)]
        public virtual Int32 skill_type
        {
            get { return _skill_type; }
            set { if (OnPropertyChanging(__.skill_type, value)) { _skill_type = value; OnPropertyChanged(__.skill_type); } }
        }

        private Int32 _skill_level;
        /// <summary></summary>
        [DisplayName("Level1")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(115, "skill_level", "", null, "int", 10, 0, false)]
        public virtual Int32 skill_level
        {
            get { return _skill_level; }
            set { if (OnPropertyChanging(__.skill_level, value)) { _skill_level = value; OnPropertyChanged(__.skill_level); } }
        }

        private Int64 _skill_time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(116, "skill_time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 skill_time
        {
            get { return _skill_time; }
            set { if (OnPropertyChanging(__.skill_time, value)) { _skill_time = value; OnPropertyChanged(__.skill_time); } }
        }

        private Int32 _skill_genre;
        /// <summary></summary>
        [DisplayName("Genre")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(117, "skill_genre", "", null, "int", 10, 0, false)]
        public virtual Int32 skill_genre
        {
            get { return _skill_genre; }
            set { if (OnPropertyChanging(__.skill_genre, value)) { _skill_genre = value; OnPropertyChanged(__.skill_genre); } }
        }

        private Int32 _role_genre;
        /// <summary></summary>
        [DisplayName("Genre1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(118, "role_genre", "", null, "int", 10, 0, false)]
        public virtual Int32 role_genre
        {
            get { return _role_genre; }
            set { if (OnPropertyChanging(__.role_genre, value)) { _role_genre = value; OnPropertyChanged(__.role_genre); } }
        }

        private Double _att_crit_probability;
        /// <summary></summary>
        [DisplayName("Crit_Probability")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(119, "att_crit_probability", "", null, "float", 53, 0, false)]
        public virtual Double att_crit_probability
        {
            get { return _att_crit_probability; }
            set { if (OnPropertyChanging(__.att_crit_probability, value)) { _att_crit_probability = value; OnPropertyChanged(__.att_crit_probability); } }
        }

        private Double _att_crit_addition;
        /// <summary></summary>
        [DisplayName("Crit_Addition")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(120, "att_crit_addition", "", null, "float", 53, 0, false)]
        public virtual Double att_crit_addition
        {
            get { return _att_crit_addition; }
            set { if (OnPropertyChanging(__.att_crit_addition, value)) { _att_crit_addition = value; OnPropertyChanged(__.att_crit_addition); } }
        }

        private Double _att_dodge_probability;
        /// <summary></summary>
        [DisplayName("Dodge_Probability")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(121, "att_dodge_probability", "", null, "float", 53, 0, false)]
        public virtual Double att_dodge_probability
        {
            get { return _att_dodge_probability; }
            set { if (OnPropertyChanging(__.att_dodge_probability, value)) { _att_dodge_probability = value; OnPropertyChanged(__.att_dodge_probability); } }
        }

        private Double _att_mystery_probability;
        /// <summary></summary>
        [DisplayName("Mystery_Probability")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(122, "att_mystery_probability", "", null, "float", 53, 0, false)]
        public virtual Double att_mystery_probability
        {
            get { return _att_mystery_probability; }
            set { if (OnPropertyChanging(__.att_mystery_probability, value)) { _att_mystery_probability = value; OnPropertyChanged(__.att_mystery_probability); } }
        }

        private Int32 _skill_state;
        /// <summary></summary>
        [DisplayName("State1")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(123, "skill_state", "", null, "int", 10, 0, false)]
        public virtual Int32 skill_state
        {
            get { return _skill_state; }
            set { if (OnPropertyChanging(__.skill_state, value)) { _skill_state = value; OnPropertyChanged(__.skill_state); } }
        }

        private Int32 _role_ninja;
        /// <summary></summary>
        [DisplayName("Ninja")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(124, "role_ninja", "", null, "int", 10, 0, false)]
        public virtual Int32 role_ninja
        {
            get { return _role_ninja; }
            set { if (OnPropertyChanging(__.role_ninja, value)) { _role_ninja = value; OnPropertyChanged(__.role_ninja); } }
        }

        private Double _base_captain_life;
        /// <summary></summary>
        [DisplayName("Captain_Life")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(125, "base_captain_life", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_life
        {
            get { return _base_captain_life; }
            set { if (OnPropertyChanging(__.base_captain_life, value)) { _base_captain_life = value; OnPropertyChanged(__.base_captain_life); } }
        }

        private Double _base_captain_train;
        /// <summary></summary>
        [DisplayName("Captain_Train")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(126, "base_captain_train", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_train
        {
            get { return _base_captain_train; }
            set { if (OnPropertyChanging(__.base_captain_train, value)) { _base_captain_train = value; OnPropertyChanged(__.base_captain_train); } }
        }

        private Double _base_captain_level;
        /// <summary></summary>
        [DisplayName("Captain_Level")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(127, "base_captain_level", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_level
        {
            get { return _base_captain_level; }
            set { if (OnPropertyChanging(__.base_captain_level, value)) { _base_captain_level = value; OnPropertyChanged(__.base_captain_level); } }
        }

        private Double _base_captain_spirit;
        /// <summary></summary>
        [DisplayName("Captain_Spirit")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(128, "base_captain_spirit", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_spirit
        {
            get { return _base_captain_spirit; }
            set { if (OnPropertyChanging(__.base_captain_spirit, value)) { _base_captain_spirit = value; OnPropertyChanged(__.base_captain_spirit); } }
        }

        private Double _base_captain_equip;
        /// <summary></summary>
        [DisplayName("Captain_Equip")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(129, "base_captain_equip", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_equip
        {
            get { return _base_captain_equip; }
            set { if (OnPropertyChanging(__.base_captain_equip, value)) { _base_captain_equip = value; OnPropertyChanged(__.base_captain_equip); } }
        }

        private Double _base_captain_title;
        /// <summary></summary>
        [DisplayName("Captain_Title")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(130, "base_captain_title", "", null, "float", 53, 0, false)]
        public virtual Double base_captain_title
        {
            get { return _base_captain_title; }
            set { if (OnPropertyChanging(__.base_captain_title, value)) { _base_captain_title = value; OnPropertyChanged(__.base_captain_title); } }
        }

        private Double _base_force_life;
        /// <summary></summary>
        [DisplayName("Force_Life")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(131, "base_force_life", "", null, "float", 53, 0, false)]
        public virtual Double base_force_life
        {
            get { return _base_force_life; }
            set { if (OnPropertyChanging(__.base_force_life, value)) { _base_force_life = value; OnPropertyChanged(__.base_force_life); } }
        }

        private Double _base_force_train;
        /// <summary></summary>
        [DisplayName("Force_Train")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(132, "base_force_train", "", null, "float", 53, 0, false)]
        public virtual Double base_force_train
        {
            get { return _base_force_train; }
            set { if (OnPropertyChanging(__.base_force_train, value)) { _base_force_train = value; OnPropertyChanged(__.base_force_train); } }
        }

        private Double _base_force_level;
        /// <summary></summary>
        [DisplayName("Force_Level")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(133, "base_force_level", "", null, "float", 53, 0, false)]
        public virtual Double base_force_level
        {
            get { return _base_force_level; }
            set { if (OnPropertyChanging(__.base_force_level, value)) { _base_force_level = value; OnPropertyChanged(__.base_force_level); } }
        }

        private Double _base_force_spirit;
        /// <summary></summary>
        [DisplayName("Force_Spirit")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(134, "base_force_spirit", "", null, "float", 53, 0, false)]
        public virtual Double base_force_spirit
        {
            get { return _base_force_spirit; }
            set { if (OnPropertyChanging(__.base_force_spirit, value)) { _base_force_spirit = value; OnPropertyChanged(__.base_force_spirit); } }
        }

        private Double _base_force_equip;
        /// <summary></summary>
        [DisplayName("Force_Equip")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(135, "base_force_equip", "", null, "float", 53, 0, false)]
        public virtual Double base_force_equip
        {
            get { return _base_force_equip; }
            set { if (OnPropertyChanging(__.base_force_equip, value)) { _base_force_equip = value; OnPropertyChanged(__.base_force_equip); } }
        }

        private Double _base_force_title;
        /// <summary></summary>
        [DisplayName("Force_Title")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(136, "base_force_title", "", null, "float", 53, 0, false)]
        public virtual Double base_force_title
        {
            get { return _base_force_title; }
            set { if (OnPropertyChanging(__.base_force_title, value)) { _base_force_title = value; OnPropertyChanged(__.base_force_title); } }
        }

        private Double _base_brains_life;
        /// <summary></summary>
        [DisplayName("Brains_Life")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(137, "base_brains_life", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_life
        {
            get { return _base_brains_life; }
            set { if (OnPropertyChanging(__.base_brains_life, value)) { _base_brains_life = value; OnPropertyChanged(__.base_brains_life); } }
        }

        private Double _base_brains_train;
        /// <summary></summary>
        [DisplayName("Brains_Train")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(138, "base_brains_train", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_train
        {
            get { return _base_brains_train; }
            set { if (OnPropertyChanging(__.base_brains_train, value)) { _base_brains_train = value; OnPropertyChanged(__.base_brains_train); } }
        }

        private Double _base_brains_level;
        /// <summary></summary>
        [DisplayName("Brains_Level")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(139, "base_brains_level", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_level
        {
            get { return _base_brains_level; }
            set { if (OnPropertyChanging(__.base_brains_level, value)) { _base_brains_level = value; OnPropertyChanged(__.base_brains_level); } }
        }

        private Double _base_brains_spirit;
        /// <summary></summary>
        [DisplayName("Brains_Spirit")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(140, "base_brains_spirit", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_spirit
        {
            get { return _base_brains_spirit; }
            set { if (OnPropertyChanging(__.base_brains_spirit, value)) { _base_brains_spirit = value; OnPropertyChanged(__.base_brains_spirit); } }
        }

        private Double _base_brains_equip;
        /// <summary></summary>
        [DisplayName("Brains_Equip")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(141, "base_brains_equip", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_equip
        {
            get { return _base_brains_equip; }
            set { if (OnPropertyChanging(__.base_brains_equip, value)) { _base_brains_equip = value; OnPropertyChanged(__.base_brains_equip); } }
        }

        private Double _base_brains_title;
        /// <summary></summary>
        [DisplayName("Brains_Title")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(142, "base_brains_title", "", null, "float", 53, 0, false)]
        public virtual Double base_brains_title
        {
            get { return _base_brains_title; }
            set { if (OnPropertyChanging(__.base_brains_title, value)) { _base_brains_title = value; OnPropertyChanged(__.base_brains_title); } }
        }

        private Double _base_charm_life;
        /// <summary></summary>
        [DisplayName("Charm_Life")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(143, "base_charm_life", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_life
        {
            get { return _base_charm_life; }
            set { if (OnPropertyChanging(__.base_charm_life, value)) { _base_charm_life = value; OnPropertyChanged(__.base_charm_life); } }
        }

        private Double _base_charm_train;
        /// <summary></summary>
        [DisplayName("Charm_Train")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(144, "base_charm_train", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_train
        {
            get { return _base_charm_train; }
            set { if (OnPropertyChanging(__.base_charm_train, value)) { _base_charm_train = value; OnPropertyChanged(__.base_charm_train); } }
        }

        private Double _base_charm_level;
        /// <summary></summary>
        [DisplayName("Charm_Level")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(145, "base_charm_level", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_level
        {
            get { return _base_charm_level; }
            set { if (OnPropertyChanging(__.base_charm_level, value)) { _base_charm_level = value; OnPropertyChanged(__.base_charm_level); } }
        }

        private Double _base_charm_spirit;
        /// <summary></summary>
        [DisplayName("Charm_Spirit")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(146, "base_charm_spirit", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_spirit
        {
            get { return _base_charm_spirit; }
            set { if (OnPropertyChanging(__.base_charm_spirit, value)) { _base_charm_spirit = value; OnPropertyChanged(__.base_charm_spirit); } }
        }

        private Double _base_charm_equip;
        /// <summary></summary>
        [DisplayName("Charm_Equip")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(147, "base_charm_equip", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_equip
        {
            get { return _base_charm_equip; }
            set { if (OnPropertyChanging(__.base_charm_equip, value)) { _base_charm_equip = value; OnPropertyChanged(__.base_charm_equip); } }
        }

        private Double _base_charm_title;
        /// <summary></summary>
        [DisplayName("Charm_Title")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(148, "base_charm_title", "", null, "float", 53, 0, false)]
        public virtual Double base_charm_title
        {
            get { return _base_charm_title; }
            set { if (OnPropertyChanging(__.base_charm_title, value)) { _base_charm_title = value; OnPropertyChanged(__.base_charm_title); } }
        }

        private Double _base_govern_life;
        /// <summary></summary>
        [DisplayName("Govern_Life")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(149, "base_govern_life", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_life
        {
            get { return _base_govern_life; }
            set { if (OnPropertyChanging(__.base_govern_life, value)) { _base_govern_life = value; OnPropertyChanged(__.base_govern_life); } }
        }

        private Double _base_govern_train;
        /// <summary></summary>
        [DisplayName("Govern_Train")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(150, "base_govern_train", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_train
        {
            get { return _base_govern_train; }
            set { if (OnPropertyChanging(__.base_govern_train, value)) { _base_govern_train = value; OnPropertyChanged(__.base_govern_train); } }
        }

        private Double _base_govern_level;
        /// <summary></summary>
        [DisplayName("Govern_Level")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(151, "base_govern_level", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_level
        {
            get { return _base_govern_level; }
            set { if (OnPropertyChanging(__.base_govern_level, value)) { _base_govern_level = value; OnPropertyChanged(__.base_govern_level); } }
        }

        private Double _base_govern_spirit;
        /// <summary></summary>
        [DisplayName("Govern_Spirit")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(152, "base_govern_spirit", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_spirit
        {
            get { return _base_govern_spirit; }
            set { if (OnPropertyChanging(__.base_govern_spirit, value)) { _base_govern_spirit = value; OnPropertyChanged(__.base_govern_spirit); } }
        }

        private Double _base_govern_equip;
        /// <summary></summary>
        [DisplayName("Govern_Equip")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(153, "base_govern_equip", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_equip
        {
            get { return _base_govern_equip; }
            set { if (OnPropertyChanging(__.base_govern_equip, value)) { _base_govern_equip = value; OnPropertyChanged(__.base_govern_equip); } }
        }

        private Double _base_govern_title;
        /// <summary></summary>
        [DisplayName("Govern_Title")]
        [Description("")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(154, "base_govern_title", "", null, "float", 53, 0, false)]
        public virtual Double base_govern_title
        {
            get { return _base_govern_title; }
            set { if (OnPropertyChanging(__.base_govern_title, value)) { _base_govern_title = value; OnPropertyChanged(__.base_govern_title); } }
        }

        private Int32 _type_sub;
        /// <summary></summary>
        [DisplayName("Sub")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(155, "type_sub", "", null, "int", 10, 0, false)]
        public virtual Int32 type_sub
        {
            get { return _type_sub; }
            set { if (OnPropertyChanging(__.type_sub, value)) { _type_sub = value; OnPropertyChanged(__.type_sub); } }
        }

        private Int64 _title_sword;
        /// <summary></summary>
        [DisplayName("Sword")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(156, "title_sword", "", null, "bigint", 19, 0, false)]
        public virtual Int64 title_sword
        {
            get { return _title_sword; }
            set { if (OnPropertyChanging(__.title_sword, value)) { _title_sword = value; OnPropertyChanged(__.title_sword); } }
        }

        private Int64 _title_gun;
        /// <summary></summary>
        [DisplayName("Gun")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(157, "title_gun", "", null, "bigint", 19, 0, false)]
        public virtual Int64 title_gun
        {
            get { return _title_gun; }
            set { if (OnPropertyChanging(__.title_gun, value)) { _title_gun = value; OnPropertyChanged(__.title_gun); } }
        }

        private Int64 _title_tea;
        /// <summary></summary>
        [DisplayName("Tea2")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(158, "title_tea", "", null, "bigint", 19, 0, false)]
        public virtual Int64 title_tea
        {
            get { return _title_tea; }
            set { if (OnPropertyChanging(__.title_tea, value)) { _title_tea = value; OnPropertyChanged(__.title_tea); } }
        }

        private Int64 _title_eloquence;
        /// <summary></summary>
        [DisplayName("Eloquence1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(159, "title_eloquence", "", null, "bigint", 19, 0, false)]
        public virtual Int64 title_eloquence
        {
            get { return _title_eloquence; }
            set { if (OnPropertyChanging(__.title_eloquence, value)) { _title_eloquence = value; OnPropertyChanged(__.title_eloquence); } }
        }

        private Int32 _buff_power;
        /// <summary></summary>
        [DisplayName("Power1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(160, "buff_power", "", null, "int", 10, 0, false)]
        public virtual Int32 buff_power
        {
            get { return _buff_power; }
            set { if (OnPropertyChanging(__.buff_power, value)) { _buff_power = value; OnPropertyChanged(__.buff_power); } }
        }

        private Int32 _total_honor;
        /// <summary></summary>
        [DisplayName("Honor1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(161, "total_honor", "", null, "int", 10, 0, false)]
        public virtual Int32 total_honor
        {
            get { return _total_honor; }
            set { if (OnPropertyChanging(__.total_honor, value)) { _total_honor = value; OnPropertyChanged(__.total_honor); } }
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
                    case __.att_life : return _att_life;
                    case __.att_attack : return _att_attack;
                    case __.att_defense : return _att_defense;
                    case __.att_sub_hurtIncrease : return _att_sub_hurtIncrease;
                    case __.att_sub_hurtReduce : return _att_sub_hurtReduce;
                    case __.art_mystery : return _art_mystery;
                    case __.art_cheat_code : return _art_cheat_code;
                    case __.equip_weapon : return _equip_weapon;
                    case __.equip_barbarian : return _equip_barbarian;
                    case __.equip_mounts : return _equip_mounts;
                    case __.equip_armor : return _equip_armor;
                    case __.equip_gem : return _equip_gem;
                    case __.equip_craft : return _equip_craft;
                    case __.equip_tea : return _equip_tea;
                    case __.equip_book : return _equip_book;
                    case __.att_points : return _att_points;
                    case __.lid : return _lid;
                    case __.sub_tea : return _sub_tea;
                    case __.sub_calculate : return _sub_calculate;
                    case __.sub_build : return _sub_build;
                    case __.sub_eloquence : return _sub_eloquence;
                    case __.sub_equestrian : return _sub_equestrian;
                    case __.sub_reclaimed : return _sub_reclaimed;
                    case __.sub_ashigaru : return _sub_ashigaru;
                    case __.sub_artillery : return _sub_artillery;
                    case __.sub_mine : return _sub_mine;
                    case __.sub_craft : return _sub_craft;
                    case __.sub_archer : return _sub_archer;
                    case __.sub_etiquette : return _sub_etiquette;
                    case __.sub_martial : return _sub_martial;
                    case __.sub_tactical : return _sub_tactical;
                    case __.sub_medical : return _sub_medical;
                    case __.sub_ninjitsu : return _sub_ninjitsu;
                    case __.sub_tea_progress : return _sub_tea_progress;
                    case __.sub_calculate_progress : return _sub_calculate_progress;
                    case __.sub_build_progress : return _sub_build_progress;
                    case __.sub_eloquence_progress : return _sub_eloquence_progress;
                    case __.sub_equestrian_progress : return _sub_equestrian_progress;
                    case __.sub_reclaimed_progress : return _sub_reclaimed_progress;
                    case __.sub_ashigaru_progress : return _sub_ashigaru_progress;
                    case __.sub_artillery_progress : return _sub_artillery_progress;
                    case __.sub_mine_progress : return _sub_mine_progress;
                    case __.sub_craft_progress : return _sub_craft_progress;
                    case __.sub_archer_progress : return _sub_archer_progress;
                    case __.sub_etiquette_progress : return _sub_etiquette_progress;
                    case __.sub_martial_progress : return _sub_martial_progress;
                    case __.sub_tactical_progress : return _sub_tactical_progress;
                    case __.sub_medical_progress : return _sub_medical_progress;
                    case __.sub_ninjitsu_progress : return _sub_ninjitsu_progress;
                    case __.sub_tea_time : return _sub_tea_time;
                    case __.sub_calculate_time : return _sub_calculate_time;
                    case __.sub_build_time : return _sub_build_time;
                    case __.sub_eloquence_time : return _sub_eloquence_time;
                    case __.sub_equestrian_time : return _sub_equestrian_time;
                    case __.sub_reclaimed_time : return _sub_reclaimed_time;
                    case __.sub_ashigaru_time : return _sub_ashigaru_time;
                    case __.sub_artillery_time : return _sub_artillery_time;
                    case __.sub_mine_time : return _sub_mine_time;
                    case __.sub_craft_time : return _sub_craft_time;
                    case __.sub_archer_time : return _sub_archer_time;
                    case __.sub_etiquette_time : return _sub_etiquette_time;
                    case __.sub_martial_time : return _sub_martial_time;
                    case __.sub_tactical_time : return _sub_tactical_time;
                    case __.sub_medical_time : return _sub_medical_time;
                    case __.sub_ninjitsu_time : return _sub_ninjitsu_time;
                    case __.sub_tea_level : return _sub_tea_level;
                    case __.sub_calculate_level : return _sub_calculate_level;
                    case __.sub_build_level : return _sub_build_level;
                    case __.sub_eloquence_level : return _sub_eloquence_level;
                    case __.sub_equestrian_level : return _sub_equestrian_level;
                    case __.sub_reclaimed_level : return _sub_reclaimed_level;
                    case __.sub_ashigaru_level : return _sub_ashigaru_level;
                    case __.sub_artillery_level : return _sub_artillery_level;
                    case __.sub_mine_level : return _sub_mine_level;
                    case __.sub_craft_level : return _sub_craft_level;
                    case __.sub_archer_level : return _sub_archer_level;
                    case __.sub_etiquette_level : return _sub_etiquette_level;
                    case __.sub_martial_level : return _sub_martial_level;
                    case __.sub_tactical_level : return _sub_tactical_level;
                    case __.sub_medical_level : return _sub_medical_level;
                    case __.sub_ninjitsu_level : return _sub_ninjitsu_level;
                    case __.sub_tea_state : return _sub_tea_state;
                    case __.sub_calculate_state : return _sub_calculate_state;
                    case __.sub_build_state : return _sub_build_state;
                    case __.sub_eloquence_state : return _sub_eloquence_state;
                    case __.sub_equestrian_state : return _sub_equestrian_state;
                    case __.sub_reclaimed_state : return _sub_reclaimed_state;
                    case __.sub_ashigaru_state : return _sub_ashigaru_state;
                    case __.sub_artillery_state : return _sub_artillery_state;
                    case __.sub_mine_state : return _sub_mine_state;
                    case __.sub_craft_state : return _sub_craft_state;
                    case __.sub_archer_state : return _sub_archer_state;
                    case __.sub_etiquette_state : return _sub_etiquette_state;
                    case __.sub_martial_state : return _sub_martial_state;
                    case __.sub_tactical_state : return _sub_tactical_state;
                    case __.sub_medical_state : return _sub_medical_state;
                    case __.sub_ninjitsu_state : return _sub_ninjitsu_state;
                    case __.fid : return _fid;
                    case __.skill_id : return _skill_id;
                    case __.skill_type : return _skill_type;
                    case __.skill_level : return _skill_level;
                    case __.skill_time : return _skill_time;
                    case __.skill_genre : return _skill_genre;
                    case __.role_genre : return _role_genre;
                    case __.att_crit_probability : return _att_crit_probability;
                    case __.att_crit_addition : return _att_crit_addition;
                    case __.att_dodge_probability : return _att_dodge_probability;
                    case __.att_mystery_probability : return _att_mystery_probability;
                    case __.skill_state : return _skill_state;
                    case __.role_ninja : return _role_ninja;
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
                    case __.type_sub : return _type_sub;
                    case __.title_sword : return _title_sword;
                    case __.title_gun : return _title_gun;
                    case __.title_tea : return _title_tea;
                    case __.title_eloquence : return _title_eloquence;
                    case __.buff_power : return _buff_power;
                    case __.total_honor : return _total_honor;
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
                    case __.att_life : _att_life = Convert.ToInt32(value); break;
                    case __.att_attack : _att_attack = Convert.ToDouble(value); break;
                    case __.att_defense : _att_defense = Convert.ToDouble(value); break;
                    case __.att_sub_hurtIncrease : _att_sub_hurtIncrease = Convert.ToDouble(value); break;
                    case __.att_sub_hurtReduce : _att_sub_hurtReduce = Convert.ToDouble(value); break;
                    case __.art_mystery : _art_mystery = Convert.ToInt64(value); break;
                    case __.art_cheat_code : _art_cheat_code = Convert.ToInt64(value); break;
                    case __.equip_weapon : _equip_weapon = Convert.ToInt64(value); break;
                    case __.equip_barbarian : _equip_barbarian = Convert.ToInt64(value); break;
                    case __.equip_mounts : _equip_mounts = Convert.ToInt64(value); break;
                    case __.equip_armor : _equip_armor = Convert.ToInt64(value); break;
                    case __.equip_gem : _equip_gem = Convert.ToInt64(value); break;
                    case __.equip_craft : _equip_craft = Convert.ToInt64(value); break;
                    case __.equip_tea : _equip_tea = Convert.ToInt64(value); break;
                    case __.equip_book : _equip_book = Convert.ToInt64(value); break;
                    case __.att_points : _att_points = Convert.ToInt32(value); break;
                    case __.lid : _lid = Convert.ToInt64(value); break;
                    case __.sub_tea : _sub_tea = Convert.ToInt32(value); break;
                    case __.sub_calculate : _sub_calculate = Convert.ToInt32(value); break;
                    case __.sub_build : _sub_build = Convert.ToInt32(value); break;
                    case __.sub_eloquence : _sub_eloquence = Convert.ToInt32(value); break;
                    case __.sub_equestrian : _sub_equestrian = Convert.ToInt32(value); break;
                    case __.sub_reclaimed : _sub_reclaimed = Convert.ToInt32(value); break;
                    case __.sub_ashigaru : _sub_ashigaru = Convert.ToInt32(value); break;
                    case __.sub_artillery : _sub_artillery = Convert.ToInt32(value); break;
                    case __.sub_mine : _sub_mine = Convert.ToInt32(value); break;
                    case __.sub_craft : _sub_craft = Convert.ToInt32(value); break;
                    case __.sub_archer : _sub_archer = Convert.ToInt32(value); break;
                    case __.sub_etiquette : _sub_etiquette = Convert.ToInt32(value); break;
                    case __.sub_martial : _sub_martial = Convert.ToInt32(value); break;
                    case __.sub_tactical : _sub_tactical = Convert.ToInt32(value); break;
                    case __.sub_medical : _sub_medical = Convert.ToInt32(value); break;
                    case __.sub_ninjitsu : _sub_ninjitsu = Convert.ToInt32(value); break;
                    case __.sub_tea_progress : _sub_tea_progress = Convert.ToInt32(value); break;
                    case __.sub_calculate_progress : _sub_calculate_progress = Convert.ToInt32(value); break;
                    case __.sub_build_progress : _sub_build_progress = Convert.ToInt32(value); break;
                    case __.sub_eloquence_progress : _sub_eloquence_progress = Convert.ToInt32(value); break;
                    case __.sub_equestrian_progress : _sub_equestrian_progress = Convert.ToInt32(value); break;
                    case __.sub_reclaimed_progress : _sub_reclaimed_progress = Convert.ToInt32(value); break;
                    case __.sub_ashigaru_progress : _sub_ashigaru_progress = Convert.ToInt32(value); break;
                    case __.sub_artillery_progress : _sub_artillery_progress = Convert.ToInt32(value); break;
                    case __.sub_mine_progress : _sub_mine_progress = Convert.ToInt32(value); break;
                    case __.sub_craft_progress : _sub_craft_progress = Convert.ToInt32(value); break;
                    case __.sub_archer_progress : _sub_archer_progress = Convert.ToInt32(value); break;
                    case __.sub_etiquette_progress : _sub_etiquette_progress = Convert.ToInt32(value); break;
                    case __.sub_martial_progress : _sub_martial_progress = Convert.ToInt32(value); break;
                    case __.sub_tactical_progress : _sub_tactical_progress = Convert.ToInt32(value); break;
                    case __.sub_medical_progress : _sub_medical_progress = Convert.ToInt32(value); break;
                    case __.sub_ninjitsu_progress : _sub_ninjitsu_progress = Convert.ToInt32(value); break;
                    case __.sub_tea_time : _sub_tea_time = Convert.ToInt64(value); break;
                    case __.sub_calculate_time : _sub_calculate_time = Convert.ToInt64(value); break;
                    case __.sub_build_time : _sub_build_time = Convert.ToInt64(value); break;
                    case __.sub_eloquence_time : _sub_eloquence_time = Convert.ToInt64(value); break;
                    case __.sub_equestrian_time : _sub_equestrian_time = Convert.ToInt64(value); break;
                    case __.sub_reclaimed_time : _sub_reclaimed_time = Convert.ToInt64(value); break;
                    case __.sub_ashigaru_time : _sub_ashigaru_time = Convert.ToInt64(value); break;
                    case __.sub_artillery_time : _sub_artillery_time = Convert.ToInt64(value); break;
                    case __.sub_mine_time : _sub_mine_time = Convert.ToInt64(value); break;
                    case __.sub_craft_time : _sub_craft_time = Convert.ToInt64(value); break;
                    case __.sub_archer_time : _sub_archer_time = Convert.ToInt64(value); break;
                    case __.sub_etiquette_time : _sub_etiquette_time = Convert.ToInt64(value); break;
                    case __.sub_martial_time : _sub_martial_time = Convert.ToInt64(value); break;
                    case __.sub_tactical_time : _sub_tactical_time = Convert.ToInt64(value); break;
                    case __.sub_medical_time : _sub_medical_time = Convert.ToInt64(value); break;
                    case __.sub_ninjitsu_time : _sub_ninjitsu_time = Convert.ToInt64(value); break;
                    case __.sub_tea_level : _sub_tea_level = Convert.ToInt32(value); break;
                    case __.sub_calculate_level : _sub_calculate_level = Convert.ToInt32(value); break;
                    case __.sub_build_level : _sub_build_level = Convert.ToInt32(value); break;
                    case __.sub_eloquence_level : _sub_eloquence_level = Convert.ToInt32(value); break;
                    case __.sub_equestrian_level : _sub_equestrian_level = Convert.ToInt32(value); break;
                    case __.sub_reclaimed_level : _sub_reclaimed_level = Convert.ToInt32(value); break;
                    case __.sub_ashigaru_level : _sub_ashigaru_level = Convert.ToInt32(value); break;
                    case __.sub_artillery_level : _sub_artillery_level = Convert.ToInt32(value); break;
                    case __.sub_mine_level : _sub_mine_level = Convert.ToInt32(value); break;
                    case __.sub_craft_level : _sub_craft_level = Convert.ToInt32(value); break;
                    case __.sub_archer_level : _sub_archer_level = Convert.ToInt32(value); break;
                    case __.sub_etiquette_level : _sub_etiquette_level = Convert.ToInt32(value); break;
                    case __.sub_martial_level : _sub_martial_level = Convert.ToInt32(value); break;
                    case __.sub_tactical_level : _sub_tactical_level = Convert.ToInt32(value); break;
                    case __.sub_medical_level : _sub_medical_level = Convert.ToInt32(value); break;
                    case __.sub_ninjitsu_level : _sub_ninjitsu_level = Convert.ToInt32(value); break;
                    case __.sub_tea_state : _sub_tea_state = Convert.ToInt32(value); break;
                    case __.sub_calculate_state : _sub_calculate_state = Convert.ToInt32(value); break;
                    case __.sub_build_state : _sub_build_state = Convert.ToInt32(value); break;
                    case __.sub_eloquence_state : _sub_eloquence_state = Convert.ToInt32(value); break;
                    case __.sub_equestrian_state : _sub_equestrian_state = Convert.ToInt32(value); break;
                    case __.sub_reclaimed_state : _sub_reclaimed_state = Convert.ToInt32(value); break;
                    case __.sub_ashigaru_state : _sub_ashigaru_state = Convert.ToInt32(value); break;
                    case __.sub_artillery_state : _sub_artillery_state = Convert.ToInt32(value); break;
                    case __.sub_mine_state : _sub_mine_state = Convert.ToInt32(value); break;
                    case __.sub_craft_state : _sub_craft_state = Convert.ToInt32(value); break;
                    case __.sub_archer_state : _sub_archer_state = Convert.ToInt32(value); break;
                    case __.sub_etiquette_state : _sub_etiquette_state = Convert.ToInt32(value); break;
                    case __.sub_martial_state : _sub_martial_state = Convert.ToInt32(value); break;
                    case __.sub_tactical_state : _sub_tactical_state = Convert.ToInt32(value); break;
                    case __.sub_medical_state : _sub_medical_state = Convert.ToInt32(value); break;
                    case __.sub_ninjitsu_state : _sub_ninjitsu_state = Convert.ToInt32(value); break;
                    case __.fid : _fid = Convert.ToInt64(value); break;
                    case __.skill_id : _skill_id = Convert.ToInt32(value); break;
                    case __.skill_type : _skill_type = Convert.ToInt32(value); break;
                    case __.skill_level : _skill_level = Convert.ToInt32(value); break;
                    case __.skill_time : _skill_time = Convert.ToInt64(value); break;
                    case __.skill_genre : _skill_genre = Convert.ToInt32(value); break;
                    case __.role_genre : _role_genre = Convert.ToInt32(value); break;
                    case __.att_crit_probability : _att_crit_probability = Convert.ToDouble(value); break;
                    case __.att_crit_addition : _att_crit_addition = Convert.ToDouble(value); break;
                    case __.att_dodge_probability : _att_dodge_probability = Convert.ToDouble(value); break;
                    case __.att_mystery_probability : _att_mystery_probability = Convert.ToDouble(value); break;
                    case __.skill_state : _skill_state = Convert.ToInt32(value); break;
                    case __.role_ninja : _role_ninja = Convert.ToInt32(value); break;
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
                    case __.type_sub : _type_sub = Convert.ToInt32(value); break;
                    case __.title_sword : _title_sword = Convert.ToInt64(value); break;
                    case __.title_gun : _title_gun = Convert.ToInt64(value); break;
                    case __.title_tea : _title_tea = Convert.ToInt64(value); break;
                    case __.title_eloquence : _title_eloquence = Convert.ToInt64(value); break;
                    case __.buff_power : _buff_power = Convert.ToInt32(value); break;
                    case __.total_honor : _total_honor = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Role字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field role_id = FindByName(__.role_id);

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
            public static readonly Field base_captain = FindByName(__.base_captain);

            ///<summary></summary>
            public static readonly Field base_force = FindByName(__.base_force);

            ///<summary></summary>
            public static readonly Field base_brains = FindByName(__.base_brains);

            ///<summary></summary>
            public static readonly Field base_charm = FindByName(__.base_charm);

            ///<summary></summary>
            public static readonly Field base_govern = FindByName(__.base_govern);

            ///<summary></summary>
            public static readonly Field att_life = FindByName(__.att_life);

            ///<summary></summary>
            public static readonly Field att_attack = FindByName(__.att_attack);

            ///<summary></summary>
            public static readonly Field att_defense = FindByName(__.att_defense);

            ///<summary></summary>
            public static readonly Field att_sub_hurtIncrease = FindByName(__.att_sub_hurtIncrease);

            ///<summary></summary>
            public static readonly Field att_sub_hurtReduce = FindByName(__.att_sub_hurtReduce);

            ///<summary></summary>
            public static readonly Field art_mystery = FindByName(__.art_mystery);

            ///<summary></summary>
            public static readonly Field art_cheat_code = FindByName(__.art_cheat_code);

            ///<summary></summary>
            public static readonly Field equip_weapon = FindByName(__.equip_weapon);

            ///<summary></summary>
            public static readonly Field equip_barbarian = FindByName(__.equip_barbarian);

            ///<summary></summary>
            public static readonly Field equip_mounts = FindByName(__.equip_mounts);

            ///<summary></summary>
            public static readonly Field equip_armor = FindByName(__.equip_armor);

            ///<summary></summary>
            public static readonly Field equip_gem = FindByName(__.equip_gem);

            ///<summary></summary>
            public static readonly Field equip_craft = FindByName(__.equip_craft);

            ///<summary></summary>
            public static readonly Field equip_tea = FindByName(__.equip_tea);

            ///<summary></summary>
            public static readonly Field equip_book = FindByName(__.equip_book);

            ///<summary></summary>
            public static readonly Field att_points = FindByName(__.att_points);

            ///<summary></summary>
            public static readonly Field lid = FindByName(__.lid);

            ///<summary></summary>
            public static readonly Field sub_tea = FindByName(__.sub_tea);

            ///<summary></summary>
            public static readonly Field sub_calculate = FindByName(__.sub_calculate);

            ///<summary></summary>
            public static readonly Field sub_build = FindByName(__.sub_build);

            ///<summary></summary>
            public static readonly Field sub_eloquence = FindByName(__.sub_eloquence);

            ///<summary></summary>
            public static readonly Field sub_equestrian = FindByName(__.sub_equestrian);

            ///<summary></summary>
            public static readonly Field sub_reclaimed = FindByName(__.sub_reclaimed);

            ///<summary></summary>
            public static readonly Field sub_ashigaru = FindByName(__.sub_ashigaru);

            ///<summary></summary>
            public static readonly Field sub_artillery = FindByName(__.sub_artillery);

            ///<summary></summary>
            public static readonly Field sub_mine = FindByName(__.sub_mine);

            ///<summary></summary>
            public static readonly Field sub_craft = FindByName(__.sub_craft);

            ///<summary></summary>
            public static readonly Field sub_archer = FindByName(__.sub_archer);

            ///<summary></summary>
            public static readonly Field sub_etiquette = FindByName(__.sub_etiquette);

            ///<summary></summary>
            public static readonly Field sub_martial = FindByName(__.sub_martial);

            ///<summary></summary>
            public static readonly Field sub_tactical = FindByName(__.sub_tactical);

            ///<summary></summary>
            public static readonly Field sub_medical = FindByName(__.sub_medical);

            ///<summary></summary>
            public static readonly Field sub_ninjitsu = FindByName(__.sub_ninjitsu);

            ///<summary></summary>
            public static readonly Field sub_tea_progress = FindByName(__.sub_tea_progress);

            ///<summary></summary>
            public static readonly Field sub_calculate_progress = FindByName(__.sub_calculate_progress);

            ///<summary></summary>
            public static readonly Field sub_build_progress = FindByName(__.sub_build_progress);

            ///<summary></summary>
            public static readonly Field sub_eloquence_progress = FindByName(__.sub_eloquence_progress);

            ///<summary></summary>
            public static readonly Field sub_equestrian_progress = FindByName(__.sub_equestrian_progress);

            ///<summary></summary>
            public static readonly Field sub_reclaimed_progress = FindByName(__.sub_reclaimed_progress);

            ///<summary></summary>
            public static readonly Field sub_ashigaru_progress = FindByName(__.sub_ashigaru_progress);

            ///<summary></summary>
            public static readonly Field sub_artillery_progress = FindByName(__.sub_artillery_progress);

            ///<summary></summary>
            public static readonly Field sub_mine_progress = FindByName(__.sub_mine_progress);

            ///<summary></summary>
            public static readonly Field sub_craft_progress = FindByName(__.sub_craft_progress);

            ///<summary></summary>
            public static readonly Field sub_archer_progress = FindByName(__.sub_archer_progress);

            ///<summary></summary>
            public static readonly Field sub_etiquette_progress = FindByName(__.sub_etiquette_progress);

            ///<summary></summary>
            public static readonly Field sub_martial_progress = FindByName(__.sub_martial_progress);

            ///<summary></summary>
            public static readonly Field sub_tactical_progress = FindByName(__.sub_tactical_progress);

            ///<summary></summary>
            public static readonly Field sub_medical_progress = FindByName(__.sub_medical_progress);

            ///<summary></summary>
            public static readonly Field sub_ninjitsu_progress = FindByName(__.sub_ninjitsu_progress);

            ///<summary></summary>
            public static readonly Field sub_tea_time = FindByName(__.sub_tea_time);

            ///<summary></summary>
            public static readonly Field sub_calculate_time = FindByName(__.sub_calculate_time);

            ///<summary></summary>
            public static readonly Field sub_build_time = FindByName(__.sub_build_time);

            ///<summary></summary>
            public static readonly Field sub_eloquence_time = FindByName(__.sub_eloquence_time);

            ///<summary></summary>
            public static readonly Field sub_equestrian_time = FindByName(__.sub_equestrian_time);

            ///<summary></summary>
            public static readonly Field sub_reclaimed_time = FindByName(__.sub_reclaimed_time);

            ///<summary></summary>
            public static readonly Field sub_ashigaru_time = FindByName(__.sub_ashigaru_time);

            ///<summary></summary>
            public static readonly Field sub_artillery_time = FindByName(__.sub_artillery_time);

            ///<summary></summary>
            public static readonly Field sub_mine_time = FindByName(__.sub_mine_time);

            ///<summary></summary>
            public static readonly Field sub_craft_time = FindByName(__.sub_craft_time);

            ///<summary></summary>
            public static readonly Field sub_archer_time = FindByName(__.sub_archer_time);

            ///<summary></summary>
            public static readonly Field sub_etiquette_time = FindByName(__.sub_etiquette_time);

            ///<summary></summary>
            public static readonly Field sub_martial_time = FindByName(__.sub_martial_time);

            ///<summary></summary>
            public static readonly Field sub_tactical_time = FindByName(__.sub_tactical_time);

            ///<summary></summary>
            public static readonly Field sub_medical_time = FindByName(__.sub_medical_time);

            ///<summary></summary>
            public static readonly Field sub_ninjitsu_time = FindByName(__.sub_ninjitsu_time);

            ///<summary></summary>
            public static readonly Field sub_tea_level = FindByName(__.sub_tea_level);

            ///<summary></summary>
            public static readonly Field sub_calculate_level = FindByName(__.sub_calculate_level);

            ///<summary></summary>
            public static readonly Field sub_build_level = FindByName(__.sub_build_level);

            ///<summary></summary>
            public static readonly Field sub_eloquence_level = FindByName(__.sub_eloquence_level);

            ///<summary></summary>
            public static readonly Field sub_equestrian_level = FindByName(__.sub_equestrian_level);

            ///<summary></summary>
            public static readonly Field sub_reclaimed_level = FindByName(__.sub_reclaimed_level);

            ///<summary></summary>
            public static readonly Field sub_ashigaru_level = FindByName(__.sub_ashigaru_level);

            ///<summary></summary>
            public static readonly Field sub_artillery_level = FindByName(__.sub_artillery_level);

            ///<summary></summary>
            public static readonly Field sub_mine_level = FindByName(__.sub_mine_level);

            ///<summary></summary>
            public static readonly Field sub_craft_level = FindByName(__.sub_craft_level);

            ///<summary></summary>
            public static readonly Field sub_archer_level = FindByName(__.sub_archer_level);

            ///<summary></summary>
            public static readonly Field sub_etiquette_level = FindByName(__.sub_etiquette_level);

            ///<summary></summary>
            public static readonly Field sub_martial_level = FindByName(__.sub_martial_level);

            ///<summary></summary>
            public static readonly Field sub_tactical_level = FindByName(__.sub_tactical_level);

            ///<summary></summary>
            public static readonly Field sub_medical_level = FindByName(__.sub_medical_level);

            ///<summary></summary>
            public static readonly Field sub_ninjitsu_level = FindByName(__.sub_ninjitsu_level);

            ///<summary></summary>
            public static readonly Field sub_tea_state = FindByName(__.sub_tea_state);

            ///<summary></summary>
            public static readonly Field sub_calculate_state = FindByName(__.sub_calculate_state);

            ///<summary></summary>
            public static readonly Field sub_build_state = FindByName(__.sub_build_state);

            ///<summary></summary>
            public static readonly Field sub_eloquence_state = FindByName(__.sub_eloquence_state);

            ///<summary></summary>
            public static readonly Field sub_equestrian_state = FindByName(__.sub_equestrian_state);

            ///<summary></summary>
            public static readonly Field sub_reclaimed_state = FindByName(__.sub_reclaimed_state);

            ///<summary></summary>
            public static readonly Field sub_ashigaru_state = FindByName(__.sub_ashigaru_state);

            ///<summary></summary>
            public static readonly Field sub_artillery_state = FindByName(__.sub_artillery_state);

            ///<summary></summary>
            public static readonly Field sub_mine_state = FindByName(__.sub_mine_state);

            ///<summary></summary>
            public static readonly Field sub_craft_state = FindByName(__.sub_craft_state);

            ///<summary></summary>
            public static readonly Field sub_archer_state = FindByName(__.sub_archer_state);

            ///<summary></summary>
            public static readonly Field sub_etiquette_state = FindByName(__.sub_etiquette_state);

            ///<summary></summary>
            public static readonly Field sub_martial_state = FindByName(__.sub_martial_state);

            ///<summary></summary>
            public static readonly Field sub_tactical_state = FindByName(__.sub_tactical_state);

            ///<summary></summary>
            public static readonly Field sub_medical_state = FindByName(__.sub_medical_state);

            ///<summary></summary>
            public static readonly Field sub_ninjitsu_state = FindByName(__.sub_ninjitsu_state);

            ///<summary></summary>
            public static readonly Field fid = FindByName(__.fid);

            ///<summary></summary>
            public static readonly Field skill_id = FindByName(__.skill_id);

            ///<summary></summary>
            public static readonly Field skill_type = FindByName(__.skill_type);

            ///<summary></summary>
            public static readonly Field skill_level = FindByName(__.skill_level);

            ///<summary></summary>
            public static readonly Field skill_time = FindByName(__.skill_time);

            ///<summary></summary>
            public static readonly Field skill_genre = FindByName(__.skill_genre);

            ///<summary></summary>
            public static readonly Field role_genre = FindByName(__.role_genre);

            ///<summary></summary>
            public static readonly Field att_crit_probability = FindByName(__.att_crit_probability);

            ///<summary></summary>
            public static readonly Field att_crit_addition = FindByName(__.att_crit_addition);

            ///<summary></summary>
            public static readonly Field att_dodge_probability = FindByName(__.att_dodge_probability);

            ///<summary></summary>
            public static readonly Field att_mystery_probability = FindByName(__.att_mystery_probability);

            ///<summary></summary>
            public static readonly Field skill_state = FindByName(__.skill_state);

            ///<summary></summary>
            public static readonly Field role_ninja = FindByName(__.role_ninja);

            ///<summary></summary>
            public static readonly Field base_captain_life = FindByName(__.base_captain_life);

            ///<summary></summary>
            public static readonly Field base_captain_train = FindByName(__.base_captain_train);

            ///<summary></summary>
            public static readonly Field base_captain_level = FindByName(__.base_captain_level);

            ///<summary></summary>
            public static readonly Field base_captain_spirit = FindByName(__.base_captain_spirit);

            ///<summary></summary>
            public static readonly Field base_captain_equip = FindByName(__.base_captain_equip);

            ///<summary></summary>
            public static readonly Field base_captain_title = FindByName(__.base_captain_title);

            ///<summary></summary>
            public static readonly Field base_force_life = FindByName(__.base_force_life);

            ///<summary></summary>
            public static readonly Field base_force_train = FindByName(__.base_force_train);

            ///<summary></summary>
            public static readonly Field base_force_level = FindByName(__.base_force_level);

            ///<summary></summary>
            public static readonly Field base_force_spirit = FindByName(__.base_force_spirit);

            ///<summary></summary>
            public static readonly Field base_force_equip = FindByName(__.base_force_equip);

            ///<summary></summary>
            public static readonly Field base_force_title = FindByName(__.base_force_title);

            ///<summary></summary>
            public static readonly Field base_brains_life = FindByName(__.base_brains_life);

            ///<summary></summary>
            public static readonly Field base_brains_train = FindByName(__.base_brains_train);

            ///<summary></summary>
            public static readonly Field base_brains_level = FindByName(__.base_brains_level);

            ///<summary></summary>
            public static readonly Field base_brains_spirit = FindByName(__.base_brains_spirit);

            ///<summary></summary>
            public static readonly Field base_brains_equip = FindByName(__.base_brains_equip);

            ///<summary></summary>
            public static readonly Field base_brains_title = FindByName(__.base_brains_title);

            ///<summary></summary>
            public static readonly Field base_charm_life = FindByName(__.base_charm_life);

            ///<summary></summary>
            public static readonly Field base_charm_train = FindByName(__.base_charm_train);

            ///<summary></summary>
            public static readonly Field base_charm_level = FindByName(__.base_charm_level);

            ///<summary></summary>
            public static readonly Field base_charm_spirit = FindByName(__.base_charm_spirit);

            ///<summary></summary>
            public static readonly Field base_charm_equip = FindByName(__.base_charm_equip);

            ///<summary></summary>
            public static readonly Field base_charm_title = FindByName(__.base_charm_title);

            ///<summary></summary>
            public static readonly Field base_govern_life = FindByName(__.base_govern_life);

            ///<summary></summary>
            public static readonly Field base_govern_train = FindByName(__.base_govern_train);

            ///<summary></summary>
            public static readonly Field base_govern_level = FindByName(__.base_govern_level);

            ///<summary></summary>
            public static readonly Field base_govern_spirit = FindByName(__.base_govern_spirit);

            ///<summary></summary>
            public static readonly Field base_govern_equip = FindByName(__.base_govern_equip);

            ///<summary></summary>
            public static readonly Field base_govern_title = FindByName(__.base_govern_title);

            ///<summary></summary>
            public static readonly Field type_sub = FindByName(__.type_sub);

            ///<summary></summary>
            public static readonly Field title_sword = FindByName(__.title_sword);

            ///<summary></summary>
            public static readonly Field title_gun = FindByName(__.title_gun);

            ///<summary></summary>
            public static readonly Field title_tea = FindByName(__.title_tea);

            ///<summary></summary>
            public static readonly Field title_eloquence = FindByName(__.title_eloquence);

            ///<summary></summary>
            public static readonly Field buff_power = FindByName(__.buff_power);

            ///<summary></summary>
            public static readonly Field total_honor = FindByName(__.total_honor);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Role字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String role_id = "role_id";

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
            public const String base_captain = "base_captain";

            ///<summary></summary>
            public const String base_force = "base_force";

            ///<summary></summary>
            public const String base_brains = "base_brains";

            ///<summary></summary>
            public const String base_charm = "base_charm";

            ///<summary></summary>
            public const String base_govern = "base_govern";

            ///<summary></summary>
            public const String att_life = "att_life";

            ///<summary></summary>
            public const String att_attack = "att_attack";

            ///<summary></summary>
            public const String att_defense = "att_defense";

            ///<summary></summary>
            public const String att_sub_hurtIncrease = "att_sub_hurtIncrease";

            ///<summary></summary>
            public const String att_sub_hurtReduce = "att_sub_hurtReduce";

            ///<summary></summary>
            public const String art_mystery = "art_mystery";

            ///<summary></summary>
            public const String art_cheat_code = "art_cheat_code";

            ///<summary></summary>
            public const String equip_weapon = "equip_weapon";

            ///<summary></summary>
            public const String equip_barbarian = "equip_barbarian";

            ///<summary></summary>
            public const String equip_mounts = "equip_mounts";

            ///<summary></summary>
            public const String equip_armor = "equip_armor";

            ///<summary></summary>
            public const String equip_gem = "equip_gem";

            ///<summary></summary>
            public const String equip_craft = "equip_craft";

            ///<summary></summary>
            public const String equip_tea = "equip_tea";

            ///<summary></summary>
            public const String equip_book = "equip_book";

            ///<summary></summary>
            public const String att_points = "att_points";

            ///<summary></summary>
            public const String lid = "lid";

            ///<summary></summary>
            public const String sub_tea = "sub_tea";

            ///<summary></summary>
            public const String sub_calculate = "sub_calculate";

            ///<summary></summary>
            public const String sub_build = "sub_build";

            ///<summary></summary>
            public const String sub_eloquence = "sub_eloquence";

            ///<summary></summary>
            public const String sub_equestrian = "sub_equestrian";

            ///<summary></summary>
            public const String sub_reclaimed = "sub_reclaimed";

            ///<summary></summary>
            public const String sub_ashigaru = "sub_ashigaru";

            ///<summary></summary>
            public const String sub_artillery = "sub_artillery";

            ///<summary></summary>
            public const String sub_mine = "sub_mine";

            ///<summary></summary>
            public const String sub_craft = "sub_craft";

            ///<summary></summary>
            public const String sub_archer = "sub_archer";

            ///<summary></summary>
            public const String sub_etiquette = "sub_etiquette";

            ///<summary></summary>
            public const String sub_martial = "sub_martial";

            ///<summary></summary>
            public const String sub_tactical = "sub_tactical";

            ///<summary></summary>
            public const String sub_medical = "sub_medical";

            ///<summary></summary>
            public const String sub_ninjitsu = "sub_ninjitsu";

            ///<summary></summary>
            public const String sub_tea_progress = "sub_tea_progress";

            ///<summary></summary>
            public const String sub_calculate_progress = "sub_calculate_progress";

            ///<summary></summary>
            public const String sub_build_progress = "sub_build_progress";

            ///<summary></summary>
            public const String sub_eloquence_progress = "sub_eloquence_progress";

            ///<summary></summary>
            public const String sub_equestrian_progress = "sub_equestrian_progress";

            ///<summary></summary>
            public const String sub_reclaimed_progress = "sub_reclaimed_progress";

            ///<summary></summary>
            public const String sub_ashigaru_progress = "sub_ashigaru_progress";

            ///<summary></summary>
            public const String sub_artillery_progress = "sub_artillery_progress";

            ///<summary></summary>
            public const String sub_mine_progress = "sub_mine_progress";

            ///<summary></summary>
            public const String sub_craft_progress = "sub_craft_progress";

            ///<summary></summary>
            public const String sub_archer_progress = "sub_archer_progress";

            ///<summary></summary>
            public const String sub_etiquette_progress = "sub_etiquette_progress";

            ///<summary></summary>
            public const String sub_martial_progress = "sub_martial_progress";

            ///<summary></summary>
            public const String sub_tactical_progress = "sub_tactical_progress";

            ///<summary></summary>
            public const String sub_medical_progress = "sub_medical_progress";

            ///<summary></summary>
            public const String sub_ninjitsu_progress = "sub_ninjitsu_progress";

            ///<summary></summary>
            public const String sub_tea_time = "sub_tea_time";

            ///<summary></summary>
            public const String sub_calculate_time = "sub_calculate_time";

            ///<summary></summary>
            public const String sub_build_time = "sub_build_time";

            ///<summary></summary>
            public const String sub_eloquence_time = "sub_eloquence_time";

            ///<summary></summary>
            public const String sub_equestrian_time = "sub_equestrian_time";

            ///<summary></summary>
            public const String sub_reclaimed_time = "sub_reclaimed_time";

            ///<summary></summary>
            public const String sub_ashigaru_time = "sub_ashigaru_time";

            ///<summary></summary>
            public const String sub_artillery_time = "sub_artillery_time";

            ///<summary></summary>
            public const String sub_mine_time = "sub_mine_time";

            ///<summary></summary>
            public const String sub_craft_time = "sub_craft_time";

            ///<summary></summary>
            public const String sub_archer_time = "sub_archer_time";

            ///<summary></summary>
            public const String sub_etiquette_time = "sub_etiquette_time";

            ///<summary></summary>
            public const String sub_martial_time = "sub_martial_time";

            ///<summary></summary>
            public const String sub_tactical_time = "sub_tactical_time";

            ///<summary></summary>
            public const String sub_medical_time = "sub_medical_time";

            ///<summary></summary>
            public const String sub_ninjitsu_time = "sub_ninjitsu_time";

            ///<summary></summary>
            public const String sub_tea_level = "sub_tea_level";

            ///<summary></summary>
            public const String sub_calculate_level = "sub_calculate_level";

            ///<summary></summary>
            public const String sub_build_level = "sub_build_level";

            ///<summary></summary>
            public const String sub_eloquence_level = "sub_eloquence_level";

            ///<summary></summary>
            public const String sub_equestrian_level = "sub_equestrian_level";

            ///<summary></summary>
            public const String sub_reclaimed_level = "sub_reclaimed_level";

            ///<summary></summary>
            public const String sub_ashigaru_level = "sub_ashigaru_level";

            ///<summary></summary>
            public const String sub_artillery_level = "sub_artillery_level";

            ///<summary></summary>
            public const String sub_mine_level = "sub_mine_level";

            ///<summary></summary>
            public const String sub_craft_level = "sub_craft_level";

            ///<summary></summary>
            public const String sub_archer_level = "sub_archer_level";

            ///<summary></summary>
            public const String sub_etiquette_level = "sub_etiquette_level";

            ///<summary></summary>
            public const String sub_martial_level = "sub_martial_level";

            ///<summary></summary>
            public const String sub_tactical_level = "sub_tactical_level";

            ///<summary></summary>
            public const String sub_medical_level = "sub_medical_level";

            ///<summary></summary>
            public const String sub_ninjitsu_level = "sub_ninjitsu_level";

            ///<summary></summary>
            public const String sub_tea_state = "sub_tea_state";

            ///<summary></summary>
            public const String sub_calculate_state = "sub_calculate_state";

            ///<summary></summary>
            public const String sub_build_state = "sub_build_state";

            ///<summary></summary>
            public const String sub_eloquence_state = "sub_eloquence_state";

            ///<summary></summary>
            public const String sub_equestrian_state = "sub_equestrian_state";

            ///<summary></summary>
            public const String sub_reclaimed_state = "sub_reclaimed_state";

            ///<summary></summary>
            public const String sub_ashigaru_state = "sub_ashigaru_state";

            ///<summary></summary>
            public const String sub_artillery_state = "sub_artillery_state";

            ///<summary></summary>
            public const String sub_mine_state = "sub_mine_state";

            ///<summary></summary>
            public const String sub_craft_state = "sub_craft_state";

            ///<summary></summary>
            public const String sub_archer_state = "sub_archer_state";

            ///<summary></summary>
            public const String sub_etiquette_state = "sub_etiquette_state";

            ///<summary></summary>
            public const String sub_martial_state = "sub_martial_state";

            ///<summary></summary>
            public const String sub_tactical_state = "sub_tactical_state";

            ///<summary></summary>
            public const String sub_medical_state = "sub_medical_state";

            ///<summary></summary>
            public const String sub_ninjitsu_state = "sub_ninjitsu_state";

            ///<summary></summary>
            public const String fid = "fid";

            ///<summary></summary>
            public const String skill_id = "skill_id";

            ///<summary></summary>
            public const String skill_type = "skill_type";

            ///<summary></summary>
            public const String skill_level = "skill_level";

            ///<summary></summary>
            public const String skill_time = "skill_time";

            ///<summary></summary>
            public const String skill_genre = "skill_genre";

            ///<summary></summary>
            public const String role_genre = "role_genre";

            ///<summary></summary>
            public const String att_crit_probability = "att_crit_probability";

            ///<summary></summary>
            public const String att_crit_addition = "att_crit_addition";

            ///<summary></summary>
            public const String att_dodge_probability = "att_dodge_probability";

            ///<summary></summary>
            public const String att_mystery_probability = "att_mystery_probability";

            ///<summary></summary>
            public const String skill_state = "skill_state";

            ///<summary></summary>
            public const String role_ninja = "role_ninja";

            ///<summary></summary>
            public const String base_captain_life = "base_captain_life";

            ///<summary></summary>
            public const String base_captain_train = "base_captain_train";

            ///<summary></summary>
            public const String base_captain_level = "base_captain_level";

            ///<summary></summary>
            public const String base_captain_spirit = "base_captain_spirit";

            ///<summary></summary>
            public const String base_captain_equip = "base_captain_equip";

            ///<summary></summary>
            public const String base_captain_title = "base_captain_title";

            ///<summary></summary>
            public const String base_force_life = "base_force_life";

            ///<summary></summary>
            public const String base_force_train = "base_force_train";

            ///<summary></summary>
            public const String base_force_level = "base_force_level";

            ///<summary></summary>
            public const String base_force_spirit = "base_force_spirit";

            ///<summary></summary>
            public const String base_force_equip = "base_force_equip";

            ///<summary></summary>
            public const String base_force_title = "base_force_title";

            ///<summary></summary>
            public const String base_brains_life = "base_brains_life";

            ///<summary></summary>
            public const String base_brains_train = "base_brains_train";

            ///<summary></summary>
            public const String base_brains_level = "base_brains_level";

            ///<summary></summary>
            public const String base_brains_spirit = "base_brains_spirit";

            ///<summary></summary>
            public const String base_brains_equip = "base_brains_equip";

            ///<summary></summary>
            public const String base_brains_title = "base_brains_title";

            ///<summary></summary>
            public const String base_charm_life = "base_charm_life";

            ///<summary></summary>
            public const String base_charm_train = "base_charm_train";

            ///<summary></summary>
            public const String base_charm_level = "base_charm_level";

            ///<summary></summary>
            public const String base_charm_spirit = "base_charm_spirit";

            ///<summary></summary>
            public const String base_charm_equip = "base_charm_equip";

            ///<summary></summary>
            public const String base_charm_title = "base_charm_title";

            ///<summary></summary>
            public const String base_govern_life = "base_govern_life";

            ///<summary></summary>
            public const String base_govern_train = "base_govern_train";

            ///<summary></summary>
            public const String base_govern_level = "base_govern_level";

            ///<summary></summary>
            public const String base_govern_spirit = "base_govern_spirit";

            ///<summary></summary>
            public const String base_govern_equip = "base_govern_equip";

            ///<summary></summary>
            public const String base_govern_title = "base_govern_title";

            ///<summary></summary>
            public const String type_sub = "type_sub";

            ///<summary></summary>
            public const String title_sword = "title_sword";

            ///<summary></summary>
            public const String title_gun = "title_gun";

            ///<summary></summary>
            public const String title_tea = "title_tea";

            ///<summary></summary>
            public const String title_eloquence = "title_eloquence";

            ///<summary></summary>
            public const String buff_power = "buff_power";

            ///<summary></summary>
            public const String total_honor = "total_honor";

        }
        #endregion
    }

    /// <summary>Role接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_role
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        Int32 role_id { get; set; }

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
        Double base_captain { get; set; }

        /// <summary></summary>
        Double base_force { get; set; }

        /// <summary></summary>
        Double base_brains { get; set; }

        /// <summary></summary>
        Double base_charm { get; set; }

        /// <summary></summary>
        Double base_govern { get; set; }

        /// <summary></summary>
        Int32 att_life { get; set; }

        /// <summary></summary>
        Double att_attack { get; set; }

        /// <summary></summary>
        Double att_defense { get; set; }

        /// <summary></summary>
        Double att_sub_hurtIncrease { get; set; }

        /// <summary></summary>
        Double att_sub_hurtReduce { get; set; }

        /// <summary></summary>
        Int64 art_mystery { get; set; }

        /// <summary></summary>
        Int64 art_cheat_code { get; set; }

        /// <summary></summary>
        Int64 equip_weapon { get; set; }

        /// <summary></summary>
        Int64 equip_barbarian { get; set; }

        /// <summary></summary>
        Int64 equip_mounts { get; set; }

        /// <summary></summary>
        Int64 equip_armor { get; set; }

        /// <summary></summary>
        Int64 equip_gem { get; set; }

        /// <summary></summary>
        Int64 equip_craft { get; set; }

        /// <summary></summary>
        Int64 equip_tea { get; set; }

        /// <summary></summary>
        Int64 equip_book { get; set; }

        /// <summary></summary>
        Int32 att_points { get; set; }

        /// <summary></summary>
        Int64 lid { get; set; }

        /// <summary></summary>
        Int32 sub_tea { get; set; }

        /// <summary></summary>
        Int32 sub_calculate { get; set; }

        /// <summary></summary>
        Int32 sub_build { get; set; }

        /// <summary></summary>
        Int32 sub_eloquence { get; set; }

        /// <summary></summary>
        Int32 sub_equestrian { get; set; }

        /// <summary></summary>
        Int32 sub_reclaimed { get; set; }

        /// <summary></summary>
        Int32 sub_ashigaru { get; set; }

        /// <summary></summary>
        Int32 sub_artillery { get; set; }

        /// <summary></summary>
        Int32 sub_mine { get; set; }

        /// <summary></summary>
        Int32 sub_craft { get; set; }

        /// <summary></summary>
        Int32 sub_archer { get; set; }

        /// <summary></summary>
        Int32 sub_etiquette { get; set; }

        /// <summary></summary>
        Int32 sub_martial { get; set; }

        /// <summary></summary>
        Int32 sub_tactical { get; set; }

        /// <summary></summary>
        Int32 sub_medical { get; set; }

        /// <summary></summary>
        Int32 sub_ninjitsu { get; set; }

        /// <summary></summary>
        Int32 sub_tea_progress { get; set; }

        /// <summary></summary>
        Int32 sub_calculate_progress { get; set; }

        /// <summary></summary>
        Int32 sub_build_progress { get; set; }

        /// <summary></summary>
        Int32 sub_eloquence_progress { get; set; }

        /// <summary></summary>
        Int32 sub_equestrian_progress { get; set; }

        /// <summary></summary>
        Int32 sub_reclaimed_progress { get; set; }

        /// <summary></summary>
        Int32 sub_ashigaru_progress { get; set; }

        /// <summary></summary>
        Int32 sub_artillery_progress { get; set; }

        /// <summary></summary>
        Int32 sub_mine_progress { get; set; }

        /// <summary></summary>
        Int32 sub_craft_progress { get; set; }

        /// <summary></summary>
        Int32 sub_archer_progress { get; set; }

        /// <summary></summary>
        Int32 sub_etiquette_progress { get; set; }

        /// <summary></summary>
        Int32 sub_martial_progress { get; set; }

        /// <summary></summary>
        Int32 sub_tactical_progress { get; set; }

        /// <summary></summary>
        Int32 sub_medical_progress { get; set; }

        /// <summary></summary>
        Int32 sub_ninjitsu_progress { get; set; }

        /// <summary></summary>
        Int64 sub_tea_time { get; set; }

        /// <summary></summary>
        Int64 sub_calculate_time { get; set; }

        /// <summary></summary>
        Int64 sub_build_time { get; set; }

        /// <summary></summary>
        Int64 sub_eloquence_time { get; set; }

        /// <summary></summary>
        Int64 sub_equestrian_time { get; set; }

        /// <summary></summary>
        Int64 sub_reclaimed_time { get; set; }

        /// <summary></summary>
        Int64 sub_ashigaru_time { get; set; }

        /// <summary></summary>
        Int64 sub_artillery_time { get; set; }

        /// <summary></summary>
        Int64 sub_mine_time { get; set; }

        /// <summary></summary>
        Int64 sub_craft_time { get; set; }

        /// <summary></summary>
        Int64 sub_archer_time { get; set; }

        /// <summary></summary>
        Int64 sub_etiquette_time { get; set; }

        /// <summary></summary>
        Int64 sub_martial_time { get; set; }

        /// <summary></summary>
        Int64 sub_tactical_time { get; set; }

        /// <summary></summary>
        Int64 sub_medical_time { get; set; }

        /// <summary></summary>
        Int64 sub_ninjitsu_time { get; set; }

        /// <summary></summary>
        Int32 sub_tea_level { get; set; }

        /// <summary></summary>
        Int32 sub_calculate_level { get; set; }

        /// <summary></summary>
        Int32 sub_build_level { get; set; }

        /// <summary></summary>
        Int32 sub_eloquence_level { get; set; }

        /// <summary></summary>
        Int32 sub_equestrian_level { get; set; }

        /// <summary></summary>
        Int32 sub_reclaimed_level { get; set; }

        /// <summary></summary>
        Int32 sub_ashigaru_level { get; set; }

        /// <summary></summary>
        Int32 sub_artillery_level { get; set; }

        /// <summary></summary>
        Int32 sub_mine_level { get; set; }

        /// <summary></summary>
        Int32 sub_craft_level { get; set; }

        /// <summary></summary>
        Int32 sub_archer_level { get; set; }

        /// <summary></summary>
        Int32 sub_etiquette_level { get; set; }

        /// <summary></summary>
        Int32 sub_martial_level { get; set; }

        /// <summary></summary>
        Int32 sub_tactical_level { get; set; }

        /// <summary></summary>
        Int32 sub_medical_level { get; set; }

        /// <summary></summary>
        Int32 sub_ninjitsu_level { get; set; }

        /// <summary></summary>
        Int32 sub_tea_state { get; set; }

        /// <summary></summary>
        Int32 sub_calculate_state { get; set; }

        /// <summary></summary>
        Int32 sub_build_state { get; set; }

        /// <summary></summary>
        Int32 sub_eloquence_state { get; set; }

        /// <summary></summary>
        Int32 sub_equestrian_state { get; set; }

        /// <summary></summary>
        Int32 sub_reclaimed_state { get; set; }

        /// <summary></summary>
        Int32 sub_ashigaru_state { get; set; }

        /// <summary></summary>
        Int32 sub_artillery_state { get; set; }

        /// <summary></summary>
        Int32 sub_mine_state { get; set; }

        /// <summary></summary>
        Int32 sub_craft_state { get; set; }

        /// <summary></summary>
        Int32 sub_archer_state { get; set; }

        /// <summary></summary>
        Int32 sub_etiquette_state { get; set; }

        /// <summary></summary>
        Int32 sub_martial_state { get; set; }

        /// <summary></summary>
        Int32 sub_tactical_state { get; set; }

        /// <summary></summary>
        Int32 sub_medical_state { get; set; }

        /// <summary></summary>
        Int32 sub_ninjitsu_state { get; set; }

        /// <summary></summary>
        Int64 fid { get; set; }

        /// <summary></summary>
        Int32 skill_id { get; set; }

        /// <summary></summary>
        Int32 skill_type { get; set; }

        /// <summary></summary>
        Int32 skill_level { get; set; }

        /// <summary></summary>
        Int64 skill_time { get; set; }

        /// <summary></summary>
        Int32 skill_genre { get; set; }

        /// <summary></summary>
        Int32 role_genre { get; set; }

        /// <summary></summary>
        Double att_crit_probability { get; set; }

        /// <summary></summary>
        Double att_crit_addition { get; set; }

        /// <summary></summary>
        Double att_dodge_probability { get; set; }

        /// <summary></summary>
        Double att_mystery_probability { get; set; }

        /// <summary></summary>
        Int32 skill_state { get; set; }

        /// <summary></summary>
        Int32 role_ninja { get; set; }

        /// <summary></summary>
        Double base_captain_life { get; set; }

        /// <summary></summary>
        Double base_captain_train { get; set; }

        /// <summary></summary>
        Double base_captain_level { get; set; }

        /// <summary></summary>
        Double base_captain_spirit { get; set; }

        /// <summary></summary>
        Double base_captain_equip { get; set; }

        /// <summary></summary>
        Double base_captain_title { get; set; }

        /// <summary></summary>
        Double base_force_life { get; set; }

        /// <summary></summary>
        Double base_force_train { get; set; }

        /// <summary></summary>
        Double base_force_level { get; set; }

        /// <summary></summary>
        Double base_force_spirit { get; set; }

        /// <summary></summary>
        Double base_force_equip { get; set; }

        /// <summary></summary>
        Double base_force_title { get; set; }

        /// <summary></summary>
        Double base_brains_life { get; set; }

        /// <summary></summary>
        Double base_brains_train { get; set; }

        /// <summary></summary>
        Double base_brains_level { get; set; }

        /// <summary></summary>
        Double base_brains_spirit { get; set; }

        /// <summary></summary>
        Double base_brains_equip { get; set; }

        /// <summary></summary>
        Double base_brains_title { get; set; }

        /// <summary></summary>
        Double base_charm_life { get; set; }

        /// <summary></summary>
        Double base_charm_train { get; set; }

        /// <summary></summary>
        Double base_charm_level { get; set; }

        /// <summary></summary>
        Double base_charm_spirit { get; set; }

        /// <summary></summary>
        Double base_charm_equip { get; set; }

        /// <summary></summary>
        Double base_charm_title { get; set; }

        /// <summary></summary>
        Double base_govern_life { get; set; }

        /// <summary></summary>
        Double base_govern_train { get; set; }

        /// <summary></summary>
        Double base_govern_level { get; set; }

        /// <summary></summary>
        Double base_govern_spirit { get; set; }

        /// <summary></summary>
        Double base_govern_equip { get; set; }

        /// <summary></summary>
        Double base_govern_title { get; set; }

        /// <summary></summary>
        Int32 type_sub { get; set; }

        /// <summary></summary>
        Int64 title_sword { get; set; }

        /// <summary></summary>
        Int64 title_gun { get; set; }

        /// <summary></summary>
        Int64 title_tea { get; set; }

        /// <summary></summary>
        Int64 title_eloquence { get; set; }

        /// <summary></summary>
        Int32 buff_power { get; set; }

        /// <summary></summary>
        Int32 total_honor { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}