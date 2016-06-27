using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>服务器留存记录</summary>
    [DataObject]
    [Description("服务器留存记录")]
    [BindIndex("PK_tgm_record_keep", true, "id")]
    [BindTable("tgm_record_keep", Description = "服务器留存记录", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_record_keep : Itgm_record_keep
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

        private Int32 _pid;
        /// <summary>平台编号</summary>
        [DisplayName("平台编号")]
        [Description("平台编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "平台编号", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private Int32 _sid;
        /// <summary>服务器编号</summary>
        [DisplayName("服务器编号")]
        [Description("服务器编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "sid", "服务器编号", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private String _server_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "server_name", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String server_name
        {
            get { return _server_name; }
            set { if (OnPropertyChanging(__.server_name, value)) { _server_name = value; OnPropertyChanged(__.server_name); } }
        }

        private Int32 _login_30;
        /// <summary>30天登陆人数</summary>
        [DisplayName("30天登陆人数")]
        [Description("30天登陆人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "login_30", "30天登陆人数", "0", "int", 10, 0, false)]
        public virtual Int32 login_30
        {
            get { return _login_30; }
            set { if (OnPropertyChanging(__.login_30, value)) { _login_30 = value; OnPropertyChanged(__.login_30); } }
        }

        private Double _keep_1;
        /// <summary>1天留存率</summary>
        [DisplayName("1天留存率")]
        [Description("1天留存率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(6, "keep_1", "1天留存率", "0", "float", 53, 0, false)]
        public virtual Double keep_1
        {
            get { return _keep_1; }
            set { if (OnPropertyChanging(__.keep_1, value)) { _keep_1 = value; OnPropertyChanged(__.keep_1); } }
        }

        private Double _keep_3;
        /// <summary>3天留存率</summary>
        [DisplayName("3天留存率")]
        [Description("3天留存率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(7, "keep_3", "3天留存率", "0", "float", 53, 0, false)]
        public virtual Double keep_3
        {
            get { return _keep_3; }
            set { if (OnPropertyChanging(__.keep_3, value)) { _keep_3 = value; OnPropertyChanged(__.keep_3); } }
        }

        private Double _keep_5;
        /// <summary>5天留存率</summary>
        [DisplayName("5天留存率")]
        [Description("5天留存率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(8, "keep_5", "5天留存率", "0", "float", 53, 0, false)]
        public virtual Double keep_5
        {
            get { return _keep_5; }
            set { if (OnPropertyChanging(__.keep_5, value)) { _keep_5 = value; OnPropertyChanged(__.keep_5); } }
        }

        private Double _keep_7;
        /// <summary>7天留存率</summary>
        [DisplayName("7天留存率")]
        [Description("7天留存率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(9, "keep_7", "7天留存率", "0", "float", 53, 0, false)]
        public virtual Double keep_7
        {
            get { return _keep_7; }
            set { if (OnPropertyChanging(__.keep_7, value)) { _keep_7 = value; OnPropertyChanged(__.keep_7); } }
        }

        private Double _keep_30;
        /// <summary>30天留存率</summary>
        [DisplayName("30天留存率")]
        [Description("30天留存率")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(10, "keep_30", "30天留存率", "30", "float", 53, 0, false)]
        public virtual Double keep_30
        {
            get { return _keep_30; }
            set { if (OnPropertyChanging(__.keep_30, value)) { _keep_30 = value; OnPropertyChanged(__.keep_30); } }
        }

        private Int64 _createtime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "createtime", "创建时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
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
                    case __.server_name : return _server_name;
                    case __.login_30 : return _login_30;
                    case __.keep_1 : return _keep_1;
                    case __.keep_3 : return _keep_3;
                    case __.keep_5 : return _keep_5;
                    case __.keep_7 : return _keep_7;
                    case __.keep_30 : return _keep_30;
                    case __.createtime : return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.pid : _pid = Convert.ToInt32(value); break;
                    case __.sid : _sid = Convert.ToInt32(value); break;
                    case __.server_name : _server_name = Convert.ToString(value); break;
                    case __.login_30 : _login_30 = Convert.ToInt32(value); break;
                    case __.keep_1 : _keep_1 = Convert.ToDouble(value); break;
                    case __.keep_3 : _keep_3 = Convert.ToDouble(value); break;
                    case __.keep_5 : _keep_5 = Convert.ToDouble(value); break;
                    case __.keep_7 : _keep_7 = Convert.ToDouble(value); break;
                    case __.keep_30 : _keep_30 = Convert.ToDouble(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得服务器留存记录字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>平台编号</summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>服务器编号</summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary></summary>
            public static readonly Field server_name = FindByName(__.server_name);

            ///<summary>30天登陆人数</summary>
            public static readonly Field login_30 = FindByName(__.login_30);

            ///<summary>1天留存率</summary>
            public static readonly Field keep_1 = FindByName(__.keep_1);

            ///<summary>3天留存率</summary>
            public static readonly Field keep_3 = FindByName(__.keep_3);

            ///<summary>5天留存率</summary>
            public static readonly Field keep_5 = FindByName(__.keep_5);

            ///<summary>7天留存率</summary>
            public static readonly Field keep_7 = FindByName(__.keep_7);

            ///<summary>30天留存率</summary>
            public static readonly Field keep_30 = FindByName(__.keep_30);

            ///<summary>创建时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得服务器留存记录字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>平台编号</summary>
            public const String pid = "pid";

            ///<summary>服务器编号</summary>
            public const String sid = "sid";

            ///<summary></summary>
            public const String server_name = "server_name";

            ///<summary>30天登陆人数</summary>
            public const String login_30 = "login_30";

            ///<summary>1天留存率</summary>
            public const String keep_1 = "keep_1";

            ///<summary>3天留存率</summary>
            public const String keep_3 = "keep_3";

            ///<summary>5天留存率</summary>
            public const String keep_5 = "keep_5";

            ///<summary>7天留存率</summary>
            public const String keep_7 = "keep_7";

            ///<summary>30天留存率</summary>
            public const String keep_30 = "keep_30";

            ///<summary>创建时间</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>服务器留存记录接口</summary>
    public partial interface Itgm_record_keep
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>平台编号</summary>
        Int32 pid { get; set; }

        /// <summary>服务器编号</summary>
        Int32 sid { get; set; }

        /// <summary></summary>
        String server_name { get; set; }

        /// <summary>30天登陆人数</summary>
        Int32 login_30 { get; set; }

        /// <summary>1天留存率</summary>
        Double keep_1 { get; set; }

        /// <summary>3天留存率</summary>
        Double keep_3 { get; set; }

        /// <summary>5天留存率</summary>
        Double keep_5 { get; set; }

        /// <summary>7天留存率</summary>
        Double keep_7 { get; set; }

        /// <summary>30天留存率</summary>
        Double keep_30 { get; set; }

        /// <summary>创建时间</summary>
        Int64 createtime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}