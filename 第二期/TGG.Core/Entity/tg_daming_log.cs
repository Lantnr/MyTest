using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Daming_Log</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindIndex("PK_DaMingLog", true, "id")]
    [BindTable("tg_daming_log", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_daming_log : Itg_daming_log
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

        private Int32 _base_id;
        /// <summary>基表id</summary>
        [DisplayName("基表id")]
        [Description("基表id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "base_id", "基表id", null, "int", 10, 0, false)]
        public virtual Int32 base_id
        {
            get { return _base_id; }
            set { if (OnPropertyChanging(__.base_id, value)) { _base_id = value; OnPropertyChanged(__.base_id); } }
        }

        private Int32 _user_finish;
        /// <summary>用户完成度</summary>
        [DisplayName("用户完成度")]
        [Description("用户完成度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "user_finish", "用户完成度", null, "int", 10, 0, false)]
        public virtual Int32 user_finish
        {
            get { return _user_finish; }
            set { if (OnPropertyChanging(__.user_finish, value)) { _user_finish = value; OnPropertyChanged(__.user_finish); } }
        }

        private Int64 _user_id;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "user_id", "用户id", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _is_reward;
        /// <summary>是否领取奖励： 0：未领取 1：可领取  2：已领取</summary>
        [DisplayName("是否领取奖励： 0：未领取 1：可领取  2：已领取")]
        [Description("是否领取奖励： 0：未领取 1：可领取  2：已领取")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "is_reward", "是否领取奖励： 0：未领取 1：可领取  2：已领取", "0", "int", 10, 0, false)]
        public virtual Int32 is_reward
        {
            get { return _is_reward; }
            set { if (OnPropertyChanging(__.is_reward, value)) { _is_reward = value; OnPropertyChanged(__.is_reward); } }
        }

        private Int32 _is_finish;
        /// <summary>是否达到领取奖励标准 0：未达到 1：已经达到</summary>
        [DisplayName("是否达到领取奖励标准0：未达到1：已经达到")]
        [Description("是否达到领取奖励标准 0：未达到 1：已经达到")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "is_finish", "是否达到领取奖励标准 0：未达到 1：已经达到", "0", "int", 10, 0, false)]
        public virtual Int32 is_finish
        {
            get { return _is_finish; }
            set { if (OnPropertyChanging(__.is_finish, value)) { _is_finish = value; OnPropertyChanged(__.is_finish); } }
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
                    case __.base_id : return _base_id;
                    case __.user_finish : return _user_finish;
                    case __.user_id : return _user_id;
                    case __.is_reward : return _is_reward;
                    case __.is_finish : return _is_finish;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.base_id : _base_id = Convert.ToInt32(value); break;
                    case __.user_finish : _user_finish = Convert.ToInt32(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.is_reward : _is_reward = Convert.ToInt32(value); break;
                    case __.is_finish : _is_finish = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Daming_Log字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>基表id</summary>
            public static readonly Field base_id = FindByName(__.base_id);

            ///<summary>用户完成度</summary>
            public static readonly Field user_finish = FindByName(__.user_finish);

            ///<summary>用户id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>是否领取奖励： 0：未领取 1：可领取  2：已领取</summary>
            public static readonly Field is_reward = FindByName(__.is_reward);

            ///<summary>是否达到领取奖励标准 0：未达到 1：已经达到</summary>
            public static readonly Field is_finish = FindByName(__.is_finish);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Daming_Log字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>基表id</summary>
            public const String base_id = "base_id";

            ///<summary>用户完成度</summary>
            public const String user_finish = "user_finish";

            ///<summary>用户id</summary>
            public const String user_id = "user_id";

            ///<summary>是否领取奖励： 0：未领取 1：可领取  2：已领取</summary>
            public const String is_reward = "is_reward";

            ///<summary>是否达到领取奖励标准 0：未达到 1：已经达到</summary>
            public const String is_finish = "is_finish";

        }
        #endregion
    }

    /// <summary>Daming_Log接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_daming_log
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>基表id</summary>
        Int32 base_id { get; set; }

        /// <summary>用户完成度</summary>
        Int32 user_finish { get; set; }

        /// <summary>用户id</summary>
        Int64 user_id { get; set; }

        /// <summary>是否领取奖励： 0：未领取 1：可领取  2：已领取</summary>
        Int32 is_reward { get; set; }

        /// <summary>是否达到领取奖励标准 0：未达到 1：已经达到</summary>
        Int32 is_finish { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}