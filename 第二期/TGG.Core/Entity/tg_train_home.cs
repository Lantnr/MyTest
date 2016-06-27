using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>武将宅表</summary>
    [Serializable]
    [DataObject]
    [Description("武将宅表")]
    [BindIndex("PK__tg_train__3213E83F5F39A44A", true, "id")]
    [BindTable("tg_train_home", Description = "武将宅表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_train_home : Itg_train_home
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键编号</summary>
        [DisplayName("主键编号")]
        [Description("主键编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键编号", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _userid;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "userid", "用户id", "0", "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int32 _npc_id;
        /// <summary>npc 基表编号</summary>
        [DisplayName("npc基表编号")]
        [Description("npc 基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "npc_id", "npc 基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 npc_id
        {
            get { return _npc_id; }
            set { if (OnPropertyChanging(__.npc_id, value)) { _npc_id = value; OnPropertyChanged(__.npc_id); } }
        }

        private Int32 _npc_type;
        /// <summary>npc 难度等级</summary>
        [DisplayName("npc难度等级")]
        [Description("npc 难度等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "npc_type", "npc 难度等级", "0", "int", 10, 0, false)]
        public virtual Int32 npc_type
        {
            get { return _npc_type; }
            set { if (OnPropertyChanging(__.npc_type, value)) { _npc_type = value; OnPropertyChanged(__.npc_type); } }
        }

        private Int32 _npc_state;
        /// <summary>战斗状态 0：未战胜；1：已战胜</summary>
        [DisplayName("战斗状态0：未战胜；1：已战胜")]
        [Description("战斗状态 0：未战胜；1：已战胜")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "npc_state", "战斗状态 0：未战胜；1：已战胜", "0", "int", 10, 0, false)]
        public virtual Int32 npc_state
        {
            get { return _npc_state; }
            set { if (OnPropertyChanging(__.npc_state, value)) { _npc_state = value; OnPropertyChanged(__.npc_state); } }
        }

        private Int32 _is_steal;
        /// <summary>偷窃状态   0：未偷窃；1：已偷窃</summary>
        [DisplayName("偷窃状态0：未偷窃；1：已偷窃")]
        [Description("偷窃状态   0：未偷窃；1：已偷窃")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "is_steal", "偷窃状态   0：未偷窃；1：已偷窃", "0", "int", 10, 0, false)]
        public virtual Int32 is_steal
        {
            get { return _is_steal; }
            set { if (OnPropertyChanging(__.is_steal, value)) { _is_steal = value; OnPropertyChanged(__.is_steal); } }
        }

        private Int32 _city_id;
        /// <summary>npc 所属居城</summary>
        [DisplayName("npc所属居城")]
        [Description("npc 所属居城")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "city_id", "npc 所属居城", "0", "int", 10, 0, false)]
        public virtual Int32 city_id
        {
            get { return _city_id; }
            set { if (OnPropertyChanging(__.city_id, value)) { _city_id = value; OnPropertyChanged(__.city_id); } }
        }

        private Int32 _npc_spirit;
        /// <summary>npc 剩余魂数</summary>
        [DisplayName("npc剩余魂数")]
        [Description("npc 剩余魂数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "npc_spirit", "npc 剩余魂数", "0", "int", 10, 0, false)]
        public virtual Int32 npc_spirit
        {
            get { return _npc_spirit; }
            set { if (OnPropertyChanging(__.npc_spirit, value)) { _npc_spirit = value; OnPropertyChanged(__.npc_spirit); } }
        }

        private Int32 _total_spirit;
        /// <summary>npc 总魂数</summary>
        [DisplayName("npc总魂数")]
        [Description("npc 总魂数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "total_spirit", "npc 总魂数", "0", "int", 10, 0, false)]
        public virtual Int32 total_spirit
        {
            get { return _total_spirit; }
            set { if (OnPropertyChanging(__.total_spirit, value)) { _total_spirit = value; OnPropertyChanged(__.total_spirit); } }
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
                    case __.userid: return _userid;
                    case __.npc_id: return _npc_id;
                    case __.npc_type: return _npc_type;
                    case __.npc_state: return _npc_state;
                    case __.is_steal: return _is_steal;
                    case __.city_id: return _city_id;
                    case __.npc_spirit: return _npc_spirit;
                    case __.total_spirit: return _total_spirit;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.userid: _userid = Convert.ToInt64(value); break;
                    case __.npc_id: _npc_id = Convert.ToInt32(value); break;
                    case __.npc_type: _npc_type = Convert.ToInt32(value); break;
                    case __.npc_state: _npc_state = Convert.ToInt32(value); break;
                    case __.is_steal: _is_steal = Convert.ToInt32(value); break;
                    case __.city_id: _city_id = Convert.ToInt32(value); break;
                    case __.npc_spirit: _npc_spirit = Convert.ToInt32(value); break;
                    case __.total_spirit: _total_spirit = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得武将宅表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户id</summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary>npc 基表编号</summary>
            public static readonly Field npc_id = FindByName(__.npc_id);

            ///<summary>npc 难度等级</summary>
            public static readonly Field npc_type = FindByName(__.npc_type);

            ///<summary>战斗状态 0：未战胜；1：已战胜</summary>
            public static readonly Field npc_state = FindByName(__.npc_state);

            ///<summary>偷窃状态   0：未偷窃；1：已偷窃</summary>
            public static readonly Field is_steal = FindByName(__.is_steal);

            ///<summary>npc 所属居城</summary>
            public static readonly Field city_id = FindByName(__.city_id);

            ///<summary>npc 剩余魂数</summary>
            public static readonly Field npc_spirit = FindByName(__.npc_spirit);

            ///<summary>npc 总魂数</summary>
            public static readonly Field total_spirit = FindByName(__.total_spirit);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得武将宅表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键编号</summary>
            public const String id = "id";

            ///<summary>用户id</summary>
            public const String userid = "userid";

            ///<summary>npc 基表编号</summary>
            public const String npc_id = "npc_id";

            ///<summary>npc 难度等级</summary>
            public const String npc_type = "npc_type";

            ///<summary>战斗状态 0：未战胜；1：已战胜</summary>
            public const String npc_state = "npc_state";

            ///<summary>偷窃状态   0：未偷窃；1：已偷窃</summary>
            public const String is_steal = "is_steal";

            ///<summary>npc 所属居城</summary>
            public const String city_id = "city_id";

            ///<summary>npc 剩余魂数</summary>
            public const String npc_spirit = "npc_spirit";

            ///<summary>npc 总魂数</summary>
            public const String total_spirit = "total_spirit";

        }
        #endregion
    }

    /// <summary>武将宅表接口</summary>
    public partial interface Itg_train_home
    {
        #region 属性
        /// <summary>主键编号</summary>
        Int64 id { get; set; }

        /// <summary>用户id</summary>
        Int64 userid { get; set; }

        /// <summary>npc 基表编号</summary>
        Int32 npc_id { get; set; }

        /// <summary>npc 难度等级</summary>
        Int32 npc_type { get; set; }

        /// <summary>战斗状态 0：未战胜；1：已战胜</summary>
        Int32 npc_state { get; set; }

        /// <summary>偷窃状态   0：未偷窃；1：已偷窃</summary>
        Int32 is_steal { get; set; }

        /// <summary>npc 所属居城</summary>
        Int32 city_id { get; set; }

        /// <summary>npc 剩余魂数</summary>
        Int32 npc_spirit { get; set; }

        /// <summary>npc 总魂数</summary>
        Int32 total_spirit { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}