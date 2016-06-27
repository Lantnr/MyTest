using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>家族申请表</summary>
    [Serializable]
    [DataObject]
    [Description("家族申请表")]
    [BindIndex("PK_tg_family_apply", true, "id")]
    [BindTable("tg_family_apply", Description = "家族申请表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_family_apply : Itg_family_apply
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

        private Int64 _userid;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "userid", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int32 _state;
        /// <summary>申请结果</summary>
        [DisplayName("申请结果")]
        [Description("申请结果")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "state", "申请结果", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int64 _time;
        /// <summary>申请时间</summary>
        [DisplayName("申请时间")]
        [Description("申请时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "time", "申请时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int64 _fid;
        /// <summary>家族编号</summary>
        [DisplayName("家族编号")]
        [Description("家族编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "fid", "家族编号", "0", "bigint", 19, 0, false)]
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
                    case __.userid : return _userid;
                    case __.state : return _state;
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
                    case __.userid : _userid = Convert.ToInt64(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.fid : _fid = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得家族申请表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary>申请结果</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>申请时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>家族编号</summary>
            public static readonly Field fid = FindByName(__.fid);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得家族申请表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String userid = "userid";

            ///<summary>申请结果</summary>
            public const String state = "state";

            ///<summary>申请时间</summary>
            public const String time = "time";

            ///<summary>家族编号</summary>
            public const String fid = "fid";

        }
        #endregion
    }

    /// <summary>家族申请表接口</summary>
    public partial interface Itg_family_apply
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 userid { get; set; }

        /// <summary>申请结果</summary>
        Int32 state { get; set; }

        /// <summary>申请时间</summary>
        Int64 time { get; set; }

        /// <summary>家族编号</summary>
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