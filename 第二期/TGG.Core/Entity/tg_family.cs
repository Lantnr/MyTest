using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>家族</summary>
    [Serializable]
    [DataObject]
    [Description("家族")]
    [BindIndex("PK_tg_family", true, "id")]
    [BindTable("tg_family", Description = "家族", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_family : Itg_family
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

        private String _family_name;
        /// <summary>家族名字</summary>
        [DisplayName("家族名字")]
        [Description("家族名字")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "family_name", "家族名字", null, "nvarchar(50)", 0, 0, true)]
        public virtual String family_name
        {
            get { return _family_name; }
            set { if (OnPropertyChanging(__.family_name, value)) { _family_name = value; OnPropertyChanged(__.family_name); } }
        }

        private Int32 _family_level;
        /// <summary>等级</summary>
        [DisplayName("等级")]
        [Description("等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "family_level", "等级", "0", "int", 10, 0, false)]
        public virtual Int32 family_level
        {
            get { return _family_level; }
            set { if (OnPropertyChanging(__.family_level, value)) { _family_level = value; OnPropertyChanged(__.family_level); } }
        }

        private Int32 _number;
        /// <summary>家族人数</summary>
        [DisplayName("家族人数")]
        [Description("家族人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "number", "家族人数", "0", "int", 10, 0, false)]
        public virtual Int32 number
        {
            get { return _number; }
            set { if (OnPropertyChanging(__.number, value)) { _number = value; OnPropertyChanged(__.number); } }
        }

        private Int32 _resource;
        /// <summary>家族资源</summary>
        [DisplayName("家族资源")]
        [Description("家族资源")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "resource", "家族资源", "0", "int", 10, 0, false)]
        public virtual Int32 resource
        {
            get { return _resource; }
            set { if (OnPropertyChanging(__.resource, value)) { _resource = value; OnPropertyChanged(__.resource); } }
        }

        private Int32 _salary;
        /// <summary>俸禄</summary>
        [DisplayName("俸禄")]
        [Description("俸禄")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "salary", "俸禄", "0", "int", 10, 0, false)]
        public virtual Int32 salary
        {
            get { return _salary; }
            set { if (OnPropertyChanging(__.salary, value)) { _salary = value; OnPropertyChanged(__.salary); } }
        }

        private Int32 _renown;
        /// <summary>名望</summary>
        [DisplayName("名望")]
        [Description("名望")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "renown", "名望", "0", "int", 10, 0, false)]
        public virtual Int32 renown
        {
            get { return _renown; }
            set { if (OnPropertyChanging(__.renown, value)) { _renown = value; OnPropertyChanged(__.renown); } }
        }

        private Int32 _clanbadge;
        /// <summary>族徽基础id</summary>
        [DisplayName("族徽基础id")]
        [Description("族徽基础id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "clanbadge", "族徽基础id", "0", "int", 10, 0, false)]
        public virtual Int32 clanbadge
        {
            get { return _clanbadge; }
            set { if (OnPropertyChanging(__.clanbadge, value)) { _clanbadge = value; OnPropertyChanged(__.clanbadge); } }
        }

        private String _notice;
        /// <summary>公告</summary>
        [DisplayName("公告")]
        [Description("公告")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(9, "notice", "公告", null, "nvarchar(MAX)", 0, 0, true)]
        public virtual String notice
        {
            get { return _notice; }
            set { if (OnPropertyChanging(__.notice, value)) { _notice = value; OnPropertyChanged(__.notice); } }
        }

        private Int64 _userid;
        /// <summary>族长id</summary>
        [DisplayName("族长id")]
        [Description("族长id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "userid", "族长id", null, "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int64 _time;
        /// <summary>升级时间</summary>
        [DisplayName("升级时间")]
        [Description("升级时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "time", "升级时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
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
                    case __.family_name : return _family_name;
                    case __.family_level : return _family_level;
                    case __.number : return _number;
                    case __.resource : return _resource;
                    case __.salary : return _salary;
                    case __.renown : return _renown;
                    case __.clanbadge : return _clanbadge;
                    case __.notice : return _notice;
                    case __.userid : return _userid;
                    case __.time : return _time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.family_name : _family_name = Convert.ToString(value); break;
                    case __.family_level : _family_level = Convert.ToInt32(value); break;
                    case __.number : _number = Convert.ToInt32(value); break;
                    case __.resource : _resource = Convert.ToInt32(value); break;
                    case __.salary : _salary = Convert.ToInt32(value); break;
                    case __.renown : _renown = Convert.ToInt32(value); break;
                    case __.clanbadge : _clanbadge = Convert.ToInt32(value); break;
                    case __.notice : _notice = Convert.ToString(value); break;
                    case __.userid : _userid = Convert.ToInt64(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得家族字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>家族名字</summary>
            public static readonly Field family_name = FindByName(__.family_name);

            ///<summary>等级</summary>
            public static readonly Field family_level = FindByName(__.family_level);

            ///<summary>家族人数</summary>
            public static readonly Field number = FindByName(__.number);

            ///<summary>家族资源</summary>
            public static readonly Field resource = FindByName(__.resource);

            ///<summary>俸禄</summary>
            public static readonly Field salary = FindByName(__.salary);

            ///<summary>名望</summary>
            public static readonly Field renown = FindByName(__.renown);

            ///<summary>族徽基础id</summary>
            public static readonly Field clanbadge = FindByName(__.clanbadge);

            ///<summary>公告</summary>
            public static readonly Field notice = FindByName(__.notice);

            ///<summary>族长id</summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary>升级时间</summary>
            public static readonly Field time = FindByName(__.time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得家族字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>家族名字</summary>
            public const String family_name = "family_name";

            ///<summary>等级</summary>
            public const String family_level = "family_level";

            ///<summary>家族人数</summary>
            public const String number = "number";

            ///<summary>家族资源</summary>
            public const String resource = "resource";

            ///<summary>俸禄</summary>
            public const String salary = "salary";

            ///<summary>名望</summary>
            public const String renown = "renown";

            ///<summary>族徽基础id</summary>
            public const String clanbadge = "clanbadge";

            ///<summary>公告</summary>
            public const String notice = "notice";

            ///<summary>族长id</summary>
            public const String userid = "userid";

            ///<summary>升级时间</summary>
            public const String time = "time";

        }
        #endregion
    }

    /// <summary>家族接口</summary>
    public partial interface Itg_family
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>家族名字</summary>
        String family_name { get; set; }

        /// <summary>等级</summary>
        Int32 family_level { get; set; }

        /// <summary>家族人数</summary>
        Int32 number { get; set; }

        /// <summary>家族资源</summary>
        Int32 resource { get; set; }

        /// <summary>俸禄</summary>
        Int32 salary { get; set; }

        /// <summary>名望</summary>
        Int32 renown { get; set; }

        /// <summary>族徽基础id</summary>
        Int32 clanbadge { get; set; }

        /// <summary>公告</summary>
        String notice { get; set; }

        /// <summary>族长id</summary>
        Int64 userid { get; set; }

        /// <summary>升级时间</summary>
        Int64 time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}