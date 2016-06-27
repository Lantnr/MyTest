using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>好友表</summary>
    [Serializable]
    [DataObject]
    [Description("好友表")]
    [BindIndex("PK_tg_friends", true, "id")]
    [BindTable("tg_friends", Description = "好友表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_friends : Itg_friends
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

        private Int64 _friend_id;
        /// <summary>好友id</summary>
        [DisplayName("好友id")]
        [Description("好友id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "friend_id", "好友id", "0", "bigint", 19, 0, false)]
        public virtual Int64 friend_id
        {
            get { return _friend_id; }
            set { if (OnPropertyChanging(__.friend_id, value)) { _friend_id = value; OnPropertyChanged(__.friend_id); } }
        }

        private Int64 _user_id;
        /// <summary>玩家id</summary>
        [DisplayName("玩家id")]
        [Description("玩家id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "user_id", "玩家id", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _friend_state;
        /// <summary>好友状态 0:好友 1:黑名单</summary>
        [DisplayName("好友状态0:好友1:黑名单")]
        [Description("好友状态 0:好友 1:黑名单")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "friend_state", "好友状态 0:好友 1:黑名单", "0", "int", 10, 0, false)]
        public virtual Int32 friend_state
        {
            get { return _friend_state; }
            set { if (OnPropertyChanging(__.friend_state, value)) { _friend_state = value; OnPropertyChanged(__.friend_state); } }
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
                    case __.friend_id : return _friend_id;
                    case __.user_id : return _user_id;
                    case __.friend_state : return _friend_state;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.friend_id : _friend_id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.friend_state : _friend_state = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得好友表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>好友id</summary>
            public static readonly Field friend_id = FindByName(__.friend_id);

            ///<summary>玩家id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>好友状态 0:好友 1:黑名单</summary>
            public static readonly Field friend_state = FindByName(__.friend_state);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得好友表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>好友id</summary>
            public const String friend_id = "friend_id";

            ///<summary>玩家id</summary>
            public const String user_id = "user_id";

            ///<summary>好友状态 0:好友 1:黑名单</summary>
            public const String friend_state = "friend_state";

        }
        #endregion
    }

    /// <summary>好友表接口</summary>
    public partial interface Itg_friends
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>好友id</summary>
        Int64 friend_id { get; set; }

        /// <summary>玩家id</summary>
        Int64 user_id { get; set; }

        /// <summary>好友状态 0:好友 1:黑名单</summary>
        Int32 friend_state { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}