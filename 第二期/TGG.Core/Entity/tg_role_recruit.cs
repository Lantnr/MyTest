using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>武将招募表</summary>
    [Serializable]
    [DataObject]
    [Description("武将招募表")]
    [BindTable("tg_role_recruit", Description = "武将招募表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_role_recruit : Itg_role_recruit
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键</summary>
        [DisplayName("主键")]
        [Description("主键")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "用户id", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _prop_id;
        /// <summary>道具基表ID</summary>
        [DisplayName("道具基表ID")]
        [Description("道具基表ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "prop_id", "道具基表ID", "0", "int", 10, 0, false)]
        public virtual Int32 prop_id
        {
            get { return _prop_id; }
            set { if (OnPropertyChanging(__.prop_id, value)) { _prop_id = value; OnPropertyChanged(__.prop_id); } }
        }

        private Int32 _role_id;
        /// <summary>武将基表id</summary>
        [DisplayName("武将基表id")]
        [Description("武将基表id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "role_id", "武将基表id", "0", "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private Int32 _position;
        /// <summary>武将位置</summary>
        [DisplayName("武将位置")]
        [Description("武将位置")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "position", "武将位置", "0", "int", 10, 0, false)]
        public virtual Int32 position
        {
            get { return _position; }
            set { if (OnPropertyChanging(__.position, value)) { _position = value; OnPropertyChanged(__.position); } }
        }

        private Int32 _grade;
        /// <summary>品质</summary>
        [DisplayName("品质")]
        [Description("品质")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "grade", "品质", "0", "int", 10, 0, false)]
        public virtual Int32 grade
        {
            get { return _grade; }
            set { if (OnPropertyChanging(__.grade, value)) { _grade = value; OnPropertyChanged(__.grade); } }
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
                    case __.prop_id : return _prop_id;
                    case __.role_id : return _role_id;
                    case __.position : return _position;
                    case __.grade : return _grade;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.prop_id : _prop_id = Convert.ToInt32(value); break;
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.position : _position = Convert.ToInt32(value); break;
                    case __.grade : _grade = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得武将招募表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>道具基表ID</summary>
            public static readonly Field prop_id = FindByName(__.prop_id);

            ///<summary>武将基表id</summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary>武将位置</summary>
            public static readonly Field position = FindByName(__.position);

            ///<summary>品质</summary>
            public static readonly Field grade = FindByName(__.grade);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得武将招募表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键</summary>
            public const String id = "id";

            ///<summary>用户id</summary>
            public const String user_id = "user_id";

            ///<summary>道具基表ID</summary>
            public const String prop_id = "prop_id";

            ///<summary>武将基表id</summary>
            public const String role_id = "role_id";

            ///<summary>武将位置</summary>
            public const String position = "position";

            ///<summary>品质</summary>
            public const String grade = "grade";

        }
        #endregion
    }

    /// <summary>武将招募表接口</summary>
    public partial interface Itg_role_recruit
    {
        #region 属性
        /// <summary>主键</summary>
        Int64 id { get; set; }

        /// <summary>用户id</summary>
        Int64 user_id { get; set; }

        /// <summary>道具基表ID</summary>
        Int32 prop_id { get; set; }

        /// <summary>武将基表id</summary>
        Int32 role_id { get; set; }

        /// <summary>武将位置</summary>
        Int32 position { get; set; }

        /// <summary>品质</summary>
        Int32 grade { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}