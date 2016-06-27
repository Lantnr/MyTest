using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>合战武将表</summary>
    [DataObject]
    [Description("合战武将表")]
    [BindIndex("PK__tg_war_r__3213E83F61316BF4", true, "id")]
    [BindTable("tg_war_role", Description = "合战武将表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_war_role : Itg_war_role
    {
        #region 属性
        private Int64 _id;
        /// <summary>编号Id</summary>
        [DisplayName("编号Id")]
        [Description("编号Id")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "编号Id", null, "bigint", 19, 0, false)]
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
        [BindColumn(2, "user_id", "", null, "bigint", 19, 0, false)]
        public virtual Int64 user_id
        {
            get { return _user_id; }
            set { if (OnPropertyChanging(__.user_id, value)) { _user_id = value; OnPropertyChanged(__.user_id); } }
        }

        private Int64 _rid;
        /// <summary>武将主Id(或备大将基表id)</summary>
        [DisplayName("武将主Id或备大将基表id")]
        [Description("武将主Id(或备大将基表id)")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(3, "rid", "武将主Id(或备大将基表id)", null, "bigint", 19, 0, false)]
        public virtual Int64 rid
        {
            get { return _rid; }
            set { if (OnPropertyChanging(__.rid, value)) { _rid = value; OnPropertyChanged(__.rid); } }
        }

        private Int32 _type;
        /// <summary>武将类型(0:玩家武将,1:备大将)</summary>
        [DisplayName("武将类型0:玩家武将,1:备大将")]
        [Description("武将类型(0:玩家武将,1:备大将)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "type", "武将类型(0:玩家武将,1:备大将)", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private Int32 _state;
        /// <summary>合战武将状态(破坏,放火,建交,徵兵等)</summary>
        [DisplayName("合战武将状态破坏,放火,建交,徵兵等")]
        [Description("合战武将状态(破坏,放火,建交,徵兵等)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "state", "合战武将状态(破坏,放火,建交,徵兵等)", null, "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _station;
        /// <summary>武将所在据点</summary>
        [DisplayName("武将所在据点")]
        [Description("武将所在据点")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "station", "武将所在据点", null, "int", 10, 0, false)]
        public virtual Int32 station
        {
            get { return _station; }
            set { if (OnPropertyChanging(__.station, value)) { _station = value; OnPropertyChanged(__.station); } }
        }

        private Int32 _resource;
        /// <summary>半小时的产出资源</summary>
        [DisplayName("半小时的产出资源")]
        [Description("半小时的产出资源")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "resource", "半小时的产出资源", null, "int", 10, 0, false)]
        public virtual Int32 resource
        {
            get { return _resource; }
            set { if (OnPropertyChanging(__.resource, value)) { _resource = value; OnPropertyChanged(__.resource); } }
        }

        private Int32 _count;
        /// <summary>剩余开发次数</summary>
        [DisplayName("剩余开发次数")]
        [Description("剩余开发次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "count", "剩余开发次数", "0", "int", 10, 0, false)]
        public virtual Int32 count
        {
            get { return _count; }
            set { if (OnPropertyChanging(__.count, value)) { _count = value; OnPropertyChanged(__.count); } }
        }

        private Int32 _total_count;
        /// <summary>总开发次数</summary>
        [DisplayName("总开发次数")]
        [Description("总开发次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "total_count", "总开发次数", "0", "int", 10, 0, false)]
        public virtual Int32 total_count
        {
            get { return _total_count; }
            set { if (OnPropertyChanging(__.total_count, value)) { _total_count = value; OnPropertyChanged(__.total_count); } }
        }

        private Int64 _state_end_time;
        /// <summary>状态结束时间</summary>
        [DisplayName("状态结束时间")]
        [Description("状态结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(10, "state_end_time", "状态结束时间", null, "bigint", 19, 0, false)]
        public virtual Int64 state_end_time
        {
            get { return _state_end_time; }
            set { if (OnPropertyChanging(__.state_end_time, value)) { _state_end_time = value; OnPropertyChanged(__.state_end_time); } }
        }

        private Int32 _army_id;
        /// <summary>兵种基表编号</summary>
        [DisplayName("兵种基表编号")]
        [Description("兵种基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "army_id", "兵种基表编号", null, "int", 10, 0, false)]
        public virtual Int32 army_id
        {
            get { return _army_id; }
            set { if (OnPropertyChanging(__.army_id, value)) { _army_id = value; OnPropertyChanged(__.army_id); } }
        }

        private Int32 _army_horse;
        /// <summary>马匹</summary>
        [DisplayName("马匹")]
        [Description("马匹")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "army_horse", "马匹", "0", "int", 10, 0, false)]
        public virtual Int32 army_horse
        {
            get { return _army_horse; }
            set { if (OnPropertyChanging(__.army_horse, value)) { _army_horse = value; OnPropertyChanged(__.army_horse); } }
        }

        private Int32 _army_gun;
        /// <summary>铁炮</summary>
        [DisplayName("铁炮")]
        [Description("铁炮")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "army_gun", "铁炮", "0", "int", 10, 0, false)]
        public virtual Int32 army_gun
        {
            get { return _army_gun; }
            set { if (OnPropertyChanging(__.army_gun, value)) { _army_gun = value; OnPropertyChanged(__.army_gun); } }
        }

        private Int32 _army_soldier;
        /// <summary>士兵数(足轻数)</summary>
        [DisplayName("士兵数足轻数")]
        [Description("士兵数(足轻数)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "army_soldier", "士兵数(足轻数)", "0", "int", 10, 0, false)]
        public virtual Int32 army_soldier
        {
            get { return _army_soldier; }
            set { if (OnPropertyChanging(__.army_soldier, value)) { _army_soldier = value; OnPropertyChanged(__.army_soldier); } }
        }

        private Int32 _army_morale;
        /// <summary>士气</summary>
        [DisplayName("士气")]
        [Description("士气")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "army_morale", "士气", "0", "int", 10, 0, false)]
        public virtual Int32 army_morale
        {
            get { return _army_morale; }
            set { if (OnPropertyChanging(__.army_morale, value)) { _army_morale = value; OnPropertyChanged(__.army_morale); } }
        }

        private Double _army_funds;
        /// <summary>军资金</summary>
        [DisplayName("军资金")]
        [Description("军资金")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(16, "army_funds", "军资金", "0", "float", 53, 0, false)]
        public virtual Double army_funds
        {
            get { return _army_funds; }
            set { if (OnPropertyChanging(__.army_funds, value)) { _army_funds = value; OnPropertyChanged(__.army_funds); } }
        }

        private Int32 _army_kuwu;
        /// <summary>苦无</summary>
        [DisplayName("苦无")]
        [Description("苦无")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "army_kuwu", "苦无", "0", "int", 10, 0, false)]
        public virtual Int32 army_kuwu
        {
            get { return _army_kuwu; }
            set { if (OnPropertyChanging(__.army_kuwu, value)) { _army_kuwu = value; OnPropertyChanged(__.army_kuwu); } }
        }

        private Int32 _army_foods;
        /// <summary>兵娘</summary>
        [DisplayName("兵娘")]
        [Description("兵娘")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "army_foods", "兵娘", "0", "int", 10, 0, false)]
        public virtual Int32 army_foods
        {
            get { return _army_foods; }
            set { if (OnPropertyChanging(__.army_foods, value)) { _army_foods = value; OnPropertyChanged(__.army_foods); } }
        }

        private Int32 _army_razor;
        /// <summary>薙刀</summary>
        [DisplayName("薙刀")]
        [Description("薙刀")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(19, "army_razor", "薙刀", "0", "int", 10, 0, false)]
        public virtual Int32 army_razor
        {
            get { return _army_razor; }
            set { if (OnPropertyChanging(__.army_razor, value)) { _army_razor = value; OnPropertyChanged(__.army_razor); } }
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
                    case __.rid : return _rid;
                    case __.type : return _type;
                    case __.state : return _state;
                    case __.station : return _station;
                    case __.resource : return _resource;
                    case __.count : return _count;
                    case __.total_count : return _total_count;
                    case __.state_end_time : return _state_end_time;
                    case __.army_id : return _army_id;
                    case __.army_horse : return _army_horse;
                    case __.army_gun : return _army_gun;
                    case __.army_soldier : return _army_soldier;
                    case __.army_morale : return _army_morale;
                    case __.army_funds : return _army_funds;
                    case __.army_kuwu : return _army_kuwu;
                    case __.army_foods : return _army_foods;
                    case __.army_razor : return _army_razor;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.rid : _rid = Convert.ToInt64(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.station : _station = Convert.ToInt32(value); break;
                    case __.resource : _resource = Convert.ToInt32(value); break;
                    case __.count : _count = Convert.ToInt32(value); break;
                    case __.total_count : _total_count = Convert.ToInt32(value); break;
                    case __.state_end_time : _state_end_time = Convert.ToInt64(value); break;
                    case __.army_id : _army_id = Convert.ToInt32(value); break;
                    case __.army_horse : _army_horse = Convert.ToInt32(value); break;
                    case __.army_gun : _army_gun = Convert.ToInt32(value); break;
                    case __.army_soldier : _army_soldier = Convert.ToInt32(value); break;
                    case __.army_morale : _army_morale = Convert.ToInt32(value); break;
                    case __.army_funds : _army_funds = Convert.ToDouble(value); break;
                    case __.army_kuwu : _army_kuwu = Convert.ToInt32(value); break;
                    case __.army_foods : _army_foods = Convert.ToInt32(value); break;
                    case __.army_razor : _army_razor = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得合战武将表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号Id</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary></summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>武将主Id(或备大将基表id)</summary>
            public static readonly Field rid = FindByName(__.rid);

            ///<summary>武将类型(0:玩家武将,1:备大将)</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>合战武将状态(破坏,放火,建交,徵兵等)</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>武将所在据点</summary>
            public static readonly Field station = FindByName(__.station);

            ///<summary>半小时的产出资源</summary>
            public static readonly Field resource = FindByName(__.resource);

            ///<summary>剩余开发次数</summary>
            public static readonly Field count = FindByName(__.count);

            ///<summary>总开发次数</summary>
            public static readonly Field total_count = FindByName(__.total_count);

            ///<summary>状态结束时间</summary>
            public static readonly Field state_end_time = FindByName(__.state_end_time);

            ///<summary>兵种基表编号</summary>
            public static readonly Field army_id = FindByName(__.army_id);

            ///<summary>马匹</summary>
            public static readonly Field army_horse = FindByName(__.army_horse);

            ///<summary>铁炮</summary>
            public static readonly Field army_gun = FindByName(__.army_gun);

            ///<summary>士兵数(足轻数)</summary>
            public static readonly Field army_soldier = FindByName(__.army_soldier);

            ///<summary>士气</summary>
            public static readonly Field army_morale = FindByName(__.army_morale);

            ///<summary>军资金</summary>
            public static readonly Field army_funds = FindByName(__.army_funds);

            ///<summary>苦无</summary>
            public static readonly Field army_kuwu = FindByName(__.army_kuwu);

            ///<summary>兵娘</summary>
            public static readonly Field army_foods = FindByName(__.army_foods);

            ///<summary>薙刀</summary>
            public static readonly Field army_razor = FindByName(__.army_razor);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得合战武将表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号Id</summary>
            public const String id = "id";

            ///<summary></summary>
            public const String user_id = "user_id";

            ///<summary>武将主Id(或备大将基表id)</summary>
            public const String rid = "rid";

            ///<summary>武将类型(0:玩家武将,1:备大将)</summary>
            public const String type = "type";

            ///<summary>合战武将状态(破坏,放火,建交,徵兵等)</summary>
            public const String state = "state";

            ///<summary>武将所在据点</summary>
            public const String station = "station";

            ///<summary>半小时的产出资源</summary>
            public const String resource = "resource";

            ///<summary>剩余开发次数</summary>
            public const String count = "count";

            ///<summary>总开发次数</summary>
            public const String total_count = "total_count";

            ///<summary>状态结束时间</summary>
            public const String state_end_time = "state_end_time";

            ///<summary>兵种基表编号</summary>
            public const String army_id = "army_id";

            ///<summary>马匹</summary>
            public const String army_horse = "army_horse";

            ///<summary>铁炮</summary>
            public const String army_gun = "army_gun";

            ///<summary>士兵数(足轻数)</summary>
            public const String army_soldier = "army_soldier";

            ///<summary>士气</summary>
            public const String army_morale = "army_morale";

            ///<summary>军资金</summary>
            public const String army_funds = "army_funds";

            ///<summary>苦无</summary>
            public const String army_kuwu = "army_kuwu";

            ///<summary>兵娘</summary>
            public const String army_foods = "army_foods";

            ///<summary>薙刀</summary>
            public const String army_razor = "army_razor";

        }
        #endregion
    }

    /// <summary>合战武将表接口</summary>
    public partial interface Itg_war_role
    {
        #region 属性
        /// <summary>编号Id</summary>
        Int64 id { get; set; }

        /// <summary></summary>
        Int64 user_id { get; set; }

        /// <summary>武将主Id(或备大将基表id)</summary>
        Int64 rid { get; set; }

        /// <summary>武将类型(0:玩家武将,1:备大将)</summary>
        Int32 type { get; set; }

        /// <summary>合战武将状态(破坏,放火,建交,徵兵等)</summary>
        Int32 state { get; set; }

        /// <summary>武将所在据点</summary>
        Int32 station { get; set; }

        /// <summary>半小时的产出资源</summary>
        Int32 resource { get; set; }

        /// <summary>剩余开发次数</summary>
        Int32 count { get; set; }

        /// <summary>总开发次数</summary>
        Int32 total_count { get; set; }

        /// <summary>状态结束时间</summary>
        Int64 state_end_time { get; set; }

        /// <summary>兵种基表编号</summary>
        Int32 army_id { get; set; }

        /// <summary>马匹</summary>
        Int32 army_horse { get; set; }

        /// <summary>铁炮</summary>
        Int32 army_gun { get; set; }

        /// <summary>士兵数(足轻数)</summary>
        Int32 army_soldier { get; set; }

        /// <summary>士气</summary>
        Int32 army_morale { get; set; }

        /// <summary>军资金</summary>
        Double army_funds { get; set; }

        /// <summary>苦无</summary>
        Int32 army_kuwu { get; set; }

        /// <summary>兵娘</summary>
        Int32 army_foods { get; set; }

        /// <summary>薙刀</summary>
        Int32 army_razor { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}