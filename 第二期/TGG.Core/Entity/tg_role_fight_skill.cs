using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>武将个人战技能表</summary>
    [Serializable]
    [DataObject]
    [Description("武将个人战技能表")]
    [BindIndex("PK_tg_role_fight_skill", true, "id")]
    [BindTable("tg_role_fight_skill", Description = "武将个人战技能表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_role_fight_skill : Itg_role_fight_skill
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

        private Int32 _skill_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "skill_id", "", "0", "int", 10, 0, false)]
        public virtual Int32 skill_id
        {
            get { return _skill_id; }
            set { if (OnPropertyChanging(__.skill_id, value)) { _skill_id = value; OnPropertyChanged(__.skill_id); } }
        }

        private Int32 _skill_type;
        /// <summary>技能类型</summary>
        [DisplayName("技能类型")]
        [Description("技能类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "skill_type", "技能类型", "0", "int", 10, 0, false)]
        public virtual Int32 skill_type
        {
            get { return _skill_type; }
            set { if (OnPropertyChanging(__.skill_type, value)) { _skill_type = value; OnPropertyChanged(__.skill_type); } }
        }

        private Int32 _skill_genre;
        /// <summary>技能流派</summary>
        [DisplayName("技能流派")]
        [Description("技能流派")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "skill_genre", "技能流派", "0", "int", 10, 0, false)]
        public virtual Int32 skill_genre
        {
            get { return _skill_genre; }
            set { if (OnPropertyChanging(__.skill_genre, value)) { _skill_genre = value; OnPropertyChanged(__.skill_genre); } }
        }

        private Int32 _skill_level;
        /// <summary>技能等级</summary>
        [DisplayName("技能等级")]
        [Description("技能等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "skill_level", "技能等级", "0", "int", 10, 0, false)]
        public virtual Int32 skill_level
        {
            get { return _skill_level; }
            set { if (OnPropertyChanging(__.skill_level, value)) { _skill_level = value; OnPropertyChanged(__.skill_level); } }
        }

        private Int64 _skill_time;
        /// <summary>技能学习到达时间</summary>
        [DisplayName("技能学习到达时间")]
        [Description("技能学习到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "skill_time", "技能学习到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 skill_time
        {
            get { return _skill_time; }
            set { if (OnPropertyChanging(__.skill_time, value)) { _skill_time = value; OnPropertyChanged(__.skill_time); } }
        }

        private Int32 _skill_state;
        /// <summary>战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）</summary>
        [DisplayName("战斗技能状态0:未学；1:已学已升级；2:可学；3:正在学正在升级")]
        [Description("战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "skill_state", "战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）", "0", "int", 10, 0, false)]
        public virtual Int32 skill_state
        {
            get { return _skill_state; }
            set { if (OnPropertyChanging(__.skill_state, value)) { _skill_state = value; OnPropertyChanged(__.skill_state); } }
        }

        private Int32 _type_sub;
        /// <summary>战斗类型：1.个人战  2.合战</summary>
        [DisplayName("战斗类型：1")]
        [Description("战斗类型：1.个人战  2.合战")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "type_sub", "战斗类型：1.个人战  2.合战", "0", "int", 10, 0, false)]
        public virtual Int32 type_sub
        {
            get { return _type_sub; }
            set { if (OnPropertyChanging(__.type_sub, value)) { _type_sub = value; OnPropertyChanged(__.type_sub); } }
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
                    case __.rid: return _rid;
                    case __.skill_id: return _skill_id;
                    case __.skill_type: return _skill_type;
                    case __.skill_genre: return _skill_genre;
                    case __.skill_level: return _skill_level;
                    case __.skill_time: return _skill_time;
                    case __.skill_state: return _skill_state;
                    case __.type_sub: return _type_sub;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.rid: _rid = Convert.ToInt64(value); break;
                    case __.skill_id: _skill_id = Convert.ToInt32(value); break;
                    case __.skill_type: _skill_type = Convert.ToInt32(value); break;
                    case __.skill_genre: _skill_genre = Convert.ToInt32(value); break;
                    case __.skill_level: _skill_level = Convert.ToInt32(value); break;
                    case __.skill_time: _skill_time = Convert.ToInt64(value); break;
                    case __.skill_state: _skill_state = Convert.ToInt32(value); break;
                    case __.type_sub: _type_sub = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得武将个人战技能表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>武将表编号</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary></summary>
            public static readonly Field skill_id = FindByName(__.skill_id);

            ///<summary>技能类型</summary>
            public static readonly Field skill_type = FindByName(__.skill_type);

            ///<summary>技能流派</summary>
            public static readonly Field skill_genre = FindByName(__.skill_genre);

            ///<summary>技能等级</summary>
            public static readonly Field skill_level = FindByName(__.skill_level);

            ///<summary>技能学习到达时间</summary>
            public static readonly Field skill_time = FindByName(__.skill_time);

            ///<summary>战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）</summary>
            public static readonly Field skill_state = FindByName(__.skill_state);

            ///<summary>战斗类型：1.个人战  2.合战</summary>
            public static readonly Field type_sub = FindByName(__.type_sub);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得武将个人战技能表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>武将表编号</summary>
            public const String rid = "rid";

            ///<summary></summary>
            public const String skill_id = "skill_id";

            ///<summary>技能类型</summary>
            public const String skill_type = "skill_type";

            ///<summary>技能流派</summary>
            public const String skill_genre = "skill_genre";

            ///<summary>技能等级</summary>
            public const String skill_level = "skill_level";

            ///<summary>技能学习到达时间</summary>
            public const String skill_time = "skill_time";

            ///<summary>战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）</summary>
            public const String skill_state = "skill_state";

            ///<summary>战斗类型：1.个人战  2.合战</summary>
            public const String type_sub = "type_sub";

        }
        #endregion
    }

    /// <summary>武将个人战技能表接口</summary>
    public partial interface Itg_role_fight_skill
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>武将表编号</summary>
        Int64 rid { get; set; }

        /// <summary></summary>
        Int32 skill_id { get; set; }

        /// <summary>技能类型</summary>
        Int32 skill_type { get; set; }

        /// <summary>技能流派</summary>
        Int32 skill_genre { get; set; }

        /// <summary>技能等级</summary>
        Int32 skill_level { get; set; }

        /// <summary>技能学习到达时间</summary>
        Int64 skill_time { get; set; }

        /// <summary>战斗技能状态 0:未学；1:已学（已升级）；2:可学；3:正在学（正在升级）</summary>
        Int32 skill_state { get; set; }

        /// <summary>战斗类型：1.个人战  2.合战</summary>
        Int32 type_sub { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}