using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>防沉迷临时表</summary>
    [Serializable]
    [DataObject]
    [Description("防沉迷临时表")]
    [BindTable("tg_user_temp", Description = "防沉迷临时表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_temp : Itg_user_temp
    {
        #region 属性
        private Int32 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private String _user_code;
        /// <summary>玩家登陆账号</summary>
        [DisplayName("玩家登陆账号")]
        [Description("玩家登陆账号")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(2, "user_code", "玩家登陆账号", "0", "nvarchar(200)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
        }

        private Int32 _isAdult;
        /// <summary>是否成年(0:未成年 1:成年)</summary>
        [DisplayName("是否成年0:未成年1:成年")]
        [Description("是否成年(0:未成年 1:成年)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "isAdult", "是否成年(0:未成年 1:成年)", "0", "int", 10, 0, false)]
        public virtual Int32 isAdult
        {
            get { return _isAdult; }
            set { if (OnPropertyChanging(__.isAdult, value)) { _isAdult = value; OnPropertyChanged(__.isAdult); } }
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
                    case __.user_code : return _user_code;
                    case __.isAdult : return _isAdult;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.user_code : _user_code = Convert.ToString(value); break;
                    case __.isAdult : _isAdult = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得防沉迷临时表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家登陆账号</summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary>是否成年(0:未成年 1:成年)</summary>
            public static readonly Field isAdult = FindByName(__.isAdult);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得防沉迷临时表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家登陆账号</summary>
            public const String user_code = "user_code";

            ///<summary>是否成年(0:未成年 1:成年)</summary>
            public const String isAdult = "isAdult";

        }
        #endregion
    }

    /// <summary>防沉迷临时表接口</summary>
    public partial interface Itg_user_temp
    {
        #region 属性
        /// <summary></summary>
        Int32 id { get; set; }

        /// <summary>玩家登陆账号</summary>
        String user_code { get; set; }

        /// <summary>是否成年(0:未成年 1:成年)</summary>
        Int32 isAdult { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}