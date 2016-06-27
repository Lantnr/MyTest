using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>Scene_User</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("view_scene_user", Description = "", ConnName = "DB", DbType = DatabaseType.SqlServer, IsView = true)]
    public partial class view_scene_user : Iview_scene_user
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

        private Int64 _scene_id;
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "scene_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 scene_id
        {
            get { return _scene_id; }
            set { if (OnPropertyChanging(__.scene_id, value)) { _scene_id = value; OnPropertyChanged(__.scene_id); } }
        }

        private Int64 _user_id;
        /// <summary></summary>
        [DisplayName("ID2")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _X;
        /// <summary></summary>
        [DisplayName("X")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "X", "", null, "int", 10, 0, false)]
        public virtual Int32 X
        {
            get { return _X; }
            set { if (OnPropertyChanging(__.X, value)) { _X = value; OnPropertyChanged(__.X); } }
        }

        private Int32 _Y;
        /// <summary></summary>
        [DisplayName("Y")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "Y", "", null, "int", 10, 0, false)]
        public virtual Int32 Y
        {
            get { return _Y; }
            set { if (OnPropertyChanging(__.Y, value)) { _Y = value; OnPropertyChanged(__.Y); } }
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

        private Int32 _player_sex;
        /// <summary></summary>
        [DisplayName("Sex")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "player_sex", "", null, "int", 10, 0, false)]
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
        [BindColumn(8, "player_vocation", "", null, "int", 10, 0, false)]
        public virtual Int32 player_vocation
        {
            get { return _player_vocation; }
            set { if (OnPropertyChanging(__.player_vocation, value)) { _player_vocation = value; OnPropertyChanged(__.player_vocation); } }
        }

        private Int32 _role_level;
        /// <summary></summary>
        [DisplayName("Level")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "role_level", "", null, "int", 10, 0, false)]
        public virtual Int32 role_level
        {
            get { return _role_level; }
            set { if (OnPropertyChanging(__.role_level, value)) { _role_level = value; OnPropertyChanged(__.role_level); } }
        }

        private Int32 _model_number;
        /// <summary></summary>
        [DisplayName("Number")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "model_number", "", null, "int", 10, 0, false)]
        public virtual Int32 model_number
        {
            get { return _model_number; }
            set { if (OnPropertyChanging(__.model_number, value)) { _model_number = value; OnPropertyChanged(__.model_number); } }
        }

        private Int32 _player_camp;
        /// <summary></summary>
        [DisplayName("Camp")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "player_camp", "", null, "int", 10, 0, false)]
        public virtual Int32 player_camp
        {
            get { return _player_camp; }
            set { if (OnPropertyChanging(__.player_camp, value)) { _player_camp = value; OnPropertyChanged(__.player_camp); } }
        }

        private Int32 _role_identity;
        /// <summary></summary>
        [DisplayName("Identity")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "role_identity", "", null, "int", 10, 0, false)]
        public virtual Int32 role_identity
        {
            get { return _role_identity; }
            set { if (OnPropertyChanging(__.role_identity, value)) { _role_identity = value; OnPropertyChanged(__.role_identity); } }
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
                    case __.scene_id : return _scene_id;
                    case __.user_id : return _user_id;
                    case __.X : return _X;
                    case __.Y : return _Y;
                    case __.player_name : return _player_name;
                    case __.player_sex : return _player_sex;
                    case __.player_vocation : return _player_vocation;
                    case __.role_level : return _role_level;
                    case __.model_number : return _model_number;
                    case __.player_camp : return _player_camp;
                    case __.role_identity : return _role_identity;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.scene_id : _scene_id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.X : _X = Convert.ToInt32(value); break;
                    case __.Y : _Y = Convert.ToInt32(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.player_sex : _player_sex = Convert.ToInt32(value); break;
                    case __.player_vocation : _player_vocation = Convert.ToInt32(value); break;
                    case __.role_level : _role_level = Convert.ToInt32(value); break;
                    case __.model_number : _model_number = Convert.ToInt32(value); break;
                    case __.player_camp : _player_camp = Convert.ToInt32(value); break;
                    case __.role_identity : _role_identity = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Scene_User字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field scene_id = FindByName(__.scene_id);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary></summary>
            public static readonly Field X = FindByName(__.X);

            ///<summary></summary>
            public static readonly Field Y = FindByName(__.Y);

            ///<summary></summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary></summary>
            public static readonly Field player_sex = FindByName(__.player_sex);

            ///<summary></summary>
            public static readonly Field player_vocation = FindByName(__.player_vocation);

            ///<summary></summary>
            public static readonly Field role_level = FindByName(__.role_level);

            ///<summary></summary>
            public static readonly Field model_number = FindByName(__.model_number);

            ///<summary></summary>
            public static readonly Field player_camp = FindByName(__.player_camp);

            ///<summary></summary>
            public static readonly Field role_identity = FindByName(__.role_identity);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Scene_User字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String scene_id = "scene_id";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary></summary>
            public const String X = "X";

            ///<summary></summary>
            public const String Y = "Y";

            ///<summary></summary>
            public const String player_name = "player_name";

            ///<summary></summary>
            public const String player_sex = "player_sex";

            ///<summary></summary>
            public const String player_vocation = "player_vocation";

            ///<summary></summary>
            public const String role_level = "role_level";

            ///<summary></summary>
            public const String model_number = "model_number";

            ///<summary></summary>
            public const String player_camp = "player_camp";

            ///<summary></summary>
            public const String role_identity = "role_identity";

        }
        #endregion
    }

    /// <summary>Scene_User接口</summary>
    /// <remarks></remarks>
    public partial interface Iview_scene_user
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 scene_id { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary></summary>
        Int32 X { get; set; }

        /// <summary></summary>
        Int32 Y { get; set; }

        /// <summary></summary>
        String player_name { get; set; }

        /// <summary></summary>
        Int32 player_sex { get; set; }

        /// <summary></summary>
        Int32 player_vocation { get; set; }

        /// <summary></summary>
        Int32 role_level { get; set; }

        /// <summary></summary>
        Int32 model_number { get; set; }

        /// <summary></summary>
        Int32 player_camp { get; set; }

        /// <summary></summary>
        Int32 role_identity { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}