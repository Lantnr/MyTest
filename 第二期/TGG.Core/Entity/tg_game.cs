using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>游艺园表</summary>
    [Serializable]
    [DataObject]
    [Description("游艺园表")]
    [BindIndex("PK_tg_game", true, "id")]
    [BindTable("tg_game", Description = "游艺园表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_game : Itg_game
    {
        #region 属性
        private Int64 _id;
        /// <summary>主键ID</summary>
        [DisplayName("主键ID")]
        [Description("主键ID")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "主键ID", null, "bigint", 19, 0, false)]
        public virtual Int64 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int64 _user_id;
        /// <summary>用户ID</summary>
        [DisplayName("用户ID")]
        [Description("用户ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "用户ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _tea_max_pass;
        /// <summary>花月茶道本周最高闯关</summary>
        [DisplayName("花月茶道本周最高闯关")]
        [Description("花月茶道本周最高闯关")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "tea_max_pass", "花月茶道本周最高闯关", "0", "int", 10, 0, false)]
        public virtual Int32 tea_max_pass
        {
            get { return _tea_max_pass; }
            set { if (OnPropertyChanging(__.tea_max_pass, value)) { _tea_max_pass = value; OnPropertyChanged(__.tea_max_pass); } }
        }

        private Int32 _ninjutsu_max_pass;
        /// <summary>猜宝本周最高闯关</summary>
        [DisplayName("猜宝本周最高闯关")]
        [Description("猜宝本周最高闯关")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "ninjutsu_max_pass", "猜宝本周最高闯关", "0", "int", 10, 0, false)]
        public virtual Int32 ninjutsu_max_pass
        {
            get { return _ninjutsu_max_pass; }
            set { if (OnPropertyChanging(__.ninjutsu_max_pass, value)) { _ninjutsu_max_pass = value; OnPropertyChanged(__.ninjutsu_max_pass); } }
        }

        private Int32 _calculate_max_pass;
        /// <summary>老虎机本周最高闯关</summary>
        [DisplayName("老虎机本周最高闯关")]
        [Description("老虎机本周最高闯关")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "calculate_max_pass", "老虎机本周最高闯关", "0", "int", 10, 0, false)]
        public virtual Int32 calculate_max_pass
        {
            get { return _calculate_max_pass; }
            set { if (OnPropertyChanging(__.calculate_max_pass, value)) { _calculate_max_pass = value; OnPropertyChanged(__.calculate_max_pass); } }
        }

        private Int32 _eloquence_max_pass;
        /// <summary>辩驳本周最高闯关</summary>
        [DisplayName("辩驳本周最高闯关")]
        [Description("辩驳本周最高闯关")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "eloquence_max_pass", "辩驳本周最高闯关", "0", "int", 10, 0, false)]
        public virtual Int32 eloquence_max_pass
        {
            get { return _eloquence_max_pass; }
            set { if (OnPropertyChanging(__.eloquence_max_pass, value)) { _eloquence_max_pass = value; OnPropertyChanged(__.eloquence_max_pass); } }
        }

        private Int32 _spirit_max_pass;
        /// <summary>猎魂本周最高闯关</summary>
        [DisplayName("猎魂本周最高闯关")]
        [Description("猎魂本周最高闯关")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "spirit_max_pass", "猎魂本周最高闯关", "0", "int", 10, 0, false)]
        public virtual Int32 spirit_max_pass
        {
            get { return _spirit_max_pass; }
            set { if (OnPropertyChanging(__.spirit_max_pass, value)) { _spirit_max_pass = value; OnPropertyChanged(__.spirit_max_pass); } }
        }

        private Int32 _week_max_pass;
        /// <summary>本周最高闯关总数</summary>
        [DisplayName("本周最高闯关总数")]
        [Description("本周最高闯关总数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "week_max_pass", "本周最高闯关总数", "0", "int", 10, 0, false)]
        public virtual Int32 week_max_pass
        {
            get { return _week_max_pass; }
            set { if (OnPropertyChanging(__.week_max_pass, value)) { _week_max_pass = value; OnPropertyChanged(__.week_max_pass); } }
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
                    case __.user_id: return _user_id;
                    case __.tea_max_pass: return _tea_max_pass;
                    case __.ninjutsu_max_pass: return _ninjutsu_max_pass;
                    case __.calculate_max_pass: return _calculate_max_pass;
                    case __.eloquence_max_pass: return _eloquence_max_pass;
                    case __.spirit_max_pass: return _spirit_max_pass;
                    case __.week_max_pass: return _week_max_pass;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt64(value); break;
                    case __.user_id: _user_id = Convert.ToInt64(value); break;
                    case __.tea_max_pass: _tea_max_pass = Convert.ToInt32(value); break;
                    case __.ninjutsu_max_pass: _ninjutsu_max_pass = Convert.ToInt32(value); break;
                    case __.calculate_max_pass: _calculate_max_pass = Convert.ToInt32(value); break;
                    case __.eloquence_max_pass: _eloquence_max_pass = Convert.ToInt32(value); break;
                    case __.spirit_max_pass: _spirit_max_pass = Convert.ToInt32(value); break;
                    case __.week_max_pass: _week_max_pass = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得游艺园表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>用户ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>花月茶道本周最高闯关</summary>
            public static readonly Field tea_max_pass = FindByName(__.tea_max_pass);

            ///<summary>猜宝本周最高闯关</summary>
            public static readonly Field ninjutsu_max_pass = FindByName(__.ninjutsu_max_pass);

            ///<summary>老虎机本周最高闯关</summary>
            public static readonly Field calculate_max_pass = FindByName(__.calculate_max_pass);

            ///<summary>辩驳本周最高闯关</summary>
            public static readonly Field eloquence_max_pass = FindByName(__.eloquence_max_pass);

            ///<summary>猎魂本周最高闯关</summary>
            public static readonly Field spirit_max_pass = FindByName(__.spirit_max_pass);

            ///<summary>本周最高闯关总数</summary>
            public static readonly Field week_max_pass = FindByName(__.week_max_pass);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得游艺园表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键ID</summary>
            public const String id = "id";

            ///<summary>用户ID</summary>
            public const String user_id = "user_id";

            ///<summary>花月茶道本周最高闯关</summary>
            public const String tea_max_pass = "tea_max_pass";

            ///<summary>猜宝本周最高闯关</summary>
            public const String ninjutsu_max_pass = "ninjutsu_max_pass";

            ///<summary>老虎机本周最高闯关</summary>
            public const String calculate_max_pass = "calculate_max_pass";

            ///<summary>辩驳本周最高闯关</summary>
            public const String eloquence_max_pass = "eloquence_max_pass";

            ///<summary>猎魂本周最高闯关</summary>
            public const String spirit_max_pass = "spirit_max_pass";

            ///<summary>本周最高闯关总数</summary>
            public const String week_max_pass = "week_max_pass";

        }
        #endregion
    }

    /// <summary>游艺园表接口</summary>
    public partial interface Itg_game
    {
        #region 属性
        /// <summary>主键ID</summary>
        Int64 id { get; set; }

        /// <summary>用户ID</summary>
        Int64 user_id { get; set; }

        /// <summary>花月茶道本周最高闯关</summary>
        Int32 tea_max_pass { get; set; }

        /// <summary>猜宝本周最高闯关</summary>
        Int32 ninjutsu_max_pass { get; set; }

        /// <summary>老虎机本周最高闯关</summary>
        Int32 calculate_max_pass { get; set; }

        /// <summary>辩驳本周最高闯关</summary>
        Int32 eloquence_max_pass { get; set; }

        /// <summary>猎魂本周最高闯关</summary>
        Int32 spirit_max_pass { get; set; }

        /// <summary>本周最高闯关总数</summary>
        Int32 week_max_pass { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}