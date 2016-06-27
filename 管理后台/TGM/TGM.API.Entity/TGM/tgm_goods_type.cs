using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>福利卡类型表</summary>
    [Serializable]
    [DataObject]
    [Description("福利卡类型表")]
    [BindTable("tgm_goods_type", Description = "福利卡类型表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_goods_type : Itgm_goods_type
    {
        #region 属性
        private Int32 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "主键id", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _type_id;
        /// <summary>福利卡类型枚举Id</summary>
        [DisplayName("福利卡类型枚举Id")]
        [Description("福利卡类型枚举Id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "type_id", "福利卡类型枚举Id", "0", "int", 10, 0, false)]
        public virtual Int32 type_id
        {
            get { return _type_id; }
            set { if (OnPropertyChanging(__.type_id, value)) { _type_id = value; OnPropertyChanged(__.type_id); } }
        }

        private String _name;
        /// <summary>福利卡类型名称</summary>
        [DisplayName("福利卡类型名称")]
        [Description("福利卡类型名称")]
        [DataObjectField(false, false, false, 20)]
        [BindColumn(3, "name", "福利卡类型名称", "0", "nvarchar(20)", 0, 0, true)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
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
                    case __.type_id : return _type_id;
                    case __.name : return _name;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.type_id : _type_id = Convert.ToInt32(value); break;
                    case __.name : _name = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得福利卡类型表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>福利卡类型枚举Id</summary>
            public static readonly Field type_id = FindByName(__.type_id);

            ///<summary>福利卡类型名称</summary>
            public static readonly Field name = FindByName(__.name);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得福利卡类型表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>福利卡类型枚举Id</summary>
            public const String type_id = "type_id";

            ///<summary>福利卡类型名称</summary>
            public const String name = "name";

        }
        #endregion
    }

    /// <summary>福利卡类型表接口</summary>
    public partial interface Itgm_goods_type
    {
        #region 属性
        /// <summary>主键id</summary>
        Int32 id { get; set; }

        /// <summary>福利卡类型枚举Id</summary>
        Int32 type_id { get; set; }

        /// <summary>福利卡类型名称</summary>
        String name { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}