using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>监狱表</summary>
    [Serializable]
    [DataObject]
    [Description("监狱表")]
    [BindIndex("PK_tg_prison", true, "id")]
    [BindTable("tg_prison", Description = "监狱表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_prison : Itg_prison
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

        private Int64 _prison_time;
        /// <summary>服刑时间</summary>
        [DisplayName("服刑时间")]
        [Description("服刑时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "prison_time", "服刑时间", null, "bigint", 19, 0, false)]
        public virtual Int64 prison_time
        {
            get { return _prison_time; }
            set { if (OnPropertyChanging(__.prison_time, value)) { _prison_time = value; OnPropertyChanged(__.prison_time); } }
        }

        private Int64 _user_id;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "user_id", "用户id", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
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
                    case __.prison_time : return _prison_time;
                    case __.user_id : return _user_id;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.prison_time : _prison_time = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得监狱表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>服刑时间</summary>
            public static readonly Field prison_time = FindByName(__.prison_time);

            ///<summary>用户id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得监狱表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>服刑时间</summary>
            public const String prison_time = "prison_time";

            ///<summary>用户id</summary>
            public const String user_id = "user_id";

        }
        #endregion
    }

    /// <summary>监狱表接口</summary>
    public partial interface Itg_prison
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>服刑时间</summary>
        Int64 prison_time { get; set; }

        /// <summary>用户id</summary>
        Int64 user_id { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}