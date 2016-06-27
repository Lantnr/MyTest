using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>数据日志表</summary>
    [Serializable]
    [DataObject]
    [Description("数据日志表")]
    [BindTable("tg_log_operate", Description = "数据日志表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_log_operate : Itg_log_operate
    {
        #region 属性
        private Int64 _id;
        /// <summary>ID</summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "ID", null, "bigint", 19, 0, false)]
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

        private Int32 _module_number;
        /// <summary>模块编号</summary>
        [DisplayName("模块编号")]
        [Description("模块编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "module_number", "模块编号", "0", "int", 10, 0, false)]
        public virtual Int32 module_number
        {
            get { return _module_number; }
            set { if (OnPropertyChanging(__.module_number, value)) { _module_number = value; OnPropertyChanged(__.module_number); } }
        }

        private Int32 _command_number;
        /// <summary>指令编号</summary>
        [DisplayName("指令编号")]
        [Description("指令编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "command_number", "指令编号", "0", "int", 10, 0, false)]
        public virtual Int32 command_number
        {
            get { return _command_number; }
            set { if (OnPropertyChanging(__.command_number, value)) { _command_number = value; OnPropertyChanged(__.command_number); } }
        }

        private Int32 _type;
        /// <summary>操作类型</summary>
        [DisplayName("操作类型")]
        [Description("操作类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "type", "操作类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private String _data;
        /// <summary>操作数据</summary>
        [DisplayName("操作数据")]
        [Description("操作数据")]
        [DataObjectField(false, false, true, 1000)]
        [BindColumn(6, "data", "操作数据", null, "nvarchar(1000)", 0, 0, true)]
        public virtual String data
        {
            get { return _data; }
            set { if (OnPropertyChanging(__.data, value)) { _data = value; OnPropertyChanged(__.data); } }
        }

        private DateTime _time;
        /// <summary>操作时间</summary>
        [DisplayName("操作时间")]
        [Description("操作时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "time", "操作时间", "getdate()", "datetime", 3, 0, false)]
        public virtual DateTime time
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
                    case __.user_id : return _user_id;
                    case __.module_number : return _module_number;
                    case __.command_number : return _command_number;
                    case __.type : return _type;
                    case __.data : return _data;
                    case __.time : return _time;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.module_number : _module_number = Convert.ToInt32(value); break;
                    case __.command_number : _command_number = Convert.ToInt32(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.data : _data = Convert.ToString(value); break;
                    case __.time : _time = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得数据日志表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>模块编号</summary>
            public static readonly Field module_number = FindByName(__.module_number);

            ///<summary>指令编号</summary>
            public static readonly Field command_number = FindByName(__.command_number);

            ///<summary>操作类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>操作数据</summary>
            public static readonly Field data = FindByName(__.data);

            ///<summary>操作时间</summary>
            public static readonly Field time = FindByName(__.time);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得数据日志表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>模块编号</summary>
            public const String module_number = "module_number";

            ///<summary>指令编号</summary>
            public const String command_number = "command_number";

            ///<summary>操作类型</summary>
            public const String type = "type";

            ///<summary>操作数据</summary>
            public const String data = "data";

            ///<summary>操作时间</summary>
            public const String time = "time";

        }
        #endregion
    }

    /// <summary>数据日志表接口</summary>
    public partial interface Itg_log_operate
    {
        #region 属性
        /// <summary>ID</summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>模块编号</summary>
        Int32 module_number { get; set; }

        /// <summary>指令编号</summary>
        Int32 command_number { get; set; }

        /// <summary>操作类型</summary>
        Int32 type { get; set; }

        /// <summary>操作数据</summary>
        String data { get; set; }

        /// <summary>操作时间</summary>
        DateTime time { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}