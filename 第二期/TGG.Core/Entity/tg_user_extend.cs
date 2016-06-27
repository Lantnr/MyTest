using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    /// <summary>玩家扩展表</summary>
    [Serializable]
    [DataObject]
    [Description("玩家扩展表")]
    [BindIndex("PK__tg_user___3213E83F6ED0ED95", true, "id")]
    [BindTable("tg_user_extend", Description = "玩家扩展表", ConnName = "DB", DbType = DatabaseType.SqlServer)]
    public partial class tg_user_extend : Itg_user_extend
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

        private Int32 _train_bar;
        /// <summary>武将修行栏开启数</summary>
        [DisplayName("武将修行栏开启数")]
        [Description("武将修行栏开启数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "train_bar", "武将修行栏开启数", "3", "int", 10, 0, false)]
        public virtual Int32 train_bar
        {
            get { return _train_bar; }
            set { if (OnPropertyChanging(__.train_bar, value)) { _train_bar = value; OnPropertyChanged(__.train_bar); } }
        }

        private Int32 _task_vocation_refresh;
        /// <summary>职业任务刷新</summary>
        [DisplayName("职业任务刷新")]
        [Description("职业任务刷新")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(4, "task_vocation_refresh", "职业任务刷新", "0", "int", 10, 0, false)]
        public virtual Int32 task_vocation_refresh
        {
            get { return _task_vocation_refresh; }
            set { if (OnPropertyChanging(__.task_vocation_refresh, value)) { _task_vocation_refresh = value; OnPropertyChanged(__.task_vocation_refresh); } }
        }

        private Int32 _shot_count;
        /// <summary>名塔闯关次数</summary>
        [DisplayName("名塔闯关次数")]
        [Description("名塔闯关次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(5, "shot_count", "名塔闯关次数", "3", "int", 10, 0, false)]
        public virtual Int32 shot_count
        {
            get { return _shot_count; }
            set { if (OnPropertyChanging(__.shot_count, value)) { _shot_count = value; OnPropertyChanged(__.shot_count); } }
        }

        private Int32 _bag_count;
        /// <summary>背包格子总数</summary>
        [DisplayName("背包格子总数")]
        [Description("背包格子总数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(6, "bag_count", "背包格子总数", "0", "int", 10, 0, false)]
        public virtual Int32 bag_count
        {
            get { return _bag_count; }
            set { if (OnPropertyChanging(__.bag_count, value)) { _bag_count = value; OnPropertyChanged(__.bag_count); } }
        }

        private Int32 _daySalary;
        /// <summary>家族领取俸禄状态</summary>
        [DisplayName("家族领取俸禄状态")]
        [Description("家族领取俸禄状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(7, "daySalary", "家族领取俸禄状态", "0", "int", 10, 0, false)]
        public virtual Int32 daySalary
        {
            get { return _daySalary; }
            set { if (OnPropertyChanging(__.daySalary, value)) { _daySalary = value; OnPropertyChanged(__.daySalary); } }
        }

        private Int32 _donate;
        /// <summary>家族每天捐献金钱</summary>
        [DisplayName("家族每天捐献金钱")]
        [Description("家族每天捐献金钱")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(8, "donate", "家族每天捐献金钱", "0", "int", 10, 0, false)]
        public virtual Int32 donate
        {
            get { return _donate; }
            set { if (OnPropertyChanging(__.donate, value)) { _donate = value; OnPropertyChanged(__.donate); } }
        }

        private Int32 _sword_win_count;
        /// <summary>使用剑击杀胜利次数</summary>
        [DisplayName("使用剑击杀胜利次数")]
        [Description("使用剑击杀胜利次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "sword_win_count", "使用剑击杀胜利次数", "0", "int", 10, 0, false)]
        public virtual Int32 sword_win_count
        {
            get { return _sword_win_count; }
            set { if (OnPropertyChanging(__.sword_win_count, value)) { _sword_win_count = value; OnPropertyChanged(__.sword_win_count); } }
        }

        private Int32 _gun_win_count;
        /// <summary>使用枪击杀胜利次数</summary>
        [DisplayName("使用枪击杀胜利次数")]
        [Description("使用枪击杀胜利次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "gun_win_count", "使用枪击杀胜利次数", "0", "int", 10, 0, false)]
        public virtual Int32 gun_win_count
        {
            get { return _gun_win_count; }
            set { if (OnPropertyChanging(__.gun_win_count, value)) { _gun_win_count = value; OnPropertyChanged(__.gun_win_count); } }
        }

        private Int32 _tea_table_count;
        /// <summary>使用茶席次数</summary>
        [DisplayName("使用茶席次数")]
        [Description("使用茶席次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "tea_table_count", "使用茶席次数", "0", "int", 10, 0, false)]
        public virtual Int32 tea_table_count
        {
            get { return _tea_table_count; }
            set { if (OnPropertyChanging(__.tea_table_count, value)) { _tea_table_count = value; OnPropertyChanged(__.tea_table_count); } }
        }

        private Int32 _bargain_success_count;
        /// <summary>讲价成功次数</summary>
        [DisplayName("讲价成功次数")]
        [Description("讲价成功次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(12, "bargain_success_count", "讲价成功次数", "0", "int", 10, 0, false)]
        public virtual Int32 bargain_success_count
        {
            get { return _bargain_success_count; }
            set { if (OnPropertyChanging(__.bargain_success_count, value)) { _bargain_success_count = value; OnPropertyChanged(__.bargain_success_count); } }
        }

        private Int32 _task_role_refresh;
        /// <summary>家臣任务刷新次数</summary>
        [DisplayName("家臣任务刷新次数")]
        [Description("家臣任务刷新次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(13, "task_role_refresh", "家臣任务刷新次数", "0", "int", 10, 0, false)]
        public virtual Int32 task_role_refresh
        {
            get { return _task_role_refresh; }
            set { if (OnPropertyChanging(__.task_role_refresh, value)) { _task_role_refresh = value; OnPropertyChanged(__.task_role_refresh); } }
        }

        private Int32 _npc_refresh_count;
        /// <summary>翻将次数</summary>
        [DisplayName("翻将次数")]
        [Description("翻将次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "npc_refresh_count", "翻将次数", "3", "int", 10, 0, false)]
        public virtual Int32 npc_refresh_count
        {
            get { return _npc_refresh_count; }
            set { if (OnPropertyChanging(__.npc_refresh_count, value)) { _npc_refresh_count = value; OnPropertyChanged(__.npc_refresh_count); } }
        }

        private Int32 _bargain_count;
        /// <summary>武将已经议价次数</summary>
        [DisplayName("武将已经议价次数")]
        [Description("武将已经议价次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(15, "bargain_count", "武将已经议价次数", "0", "int", 10, 0, false)]
        public virtual Int32 bargain_count
        {
            get { return _bargain_count; }
            set { if (OnPropertyChanging(__.bargain_count, value)) { _bargain_count = value; OnPropertyChanged(__.bargain_count); } }
        }

        private Int32 _challenge_count;
        /// <summary>爬塔挑战次数</summary>
        [DisplayName("爬塔挑战次数")]
        [Description("爬塔挑战次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "challenge_count", "爬塔挑战次数", "1", "int", 10, 0, false)]
        public virtual Int32 challenge_count
        {
            get { return _challenge_count; }
            set { if (OnPropertyChanging(__.challenge_count, value)) { _challenge_count = value; OnPropertyChanged(__.challenge_count); } }
        }

        private Int32 _salary_state;
        /// <summary></summary>
        [DisplayName("State")]
        [Description("")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "salary_state", "", "0", "int", 10, 0, false)]
        public virtual Int32 salary_state
        {
            get { return _salary_state; }
            set { if (OnPropertyChanging(__.salary_state, value)) { _salary_state = value; OnPropertyChanged(__.salary_state); } }
        }

        private Int32 _power_buy_count;
        /// <summary>购买体力次数</summary>
        [DisplayName("购买体力次数")]
        [Description("购买体力次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(18, "power_buy_count", "购买体力次数", "0", "int", 10, 0, false)]
        public virtual Int32 power_buy_count
        {
            get { return _power_buy_count; }
            set { if (OnPropertyChanging(__.power_buy_count, value)) { _power_buy_count = value; OnPropertyChanged(__.power_buy_count); } }
        }

        private Int32 _task_vocation_isgo;
        /// <summary>身份替身后是否继续原来身份的任务  0：是 1：否</summary>
        [DisplayName("身份替身后是否继续原来身份的任务0：是1：否")]
        [Description("身份替身后是否继续原来身份的任务  0：是 1：否")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(19, "task_vocation_isgo", "身份替身后是否继续原来身份的任务  0：是 1：否", "0", "int", 10, 0, false)]
        public virtual Int32 task_vocation_isgo
        {
            get { return _task_vocation_isgo; }
            set { if (OnPropertyChanging(__.task_vocation_isgo, value)) { _task_vocation_isgo = value; OnPropertyChanged(__.task_vocation_isgo); } }
        }

        private Int64 _hire_id;
        /// <summary>雇佣id</summary>
        [DisplayName("雇佣id")]
        [Description("雇佣id")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(20, "hire_id", "雇佣id", "0", "bigint", 19, 0, false)]
        public virtual Int64 hire_id
        {
            get { return _hire_id; }
            set { if (OnPropertyChanging(__.hire_id, value)) { _hire_id = value; OnPropertyChanged(__.hire_id); } }
        }

        private Int64 _hire_time;
        /// <summary>雇佣到达时间</summary>
        [DisplayName("雇佣到达时间")]
        [Description("雇佣到达时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(21, "hire_time", "雇佣到达时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 hire_time
        {
            get { return _hire_time; }
            set { if (OnPropertyChanging(__.hire_time, value)) { _hire_time = value; OnPropertyChanged(__.hire_time); } }
        }

        private Int64 _recruit_time;
        /// <summary>酒馆招募刷新冷却时间</summary>
        [DisplayName("酒馆招募刷新冷却时间")]
        [Description("酒馆招募刷新冷却时间")]
        [DataObjectField(false, false, false, 19)]
        [BindColumn(22, "recruit_time", "酒馆招募刷新冷却时间", "0", "bigint", 19, 0, false)]
        public virtual Int64 recruit_time
        {
            get { return _recruit_time; }
            set { if (OnPropertyChanging(__.recruit_time, value)) { _recruit_time = value; OnPropertyChanged(__.recruit_time); } }
        }

        private Int32 _fcm;
        /// <summary>防沉迷 0:未验证 1:已验证</summary>
        [DisplayName("防沉迷0:未验证1:已验证")]
        [Description("防沉迷 0:未验证 1:已验证")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(23, "fcm", "防沉迷 0:未验证 1:已验证", "0", "int", 10, 0, false)]
        public virtual Int32 fcm
        {
            get { return _fcm; }
            set { if (OnPropertyChanging(__.fcm, value)) { _fcm = value; OnPropertyChanged(__.fcm); } }
        }

        private Int32 _dml;
        /// <summary>大名令  0:未激活 1:已激活 </summary>
        [DisplayName("大名令0:未激活1:已激活")]
        [Description("大名令  0:未激活 1:已激活 ")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(24, "dml", "大名令  0:未激活 1:已激活 ", "0", "int", 10, 0, false)]
        public virtual Int32 dml
        {
            get { return _dml; }
            set { if (OnPropertyChanging(__.dml, value)) { _dml = value; OnPropertyChanged(__.dml); } }
        }

        private Int32 _eloquence_count;
        /// <summary>辩才游戏使用次数</summary>
        [DisplayName("辩才游戏使用次数")]
        [Description("辩才游戏使用次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(25, "eloquence_count", "辩才游戏使用次数", "0", "int", 10, 0, false)]
        public virtual Int32 eloquence_count
        {
            get { return _eloquence_count; }
            set { if (OnPropertyChanging(__.eloquence_count, value)) { _eloquence_count = value; OnPropertyChanged(__.eloquence_count); } }
        }

        private Int32 _tea_count;
        /// <summary>茶道游戏使用次数</summary>
        [DisplayName("茶道游戏使用次数")]
        [Description("茶道游戏使用次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(26, "tea_count", "茶道游戏使用次数", "0", "int", 10, 0, false)]
        public virtual Int32 tea_count
        {
            get { return _tea_count; }
            set { if (OnPropertyChanging(__.tea_count, value)) { _tea_count = value; OnPropertyChanged(__.tea_count); } }
        }

        private Int32 _calculate_count;
        /// <summary>算术游戏使用次数</summary>
        [DisplayName("算术游戏使用次数")]
        [Description("算术游戏使用次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(27, "calculate_count", "算术游戏使用次数", "0", "int", 10, 0, false)]
        public virtual Int32 calculate_count
        {
            get { return _calculate_count; }
            set { if (OnPropertyChanging(__.calculate_count, value)) { _calculate_count = value; OnPropertyChanged(__.calculate_count); } }
        }

        private Int32 _ninjutsu_count;
        /// <summary>忍术游戏使用次数</summary>
        [DisplayName("忍术游戏使用次数")]
        [Description("忍术游戏使用次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(28, "ninjutsu_count", "忍术游戏使用次数", "0", "int", 10, 0, false)]
        public virtual Int32 ninjutsu_count
        {
            get { return _ninjutsu_count; }
            set { if (OnPropertyChanging(__.ninjutsu_count, value)) { _ninjutsu_count = value; OnPropertyChanged(__.ninjutsu_count); } }
        }

        private Int32 _ball_count;
        /// <summary>打气球次数</summary>
        [DisplayName("打气球次数")]
        [Description("打气球次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(29, "ball_count", "打气球次数", "0", "int", 10, 0, false)]
        public virtual Int32 ball_count
        {
            get { return _ball_count; }
            set { if (OnPropertyChanging(__.ball_count, value)) { _ball_count = value; OnPropertyChanged(__.ball_count); } }
        }

        private Int32 _game_finish_count;
        /// <summary>每日完成次数</summary>
        [DisplayName("每日完成次数")]
        [Description("每日完成次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(30, "game_finish_count", "每日完成次数", "0", "int", 10, 0, false)]
        public virtual Int32 game_finish_count
        {
            get { return _game_finish_count; }
            set { if (OnPropertyChanging(__.game_finish_count, value)) { _game_finish_count = value; OnPropertyChanged(__.game_finish_count); } }
        }

        private Int32 _game_receive;
        /// <summary>小游戏领取奖励状态 0：未领取  1：可领取  2：已领取</summary>
        [DisplayName("小游戏领取奖励状态0：未领取1：可领取2：已领取")]
        [Description("小游戏领取奖励状态 0：未领取  1：可领取  2：已领取")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(31, "game_receive", "小游戏领取奖励状态 0：未领取  1：可领取  2：已领取", "0", "int", 10, 0, false)]
        public virtual Int32 game_receive
        {
            get { return _game_receive; }
            set { if (OnPropertyChanging(__.game_receive, value)) { _game_receive = value; OnPropertyChanged(__.game_receive); } }
        }

        private Int32 _refresh_count;
        /// <summary>武将宅刷新次数</summary>
        [DisplayName("武将宅刷新次数")]
        [Description("武将宅刷新次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(32, "refresh_count", "武将宅刷新次数", "0", "int", 10, 0, false)]
        public virtual Int32 refresh_count
        {
            get { return _refresh_count; }
            set { if (OnPropertyChanging(__.refresh_count, value)) { _refresh_count = value; OnPropertyChanged(__.refresh_count); } }
        }

        private Int32 _steal_fail_count;
        /// <summary>武将宅偷窃失败次数</summary>
        [DisplayName("武将宅偷窃失败次数")]
        [Description("武将宅偷窃失败次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(33, "steal_fail_count", "武将宅偷窃失败次数", "0", "int", 10, 0, false)]
        public virtual Int32 steal_fail_count
        {
            get { return _steal_fail_count; }
            set { if (OnPropertyChanging(__.steal_fail_count, value)) { _steal_fail_count = value; OnPropertyChanged(__.steal_fail_count); } }
        }

        private Int32 _fight_count;
        /// <summary>武将宅已挑战次数</summary>
        [DisplayName("武将宅已挑战次数")]
        [Description("武将宅已挑战次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(34, "fight_count", "武将宅已挑战次数", "0", "int", 10, 0, false)]
        public virtual Int32 fight_count
        {
            get { return _fight_count; }
            set { if (OnPropertyChanging(__.fight_count, value)) { _fight_count = value; OnPropertyChanged(__.fight_count); } }
        }

        private Int32 _fight_buy;
        /// <summary>武将宅已购买挑战次数</summary>
        [DisplayName("武将宅已购买挑战次数")]
        [Description("武将宅已购买挑战次数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(35, "fight_buy", "武将宅已购买挑战次数", "0", "int", 10, 0, false)]
        public virtual Int32 fight_buy
        {
            get { return _fight_buy; }
            set { if (OnPropertyChanging(__.fight_buy, value)) { _fight_buy = value; OnPropertyChanged(__.fight_buy); } }
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
                    case __.train_bar : return _train_bar;
                    case __.task_vocation_refresh : return _task_vocation_refresh;
                    case __.shot_count : return _shot_count;
                    case __.bag_count : return _bag_count;
                    case __.daySalary : return _daySalary;
                    case __.donate : return _donate;
                    case __.sword_win_count : return _sword_win_count;
                    case __.gun_win_count : return _gun_win_count;
                    case __.tea_table_count : return _tea_table_count;
                    case __.bargain_success_count : return _bargain_success_count;
                    case __.task_role_refresh : return _task_role_refresh;
                    case __.npc_refresh_count : return _npc_refresh_count;
                    case __.bargain_count : return _bargain_count;
                    case __.challenge_count : return _challenge_count;
                    case __.salary_state : return _salary_state;
                    case __.power_buy_count : return _power_buy_count;
                    case __.task_vocation_isgo : return _task_vocation_isgo;
                    case __.hire_id : return _hire_id;
                    case __.hire_time : return _hire_time;
                    case __.recruit_time : return _recruit_time;
                    case __.fcm : return _fcm;
                    case __.dml : return _dml;
                    case __.eloquence_count : return _eloquence_count;
                    case __.tea_count : return _tea_count;
                    case __.calculate_count : return _calculate_count;
                    case __.ninjutsu_count : return _ninjutsu_count;
                    case __.ball_count : return _ball_count;
                    case __.game_finish_count : return _game_finish_count;
                    case __.game_receive : return _game_receive;
                    case __.refresh_count : return _refresh_count;
                    case __.steal_fail_count : return _steal_fail_count;
                    case __.fight_count : return _fight_count;
                    case __.fight_buy : return _fight_buy;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.id : _id = Convert.ToInt64(value); break;
                    case __.user_id : _user_id = Convert.ToInt64(value); break;
                    case __.train_bar : _train_bar = Convert.ToInt32(value); break;
                    case __.task_vocation_refresh : _task_vocation_refresh = Convert.ToInt32(value); break;
                    case __.shot_count : _shot_count = Convert.ToInt32(value); break;
                    case __.bag_count : _bag_count = Convert.ToInt32(value); break;
                    case __.daySalary : _daySalary = Convert.ToInt32(value); break;
                    case __.donate : _donate = Convert.ToInt32(value); break;
                    case __.sword_win_count : _sword_win_count = Convert.ToInt32(value); break;
                    case __.gun_win_count : _gun_win_count = Convert.ToInt32(value); break;
                    case __.tea_table_count : _tea_table_count = Convert.ToInt32(value); break;
                    case __.bargain_success_count : _bargain_success_count = Convert.ToInt32(value); break;
                    case __.task_role_refresh : _task_role_refresh = Convert.ToInt32(value); break;
                    case __.npc_refresh_count : _npc_refresh_count = Convert.ToInt32(value); break;
                    case __.bargain_count : _bargain_count = Convert.ToInt32(value); break;
                    case __.challenge_count : _challenge_count = Convert.ToInt32(value); break;
                    case __.salary_state : _salary_state = Convert.ToInt32(value); break;
                    case __.power_buy_count : _power_buy_count = Convert.ToInt32(value); break;
                    case __.task_vocation_isgo : _task_vocation_isgo = Convert.ToInt32(value); break;
                    case __.hire_id : _hire_id = Convert.ToInt64(value); break;
                    case __.hire_time : _hire_time = Convert.ToInt64(value); break;
                    case __.recruit_time : _recruit_time = Convert.ToInt64(value); break;
                    case __.fcm : _fcm = Convert.ToInt32(value); break;
                    case __.dml : _dml = Convert.ToInt32(value); break;
                    case __.eloquence_count : _eloquence_count = Convert.ToInt32(value); break;
                    case __.tea_count : _tea_count = Convert.ToInt32(value); break;
                    case __.calculate_count : _calculate_count = Convert.ToInt32(value); break;
                    case __.ninjutsu_count : _ninjutsu_count = Convert.ToInt32(value); break;
                    case __.ball_count : _ball_count = Convert.ToInt32(value); break;
                    case __.game_finish_count : _game_finish_count = Convert.ToInt32(value); break;
                    case __.game_receive : _game_receive = Convert.ToInt32(value); break;
                    case __.refresh_count : _refresh_count = Convert.ToInt32(value); break;
                    case __.steal_fail_count : _steal_fail_count = Convert.ToInt32(value); break;
                    case __.fight_count : _fight_count = Convert.ToInt32(value); break;
                    case __.fight_buy : _fight_buy = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得玩家扩展表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field id = FindByName(__.id);

            ///<summary>玩家编号</summary>
            public static readonly Field user_id = FindByName(__.user_id);

            ///<summary>武将修行栏开启数</summary>
            public static readonly Field train_bar = FindByName(__.train_bar);

            ///<summary>职业任务刷新</summary>
            public static readonly Field task_vocation_refresh = FindByName(__.task_vocation_refresh);

            ///<summary>名塔闯关次数</summary>
            public static readonly Field shot_count = FindByName(__.shot_count);

            ///<summary>背包格子总数</summary>
            public static readonly Field bag_count = FindByName(__.bag_count);

            ///<summary>家族领取俸禄状态</summary>
            public static readonly Field daySalary = FindByName(__.daySalary);

            ///<summary>家族每天捐献金钱</summary>
            public static readonly Field donate = FindByName(__.donate);

            ///<summary>使用剑击杀胜利次数</summary>
            public static readonly Field sword_win_count = FindByName(__.sword_win_count);

            ///<summary>使用枪击杀胜利次数</summary>
            public static readonly Field gun_win_count = FindByName(__.gun_win_count);

            ///<summary>使用茶席次数</summary>
            public static readonly Field tea_table_count = FindByName(__.tea_table_count);

            ///<summary>讲价成功次数</summary>
            public static readonly Field bargain_success_count = FindByName(__.bargain_success_count);

            ///<summary>家臣任务刷新次数</summary>
            public static readonly Field task_role_refresh = FindByName(__.task_role_refresh);

            ///<summary>翻将次数</summary>
            public static readonly Field npc_refresh_count = FindByName(__.npc_refresh_count);

            ///<summary>武将已经议价次数</summary>
            public static readonly Field bargain_count = FindByName(__.bargain_count);

            ///<summary>爬塔挑战次数</summary>
            public static readonly Field challenge_count = FindByName(__.challenge_count);

            ///<summary></summary>
            public static readonly Field salary_state = FindByName(__.salary_state);

            ///<summary>购买体力次数</summary>
            public static readonly Field power_buy_count = FindByName(__.power_buy_count);

            ///<summary>身份替身后是否继续原来身份的任务  0：是 1：否</summary>
            public static readonly Field task_vocation_isgo = FindByName(__.task_vocation_isgo);

            ///<summary>雇佣id</summary>
            public static readonly Field hire_id = FindByName(__.hire_id);

            ///<summary>雇佣到达时间</summary>
            public static readonly Field hire_time = FindByName(__.hire_time);

            ///<summary>酒馆招募刷新冷却时间</summary>
            public static readonly Field recruit_time = FindByName(__.recruit_time);

            ///<summary>防沉迷 0:未验证 1:已验证</summary>
            public static readonly Field fcm = FindByName(__.fcm);

            ///<summary>大名令  0:未激活 1:已激活 </summary>
            public static readonly Field dml = FindByName(__.dml);

            ///<summary>辩才游戏使用次数</summary>
            public static readonly Field eloquence_count = FindByName(__.eloquence_count);

            ///<summary>茶道游戏使用次数</summary>
            public static readonly Field tea_count = FindByName(__.tea_count);

            ///<summary>算术游戏使用次数</summary>
            public static readonly Field calculate_count = FindByName(__.calculate_count);

            ///<summary>忍术游戏使用次数</summary>
            public static readonly Field ninjutsu_count = FindByName(__.ninjutsu_count);

            ///<summary>打气球次数</summary>
            public static readonly Field ball_count = FindByName(__.ball_count);

            ///<summary>每日完成次数</summary>
            public static readonly Field game_finish_count = FindByName(__.game_finish_count);

            ///<summary>小游戏领取奖励状态 0：未领取  1：可领取  2：已领取</summary>
            public static readonly Field game_receive = FindByName(__.game_receive);

            ///<summary>武将宅刷新次数</summary>
            public static readonly Field refresh_count = FindByName(__.refresh_count);

            ///<summary>武将宅偷窃失败次数</summary>
            public static readonly Field steal_fail_count = FindByName(__.steal_fail_count);

            ///<summary>武将宅已挑战次数</summary>
            public static readonly Field fight_count = FindByName(__.fight_count);

            ///<summary>武将宅已购买挑战次数</summary>
            public static readonly Field fight_buy = FindByName(__.fight_buy);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得玩家扩展表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String id = "id";

            ///<summary>玩家编号</summary>
            public const String user_id = "user_id";

            ///<summary>武将修行栏开启数</summary>
            public const String train_bar = "train_bar";

            ///<summary>职业任务刷新</summary>
            public const String task_vocation_refresh = "task_vocation_refresh";

            ///<summary>名塔闯关次数</summary>
            public const String shot_count = "shot_count";

            ///<summary>背包格子总数</summary>
            public const String bag_count = "bag_count";

            ///<summary>家族领取俸禄状态</summary>
            public const String daySalary = "daySalary";

            ///<summary>家族每天捐献金钱</summary>
            public const String donate = "donate";

            ///<summary>使用剑击杀胜利次数</summary>
            public const String sword_win_count = "sword_win_count";

            ///<summary>使用枪击杀胜利次数</summary>
            public const String gun_win_count = "gun_win_count";

            ///<summary>使用茶席次数</summary>
            public const String tea_table_count = "tea_table_count";

            ///<summary>讲价成功次数</summary>
            public const String bargain_success_count = "bargain_success_count";

            ///<summary>家臣任务刷新次数</summary>
            public const String task_role_refresh = "task_role_refresh";

            ///<summary>翻将次数</summary>
            public const String npc_refresh_count = "npc_refresh_count";

            ///<summary>武将已经议价次数</summary>
            public const String bargain_count = "bargain_count";

            ///<summary>爬塔挑战次数</summary>
            public const String challenge_count = "challenge_count";

            ///<summary></summary>
            public const String salary_state = "salary_state";

            ///<summary>购买体力次数</summary>
            public const String power_buy_count = "power_buy_count";

            ///<summary>身份替身后是否继续原来身份的任务  0：是 1：否</summary>
            public const String task_vocation_isgo = "task_vocation_isgo";

            ///<summary>雇佣id</summary>
            public const String hire_id = "hire_id";

            ///<summary>雇佣到达时间</summary>
            public const String hire_time = "hire_time";

            ///<summary>酒馆招募刷新冷却时间</summary>
            public const String recruit_time = "recruit_time";

            ///<summary>防沉迷 0:未验证 1:已验证</summary>
            public const String fcm = "fcm";

            ///<summary>大名令  0:未激活 1:已激活 </summary>
            public const String dml = "dml";

            ///<summary>辩才游戏使用次数</summary>
            public const String eloquence_count = "eloquence_count";

            ///<summary>茶道游戏使用次数</summary>
            public const String tea_count = "tea_count";

            ///<summary>算术游戏使用次数</summary>
            public const String calculate_count = "calculate_count";

            ///<summary>忍术游戏使用次数</summary>
            public const String ninjutsu_count = "ninjutsu_count";

            ///<summary>打气球次数</summary>
            public const String ball_count = "ball_count";

            ///<summary>每日完成次数</summary>
            public const String game_finish_count = "game_finish_count";

            ///<summary>小游戏领取奖励状态 0：未领取  1：可领取  2：已领取</summary>
            public const String game_receive = "game_receive";

            ///<summary>武将宅刷新次数</summary>
            public const String refresh_count = "refresh_count";

            ///<summary>武将宅偷窃失败次数</summary>
            public const String steal_fail_count = "steal_fail_count";

            ///<summary>武将宅已挑战次数</summary>
            public const String fight_count = "fight_count";

            ///<summary>武将宅已购买挑战次数</summary>
            public const String fight_buy = "fight_buy";

        }
        #endregion
    }

    /// <summary>玩家扩展表接口</summary>
    public partial interface Itg_user_extend
    {
        #region 属性
        /// <summary></summary>
        Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        Int64 user_id { get; set; }

        /// <summary>武将修行栏开启数</summary>
        Int32 train_bar { get; set; }

        /// <summary>职业任务刷新</summary>
        Int32 task_vocation_refresh { get; set; }

        /// <summary>名塔闯关次数</summary>
        Int32 shot_count { get; set; }

        /// <summary>背包格子总数</summary>
        Int32 bag_count { get; set; }

        /// <summary>家族领取俸禄状态</summary>
        Int32 daySalary { get; set; }

        /// <summary>家族每天捐献金钱</summary>
        Int32 donate { get; set; }

        /// <summary>使用剑击杀胜利次数</summary>
        Int32 sword_win_count { get; set; }

        /// <summary>使用枪击杀胜利次数</summary>
        Int32 gun_win_count { get; set; }

        /// <summary>使用茶席次数</summary>
        Int32 tea_table_count { get; set; }

        /// <summary>讲价成功次数</summary>
        Int32 bargain_success_count { get; set; }

        /// <summary>家臣任务刷新次数</summary>
        Int32 task_role_refresh { get; set; }

        /// <summary>翻将次数</summary>
        Int32 npc_refresh_count { get; set; }

        /// <summary>武将已经议价次数</summary>
        Int32 bargain_count { get; set; }

        /// <summary>爬塔挑战次数</summary>
        Int32 challenge_count { get; set; }

        /// <summary></summary>
        Int32 salary_state { get; set; }

        /// <summary>购买体力次数</summary>
        Int32 power_buy_count { get; set; }

        /// <summary>身份替身后是否继续原来身份的任务  0：是 1：否</summary>
        Int32 task_vocation_isgo { get; set; }

        /// <summary>雇佣id</summary>
        Int64 hire_id { get; set; }

        /// <summary>雇佣到达时间</summary>
        Int64 hire_time { get; set; }

        /// <summary>酒馆招募刷新冷却时间</summary>
        Int64 recruit_time { get; set; }

        /// <summary>防沉迷 0:未验证 1:已验证</summary>
        Int32 fcm { get; set; }

        /// <summary>大名令  0:未激活 1:已激活 </summary>
        Int32 dml { get; set; }

        /// <summary>辩才游戏使用次数</summary>
        Int32 eloquence_count { get; set; }

        /// <summary>茶道游戏使用次数</summary>
        Int32 tea_count { get; set; }

        /// <summary>算术游戏使用次数</summary>
        Int32 calculate_count { get; set; }

        /// <summary>忍术游戏使用次数</summary>
        Int32 ninjutsu_count { get; set; }

        /// <summary>打气球次数</summary>
        Int32 ball_count { get; set; }

        /// <summary>每日完成次数</summary>
        Int32 game_finish_count { get; set; }

        /// <summary>小游戏领取奖励状态 0：未领取  1：可领取  2：已领取</summary>
        Int32 game_receive { get; set; }

        /// <summary>武将宅刷新次数</summary>
        Int32 refresh_count { get; set; }

        /// <summary>武将宅偷窃失败次数</summary>
        Int32 steal_fail_count { get; set; }

        /// <summary>武将宅已挑战次数</summary>
        Int32 fight_count { get; set; }

        /// <summary>武将宅已购买挑战次数</summary>
        Int32 fight_buy { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}