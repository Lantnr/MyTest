using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家场景表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家场景表")]
    [BindIndex("PK_tg_scene", true, "id")]
    [BindTable("tg_scene", Description = "玩家场景表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_scene : Itg_scene
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

        private Int64 _scene_id;
        /// <summary>场景编号</summary>
        [DisplayName("场景编号")]
        [Description("场景编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "scene_id", "场景编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 scene_id
        {
            get { return _scene_id; }
            set { if (OnPropertyChanging(__.scene_id, value)) { _scene_id = value; OnPropertyChanged(__.scene_id); } }
        }

        private Int64 _user_id;
        /// <summary>用户ID</summary>
        [DisplayName("用户ID")]
        [Description("用户ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "user_id", "用户ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _X;
        /// <summary>玩家X坐标</summary>
        [DisplayName("玩家X坐标")]
        [Description("玩家X坐标")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "X", "玩家X坐标", "0", "int", 10, 0, false)]
        public virtual Int32 X
        {
            get { return _X; }
            set { if (OnPropertyChanging(__.X, value)) { _X = value; OnPropertyChanged(__.X); } }
        }

        private Int32 _Y;
        /// <summary>玩家Y坐标</summary>
        [DisplayName("玩家Y坐标")]
        [Description("玩家Y坐标")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "Y", "玩家Y坐标", "0", "int", 10, 0, false)]
        public virtual Int32 Y
        {
            get { return _Y; }
            set { if (OnPropertyChanging(__.Y, value)) { _Y = value; OnPropertyChanged(__.Y); } }
        }

        private Int32 _model_number;
        /// <summary>模块号</summary>
        [DisplayName("模块号")]
        [Description("模块号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "model_number", "模块号", "0", "int", 10, 0, false)]
        public virtual Int32 model_number
        {
            get { return _model_number; }
            set { if (OnPropertyChanging(__.model_number, value)) { _model_number = value; OnPropertyChanged(__.model_number); } }
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
                    case __.model_number : return _model_number;
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
                    case __.model_number : _model_number = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家场景表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>场景编号</summary>
            public static readonly Field scene_id = FindByName(__.scene_id);

            ///<summary>用户ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>玩家X坐标</summary>
            public static readonly Field X = FindByName(__.X);

            ///<summary>玩家Y坐标</summary>
            public static readonly Field Y = FindByName(__.Y);

            ///<summary>模块号</summary>
            public static readonly Field model_number = FindByName(__.model_number);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家场景表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>场景编号</summary>
            public const String scene_id = "scene_id";

            ///<summary>用户ID</summary>
            public const String user_id = "user_id";

            ///<summary>玩家X坐标</summary>
            public const String X = "X";

            ///<summary>玩家Y坐标</summary>
            public const String Y = "Y";

            ///<summary>模块号</summary>
            public const String model_number = "model_number";

        }
        #endregion
    }

    /// <summary>玩家场景表接口</summary>
    public partial interface Itg_scene
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>场景编号</summary>
        Int64 scene_id { get; set; }

        /// <summary>用户ID</summary>
        Int64 user_id { get; set; }

        /// <summary>玩家X坐标</summary>
        Int32 X { get; set; }

        /// <summary>玩家Y坐标</summary>
        Int32 Y { get; set; }

        /// <summary>模块号</summary>
        Int32 model_number { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}