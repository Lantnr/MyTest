using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>角色表</summary>
    [DataObject]
    [Description("角色表")]
    [BindIndex("PK_tgm_role", true, "id")]
    [BindTable("tgm_role", Description = "角色表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_role : Itgm_role
    {
        #region 属性
        private Int32 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _pid;
        /// <summary></summary>
        [DisplayName("Pid")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private String _name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "name", "名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
        }

        private String _password;
        /// <summary>密码</summary>
        [DisplayName("密码")]
        [Description("密码")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "password", "密码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String password
        {
            get { return _password; }
            set { if (OnPropertyChanging(__.password, value)) { _password = value; OnPropertyChanged(__.password); } }
        }

        private Int32 _role;
        /// <summary></summary>
        [DisplayName("Role")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "role", "", "0", "int", 10, 0, false)]
        public virtual Int32 role
        {
            get { return _role; }
            set { if (OnPropertyChanging(__.role, value)) { _role = value; OnPropertyChanged(__.role); } }
        }

        private Int64 _createtime;
        /// <summary></summary>
        [DisplayName("Createtime")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "createtime", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
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
                    case __.pid : return _pid;
                    case __.name : return _name;
                    case __.password : return _password;
                    case __.role : return _role;
                    case __.createtime : return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.pid : _pid = Convert.ToInt32(value); break;
                    case __.name : _name = Convert.ToString(value); break;
                    case __.password : _password = Convert.ToString(value); break;
                    case __.role : _role = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得角色表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>名称</summary>
            public static readonly Field name = FindByName(__.name);

            ///<summary>密码</summary>
            public static readonly Field password = FindByName(__.password);

            ///<summary></summary>
            public static readonly Field role = FindByName(__.role);

            ///<summary></summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得角色表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String pid = "pid";

            ///<summary>名称</summary>
            public const String name = "name";

            ///<summary>密码</summary>
            public const String password = "password";

            ///<summary></summary>
            public const String role = "role";

            ///<summary></summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>角色表接口</summary>
    public partial interface Itgm_role
    {
        #region 属性
        /// <summary></summary>
        Int32 id { get; set; }

        /// <summary></summary>
        Int32 pid { get; set; }

        /// <summary>名称</summary>
        String name { get; set; }

        /// <summary>密码</summary>
        String password { get; set; }

        /// <summary></summary>
        Int32 role { get; set; }

        /// <summary></summary>
        Int64 createtime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}