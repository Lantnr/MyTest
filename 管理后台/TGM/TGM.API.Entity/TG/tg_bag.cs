using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>背包表</summary>
    [DataObject]
    [Description("背包表")]
    [BindIndex("PK__tg_bag__3213E83F11D4A34F", true, "id")]
    [BindTable("tg_bag", Description = "背包表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_bag : Itg_bag
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

        private Int32 _base_id;
        /// <summary>物品基表ID</summary>
        [DisplayName("物品基表ID")]
        [Description("物品基表ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "base_id", "物品基表ID", "0", "int", 10, 0, false)]
        public virtual Int32 base_id
        {
            get { return _base_id; }
            set { if (OnPropertyChanging(__.base_id, value)) { _base_id = value; OnPropertyChanged(__.base_id); } }
        }

        private Int64 _user_id;
        /// <summary>玩家ID</summary>
        [DisplayName("玩家ID")]
        [Description("玩家ID")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "user_id", "玩家ID", "0", "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int32 _type;
        /// <summary>物品类型</summary>
        [DisplayName("物品类型")]
        [Description("物品类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "type", "物品类型", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _equip_type;
        /// <summary>装备类型</summary>
        [DisplayName("装备类型")]
        [Description("装备类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "equip_type", "装备类型", "0", "int", 10, 0, false)]
        public virtual Int32 equip_type
        {
            get { return _equip_type; }
            set { if (OnPropertyChanging(__.equip_type, value)) { _equip_type = value; OnPropertyChanged(__.equip_type); } }
        }

        private Int32 _bind;
        /// <summary>是否绑定</summary>
        [DisplayName("是否绑定")]
        [Description("是否绑定")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "bind", "是否绑定", "0", "int", 10, 0, false)]
        public virtual Int32 bind
        {
            get { return _bind; }
            set { if (OnPropertyChanging(__.bind, value)) { _bind = value; OnPropertyChanged(__.bind); } }
        }

        private Int32 _state;
        /// <summary>装备状态</summary>
        [DisplayName("装备状态")]
        [Description("装备状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "state", "装备状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _count;
        /// <summary>数量</summary>
        [DisplayName("数量")]
        [Description("数量")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "count", "数量", "0", "int", 10, 0, false)]
        public virtual Int32 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
        }

        private Int32 _attribute1_type;
        /// <summary>属性1类型</summary>
        [DisplayName("属性1类型")]
        [Description("属性1类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "attribute1_type", "属性1类型", "0", "int", 10, 0, false)]
        public virtual Int32 attribute1_type
        {
            get { return _attribute1_type; }
            set { if (OnPropertyChanging(__.attribute1_type, value)) { _attribute1_type = value; OnPropertyChanged(__.attribute1_type); } }
        }

        private Double _attribute1_value;
        /// <summary>属性1值</summary>
        [DisplayName("属性1值")]
        [Description("属性1值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(10, "attribute1_value", "属性1值", "0", "float", 53, 0, false)]
        public virtual Double attribute1_value
        {
            get { return _attribute1_value; }
            set { if (OnPropertyChanging(__.attribute1_value, value)) { _attribute1_value = value; OnPropertyChanged(__.attribute1_value); } }
        }

        private Double _attribute1_value_spirit;
        /// <summary>属性1注魂值</summary>
        [DisplayName("属性1注魂值")]
        [Description("属性1注魂值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(11, "attribute1_value_spirit", "属性1注魂值", "0", "float", 53, 0, false)]
        public virtual Double attribute1_value_spirit
        {
            get { return _attribute1_value_spirit; }
            set { if (OnPropertyChanging(__.attribute1_value_spirit, value)) { _attribute1_value_spirit = value; OnPropertyChanged(__.attribute1_value_spirit); } }
        }

        private Int32 _attribute2_type;
        /// <summary>属性2类型</summary>
        [DisplayName("属性2类型")]
        [Description("属性2类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "attribute2_type", "属性2类型", "0", "int", 10, 0, false)]
        public virtual Int32 attribute2_type
        {
            get { return _attribute2_type; }
            set { if (OnPropertyChanging(__.attribute2_type, value)) { _attribute2_type = value; OnPropertyChanged(__.attribute2_type); } }
        }

        private Double _attribute2_value;
        /// <summary>属性2值</summary>
        [DisplayName("属性2值")]
        [Description("属性2值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(13, "attribute2_value", "属性2值", "0", "float", 53, 0, false)]
        public virtual Double attribute2_value
        {
            get { return _attribute2_value; }
            set { if (OnPropertyChanging(__.attribute2_value, value)) { _attribute2_value = value; OnPropertyChanged(__.attribute2_value); } }
        }

        private Double _attribute2_value_spirit;
        /// <summary>属性2注魂值</summary>
        [DisplayName("属性2注魂值")]
        [Description("属性2注魂值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(14, "attribute2_value_spirit", "属性2注魂值", "0", "float", 53, 0, false)]
        public virtual Double attribute2_value_spirit
        {
            get { return _attribute2_value_spirit; }
            set { if (OnPropertyChanging(__.attribute2_value_spirit, value)) { _attribute2_value_spirit = value; OnPropertyChanged(__.attribute2_value_spirit); } }
        }

        private Int32 _attribute3_type;
        /// <summary>属性3类型</summary>
        [DisplayName("属性3类型")]
        [Description("属性3类型")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "attribute3_type", "属性3类型", "0", "int", 10, 0, false)]
        public virtual Int32 attribute3_type
        {
            get { return _attribute3_type; }
            set { if (OnPropertyChanging(__.attribute3_type, value)) { _attribute3_type = value; OnPropertyChanged(__.attribute3_type); } }
        }

        private Double _attribute3_value;
        /// <summary>属性3值</summary>
        [DisplayName("属性3值")]
        [Description("属性3值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(16, "attribute3_value", "属性3值", "0", "float", 53, 0, false)]
        public virtual Double attribute3_value
        {
            get { return _attribute3_value; }
            set { if (OnPropertyChanging(__.attribute3_value, value)) { _attribute3_value = value; OnPropertyChanged(__.attribute3_value); } }
        }

        private Double _attribute3_value_spirit;
        /// <summary>属性3注魂值</summary>
        [DisplayName("属性3注魂值")]
        [Description("属性3注魂值")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(17, "attribute3_value_spirit", "属性3注魂值", "0", "float", 53, 0, false)]
        public virtual Double attribute3_value_spirit
        {
            get { return _attribute3_value_spirit; }
            set { if (OnPropertyChanging(__.attribute3_value_spirit, value)) { _attribute3_value_spirit = value; OnPropertyChanged(__.attribute3_value_spirit); } }
        }

        private Int32 _attribute1_spirit_level;
        /// <summary>属性1铸魂等级</summary>
        [DisplayName("属性1铸魂等级")]
        [Description("属性1铸魂等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "attribute1_spirit_level", "属性1铸魂等级", "0", "int", 10, 0, false)]
        public virtual Int32 attribute1_spirit_level
        {
            get { return _attribute1_spirit_level; }
            set { if (OnPropertyChanging(__.attribute1_spirit_level, value)) { _attribute1_spirit_level = value; OnPropertyChanged(__.attribute1_spirit_level); } }
        }

        private Int32 _attribute1_spirit_value;
        /// <summary>属性1铸魂当前值</summary>
        [DisplayName("属性1铸魂当前值")]
        [Description("属性1铸魂当前值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(19, "attribute1_spirit_value", "属性1铸魂当前值", "0", "int", 10, 0, false)]
        public virtual Int32 attribute1_spirit_value
        {
            get { return _attribute1_spirit_value; }
            set { if (OnPropertyChanging(__.attribute1_spirit_value, value)) { _attribute1_spirit_value = value; OnPropertyChanged(__.attribute1_spirit_value); } }
        }

        private Int32 _attribute1_spirit_lock;
        /// <summary>属性1下一阶是否锁定</summary>
        [DisplayName("属性1下一阶是否锁定")]
        [Description("属性1下一阶是否锁定")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(20, "attribute1_spirit_lock", "属性1下一阶是否锁定", "0", "int", 10, 0, false)]
        public virtual Int32 attribute1_spirit_lock
        {
            get { return _attribute1_spirit_lock; }
            set { if (OnPropertyChanging(__.attribute1_spirit_lock, value)) { _attribute1_spirit_lock = value; OnPropertyChanged(__.attribute1_spirit_lock); } }
        }

        private Int32 _attribute2_spirit_level;
        /// <summary>属性2铸魂等级</summary>
        [DisplayName("属性2铸魂等级")]
        [Description("属性2铸魂等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(21, "attribute2_spirit_level", "属性2铸魂等级", "0", "int", 10, 0, false)]
        public virtual Int32 attribute2_spirit_level
        {
            get { return _attribute2_spirit_level; }
            set { if (OnPropertyChanging(__.attribute2_spirit_level, value)) { _attribute2_spirit_level = value; OnPropertyChanged(__.attribute2_spirit_level); } }
        }

        private Int32 _attribute2_spirit_value;
        /// <summary>属性2铸魂当前值</summary>
        [DisplayName("属性2铸魂当前值")]
        [Description("属性2铸魂当前值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(22, "attribute2_spirit_value", "属性2铸魂当前值", "0", "int", 10, 0, false)]
        public virtual Int32 attribute2_spirit_value
        {
            get { return _attribute2_spirit_value; }
            set { if (OnPropertyChanging(__.attribute2_spirit_value, value)) { _attribute2_spirit_value = value; OnPropertyChanged(__.attribute2_spirit_value); } }
        }

        private Int32 _attribute2_spirit_lock;
        /// <summary>属性2下一阶是否锁定</summary>
        [DisplayName("属性2下一阶是否锁定")]
        [Description("属性2下一阶是否锁定")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(23, "attribute2_spirit_lock", "属性2下一阶是否锁定", "0", "int", 10, 0, false)]
        public virtual Int32 attribute2_spirit_lock
        {
            get { return _attribute2_spirit_lock; }
            set { if (OnPropertyChanging(__.attribute2_spirit_lock, value)) { _attribute2_spirit_lock = value; OnPropertyChanged(__.attribute2_spirit_lock); } }
        }

        private Int32 _attribute3_spirit_level;
        /// <summary>属性3铸魂等级</summary>
        [DisplayName("属性3铸魂等级")]
        [Description("属性3铸魂等级")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(24, "attribute3_spirit_level", "属性3铸魂等级", "0", "int", 10, 0, false)]
        public virtual Int32 attribute3_spirit_level
        {
            get { return _attribute3_spirit_level; }
            set { if (OnPropertyChanging(__.attribute3_spirit_level, value)) { _attribute3_spirit_level = value; OnPropertyChanged(__.attribute3_spirit_level); } }
        }

        private Int32 _attribute3_spirit_value;
        /// <summary>属性3铸魂当前值</summary>
        [DisplayName("属性3铸魂当前值")]
        [Description("属性3铸魂当前值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(25, "attribute3_spirit_value", "属性3铸魂当前值", "0", "int", 10, 0, false)]
        public virtual Int32 attribute3_spirit_value
        {
            get { return _attribute3_spirit_value; }
            set { if (OnPropertyChanging(__.attribute3_spirit_value, value)) { _attribute3_spirit_value = value; OnPropertyChanged(__.attribute3_spirit_value); } }
        }

        private Int32 _attribute3_spirit_lock;
        /// <summary>属性3下一阶是否锁定</summary>
        [DisplayName("属性3下一阶是否锁定")]
        [Description("属性3下一阶是否锁定")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(26, "attribute3_spirit_lock", "属性3下一阶是否锁定", "0", "int", 10, 0, false)]
        public virtual Int32 attribute3_spirit_lock
        {
            get { return _attribute3_spirit_lock; }
            set { if (OnPropertyChanging(__.attribute3_spirit_lock, value)) { _attribute3_spirit_lock = value; OnPropertyChanged(__.attribute3_spirit_lock); } }
        }

        private Int32 _baptize_count;
        /// <summary>装备洗炼次数</summary>
        [DisplayName("装备洗炼次数")]
        [Description("装备洗炼次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(27, "baptize_count", "装备洗炼次数", "0", "int", 10, 0, false)]
        public virtual Int32 baptize_count
        {
            get { return _baptize_count; }
            set { if (OnPropertyChanging(__.baptize_count, value)) { _baptize_count = value; OnPropertyChanged(__.baptize_count); } }
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
                    case __.base_id : return _base_id;
                    case __.user_id : return _user_id;
                    case __.type : return _type;
                    case __.equip_type : return _equip_type;
                    case __.bind : return _bind;
                    case __.state : return _state;
                    case __.count : return _count;
                    case __.attribute1_type : return _attribute1_type;
                    case __.attribute1_value : return _attribute1_value;
                    case __.attribute1_value_spirit : return _attribute1_value_spirit;
                    case __.attribute2_type : return _attribute2_type;
                    case __.attribute2_value : return _attribute2_value;
                    case __.attribute2_value_spirit : return _attribute2_value_spirit;
                    case __.attribute3_type : return _attribute3_type;
                    case __.attribute3_value : return _attribute3_value;
                    case __.attribute3_value_spirit : return _attribute3_value_spirit;
                    case __.attribute1_spirit_level : return _attribute1_spirit_level;
                    case __.attribute1_spirit_value : return _attribute1_spirit_value;
                    case __.attribute1_spirit_lock : return _attribute1_spirit_lock;
                    case __.attribute2_spirit_level : return _attribute2_spirit_level;
                    case __.attribute2_spirit_value : return _attribute2_spirit_value;
                    case __.attribute2_spirit_lock : return _attribute2_spirit_lock;
                    case __.attribute3_spirit_level : return _attribute3_spirit_level;
                    case __.attribute3_spirit_value : return _attribute3_spirit_value;
                    case __.attribute3_spirit_lock : return _attribute3_spirit_lock;
                    case __.baptize_count : return _baptize_count;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.base_id : _base_id = Convert.ToInt32(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.equip_type : _equip_type = Convert.ToInt32(value); break;
                    case __.bind : _bind = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.count : _count = Convert.ToInt32(value); break;
                    case __.attribute1_type : _attribute1_type = Convert.ToInt32(value); break;
                    case __.attribute1_value : _attribute1_value = Convert.ToDouble(value); break;
                    case __.attribute1_value_spirit : _attribute1_value_spirit = Convert.ToDouble(value); break;
                    case __.attribute2_type : _attribute2_type = Convert.ToInt32(value); break;
                    case __.attribute2_value : _attribute2_value = Convert.ToDouble(value); break;
                    case __.attribute2_value_spirit : _attribute2_value_spirit = Convert.ToDouble(value); break;
                    case __.attribute3_type : _attribute3_type = Convert.ToInt32(value); break;
                    case __.attribute3_value : _attribute3_value = Convert.ToDouble(value); break;
                    case __.attribute3_value_spirit : _attribute3_value_spirit = Convert.ToDouble(value); break;
                    case __.attribute1_spirit_level : _attribute1_spirit_level = Convert.ToInt32(value); break;
                    case __.attribute1_spirit_value : _attribute1_spirit_value = Convert.ToInt32(value); break;
                    case __.attribute1_spirit_lock : _attribute1_spirit_lock = Convert.ToInt32(value); break;
                    case __.attribute2_spirit_level : _attribute2_spirit_level = Convert.ToInt32(value); break;
                    case __.attribute2_spirit_value : _attribute2_spirit_value = Convert.ToInt32(value); break;
                    case __.attribute2_spirit_lock : _attribute2_spirit_lock = Convert.ToInt32(value); break;
                    case __.attribute3_spirit_level : _attribute3_spirit_level = Convert.ToInt32(value); break;
                    case __.attribute3_spirit_value : _attribute3_spirit_value = Convert.ToInt32(value); break;
                    case __.attribute3_spirit_lock : _attribute3_spirit_lock = Convert.ToInt32(value); break;
                    case __.baptize_count : _baptize_count = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得背包表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>主键ID</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>物品基表ID</summary>
            public static readonly Field base_id = FindByName(__.base_id);

            ///<summary>玩家ID</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>物品类型</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>装备类型</summary>
            public static readonly Field equip_type = FindByName(__.equip_type);

            ///<summary>是否绑定</summary>
            public static readonly Field bind = FindByName(__.bind);

            ///<summary>装备状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>数量</summary>
            public static readonly Field count = FindByName(__.count);

            ///<summary>属性1类型</summary>
            public static readonly Field attribute1_type = FindByName(__.attribute1_type);

            ///<summary>属性1值</summary>
            public static readonly Field attribute1_value = FindByName(__.attribute1_value);

            ///<summary>属性1注魂值</summary>
            public static readonly Field attribute1_value_spirit = FindByName(__.attribute1_value_spirit);

            ///<summary>属性2类型</summary>
            public static readonly Field attribute2_type = FindByName(__.attribute2_type);

            ///<summary>属性2值</summary>
            public static readonly Field attribute2_value = FindByName(__.attribute2_value);

            ///<summary>属性2注魂值</summary>
            public static readonly Field attribute2_value_spirit = FindByName(__.attribute2_value_spirit);

            ///<summary>属性3类型</summary>
            public static readonly Field attribute3_type = FindByName(__.attribute3_type);

            ///<summary>属性3值</summary>
            public static readonly Field attribute3_value = FindByName(__.attribute3_value);

            ///<summary>属性3注魂值</summary>
            public static readonly Field attribute3_value_spirit = FindByName(__.attribute3_value_spirit);

            ///<summary>属性1铸魂等级</summary>
            public static readonly Field attribute1_spirit_level = FindByName(__.attribute1_spirit_level);

            ///<summary>属性1铸魂当前值</summary>
            public static readonly Field attribute1_spirit_value = FindByName(__.attribute1_spirit_value);

            ///<summary>属性1下一阶是否锁定</summary>
            public static readonly Field attribute1_spirit_lock = FindByName(__.attribute1_spirit_lock);

            ///<summary>属性2铸魂等级</summary>
            public static readonly Field attribute2_spirit_level = FindByName(__.attribute2_spirit_level);

            ///<summary>属性2铸魂当前值</summary>
            public static readonly Field attribute2_spirit_value = FindByName(__.attribute2_spirit_value);

            ///<summary>属性2下一阶是否锁定</summary>
            public static readonly Field attribute2_spirit_lock = FindByName(__.attribute2_spirit_lock);

            ///<summary>属性3铸魂等级</summary>
            public static readonly Field attribute3_spirit_level = FindByName(__.attribute3_spirit_level);

            ///<summary>属性3铸魂当前值</summary>
            public static readonly Field attribute3_spirit_value = FindByName(__.attribute3_spirit_value);

            ///<summary>属性3下一阶是否锁定</summary>
            public static readonly Field attribute3_spirit_lock = FindByName(__.attribute3_spirit_lock);

            ///<summary>装备洗炼次数</summary>
            public static readonly Field baptize_count = FindByName(__.baptize_count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得背包表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>主键ID</summary>
            public const String id = "id";

            ///<summary>物品基表ID</summary>
            public const String base_id = "base_id";

            ///<summary>玩家ID</summary>
            public const String user_id = "user_id";

            ///<summary>物品类型</summary>
            public const String type = "type";

            ///<summary>装备类型</summary>
            public const String equip_type = "equip_type";

            ///<summary>是否绑定</summary>
            public const String bind = "bind";

            ///<summary>装备状态</summary>
            public const String state = "state";

            ///<summary>数量</summary>
            public const String count = "count";

            ///<summary>属性1类型</summary>
            public const String attribute1_type = "attribute1_type";

            ///<summary>属性1值</summary>
            public const String attribute1_value = "attribute1_value";

            ///<summary>属性1注魂值</summary>
            public const String attribute1_value_spirit = "attribute1_value_spirit";

            ///<summary>属性2类型</summary>
            public const String attribute2_type = "attribute2_type";

            ///<summary>属性2值</summary>
            public const String attribute2_value = "attribute2_value";

            ///<summary>属性2注魂值</summary>
            public const String attribute2_value_spirit = "attribute2_value_spirit";

            ///<summary>属性3类型</summary>
            public const String attribute3_type = "attribute3_type";

            ///<summary>属性3值</summary>
            public const String attribute3_value = "attribute3_value";

            ///<summary>属性3注魂值</summary>
            public const String attribute3_value_spirit = "attribute3_value_spirit";

            ///<summary>属性1铸魂等级</summary>
            public const String attribute1_spirit_level = "attribute1_spirit_level";

            ///<summary>属性1铸魂当前值</summary>
            public const String attribute1_spirit_value = "attribute1_spirit_value";

            ///<summary>属性1下一阶是否锁定</summary>
            public const String attribute1_spirit_lock = "attribute1_spirit_lock";

            ///<summary>属性2铸魂等级</summary>
            public const String attribute2_spirit_level = "attribute2_spirit_level";

            ///<summary>属性2铸魂当前值</summary>
            public const String attribute2_spirit_value = "attribute2_spirit_value";

            ///<summary>属性2下一阶是否锁定</summary>
            public const String attribute2_spirit_lock = "attribute2_spirit_lock";

            ///<summary>属性3铸魂等级</summary>
            public const String attribute3_spirit_level = "attribute3_spirit_level";

            ///<summary>属性3铸魂当前值</summary>
            public const String attribute3_spirit_value = "attribute3_spirit_value";

            ///<summary>属性3下一阶是否锁定</summary>
            public const String attribute3_spirit_lock = "attribute3_spirit_lock";

            ///<summary>装备洗炼次数</summary>
            public const String baptize_count = "baptize_count";

        }
        #endregion
    }

    /// <summary>背包表接口</summary>
    public partial interface Itg_bag
    {
        #region 属性
        /// <summary>主键ID</summary>
        Int64 id { get; set; }

        /// <summary>物品基表ID</summary>
        Int32 base_id { get; set; }

        /// <summary>玩家ID</summary>
        Int64 user_id { get; set; }

        /// <summary>物品类型</summary>
        Int32 type { get; set; }

        /// <summary>装备类型</summary>
        Int32 equip_type { get; set; }

        /// <summary>是否绑定</summary>
        Int32 bind { get; set; }

        /// <summary>装备状态</summary>
        Int32 state { get; set; }

        /// <summary>数量</summary>
        Int32 count { get; set; }

        /// <summary>属性1类型</summary>
        Int32 attribute1_type { get; set; }

        /// <summary>属性1值</summary>
        Double attribute1_value { get; set; }

        /// <summary>属性1注魂值</summary>
        Double attribute1_value_spirit { get; set; }

        /// <summary>属性2类型</summary>
        Int32 attribute2_type { get; set; }

        /// <summary>属性2值</summary>
        Double attribute2_value { get; set; }

        /// <summary>属性2注魂值</summary>
        Double attribute2_value_spirit { get; set; }

        /// <summary>属性3类型</summary>
        Int32 attribute3_type { get; set; }

        /// <summary>属性3值</summary>
        Double attribute3_value { get; set; }

        /// <summary>属性3注魂值</summary>
        Double attribute3_value_spirit { get; set; }

        /// <summary>属性1铸魂等级</summary>
        Int32 attribute1_spirit_level { get; set; }

        /// <summary>属性1铸魂当前值</summary>
        Int32 attribute1_spirit_value { get; set; }

        /// <summary>属性1下一阶是否锁定</summary>
        Int32 attribute1_spirit_lock { get; set; }

        /// <summary>属性2铸魂等级</summary>
        Int32 attribute2_spirit_level { get; set; }

        /// <summary>属性2铸魂当前值</summary>
        Int32 attribute2_spirit_value { get; set; }

        /// <summary>属性2下一阶是否锁定</summary>
        Int32 attribute2_spirit_lock { get; set; }

        /// <summary>属性3铸魂等级</summary>
        Int32 attribute3_spirit_level { get; set; }

        /// <summary>属性3铸魂当前值</summary>
        Int32 attribute3_spirit_value { get; set; }

        /// <summary>属性3下一阶是否锁定</summary>
        Int32 attribute3_spirit_lock { get; set; }

        /// <summary>装备洗炼次数</summary>
        Int32 baptize_count { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}