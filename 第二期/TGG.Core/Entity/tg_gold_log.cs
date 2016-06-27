using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Gold_Log</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindIndex("PK_tg_gold_log", true, "id")]
    [BindTable("tg_gold_log", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_gold_log : Itg_gold_log
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键Id</summary>
        [DisplayName("主键Id")]
        [Description("主键Id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键Id", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _consume;
        /// <summary>消费</summary>
        [DisplayName("消费")]
        [Description("消费")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "consume", "消费", "0", "int", 10, 0, false)]
        public virtual Int32 consume
        {
            get { return _consume; }
            set { if (OnPropertyChanging(__.consume, value)) { _consume = value; OnPropertyChanged(__.consume); } }
        }

        private Int32 _module_number;
        /// <summary>模块号</summary>
        [DisplayName("模块号")]
        [Description("模块号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "module_number", "模块号", "0", "int", 10, 0, false)]
        public virtual Int32 module_number
        {
            get { return _module_number; }
            set { if (OnPropertyChanging(__.module_number, value)) { _module_number = value; OnPropertyChanged(__.module_number); } }
        }

        private Int32 _command_number;
        /// <summary>指令号</summary>
        [DisplayName("指令号")]
        [Description("指令号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "command_number", "指令号", "0", "int", 10, 0, false)]
        public virtual Int32 command_number
        {
            get { return _command_number; }
            set { if (OnPropertyChanging(__.command_number, value)) { _command_number = value; OnPropertyChanged(__.command_number); } }
        }

        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "user_id", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int64 _time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "time", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
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
                    case __.consume : return _consume;
                    case __.module_number : return _module_number;
                    case __.command_number : return _command_number;
                    case __.user_id : return _user_id;
                    case __.time : return _time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.consume : _consume = Convert.ToInt32(value); break;
                    case __.module_number : _module_number = Convert.ToInt32(value); break;
                    case __.command_number : _command_number = Convert.ToInt32(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Gold_Log字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键Id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>消费</summary>
            public static readonly Field consume = FindByName(__.consume);

            ///<summary>模块号</summary>
            public static readonly Field module_number = FindByName(__.module_number);

            ///<summary>指令号</summary>
            public static readonly Field command_number = FindByName(__.command_number);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field time = FindByName(__.time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Gold_Log字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键Id</summary>
            public const String id = "id";

            ///<summary>消费</summary>
            public const String consume = "consume";

            ///<summary>模块号</summary>
            public const String module_number = "module_number";

            ///<summary>指令号</summary>
            public const String command_number = "command_number";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String time = "time";

        }
        #endregion
    }

    /// <summary>Gold_Log接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_gold_log
    {
        #region 属性
        /// <summary>主键Id</summary>
        Int64 id { get; set; }

        /// <summary>消费</summary>
        Int32 consume { get; set; }

        /// <summary>模块号</summary>
        Int32 module_number { get; set; }

        /// <summary>指令号</summary>
        Int32 command_number { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        Int64 time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}