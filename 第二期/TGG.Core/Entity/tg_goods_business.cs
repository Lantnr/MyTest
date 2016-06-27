using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>跑商货物表</summary>
    [Serializable]
    [DataObject]
    [Description("跑商货物表")]
    [BindIndex("PK_tg_goods_business", true, "id")]
    [BindTable("tg_goods_business", Description = "跑商货物表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_goods_business : Itg_goods_business
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

        private Int64 _cid;
        /// <summary>马车表ID</summary>
        [DisplayName("马车表ID")]
        [Description("马车表ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "cid", "马车表ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 cid
        {
            get { return _cid; }
            set { if (OnPropertyChanging(__.cid, value)) { _cid = value; OnPropertyChanged(__.cid); } }
        }

        private Int64 _goods_id;
        /// <summary>货物基表编号</summary>
        [DisplayName("货物基表编号")]
        [Description("货物基表编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "goods_id", "货物基表编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 goods_id
        {
            get { return _goods_id; }
            set { if (OnPropertyChanging(__.goods_id, value)) { _goods_id = value; OnPropertyChanged(__.goods_id); } }
        }

        private Int32 _goods_type;
        /// <summary>货物类型</summary>
        [DisplayName("货物类型")]
        [Description("货物类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "goods_type", "货物类型", "0", "int", 10, 0, false)]
        public virtual Int32 goods_type
        {
            get { return _goods_type; }
            set { if (OnPropertyChanging(__.goods_type, value)) { _goods_type = value; OnPropertyChanged(__.goods_type); } }
        }

        private Int32 _goods_number;
        /// <summary>货物数量</summary>
        [DisplayName("货物数量")]
        [Description("货物数量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "goods_number", "货物数量", "0", "int", 10, 0, false)]
        public virtual Int32 goods_number
        {
            get { return _goods_number; }
            set { if (OnPropertyChanging(__.goods_number, value)) { _goods_number = value; OnPropertyChanged(__.goods_number); } }
        }

        private Int32 _price;
        /// <summary>货物价格</summary>
        [DisplayName("货物价格")]
        [Description("货物价格")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "price", "货物价格", "0", "int", 10, 0, false)]
        public virtual Int32 price
        {
            get { return _price; }
            set { if (OnPropertyChanging(__.price, value)) { _price = value; OnPropertyChanged(__.price); } }
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
                    case __.cid : return _cid;
                    case __.goods_id : return _goods_id;
                    case __.goods_type : return _goods_type;
                    case __.goods_number : return _goods_number;
                    case __.price : return _price;
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
                    case __.cid : _cid = Convert.ToInt64(value); break;
                    case __.goods_id : _goods_id = Convert.ToInt64(value); break;
                    case __.goods_type : _goods_type = Convert.ToInt32(value); break;
                    case __.goods_number : _goods_number = Convert.ToInt32(value); break;
                    case __.price : _price = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得跑商货物表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>町基表编号</summary>
            public static readonly Field ting_id = FindByName(__.ting_id);

            ///<summary>马车表ID</summary>
            public static readonly Field cid = FindByName(__.cid);

            ///<summary>货物基表编号</summary>
            public static readonly Field goods_id = FindByName(__.goods_id);

            ///<summary>货物类型</summary>
            public static readonly Field goods_type = FindByName(__.goods_type);

            ///<summary>货物数量</summary>
            public static readonly Field goods_number = FindByName(__.goods_number);

            ///<summary>货物价格</summary>
            public static readonly Field price = FindByName(__.price);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得跑商货物表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>町基表编号</summary>
            public const String ting_id = "ting_id";

            ///<summary>马车表ID</summary>
            public const String cid = "cid";

            ///<summary>货物基表编号</summary>
            public const String goods_id = "goods_id";

            ///<summary>货物类型</summary>
            public const String goods_type = "goods_type";

            ///<summary>货物数量</summary>
            public const String goods_number = "goods_number";

            ///<summary>货物价格</summary>
            public const String price = "price";

        }
        #endregion
    }

    /// <summary>跑商货物表接口</summary>
    public partial interface Itg_goods_business
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>町基表编号</summary>
        Int32 ting_id { get; set; }

        /// <summary>马车表ID</summary>
        Int64 cid { get; set; }

        /// <summary>货物基表编号</summary>
        Int64 goods_id { get; set; }

        /// <summary>货物类型</summary>
        Int32 goods_type { get; set; }

        /// <summary>货物数量</summary>
        Int32 goods_number { get; set; }

        /// <summary>货物价格</summary>
        Int32 price { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}