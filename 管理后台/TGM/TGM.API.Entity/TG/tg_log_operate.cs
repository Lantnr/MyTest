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
    [BindTable("tg_log_operate", Description = "数据日志表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
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

        private String _module_name;
        /// <summary>模块名称</summary>
        [DisplayName("模块名称")]
        [Description("模块名称")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "module_name", "模块名称", null, "nchar(10)", 0, 0, true)]
        public virtual String module_name
        {
            get { return _module_name; }
            set { if (OnPropertyChanging(__.module_name, value)) { _module_name = value; OnPropertyChanged(__.module_name); } }
        }

        private Int32 _command_number;
        /// <summary>指令编号</summary>
        [DisplayName("指令编号")]
        [Description("指令编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "command_number", "指令编号", "0", "int", 10, 0, false)]
        public virtual Int32 command_number
        {
            get { return _command_number; }
            set { if (OnPropertyChanging(__.command_number, value)) { _command_number = value; OnPropertyChanged(__.command_number); } }
        }

        private String _command_name;
        /// <summary>指令名称</summary>
        [DisplayName("指令名称")]
        [Description("指令名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(6, "command_name", "指令名称", null, "nchar(50)", 0, 0, true)]
        public virtual String command_name
        {
            get { return _command_name; }
            set { if (OnPropertyChanging(__.command_name, value)) { _command_name = value; OnPropertyChanged(__.command_name); } }
        }

        private Int32 _type;
        /// <summary>操作类型</summary>
        [DisplayName("操作类型")]
        [Description("操作类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "type", "操作类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private String _resource_name;
        /// <summary>资源名称</summary>
        [DisplayName("资源名称")]
        [Description("资源名称")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "resource_name", "资源名称", null, "nchar(10)", 0, 0, true)]
        public virtual String resource_name
        {
            get { return _resource_name; }
            set { if (OnPropertyChanging(__.resource_name, value)) { _resource_name = value; OnPropertyChanged(__.resource_name); } }
        }

        private Int32 _resource_type;
        /// <summary>资源类型</summary>
        [DisplayName("资源类型")]
        [Description("资源类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "resource_type", "资源类型", "0", "int", 10, 0, false)]
        public virtual Int32 resource_type
        {
            get { return _resource_type; }
            set { if (OnPropertyChanging(__.resource_type, value)) { _resource_type = value; OnPropertyChanged(__.resource_type); } }
        }

        private Int64 _count;
        /// <summary>增加或减少的资源数量</summary>
        [DisplayName("增加或减少的资源数量")]
        [Description("增加或减少的资源数量")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "count", "增加或减少的资源数量", "0", "bigint", 19, 0, false)]
        public virtual Int64 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
        }

        private Int64 _surplus;
        /// <summary>资源剩余数量</summary>
        [DisplayName("资源剩余数量")]
        [Description("资源剩余数量")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "surplus", "资源剩余数量", "0", "bigint", 19, 0, false)]
        public virtual Int64 surplus
        {
            get { return _surplus; }
            set { if (OnPropertyChanging(__.surplus, value)) { _surplus = value; OnPropertyChanged(__.surplus); } }
        }

        private String _data;
        /// <summary>操作数据</summary>
        [DisplayName("操作数据")]
        [Description("操作数据")]
        [DataObjectField(false, false, true, 1000)]
        [BindColumn(12, "data", "操作数据", null, "nvarchar(1000)", 0, 0, true)]
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
        [BindColumn(13, "time", "操作时间", "getdate()", "datetime", 3, 0, false)]
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
                    case __.module_name : return _module_name;
                    case __.command_number : return _command_number;
                    case __.command_name : return _command_name;
                    case __.type : return _type;
                    case __.resource_name : return _resource_name;
                    case __.resource_type : return _resource_type;
                    case __.count : return _count;
                    case __.surplus : return _surplus;
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
                    case __.module_name : _module_name = Convert.ToString(value); break;
                    case __.command_number : _command_number = Convert.ToInt32(value); break;
                    case __.command_name : _command_name = Convert.ToString(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.resource_name : _resource_name = Convert.ToString(value); break;
                    case __.resource_type : _resource_type = Convert.ToInt32(value); break;
                    case __.count : _count = Convert.ToInt64(value); break;
                    case __.surplus : _surplus = Convert.ToInt64(value); break;
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

            ///<summary>模块名称</summary>
            public static readonly Field module_name = FindByName(__.module_name);

            ///<summary>指令编号</summary>
            public static readonly Field command_number = FindByName(__.command_number);

            ///<summary>指令名称</summary>
            public static readonly Field command_name = FindByName(__.command_name);

            ///<summary>操作类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>资源名称</summary>
            public static readonly Field resource_name = FindByName(__.resource_name);

            ///<summary>资源类型</summary>
            public static readonly Field resource_type = FindByName(__.resource_type);

            ///<summary>增加或减少的资源数量</summary>
            public static readonly Field count = FindByName(__.count);

            ///<summary>资源剩余数量</summary>
            public static readonly Field surplus = FindByName(__.surplus);

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

            ///<summary>模块名称</summary>
            public const String module_name = "module_name";

            ///<summary>指令编号</summary>
            public const String command_number = "command_number";

            ///<summary>指令名称</summary>
            public const String command_name = "command_name";

            ///<summary>操作类型</summary>
            public const String type = "type";

            ///<summary>资源名称</summary>
            public const String resource_name = "resource_name";

            ///<summary>资源类型</summary>
            public const String resource_type = "resource_type";

            ///<summary>增加或减少的资源数量</summary>
            public const String count = "count";

            ///<summary>资源剩余数量</summary>
            public const String surplus = "surplus";

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

        /// <summary>模块名称</summary>
        String module_name { get; set; }

        /// <summary>指令编号</summary>
        Int32 command_number { get; set; }

        /// <summary>指令名称</summary>
        String command_name { get; set; }

        /// <summary>操作类型</summary>
        Int32 type { get; set; }

        /// <summary>资源名称</summary>
        String resource_name { get; set; }

        /// <summary>资源类型</summary>
        Int32 resource_type { get; set; }

        /// <summary>增加或减少的资源数量</summary>
        Int64 count { get; set; }

        /// <summary>资源剩余数量</summary>
        Int64 surplus { get; set; }

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