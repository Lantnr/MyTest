using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>家族成员表</summary>
    [Serializable]
    [DataObject]
    [Description("家族成员表")]
    [BindIndex("PK_tg_family_member", true, "id")]
    [BindTable("tg_family_member", Description = "家族成员表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_family_member : Itg_family_member
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

        private Int64 _fid;
        /// <summary>家族表编号</summary>
        [DisplayName("家族表编号")]
        [Description("家族表编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "fid", "家族表编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 fid
        {
            get { return _fid; }
            set { if (OnPropertyChanging(__.fid, value)) { _fid = value; OnPropertyChanged(__.fid); } }
        }

        private Int64 _userid;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "userid", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int32 _degree;
        /// <summary>身份</summary>
        [DisplayName("身份")]
        [Description("身份")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "degree", "身份", "0", "int", 10, 0, false)]
        public virtual Int32 degree
        {
            get { return _degree; }
            set { if (OnPropertyChanging(__.degree, value)) { _degree = value; OnPropertyChanged(__.degree); } }
        }

        private Int32 _devote;
        /// <summary>贡献</summary>
        [DisplayName("贡献")]
        [Description("贡献")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "devote", "贡献", "0", "int", 10, 0, false)]
        public virtual Int32 devote
        {
            get { return _devote; }
            set { if (OnPropertyChanging(__.devote, value)) { _devote = value; OnPropertyChanged(__.devote); } }
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
                    case __.fid : return _fid;
                    case __.userid : return _userid;
                    case __.degree : return _degree;
                    case __.devote : return _devote;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.fid : _fid = Convert.ToInt64(value); break;
                    case __.userid : _userid = Convert.ToInt64(value); break;
                    case __.degree : _degree = Convert.ToInt32(value); break;
                    case __.devote : _devote = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得家族成员表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>家族表编号</summary>
            public static readonly Field fid = FindByName(__.fid);

            ///<summary>玩家编号</summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary>身份</summary>
            public static readonly Field degree = FindByName(__.degree);

            ///<summary>贡献</summary>
            public static readonly Field devote = FindByName(__.devote);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得家族成员表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>家族表编号</summary>
            public const String fid = "fid";

            ///<summary>玩家编号</summary>
            public const String userid = "userid";

            ///<summary>身份</summary>
            public const String degree = "degree";

            ///<summary>贡献</summary>
            public const String devote = "devote";

        }
        #endregion
    }

    /// <summary>家族成员表接口</summary>
    public partial interface Itg_family_member
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>家族表编号</summary>
        Int64 fid { get; set; }

        /// <summary>玩家编号</summary>
        Int64 userid { get; set; }

        /// <summary>身份</summary>
        Int32 degree { get; set; }

        /// <summary>贡献</summary>
        Int32 devote { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}