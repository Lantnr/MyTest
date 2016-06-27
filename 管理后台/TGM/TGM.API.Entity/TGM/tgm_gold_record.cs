using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>元宝充值消费记录</summary>
    [Serializable]
    [DataObject]
    [Description("元宝充值消费记录")]
    [BindTable("tgm_gold_record", Description = "元宝充值消费记录", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_gold_record : Itgm_gold_record
    {
        #region 属性
        private Int32 _id;
        /// <summary>主键id</summary>
        [DisplayName("主键id")]
        [Description("主键id")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "主键id", null, "int", 10, 0, false)]
        public virtual Int32 id
        {
            get { return _id; }
            set { if (OnPropertyChanging(__.id, value)) { _id = value; OnPropertyChanged(__.id); } }
        }

        private Int32 _sid;
        /// <summary>服务器id</summary>
        [DisplayName("服务器id")]
        [Description("服务器id")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "sid", "服务器id", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private Int32 _recharge_count;
        /// <summary>充值金额</summary>
        [DisplayName("充值金额")]
        [Description("充值金额")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "recharge_count", "充值金额", "0", "int", 10, 0, false)]
        public virtual Int32 recharge_count
        {
            get { return _recharge_count; }
            set { if (OnPropertyChanging(__.recharge_count, value)) { _recharge_count = value; OnPropertyChanged(__.recharge_count); } }
        }

        private Int32 _recharge_people;
        /// <summary>充值人数</summary>
        [DisplayName("充值人数")]
        [Description("充值人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "recharge_people", "充值人数", "0", "int", 10, 0, false)]
        public virtual Int32 recharge_people
        {
            get { return _recharge_people; }
            set { if (OnPropertyChanging(__.recharge_people, value)) { _recharge_people = value; OnPropertyChanged(__.recharge_people); } }
        }

        private Int32 _consume;
        /// <summary>消耗元宝数量</summary>
        [DisplayName("消耗元宝数量")]
        [Description("消耗元宝数量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "consume", "消耗元宝数量", "0", "int", 10, 0, false)]
        public virtual Int32 consume
        {
            get { return _consume; }
            set { if (OnPropertyChanging(__.consume, value)) { _consume = value; OnPropertyChanged(__.consume); } }
        }

        private Int64 _createtime;
        /// <summary>日期</summary>
        [DisplayName("日期")]
        [Description("日期")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(6, "createtime", "日期", "0", "bigint", 19, 0, false)]
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
                    case __.id: return _id;
                    case __.sid: return _sid;
                    case __.recharge_count: return _recharge_count;
                    case __.recharge_people: return _recharge_people;
                    case __.consume: return _consume;
                    case __.createtime: return _createtime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id: _id = Convert.ToInt32(value); break;
                    case __.sid: _sid = Convert.ToInt32(value); break;
                    case __.recharge_count: _recharge_count = Convert.ToInt32(value); break;
                    case __.recharge_people: _recharge_people = Convert.ToInt32(value); break;
                    case __.consume: _consume = Convert.ToInt32(value); break;
                    case __.createtime: _createtime = Convert.ToInt64(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得元宝充值消费记录字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>服务器id</summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary>充值金额</summary>
            public static readonly Field recharge_count = FindByName(__.recharge_count);

            ///<summary>充值人数</summary>
            public static readonly Field recharge_people = FindByName(__.recharge_people);

            ///<summary>消耗元宝数量</summary>
            public static readonly Field consume = FindByName(__.consume);

            ///<summary>日期</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得元宝充值消费记录字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键id</summary>
            public const String id = "id";

            ///<summary>服务器id</summary>
            public const String sid = "sid";

            ///<summary>充值金额</summary>
            public const String recharge_count = "recharge_count";

            ///<summary>充值人数</summary>
            public const String recharge_people = "recharge_people";

            ///<summary>消耗元宝数量</summary>
            public const String consume = "consume";

            ///<summary>日期</summary>
            public const String createtime = "createtime";

        }
        #endregion
    }

    /// <summary>元宝充值消费记录接口</summary>
    public partial interface Itgm_gold_record
    {
        #region 属性
        /// <summary>主键id</summary>
        Int32 id { get; set; }

        /// <summary>服务器id</summary>
        Int32 sid { get; set; }

        /// <summary>充值金额</summary>
        Int32 recharge_count { get; set; }

        /// <summary>充值人数</summary>
        Int32 recharge_people { get; set; }

        /// <summary>消耗元宝数量</summary>
        Int32 consume { get; set; }

        /// <summary>日期</summary>
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