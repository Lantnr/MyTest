using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>身份分布记录表</summary>
    [DataObject]
    [Description("身份分布记录表")]
    [BindIndex("PK__report_i__3213E83F182C9B23", true, "id")]
    [BindTable("report_identity_day", Description = "身份分布记录表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class report_identity_day : Ireport_identity_day
    {
        #region 属性
        private Int32 _id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "编号", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _identity1_count;
        /// <summary>第一阶身份人数</summary>
        [DisplayName("第一阶身份人数")]
        [Description("第一阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "identity1_count", "第一阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity1_count
        {
            get { return _identity1_count; }
            set { if (OnPropertyChanging(__.identity1_count, value)) { _identity1_count = value; OnPropertyChanged(__.identity1_count); } }
        }

        private Int32 _identity2_count;
        /// <summary>第二阶身份人数</summary>
        [DisplayName("第二阶身份人数")]
        [Description("第二阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "identity2_count", "第二阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity2_count
        {
            get { return _identity2_count; }
            set { if (OnPropertyChanging(__.identity2_count, value)) { _identity2_count = value; OnPropertyChanged(__.identity2_count); } }
        }

        private Int32 _identity3_count;
        /// <summary>第三阶身份人数</summary>
        [DisplayName("第三阶身份人数")]
        [Description("第三阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "identity3_count", "第三阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity3_count
        {
            get { return _identity3_count; }
            set { if (OnPropertyChanging(__.identity3_count, value)) { _identity3_count = value; OnPropertyChanged(__.identity3_count); } }
        }

        private Int32 _identity4_count;
        /// <summary>第四阶身份人数</summary>
        [DisplayName("第四阶身份人数")]
        [Description("第四阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "identity4_count", "第四阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity4_count
        {
            get { return _identity4_count; }
            set { if (OnPropertyChanging(__.identity4_count, value)) { _identity4_count = value; OnPropertyChanged(__.identity4_count); } }
        }

        private Int32 _identity5_count;
        /// <summary>第五阶身份人数</summary>
        [DisplayName("第五阶身份人数")]
        [Description("第五阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "identity5_count", "第五阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity5_count
        {
            get { return _identity5_count; }
            set { if (OnPropertyChanging(__.identity5_count, value)) { _identity5_count = value; OnPropertyChanged(__.identity5_count); } }
        }

        private Int32 _identity6_count;
        /// <summary>第六阶身份人数</summary>
        [DisplayName("第六阶身份人数")]
        [Description("第六阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "identity6_count", "第六阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity6_count
        {
            get { return _identity6_count; }
            set { if (OnPropertyChanging(__.identity6_count, value)) { _identity6_count = value; OnPropertyChanged(__.identity6_count); } }
        }

        private Int32 _identity7_count;
        /// <summary>第七阶身份人数</summary>
        [DisplayName("第七阶身份人数")]
        [Description("第七阶身份人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "identity7_count", "第七阶身份人数", "0", "int", 10, 0, false)]
        public virtual Int32 identity7_count
        {
            get { return _identity7_count; }
            set { if (OnPropertyChanging(__.identity7_count, value)) { _identity7_count = value; OnPropertyChanged(__.identity7_count); } }
        }

        private Int64 _createtime;
        /// <summary>日期</summary>
        [DisplayName("日期")]
        [Description("日期")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "createtime", "日期", "0", "bigint", 19, 0, false)]
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
                    case __.identity1_count : return _identity1_count;
                    case __.identity2_count : return _identity2_count;
                    case __.identity3_count : return _identity3_count;
                    case __.identity4_count : return _identity4_count;
                    case __.identity5_count : return _identity5_count;
                    case __.identity6_count : return _identity6_count;
                    case __.identity7_count : return _identity7_count;
                    case __.createtime : return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.identity1_count : _identity1_count = Convert.ToInt32(value); break;
                    case __.identity2_count : _identity2_count = Convert.ToInt32(value); break;
                    case __.identity3_count : _identity3_count = Convert.ToInt32(value); break;
                    case __.identity4_count : _identity4_count = Convert.ToInt32(value); break;
                    case __.identity5_count : _identity5_count = Convert.ToInt32(value); break;
                    case __.identity6_count : _identity6_count = Convert.ToInt32(value); break;
                    case __.identity7_count : _identity7_count = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得身份分布记录表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>第一阶身份人数</summary>
            public static readonly Field identity1_count = FindByName(__.identity1_count);

            ///<summary>第二阶身份人数</summary>
            public static readonly Field identity2_count = FindByName(__.identity2_count);

            ///<summary>第三阶身份人数</summary>
            public static readonly Field identity3_count = FindByName(__.identity3_count);

            ///<summary>第四阶身份人数</summary>
            public static readonly Field identity4_count = FindByName(__.identity4_count);

            ///<summary>第五阶身份人数</summary>
            public static readonly Field identity5_count = FindByName(__.identity5_count);

            ///<summary>第六阶身份人数</summary>
            public static readonly Field identity6_count = FindByName(__.identity6_count);

            ///<summary>第七阶身份人数</summary>
            public static readonly Field identity7_count = FindByName(__.identity7_count);

            ///<summary>日期</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得身份分布记录表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>第一阶身份人数</summary>
            public const String identity1_count = "identity1_count";

            ///<summary>第二阶身份人数</summary>
            public const String identity2_count = "identity2_count";

            ///<summary>第三阶身份人数</summary>
            public const String identity3_count = "identity3_count";

            ///<summary>第四阶身份人数</summary>
            public const String identity4_count = "identity4_count";

            ///<summary>第五阶身份人数</summary>
            public const String identity5_count = "identity5_count";

            ///<summary>第六阶身份人数</summary>
            public const String identity6_count = "identity6_count";

            ///<summary>第七阶身份人数</summary>
            public const String identity7_count = "identity7_count";

            ///<summary>日期</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>身份分布记录表接口</summary>
    public partial interface Ireport_identity_day
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 id { get; set; }

        /// <summary>第一阶身份人数</summary>
        Int32 identity1_count { get; set; }

        /// <summary>第二阶身份人数</summary>
        Int32 identity2_count { get; set; }

        /// <summary>第三阶身份人数</summary>
        Int32 identity3_count { get; set; }

        /// <summary>第四阶身份人数</summary>
        Int32 identity4_count { get; set; }

        /// <summary>第五阶身份人数</summary>
        Int32 identity5_count { get; set; }

        /// <summary>第六阶身份人数</summary>
        Int32 identity6_count { get; set; }

        /// <summary>第七阶身份人数</summary>
        Int32 identity7_count { get; set; }

        /// <summary>日期</summary>
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