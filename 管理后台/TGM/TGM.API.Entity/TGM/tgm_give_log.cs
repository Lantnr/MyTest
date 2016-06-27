using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>福利卡发放记录表</summary>
    [Serializable]
    [DataObject]
    [Description("福利卡发放记录表")]
    [BindTable("tgm_give_log", Description = "福利卡发放记录表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_give_log : Itgm_give_log
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
        /// <summary></summary>
        [DisplayName("Pid")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "pid", "", "0", "int", 10, 0, false)]
        public virtual Int32 pid
        {
            get { return _pid; }
            set { if (OnPropertyChanging(__.pid, value)) { _pid = value; OnPropertyChanged(__.pid); } }
        }

        private String _platform_name;
        /// <summary>平台名称</summary>
        [DisplayName("平台名称")]
        [Description("平台名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(3, "platform_name", "平台名称", "0", "nvarchar(100)", 0, 0, true)]
        public virtual String platform_name
        {
            get { return _platform_name; }
            set { if (OnPropertyChanging(__.platform_name, value)) { _platform_name = value; OnPropertyChanged(__.platform_name); } }
        }

        private Int32 _sid;
        /// <summary></summary>
        [DisplayName("Sid")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "sid", "", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private String _server_name;
        /// <summary>服务器名称</summary>
        [DisplayName("服务器名称")]
        [Description("服务器名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(5, "server_name", "服务器名称", "0", "nvarchar(100)", 0, 0, true)]
        public virtual String server_name
        {
            get { return _server_name; }
            set { if (OnPropertyChanging(__.server_name, value)) { _server_name = value; OnPropertyChanged(__.server_name); } }
        }

        private String _kind;
        /// <summary>生成批次(默认yyyyMMDDHHII)</summary>
        [DisplayName("生成批次默认yyyyMMDDHHII")]
        [Description("生成批次(默认yyyyMMDDHHII)")]
        [DataObjectField(false, false, false, 20)]
        [BindColumn(6, "kind", "生成批次(默认yyyyMMDDHHII)", "0", "nvarchar(20)", 0, 0, true)]
        public virtual String kind
        {
            get { return _kind; }
            set { if (OnPropertyChanging(__.kind, value)) { _kind = value; OnPropertyChanged(__.kind); } }
        }

        private Int32 _give_type;
        /// <summary>发放激活码类型</summary>
        [DisplayName("发放激活码类型")]
        [Description("发放激活码类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "give_type", "发放激活码类型", "0", "int", 10, 0, false)]
        public virtual Int32 give_type
        {
            get { return _give_type; }
            set { if (OnPropertyChanging(__.give_type, value)) { _give_type = value; OnPropertyChanged(__.give_type); } }
        }

        private DateTime _createtime;
        /// <summary></summary>
        [DisplayName("Createtime")]
        [Description("")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(8, "createtime", "", "getdate()", "datetime", 3, 0, false)]
        public virtual DateTime createtime
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
                    case __.id: return _id;
                    case __.pid: return _pid;
                    case __.platform_name: return _platform_name;
                    case __.sid: return _sid;
                    case __.server_name: return _server_name;
                    case __.kind: return _kind;
                    case __.give_type: return _give_type;
                    case __.createtime: return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.pid: _pid = Convert.ToInt32(value); break;
                    case __.platform_name: _platform_name = Convert.ToString(value); break;
                    case __.sid: _sid = Convert.ToInt32(value); break;
                    case __.server_name: _server_name = Convert.ToString(value); break;
                    case __.kind: _kind = Convert.ToString(value); break;
                    case __.give_type: _give_type = Convert.ToInt32(value); break;
                    case __.createtime: _createtime = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得福利卡发放记录表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field pid = FindByName(__.pid);

            ///<summary>平台名称</summary>
            public static readonly Field platform_name = FindByName(__.platform_name);

            ///<summary></summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary>服务器名称</summary>
            public static readonly Field server_name = FindByName(__.server_name);

            ///<summary>生成批次(默认yyyyMMDDHHII)</summary>
            public static readonly Field kind = FindByName(__.kind);

            ///<summary>发放激活码类型</summary>
            public static readonly Field give_type = FindByName(__.give_type);

            ///<summary></summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得福利卡发放记录表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String pid = "pid";

            ///<summary>平台名称</summary>
            public const String platform_name = "platform_name";

            ///<summary></summary>
            public const String sid = "sid";

            ///<summary>服务器名称</summary>
            public const String server_name = "server_name";

            ///<summary>生成批次(默认yyyyMMDDHHII)</summary>
            public const String kind = "kind";

            ///<summary>发放激活码类型</summary>
            public const String give_type = "give_type";

            ///<summary></summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>福利卡发放记录表接口</summary>
    public partial interface Itgm_give_log
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int32 pid { get; set; }

        /// <summary>平台名称</summary>
        String platform_name { get; set; }

        /// <summary></summary>
        Int32 sid { get; set; }

        /// <summary>服务器名称</summary>
        String server_name { get; set; }

        /// <summary>生成批次(默认yyyyMMDDHHII)</summary>
        String kind { get; set; }

        /// <summary>发放激活码类型</summary>
        Int32 give_type { get; set; }

        /// <summary></summary>
        DateTime createtime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}