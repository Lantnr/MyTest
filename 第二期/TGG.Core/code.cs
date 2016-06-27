using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>激活码表</summary>
    [Serializable]
    [DataObject]
    [Description("激活码表")]
    [BindTable("code", Description = "激活码表", ConnName = "ACT", DbType = DatabaseType.SqlServer)]
    public partial class code : Icode
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

        private Int32 _state;
        /// <summary></summary>
        [DisplayName("State")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "state", "", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private String _user_code;
        /// <summary></summary>
        [DisplayName("Code")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "user_code", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
        }

        private String _user_key;
        /// <summary></summary>
        [DisplayName("Key")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "user_key", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String user_key
        {
            get { return _user_key; }
            set { if (OnPropertyChanging(__.user_key, value)) { _user_key = value; OnPropertyChanged(__.user_key); } }
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
                    case __.state : return _state;
                    case __.user_code : return _user_code;
                    case __.user_key : return _user_key;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.user_code : _user_code = Convert.ToString(value); break;
                    case __.user_key : _user_key = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得激活码表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary></summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary></summary>
            public static readonly Field user_key = FindByName(__.user_key);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得激活码表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String state = "state";

            ///<summary></summary>
            public const String user_code = "user_code";

            ///<summary></summary>
            public const String user_key = "user_key";

        }
        #endregion
    }

    /// <summary>激活码表接口</summary>
    public partial interface Icode
    {
        #region 属性
        /// <summary></summary>
        Int32 id { get; set; }

        /// <summary></summary>
        Int32 state { get; set; }

        /// <summary></summary>
        String user_code { get; set; }

        /// <summary></summary>
        String user_key { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}