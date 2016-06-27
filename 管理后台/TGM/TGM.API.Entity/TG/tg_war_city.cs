using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>据点表</summary>
    [DataObject]
    [Description("据点表")]
    [BindIndex("PK__tg_war_c__3213E83F0A688BB1", true, "id")]
    [BindTable("tg_war_city", Description = "据点表", ConnName = "TGG", DbType = DatabaseType.SqlServer)]
    public partial class tg_war_city : Itg_war_city
    {
        #region 属性
        private Int64 _id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 19)]
        [BindColumn(1, "id", "编号", null, "bigint", 19, 0, false)]
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

        private Int32 _base_id;
        /// <summary>据点基表编号</summary>
        [DisplayName("据点基表编号")]
        [Description("据点基表编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "base_id", "据点基表编号", "0", "int", 10, 0, false)]
        public virtual Int32 base_id
        {
            get { return _base_id; }
            set { if (OnPropertyChanging(__.base_id, value)) { _base_id = value; OnPropertyChanged(__.base_id); } }
        }

        private Int32 _state;
        /// <summary>据点状态</summary>
        [DisplayName("据点状态")]
        [Description("据点状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "state", "据点状态", "0", "int", 10, 0, false)]
        public virtual Int32 state
        {
            get { return _state; }
            set { if (OnPropertyChanging(__.state, value)) { _state = value; OnPropertyChanged(__.state); } }
        }

        private Int32 _type;
        /// <summary>据点类型(0:支城1:本城)</summary>
        [DisplayName("据点类型0:支城1:本城")]
        [Description("据点类型(0:支城1:本城)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "type", "据点类型(0:支城1:本城)", "0", "int", 10, 0, false)]
        public virtual Int32 type
        {
            get { return _type; }
            set { if (OnPropertyChanging(__.type, value)) { _type = value; OnPropertyChanged(__.type); } }
        }

        private String _name;
        /// <summary>据点名称</summary>
        [DisplayName("据点名称")]
        [Description("据点名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(6, "name", "据点名称", "0", "varchar(50)", 0, 0, false)]
        public virtual String name
        {
            get { return _name; }
            set { if (OnPropertyChanging(__.name, value)) { _name = value; OnPropertyChanged(__.name); } }
        }

        private Int32 _size;
        /// <summary>规模</summary>
        [DisplayName("规模")]
        [Description("规模")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "size", "规模", "0", "int", 10, 0, false)]
        public virtual Int32 size
        {
            get { return _size; }
            set { if (OnPropertyChanging(__.size, value)) { _size = value; OnPropertyChanged(__.size); } }
        }

        private Double _boom;
        /// <summary>繁荣度</summary>
        [DisplayName("繁荣度")]
        [Description("繁荣度")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(8, "boom", "繁荣度", "0", "float", 53, 0, false)]
        public virtual Double boom
        {
            get { return _boom; }
            set { if (OnPropertyChanging(__.boom, value)) { _boom = value; OnPropertyChanged(__.boom); } }
        }

        private Double _strong;
        /// <summary>耐久度</summary>
        [DisplayName("耐久度")]
        [Description("耐久度")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(9, "strong", "耐久度", "0", "float", 53, 0, false)]
        public virtual Double strong
        {
            get { return _strong; }
            set { if (OnPropertyChanging(__.strong, value)) { _strong = value; OnPropertyChanged(__.strong); } }
        }

        private Double _peace;
        /// <summary>治安</summary>
        [DisplayName("治安")]
        [Description("治安")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(10, "peace", "治安", "0", "float", 53, 0, false)]
        public virtual Double peace
        {
            get { return _peace; }
            set { if (OnPropertyChanging(__.peace, value)) { _peace = value; OnPropertyChanged(__.peace); } }
        }

        private Int64 _guard_time;
        /// <summary>保护时间</summary>
        [DisplayName("保护时间")]
        [Description("保护时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(11, "guard_time", "保护时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 guard_time
        {
            get { return _guard_time; }
            set { if (OnPropertyChanging(__.guard_time, value)) { _guard_time = value; OnPropertyChanged(__.guard_time); } }
        }

        private Int32 _module_number;
        /// <summary>所属模块</summary>
        [DisplayName("所属模块")]
        [Description("所属模块")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "module_number", "所属模块", null, "int", 10, 0, false)]
        public virtual Int32 module_number
        {
            get { return _module_number; }
            set { if (OnPropertyChanging(__.module_number, value)) { _module_number = value; OnPropertyChanged(__.module_number); } }
        }

        private Int32 _defense_id;
        /// <summary></summary>
        [DisplayName("ID3")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "defense_id", "", "0", "int", 10, 0, false)]
        public virtual Int32 defense_id
        {
            get { return _defense_id; }
            set { if (OnPropertyChanging(__.defense_id, value)) { _defense_id = value; OnPropertyChanged(__.defense_id); } }
        }

        private Int64 _plan_1;
        /// <summary>防守方案1</summary>
        [DisplayName("防守方案1")]
        [Description("防守方案1")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(14, "plan_1", "防守方案1", "0", "bigint", 19, 0, false)]
        public virtual Int64 plan_1
        {
            get { return _plan_1; }
            set { if (OnPropertyChanging(__.plan_1, value)) { _plan_1 = value; OnPropertyChanged(__.plan_1); } }
        }

        private Int64 _plan_2;
        /// <summary>防守方案2</summary>
        [DisplayName("防守方案2")]
        [Description("防守方案2")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(15, "plan_2", "防守方案2", "0", "bigint", 19, 0, false)]
        public virtual Int64 plan_2
        {
            get { return _plan_2; }
            set { if (OnPropertyChanging(__.plan_2, value)) { _plan_2 = value; OnPropertyChanged(__.plan_2); } }
        }

        private Int64 _plan_3;
        /// <summary>防守方案3</summary>
        [DisplayName("防守方案3")]
        [Description("防守方案3")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(16, "plan_3", "防守方案3", "0", "bigint", 19, 0, false)]
        public virtual Int64 plan_3
        {
            get { return _plan_3; }
            set { if (OnPropertyChanging(__.plan_3, value)) { _plan_3 = value; OnPropertyChanged(__.plan_3); } }
        }

        private Int64 _time;
        /// <summary>筑城时间</summary>
        [DisplayName("筑城时间")]
        [Description("筑城时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(17, "time", "筑城时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 time
        {
            get { return _time; }
            set { if (OnPropertyChanging(__.time, value)) { _time = value; OnPropertyChanged(__.time); } }
        }

        private Int32 _own;
        /// <summary>当前据点占有度</summary>
        [DisplayName("当前据点占有度")]
        [Description("当前据点占有度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "own", "当前据点占有度", "0", "int", 10, 0, false)]
        public virtual Int32 own
        {
            get { return _own; }
            set { if (OnPropertyChanging(__.own, value)) { _own = value; OnPropertyChanged(__.own); } }
        }

        private Int64 _fire_time;
        /// <summary>放火结束时间</summary>
        [DisplayName("放火结束时间")]
        [Description("放火结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(19, "fire_time", "放火结束时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 fire_time
        {
            get { return _fire_time; }
            set { if (OnPropertyChanging(__.fire_time, value)) { _fire_time = value; OnPropertyChanged(__.fire_time); } }
        }

        private Int64 _destroy_time;
        /// <summary>破坏结束时间</summary>
        [DisplayName("破坏结束时间")]
        [Description("破坏结束时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(20, "destroy_time", "破坏结束时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 destroy_time
        {
            get { return _destroy_time; }
            set { if (OnPropertyChanging(__.destroy_time, value)) { _destroy_time = value; OnPropertyChanged(__.destroy_time); } }
        }

        private Int32 _ownership_type;
        /// <summary></summary>
        [DisplayName("Type1")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(21, "ownership_type", "", "0", "int", 10, 0, false)]
        public virtual Int32 ownership_type
        {
            get { return _ownership_type; }
            set { if (OnPropertyChanging(__.ownership_type, value)) { _ownership_type = value; OnPropertyChanged(__.ownership_type); } }
        }

        private Int32 _res_soldier;
        /// <summary>士兵(足轻)</summary>
        [DisplayName("士兵足轻")]
        [Description("士兵(足轻)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(22, "res_soldier", "士兵(足轻)", "0", "int", 10, 0, false)]
        public virtual Int32 res_soldier
        {
            get { return _res_soldier; }
            set { if (OnPropertyChanging(__.res_soldier, value)) { _res_soldier = value; OnPropertyChanged(__.res_soldier); } }
        }

        private Double _res_funds;
        /// <summary>军资金</summary>
        [DisplayName("军资金")]
        [Description("军资金")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(23, "res_funds", "军资金", "0", "float", 53, 0, false)]
        public virtual Double res_funds
        {
            get { return _res_funds; }
            set { if (OnPropertyChanging(__.res_funds, value)) { _res_funds = value; OnPropertyChanged(__.res_funds); } }
        }

        private Int32 _res_foods;
        /// <summary>军粮</summary>
        [DisplayName("军粮")]
        [Description("军粮")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(24, "res_foods", "军粮", "0", "int", 10, 0, false)]
        public virtual Int32 res_foods
        {
            get { return _res_foods; }
            set { if (OnPropertyChanging(__.res_foods, value)) { _res_foods = value; OnPropertyChanged(__.res_foods); } }
        }

        private Int32 _res_horse;
        /// <summary>马匹</summary>
        [DisplayName("马匹")]
        [Description("马匹")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(25, "res_horse", "马匹", "0", "int", 10, 0, false)]
        public virtual Int32 res_horse
        {
            get { return _res_horse; }
            set { if (OnPropertyChanging(__.res_horse, value)) { _res_horse = value; OnPropertyChanged(__.res_horse); } }
        }

        private Int32 _res_gun;
        /// <summary>铁炮</summary>
        [DisplayName("铁炮")]
        [Description("铁炮")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(26, "res_gun", "铁炮", "0", "int", 10, 0, false)]
        public virtual Int32 res_gun
        {
            get { return _res_gun; }
            set { if (OnPropertyChanging(__.res_gun, value)) { _res_gun = value; OnPropertyChanged(__.res_gun); } }
        }

        private Int32 _res_razor;
        /// <summary>薙刀</summary>
        [DisplayName("薙刀")]
        [Description("薙刀")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(27, "res_razor", "薙刀", "0", "int", 10, 0, false)]
        public virtual Int32 res_razor
        {
            get { return _res_razor; }
            set { if (OnPropertyChanging(__.res_razor, value)) { _res_razor = value; OnPropertyChanged(__.res_razor); } }
        }

        private Int32 _res_kuwu;
        /// <summary>苦无</summary>
        [DisplayName("苦无")]
        [Description("苦无")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(28, "res_kuwu", "苦无", "0", "int", 10, 0, false)]
        public virtual Int32 res_kuwu
        {
            get { return _res_kuwu; }
            set { if (OnPropertyChanging(__.res_kuwu, value)) { _res_kuwu = value; OnPropertyChanged(__.res_kuwu); } }
        }

        private Int32 _res_use_soldier;
        /// <summary>以配置士兵(足轻)</summary>
        [DisplayName("以配置士兵足轻")]
        [Description("以配置士兵(足轻)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(29, "res_use_soldier", "以配置士兵(足轻)", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_soldier
        {
            get { return _res_use_soldier; }
            set { if (OnPropertyChanging(__.res_use_soldier, value)) { _res_use_soldier = value; OnPropertyChanged(__.res_use_soldier); } }
        }

        private Double _res_use_funds;
        /// <summary>以配置军资金</summary>
        [DisplayName("以配置军资金")]
        [Description("以配置军资金")]
        [DataObjectField(false, false, false, 53)]
        [BindColumn(30, "res_use_funds", "以配置军资金", "0", "float", 53, 0, false)]
        public virtual Double res_use_funds
        {
            get { return _res_use_funds; }
            set { if (OnPropertyChanging(__.res_use_funds, value)) { _res_use_funds = value; OnPropertyChanged(__.res_use_funds); } }
        }

        private Int32 _res_use_foods;
        /// <summary>以配置军粮</summary>
        [DisplayName("以配置军粮")]
        [Description("以配置军粮")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(31, "res_use_foods", "以配置军粮", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_foods
        {
            get { return _res_use_foods; }
            set { if (OnPropertyChanging(__.res_use_foods, value)) { _res_use_foods = value; OnPropertyChanged(__.res_use_foods); } }
        }

        private Int32 _res_use_horse;
        /// <summary>以配置马匹</summary>
        [DisplayName("以配置马匹")]
        [Description("以配置马匹")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(32, "res_use_horse", "以配置马匹", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_horse
        {
            get { return _res_use_horse; }
            set { if (OnPropertyChanging(__.res_use_horse, value)) { _res_use_horse = value; OnPropertyChanged(__.res_use_horse); } }
        }

        private Int32 _res_use_gun;
        /// <summary>以配置铁炮</summary>
        [DisplayName("以配置铁炮")]
        [Description("以配置铁炮")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(33, "res_use_gun", "以配置铁炮", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_gun
        {
            get { return _res_use_gun; }
            set { if (OnPropertyChanging(__.res_use_gun, value)) { _res_use_gun = value; OnPropertyChanged(__.res_use_gun); } }
        }

        private Int32 _res_use_razor;
        /// <summary>以配置薙刀</summary>
        [DisplayName("以配置薙刀")]
        [Description("以配置薙刀")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(34, "res_use_razor", "以配置薙刀", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_razor
        {
            get { return _res_use_razor; }
            set { if (OnPropertyChanging(__.res_use_razor, value)) { _res_use_razor = value; OnPropertyChanged(__.res_use_razor); } }
        }

        private Int32 _res_use_kuwu;
        /// <summary>以配置苦无</summary>
        [DisplayName("以配置苦无")]
        [Description("以配置苦无")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(35, "res_use_kuwu", "以配置苦无", "0", "int", 10, 0, false)]
        public virtual Int32 res_use_kuwu
        {
            get { return _res_use_kuwu; }
            set { if (OnPropertyChanging(__.res_use_kuwu, value)) { _res_use_kuwu = value; OnPropertyChanged(__.res_use_kuwu); } }
        }

        private Int32 _res_morale;
        /// <summary>士气值</summary>
        [DisplayName("士气值")]
        [Description("士气值")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(36, "res_morale", "士气值", "0", "int", 10, 0, false)]
        public virtual Int32 res_morale
        {
            get { return _res_morale; }
            set { if (OnPropertyChanging(__.res_morale, value)) { _res_morale = value; OnPropertyChanged(__.res_morale); } }
        }

        private Int32 _interior_bar;
        /// <summary>内政解锁栏</summary>
        [DisplayName("内政解锁栏")]
        [Description("内政解锁栏")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(37, "interior_bar", "内政解锁栏", "0", "int", 10, 0, false)]
        public virtual Int32 interior_bar
        {
            get { return _interior_bar; }
            set { if (OnPropertyChanging(__.interior_bar, value)) { _interior_bar = value; OnPropertyChanged(__.interior_bar); } }
        }

        private Int32 _levy_bar;
        /// <summary>徵兵解锁栏</summary>
        [DisplayName("徵兵解锁栏")]
        [Description("徵兵解锁栏")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(38, "levy_bar", "徵兵解锁栏", "0", "int", 10, 0, false)]
        public virtual Int32 levy_bar
        {
            get { return _levy_bar; }
            set { if (OnPropertyChanging(__.levy_bar, value)) { _levy_bar = value; OnPropertyChanged(__.levy_bar); } }
        }

        private Int32 _train_bar;
        /// <summary>训练解锁栏</summary>
        [DisplayName("训练解锁栏")]
        [Description("训练解锁栏")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(39, "train_bar", "训练解锁栏", "0", "int", 10, 0, false)]
        public virtual Int32 train_bar
        {
            get { return _train_bar; }
            set { if (OnPropertyChanging(__.train_bar, value)) { _train_bar = value; OnPropertyChanged(__.train_bar); } }
        }

        private Int32 _residence;
        /// <summary>驻扎人数</summary>
        [DisplayName("驻扎人数")]
        [Description("驻扎人数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(40, "residence", "驻扎人数", "0", "int", 10, 0, false)]
        public virtual Int32 residence
        {
            get { return _residence; }
            set { if (OnPropertyChanging(__.residence, value)) { _residence = value; OnPropertyChanged(__.residence); } }
        }

        private Int32 _destroy_boom;
        /// <summary>破坏扣除繁荣度</summary>
        [DisplayName("破坏扣除繁荣度")]
        [Description("破坏扣除繁荣度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(41, "destroy_boom", "破坏扣除繁荣度", "0", "int", 10, 0, false)]
        public virtual Int32 destroy_boom
        {
            get { return _destroy_boom; }
            set { if (OnPropertyChanging(__.destroy_boom, value)) { _destroy_boom = value; OnPropertyChanged(__.destroy_boom); } }
        }

        private Int32 _destroy_strong;
        /// <summary>破坏扣除耐久度</summary>
        [DisplayName("破坏扣除耐久度")]
        [Description("破坏扣除耐久度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(42, "destroy_strong", "破坏扣除耐久度", "0", "int", 10, 0, false)]
        public virtual Int32 destroy_strong
        {
            get { return _destroy_strong; }
            set { if (OnPropertyChanging(__.destroy_strong, value)) { _destroy_strong = value; OnPropertyChanged(__.destroy_strong); } }
        }

        private Int32 _destroy_peace;
        /// <summary>破坏扣除治安</summary>
        [DisplayName("破坏扣除治安")]
        [Description("破坏扣除治安")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(43, "destroy_peace", "破坏扣除治安", "0", "int", 10, 0, false)]
        public virtual Int32 destroy_peace
        {
            get { return _destroy_peace; }
            set { if (OnPropertyChanging(__.destroy_peace, value)) { _destroy_peace = value; OnPropertyChanged(__.destroy_peace); } }
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
                    case __.base_id : return _base_id;
                    case __.state : return _state;
                    case __.type : return _type;
                    case __.name : return _name;
                    case __.size : return _size;
                    case __.boom : return _boom;
                    case __.strong : return _strong;
                    case __.peace : return _peace;
                    case __.guard_time : return _guard_time;
                    case __.module_number : return _module_number;
                    case __.defense_id : return _defense_id;
                    case __.plan_1 : return _plan_1;
                    case __.plan_2 : return _plan_2;
                    case __.plan_3 : return _plan_3;
                    case __.time : return _time;
                    case __.own : return _own;
                    case __.fire_time : return _fire_time;
                    case __.destroy_time : return _destroy_time;
                    case __.ownership_type : return _ownership_type;
                    case __.res_soldier : return _res_soldier;
                    case __.res_funds : return _res_funds;
                    case __.res_foods : return _res_foods;
                    case __.res_horse : return _res_horse;
                    case __.res_gun : return _res_gun;
                    case __.res_razor : return _res_razor;
                    case __.res_kuwu : return _res_kuwu;
                    case __.res_use_soldier : return _res_use_soldier;
                    case __.res_use_funds : return _res_use_funds;
                    case __.res_use_foods : return _res_use_foods;
                    case __.res_use_horse : return _res_use_horse;
                    case __.res_use_gun : return _res_use_gun;
                    case __.res_use_razor : return _res_use_razor;
                    case __.res_use_kuwu : return _res_use_kuwu;
                    case __.res_morale : return _res_morale;
                    case __.interior_bar : return _interior_bar;
                    case __.levy_bar : return _levy_bar;
                    case __.train_bar : return _train_bar;
                    case __.residence : return _residence;
                    case __.destroy_boom : return _destroy_boom;
                    case __.destroy_strong : return _destroy_strong;
                    case __.destroy_peace : return _destroy_peace;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.base_id : _base_id = Convert.ToInt32(value); break;
                    case __.state : _state = Convert.ToInt32(value); break;
                    case __.type : _type = Convert.ToInt32(value); break;
                    case __.name : _name = Convert.ToString(value); break;
                    case __.size : _size = Convert.ToInt32(value); break;
                    case __.boom : _boom = Convert.ToDouble(value); break;
                    case __.strong : _strong = Convert.ToDouble(value); break;
                    case __.peace : _peace = Convert.ToDouble(value); break;
                    case __.guard_time : _guard_time = Convert.ToInt64(value); break;
                    case __.module_number : _module_number = Convert.ToInt32(value); break;
                    case __.defense_id : _defense_id = Convert.ToInt32(value); break;
                    case __.plan_1 : _plan_1 = Convert.ToInt64(value); break;
                    case __.plan_2 : _plan_2 = Convert.ToInt64(value); break;
                    case __.plan_3 : _plan_3 = Convert.ToInt64(value); break;
                    case __.time : _time = Convert.ToInt64(value); break;
                    case __.own : _own = Convert.ToInt32(value); break;
                    case __.fire_time : _fire_time = Convert.ToInt64(value); break;
                    case __.destroy_time : _destroy_time = Convert.ToInt64(value); break;
                    case __.ownership_type : _ownership_type = Convert.ToInt32(value); break;
                    case __.res_soldier : _res_soldier = Convert.ToInt32(value); break;
                    case __.res_funds : _res_funds = Convert.ToDouble(value); break;
                    case __.res_foods : _res_foods = Convert.ToInt32(value); break;
                    case __.res_horse : _res_horse = Convert.ToInt32(value); break;
                    case __.res_gun : _res_gun = Convert.ToInt32(value); break;
                    case __.res_razor : _res_razor = Convert.ToInt32(value); break;
                    case __.res_kuwu : _res_kuwu = Convert.ToInt32(value); break;
                    case __.res_use_soldier : _res_use_soldier = Convert.ToInt32(value); break;
                    case __.res_use_funds : _res_use_funds = Convert.ToDouble(value); break;
                    case __.res_use_foods : _res_use_foods = Convert.ToInt32(value); break;
                    case __.res_use_horse : _res_use_horse = Convert.ToInt32(value); break;
                    case __.res_use_gun : _res_use_gun = Convert.ToInt32(value); break;
                    case __.res_use_razor : _res_use_razor = Convert.ToInt32(value); break;
                    case __.res_use_kuwu : _res_use_kuwu = Convert.ToInt32(value); break;
                    case __.res_morale : _res_morale = Convert.ToInt32(value); break;
                    case __.interior_bar : _interior_bar = Convert.ToInt32(value); break;
                    case __.levy_bar : _levy_bar = Convert.ToInt32(value); break;
                    case __.train_bar : _train_bar = Convert.ToInt32(value); break;
                    case __.residence : _residence = Convert.ToInt32(value); break;
                    case __.destroy_boom : _destroy_boom = Convert.ToInt32(value); break;
                    case __.destroy_strong : _destroy_strong = Convert.ToInt32(value); break;
                    case __.destroy_peace : _destroy_peace = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得据点表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>据点基表编号</summary>
            public static readonly Field base_id = FindByName(__.base_id);

            ///<summary>据点状态</summary>
            public static readonly Field state = FindByName(__.state);

            ///<summary>据点类型(0:支城1:本城)</summary>
            public static readonly Field type = FindByName(__.type);

            ///<summary>据点名称</summary>
            public static readonly Field name = FindByName(__.name);

            ///<summary>规模</summary>
            public static readonly Field size = FindByName(__.size);

            ///<summary>繁荣度</summary>
            public static readonly Field boom = FindByName(__.boom);

            ///<summary>耐久度</summary>
            public static readonly Field strong = FindByName(__.strong);

            ///<summary>治安</summary>
            public static readonly Field peace = FindByName(__.peace);

            ///<summary>保护时间</summary>
            public static readonly Field guard_time = FindByName(__.guard_time);

            ///<summary>所属模块</summary>
            public static readonly Field module_number = FindByName(__.module_number);

            ///<summary></summary>
            public static readonly Field defense_id = FindByName(__.defense_id);

            ///<summary>防守方案1</summary>
            public static readonly Field plan_1 = FindByName(__.plan_1);

            ///<summary>防守方案2</summary>
            public static readonly Field plan_2 = FindByName(__.plan_2);

            ///<summary>防守方案3</summary>
            public static readonly Field plan_3 = FindByName(__.plan_3);

            ///<summary>筑城时间</summary>
            public static readonly Field time = FindByName(__.time);

            ///<summary>当前据点占有度</summary>
            public static readonly Field own = FindByName(__.own);

            ///<summary>放火结束时间</summary>
            public static readonly Field fire_time = FindByName(__.fire_time);

            ///<summary>破坏结束时间</summary>
            public static readonly Field destroy_time = FindByName(__.destroy_time);

            ///<summary></summary>
            public static readonly Field ownership_type = FindByName(__.ownership_type);

            ///<summary>士兵(足轻)</summary>
            public static readonly Field res_soldier = FindByName(__.res_soldier);

            ///<summary>军资金</summary>
            public static readonly Field res_funds = FindByName(__.res_funds);

            ///<summary>军粮</summary>
            public static readonly Field res_foods = FindByName(__.res_foods);

            ///<summary>马匹</summary>
            public static readonly Field res_horse = FindByName(__.res_horse);

            ///<summary>铁炮</summary>
            public static readonly Field res_gun = FindByName(__.res_gun);

            ///<summary>薙刀</summary>
            public static readonly Field res_razor = FindByName(__.res_razor);

            ///<summary>苦无</summary>
            public static readonly Field res_kuwu = FindByName(__.res_kuwu);

            ///<summary>以配置士兵(足轻)</summary>
            public static readonly Field res_use_soldier = FindByName(__.res_use_soldier);

            ///<summary>以配置军资金</summary>
            public static readonly Field res_use_funds = FindByName(__.res_use_funds);

            ///<summary>以配置军粮</summary>
            public static readonly Field res_use_foods = FindByName(__.res_use_foods);

            ///<summary>以配置马匹</summary>
            public static readonly Field res_use_horse = FindByName(__.res_use_horse);

            ///<summary>以配置铁炮</summary>
            public static readonly Field res_use_gun = FindByName(__.res_use_gun);

            ///<summary>以配置薙刀</summary>
            public static readonly Field res_use_razor = FindByName(__.res_use_razor);

            ///<summary>以配置苦无</summary>
            public static readonly Field res_use_kuwu = FindByName(__.res_use_kuwu);

            ///<summary>士气值</summary>
            public static readonly Field res_morale = FindByName(__.res_morale);

            ///<summary>内政解锁栏</summary>
            public static readonly Field interior_bar = FindByName(__.interior_bar);

            ///<summary>徵兵解锁栏</summary>
            public static readonly Field levy_bar = FindByName(__.levy_bar);

            ///<summary>训练解锁栏</summary>
            public static readonly Field train_bar = FindByName(__.train_bar);

            ///<summary>驻扎人数</summary>
            public static readonly Field residence = FindByName(__.residence);

            ///<summary>破坏扣除繁荣度</summary>
            public static readonly Field destroy_boom = FindByName(__.destroy_boom);

            ///<summary>破坏扣除耐久度</summary>
            public static readonly Field destroy_strong = FindByName(__.destroy_strong);

            ///<summary>破坏扣除治安</summary>
            public static readonly Field destroy_peace = FindByName(__.destroy_peace);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得据点表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>据点基表编号</summary>
            public const String base_id = "base_id";

            ///<summary>据点状态</summary>
            public const String state = "state";

            ///<summary>据点类型(0:支城1:本城)</summary>
            public const String type = "type";

            ///<summary>据点名称</summary>
            public const String name = "name";

            ///<summary>规模</summary>
            public const String size = "size";

            ///<summary>繁荣度</summary>
            public const String boom = "boom";

            ///<summary>耐久度</summary>
            public const String strong = "strong";

            ///<summary>治安</summary>
            public const String peace = "peace";

            ///<summary>保护时间</summary>
            public const String guard_time = "guard_time";

            ///<summary>所属模块</summary>
            public const String module_number = "module_number";

            ///<summary></summary>
            public const String defense_id = "defense_id";

            ///<summary>防守方案1</summary>
            public const String plan_1 = "plan_1";

            ///<summary>防守方案2</summary>
            public const String plan_2 = "plan_2";

            ///<summary>防守方案3</summary>
            public const String plan_3 = "plan_3";

            ///<summary>筑城时间</summary>
            public const String time = "time";

            ///<summary>当前据点占有度</summary>
            public const String own = "own";

            ///<summary>放火结束时间</summary>
            public const String fire_time = "fire_time";

            ///<summary>破坏结束时间</summary>
            public const String destroy_time = "destroy_time";

            ///<summary></summary>
            public const String ownership_type = "ownership_type";

            ///<summary>士兵(足轻)</summary>
            public const String res_soldier = "res_soldier";

            ///<summary>军资金</summary>
            public const String res_funds = "res_funds";

            ///<summary>军粮</summary>
            public const String res_foods = "res_foods";

            ///<summary>马匹</summary>
            public const String res_horse = "res_horse";

            ///<summary>铁炮</summary>
            public const String res_gun = "res_gun";

            ///<summary>薙刀</summary>
            public const String res_razor = "res_razor";

            ///<summary>苦无</summary>
            public const String res_kuwu = "res_kuwu";

            ///<summary>以配置士兵(足轻)</summary>
            public const String res_use_soldier = "res_use_soldier";

            ///<summary>以配置军资金</summary>
            public const String res_use_funds = "res_use_funds";

            ///<summary>以配置军粮</summary>
            public const String res_use_foods = "res_use_foods";

            ///<summary>以配置马匹</summary>
            public const String res_use_horse = "res_use_horse";

            ///<summary>以配置铁炮</summary>
            public const String res_use_gun = "res_use_gun";

            ///<summary>以配置薙刀</summary>
            public const String res_use_razor = "res_use_razor";

            ///<summary>以配置苦无</summary>
            public const String res_use_kuwu = "res_use_kuwu";

            ///<summary>士气值</summary>
            public const String res_morale = "res_morale";

            ///<summary>内政解锁栏</summary>
            public const String interior_bar = "interior_bar";

            ///<summary>徵兵解锁栏</summary>
            public const String levy_bar = "levy_bar";

            ///<summary>训练解锁栏</summary>
            public const String train_bar = "train_bar";

            ///<summary>驻扎人数</summary>
            public const String residence = "residence";

            ///<summary>破坏扣除繁荣度</summary>
            public const String destroy_boom = "destroy_boom";

            ///<summary>破坏扣除耐久度</summary>
            public const String destroy_strong = "destroy_strong";

            ///<summary>破坏扣除治安</summary>
            public const String destroy_peace = "destroy_peace";

        }
        #endregion
    }

    /// <summary>据点表接口</summary>
    public partial interface Itg_war_city
    {
        #region 属性
        /// <summary>编号</summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>据点基表编号</summary>
        Int32 base_id { get; set; }

        /// <summary>据点状态</summary>
        Int32 state { get; set; }

        /// <summary>据点类型(0:支城1:本城)</summary>
        Int32 type { get; set; }

        /// <summary>据点名称</summary>
        String name { get; set; }

        /// <summary>规模</summary>
        Int32 size { get; set; }

        /// <summary>繁荣度</summary>
        Double boom { get; set; }

        /// <summary>耐久度</summary>
        Double strong { get; set; }

        /// <summary>治安</summary>
        Double peace { get; set; }

        /// <summary>保护时间</summary>
        Int64 guard_time { get; set; }

        /// <summary>所属模块</summary>
        Int32 module_number { get; set; }

        /// <summary></summary>
        Int32 defense_id { get; set; }

        /// <summary>防守方案1</summary>
        Int64 plan_1 { get; set; }

        /// <summary>防守方案2</summary>
        Int64 plan_2 { get; set; }

        /// <summary>防守方案3</summary>
        Int64 plan_3 { get; set; }

        /// <summary>筑城时间</summary>
        Int64 time { get; set; }

        /// <summary>当前据点占有度</summary>
        Int32 own { get; set; }

        /// <summary>放火结束时间</summary>
        Int64 fire_time { get; set; }

        /// <summary>破坏结束时间</summary>
        Int64 destroy_time { get; set; }

        /// <summary></summary>
        Int32 ownership_type { get; set; }

        /// <summary>士兵(足轻)</summary>
        Int32 res_soldier { get; set; }

        /// <summary>军资金</summary>
        Double res_funds { get; set; }

        /// <summary>军粮</summary>
        Int32 res_foods { get; set; }

        /// <summary>马匹</summary>
        Int32 res_horse { get; set; }

        /// <summary>铁炮</summary>
        Int32 res_gun { get; set; }

        /// <summary>薙刀</summary>
        Int32 res_razor { get; set; }

        /// <summary>苦无</summary>
        Int32 res_kuwu { get; set; }

        /// <summary>以配置士兵(足轻)</summary>
        Int32 res_use_soldier { get; set; }

        /// <summary>以配置军资金</summary>
        Double res_use_funds { get; set; }

        /// <summary>以配置军粮</summary>
        Int32 res_use_foods { get; set; }

        /// <summary>以配置马匹</summary>
        Int32 res_use_horse { get; set; }

        /// <summary>以配置铁炮</summary>
        Int32 res_use_gun { get; set; }

        /// <summary>以配置薙刀</summary>
        Int32 res_use_razor { get; set; }

        /// <summary>以配置苦无</summary>
        Int32 res_use_kuwu { get; set; }

        /// <summary>士气值</summary>
        Int32 res_morale { get; set; }

        /// <summary>内政解锁栏</summary>
        Int32 interior_bar { get; set; }

        /// <summary>徵兵解锁栏</summary>
        Int32 levy_bar { get; set; }

        /// <summary>训练解锁栏</summary>
        Int32 train_bar { get; set; }

        /// <summary>驻扎人数</summary>
        Int32 residence { get; set; }

        /// <summary>破坏扣除繁荣度</summary>
        Int32 destroy_boom { get; set; }

        /// <summary>破坏扣除耐久度</summary>
        Int32 destroy_strong { get; set; }

        /// <summary>破坏扣除治安</summary>
        Int32 destroy_peace { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}