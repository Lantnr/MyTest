using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>系统公告表</summary>
    [Serializable]
    [DataObject]
    [Description("系统公告表")]
    [BindTable("tgm_notice", Description = "系统公告表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_notice : Itgm_notice
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

        private Int64 _pid;
        /// <summary></summary>
        [DisplayName("Pid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "pid", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private Int64 _sid;
        /// <summary></summary>
        [DisplayName("Sid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "sid", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private Int64 _player_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "player_id", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 player_id
        {
            get { return _player_id; }
            set { if (OnPropertyChanging(__.player_id, value)) { _player_id = value; OnPropertyChanged(__.player_id); } }
        }

        private Int64 _start_time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "start_time", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 start_time
        {
            get { return _start_time; }
            set { if (OnPropertyChanging(__.start_time, value)) { _start_time = value; OnPropertyChanged(__.start_time); } }
        }

        private Int64 _end_time;
        /// <summary></summary>
        [DisplayName("Time1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "end_time", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 end_time
        {
            get { return _end_time; }
            set { if (OnPropertyChanging(__.end_time, value)) { _end_time = value; OnPropertyChanged(__.end_time); } }
        }

        private String _content;
        /// <summary></summary>
        [DisplayName("Content")]
        [Description("")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(7, "content", "", "0", "nvarchar(500)", 0, 0, true)]
        public virtual String content
        {
            get { return _content; }
            set { if (OnPropertyChanging(__.content, value)) { _content = value; OnPropertyChanged(__.content); } }
        }

        private Int64 _gameid;
        /// <summary>游戏数据表的主键id</summary>
        [DisplayName("游戏数据表的主键id")]
        [Description("游戏数据表的主键id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "gameid", "游戏数据表的主键id", "0", "bigint", 19, 0, false)]
        public virtual Int64 gameid
        {
            get { return _gameid; }
            set { if (OnPropertyChanging(__.gameid, value)) { _gameid = value; OnPropertyChanged(__.gameid); } }
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
                    case __.pid : return _pid;
                    case __.sid : return _sid;
                    case __.player_id : return _player_id;
                    case __.start_time : return _start_time;
                    case __.end_time : return _end_time;
                    case __.content : return _content;
                    case __.gameid : return _gameid;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.pid : _pid = Convert.ToInt64(value); break;
                    case __.sid : _sid = Convert.ToInt64(value); break;
                    case __.player_id : _player_id = Convert.ToInt64(value); break;
                    case __.start_time : _start_time = Convert.ToInt64(value); break;
                    case __.end_time : _end_time = Convert.ToInt64(value); break;
                    case __.content : _content = Convert.ToString(value); break;
                    case __.gameid : _gameid = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得系统公告表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary></summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary></summary>
            public static readonly Field player_id = FindByName(__.player_id);

            ///<summary></summary>
            public static readonly Field start_time = FindByName(__.start_time);

            ///<summary></summary>
            public static readonly Field end_time = FindByName(__.end_time);

            ///<summary></summary>
            public static readonly Field content = FindByName(__.content);

            ///<summary>游戏数据表的主键id</summary>
            public static readonly Field gameid = FindByName(__.gameid);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得系统公告表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String pid = "pid";

            ///<summary></summary>
            public const String sid = "sid";

            ///<summary></summary>
            public const String player_id = "player_id";

            ///<summary></summary>
            public const String start_time = "start_time";

            ///<summary></summary>
            public const String end_time = "end_time";

            ///<summary></summary>
            public const String content = "content";

            ///<summary>游戏数据表的主键id</summary>
            public const String gameid = "gameid";

        }
        #endregion
    }

    /// <summary>系统公告表接口</summary>
    public partial interface Itgm_notice
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 pid { get; set; }

        /// <summary></summary>
        Int64 sid { get; set; }

        /// <summary></summary>
        Int64 player_id { get; set; }

        /// <summary></summary>
        Int64 start_time { get; set; }

        /// <summary></summary>
        Int64 end_time { get; set; }

        /// <summary></summary>
        String content { get; set; }

        /// <summary>游戏数据表的主键id</summary>
        Int64 gameid { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}