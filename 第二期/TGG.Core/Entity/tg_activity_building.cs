using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Activity_Building</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindIndex("PK_tg_activity_building", true, "id")]
    [BindTable("tg_activity_building", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_activity_building : Itg_activity_building
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键id", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _wood;
        /// <summary>木材</summary>
        [DisplayName("木材")]
        [Description("木材")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "wood", "木材", "0", "int", 10, 0, false)]
        public virtual Int32 wood
        {
            get { return _wood; }
            set { if (OnPropertyChanging(__.wood, value)) { _wood = value; OnPropertyChanged(__.wood); } }
        }

        private Int32 _makebuild;
        /// <summary>建材</summary>
        [DisplayName("建材")]
        [Description("建材")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "makebuild", "建材", "0", "int", 10, 0, false)]
        public virtual Int32 makebuild
        {
            get { return _makebuild; }
            set { if (OnPropertyChanging(__.makebuild, value)) { _makebuild = value; OnPropertyChanged(__.makebuild); } }
        }

        private Int32 _torch;
        /// <summary>火把</summary>
        [DisplayName("火把")]
        [Description("火把")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "torch", "火把", "0", "int", 10, 0, false)]
        public virtual Int32 torch
        {
            get { return _torch; }
            set { if (OnPropertyChanging(__.torch, value)) { _torch = value; OnPropertyChanged(__.torch); } }
        }

        private Int32 _fame;
        /// <summary>活动声望</summary>
        [DisplayName("活动声望")]
        [Description("活动声望")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "fame", "活动声望", "0", "int", 10, 0, false)]
        public virtual Int32 fame
        {
            get { return _fame; }
            set { if (OnPropertyChanging(__.fame, value)) { _fame = value; OnPropertyChanged(__.fame); } }
        }

        private Int32 _wintype;
        /// <summary>胜利类型：1.东军胜 2.西军胜 3.平局</summary>
        [DisplayName("胜利类型：1")]
        [Description("胜利类型：1.东军胜 2.西军胜 3.平局")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "wintype", "胜利类型：1.东军胜 2.西军胜 3.平局", "0", "int", 10, 0, false)]
        public virtual Int32 wintype
        {
            get { return _wintype; }
            set { if (OnPropertyChanging(__.wintype, value)) { _wintype = value; OnPropertyChanged(__.wintype); } }
        }

        private Int32 _team_money;
        /// <summary>获取的团队金钱</summary>
        [DisplayName("获取的团队金钱")]
        [Description("获取的团队金钱")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "team_money", "获取的团队金钱", "0", "int", 10, 0, false)]
        public virtual Int32 team_money
        {
            get { return _team_money; }
            set { if (OnPropertyChanging(__.team_money, value)) { _team_money = value; OnPropertyChanged(__.team_money); } }
        }

        private Int32 _team_fame;
        /// <summary>获取的团队声望</summary>
        [DisplayName("获取的团队声望")]
        [Description("获取的团队声望")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "team_fame", "获取的团队声望", "0", "int", 10, 0, false)]
        public virtual Int32 team_fame
        {
            get { return _team_fame; }
            set { if (OnPropertyChanging(__.team_fame, value)) { _team_fame = value; OnPropertyChanged(__.team_fame); } }
        }

        private Int64 _endtime;
        /// <summary>结束时间</summary>
        [DisplayName("结束时间")]
        [Description("结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(9, "endtime", "结束时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 endtime
        {
            get { return _endtime; }
            set { if (OnPropertyChanging(__.endtime, value)) { _endtime = value; OnPropertyChanged(__.endtime); } }
        }

        private Int64 _user_id;
        /// <summary>用户id</summary>
        [DisplayName("用户id")]
        [Description("用户id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "user_id", "用户id", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
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
                    case __.wood : return _wood;
                    case __.makebuild : return _makebuild;
                    case __.torch : return _torch;
                    case __.fame : return _fame;
                    case __.wintype : return _wintype;
                    case __.team_money : return _team_money;
                    case __.team_fame : return _team_fame;
                    case __.endtime : return _endtime;
                    case __.user_id : return _user_id;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.wood : _wood = Convert.ToInt32(value); break;
                    case __.makebuild : _makebuild = Convert.ToInt32(value); break;
                    case __.torch : _torch = Convert.ToInt32(value); break;
                    case __.fame : _fame = Convert.ToInt32(value); break;
                    case __.wintype : _wintype = Convert.ToInt32(value); break;
                    case __.team_money : _team_money = Convert.ToInt32(value); break;
                    case __.team_fame : _team_fame = Convert.ToInt32(value); break;
                    case __.endtime : _endtime = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Activity_Building字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>木材</summary>
            public static readonly Field wood = FindByName(__.wood);

            ///<summary>建材</summary>
            public static readonly Field makebuild = FindByName(__.makebuild);

            ///<summary>火把</summary>
            public static readonly Field torch = FindByName(__.torch);

            ///<summary>活动声望</summary>
            public static readonly Field fame = FindByName(__.fame);

            ///<summary>胜利类型：1.东军胜 2.西军胜 3.平局</summary>
            public static readonly Field wintype = FindByName(__.wintype);

            ///<summary>获取的团队金钱</summary>
            public static readonly Field team_money = FindByName(__.team_money);

            ///<summary>获取的团队声望</summary>
            public static readonly Field team_fame = FindByName(__.team_fame);

            ///<summary>结束时间</summary>
            public static readonly Field endtime = FindByName(__.endtime);

            ///<summary>用户id</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Activity_Building字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>木材</summary>
            public const String wood = "wood";

            ///<summary>建材</summary>
            public const String makebuild = "makebuild";

            ///<summary>火把</summary>
            public const String torch = "torch";

            ///<summary>活动声望</summary>
            public const String fame = "fame";

            ///<summary>胜利类型：1.东军胜 2.西军胜 3.平局</summary>
            public const String wintype = "wintype";

            ///<summary>获取的团队金钱</summary>
            public const String team_money = "team_money";

            ///<summary>获取的团队声望</summary>
            public const String team_fame = "team_fame";

            ///<summary>结束时间</summary>
            public const String endtime = "endtime";

            ///<summary>用户id</summary>
            public const String user_id = "user_id";

        }
        #endregion
    }

    /// <summary>Activity_Building接口</summary>
    /// <remarks></remarks>
    public partial interface Itg_activity_building
    {
        #region 属性
        /// <summary>主键id</summary>
        Int64 id { get; set; }

        /// <summary>木材</summary>
        Int32 wood { get; set; }

        /// <summary>建材</summary>
        Int32 makebuild { get; set; }

        /// <summary>火把</summary>
        Int32 torch { get; set; }

        /// <summary>活动声望</summary>
        Int32 fame { get; set; }

        /// <summary>胜利类型：1.东军胜 2.西军胜 3.平局</summary>
        Int32 wintype { get; set; }

        /// <summary>获取的团队金钱</summary>
        Int32 team_money { get; set; }

        /// <summary>获取的团队声望</summary>
        Int32 team_fame { get; set; }

        /// <summary>结束时间</summary>
        Int64 endtime { get; set; }

        /// <summary>用户id</summary>
        Int64 user_id { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}