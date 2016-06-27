using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>个人战表</summary>
    [Serializable]
    [DataObject]
    [Description("个人战表")]
    [BindIndex("PK_tg_fight_personal", true, "id")]
    [BindTable("tg_fight_personal", Description = "个人战表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_fight_personal : Itg_fight_personal
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

        private Int64 _yid;
        /// <summary>印编号</summary>
        [DisplayName("印编号")]
        [Description("印编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "yid", "印编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 yid
        {
            get { return _yid; }
            set { if (OnPropertyChanging(__.yid, value)) { _yid = value; OnPropertyChanged(__.yid); } }
        }

        private Int64 _matrix1_rid;
        /// <summary>阵法位置1武将表编号</summary>
        [DisplayName("阵法位置1武将表编号")]
        [Description("阵法位置1武将表编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(4, "matrix1_rid", "阵法位置1武将表编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 matrix1_rid
        {
            get { return _matrix1_rid; }
            set { if (OnPropertyChanging(__.matrix1_rid, value)) { _matrix1_rid = value; OnPropertyChanged(__.matrix1_rid); } }
        }

        private Int64 _matrix2_rid;
        /// <summary>阵法位置2武将编号</summary>
        [DisplayName("阵法位置2武将编号")]
        [Description("阵法位置2武将编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(5, "matrix2_rid", "阵法位置2武将编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 matrix2_rid
        {
            get { return _matrix2_rid; }
            set { if (OnPropertyChanging(__.matrix2_rid, value)) { _matrix2_rid = value; OnPropertyChanged(__.matrix2_rid); } }
        }

        private Int64 _matrix3_rid;
        /// <summary>阵法位置3武将编号</summary>
        [DisplayName("阵法位置3武将编号")]
        [Description("阵法位置3武将编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "matrix3_rid", "阵法位置3武将编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 matrix3_rid
        {
            get { return _matrix3_rid; }
            set { if (OnPropertyChanging(__.matrix3_rid, value)) { _matrix3_rid = value; OnPropertyChanged(__.matrix3_rid); } }
        }

        private Int64 _matrix4_rid;
        /// <summary>阵法位置4武将编号</summary>
        [DisplayName("阵法位置4武将编号")]
        [Description("阵法位置4武将编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(7, "matrix4_rid", "阵法位置4武将编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 matrix4_rid
        {
            get { return _matrix4_rid; }
            set { if (OnPropertyChanging(__.matrix4_rid, value)) { _matrix4_rid = value; OnPropertyChanged(__.matrix4_rid); } }
        }

        private Int64 _matrix5_rid;
        /// <summary>阵法位置5武将编号</summary>
        [DisplayName("阵法位置5武将编号")]
        [Description("阵法位置5武将编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(8, "matrix5_rid", "阵法位置5武将编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 matrix5_rid
        {
            get { return _matrix5_rid; }
            set { if (OnPropertyChanging(__.matrix5_rid, value)) { _matrix5_rid = value; OnPropertyChanged(__.matrix5_rid); } }
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
                    case __.yid : return _yid;
                    case __.matrix1_rid : return _matrix1_rid;
                    case __.matrix2_rid : return _matrix2_rid;
                    case __.matrix3_rid : return _matrix3_rid;
                    case __.matrix4_rid : return _matrix4_rid;
                    case __.matrix5_rid : return _matrix5_rid;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.yid : _yid = Convert.ToInt64(value); break;
                    case __.matrix1_rid : _matrix1_rid = Convert.ToInt64(value); break;
                    case __.matrix2_rid : _matrix2_rid = Convert.ToInt64(value); break;
                    case __.matrix3_rid : _matrix3_rid = Convert.ToInt64(value); break;
                    case __.matrix4_rid : _matrix4_rid = Convert.ToInt64(value); break;
                    case __.matrix5_rid : _matrix5_rid = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得个人战表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>印编号</summary>
            public static readonly Field yid = FindByName(__.yid);

            ///<summary>阵法位置1武将表编号</summary>
            public static readonly Field matrix1_rid = FindByName(__.matrix1_rid);

            ///<summary>阵法位置2武将编号</summary>
            public static readonly Field matrix2_rid = FindByName(__.matrix2_rid);

            ///<summary>阵法位置3武将编号</summary>
            public static readonly Field matrix3_rid = FindByName(__.matrix3_rid);

            ///<summary>阵法位置4武将编号</summary>
            public static readonly Field matrix4_rid = FindByName(__.matrix4_rid);

            ///<summary>阵法位置5武将编号</summary>
            public static readonly Field matrix5_rid = FindByName(__.matrix5_rid);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得个人战表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>印编号</summary>
            public const String yid = "yid";

            ///<summary>阵法位置1武将表编号</summary>
            public const String matrix1_rid = "matrix1_rid";

            ///<summary>阵法位置2武将编号</summary>
            public const String matrix2_rid = "matrix2_rid";

            ///<summary>阵法位置3武将编号</summary>
            public const String matrix3_rid = "matrix3_rid";

            ///<summary>阵法位置4武将编号</summary>
            public const String matrix4_rid = "matrix4_rid";

            ///<summary>阵法位置5武将编号</summary>
            public const String matrix5_rid = "matrix5_rid";

        }
        #endregion
    }

    /// <summary>个人战表接口</summary>
    public partial interface Itg_fight_personal
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>印编号</summary>
        Int64 yid { get; set; }

        /// <summary>阵法位置1武将表编号</summary>
        Int64 matrix1_rid { get; set; }

        /// <summary>阵法位置2武将编号</summary>
        Int64 matrix2_rid { get; set; }

        /// <summary>阵法位置3武将编号</summary>
        Int64 matrix3_rid { get; set; }

        /// <summary>阵法位置4武将编号</summary>
        Int64 matrix4_rid { get; set; }

        /// <summary>阵法位置5武将编号</summary>
        Int64 matrix5_rid { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}