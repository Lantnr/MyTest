using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>游戏卡领取表</summary>
    [DataObject]
    [Description("游戏卡领取表")]
    [BindIndex("PK_tg_card_receive", true, "id")]
    [BindTable("tg_goods", Description = "游戏卡领取表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_goods : Itg_goods
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
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private String _card_key;
        /// <summary></summary>
        [DisplayName("Key")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "card_key", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String card_key
        {
            get { return _card_key; }
            set { if (OnPropertyChanging(__.card_key, value)) { _card_key = value; OnPropertyChanged(__.card_key); } }
        }

        private Int32 _type;
        /// <summary>卡类型  1：新手卡  2：媒体卡</summary>
        [DisplayName("卡类型1：新手卡2：媒体卡")]
        [Description("卡类型  1：新手卡  2：媒体卡")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "type", "卡类型  1：新手卡  2：媒体卡", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _state;
        /// <summary>领取状态  0：未领取  1：已领取</summary>
        [DisplayName("领取状态0：未领取1：已领取")]
        [Description("领取状态  0：未领取  1：已领取")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "state", "领取状态  0：未领取  1：已领取", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
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
                    case __.card_key : return _card_key;
                    case __.type : return _type;
                    case __.state : return _state;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.card_key : _card_key = Convert.ToString(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得游戏卡领取表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field card_key = FindByName(__.card_key);

            ///<summary>卡类型  1：新手卡  2：媒体卡</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>领取状态  0：未领取  1：已领取</summary>
            public static readonly Field state = FindByName(__.state);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得游戏卡领取表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String card_key = "card_key";

            ///<summary>卡类型  1：新手卡  2：媒体卡</summary>
            public const String type = "type";

            ///<summary>领取状态  0：未领取  1：已领取</summary>
            public const String state = "state";

        }
        #endregion
    }

    /// <summary>游戏卡领取表接口</summary>
    public partial interface Itg_goods
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        String card_key { get; set; }

        /// <summary>卡类型  1：新手卡  2：媒体卡</summary>
        Int32 type { get; set; }

        /// <summary>领取状态  0：未领取  1：已领取</summary>
        Int32 state { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}