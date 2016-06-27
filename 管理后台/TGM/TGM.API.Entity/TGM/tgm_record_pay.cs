using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>游戏支付记录</summary>
    [Serializable]
    [DataObject]
    [Description("游戏支付记录")]
    [BindTable("tgm_record_pay", Description = "游戏支付记录", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tgm_record_pay : Itgm_record_pay
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

        private Int32 _sid;
        /// <summary>平台</summary>
        [DisplayName("平台")]
        [Description("平台")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "sid", "平台", "0", "int", 10, 0, false)]
        public virtual Int32 sid
        {
            get { return _sid; }
            set { if (OnPropertyChanging(__.sid, value)) { _sid = value; OnPropertyChanged(__.sid); } }
        }

        private Int64 _player_id;
        /// <summary>玩家编号</summary>
        [DisplayName("玩家编号")]
        [Description("玩家编号")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "player_id", "玩家编号", "0", "bigint", 19, 0, false)]
        public virtual Int64 player_id
        {
            get { return _player_id; }
            set { if (OnPropertyChanging(__.player_id, value)) { _player_id = value; OnPropertyChanged(__.player_id); } }
        }

        private String _user_code;
        /// <summary>玩家账号</summary>
        [DisplayName("玩家账号")]
        [Description("玩家账号")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(4, "user_code", "玩家账号", null, "nvarchar(200)", 0, 0, true)]
        public virtual String user_code
        {
            get { return _user_code; }
            set { if (OnPropertyChanging(__.user_code, value)) { _user_code = value; OnPropertyChanged(__.user_code); } }
        }

        private String _player_name;
        /// <summary>玩家名称</summary>
        [DisplayName("玩家名称")]
        [Description("玩家名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(5, "player_name", "玩家名称", null, "nvarchar(100)", 0, 0, true)]
        public virtual String player_name
        {
            get { return _player_name; }
            set { if (OnPropertyChanging(__.player_name, value)) { _player_name = value; OnPropertyChanged(__.player_name); } }
        }

        private String _order_id;
        /// <summary>订单号</summary>
        [DisplayName("订单号")]
        [Description("订单号")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(6, "order_id", "订单号", null, "nvarchar(100)", 0, 0, true)]
        public virtual String order_id
        {
            get { return _order_id; }
            set { if (OnPropertyChanging(__.order_id, value)) { _order_id = value; OnPropertyChanged(__.order_id); } }
        }

        private String _channel;
        /// <summary>渠道</summary>
        [DisplayName("渠道")]
        [Description("渠道")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(7, "channel", "渠道", null, "nvarchar(100)", 0, 0, true)]
        public virtual String channel
        {
            get { return _channel; }
            set { if (OnPropertyChanging(__.channel, value)) { _channel = value; OnPropertyChanged(__.channel); } }
        }

        private Int32 _pay_type;
        /// <summary>充值类型(0:RMB,1:(1:1),2(1:10)...)</summary>
        [DisplayName("充值类型0:RMB,1:1:1,21:10")]
        [Description("充值类型(0:RMB,1:(1:1),2(1:10)...)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "pay_type", "充值类型(0:RMB,1:(1:1),2(1:10)...)", "0", "int", 10, 0, false)]
        public virtual Int32 pay_type
        {
            get { return _pay_type; }
            set { if (OnPropertyChanging(__.pay_type, value)) { _pay_type = value; OnPropertyChanged(__.pay_type); } }
        }

        private Int32 _amount;
        /// <summary>金额</summary>
        [DisplayName("金额")]
        [Description("金额")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "amount", "金额", "0", "int", 10, 0, false)]
        public virtual Int32 amount
        {
            get { return _amount; }
            set { if (OnPropertyChanging(__.amount, value)) { _amount = value; OnPropertyChanged(__.amount); } }
        }

        private Int32 _pay_state;
        /// <summary>支付状态</summary>
        [DisplayName("支付状态")]
        [Description("支付状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "pay_state", "支付状态", "0", "int", 10, 0, false)]
        public virtual Int32 pay_state
        {
            get { return _pay_state; }
            set { if (OnPropertyChanging(__.pay_state, value)) { _pay_state = value; OnPropertyChanged(__.pay_state); } }
        }

        private Int64 _createtime;
        /// <summary>时间</summary>
        [DisplayName("时间")]
        [Description("时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "createtime", "时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 createtime
        {
            get { return _createtime; }
            set { if (OnPropertyChanging(__.createtime, value)) { _createtime = value; OnPropertyChanged(__.createtime); } }
        }

        private Int32 _money;
        /// <summary>充值的钱</summary>
        [DisplayName("充值的钱")]
        [Description("充值的钱")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "money", "充值的钱", "0", "int", 10, 0, false)]
        public virtual Int32 money
        {
            get { return _money; }
            set { if (OnPropertyChanging(__.money, value)) { _money = value; OnPropertyChanged(__.money); } }
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
                    case __.sid : return _sid;
                    case __.player_id : return _player_id;
                    case __.user_code : return _user_code;
                    case __.player_name : return _player_name;
                    case __.order_id : return _order_id;
                    case __.channel : return _channel;
                    case __.pay_type : return _pay_type;
                    case __.amount : return _amount;
                    case __.pay_state : return _pay_state;
                    case __.createtime : return _createtime;
                    case __.money : return _money;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.sid : _sid = Convert.ToInt32(value); break;
                    case __.player_id : _player_id = Convert.ToInt64(value); break;
                    case __.user_code : _user_code = Convert.ToString(value); break;
                    case __.player_name : _player_name = Convert.ToString(value); break;
                    case __.order_id : _order_id = Convert.ToString(value); break;
                    case __.channel : _channel = Convert.ToString(value); break;
                    case __.pay_type : _pay_type = Convert.ToInt32(value); break;
                    case __.amount : _amount = Convert.ToInt32(value); break;
                    case __.pay_state : _pay_state = Convert.ToInt32(value); break;
                    case __.createtime : _createtime = Convert.ToInt64(value); break;
                    case __.money : _money = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得游戏支付记录字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>平台</summary>
            public static readonly Field sid = FindByName(__.sid);

            ///<summary>玩家编号</summary>
            public static readonly Field player_id = FindByName(__.player_id);

            ///<summary>玩家账号</summary>
            public static readonly Field user_code = FindByName(__.user_code);

            ///<summary>玩家名称</summary>
            public static readonly Field player_name = FindByName(__.player_name);

            ///<summary>订单号</summary>
            public static readonly Field order_id = FindByName(__.order_id);

            ///<summary>渠道</summary>
            public static readonly Field channel = FindByName(__.channel);

            ///<summary>充值类型(0:RMB,1:(1:1),2(1:10)...)</summary>
            public static readonly Field pay_type = FindByName(__.pay_type);

            ///<summary>金额</summary>
            public static readonly Field amount = FindByName(__.amount);

            ///<summary>支付状态</summary>
            public static readonly Field pay_state = FindByName(__.pay_state);

            ///<summary>时间</summary>
            public static readonly Field createtime = FindByName(__.createtime);

            ///<summary>充值的钱</summary>
            public static readonly Field money = FindByName(__.money);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得游戏支付记录字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>平台</summary>
            public const String sid = "sid";

            ///<summary>玩家编号</summary>
            public const String player_id = "player_id";

            ///<summary>玩家账号</summary>
            public const String user_code = "user_code";

            ///<summary>玩家名称</summary>
            public const String player_name = "player_name";

            ///<summary>订单号</summary>
            public const String order_id = "order_id";

            ///<summary>渠道</summary>
            public const String channel = "channel";

            ///<summary>充值类型(0:RMB,1:(1:1),2(1:10)...)</summary>
            public const String pay_type = "pay_type";

            ///<summary>金额</summary>
            public const String amount = "amount";

            ///<summary>支付状态</summary>
            public const String pay_state = "pay_state";

            ///<summary>时间</summary>
            public const String createtime = "createtime";

            ///<summary>充值的钱</summary>
            public const String money = "money";

        }
        #endregion
    }

    /// <summary>游戏支付记录接口</summary>
    public partial interface Itgm_record_pay
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>平台</summary>
        Int32 sid { get; set; }

        /// <summary>玩家编号</summary>
        Int64 player_id { get; set; }

        /// <summary>玩家账号</summary>
        String user_code { get; set; }

        /// <summary>玩家名称</summary>
        String player_name { get; set; }

        /// <summary>订单号</summary>
        String order_id { get; set; }

        /// <summary>渠道</summary>
        String channel { get; set; }

        /// <summary>充值类型(0:RMB,1:(1:1),2(1:10)...)</summary>
        Int32 pay_type { get; set; }

        /// <summary>金额</summary>
        Int32 amount { get; set; }

        /// <summary>支付状态</summary>
        Int32 pay_state { get; set; }

        /// <summary>时间</summary>
        Int64 createtime { get; set; }

        /// <summary>充值的钱</summary>
        Int32 money { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}