using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>后台平台表</summary>
    [DataObject]
    [Description("后台平台表")]
    [BindIndex("PK__tgm_plat__3213E83F2B5F6B28", true, "id")]
    [BindTable("tgm_platform", Description = "后台平台表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_platform : Itgm_platform
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

        private Guid _token;
        /// <summary>令牌</summary>
        [DisplayName("令牌")]
        [Description("令牌")]
        [DataObjectField(false, false, false, 16)]
        [BindColumn(2, "token", "令牌", "newid()", "uniqueidentifier", 0, 0, false)]
        public virtual Guid token
        {
            get { return _token; }
            set { if (OnPropertyChanging(__.token, value)) { _token = value; OnPropertyChanged(__.token); } }
        }

        private String _name;
        /// <summary>平台名称</summary>
        [DisplayName("平台名称")]
        [Description("平台名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(3, "name", "平台名称", null, "nvarchar(100)", 0, 0, true)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
        }

        private String _encrypt;
        /// <summary>加密字符串</summary>
        [DisplayName("加密字符串")]
        [Description("加密字符串")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(5, "encrypt", "加密字符串", "123456", "nvarchar(100)", 0, 0, true)]
        public virtual String encrypt
        {
            get { return _encrypt; }
            set { if (OnPropertyChanging(__.encrypt, value)) { _encrypt = value; OnPropertyChanged(__.encrypt); } }
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
                    case __.token : return _token;
                    case __.name : return _name;
                    case __.createtime : return _createtime;
                    case __.encrypt : return _encrypt;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.token : _token = (Guid)value; break;
                    case __.name : _name = Convert.ToString(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    case __.encrypt : _encrypt = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得后台平台表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>令牌</summary>
            public static readonly Field token = FindByName(__.token);

            ///<summary>平台名称</summary>
            public static readonly Field name = FindByName(__.name);

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            ///<summary>加密字符串</summary>
            public static readonly Field encrypt = FindByName(__.encrypt);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得后台平台表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>令牌</summary>
            public const String token = "token";

            ///<summary>平台名称</summary>
            public const String name = "name";

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

            ///<summary>加密字符串</summary>
            public const String encrypt = "encrypt";

        }
        #endregion
    }

    /// <summary>后台平台表接口</summary>
    public partial interface Itgm_platform
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 id { get; set; }

        /// <summary>令牌</summary>
        Guid token { get; set; }

        /// <summary>平台名称</summary>
        String name { get; set; }

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }

        /// <summary>加密字符串</summary>
        String encrypt { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}