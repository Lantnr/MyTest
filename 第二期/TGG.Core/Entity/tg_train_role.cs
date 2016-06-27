using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>武将修行表</summary>
    [Serializable]
    [DataObject]
    [Description("武将修行表")]
    [BindIndex("PK__tg_train__3213E83F69B732BD", true, "id")]
    [BindTable("tg_train_role", Description = "武将修行表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_train_role : Itg_train_role
    {
        #region 属性
        private Int64 _id;
        /// <summary>武将修行编号</summary>
        [DisplayName("武将修行编号")]
        [Description("武将修行编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "武将修行编号", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _rid;
        /// <summary>武将表编号</summary>
        [DisplayName("武将表编号")]
        [Description("武将表编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "rid", "武将表编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _state;
        /// <summary>修行状态</summary>
        [DisplayName("修行状态")]
        [Description("修行状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "state", "修行状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int64 _time;
        /// <summary>修行到达时间</summary>
        [DisplayName("修行到达时间")]
        [Description("修行到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "time", "修行到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _attribute;
        /// <summary>修炼属性</summary>
        [DisplayName("修炼属性")]
        [Description("修炼属性")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "attribute", "修炼属性", "0", "int", 10, 0, false)]
        public virtual Int32 attribute
        {
            get { return _attribute; }
            set { if (OnPropertyChanging(__.attribute, value)) { _attribute = value; OnPropertyChanged(__.attribute); } }
        }

        private Int32 _type;
        /// <summary>修行类型</summary>
        [DisplayName("修行类型")]
        [Description("修行类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "type", "修行类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _count;
        /// <summary>连续修行次数</summary>
        [DisplayName("连续修行次数")]
        [Description("连续修行次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "count", "连续修行次数", "0", "int", 10, 0, false)]
        public virtual Int32 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
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
                    case __.rid : return _rid;
                    case __.state : return _state;
                    case __.time : return _time;
                    case __.attribute : return _attribute;
                    case __.type : return _type;
                    case __.count : return _count;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.attribute : _attribute = Convert.ToInt32(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.count : _count = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得武将修行表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>武将修行编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>武将表编号</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary>修行状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>修行到达时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>修炼属性</summary>
            public static readonly Field attribute = FindByName(__.attribute);

            ///<summary>修行类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>连续修行次数</summary>
            public static readonly Field count = FindByName(__.count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得武将修行表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>武将修行编号</summary>
            public const String id = "id";

            ///<summary>武将表编号</summary>
            public const String rid = "rid";

            ///<summary>修行状态</summary>
            public const String state = "state";

            ///<summary>修行到达时间</summary>
            public const String time = "time";

            ///<summary>修炼属性</summary>
            public const String attribute = "attribute";

            ///<summary>修行类型</summary>
            public const String type = "type";

            ///<summary>连续修行次数</summary>
            public const String count = "count";

        }
        #endregion
    }

    /// <summary>武将修行表接口</summary>
    public partial interface Itg_train_role
    {
        #region 属性
        /// <summary>武将修行编号</summary>
        Int64 id { get; set; }

        /// <summary>武将表编号</summary>
        Int64 rid { get; set; }

        /// <summary>修行状态</summary>
        Int32 state { get; set; }

        /// <summary>修行到达时间</summary>
        Int64 time { get; set; }

        /// <summary>修炼属性</summary>
        Int32 attribute { get; set; }

        /// <summary>修行类型</summary>
        Int32 type { get; set; }

        /// <summary>连续修行次数</summary>
        Int32 count { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}