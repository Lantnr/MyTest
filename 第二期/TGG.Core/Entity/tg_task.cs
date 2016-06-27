using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>任务表</summary>
    [Serializable]
    [DataObject]
    [Description("任务表")]
    [BindIndex("PK__tg_task__3213E83F2E8B7FF7", true, "id")]
    [BindTable("tg_task", Description = "任务表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_task : Itg_task
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

        private Int32 _task_id;
        /// <summary>任务基表编号</summary>
        [DisplayName("任务基表编号")]
        [Description("任务基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "task_id", "任务基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 task_id
        {
            get { return _task_id; }
            set { if (OnPropertyChanging(__.task_id, value)) { _task_id = value; OnPropertyChanged(__.task_id); } }
        }

        private Int32 _task_type;
        /// <summary>任务类型</summary>
        [DisplayName("任务类型")]
        [Description("任务类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "task_type", "任务类型", "0", "int", 10, 0, false)]
        public virtual Int32 task_type
        {
            get { return _task_type; }
            set { if (OnPropertyChanging(__.task_type, value)) { _task_type = value; OnPropertyChanged(__.task_type); } }
        }

        private Int32 _task_state;
        /// <summary>任务完成状态</summary>
        [DisplayName("任务完成状态")]
        [Description("任务完成状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "task_state", "任务完成状态", "0", "int", 10, 0, false)]
        public virtual Int32 task_state
        {
            get { return _task_state; }
            set { if (OnPropertyChanging(__.task_state, value)) { _task_state = value; OnPropertyChanged(__.task_state); } }
        }

        private Int32 _task_step_type;
        /// <summary>任务步骤类型</summary>
        [DisplayName("任务步骤类型")]
        [Description("任务步骤类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "task_step_type", "任务步骤类型", "0", "int", 10, 0, false)]
        public virtual Int32 task_step_type
        {
            get { return _task_step_type; }
            set { if (OnPropertyChanging(__.task_step_type, value)) { _task_step_type = value; OnPropertyChanged(__.task_step_type); } }
        }

        private String _task_step_data;
        /// <summary>任务步骤数据</summary>
        [DisplayName("任务步骤数据")]
        [Description("任务步骤数据")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(7, "task_step_data", "任务步骤数据", null, "nvarchar(MAX)", 0, 0, true)]
        public virtual String task_step_data
        {
            get { return _task_step_data; }
            set { if (OnPropertyChanging(__.task_step_data, value)) { _task_step_data = value; OnPropertyChanged(__.task_step_data); } }
        }

        private Int64 _task_starttime;
        /// <summary>任务开始时间</summary>
        [DisplayName("任务开始时间")]
        [Description("任务开始时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "task_starttime", "任务开始时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 task_starttime
        {
            get { return _task_starttime; }
            set { if (OnPropertyChanging(__.task_starttime, value)) { _task_starttime = value; OnPropertyChanged(__.task_starttime); } }
        }

        private Int64 _task_endtime;
        /// <summary>任务结束时间</summary>
        [DisplayName("任务结束时间")]
        [Description("任务结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "task_endtime", "任务结束时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 task_endtime
        {
            get { return _task_endtime; }
            set { if (OnPropertyChanging(__.task_endtime, value)) { _task_endtime = value; OnPropertyChanged(__.task_endtime); } }
        }

        private Int64 _rid;
        /// <summary>武将主将id</summary>
        [DisplayName("武将主将id")]
        [Description("武将主将id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "rid", "武将主将id", "0", "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _task_base_identify;
        /// <summary>任务刷新出来时的身份</summary>
        [DisplayName("任务刷新出来时的身份")]
        [Description("任务刷新出来时的身份")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "task_base_identify", "任务刷新出来时的身份", "0", "int", 10, 0, false)]
        public virtual Int32 task_base_identify
        {
            get { return _task_base_identify; }
            set { if (OnPropertyChanging(__.task_base_identify, value)) { _task_base_identify = value; OnPropertyChanged(__.task_base_identify); } }
        }

        private Int32 _is_lock;
        /// <summary>是否锁屏</summary>
        [DisplayName("是否锁屏")]
        [Description("是否锁屏")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "is_lock", "是否锁屏", "0", "int", 10, 0, false)]
        public virtual Int32 is_lock
        {
            get { return _is_lock; }
            set { if (OnPropertyChanging(__.is_lock, value)) { _is_lock = value; OnPropertyChanged(__.is_lock); } }
        }

        private Int32 _is_special;
        /// <summary>是否是高级评定任务：0：不是，1:是。</summary>
        [DisplayName("是否是高级评定任务：0：不是，1:是")]
        [Description("是否是高级评定任务：0：不是，1:是。")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "is_special", "是否是高级评定任务：0：不是，1:是。", "0", "int", 10, 0, false)]
        public virtual Int32 is_special
        {
            get { return _is_special; }
            set { if (OnPropertyChanging(__.is_special, value)) { _is_special = value; OnPropertyChanged(__.is_special); } }
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
                    case __.task_id : return _task_id;
                    case __.task_type : return _task_type;
                    case __.task_state : return _task_state;
                    case __.task_step_type : return _task_step_type;
                    case __.task_step_data : return _task_step_data;
                    case __.task_starttime : return _task_starttime;
                    case __.task_endtime : return _task_endtime;
                    case __.rid : return _rid;
                    case __.task_base_identify : return _task_base_identify;
                    case __.is_lock : return _is_lock;
                    case __.is_special : return _is_special;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.task_id : _task_id = Convert.ToInt32(value); break;
                    case __.task_type : _task_type = Convert.ToInt32(value); break;
                    case __.task_state : _task_state = Convert.ToInt32(value); break;
                    case __.task_step_type : _task_step_type = Convert.ToInt32(value); break;
                    case __.task_step_data : _task_step_data = Convert.ToString(value); break;
                    case __.task_starttime : _task_starttime = Convert.ToInt64(value); break;
                    case __.task_endtime : _task_endtime = Convert.ToInt64(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.task_base_identify : _task_base_identify = Convert.ToInt32(value); break;
                    case __.is_lock : _is_lock = Convert.ToInt32(value); break;
                    case __.is_special : _is_special = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得任务表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>任务基表编号</summary>
            public static readonly Field task_id = FindByName(__.task_id);

            ///<summary>任务类型</summary>
            public static readonly Field task_type = FindByName(__.task_type);

            ///<summary>任务完成状态</summary>
            public static readonly Field task_state = FindByName(__.task_state);

            ///<summary>任务步骤类型</summary>
            public static readonly Field task_step_type = FindByName(__.task_step_type);

            ///<summary>任务步骤数据</summary>
            public static readonly Field task_step_data = FindByName(__.task_step_data);

            ///<summary>任务开始时间</summary>
            public static readonly Field task_starttime = FindByName(__.task_starttime);

            ///<summary>任务结束时间</summary>
            public static readonly Field task_endtime = FindByName(__.task_endtime);

            ///<summary>武将主将id</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary>任务刷新出来时的身份</summary>
            public static readonly Field task_base_identify = FindByName(__.task_base_identify);

            ///<summary>是否锁屏</summary>
            public static readonly Field is_lock = FindByName(__.is_lock);

            ///<summary>是否是高级评定任务：0：不是，1:是。</summary>
            public static readonly Field is_special = FindByName(__.is_special);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得任务表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>任务基表编号</summary>
            public const String task_id = "task_id";

            ///<summary>任务类型</summary>
            public const String task_type = "task_type";

            ///<summary>任务完成状态</summary>
            public const String task_state = "task_state";

            ///<summary>任务步骤类型</summary>
            public const String task_step_type = "task_step_type";

            ///<summary>任务步骤数据</summary>
            public const String task_step_data = "task_step_data";

            ///<summary>任务开始时间</summary>
            public const String task_starttime = "task_starttime";

            ///<summary>任务结束时间</summary>
            public const String task_endtime = "task_endtime";

            ///<summary>武将主将id</summary>
            public const String rid = "rid";

            ///<summary>任务刷新出来时的身份</summary>
            public const String task_base_identify = "task_base_identify";

            ///<summary>是否锁屏</summary>
            public const String is_lock = "is_lock";

            ///<summary>是否是高级评定任务：0：不是，1:是。</summary>
            public const String is_special = "is_special";

        }
        #endregion
    }

    /// <summary>任务表接口</summary>
    public partial interface Itg_task
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>任务基表编号</summary>
        Int32 task_id { get; set; }

        /// <summary>任务类型</summary>
        Int32 task_type { get; set; }

        /// <summary>任务完成状态</summary>
        Int32 task_state { get; set; }

        /// <summary>任务步骤类型</summary>
        Int32 task_step_type { get; set; }

        /// <summary>任务步骤数据</summary>
        String task_step_data { get; set; }

        /// <summary>任务开始时间</summary>
        Int64 task_starttime { get; set; }

        /// <summary>任务结束时间</summary>
        Int64 task_endtime { get; set; }

        /// <summary>武将主将id</summary>
        Int64 rid { get; set; }

        /// <summary>任务刷新出来时的身份</summary>
        Int32 task_base_identify { get; set; }

        /// <summary>是否锁屏</summary>
        Int32 is_lock { get; set; }

        /// <summary>是否是高级评定任务：0：不是，1:是。</summary>
        Int32 is_special { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}