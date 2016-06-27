using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>启服日志表</summary>
    [Serializable]
    [DataObject]
    [Description("启服日志表")]
    [BindTable("tg_log_run", Description = "启服日志表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_log_run : Itg_log_run
    {
        #region 属性
        private Int64 _id;
        /// <summary>ID</summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "ID", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private DateTime _time;
        /// <summary>执行时间</summary>
        [DisplayName("执行时间")]
        [Description("执行时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(2, "time", "执行时间", "getdate()", "datetime", 3, 0, false)]
        public virtual DateTime time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _type;
        /// <summary>运行类型</summary>
        [DisplayName("运行类型")]
        [Description("运行类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "type", "运行类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
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
                    case __.time : return _time;
                    case __.type : return _type;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.time : _time = Convert.ToDateTime(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得启服日志表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>执行时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>运行类型</summary>
            public static readonly Field type = FindByName(__.type);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得启服日志表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String id = "id";

            ///<summary>执行时间</summary>
            public const String time = "time";

            ///<summary>运行类型</summary>
            public const String type = "type";

        }
        #endregion
    }

    /// <summary>启服日志表接口</summary>
    public partial interface Itg_log_run
    {
        #region 属性
        /// <summary>ID</summary>
        Int64 id { get; set; }

        /// <summary>执行时间</summary>
        DateTime time { get; set; }

        /// <summary>运行类型</summary>
        Int32 type { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}