using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>马车表</summary>
    [DataObject]
    [Description("马车表")]
    [BindIndex("PK__tg_car__3213E83F4B0D20AB", true, "id")]
    [BindTable("tg_car", Description = "马车表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_car : Itg_car
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

        private Int64 _rid;
        /// <summary>武将表ID</summary>
        [DisplayName("武将表ID")]
        [Description("武将表ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "rid", "武将表ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _car_id;
        /// <summary>马车基表编号</summary>
        [DisplayName("马车基表编号")]
        [Description("马车基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "car_id", "马车基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 car_id
        {
            get { return _car_id; }
            set { if (OnPropertyChanging(__.car_id, value)) { _car_id = value; OnPropertyChanged(__.car_id); } }
        }

        private Int32 _speed;
        /// <summary>马车速度</summary>
        [DisplayName("马车速度")]
        [Description("马车速度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "speed", "马车速度", "0", "int", 10, 0, false)]
        public virtual Int32 speed
        {
            get { return _speed; }
            set { if (OnPropertyChanging(__.speed, value)) { _speed = value; OnPropertyChanged(__.speed); } }
        }

        private Int32 _packet;
        /// <summary>马车格子数</summary>
        [DisplayName("马车格子数")]
        [Description("马车格子数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "packet", "马车格子数", "0", "int", 10, 0, false)]
        public virtual Int32 packet
        {
            get { return _packet; }
            set { if (OnPropertyChanging(__.packet, value)) { _packet = value; OnPropertyChanged(__.packet); } }
        }

        private Int32 _state;
        /// <summary>马车状态</summary>
        [DisplayName("马车状态")]
        [Description("马车状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "state", "马车状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _start_ting_id;
        /// <summary>开始町ID</summary>
        [DisplayName("开始町ID")]
        [Description("开始町ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "start_ting_id", "开始町ID", "0", "int", 10, 0, false)]
        public virtual Int32 start_ting_id
        {
            get { return _start_ting_id; }
            set { if (OnPropertyChanging(__.start_ting_id, value)) { _start_ting_id = value; OnPropertyChanged(__.start_ting_id); } }
        }

        private Int32 _stop_ting_id;
        /// <summary>停止町ID</summary>
        [DisplayName("停止町ID")]
        [Description("停止町ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "stop_ting_id", "停止町ID", "0", "int", 10, 0, false)]
        public virtual Int32 stop_ting_id
        {
            get { return _stop_ting_id; }
            set { if (OnPropertyChanging(__.stop_ting_id, value)) { _stop_ting_id = value; OnPropertyChanged(__.stop_ting_id); } }
        }

        private Int64 _time;
        /// <summary>跑商时间</summary>
        [DisplayName("跑商时间")]
        [Description("跑商时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "time", "跑商时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _distance;
        /// <summary>跑商距离</summary>
        [DisplayName("跑商距离")]
        [Description("跑商距离")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "distance", "跑商距离", "0", "int", 10, 0, false)]
        public virtual Int32 distance
        {
            get { return _distance; }
            set { if (OnPropertyChanging(__.distance, value)) { _distance = value; OnPropertyChanged(__.distance); } }
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
                    case __.rid : return _rid;
                    case __.car_id : return _car_id;
                    case __.speed : return _speed;
                    case __.packet : return _packet;
                    case __.state : return _state;
                    case __.start_ting_id : return _start_ting_id;
                    case __.stop_ting_id : return _stop_ting_id;
                    case __.time : return _time;
                    case __.distance : return _distance;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.car_id : _car_id = Convert.ToInt32(value); break;
                    case __.speed : _speed = Convert.ToInt32(value); break;
                    case __.packet : _packet = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.start_ting_id : _start_ting_id = Convert.ToInt32(value); break;
                    case __.stop_ting_id : _stop_ting_id = Convert.ToInt32(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.distance : _distance = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得马车表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>武将表ID</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary>马车基表编号</summary>
            public static readonly Field car_id = FindByName(__.car_id);

            ///<summary>马车速度</summary>
            public static readonly Field speed = FindByName(__.speed);

            ///<summary>马车格子数</summary>
            public static readonly Field packet = FindByName(__.packet);

            ///<summary>马车状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>开始町ID</summary>
            public static readonly Field start_ting_id = FindByName(__.start_ting_id);

            ///<summary>停止町ID</summary>
            public static readonly Field stop_ting_id = FindByName(__.stop_ting_id);

            ///<summary>跑商时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>跑商距离</summary>
            public static readonly Field distance = FindByName(__.distance);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得马车表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>武将表ID</summary>
            public const String rid = "rid";

            ///<summary>马车基表编号</summary>
            public const String car_id = "car_id";

            ///<summary>马车速度</summary>
            public const String speed = "speed";

            ///<summary>马车格子数</summary>
            public const String packet = "packet";

            ///<summary>马车状态</summary>
            public const String state = "state";

            ///<summary>开始町ID</summary>
            public const String start_ting_id = "start_ting_id";

            ///<summary>停止町ID</summary>
            public const String stop_ting_id = "stop_ting_id";

            ///<summary>跑商时间</summary>
            public const String time = "time";

            ///<summary>跑商距离</summary>
            public const String distance = "distance";

        }
        #endregion
    }

    /// <summary>马车表接口</summary>
    public partial interface Itg_car
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>武将表ID</summary>
        Int64 rid { get; set; }

        /// <summary>马车基表编号</summary>
        Int32 car_id { get; set; }

        /// <summary>马车速度</summary>
        Int32 speed { get; set; }

        /// <summary>马车格子数</summary>
        Int32 packet { get; set; }

        /// <summary>马车状态</summary>
        Int32 state { get; set; }

        /// <summary>开始町ID</summary>
        Int32 start_ting_id { get; set; }

        /// <summary>停止町ID</summary>
        Int32 stop_ting_id { get; set; }

        /// <summary>跑商时间</summary>
        Int64 time { get; set; }

        /// <summary>跑商距离</summary>
        Int32 distance { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}