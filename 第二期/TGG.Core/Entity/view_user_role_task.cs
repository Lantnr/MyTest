using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>User_Role_Task</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_user_role_task", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_user_role_task : Iview_user_role_task
    {
        #region 属性
        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(1, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _player_vocation;
        /// <summary></summary>
        [DisplayName("Vocation")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "player_vocation", "", null, "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "player_camp", "", null, "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
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
                    case __.user_id : return _user_id;
                    case __.player_vocation : return _player_vocation;
                    case __.role_identity : return _role_identity;
                    case __.player_camp : return _player_camp;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得User_Role_Task字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得User_Role_Task字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String player_vocation = "player_vocation";

            ///<summary></summary>
            public const String role_identity = "role_identity";

            ///<summary></summary>
            public const String player_camp = "player_camp";

        }
        #endregion
    }

    /// <summary>User_Role_Task接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_user_role_task
    {
        #region 属性
        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        Int32 player_vocation { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }

        /// <summary></summary>
        Int32 player_camp { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}