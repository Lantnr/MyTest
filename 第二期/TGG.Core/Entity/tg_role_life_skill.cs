using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>生活技能表</summary>
    [Serializable]
    [DataObject]
    [Description("生活技能表")]
    [BindIndex("PK_tg_role_life_skill", true, "id")]
    [BindTable("tg_role_life_skill", Description = "生活技能表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_role_life_skill : Itg_role_life_skill
    {
        #region 属性
        private Int64 _id;
        /// <summary>武将生活技能编号</summary>
        [DisplayName("武将生活技能编号")]
        [Description("武将生活技能编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "武将生活技能编号", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _rid;
        /// <summary>武将表编号</summary>
        [DisplayName("武将表编号")]
        [Description("武将表编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "rid", "武将表编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _sub_tea;
        /// <summary>茶道基表编号</summary>
        [DisplayName("茶道基表编号")]
        [Description("茶道基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "sub_tea", "茶道基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tea
        {
            get { return _sub_tea; }
            set { if (OnPropertyChanging(__.sub_tea, value)) { _sub_tea = value; OnPropertyChanged(__.sub_tea); } }
        }

        private Int32 _sub_calculate;
        /// <summary>算数基表编号</summary>
        [DisplayName("算数基表编号")]
        [Description("算数基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "sub_calculate", "算数基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_calculate
        {
            get { return _sub_calculate; }
            set { if (OnPropertyChanging(__.sub_calculate, value)) { _sub_calculate = value; OnPropertyChanged(__.sub_calculate); } }
        }

        private Int32 _sub_build;
        /// <summary>建筑基表编号</summary>
        [DisplayName("建筑基表编号")]
        [Description("建筑基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "sub_build", "建筑基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_build
        {
            get { return _sub_build; }
            set { if (OnPropertyChanging(__.sub_build, value)) { _sub_build = value; OnPropertyChanged(__.sub_build); } }
        }

        private Int32 _sub_eloquence;
        /// <summary>辩才基表编号</summary>
        [DisplayName("辩才基表编号")]
        [Description("辩才基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "sub_eloquence", "辩才基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_eloquence
        {
            get { return _sub_eloquence; }
            set { if (OnPropertyChanging(__.sub_eloquence, value)) { _sub_eloquence = value; OnPropertyChanged(__.sub_eloquence); } }
        }

        private Int32 _sub_equestrian;
        /// <summary>马术基表编号</summary>
        [DisplayName("马术基表编号")]
        [Description("马术基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "sub_equestrian", "马术基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_equestrian
        {
            get { return _sub_equestrian; }
            set { if (OnPropertyChanging(__.sub_equestrian, value)) { _sub_equestrian = value; OnPropertyChanged(__.sub_equestrian); } }
        }

        private Int32 _sub_reclaimed;
        /// <summary>开垦基表编号</summary>
        [DisplayName("开垦基表编号")]
        [Description("开垦基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "sub_reclaimed", "开垦基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed
        {
            get { return _sub_reclaimed; }
            set { if (OnPropertyChanging(__.sub_reclaimed, value)) { _sub_reclaimed = value; OnPropertyChanged(__.sub_reclaimed); } }
        }

        private Int32 _sub_ashigaru;
        /// <summary>足轻基表编号</summary>
        [DisplayName("足轻基表编号")]
        [Description("足轻基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "sub_ashigaru", "足轻基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru
        {
            get { return _sub_ashigaru; }
            set { if (OnPropertyChanging(__.sub_ashigaru, value)) { _sub_ashigaru = value; OnPropertyChanged(__.sub_ashigaru); } }
        }

        private Int32 _sub_artillery;
        /// <summary>铁炮基表编号</summary>
        [DisplayName("铁炮基表编号")]
        [Description("铁炮基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "sub_artillery", "铁炮基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_artillery
        {
            get { return _sub_artillery; }
            set { if (OnPropertyChanging(__.sub_artillery, value)) { _sub_artillery = value; OnPropertyChanged(__.sub_artillery); } }
        }

        private Int32 _sub_mine;
        /// <summary>矿山基表编号</summary>
        [DisplayName("矿山基表编号")]
        [Description("矿山基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "sub_mine", "矿山基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_mine
        {
            get { return _sub_mine; }
            set { if (OnPropertyChanging(__.sub_mine, value)) { _sub_mine = value; OnPropertyChanged(__.sub_mine); } }
        }

        private Int32 _sub_craft;
        /// <summary>艺术基表编号</summary>
        [DisplayName("艺术基表编号")]
        [Description("艺术基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "sub_craft", "艺术基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_craft
        {
            get { return _sub_craft; }
            set { if (OnPropertyChanging(__.sub_craft, value)) { _sub_craft = value; OnPropertyChanged(__.sub_craft); } }
        }

        private Int32 _sub_archer;
        /// <summary>弓术基表编号</summary>
        [DisplayName("弓术基表编号")]
        [Description("弓术基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "sub_archer", "弓术基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_archer
        {
            get { return _sub_archer; }
            set { if (OnPropertyChanging(__.sub_archer, value)) { _sub_archer = value; OnPropertyChanged(__.sub_archer); } }
        }

        private Int32 _sub_etiquette;
        /// <summary>礼法基表编号</summary>
        [DisplayName("礼法基表编号")]
        [Description("礼法基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "sub_etiquette", "礼法基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_etiquette
        {
            get { return _sub_etiquette; }
            set { if (OnPropertyChanging(__.sub_etiquette, value)) { _sub_etiquette = value; OnPropertyChanged(__.sub_etiquette); } }
        }

        private Int32 _sub_martial;
        /// <summary>武艺基表编号</summary>
        [DisplayName("武艺基表编号")]
        [Description("武艺基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "sub_martial", "武艺基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_martial
        {
            get { return _sub_martial; }
            set { if (OnPropertyChanging(__.sub_martial, value)) { _sub_martial = value; OnPropertyChanged(__.sub_martial); } }
        }

        private Int32 _sub_tactical;
        /// <summary>军学基表编号</summary>
        [DisplayName("军学基表编号")]
        [Description("军学基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "sub_tactical", "军学基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tactical
        {
            get { return _sub_tactical; }
            set { if (OnPropertyChanging(__.sub_tactical, value)) { _sub_tactical = value; OnPropertyChanged(__.sub_tactical); } }
        }

        private Int32 _sub_medical;
        /// <summary>医术基表编号</summary>
        [DisplayName("医术基表编号")]
        [Description("医术基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "sub_medical", "医术基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_medical
        {
            get { return _sub_medical; }
            set { if (OnPropertyChanging(__.sub_medical, value)) { _sub_medical = value; OnPropertyChanged(__.sub_medical); } }
        }

        private Int32 _sub_ninjitsu;
        /// <summary>忍术基表编号</summary>
        [DisplayName("忍术基表编号")]
        [Description("忍术基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "sub_ninjitsu", "忍术基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu
        {
            get { return _sub_ninjitsu; }
            set { if (OnPropertyChanging(__.sub_ninjitsu, value)) { _sub_ninjitsu = value; OnPropertyChanged(__.sub_ninjitsu); } }
        }

        private Int32 _sub_tea_progress;
        /// <summary>茶道当前熟练度</summary>
        [DisplayName("茶道当前熟练度")]
        [Description("茶道当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(19, "sub_tea_progress", "茶道当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tea_progress
        {
            get { return _sub_tea_progress; }
            set { if (OnPropertyChanging(__.sub_tea_progress, value)) { _sub_tea_progress = value; OnPropertyChanged(__.sub_tea_progress); } }
        }

        private Int32 _sub_calculate_progress;
        /// <summary>算数当前熟练度</summary>
        [DisplayName("算数当前熟练度")]
        [Description("算数当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(20, "sub_calculate_progress", "算数当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_calculate_progress
        {
            get { return _sub_calculate_progress; }
            set { if (OnPropertyChanging(__.sub_calculate_progress, value)) { _sub_calculate_progress = value; OnPropertyChanged(__.sub_calculate_progress); } }
        }

        private Int32 _sub_build_progress;
        /// <summary>建筑当前熟练度</summary>
        [DisplayName("建筑当前熟练度")]
        [Description("建筑当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(21, "sub_build_progress", "建筑当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_build_progress
        {
            get { return _sub_build_progress; }
            set { if (OnPropertyChanging(__.sub_build_progress, value)) { _sub_build_progress = value; OnPropertyChanged(__.sub_build_progress); } }
        }

        private Int32 _sub_eloquence_progress;
        /// <summary>口才当前熟练度</summary>
        [DisplayName("口才当前熟练度")]
        [Description("口才当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(22, "sub_eloquence_progress", "口才当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_progress
        {
            get { return _sub_eloquence_progress; }
            set { if (OnPropertyChanging(__.sub_eloquence_progress, value)) { _sub_eloquence_progress = value; OnPropertyChanged(__.sub_eloquence_progress); } }
        }

        private Int32 _sub_equestrian_progress;
        /// <summary>马术当前熟练度</summary>
        [DisplayName("马术当前熟练度")]
        [Description("马术当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(23, "sub_equestrian_progress", "马术当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_progress
        {
            get { return _sub_equestrian_progress; }
            set { if (OnPropertyChanging(__.sub_equestrian_progress, value)) { _sub_equestrian_progress = value; OnPropertyChanged(__.sub_equestrian_progress); } }
        }

        private Int32 _sub_reclaimed_progress;
        /// <summary>开垦当前熟练度</summary>
        [DisplayName("开垦当前熟练度")]
        [Description("开垦当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(24, "sub_reclaimed_progress", "开垦当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_progress
        {
            get { return _sub_reclaimed_progress; }
            set { if (OnPropertyChanging(__.sub_reclaimed_progress, value)) { _sub_reclaimed_progress = value; OnPropertyChanged(__.sub_reclaimed_progress); } }
        }

        private Int32 _sub_ashigaru_progress;
        /// <summary>足轻当前熟练度</summary>
        [DisplayName("足轻当前熟练度")]
        [Description("足轻当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(25, "sub_ashigaru_progress", "足轻当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_progress
        {
            get { return _sub_ashigaru_progress; }
            set { if (OnPropertyChanging(__.sub_ashigaru_progress, value)) { _sub_ashigaru_progress = value; OnPropertyChanged(__.sub_ashigaru_progress); } }
        }

        private Int32 _sub_artillery_progress;
        /// <summary>铁炮当前熟练度</summary>
        [DisplayName("铁炮当前熟练度")]
        [Description("铁炮当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(26, "sub_artillery_progress", "铁炮当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_artillery_progress
        {
            get { return _sub_artillery_progress; }
            set { if (OnPropertyChanging(__.sub_artillery_progress, value)) { _sub_artillery_progress = value; OnPropertyChanged(__.sub_artillery_progress); } }
        }

        private Int32 _sub_mine_progress;
        /// <summary>矿山当前熟练度</summary>
        [DisplayName("矿山当前熟练度")]
        [Description("矿山当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(27, "sub_mine_progress", "矿山当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_mine_progress
        {
            get { return _sub_mine_progress; }
            set { if (OnPropertyChanging(__.sub_mine_progress, value)) { _sub_mine_progress = value; OnPropertyChanged(__.sub_mine_progress); } }
        }

        private Int32 _sub_craft_progress;
        /// <summary>艺术当前熟练度</summary>
        [DisplayName("艺术当前熟练度")]
        [Description("艺术当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(28, "sub_craft_progress", "艺术当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_craft_progress
        {
            get { return _sub_craft_progress; }
            set { if (OnPropertyChanging(__.sub_craft_progress, value)) { _sub_craft_progress = value; OnPropertyChanged(__.sub_craft_progress); } }
        }

        private Int32 _sub_archer_progress;
        /// <summary>弓术当前熟练度</summary>
        [DisplayName("弓术当前熟练度")]
        [Description("弓术当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(29, "sub_archer_progress", "弓术当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_archer_progress
        {
            get { return _sub_archer_progress; }
            set { if (OnPropertyChanging(__.sub_archer_progress, value)) { _sub_archer_progress = value; OnPropertyChanged(__.sub_archer_progress); } }
        }

        private Int32 _sub_etiquette_progress;
        /// <summary>礼法当前熟练度</summary>
        [DisplayName("礼法当前熟练度")]
        [Description("礼法当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(30, "sub_etiquette_progress", "礼法当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_progress
        {
            get { return _sub_etiquette_progress; }
            set { if (OnPropertyChanging(__.sub_etiquette_progress, value)) { _sub_etiquette_progress = value; OnPropertyChanged(__.sub_etiquette_progress); } }
        }

        private Int32 _sub_martial_progress;
        /// <summary>武艺当前熟练度</summary>
        [DisplayName("武艺当前熟练度")]
        [Description("武艺当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(31, "sub_martial_progress", "武艺当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_martial_progress
        {
            get { return _sub_martial_progress; }
            set { if (OnPropertyChanging(__.sub_martial_progress, value)) { _sub_martial_progress = value; OnPropertyChanged(__.sub_martial_progress); } }
        }

        private Int32 _sub_tactical_progress;
        /// <summary>军学当前熟练度</summary>
        [DisplayName("军学当前熟练度")]
        [Description("军学当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(32, "sub_tactical_progress", "军学当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tactical_progress
        {
            get { return _sub_tactical_progress; }
            set { if (OnPropertyChanging(__.sub_tactical_progress, value)) { _sub_tactical_progress = value; OnPropertyChanged(__.sub_tactical_progress); } }
        }

        private Int32 _sub_medical_progress;
        /// <summary>医术当前熟练度</summary>
        [DisplayName("医术当前熟练度")]
        [Description("医术当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(33, "sub_medical_progress", "医术当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_medical_progress
        {
            get { return _sub_medical_progress; }
            set { if (OnPropertyChanging(__.sub_medical_progress, value)) { _sub_medical_progress = value; OnPropertyChanged(__.sub_medical_progress); } }
        }

        private Int32 _sub_ninjitsu_progress;
        /// <summary>忍术当前熟练度</summary>
        [DisplayName("忍术当前熟练度")]
        [Description("忍术当前熟练度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(34, "sub_ninjitsu_progress", "忍术当前熟练度", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_progress
        {
            get { return _sub_ninjitsu_progress; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_progress, value)) { _sub_ninjitsu_progress = value; OnPropertyChanged(__.sub_ninjitsu_progress); } }
        }

        private Int64 _sub_tea_time;
        /// <summary>茶道升级到达时间</summary>
        [DisplayName("茶道升级到达时间")]
        [Description("茶道升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(35, "sub_tea_time", "茶道升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_tea_time
        {
            get { return _sub_tea_time; }
            set { if (OnPropertyChanging(__.sub_tea_time, value)) { _sub_tea_time = value; OnPropertyChanged(__.sub_tea_time); } }
        }

        private Int64 _sub_calculate_time;
        /// <summary>算数升级到达时间</summary>
        [DisplayName("算数升级到达时间")]
        [Description("算数升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(36, "sub_calculate_time", "算数升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_calculate_time
        {
            get { return _sub_calculate_time; }
            set { if (OnPropertyChanging(__.sub_calculate_time, value)) { _sub_calculate_time = value; OnPropertyChanged(__.sub_calculate_time); } }
        }

        private Int64 _sub_build_time;
        /// <summary>建筑升级到达时间</summary>
        [DisplayName("建筑升级到达时间")]
        [Description("建筑升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(37, "sub_build_time", "建筑升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_build_time
        {
            get { return _sub_build_time; }
            set { if (OnPropertyChanging(__.sub_build_time, value)) { _sub_build_time = value; OnPropertyChanged(__.sub_build_time); } }
        }

        private Int64 _sub_eloquence_time;
        /// <summary>辩才升级到达时间</summary>
        [DisplayName("辩才升级到达时间")]
        [Description("辩才升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(38, "sub_eloquence_time", "辩才升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_eloquence_time
        {
            get { return _sub_eloquence_time; }
            set { if (OnPropertyChanging(__.sub_eloquence_time, value)) { _sub_eloquence_time = value; OnPropertyChanged(__.sub_eloquence_time); } }
        }

        private Int64 _sub_equestrian_time;
        /// <summary>马术升级到达时间</summary>
        [DisplayName("马术升级到达时间")]
        [Description("马术升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(39, "sub_equestrian_time", "马术升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_equestrian_time
        {
            get { return _sub_equestrian_time; }
            set { if (OnPropertyChanging(__.sub_equestrian_time, value)) { _sub_equestrian_time = value; OnPropertyChanged(__.sub_equestrian_time); } }
        }

        private Int64 _sub_reclaimed_time;
        /// <summary>开垦升级到达时间</summary>
        [DisplayName("开垦升级到达时间")]
        [Description("开垦升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(40, "sub_reclaimed_time", "开垦升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_reclaimed_time
        {
            get { return _sub_reclaimed_time; }
            set { if (OnPropertyChanging(__.sub_reclaimed_time, value)) { _sub_reclaimed_time = value; OnPropertyChanged(__.sub_reclaimed_time); } }
        }

        private Int64 _sub_ashigaru_time;
        /// <summary>足轻升级到达时间</summary>
        [DisplayName("足轻升级到达时间")]
        [Description("足轻升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(41, "sub_ashigaru_time", "足轻升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_ashigaru_time
        {
            get { return _sub_ashigaru_time; }
            set { if (OnPropertyChanging(__.sub_ashigaru_time, value)) { _sub_ashigaru_time = value; OnPropertyChanged(__.sub_ashigaru_time); } }
        }

        private Int64 _sub_artillery_time;
        /// <summary>铁炮升级到达时间</summary>
        [DisplayName("铁炮升级到达时间")]
        [Description("铁炮升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(42, "sub_artillery_time", "铁炮升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_artillery_time
        {
            get { return _sub_artillery_time; }
            set { if (OnPropertyChanging(__.sub_artillery_time, value)) { _sub_artillery_time = value; OnPropertyChanged(__.sub_artillery_time); } }
        }

        private Int64 _sub_mine_time;
        /// <summary>矿山升级到达时间</summary>
        [DisplayName("矿山升级到达时间")]
        [Description("矿山升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(43, "sub_mine_time", "矿山升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_mine_time
        {
            get { return _sub_mine_time; }
            set { if (OnPropertyChanging(__.sub_mine_time, value)) { _sub_mine_time = value; OnPropertyChanged(__.sub_mine_time); } }
        }

        private Int64 _sub_craft_time;
        /// <summary>艺术升级到达时间</summary>
        [DisplayName("艺术升级到达时间")]
        [Description("艺术升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(44, "sub_craft_time", "艺术升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_craft_time
        {
            get { return _sub_craft_time; }
            set { if (OnPropertyChanging(__.sub_craft_time, value)) { _sub_craft_time = value; OnPropertyChanged(__.sub_craft_time); } }
        }

        private Int64 _sub_archer_time;
        /// <summary>弓术升级到达时间</summary>
        [DisplayName("弓术升级到达时间")]
        [Description("弓术升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(45, "sub_archer_time", "弓术升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_archer_time
        {
            get { return _sub_archer_time; }
            set { if (OnPropertyChanging(__.sub_archer_time, value)) { _sub_archer_time = value; OnPropertyChanged(__.sub_archer_time); } }
        }

        private Int64 _sub_etiquette_time;
        /// <summary>礼法升级到达时间</summary>
        [DisplayName("礼法升级到达时间")]
        [Description("礼法升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(46, "sub_etiquette_time", "礼法升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_etiquette_time
        {
            get { return _sub_etiquette_time; }
            set { if (OnPropertyChanging(__.sub_etiquette_time, value)) { _sub_etiquette_time = value; OnPropertyChanged(__.sub_etiquette_time); } }
        }

        private Int64 _sub_martial_time;
        /// <summary>武艺升级到达时间</summary>
        [DisplayName("武艺升级到达时间")]
        [Description("武艺升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(47, "sub_martial_time", "武艺升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_martial_time
        {
            get { return _sub_martial_time; }
            set { if (OnPropertyChanging(__.sub_martial_time, value)) { _sub_martial_time = value; OnPropertyChanged(__.sub_martial_time); } }
        }

        private Int64 _sub_tactical_time;
        /// <summary>军学升级到达时间</summary>
        [DisplayName("军学升级到达时间")]
        [Description("军学升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(48, "sub_tactical_time", "军学升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_tactical_time
        {
            get { return _sub_tactical_time; }
            set { if (OnPropertyChanging(__.sub_tactical_time, value)) { _sub_tactical_time = value; OnPropertyChanged(__.sub_tactical_time); } }
        }

        private Int64 _sub_medical_time;
        /// <summary>医术升级到达时间</summary>
        [DisplayName("医术升级到达时间")]
        [Description("医术升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(49, "sub_medical_time", "医术升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_medical_time
        {
            get { return _sub_medical_time; }
            set { if (OnPropertyChanging(__.sub_medical_time, value)) { _sub_medical_time = value; OnPropertyChanged(__.sub_medical_time); } }
        }

        private Int64 _sub_ninjitsu_time;
        /// <summary>忍术升级到达时间</summary>
        [DisplayName("忍术升级到达时间")]
        [Description("忍术升级到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(50, "sub_ninjitsu_time", "忍术升级到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 sub_ninjitsu_time
        {
            get { return _sub_ninjitsu_time; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_time, value)) { _sub_ninjitsu_time = value; OnPropertyChanged(__.sub_ninjitsu_time); } }
        }

        private Int32 _sub_tea_level;
        /// <summary>茶道等级</summary>
        [DisplayName("茶道等级")]
        [Description("茶道等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(51, "sub_tea_level", "茶道等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tea_level
        {
            get { return _sub_tea_level; }
            set { if (OnPropertyChanging(__.sub_tea_level, value)) { _sub_tea_level = value; OnPropertyChanged(__.sub_tea_level); } }
        }

        private Int32 _sub_calculate_level;
        /// <summary>算数等级</summary>
        [DisplayName("算数等级")]
        [Description("算数等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(52, "sub_calculate_level", "算数等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_calculate_level
        {
            get { return _sub_calculate_level; }
            set { if (OnPropertyChanging(__.sub_calculate_level, value)) { _sub_calculate_level = value; OnPropertyChanged(__.sub_calculate_level); } }
        }

        private Int32 _sub_build_level;
        /// <summary>建筑等级</summary>
        [DisplayName("建筑等级")]
        [Description("建筑等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(53, "sub_build_level", "建筑等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_build_level
        {
            get { return _sub_build_level; }
            set { if (OnPropertyChanging(__.sub_build_level, value)) { _sub_build_level = value; OnPropertyChanged(__.sub_build_level); } }
        }

        private Int32 _sub_eloquence_level;
        /// <summary>辩才等级</summary>
        [DisplayName("辩才等级")]
        [Description("辩才等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(54, "sub_eloquence_level", "辩才等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_level
        {
            get { return _sub_eloquence_level; }
            set { if (OnPropertyChanging(__.sub_eloquence_level, value)) { _sub_eloquence_level = value; OnPropertyChanged(__.sub_eloquence_level); } }
        }

        private Int32 _sub_equestrian_level;
        /// <summary>马术等级</summary>
        [DisplayName("马术等级")]
        [Description("马术等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(55, "sub_equestrian_level", "马术等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_level
        {
            get { return _sub_equestrian_level; }
            set { if (OnPropertyChanging(__.sub_equestrian_level, value)) { _sub_equestrian_level = value; OnPropertyChanged(__.sub_equestrian_level); } }
        }

        private Int32 _sub_reclaimed_level;
        /// <summary>开垦等级</summary>
        [DisplayName("开垦等级")]
        [Description("开垦等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(56, "sub_reclaimed_level", "开垦等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_level
        {
            get { return _sub_reclaimed_level; }
            set { if (OnPropertyChanging(__.sub_reclaimed_level, value)) { _sub_reclaimed_level = value; OnPropertyChanged(__.sub_reclaimed_level); } }
        }

        private Int32 _sub_ashigaru_level;
        /// <summary>足轻等级</summary>
        [DisplayName("足轻等级")]
        [Description("足轻等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(57, "sub_ashigaru_level", "足轻等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_level
        {
            get { return _sub_ashigaru_level; }
            set { if (OnPropertyChanging(__.sub_ashigaru_level, value)) { _sub_ashigaru_level = value; OnPropertyChanged(__.sub_ashigaru_level); } }
        }

        private Int32 _sub_artillery_level;
        /// <summary>铁炮等级</summary>
        [DisplayName("铁炮等级")]
        [Description("铁炮等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(58, "sub_artillery_level", "铁炮等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_artillery_level
        {
            get { return _sub_artillery_level; }
            set { if (OnPropertyChanging(__.sub_artillery_level, value)) { _sub_artillery_level = value; OnPropertyChanged(__.sub_artillery_level); } }
        }

        private Int32 _sub_mine_level;
        /// <summary>矿山等级</summary>
        [DisplayName("矿山等级")]
        [Description("矿山等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(59, "sub_mine_level", "矿山等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_mine_level
        {
            get { return _sub_mine_level; }
            set { if (OnPropertyChanging(__.sub_mine_level, value)) { _sub_mine_level = value; OnPropertyChanged(__.sub_mine_level); } }
        }

        private Int32 _sub_craft_level;
        /// <summary>艺术等级</summary>
        [DisplayName("艺术等级")]
        [Description("艺术等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(60, "sub_craft_level", "艺术等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_craft_level
        {
            get { return _sub_craft_level; }
            set { if (OnPropertyChanging(__.sub_craft_level, value)) { _sub_craft_level = value; OnPropertyChanged(__.sub_craft_level); } }
        }

        private Int32 _sub_archer_level;
        /// <summary>弓术等级</summary>
        [DisplayName("弓术等级")]
        [Description("弓术等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(61, "sub_archer_level", "弓术等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_archer_level
        {
            get { return _sub_archer_level; }
            set { if (OnPropertyChanging(__.sub_archer_level, value)) { _sub_archer_level = value; OnPropertyChanged(__.sub_archer_level); } }
        }

        private Int32 _sub_etiquette_level;
        /// <summary>礼法等级</summary>
        [DisplayName("礼法等级")]
        [Description("礼法等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(62, "sub_etiquette_level", "礼法等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_level
        {
            get { return _sub_etiquette_level; }
            set { if (OnPropertyChanging(__.sub_etiquette_level, value)) { _sub_etiquette_level = value; OnPropertyChanged(__.sub_etiquette_level); } }
        }

        private Int32 _sub_martial_level;
        /// <summary>武艺等级</summary>
        [DisplayName("武艺等级")]
        [Description("武艺等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(63, "sub_martial_level", "武艺等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_martial_level
        {
            get { return _sub_martial_level; }
            set { if (OnPropertyChanging(__.sub_martial_level, value)) { _sub_martial_level = value; OnPropertyChanged(__.sub_martial_level); } }
        }

        private Int32 _sub_tactical_level;
        /// <summary>军学等级</summary>
        [DisplayName("军学等级")]
        [Description("军学等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(64, "sub_tactical_level", "军学等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tactical_level
        {
            get { return _sub_tactical_level; }
            set { if (OnPropertyChanging(__.sub_tactical_level, value)) { _sub_tactical_level = value; OnPropertyChanged(__.sub_tactical_level); } }
        }

        private Int32 _sub_medical_level;
        /// <summary>医术等级</summary>
        [DisplayName("医术等级")]
        [Description("医术等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(65, "sub_medical_level", "医术等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_medical_level
        {
            get { return _sub_medical_level; }
            set { if (OnPropertyChanging(__.sub_medical_level, value)) { _sub_medical_level = value; OnPropertyChanged(__.sub_medical_level); } }
        }

        private Int32 _sub_ninjitsu_level;
        /// <summary>忍术等级</summary>
        [DisplayName("忍术等级")]
        [Description("忍术等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(66, "sub_ninjitsu_level", "忍术等级", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_level
        {
            get { return _sub_ninjitsu_level; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_level, value)) { _sub_ninjitsu_level = value; OnPropertyChanged(__.sub_ninjitsu_level); } }
        }

        private Int32 _sub_tea_state;
        /// <summary>茶道状态</summary>
        [DisplayName("茶道状态")]
        [Description("茶道状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(67, "sub_tea_state", "茶道状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tea_state
        {
            get { return _sub_tea_state; }
            set { if (OnPropertyChanging(__.sub_tea_state, value)) { _sub_tea_state = value; OnPropertyChanged(__.sub_tea_state); } }
        }

        private Int32 _sub_calculate_state;
        /// <summary>算数状态</summary>
        [DisplayName("算数状态")]
        [Description("算数状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(68, "sub_calculate_state", "算数状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_calculate_state
        {
            get { return _sub_calculate_state; }
            set { if (OnPropertyChanging(__.sub_calculate_state, value)) { _sub_calculate_state = value; OnPropertyChanged(__.sub_calculate_state); } }
        }

        private Int32 _sub_build_state;
        /// <summary>建筑状态</summary>
        [DisplayName("建筑状态")]
        [Description("建筑状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(69, "sub_build_state", "建筑状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_build_state
        {
            get { return _sub_build_state; }
            set { if (OnPropertyChanging(__.sub_build_state, value)) { _sub_build_state = value; OnPropertyChanged(__.sub_build_state); } }
        }

        private Int32 _sub_eloquence_state;
        /// <summary>辩才状态</summary>
        [DisplayName("辩才状态")]
        [Description("辩才状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(70, "sub_eloquence_state", "辩才状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_eloquence_state
        {
            get { return _sub_eloquence_state; }
            set { if (OnPropertyChanging(__.sub_eloquence_state, value)) { _sub_eloquence_state = value; OnPropertyChanged(__.sub_eloquence_state); } }
        }

        private Int32 _sub_equestrian_state;
        /// <summary>马术状态</summary>
        [DisplayName("马术状态")]
        [Description("马术状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(71, "sub_equestrian_state", "马术状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_equestrian_state
        {
            get { return _sub_equestrian_state; }
            set { if (OnPropertyChanging(__.sub_equestrian_state, value)) { _sub_equestrian_state = value; OnPropertyChanged(__.sub_equestrian_state); } }
        }

        private Int32 _sub_reclaimed_state;
        /// <summary>开垦状态</summary>
        [DisplayName("开垦状态")]
        [Description("开垦状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(72, "sub_reclaimed_state", "开垦状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_reclaimed_state
        {
            get { return _sub_reclaimed_state; }
            set { if (OnPropertyChanging(__.sub_reclaimed_state, value)) { _sub_reclaimed_state = value; OnPropertyChanged(__.sub_reclaimed_state); } }
        }

        private Int32 _sub_ashigaru_state;
        /// <summary>足轻状态</summary>
        [DisplayName("足轻状态")]
        [Description("足轻状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(73, "sub_ashigaru_state", "足轻状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ashigaru_state
        {
            get { return _sub_ashigaru_state; }
            set { if (OnPropertyChanging(__.sub_ashigaru_state, value)) { _sub_ashigaru_state = value; OnPropertyChanged(__.sub_ashigaru_state); } }
        }

        private Int32 _sub_artillery_state;
        /// <summary>铁炮状态</summary>
        [DisplayName("铁炮状态")]
        [Description("铁炮状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(74, "sub_artillery_state", "铁炮状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_artillery_state
        {
            get { return _sub_artillery_state; }
            set { if (OnPropertyChanging(__.sub_artillery_state, value)) { _sub_artillery_state = value; OnPropertyChanged(__.sub_artillery_state); } }
        }

        private Int32 _sub_mine_state;
        /// <summary>矿山状态</summary>
        [DisplayName("矿山状态")]
        [Description("矿山状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(75, "sub_mine_state", "矿山状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_mine_state
        {
            get { return _sub_mine_state; }
            set { if (OnPropertyChanging(__.sub_mine_state, value)) { _sub_mine_state = value; OnPropertyChanged(__.sub_mine_state); } }
        }

        private Int32 _sub_craft_state;
        /// <summary>艺术状态</summary>
        [DisplayName("艺术状态")]
        [Description("艺术状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(76, "sub_craft_state", "艺术状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_craft_state
        {
            get { return _sub_craft_state; }
            set { if (OnPropertyChanging(__.sub_craft_state, value)) { _sub_craft_state = value; OnPropertyChanged(__.sub_craft_state); } }
        }

        private Int32 _sub_archer_state;
        /// <summary>弓术状态</summary>
        [DisplayName("弓术状态")]
        [Description("弓术状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(77, "sub_archer_state", "弓术状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_archer_state
        {
            get { return _sub_archer_state; }
            set { if (OnPropertyChanging(__.sub_archer_state, value)) { _sub_archer_state = value; OnPropertyChanged(__.sub_archer_state); } }
        }

        private Int32 _sub_etiquette_state;
        /// <summary>礼法状态</summary>
        [DisplayName("礼法状态")]
        [Description("礼法状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(78, "sub_etiquette_state", "礼法状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_etiquette_state
        {
            get { return _sub_etiquette_state; }
            set { if (OnPropertyChanging(__.sub_etiquette_state, value)) { _sub_etiquette_state = value; OnPropertyChanged(__.sub_etiquette_state); } }
        }

        private Int32 _sub_martial_state;
        /// <summary>武艺状态</summary>
        [DisplayName("武艺状态")]
        [Description("武艺状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(79, "sub_martial_state", "武艺状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_martial_state
        {
            get { return _sub_martial_state; }
            set { if (OnPropertyChanging(__.sub_martial_state, value)) { _sub_martial_state = value; OnPropertyChanged(__.sub_martial_state); } }
        }

        private Int32 _sub_tactical_state;
        /// <summary>军学状态</summary>
        [DisplayName("军学状态")]
        [Description("军学状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(80, "sub_tactical_state", "军学状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_tactical_state
        {
            get { return _sub_tactical_state; }
            set { if (OnPropertyChanging(__.sub_tactical_state, value)) { _sub_tactical_state = value; OnPropertyChanged(__.sub_tactical_state); } }
        }

        private Int32 _sub_medical_state;
        /// <summary>医术状态</summary>
        [DisplayName("医术状态")]
        [Description("医术状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(81, "sub_medical_state", "医术状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_medical_state
        {
            get { return _sub_medical_state; }
            set { if (OnPropertyChanging(__.sub_medical_state, value)) { _sub_medical_state = value; OnPropertyChanged(__.sub_medical_state); } }
        }

        private Int32 _sub_ninjitsu_state;
        /// <summary>忍术状态</summary>
        [DisplayName("忍术状态")]
        [Description("忍术状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(82, "sub_ninjitsu_state", "忍术状态", "0", "int", 10, 0, false)]
        public virtual Int32 sub_ninjitsu_state
        {
            get { return _sub_ninjitsu_state; }
            set { if (OnPropertyChanging(__.sub_ninjitsu_state, value)) { _sub_ninjitsu_state = value; OnPropertyChanged(__.sub_ninjitsu_state); } }
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
                    case __.rid : return _rid;
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
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
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
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得生活技能表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>武将生活技能编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>武将表编号</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary>茶道基表编号</summary>
            public static readonly Field sub_tea = FindByName(__.sub_tea);

            ///<summary>算数基表编号</summary>
            public static readonly Field sub_calculate = FindByName(__.sub_calculate);

            ///<summary>建筑基表编号</summary>
            public static readonly Field sub_build = FindByName(__.sub_build);

            ///<summary>辩才基表编号</summary>
            public static readonly Field sub_eloquence = FindByName(__.sub_eloquence);

            ///<summary>马术基表编号</summary>
            public static readonly Field sub_equestrian = FindByName(__.sub_equestrian);

            ///<summary>开垦基表编号</summary>
            public static readonly Field sub_reclaimed = FindByName(__.sub_reclaimed);

            ///<summary>足轻基表编号</summary>
            public static readonly Field sub_ashigaru = FindByName(__.sub_ashigaru);

            ///<summary>铁炮基表编号</summary>
            public static readonly Field sub_artillery = FindByName(__.sub_artillery);

            ///<summary>矿山基表编号</summary>
            public static readonly Field sub_mine = FindByName(__.sub_mine);

            ///<summary>艺术基表编号</summary>
            public static readonly Field sub_craft = FindByName(__.sub_craft);

            ///<summary>弓术基表编号</summary>
            public static readonly Field sub_archer = FindByName(__.sub_archer);

            ///<summary>礼法基表编号</summary>
            public static readonly Field sub_etiquette = FindByName(__.sub_etiquette);

            ///<summary>武艺基表编号</summary>
            public static readonly Field sub_martial = FindByName(__.sub_martial);

            ///<summary>军学基表编号</summary>
            public static readonly Field sub_tactical = FindByName(__.sub_tactical);

            ///<summary>医术基表编号</summary>
            public static readonly Field sub_medical = FindByName(__.sub_medical);

            ///<summary>忍术基表编号</summary>
            public static readonly Field sub_ninjitsu = FindByName(__.sub_ninjitsu);

            ///<summary>茶道当前熟练度</summary>
            public static readonly Field sub_tea_progress = FindByName(__.sub_tea_progress);

            ///<summary>算数当前熟练度</summary>
            public static readonly Field sub_calculate_progress = FindByName(__.sub_calculate_progress);

            ///<summary>建筑当前熟练度</summary>
            public static readonly Field sub_build_progress = FindByName(__.sub_build_progress);

            ///<summary>口才当前熟练度</summary>
            public static readonly Field sub_eloquence_progress = FindByName(__.sub_eloquence_progress);

            ///<summary>马术当前熟练度</summary>
            public static readonly Field sub_equestrian_progress = FindByName(__.sub_equestrian_progress);

            ///<summary>开垦当前熟练度</summary>
            public static readonly Field sub_reclaimed_progress = FindByName(__.sub_reclaimed_progress);

            ///<summary>足轻当前熟练度</summary>
            public static readonly Field sub_ashigaru_progress = FindByName(__.sub_ashigaru_progress);

            ///<summary>铁炮当前熟练度</summary>
            public static readonly Field sub_artillery_progress = FindByName(__.sub_artillery_progress);

            ///<summary>矿山当前熟练度</summary>
            public static readonly Field sub_mine_progress = FindByName(__.sub_mine_progress);

            ///<summary>艺术当前熟练度</summary>
            public static readonly Field sub_craft_progress = FindByName(__.sub_craft_progress);

            ///<summary>弓术当前熟练度</summary>
            public static readonly Field sub_archer_progress = FindByName(__.sub_archer_progress);

            ///<summary>礼法当前熟练度</summary>
            public static readonly Field sub_etiquette_progress = FindByName(__.sub_etiquette_progress);

            ///<summary>武艺当前熟练度</summary>
            public static readonly Field sub_martial_progress = FindByName(__.sub_martial_progress);

            ///<summary>军学当前熟练度</summary>
            public static readonly Field sub_tactical_progress = FindByName(__.sub_tactical_progress);

            ///<summary>医术当前熟练度</summary>
            public static readonly Field sub_medical_progress = FindByName(__.sub_medical_progress);

            ///<summary>忍术当前熟练度</summary>
            public static readonly Field sub_ninjitsu_progress = FindByName(__.sub_ninjitsu_progress);

            ///<summary>茶道升级到达时间</summary>
            public static readonly Field sub_tea_time = FindByName(__.sub_tea_time);

            ///<summary>算数升级到达时间</summary>
            public static readonly Field sub_calculate_time = FindByName(__.sub_calculate_time);

            ///<summary>建筑升级到达时间</summary>
            public static readonly Field sub_build_time = FindByName(__.sub_build_time);

            ///<summary>辩才升级到达时间</summary>
            public static readonly Field sub_eloquence_time = FindByName(__.sub_eloquence_time);

            ///<summary>马术升级到达时间</summary>
            public static readonly Field sub_equestrian_time = FindByName(__.sub_equestrian_time);

            ///<summary>开垦升级到达时间</summary>
            public static readonly Field sub_reclaimed_time = FindByName(__.sub_reclaimed_time);

            ///<summary>足轻升级到达时间</summary>
            public static readonly Field sub_ashigaru_time = FindByName(__.sub_ashigaru_time);

            ///<summary>铁炮升级到达时间</summary>
            public static readonly Field sub_artillery_time = FindByName(__.sub_artillery_time);

            ///<summary>矿山升级到达时间</summary>
            public static readonly Field sub_mine_time = FindByName(__.sub_mine_time);

            ///<summary>艺术升级到达时间</summary>
            public static readonly Field sub_craft_time = FindByName(__.sub_craft_time);

            ///<summary>弓术升级到达时间</summary>
            public static readonly Field sub_archer_time = FindByName(__.sub_archer_time);

            ///<summary>礼法升级到达时间</summary>
            public static readonly Field sub_etiquette_time = FindByName(__.sub_etiquette_time);

            ///<summary>武艺升级到达时间</summary>
            public static readonly Field sub_martial_time = FindByName(__.sub_martial_time);

            ///<summary>军学升级到达时间</summary>
            public static readonly Field sub_tactical_time = FindByName(__.sub_tactical_time);

            ///<summary>医术升级到达时间</summary>
            public static readonly Field sub_medical_time = FindByName(__.sub_medical_time);

            ///<summary>忍术升级到达时间</summary>
            public static readonly Field sub_ninjitsu_time = FindByName(__.sub_ninjitsu_time);

            ///<summary>茶道等级</summary>
            public static readonly Field sub_tea_level = FindByName(__.sub_tea_level);

            ///<summary>算数等级</summary>
            public static readonly Field sub_calculate_level = FindByName(__.sub_calculate_level);

            ///<summary>建筑等级</summary>
            public static readonly Field sub_build_level = FindByName(__.sub_build_level);

            ///<summary>辩才等级</summary>
            public static readonly Field sub_eloquence_level = FindByName(__.sub_eloquence_level);

            ///<summary>马术等级</summary>
            public static readonly Field sub_equestrian_level = FindByName(__.sub_equestrian_level);

            ///<summary>开垦等级</summary>
            public static readonly Field sub_reclaimed_level = FindByName(__.sub_reclaimed_level);

            ///<summary>足轻等级</summary>
            public static readonly Field sub_ashigaru_level = FindByName(__.sub_ashigaru_level);

            ///<summary>铁炮等级</summary>
            public static readonly Field sub_artillery_level = FindByName(__.sub_artillery_level);

            ///<summary>矿山等级</summary>
            public static readonly Field sub_mine_level = FindByName(__.sub_mine_level);

            ///<summary>艺术等级</summary>
            public static readonly Field sub_craft_level = FindByName(__.sub_craft_level);

            ///<summary>弓术等级</summary>
            public static readonly Field sub_archer_level = FindByName(__.sub_archer_level);

            ///<summary>礼法等级</summary>
            public static readonly Field sub_etiquette_level = FindByName(__.sub_etiquette_level);

            ///<summary>武艺等级</summary>
            public static readonly Field sub_martial_level = FindByName(__.sub_martial_level);

            ///<summary>军学等级</summary>
            public static readonly Field sub_tactical_level = FindByName(__.sub_tactical_level);

            ///<summary>医术等级</summary>
            public static readonly Field sub_medical_level = FindByName(__.sub_medical_level);

            ///<summary>忍术等级</summary>
            public static readonly Field sub_ninjitsu_level = FindByName(__.sub_ninjitsu_level);

            ///<summary>茶道状态</summary>
            public static readonly Field sub_tea_state = FindByName(__.sub_tea_state);

            ///<summary>算数状态</summary>
            public static readonly Field sub_calculate_state = FindByName(__.sub_calculate_state);

            ///<summary>建筑状态</summary>
            public static readonly Field sub_build_state = FindByName(__.sub_build_state);

            ///<summary>辩才状态</summary>
            public static readonly Field sub_eloquence_state = FindByName(__.sub_eloquence_state);

            ///<summary>马术状态</summary>
            public static readonly Field sub_equestrian_state = FindByName(__.sub_equestrian_state);

            ///<summary>开垦状态</summary>
            public static readonly Field sub_reclaimed_state = FindByName(__.sub_reclaimed_state);

            ///<summary>足轻状态</summary>
            public static readonly Field sub_ashigaru_state = FindByName(__.sub_ashigaru_state);

            ///<summary>铁炮状态</summary>
            public static readonly Field sub_artillery_state = FindByName(__.sub_artillery_state);

            ///<summary>矿山状态</summary>
            public static readonly Field sub_mine_state = FindByName(__.sub_mine_state);

            ///<summary>艺术状态</summary>
            public static readonly Field sub_craft_state = FindByName(__.sub_craft_state);

            ///<summary>弓术状态</summary>
            public static readonly Field sub_archer_state = FindByName(__.sub_archer_state);

            ///<summary>礼法状态</summary>
            public static readonly Field sub_etiquette_state = FindByName(__.sub_etiquette_state);

            ///<summary>武艺状态</summary>
            public static readonly Field sub_martial_state = FindByName(__.sub_martial_state);

            ///<summary>军学状态</summary>
            public static readonly Field sub_tactical_state = FindByName(__.sub_tactical_state);

            ///<summary>医术状态</summary>
            public static readonly Field sub_medical_state = FindByName(__.sub_medical_state);

            ///<summary>忍术状态</summary>
            public static readonly Field sub_ninjitsu_state = FindByName(__.sub_ninjitsu_state);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得生活技能表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>武将生活技能编号</summary>
            public const String id = "id";

            ///<summary>武将表编号</summary>
            public const String rid = "rid";

            ///<summary>茶道基表编号</summary>
            public const String sub_tea = "sub_tea";

            ///<summary>算数基表编号</summary>
            public const String sub_calculate = "sub_calculate";

            ///<summary>建筑基表编号</summary>
            public const String sub_build = "sub_build";

            ///<summary>辩才基表编号</summary>
            public const String sub_eloquence = "sub_eloquence";

            ///<summary>马术基表编号</summary>
            public const String sub_equestrian = "sub_equestrian";

            ///<summary>开垦基表编号</summary>
            public const String sub_reclaimed = "sub_reclaimed";

            ///<summary>足轻基表编号</summary>
            public const String sub_ashigaru = "sub_ashigaru";

            ///<summary>铁炮基表编号</summary>
            public const String sub_artillery = "sub_artillery";

            ///<summary>矿山基表编号</summary>
            public const String sub_mine = "sub_mine";

            ///<summary>艺术基表编号</summary>
            public const String sub_craft = "sub_craft";

            ///<summary>弓术基表编号</summary>
            public const String sub_archer = "sub_archer";

            ///<summary>礼法基表编号</summary>
            public const String sub_etiquette = "sub_etiquette";

            ///<summary>武艺基表编号</summary>
            public const String sub_martial = "sub_martial";

            ///<summary>军学基表编号</summary>
            public const String sub_tactical = "sub_tactical";

            ///<summary>医术基表编号</summary>
            public const String sub_medical = "sub_medical";

            ///<summary>忍术基表编号</summary>
            public const String sub_ninjitsu = "sub_ninjitsu";

            ///<summary>茶道当前熟练度</summary>
            public const String sub_tea_progress = "sub_tea_progress";

            ///<summary>算数当前熟练度</summary>
            public const String sub_calculate_progress = "sub_calculate_progress";

            ///<summary>建筑当前熟练度</summary>
            public const String sub_build_progress = "sub_build_progress";

            ///<summary>口才当前熟练度</summary>
            public const String sub_eloquence_progress = "sub_eloquence_progress";

            ///<summary>马术当前熟练度</summary>
            public const String sub_equestrian_progress = "sub_equestrian_progress";

            ///<summary>开垦当前熟练度</summary>
            public const String sub_reclaimed_progress = "sub_reclaimed_progress";

            ///<summary>足轻当前熟练度</summary>
            public const String sub_ashigaru_progress = "sub_ashigaru_progress";

            ///<summary>铁炮当前熟练度</summary>
            public const String sub_artillery_progress = "sub_artillery_progress";

            ///<summary>矿山当前熟练度</summary>
            public const String sub_mine_progress = "sub_mine_progress";

            ///<summary>艺术当前熟练度</summary>
            public const String sub_craft_progress = "sub_craft_progress";

            ///<summary>弓术当前熟练度</summary>
            public const String sub_archer_progress = "sub_archer_progress";

            ///<summary>礼法当前熟练度</summary>
            public const String sub_etiquette_progress = "sub_etiquette_progress";

            ///<summary>武艺当前熟练度</summary>
            public const String sub_martial_progress = "sub_martial_progress";

            ///<summary>军学当前熟练度</summary>
            public const String sub_tactical_progress = "sub_tactical_progress";

            ///<summary>医术当前熟练度</summary>
            public const String sub_medical_progress = "sub_medical_progress";

            ///<summary>忍术当前熟练度</summary>
            public const String sub_ninjitsu_progress = "sub_ninjitsu_progress";

            ///<summary>茶道升级到达时间</summary>
            public const String sub_tea_time = "sub_tea_time";

            ///<summary>算数升级到达时间</summary>
            public const String sub_calculate_time = "sub_calculate_time";

            ///<summary>建筑升级到达时间</summary>
            public const String sub_build_time = "sub_build_time";

            ///<summary>辩才升级到达时间</summary>
            public const String sub_eloquence_time = "sub_eloquence_time";

            ///<summary>马术升级到达时间</summary>
            public const String sub_equestrian_time = "sub_equestrian_time";

            ///<summary>开垦升级到达时间</summary>
            public const String sub_reclaimed_time = "sub_reclaimed_time";

            ///<summary>足轻升级到达时间</summary>
            public const String sub_ashigaru_time = "sub_ashigaru_time";

            ///<summary>铁炮升级到达时间</summary>
            public const String sub_artillery_time = "sub_artillery_time";

            ///<summary>矿山升级到达时间</summary>
            public const String sub_mine_time = "sub_mine_time";

            ///<summary>艺术升级到达时间</summary>
            public const String sub_craft_time = "sub_craft_time";

            ///<summary>弓术升级到达时间</summary>
            public const String sub_archer_time = "sub_archer_time";

            ///<summary>礼法升级到达时间</summary>
            public const String sub_etiquette_time = "sub_etiquette_time";

            ///<summary>武艺升级到达时间</summary>
            public const String sub_martial_time = "sub_martial_time";

            ///<summary>军学升级到达时间</summary>
            public const String sub_tactical_time = "sub_tactical_time";

            ///<summary>医术升级到达时间</summary>
            public const String sub_medical_time = "sub_medical_time";

            ///<summary>忍术升级到达时间</summary>
            public const String sub_ninjitsu_time = "sub_ninjitsu_time";

            ///<summary>茶道等级</summary>
            public const String sub_tea_level = "sub_tea_level";

            ///<summary>算数等级</summary>
            public const String sub_calculate_level = "sub_calculate_level";

            ///<summary>建筑等级</summary>
            public const String sub_build_level = "sub_build_level";

            ///<summary>辩才等级</summary>
            public const String sub_eloquence_level = "sub_eloquence_level";

            ///<summary>马术等级</summary>
            public const String sub_equestrian_level = "sub_equestrian_level";

            ///<summary>开垦等级</summary>
            public const String sub_reclaimed_level = "sub_reclaimed_level";

            ///<summary>足轻等级</summary>
            public const String sub_ashigaru_level = "sub_ashigaru_level";

            ///<summary>铁炮等级</summary>
            public const String sub_artillery_level = "sub_artillery_level";

            ///<summary>矿山等级</summary>
            public const String sub_mine_level = "sub_mine_level";

            ///<summary>艺术等级</summary>
            public const String sub_craft_level = "sub_craft_level";

            ///<summary>弓术等级</summary>
            public const String sub_archer_level = "sub_archer_level";

            ///<summary>礼法等级</summary>
            public const String sub_etiquette_level = "sub_etiquette_level";

            ///<summary>武艺等级</summary>
            public const String sub_martial_level = "sub_martial_level";

            ///<summary>军学等级</summary>
            public const String sub_tactical_level = "sub_tactical_level";

            ///<summary>医术等级</summary>
            public const String sub_medical_level = "sub_medical_level";

            ///<summary>忍术等级</summary>
            public const String sub_ninjitsu_level = "sub_ninjitsu_level";

            ///<summary>茶道状态</summary>
            public const String sub_tea_state = "sub_tea_state";

            ///<summary>算数状态</summary>
            public const String sub_calculate_state = "sub_calculate_state";

            ///<summary>建筑状态</summary>
            public const String sub_build_state = "sub_build_state";

            ///<summary>辩才状态</summary>
            public const String sub_eloquence_state = "sub_eloquence_state";

            ///<summary>马术状态</summary>
            public const String sub_equestrian_state = "sub_equestrian_state";

            ///<summary>开垦状态</summary>
            public const String sub_reclaimed_state = "sub_reclaimed_state";

            ///<summary>足轻状态</summary>
            public const String sub_ashigaru_state = "sub_ashigaru_state";

            ///<summary>铁炮状态</summary>
            public const String sub_artillery_state = "sub_artillery_state";

            ///<summary>矿山状态</summary>
            public const String sub_mine_state = "sub_mine_state";

            ///<summary>艺术状态</summary>
            public const String sub_craft_state = "sub_craft_state";

            ///<summary>弓术状态</summary>
            public const String sub_archer_state = "sub_archer_state";

            ///<summary>礼法状态</summary>
            public const String sub_etiquette_state = "sub_etiquette_state";

            ///<summary>武艺状态</summary>
            public const String sub_martial_state = "sub_martial_state";

            ///<summary>军学状态</summary>
            public const String sub_tactical_state = "sub_tactical_state";

            ///<summary>医术状态</summary>
            public const String sub_medical_state = "sub_medical_state";

            ///<summary>忍术状态</summary>
            public const String sub_ninjitsu_state = "sub_ninjitsu_state";

        }
        #endregion
    }

    /// <summary>生活技能表接口</summary>
    public partial interface Itg_role_life_skill
    {
        #region 属性
        /// <summary>武将生活技能编号</summary>
        Int64 id { get; set; }

        /// <summary>武将表编号</summary>
        Int64 rid { get; set; }

        /// <summary>茶道基表编号</summary>
        Int32 sub_tea { get; set; }

        /// <summary>算数基表编号</summary>
        Int32 sub_calculate { get; set; }

        /// <summary>建筑基表编号</summary>
        Int32 sub_build { get; set; }

        /// <summary>辩才基表编号</summary>
        Int32 sub_eloquence { get; set; }

        /// <summary>马术基表编号</summary>
        Int32 sub_equestrian { get; set; }

        /// <summary>开垦基表编号</summary>
        Int32 sub_reclaimed { get; set; }

        /// <summary>足轻基表编号</summary>
        Int32 sub_ashigaru { get; set; }

        /// <summary>铁炮基表编号</summary>
        Int32 sub_artillery { get; set; }

        /// <summary>矿山基表编号</summary>
        Int32 sub_mine { get; set; }

        /// <summary>艺术基表编号</summary>
        Int32 sub_craft { get; set; }

        /// <summary>弓术基表编号</summary>
        Int32 sub_archer { get; set; }

        /// <summary>礼法基表编号</summary>
        Int32 sub_etiquette { get; set; }

        /// <summary>武艺基表编号</summary>
        Int32 sub_martial { get; set; }

        /// <summary>军学基表编号</summary>
        Int32 sub_tactical { get; set; }

        /// <summary>医术基表编号</summary>
        Int32 sub_medical { get; set; }

        /// <summary>忍术基表编号</summary>
        Int32 sub_ninjitsu { get; set; }

        /// <summary>茶道当前熟练度</summary>
        Int32 sub_tea_progress { get; set; }

        /// <summary>算数当前熟练度</summary>
        Int32 sub_calculate_progress { get; set; }

        /// <summary>建筑当前熟练度</summary>
        Int32 sub_build_progress { get; set; }

        /// <summary>口才当前熟练度</summary>
        Int32 sub_eloquence_progress { get; set; }

        /// <summary>马术当前熟练度</summary>
        Int32 sub_equestrian_progress { get; set; }

        /// <summary>开垦当前熟练度</summary>
        Int32 sub_reclaimed_progress { get; set; }

        /// <summary>足轻当前熟练度</summary>
        Int32 sub_ashigaru_progress { get; set; }

        /// <summary>铁炮当前熟练度</summary>
        Int32 sub_artillery_progress { get; set; }

        /// <summary>矿山当前熟练度</summary>
        Int32 sub_mine_progress { get; set; }

        /// <summary>艺术当前熟练度</summary>
        Int32 sub_craft_progress { get; set; }

        /// <summary>弓术当前熟练度</summary>
        Int32 sub_archer_progress { get; set; }

        /// <summary>礼法当前熟练度</summary>
        Int32 sub_etiquette_progress { get; set; }

        /// <summary>武艺当前熟练度</summary>
        Int32 sub_martial_progress { get; set; }

        /// <summary>军学当前熟练度</summary>
        Int32 sub_tactical_progress { get; set; }

        /// <summary>医术当前熟练度</summary>
        Int32 sub_medical_progress { get; set; }

        /// <summary>忍术当前熟练度</summary>
        Int32 sub_ninjitsu_progress { get; set; }

        /// <summary>茶道升级到达时间</summary>
        Int64 sub_tea_time { get; set; }

        /// <summary>算数升级到达时间</summary>
        Int64 sub_calculate_time { get; set; }

        /// <summary>建筑升级到达时间</summary>
        Int64 sub_build_time { get; set; }

        /// <summary>辩才升级到达时间</summary>
        Int64 sub_eloquence_time { get; set; }

        /// <summary>马术升级到达时间</summary>
        Int64 sub_equestrian_time { get; set; }

        /// <summary>开垦升级到达时间</summary>
        Int64 sub_reclaimed_time { get; set; }

        /// <summary>足轻升级到达时间</summary>
        Int64 sub_ashigaru_time { get; set; }

        /// <summary>铁炮升级到达时间</summary>
        Int64 sub_artillery_time { get; set; }

        /// <summary>矿山升级到达时间</summary>
        Int64 sub_mine_time { get; set; }

        /// <summary>艺术升级到达时间</summary>
        Int64 sub_craft_time { get; set; }

        /// <summary>弓术升级到达时间</summary>
        Int64 sub_archer_time { get; set; }

        /// <summary>礼法升级到达时间</summary>
        Int64 sub_etiquette_time { get; set; }

        /// <summary>武艺升级到达时间</summary>
        Int64 sub_martial_time { get; set; }

        /// <summary>军学升级到达时间</summary>
        Int64 sub_tactical_time { get; set; }

        /// <summary>医术升级到达时间</summary>
        Int64 sub_medical_time { get; set; }

        /// <summary>忍术升级到达时间</summary>
        Int64 sub_ninjitsu_time { get; set; }

        /// <summary>茶道等级</summary>
        Int32 sub_tea_level { get; set; }

        /// <summary>算数等级</summary>
        Int32 sub_calculate_level { get; set; }

        /// <summary>建筑等级</summary>
        Int32 sub_build_level { get; set; }

        /// <summary>辩才等级</summary>
        Int32 sub_eloquence_level { get; set; }

        /// <summary>马术等级</summary>
        Int32 sub_equestrian_level { get; set; }

        /// <summary>开垦等级</summary>
        Int32 sub_reclaimed_level { get; set; }

        /// <summary>足轻等级</summary>
        Int32 sub_ashigaru_level { get; set; }

        /// <summary>铁炮等级</summary>
        Int32 sub_artillery_level { get; set; }

        /// <summary>矿山等级</summary>
        Int32 sub_mine_level { get; set; }

        /// <summary>艺术等级</summary>
        Int32 sub_craft_level { get; set; }

        /// <summary>弓术等级</summary>
        Int32 sub_archer_level { get; set; }

        /// <summary>礼法等级</summary>
        Int32 sub_etiquette_level { get; set; }

        /// <summary>武艺等级</summary>
        Int32 sub_martial_level { get; set; }

        /// <summary>军学等级</summary>
        Int32 sub_tactical_level { get; set; }

        /// <summary>医术等级</summary>
        Int32 sub_medical_level { get; set; }

        /// <summary>忍术等级</summary>
        Int32 sub_ninjitsu_level { get; set; }

        /// <summary>茶道状态</summary>
        Int32 sub_tea_state { get; set; }

        /// <summary>算数状态</summary>
        Int32 sub_calculate_state { get; set; }

        /// <summary>建筑状态</summary>
        Int32 sub_build_state { get; set; }

        /// <summary>辩才状态</summary>
        Int32 sub_eloquence_state { get; set; }

        /// <summary>马术状态</summary>
        Int32 sub_equestrian_state { get; set; }

        /// <summary>开垦状态</summary>
        Int32 sub_reclaimed_state { get; set; }

        /// <summary>足轻状态</summary>
        Int32 sub_ashigaru_state { get; set; }

        /// <summary>铁炮状态</summary>
        Int32 sub_artillery_state { get; set; }

        /// <summary>矿山状态</summary>
        Int32 sub_mine_state { get; set; }

        /// <summary>艺术状态</summary>
        Int32 sub_craft_state { get; set; }

        /// <summary>弓术状态</summary>
        Int32 sub_archer_state { get; set; }

        /// <summary>礼法状态</summary>
        Int32 sub_etiquette_state { get; set; }

        /// <summary>武艺状态</summary>
        Int32 sub_martial_state { get; set; }

        /// <summary>军学状态</summary>
        Int32 sub_tactical_state { get; set; }

        /// <summary>医术状态</summary>
        Int32 sub_medical_state { get; set; }

        /// <summary>忍术状态</summary>
        Int32 sub_ninjitsu_state { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}