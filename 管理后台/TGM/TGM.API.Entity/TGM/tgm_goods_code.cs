using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>福利卡激活码表</summary>
    [Serializable]
    [DataObject]
    [Description("福利卡激活码表")]
    [BindTable("tgm_goods_code", Description = "福利卡激活码表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_goods_code : Itgm_goods_code
    {
        #region 属性
        private Int32 _id;
        /// <summary>主键</summary>
        [DisplayName("主键")]
        [Description("主键")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "主键", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private String _card_key;
        /// <summary>激活码</summary>
        [DisplayName("激活码")]
        [Description("激活码")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(2, "card_key", "激活码", "0", "nvarchar(100)", 0, 0, true)]
        public virtual String card_key
        {
            get { return _card_key; }
            set { if (OnPropertyChanging(__.card_key, value)) { _card_key = value; OnPropertyChanged(__.card_key); } }
        }

        private Int32 _type;
        /// <summary>卡类型 1：新手卡 2：媒体卡</summary>
        [DisplayName("卡类型1：新手卡2：媒体卡")]
        [Description("卡类型 1：新手卡 2：媒体卡")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "type", "卡类型 1：新手卡 2：媒体卡", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _pid;
        /// <summary>平台ID</summary>
        [DisplayName("平台ID")]
        [Description("平台ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "pid", "平台ID", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private String _platform_name;
        /// <summary>服务器主键id</summary>
        [DisplayName("服务器主键id")]
        [Description("服务器主键id")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(5, "platform_name", "服务器主键id", "0", "nvarchar(100)", 0, 0, true)]
        public virtual String platform_name
        {
            get { return _platform_name; }
            set { if (OnPropertyChanging(__.platform_name, value)) { _platform_name = value; OnPropertyChanged(__.platform_name); } }
        }

        private String _kind;
        /// <summary>生成批次(默认yyyyMMDDHHII)</summary>
        [DisplayName("生成批次默认yyyyMMDDHHII")]
        [Description("生成批次(默认yyyyMMDDHHII)")]
        [DataObjectField(false, false, false, 20)]
        [BindColumn(6, "kind", "生成批次(默认yyyyMMDDHHII)", "0", "nvarchar(20)", 0, 0, true)]
        public virtual String kind
        {
            get { return _kind; }
            set { if (OnPropertyChanging(__.kind, value)) { _kind = value; OnPropertyChanged(__.kind); } }
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
                    case __.card_key : return _card_key;
                    case __.type : return _type;
                    case __.pid : return _pid;
                    case __.platform_name : return _platform_name;
                    case __.kind : return _kind;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.card_key : _card_key = Convert.ToString(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.pid : _pid = Convert.ToInt32(value); break;
                    case __.platform_name : _platform_name = Convert.ToString(value); break;
                    case __.kind : _kind = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得福利卡激活码表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>激活码</summary>
            public static readonly Field card_key = FindByName(__.card_key);

            ///<summary>卡类型 1：新手卡 2：媒体卡</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>平台ID</summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>服务器主键id</summary>
            public static readonly Field platform_name = FindByName(__.platform_name);

            ///<summary>生成批次(默认yyyyMMDDHHII)</summary>
            public static readonly Field kind = FindByName(__.kind);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得福利卡激活码表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键</summary>
            public const String id = "id";

            ///<summary>激活码</summary>
            public const String card_key = "card_key";

            ///<summary>卡类型 1：新手卡 2：媒体卡</summary>
            public const String type = "type";

            ///<summary>平台ID</summary>
            public const String pid = "pid";

            ///<summary>服务器主键id</summary>
            public const String platform_name = "platform_name";

            ///<summary>生成批次(默认yyyyMMDDHHII)</summary>
            public const String kind = "kind";

        }
        #endregion
    }

    /// <summary>福利卡激活码表接口</summary>
    public partial interface Itgm_goods_code
    {
        #region 属性
        /// <summary>主键</summary>
        Int32 id { get; set; }

        /// <summary>激活码</summary>
        String card_key { get; set; }

        /// <summary>卡类型 1：新手卡 2：媒体卡</summary>
        Int32 type { get; set; }

        /// <summary>平台ID</summary>
        Int32 pid { get; set; }

        /// <summary>服务器主键id</summary>
        String platform_name { get; set; }

        /// <summary>生成批次(默认yyyyMMDDHHII)</summary>
        String kind { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}