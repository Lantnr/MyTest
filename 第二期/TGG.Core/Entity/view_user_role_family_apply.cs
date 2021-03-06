﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>User_Role_Family_Apply</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_user_role_family_apply", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_user_role_family_apply : Iview_user_role_family_apply
    {
        #region 属性
        private Int64 _id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(1, "id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _userid;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "userid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 userid
        {
            get { return _userid; }
            set { if (OnPropertyChanging(__.userid, value)) { _userid = value; OnPropertyChanged(__.userid); } }
        }

        private Int32 _state;
        /// <summary></summary>
        [DisplayName("State")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "state", "", null, "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int64 _time;
        /// <summary></summary>
        [DisplayName("Time")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "time", "", null, "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int64 _fid;
        /// <summary></summary>
        [DisplayName("Fid")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "fid", "", null, "bigint", 19, 0, false)]
        public virtual Int64 fid
        {
            get { return _fid; }
            set { if (OnPropertyChanging(__.fid, value)) { _fid = value; OnPropertyChanged(__.fid); } }
        }

        private String _player_name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "player_name", "", null, "nvarchar(10)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private Int32 _role_id;
        /// <summary></summary>
        [DisplayName("ID2")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "role_id", "", null, "int", 10, 0, false)]
        public virtual Int32 role_id
        {
            get { return _role_id; }
            set { if (OnPropertyChanging(__.role_id, value)) { _role_id = value; OnPropertyChanged(__.role_id); } }
        }

        private Int32 _player_sex;
        /// <summary></summary>
        [DisplayName("Sex")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "player_sex", "", null, "int", 10, 0, false)]
        public virtual Int32 player_sex
        {
            get { return _player_sex; }
            set { if (OnPropertyChanging(__.player_sex, value)) { _player_sex = value; OnPropertyChanged(__.player_sex); } }
        }

        private Int32 _player_vocation;
        /// <summary></summary>
        [DisplayName("Vocation")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "player_vocation", "", null, "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _player_position;
        /// <summary></summary>
        [DisplayName("Position")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "player_position", "", null, "int", 10, 0, false)]
        public virtual Int32 player_position
        {
            get { return _player_position; }
            set { if (OnPropertyChanging(__.player_position, value)) { _player_position = value; OnPropertyChanged(__.player_position); } }
        }

        private Int32 _player_influence;
        /// <summary></summary>
        [DisplayName("Influence")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "player_influence", "", null, "int", 10, 0, false)]
        public virtual Int32 player_influence
        {
            get { return _player_influence; }
            set { if (OnPropertyChanging(__.player_influence, value)) { _player_influence = value; OnPropertyChanged(__.player_influence); } }
        }

        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "player_camp", "", null, "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
        }

        private Int32 _power;
        /// <summary></summary>
        [DisplayName("Power")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "power", "", null, "int", 10, 0, false)]
        public virtual Int32 power
        {
            get { return _power; }
            set { if (OnPropertyChanging(__.power, value)) { _power = value; OnPropertyChanged(__.power); } }
        }

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "role_level", "", null, "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
        }

        private Int32 _role_state;
        /// <summary></summary>
        [DisplayName("State1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "role_state", "", null, "int", 10, 0, false)]
        public virtual Int32 role_state
        {
            get { return _role_state; }
            set { if (OnPropertyChanging(__.role_state, value)) { _role_state = value; OnPropertyChanged(__.role_state); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
        }

        private Int32 _role_honor;
        /// <summary></summary>
        [DisplayName("Honor")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "role_honor", "", null, "int", 10, 0, false)]
        public virtual Int32 role_honor
        {
            get { return _role_honor; }
            set { if (OnPropertyChanging(__.role_honor, value)) { _role_honor = value; OnPropertyChanged(__.role_honor); } }
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
                    case __.userid : return _userid;
                    case __.state : return _state;
                    case __.time : return _time;
                    case __.fid : return _fid;
                    case __.player_name : return _player_name;
                    case __.role_id : return _role_id;
                    case __.player_sex : return _player_sex;
                    case __.player_vocation : return _player_vocation;
                    case __.player_position : return _player_position;
                    case __.player_influence : return _player_influence;
                    case __.player_camp : return _player_camp;
                    case __.power : return _power;
                    case __.role_level : return _role_level;
                    case __.role_state : return _role_state;
                    case __.role_identity : return _role_identity;
                    case __.role_honor : return _role_honor;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.userid : _userid = Convert.ToInt64(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.fid : _fid = Convert.ToInt64(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.role_id : _role_id = Convert.ToInt32(value); break;
                    case __.player_sex : _player_sex = Convert.ToInt32(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.player_position : _player_position = Convert.ToInt32(value); break;
                    case __.player_influence : _player_influence = Convert.ToInt32(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.power : _power = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.role_state : _role_state = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    case __.role_honor : _role_honor = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得User_Role_Family_Apply字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field userid = FindByName(__.userid);

            ///<summary></summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary></summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary></summary>
            public static readonly Field fid = FindByName(__.fid);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field role_id = FindByName(__.role_id);

            ///<summary></summary>
            public static readonly Field player_sex = FindByName(__.player_sex);

            ///<summary></summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary></summary>
            public static readonly Field player_position = FindByName(__.player_position);

            ///<summary></summary>
            public static readonly Field player_influence = FindByName(__.player_influence);

            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary></summary>
            public static readonly Field power = FindByName(__.power);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field role_state = FindByName(__.role_state);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            ///<summary></summary>
            public static readonly Field role_honor = FindByName(__.role_honor);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得User_Role_Family_Apply字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String userid = "userid";

            ///<summary></summary>
            public const String state = "state";

            ///<summary></summary>
            public const String time = "time";

            ///<summary></summary>
            public const String fid = "fid";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String role_id = "role_id";

            ///<summary></summary>
            public const String player_sex = "player_sex";

            ///<summary></summary>
            public const String player_vocation = "player_vocation";

            ///<summary></summary>
            public const String player_position = "player_position";

            ///<summary></summary>
            public const String player_influence = "player_influence";

            ///<summary></summary>
            public const String player_camp = "player_camp";

            ///<summary></summary>
            public const String power = "power";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String role_state = "role_state";

            ///<summary></summary>
            public const String role_identity = "role_identity";

            ///<summary></summary>
            public const String role_honor = "role_honor";

        }
        #endregion
    }

    /// <summary>User_Role_Family_Apply接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_user_role_family_apply
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 userid { get; set; }

        /// <summary></summary>
        Int32 state { get; set; }

        /// <summary></summary>
        Int64 time { get; set; }

        /// <summary></summary>
        Int64 fid { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 role_id { get; set; }

        /// <summary></summary>
        Int32 player_sex { get; set; }

        /// <summary></summary>
        Int32 player_vocation { get; set; }

        /// <summary></summary>
        Int32 player_position { get; set; }

        /// <summary></summary>
        Int32 player_influence { get; set; }

        /// <summary></summary>
        Int32 player_camp { get; set; }

        /// <summary></summary>
        Int32 power { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int32 role_state { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }

        /// <summary></summary>
        Int32 role_honor { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}