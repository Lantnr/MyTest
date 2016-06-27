using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>System_Notice</summary>
    /// <remarks></remarks>
    [DataObject]
    [Description("")]
    [BindIndex("PK__tg_syste__3213E83F1E8F7FEF", true, "id")]
    [BindTable("tg_system_notice", Description = "", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_system_notice : Itg_system_notice
    {
        #region 属性
        private Int32 _id;
        /// <summary>主键Id</summary>
        [DisplayName("主键Id")]
        [Description("主键Id")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "主键Id", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _base_Id;
        /// <summary>基表Id</summary>
        [DisplayName("基表Id")]
        [Description("基表Id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "base_Id", "基表Id", null, "int", 10, 0, false)]
        public virtual Int32 base_Id
        {
            get { return _base_Id; }
            set { if (OnPropertyChanging(__.base_Id, value)) { _base_Id = value; OnPropertyChanged(__.base_Id); } }
        }

        private Int64 _start_time;
        /// <summary>开始时间</summary>
        [DisplayName("开始时间")]
        [Description("开始时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "start_time", "开始时间", null, "bigint", 19, 0, false)]
        public virtual Int64 start_time
        {
            get { return _start_time; }
            set { if (OnPropertyChanging(__.start_time, value)) { _start_time = value; OnPropertyChanged(__.start_time); } }
        }

        private Int64 _end_time;
        /// <summary>结束时间</summary>
        [DisplayName("结束时间")]
        [Description("结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "end_time", "结束时间", null, "bigint", 19, 0, false)]
        public virtual Int64 end_time
        {
            get { return _end_time; }
            set { if (OnPropertyChanging(__.end_time, value)) { _end_time = value; OnPropertyChanged(__.end_time); } }
        }

        private Int32 _time_interval;
        /// <summary>时间间隔</summary>
        [DisplayName("时间间隔")]
        [Description("时间间隔")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "time_interval", "时间间隔", null, "int", 10, 0, false)]
        public virtual Int32 time_interval
        {
            get { return _time_interval; }
            set { if (OnPropertyChanging(__.time_interval, value)) { _time_interval = value; OnPropertyChanged(__.time_interval); } }
        }

        private String _content;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(6, "content", "内容", null, "varchar(500)", 0, 0, false)]
        public virtual String content
        {
            get { return _content; }
            set { if (OnPropertyChanging(__.content, value)) { _content = value; OnPropertyChanged(__.content); } }
        }

        private Int32 _state;
        /// <summary>状态</summary>
        [DisplayName("状态")]
        [Description("状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "state", "状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _level;
        /// <summary>优先级别</summary>
        [DisplayName("优先级别")]
        [Description("优先级别")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "level", "优先级别", null, "int", 10, 0, false)]
        public virtual Int32 level
        {
            get { return _level; }
            set { if (OnPropertyChanging(__.level, value)) { _level = value; OnPropertyChanged(__.level); } }
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
                    case __.base_Id : return _base_Id;
                    case __.start_time : return _start_time;
                    case __.end_time : return _end_time;
                    case __.time_interval : return _time_interval;
                    case __.content : return _content;
                    case __.state : return _state;
                    case __.level : return _level;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt32(value); break;
                    case __.base_Id : _base_Id = Convert.ToInt32(value); break;
                    case __.start_time : _start_time = Convert.ToInt64(value); break;
                    case __.end_time : _end_time = Convert.ToInt64(value); break;
                    case __.time_interval : _time_interval = Convert.ToInt32(value); break;
                    case __.content : _content = Convert.ToString(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.level : _level = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得System_Notice字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键Id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>基表Id</summary>
            public static readonly Field base_Id = FindByName(__.base_Id);

            ///<summary>开始时间</summary>
            public static readonly Field start_time = FindByName(__.start_time);

            ///<summary>结束时间</summary>
            public static readonly Field end_time = FindByName(__.end_time);

            ///<summary>时间间隔</summary>
            public static readonly Field time_interval = FindByName(__.time_interval);

            ///<summary>内容</summary>
            public static readonly Field content = FindByName(__.content);

            ///<summary>状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>优先级别</summary>
            public static readonly Field level = FindByName(__.level);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得System_Notice字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键Id</summary>
            public const String id = "id";

            ///<summary>基表Id</summary>
            public const String base_Id = "base_Id";

            ///<summary>开始时间</summary>
            public const String start_time = "start_time";

            ///<summary>结束时间</summary>
            public const String end_time = "end_time";

            ///<summary>时间间隔</summary>
            public const String time_interval = "time_interval";

            ///<summary>内容</summary>
            public const String content = "content";

            ///<summary>状态</summary>
            public const String state = "state";

            ///<summary>优先级别</summary>
            public const String level = "level";

        }
        #endregion
    }

    /// <summary>System_Notice接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_system_notice
    {
        #region 属性
        /// <summary>主键Id</summary>
        Int32 id { get; set; }

        /// <summary>基表Id</summary>
        Int32 base_Id { get; set; }

        /// <summary>开始时间</summary>
        Int64 start_time { get; set; }

        /// <summary>结束时间</summary>
        Int64 end_time { get; set; }

        /// <summary>时间间隔</summary>
        Int32 time_interval { get; set; }

        /// <summary>内容</summary>
        String content { get; set; }

        /// <summary>状态</summary>
        Int32 state { get; set; }

        /// <summary>优先级别</summary>
        Int32 level { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}