using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>开发模块</summary>
    [Serializable]
    [DataObject]
    [Description("开发模块")]
    [BindIndex("PK_tg_module_open", true, "id")]
    [BindTable("tg_module_open", Description = "开发模块", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_module_open : Itg_module_open
    {
        #region 属性
        private Int32 _id;
        /// <summary>ID</summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "ID", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "用户编号", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _module;
        /// <summary>模块编号</summary>
        [DisplayName("模块编号")]
        [Description("模块编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "module", "模块编号", null, "int", 10, 0, false)]
        public virtual Int32 module
        {
            get { return _module; }
            set { if (OnPropertyChanging(__.module, value)) { _module = value; OnPropertyChanged(__.module); } }
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
                    case __.module : return _module;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.module : _module = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得开发模块字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>模块编号</summary>
            public static readonly Field module = FindByName(__.module);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得开发模块字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String id = "id";

            ///<summary>用户编号</summary>
            public const String user_id = "user_id";

            ///<summary>模块编号</summary>
            public const String module = "module";

        }
        #endregion
    }

    /// <summary>开发模块接口</summary>
    public partial interface Itg_module_open
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 id { get; set; }

        /// <summary>用户编号</summary>
        Int64 user_id { get; set; }

        /// <summary>模块编号</summary>
        Int32 module { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}