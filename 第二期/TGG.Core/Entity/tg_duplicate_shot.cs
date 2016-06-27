using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>副本射击表</summary>
    [Serializable]
    [DataObject]
    [Description("副本射击表")]
    [BindIndex("PK_tg_duplicate_shot", true, "id")]
    [BindTable("tg_duplicate_shot", Description = "副本射击表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_duplicate_shot : Itg_duplicate_shot
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
        /// <summary></summary>
        [DisplayName("ID1")]
        [Description("")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(2, "user_id", "", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _score_current;
        /// <summary>当前闯关分数</summary>
        [DisplayName("当前闯关分数")]
        [Description("当前闯关分数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "score_current", "当前闯关分数", "0", "int", 10, 0, false)]
        public virtual Int32 score_current
        {
            get { return _score_current; }
            set { if (OnPropertyChanging(__.score_current, value)) { _score_current = value; OnPropertyChanged(__.score_current); } }
        }

        private Int32 _score_total;
        /// <summary>每次闯关总分</summary>
        [DisplayName("每次闯关总分")]
        [Description("每次闯关总分")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "score_total", "每次闯关总分", "0", "int", 10, 0, false)]
        public virtual Int32 score_total
        {
            get { return _score_total; }
            set { if (OnPropertyChanging(__.score_total, value)) { _score_total = value; OnPropertyChanged(__.score_total); } }
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
                    case __.score_current : return _score_current;
                    case __.score_total : return _score_total;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.score_current : _score_current = Convert.ToInt32(value); break;
                    case __.score_total : _score_total = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得副本射击表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>当前闯关分数</summary>
            public static readonly Field score_current = FindByName(__.score_current);

            ///<summary>每次闯关总分</summary>
            public static readonly Field score_total = FindByName(__.score_total);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得副本射击表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary>当前闯关分数</summary>
            public const String score_current = "score_current";

            ///<summary>每次闯关总分</summary>
            public const String score_total = "score_total";

        }
        #endregion
    }

    /// <summary>副本射击表接口</summary>
    public partial interface Itg_duplicate_shot
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary>当前闯关分数</summary>
        Int32 score_current { get; set; }

        /// <summary>每次闯关总分</summary>
        Int32 score_total { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}