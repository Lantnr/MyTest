using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>家臣称号表</summary>
    [Serializable]
    [DataObject]
    [Description("家臣称号表")]
    [BindIndex("PK__tg_role___3213E83F61D2EC77", true, "id")]
    [BindTable("tg_role_title", Description = "家臣称号表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_role_title : Itg_role_title
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键id", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>用户编号id</summary>
        [DisplayName("用户编号id")]
        [Description("用户编号id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "用户编号id", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _title_id;
        /// <summary>基表编号id</summary>
        [DisplayName("基表编号id")]
        [Description("基表编号id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "title_id", "基表编号id", "0", "int", 10, 0, false)]
        public virtual Int32 title_id
        {
            get { return _title_id; }
            set { if (OnPropertyChanging(__.title_id, value)) { _title_id = value; OnPropertyChanged(__.title_id); } }
        }

        private Int32 _title_state;
        /// <summary>称号达成状态 0：未达成  1：已达成</summary>
        [DisplayName("称号达成状态0：未达成1：已达成")]
        [Description("称号达成状态 0：未达成  1：已达成")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "title_state", "称号达成状态 0：未达成  1：已达成", "0", "int", 10, 0, false)]
        public virtual Int32 title_state
        {
            get { return _title_state; }
            set { if (OnPropertyChanging(__.title_state, value)) { _title_state = value; OnPropertyChanged(__.title_state); } }
        }

        private Int32 _title_count;
        /// <summary>称号格子数  默认开通一个</summary>
        [DisplayName("称号格子数默认开通一个")]
        [Description("称号格子数  默认开通一个")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "title_count", "称号格子数  默认开通一个", "0", "int", 10, 0, false)]
        public virtual Int32 title_count
        {
            get { return _title_count; }
            set { if (OnPropertyChanging(__.title_count, value)) { _title_count = value; OnPropertyChanged(__.title_count); } }
        }

        private Int32 _title_load_state;
        /// <summary>称号装备状态 0：未装备；1：已装备</summary>
        [DisplayName("称号装备状态0：未装备；1：已装备")]
        [Description("称号装备状态 0：未装备；1：已装备")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "title_load_state", "称号装备状态 0：未装备；1：已装备", "0", "int", 10, 0, false)]
        public virtual Int32 title_load_state
        {
            get { return _title_load_state; }
            set { if (OnPropertyChanging(__.title_load_state, value)) { _title_load_state = value; OnPropertyChanged(__.title_load_state); } }
        }

        private Int64 _packet_role1;
        /// <summary>格子1武将表编号id</summary>
        [DisplayName("格子1武将表编号id")]
        [Description("格子1武将表编号id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "packet_role1", "格子1武将表编号id", "0", "bigint", 19, 0, false)]
        public virtual Int64 packet_role1
        {
            get { return _packet_role1; }
            set { if (OnPropertyChanging(__.packet_role1, value)) { _packet_role1 = value; OnPropertyChanged(__.packet_role1); } }
        }

        private Int64 _packet_role2;
        /// <summary>格子2武将表编号id</summary>
        [DisplayName("格子2武将表编号id")]
        [Description("格子2武将表编号id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "packet_role2", "格子2武将表编号id", "0", "bigint", 19, 0, false)]
        public virtual Int64 packet_role2
        {
            get { return _packet_role2; }
            set { if (OnPropertyChanging(__.packet_role2, value)) { _packet_role2 = value; OnPropertyChanged(__.packet_role2); } }
        }

        private Int64 _packet_role3;
        /// <summary>格子3武将表编号id</summary>
        [DisplayName("格子3武将表编号id")]
        [Description("格子3武将表编号id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "packet_role3", "格子3武将表编号id", "0", "bigint", 19, 0, false)]
        public virtual Int64 packet_role3
        {
            get { return _packet_role3; }
            set { if (OnPropertyChanging(__.packet_role3, value)) { _packet_role3 = value; OnPropertyChanged(__.packet_role3); } }
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
                    case __.title_id: return _title_id;
                    case __.title_state: return _title_state;
                    case __.title_count: return _title_count;
                    case __.title_load_state: return _title_load_state;
                    case __.packet_role1: return _packet_role1;
                    case __.packet_role2: return _packet_role2;
                    case __.packet_role3: return _packet_role3;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.user_id: _user_id = Convert.ToInt64(value); break;
                    case __.title_id: _title_id = Convert.ToInt32(value); break;
                    case __.title_state: _title_state = Convert.ToInt32(value); break;
                    case __.title_count: _title_count = Convert.ToInt32(value); break;
                    case __.title_load_state: _title_load_state = Convert.ToInt32(value); break;
                    case __.packet_role1: _packet_role1 = Convert.ToInt64(value); break;
                    case __.packet_role2: _packet_role2 = Convert.ToInt64(value); break;
                    case __.packet_role3: _packet_role3 = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得家臣称号表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户编号id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>基表编号id</summary>
            public static readonly Field title_id = FindByName(__.title_id);

            ///<summary>称号达成状态 0：未达成  1：已达成</summary>
            public static readonly Field title_state = FindByName(__.title_state);

            ///<summary>称号格子数  默认开通一个</summary>
            public static readonly Field title_count = FindByName(__.title_count);

            ///<summary>称号装备状态 0：未装备；1：已装备</summary>
            public static readonly Field title_load_state = FindByName(__.title_load_state);

            ///<summary>格子1武将表编号id</summary>
            public static readonly Field packet_role1 = FindByName(__.packet_role1);

            ///<summary>格子2武将表编号id</summary>
            public static readonly Field packet_role2 = FindByName(__.packet_role2);

            ///<summary>格子3武将表编号id</summary>
            public static readonly Field packet_role3 = FindByName(__.packet_role3);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得家臣称号表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>用户编号id</summary>
            public const String user_id = "user_id";

            ///<summary>基表编号id</summary>
            public const String title_id = "title_id";

            ///<summary>称号达成状态 0：未达成  1：已达成</summary>
            public const String title_state = "title_state";

            ///<summary>称号格子数  默认开通一个</summary>
            public const String title_count = "title_count";

            ///<summary>称号装备状态 0：未装备；1：已装备</summary>
            public const String title_load_state = "title_load_state";

            ///<summary>格子1武将表编号id</summary>
            public const String packet_role1 = "packet_role1";

            ///<summary>格子2武将表编号id</summary>
            public const String packet_role2 = "packet_role2";

            ///<summary>格子3武将表编号id</summary>
            public const String packet_role3 = "packet_role3";

        }
        #endregion
    }

    /// <summary>家臣称号表接口</summary>
    public partial interface Itg_role_title
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>用户编号id</summary>
        Int64 user_id { get; set; }

        /// <summary>基表编号id</summary>
        Int32 title_id { get; set; }

        /// <summary>称号达成状态 0：未达成  1：已达成</summary>
        Int32 title_state { get; set; }

        /// <summary>称号格子数  默认开通一个</summary>
        Int32 title_count { get; set; }

        /// <summary>称号装备状态 0：未装备；1：已装备</summary>
        Int32 title_load_state { get; set; }

        /// <summary>格子1武将表编号id</summary>
        Int64 packet_role1 { get; set; }

        /// <summary>格子2武将表编号id</summary>
        Int64 packet_role2 { get; set; }

        /// <summary>格子3武将表编号id</summary>
        Int64 packet_role3 { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}