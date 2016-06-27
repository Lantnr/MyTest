using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>等级分布记录表</summary>
    [DataObject]
    [Description("等级分布记录表")]
    [BindIndex("PK__report_l__3213E83F239E4DCF", true, "id")]
    [BindTable("report_level_day", Description = "等级分布记录表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class report_level_day : Ireport_level_day
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

        private Int32 _stage1_count;
        /// <summary>第一阶段等级人数 1~20级</summary>
        [DisplayName("第一阶段等级人数1~20级")]
        [Description("第一阶段等级人数 1~20级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "stage1_count", "第一阶段等级人数 1~20级", "0", "int", 10, 0, false)]
        public virtual Int32 stage1_count
        {
            get { return _stage1_count; }
            set { if (OnPropertyChanging(__.stage1_count, value)) { _stage1_count = value; OnPropertyChanged(__.stage1_count); } }
        }

        private Int32 _stage2_count;
        /// <summary>第二阶段等级人数 21~30级</summary>
        [DisplayName("第二阶段等级人数21~30级")]
        [Description("第二阶段等级人数 21~30级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "stage2_count", "第二阶段等级人数 21~30级", "0", "int", 10, 0, false)]
        public virtual Int32 stage2_count
        {
            get { return _stage2_count; }
            set { if (OnPropertyChanging(__.stage2_count, value)) { _stage2_count = value; OnPropertyChanged(__.stage2_count); } }
        }

        private Int32 _stage3_count;
        /// <summary>第三阶段等级人数 31~35级</summary>
        [DisplayName("第三阶段等级人数31~35级")]
        [Description("第三阶段等级人数 31~35级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "stage3_count", "第三阶段等级人数 31~35级", "0", "int", 10, 0, false)]
        public virtual Int32 stage3_count
        {
            get { return _stage3_count; }
            set { if (OnPropertyChanging(__.stage3_count, value)) { _stage3_count = value; OnPropertyChanged(__.stage3_count); } }
        }

        private Int32 _stage4_count;
        /// <summary>第四阶段等级人数 36~40级</summary>
        [DisplayName("第四阶段等级人数36~40级")]
        [Description("第四阶段等级人数 36~40级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "stage4_count", "第四阶段等级人数 36~40级", "0", "int", 10, 0, false)]
        public virtual Int32 stage4_count
        {
            get { return _stage4_count; }
            set { if (OnPropertyChanging(__.stage4_count, value)) { _stage4_count = value; OnPropertyChanged(__.stage4_count); } }
        }

        private Int32 _stage5_count;
        /// <summary>第五阶段等级人数 41~45级</summary>
        [DisplayName("第五阶段等级人数41~45级")]
        [Description("第五阶段等级人数 41~45级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "stage5_count", "第五阶段等级人数 41~45级", "0", "int", 10, 0, false)]
        public virtual Int32 stage5_count
        {
            get { return _stage5_count; }
            set { if (OnPropertyChanging(__.stage5_count, value)) { _stage5_count = value; OnPropertyChanged(__.stage5_count); } }
        }

        private Int32 _stage6_count;
        /// <summary>第六阶段等级人数 46~50级</summary>
        [DisplayName("第六阶段等级人数46~50级")]
        [Description("第六阶段等级人数 46~50级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "stage6_count", "第六阶段等级人数 46~50级", "0", "int", 10, 0, false)]
        public virtual Int32 stage6_count
        {
            get { return _stage6_count; }
            set { if (OnPropertyChanging(__.stage6_count, value)) { _stage6_count = value; OnPropertyChanged(__.stage6_count); } }
        }

        private Int32 _stage7_count;
        /// <summary>第七阶段等级人数 51~55级</summary>
        [DisplayName("第七阶段等级人数51~55级")]
        [Description("第七阶段等级人数 51~55级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "stage7_count", "第七阶段等级人数 51~55级", "0", "int", 10, 0, false)]
        public virtual Int32 stage7_count
        {
            get { return _stage7_count; }
            set { if (OnPropertyChanging(__.stage7_count, value)) { _stage7_count = value; OnPropertyChanged(__.stage7_count); } }
        }

        private Int32 _stage8_count;
        /// <summary>第八阶段等级人数 56~60级</summary>
        [DisplayName("第八阶段等级人数56~60级")]
        [Description("第八阶段等级人数 56~60级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "stage8_count", "第八阶段等级人数 56~60级", "0", "int", 10, 0, false)]
        public virtual Int32 stage8_count
        {
            get { return _stage8_count; }
            set { if (OnPropertyChanging(__.stage8_count, value)) { _stage8_count = value; OnPropertyChanged(__.stage8_count); } }
        }

        private Int32 _total_count;
        /// <summary>今日总人数</summary>
        [DisplayName("今日总人数")]
        [Description("今日总人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "total_count", "今日总人数", "0", "int", 10, 0, false)]
        public virtual Int32 total_count
        {
            get { return _total_count; }
            set { if (OnPropertyChanging(__.total_count, value)) { _total_count = value; OnPropertyChanged(__.total_count); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间（日期）</summary>
        [DisplayName("创建时间日期")]
        [Description("创建时间（日期）")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "createtime", "创建时间（日期）", "0", "bigint", 19, 0, false)]
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
                    case __.stage1_count : return _stage1_count;
                    case __.stage2_count : return _stage2_count;
                    case __.stage3_count : return _stage3_count;
                    case __.stage4_count : return _stage4_count;
                    case __.stage5_count : return _stage5_count;
                    case __.stage6_count : return _stage6_count;
                    case __.stage7_count : return _stage7_count;
                    case __.stage8_count : return _stage8_count;
                    case __.total_count : return _total_count;
                    case __.createtime : return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.stage1_count : _stage1_count = Convert.ToInt32(value); break;
                    case __.stage2_count : _stage2_count = Convert.ToInt32(value); break;
                    case __.stage3_count : _stage3_count = Convert.ToInt32(value); break;
                    case __.stage4_count : _stage4_count = Convert.ToInt32(value); break;
                    case __.stage5_count : _stage5_count = Convert.ToInt32(value); break;
                    case __.stage6_count : _stage6_count = Convert.ToInt32(value); break;
                    case __.stage7_count : _stage7_count = Convert.ToInt32(value); break;
                    case __.stage8_count : _stage8_count = Convert.ToInt32(value); break;
                    case __.total_count : _total_count = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得等级分布记录表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>第一阶段等级人数 1~20级</summary>
            public static readonly Field stage1_count = FindByName(__.stage1_count);

            ///<summary>第二阶段等级人数 21~30级</summary>
            public static readonly Field stage2_count = FindByName(__.stage2_count);

            ///<summary>第三阶段等级人数 31~35级</summary>
            public static readonly Field stage3_count = FindByName(__.stage3_count);

            ///<summary>第四阶段等级人数 36~40级</summary>
            public static readonly Field stage4_count = FindByName(__.stage4_count);

            ///<summary>第五阶段等级人数 41~45级</summary>
            public static readonly Field stage5_count = FindByName(__.stage5_count);

            ///<summary>第六阶段等级人数 46~50级</summary>
            public static readonly Field stage6_count = FindByName(__.stage6_count);

            ///<summary>第七阶段等级人数 51~55级</summary>
            public static readonly Field stage7_count = FindByName(__.stage7_count);

            ///<summary>第八阶段等级人数 56~60级</summary>
            public static readonly Field stage8_count = FindByName(__.stage8_count);

            ///<summary>今日总人数</summary>
            public static readonly Field total_count = FindByName(__.total_count);

            ///<summary>创建时间（日期）</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得等级分布记录表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>第一阶段等级人数 1~20级</summary>
            public const String stage1_count = "stage1_count";

            ///<summary>第二阶段等级人数 21~30级</summary>
            public const String stage2_count = "stage2_count";

            ///<summary>第三阶段等级人数 31~35级</summary>
            public const String stage3_count = "stage3_count";

            ///<summary>第四阶段等级人数 36~40级</summary>
            public const String stage4_count = "stage4_count";

            ///<summary>第五阶段等级人数 41~45级</summary>
            public const String stage5_count = "stage5_count";

            ///<summary>第六阶段等级人数 46~50级</summary>
            public const String stage6_count = "stage6_count";

            ///<summary>第七阶段等级人数 51~55级</summary>
            public const String stage7_count = "stage7_count";

            ///<summary>第八阶段等级人数 56~60级</summary>
            public const String stage8_count = "stage8_count";

            ///<summary>今日总人数</summary>
            public const String total_count = "total_count";

            ///<summary>创建时间（日期）</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>等级分布记录表接口</summary>
    public partial interface Ireport_level_day
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 id { get; set; }

        /// <summary>第一阶段等级人数 1~20级</summary>
        Int32 stage1_count { get; set; }

        /// <summary>第二阶段等级人数 21~30级</summary>
        Int32 stage2_count { get; set; }

        /// <summary>第三阶段等级人数 31~35级</summary>
        Int32 stage3_count { get; set; }

        /// <summary>第四阶段等级人数 36~40级</summary>
        Int32 stage4_count { get; set; }

        /// <summary>第五阶段等级人数 41~45级</summary>
        Int32 stage5_count { get; set; }

        /// <summary>第六阶段等级人数 46~50级</summary>
        Int32 stage6_count { get; set; }

        /// <summary>第七阶段等级人数 51~55级</summary>
        Int32 stage7_count { get; set; }

        /// <summary>第八阶段等级人数 56~60级</summary>
        Int32 stage8_count { get; set; }

        /// <summary>今日总人数</summary>
        Int32 total_count { get; set; }

        /// <summary>创建时间（日期）</summary>
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