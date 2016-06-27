using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>战斗印表</summary>
    [Serializable]
    [DataObject]
    [Description("战斗印表")]
    [BindIndex("PK_tg_fight_yin", true, "id")]
    [BindTable("tg_fight_yin", Description = "战斗印表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_fight_yin : Itg_fight_yin
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

        private Int32 _yin_id;
        /// <summary>印基表编号</summary>
        [DisplayName("印基表编号")]
        [Description("印基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "yin_id", "印基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 yin_id
        {
            get { return _yin_id; }
            set { if (OnPropertyChanging(__.yin_id, value)) { _yin_id = value; OnPropertyChanged(__.yin_id); } }
        }

        private Int32 _yin_level;
        /// <summary>印等级</summary>
        [DisplayName("印等级")]
        [Description("印等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "yin_level", "印等级", "0", "int", 10, 0, false)]
        public virtual Int32 yin_level
        {
            get { return _yin_level; }
            set { if (OnPropertyChanging(__.yin_level, value)) { _yin_level = value; OnPropertyChanged(__.yin_level); } }
        }

        private Int32 _state;
        /// <summary>印使用状态</summary>
        [DisplayName("印使用状态")]
        [Description("印使用状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "state", "印使用状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
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
                    case __.yin_id : return _yin_id;
                    case __.yin_level : return _yin_level;
                    case __.state : return _state;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.yin_id : _yin_id = Convert.ToInt32(value); break;
                    case __.yin_level : _yin_level = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得战斗印表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>印基表编号</summary>
            public static readonly Field yin_id = FindByName(__.yin_id);

            ///<summary>印等级</summary>
            public static readonly Field yin_level = FindByName(__.yin_level);

            ///<summary>印使用状态</summary>
            public static readonly Field state = FindByName(__.state);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得战斗印表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>印基表编号</summary>
            public const String yin_id = "yin_id";

            ///<summary>印等级</summary>
            public const String yin_level = "yin_level";

            ///<summary>印使用状态</summary>
            public const String state = "state";

        }
        #endregion
    }

    /// <summary>战斗印表接口</summary>
    public partial interface Itg_fight_yin
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>印基表编号</summary>
        Int32 yin_id { get; set; }

        /// <summary>印等级</summary>
        Int32 yin_level { get; set; }

        /// <summary>印使用状态</summary>
        Int32 state { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}