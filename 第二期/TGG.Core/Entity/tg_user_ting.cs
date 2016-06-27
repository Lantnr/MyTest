using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家跑商町表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家跑商町表")]
    [BindIndex("PK_tg_user_city", true, "id")]
    [BindTable("tg_user_ting", Description = "玩家跑商町表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_ting : Itg_user_ting
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

        private Int32 _ting_id;
        /// <summary>町基表编号</summary>
        [DisplayName("町基表编号")]
        [Description("町基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "ting_id", "町基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 ting_id
        {
            get { return _ting_id; }
            set { if (OnPropertyChanging(__.ting_id, value)) { _ting_id = value; OnPropertyChanged(__.ting_id); } }
        }

        private Int32 _state;
        /// <summary>城市访问状态 0:未访问 1:已访问 系统刷新后默认为0</summary>
        [DisplayName("城市访问状态0:未访问1:已访问系统刷新后默认为0")]
        [Description("城市访问状态 0:未访问 1:已访问 系统刷新后默认为0")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "state", "城市访问状态 0:未访问 1:已访问 系统刷新后默认为0", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _area_id;
        /// <summary>商圈ID</summary>
        [DisplayName("商圈ID")]
        [Description("商圈ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "area_id", "商圈ID", "0", "int", 10, 0, false)]
        public virtual Int32 area_id
        {
            get { return _area_id; }
            set { if (OnPropertyChanging(__.area_id, value)) { _area_id = value; OnPropertyChanged(__.area_id); } }
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
                    case __.ting_id : return _ting_id;
                    case __.state : return _state;
                    case __.area_id : return _area_id;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.ting_id : _ting_id = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.area_id : _area_id = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家跑商町表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>町基表编号</summary>
            public static readonly Field ting_id = FindByName(__.ting_id);

            ///<summary>城市访问状态 0:未访问 1:已访问 系统刷新后默认为0</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>商圈ID</summary>
            public static readonly Field area_id = FindByName(__.area_id);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家跑商町表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>町基表编号</summary>
            public const String ting_id = "ting_id";

            ///<summary>城市访问状态 0:未访问 1:已访问 系统刷新后默认为0</summary>
            public const String state = "state";

            ///<summary>商圈ID</summary>
            public const String area_id = "area_id";

        }
        #endregion
    }

    /// <summary>玩家跑商町表接口</summary>
    public partial interface Itg_user_ting
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>町基表编号</summary>
        Int32 ting_id { get; set; }

        /// <summary>城市访问状态 0:未访问 1:已访问 系统刷新后默认为0</summary>
        Int32 state { get; set; }

        /// <summary>商圈ID</summary>
        Int32 area_id { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}