using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家称号表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家称号表")]
    [BindIndex("PK_tg_user_title", true, "id")]
    [BindTable("tg_user_title", Description = "玩家称号表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_title : Itg_user_title
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

        private Int64 _user_id;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _title_id;
        /// <summary>称号基表编号</summary>
        [DisplayName("称号基表编号")]
        [Description("称号基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "title_id", "称号基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 title_id
        {
            get { return _title_id; }
            set { if (OnPropertyChanging(__.title_id, value)) { _title_id = value; OnPropertyChanged(__.title_id); } }
        }

        private Int32 _title_type;
        /// <summary></summary>
        [DisplayName("Type")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "title_type", "", "0", "int", 10, 0, false)]
        public virtual Int32 title_type
        {
            get { return _title_type; }
            set { if (OnPropertyChanging(__.title_type, value)) { _title_type = value; OnPropertyChanged(__.title_type); } }
        }

        private Int32 _title_order;
        /// <summary>称号顺序</summary>
        [DisplayName("称号顺序")]
        [Description("称号顺序")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "title_order", "称号顺序", null, "int", 10, 0, false)]
        public virtual Int32 title_order
        {
            get { return _title_order; }
            set { if (OnPropertyChanging(__.title_order, value)) { _title_order = value; OnPropertyChanged(__.title_order); } }
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
                    case __.title_id : return _title_id;
                    case __.title_type : return _title_type;
                    case __.title_order : return _title_order;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.title_id : _title_id = Convert.ToInt32(value); break;
                    case __.title_type : _title_type = Convert.ToInt32(value); break;
                    case __.title_order : _title_order = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家称号表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>称号基表编号</summary>
            public static readonly Field title_id = FindByName(__.title_id);

            ///<summary></summary>
            public static readonly Field title_type = FindByName(__.title_type);

            ///<summary>称号顺序</summary>
            public static readonly Field title_order = FindByName(__.title_order);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家称号表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>称号基表编号</summary>
            public const String title_id = "title_id";

            ///<summary></summary>
            public const String title_type = "title_type";

            ///<summary>称号顺序</summary>
            public const String title_order = "title_order";

        }
        #endregion
    }

    /// <summary>玩家称号表接口</summary>
    public partial interface Itg_user_title
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>称号基表编号</summary>
        Int32 title_id { get; set; }

        /// <summary></summary>
        Int32 title_type { get; set; }

        /// <summary>称号顺序</summary>
        Int32 title_order { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}