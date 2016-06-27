using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>家族日志</summary>
    [Serializable]
    [DataObject]
    [Description("家族日志")]
    [BindIndex("PK_tg_family_log", true, "id")]
    [BindTable("tg_family_log", Description = "家族日志", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_family_log : Itg_family_log
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

        private Int32 _baseid;
        /// <summary>日志基表id</summary>
        [DisplayName("日志基表id")]
        [Description("日志基表id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "baseid", "日志基表id", "0", "int", 10, 0, false)]
        public virtual Int32 baseid
        {
            get { return _baseid; }
            set { if (OnPropertyChanging(__.baseid, value)) { _baseid = value; OnPropertyChanged(__.baseid); } }
        }

        private Int32 _type;
        /// <summary>事件类型</summary>
        [DisplayName("事件类型")]
        [Description("事件类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "type", "事件类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int64 _userid;
        /// <summary></summary>
        [DisplayName("Userid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "userid", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int64 _time;
        /// <summary>日志时间</summary>
        [DisplayName("日志时间")]
        [Description("日志时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "time", "日志时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int64 _fid;
        /// <summary>家族id</summary>
        [DisplayName("家族id")]
        [Description("家族id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "fid", "家族id", "0", "bigint", 19, 0, false)]
        public virtual Int64 fid
        {
            get { return _fid; }
            set { if (OnPropertyChanging(__.fid, value)) { _fid = value; OnPropertyChanged(__.fid); } }
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
                    case __.baseid : return _baseid;
                    case __.type : return _type;
                    case __.userid : return _userid;
                    case __.time : return _time;
                    case __.fid : return _fid;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.baseid : _baseid = Convert.ToInt32(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.userid : _userid = Convert.ToInt64(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.fid : _fid = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得家族日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>日志基表id</summary>
            public static readonly Field baseid = FindByName(__.baseid);

            ///<summary>事件类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary></summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary>日志时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>家族id</summary>
            public static readonly Field fid = FindByName(__.fid);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得家族日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>日志基表id</summary>
            public const String baseid = "baseid";

            ///<summary>事件类型</summary>
            public const String type = "type";

            ///<summary></summary>
            public const String userid = "userid";

            ///<summary>日志时间</summary>
            public const String time = "time";

            ///<summary>家族id</summary>
            public const String fid = "fid";

        }
        #endregion
    }

    /// <summary>家族日志接口</summary>
    public partial interface Itg_family_log
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>日志基表id</summary>
        Int32 baseid { get; set; }

        /// <summary>事件类型</summary>
        Int32 type { get; set; }

        /// <summary></summary>
        Int64 userid { get; set; }

        /// <summary>日志时间</summary>
        Int64 time { get; set; }

        /// <summary>家族id</summary>
        Int64 fid { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}