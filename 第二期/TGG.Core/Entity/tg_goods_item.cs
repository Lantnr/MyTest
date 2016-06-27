using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家町货物表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家町货物表")]
    [BindIndex("PK_tg_goods_item", true, "id")]
    [BindTable("tg_goods_item", Description = "玩家町货物表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_goods_item : Itg_goods_item
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
        /// <summary>玩家ID</summary>
        [DisplayName("玩家ID")]
        [Description("玩家ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "玩家ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _ting_id;
        /// <summary>町基表编号</summary>
        [DisplayName("町基表编号")]
        [Description("町基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "ting_id", "町基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 ting_id
        {
            get { return _ting_id; }
            set { if (OnPropertyChanging(__.ting_id, value)) { _ting_id = value; OnPropertyChanged(__.ting_id); } }
        }

        private Int32 _goods_id;
        /// <summary>全局城市货物关联ID</summary>
        [DisplayName("全局城市货物关联ID")]
        [Description("全局城市货物关联ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "goods_id", "全局城市货物关联ID", "0", "int", 10, 0, false)]
        public virtual Int32 goods_id
        {
            get { return _goods_id; }
            set { if (OnPropertyChanging(__.goods_id, value)) { _goods_id = value; OnPropertyChanged(__.goods_id); } }
        }

        private Int32 _number;
        /// <summary>城市当前货物数量</summary>
        [DisplayName("城市当前货物数量")]
        [Description("城市当前货物数量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "number", "城市当前货物数量", "0", "int", 10, 0, false)]
        public virtual Int32 number
        {
            get { return _number; }
            set { if (OnPropertyChanging(__.number, value)) { _number = value; OnPropertyChanged(__.number); } }
        }

        private Int32 _number_max;
        /// <summary>用户城市货物最大数量</summary>
        [DisplayName("用户城市货物最大数量")]
        [Description("用户城市货物最大数量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "number_max", "用户城市货物最大数量", "0", "int", 10, 0, false)]
        public virtual Int32 number_max
        {
            get { return _number_max; }
            set { if (OnPropertyChanging(__.number_max, value)) { _number_max = value; OnPropertyChanged(__.number_max); } }
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
                    case __.ting_id : return _ting_id;
                    case __.goods_id : return _goods_id;
                    case __.number : return _number;
                    case __.number_max : return _number_max;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.ting_id : _ting_id = Convert.ToInt32(value); break;
                    case __.goods_id : _goods_id = Convert.ToInt32(value); break;
                    case __.number : _number = Convert.ToInt32(value); break;
                    case __.number_max : _number_max = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家町货物表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>町基表编号</summary>
            public static readonly Field ting_id = FindByName(__.ting_id);

            ///<summary>全局城市货物关联ID</summary>
            public static readonly Field goods_id = FindByName(__.goods_id);

            ///<summary>城市当前货物数量</summary>
            public static readonly Field number = FindByName(__.number);

            ///<summary>用户城市货物最大数量</summary>
            public static readonly Field number_max = FindByName(__.number_max);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家町货物表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>町基表编号</summary>
            public const String ting_id = "ting_id";

            ///<summary>全局城市货物关联ID</summary>
            public const String goods_id = "goods_id";

            ///<summary>城市当前货物数量</summary>
            public const String number = "number";

            ///<summary>用户城市货物最大数量</summary>
            public const String number_max = "number_max";

        }
        #endregion
    }

    /// <summary>玩家町货物表接口</summary>
    public partial interface Itg_goods_item
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>町基表编号</summary>
        Int32 ting_id { get; set; }

        /// <summary>全局城市货物关联ID</summary>
        Int32 goods_id { get; set; }

        /// <summary>城市当前货物数量</summary>
        Int32 number { get; set; }

        /// <summary>用户城市货物最大数量</summary>
        Int32 number_max { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}