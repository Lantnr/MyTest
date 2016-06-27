using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using TGG.Share;

namespace TGG.Module.Timer
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public partial class Common
    {
        /// <summary>初始化</summary>
        public void Init()
        {
            InitFT();
            InitTPR();
            InitART();
            InitActivityTime();
            InitPowerBuffTime();
            CheckGoods();
        }

        /// <summary>执行定时方法</summary>
        public void ExecuteTimer()
        {
            FixedTimer();
            CheckGoods();
            CheckPlayerPower();
            CheckBuff();
            CheckScene();
            CheckBuild();
            CheckSiege();
            CheckArena();
            CheckNotice();
            CheckOpenTime();
        }

        /// <summary>固定时间执行方法</summary>
        public void FixedTimer()
        {
            if (DateTime.Now.Ticks < Variable.FT) return;
            InitFT();
            FixedPower();
            FixeUserExtend();
            FixeNpcSpirit();
            FixeDuplicate();
            FixeCheckPoint();
            FixeVipUpdata();
            FixeTask();
            FixeGameReward();
            FixeGame();
        }

        #region 初始化方法

        private void TestInit()
        {
#if DEBUG
            //Variable.Activity.BuildActivity.StartTime = Variable.Activity.BuildActivity.StartTime.AddMinutes(1);         
#endif
        }

        /// <summary>初始化竞技场奖励发放时间</summary>
        private void InitART()
        {
            Variable.ArenaRewardTime = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " 21:00:00");
            if (DateTime.Now > Variable.ArenaRewardTime) Variable.ArenaRewardTime = Variable.ArenaRewardTime.AddDays(1);
        }

        /// <summary>初始化定时体力时间</summary>
        private void InitTPR()
        {
            //Variable.TPR = DateTime.Now.AddMinutes(1).Ticks;//测试
            Variable.TPR = DateTime.Now.AddMinutes(Variable.TPRS).Ticks;
        }

        /// <summary>初始化固定刷新时间</summary>
        private void InitFT()
        {
            //Variable.FT = DateTime.Now.AddMinutes(3).Ticks;//测试
            var t = DateTime.Now.AddHours(Variable.FTS);
            Variable.FT = (new DateTime(t.Year, t.Month, t.Day, 0, 0, 0)).Ticks;
        }

        /// <summary>初始活动时间</summary>
        private void InitActivityTime()
        {
            #region 美浓活动时间初始

            var time = Variable.BASE_RULE.FirstOrDefault(m => m.id == "26005");
            var spaceTime = Variable.BASE_RULE.FirstOrDefault(m => m.id == "26006");
            if (time == null || spaceTime == null) return;
            Variable.Activity.Siege.BaseData.ActivityTime = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " " + time.value);//开始时间
            if (DateTime.Now > Variable.Activity.Siege.BaseData.ActivityTime.AddMinutes(Convert.ToInt32(spaceTime.value) - 5))//验证剩余时间是否足够5分钟
                Variable.Activity.Siege.BaseData.ActivityTime = Variable.Activity.Siege.BaseData.ActivityTime.AddDays(1);
            Variable.Activity.Siege.BaseData.ActivityDuration = Convert.ToInt32(spaceTime.value);//活动时长
            Variable.Activity.Siege.BaseData.ActivityEndTime = Variable.Activity.Siege.BaseData.ActivityTime.AddMinutes(Convert.ToInt16(spaceTime.value));//结束时间

            #endregion

            #region 一夜墨俣活动时间初始

            (new Building()).InitBulid();

            #endregion

        }


        /// <summary> 初始化体力buff时间 </summary>
        private void InitPowerBuffTime()
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "7034");
            if (baseinfo == null) return;
            var baseinfo1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "7035");
            if (baseinfo1 == null) return;

            Variable.PowerBuffTime1 = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " " + baseinfo.value);//更新时间1
            // Variable.PowerBuffTime1 = DateTime.Now.AddMinutes(2);//测试数据
            if (DateTime.Now > Variable.PowerBuffTime1)
                Variable.PowerBuffTime1 = Variable.PowerBuffTime1.AddDays(1);//如果当前时间大于活动时间，刷新时间改为第二天
            Variable.PowerBuffTime2 = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " " + baseinfo1.value);//更新时间2
            if (DateTime.Now > Variable.PowerBuffTime2)
                Variable.PowerBuffTime2 = Variable.PowerBuffTime2.AddDays(1);//如果当前时间大于活动时间，刷新时间改为第二天
        }

        #endregion

        #region 定时执行方法

        /// <summary>检查执行货物方法</summary>
        private void CheckGoods()
        {
            double timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (!(timeStamp >= Variable.GRWT)) return;
            var _base = Variable.BASE_RULE.FirstOrDefault(m => m.id == "3001");
            if (_base == null) return;
            var temp = _base.value.Split('-');
            var reflashtime = RNG.Next(int.Parse(temp[0]), int.Parse(temp[1]));
            var interval = 1000 * 60 * reflashtime;
            //var interval = 1000 * 60 * 10;    //测试数据
            Variable.GRWT = Variable.GRT = CommonHelper.StopTime(interval) + (Variable.GRWTS * 1000);
            (new Business()).ReflashGoods();
        }

        /// <summary>定时体力刷新</summary>
        private void CheckPlayerPower()
        {
            if (DateTime.Now.Ticks < Variable.TPR) return;
            InitTPR();
            try
            {
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1008");
                var rule_max = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1009");
                if (rule == null || rule_max == null) return;
                var power = Convert.ToInt32(rule.value);
                var max = Convert.ToInt32(rule_max.value);
                var b = tg_role.UpdateTimerPower(power, max);
                if (!b) return;

                #region 在线则推送数据
                foreach (var item in Variable.OnlinePlayer.Keys)
                {
                    var session = Variable.OnlinePlayer[item] as TGGSession;
                    if (session == null) return;
                    var temp = session.Player.Role.Kind.power + power;
                    var powernow = temp > max ? max : temp;
                    session.Player.Role.Kind.power = powernow;
                    var list = new List<string> { "power", "rolepower" };
                    var role = session.Player.Role.Kind;
                    var user_id = session.Player.User.id;
                    var obj = new TaskObject { role = role, user_id = user_id, list = list };
                    var token = new CancellationTokenSource();
                    Task.Factory.StartNew(m =>
                    {
                        var entity = m as TaskObject;
                        if (entity == null) return;
                        (new RoleAttUpdate()).RoleUpdatePush(entity.role, entity.user_id, entity.list);
                        token.Cancel();
                    }, obj, token.Token);

                }
                #endregion
            }
            catch
            {
                XTrace.WriteLine("定时主角体力恢复错误");
            }

        }

        /// <summary>体力buff发放</summary>
        private void CheckBuff()
        {
            BuffFirst();
            BuffSecend();
        }

        /// <summary>检查执行场景</summary>
        private void CheckScene()
        {
            try
            {
                if (DateTime.Now < Variable.SceneTimer) return;
                var list = Variable.SCENCE;
                Variable.SceneTimer = DateTime.Now.AddHours(Variable.SceneTimerSpan);
                if (!list.Any()) return;
                foreach (var item in list.Keys)
                {
                    tg_scene.GetSceneUpdate(list[item]);
                }
                Variable.SceneTimer = DateTime.Now.AddHours(Variable.TimerExecHourSpan);

            }
            catch
            {
                XTrace.WriteLine("定时检查执行场景错误");
            }
        }

        /// <summary>一夜墨俣活动开始检测</summary>
        private void CheckBuild()
        {
            try
            {
                if (DateTime.Now < Variable.Activity.BuildActivity.StartTime || DateTime.Now > Variable.Activity.BuildActivity.EndTime) return;
                //初始活动数据
                (new Building()).Init();
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Building", "Common");
                obje.ActivityStart();
                Variable.Activity.BuildActivity.StartTime = Variable.Activity.BuildActivity.StartTime.AddHours(Variable.TaskTimerSpan);
            }
            catch
            {
                XTrace.WriteLine("一夜墨俣活动开始检测错误");
            }
        }

        /// <summary> 美浓活动开启和关闭 </summary>
        private void CheckSiege()
        {
            try
            {
                SiegeOpen();
                SiegeClose();
            }
            catch (Exception)
            {
                XTrace.WriteLine("美浓活动开启和关闭错误");
            }
        }

        /// <summary> 竞技场定时方法 </summary>
        private void CheckArena()
        {
            ArenaUpdate();
            ArenaReward();
        }

        /// <summary>公告</summary>
        public void CheckNotice()
        {
            try
            {
                ActivityNotice();
                SystemNotice();
            }
            catch (Exception) { XTrace.WriteLine("公告错误！！"); }
        }

        /// <summary>检查防沉迷时间</summary>
        private void CheckOpenTime()
        {
            try
            {
                if (!CommonHelper.CheckOpenTime()) return;
                if (DateTime.Now < Variable.LoginTimeCheck) return;
                (new User()).UserLoginTimeCheck();
                Variable.LoginTimeCheck = Variable.LoginTimeCheck.AddMinutes(1);
            }
            catch { XTrace.WriteLine("防沉迷检测"); }

        }

        #endregion

        #region 固定十二点执行方法

        /// <summary>武将体力每日刷新</summary>
        private void FixedPower()
        {
            try
            {
                RolePowerUpdate();
            }
            catch { XTrace.WriteLine("武将体力每日定时刷新错误"); }
        }

        /// <summary>更新用户拓展表信息</summary>
        private void FixeUserExtend()
        {
            try
            {
                SetUserTimerUpdate();
            }
            catch { XTrace.WriteLine("定时更新用户拓展表信息错误"); }
        }

        /// <summary>更新武将宅NPC信息</summary>
        private void FixeNpcSpirit()
        {
            try
            {
                tg_train_home.UpdateNpcSpirit();
            }
            catch { XTrace.WriteLine("定时更新武将宅NPC信息错误"); }
        }

        /// <summary>更新爬塔重置数据</summary>
        private void FixeCheckPoint()
        {
            try
            {
                tg_duplicate_checkpoint.GetUpdate();
            }
            catch { XTrace.WriteLine("定时更新爬塔重置数据错误"); }
        }

        /// <summary>定时VIP更新</summary>
        public void FixeVipUpdata()
        {
            try
            {
                var list = Variable.BASE_VIP;
                foreach (var item in list)
                {
                    var vip = new tg_user_vip
                    {
                        power = item.power,
                        bargain = item.bargain,
                        buy = item.buy,
                        arena_buy = 0,
                        arena_cd = 0,
                        train_home = item.trainHome,
                        vip_level = item.level,
                    };
                    tg_user_vip.GetUpdateByLevel(vip);
                }
            }
            catch { XTrace.WriteLine("VIP定时重置错误！"); }
        }

        /// <summary> 爬塔每日发放守护者声望奖励 </summary>
        private void FixeDuplicate()
        {
            try
            {
                (new Duplicate()).WatchmenReward();
            }
            catch { XTrace.WriteLine("爬塔守护者发放奖励错误"); }
        }

        /// <summary>执行任务</summary>
        private void FixeTask()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Task", "TASK_PUSH");
                obje.VocationTaskUpdate();
                dynamic obje1 = CommonHelper.ReflectionMethods("TGG.Module.Appraise", "Common");
                obje1.RoleTaskReflash();
            }
            catch { XTrace.WriteLine("定时执行任务错误"); }
        }

        /// <summary> 小游戏每周日奖励 </summary>
        private void FixeGameReward()
        {
            try
            {
                if (Convert.ToInt32(DateTime.Now.DayOfWeek) != 0) return;
                (new ExpansionCommand()).SendYouYiYuanReward();
            }
            catch { XTrace.WriteLine("定时执行任务错误"); }
        }

        /// <summary>小游戏每周日重置数据</summary>
        private void FixeGame()
        {
            try
            {
                if (Convert.ToInt32(DateTime.Now.DayOfWeek) != 0) return;
                tg_game.GetUpdate();
            }
            catch { XTrace.WriteLine("定时更新游艺园重置数据错误"); }
        }

        #endregion

        #region 私有公共方法
        /// <summary>第一次Buff体力发放</summary>
        private void BuffFirst()
        {
            if (DateTime.Now < Variable.PowerBuffTime1) return;
            BuffPowerUpdate();
            Variable.PowerBuffTime1 = Variable.PowerBuffTime1.AddHours(Variable.TaskTimerSpan);
        }

        /// <summary>第二次Buff体力发放</summary>
        private void BuffSecend()
        {
            if (DateTime.Now < Variable.PowerBuffTime2) return;
            BuffPowerUpdate();
            Variable.PowerBuffTime2 = Variable.PowerBuffTime2.AddHours(Variable.TaskTimerSpan);
        }

        /// <summary> 体力buff日常更新 </summary>
        public void BuffPowerUpdate()
        {
            try
            {
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7033");
                if (rule == null) return;
                var buffvalue = Convert.ToInt32(rule.value);
                tg_role.GetPowerBuffUpdate(buffvalue);

                foreach (var item in Variable.OnlinePlayer.Keys)
                {
                    var session = Variable.OnlinePlayer[item] as TGGSession;
                    if (session == null) return;
                    session.Player.Role.Kind.buff_power = buffvalue;
                    var list = new List<string> { "power", "rolepower" };
                    var role = session.Player.Role.Kind;
                    var user_id = session.Player.User.id;
                    var obj = new TaskObject { role = role, user_id = user_id, list = list };
                    var token = new CancellationTokenSource();
                    Task.Factory.StartNew(m =>
                    {
                        var entity = m as TaskObject;
                        if (entity == null) return;
                        (new RoleAttUpdate()).RoleUpdatePush(entity.role, entity.user_id, entity.list);
                        token.Cancel();
                    }, obj, token.Token);

                }

            }
            catch { XTrace.WriteLine("体力buff日常更新错误"); }
        }

        /// <summary> 美浓活动开启 </summary>
        private void SiegeOpen()
        {
            if (DateTime.Now < Variable.Activity.Siege.BaseData.ActivityTime) return;
            new Activity().InitSiegeBase();
            Variable.Activity.Siege.BaseData.ActivityTime = Variable.Activity.Siege.BaseData.ActivityTime.AddDays(1);
        }

        /// <summary> 美浓活动关闭 </summary>
        private void SiegeClose()
        {
            if (DateTime.Now < Variable.Activity.Siege.BaseData.ActivityEndTime) return;
            Variable.Activity.Siege.BaseData.ActivityEndTime = Variable.Activity.Siege.BaseData.ActivityEndTime.AddDays(1);
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Siege", "Common");
            obje.SiegeEnd();
        }

        /// <summary>竞技场每日可挑战次数初始化</summary>
        private void ArenaUpdate()
        {
            try
            {
                if (DateTime.Now < Variable.ArenaRefreshTime) return;
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "23001");
                if (rule == null) { XTrace.WriteLine("{0}:{1}", "CheckArena()", "竞技场固定规则表23001为空！"); return; }
                var count = Convert.ToInt32(rule.value);
                if (!tg_arena.UpdateArenaCount(count))
                    XTrace.WriteLine("{0}:{1}", "CheckArena()", "竞技场每日可挑战次数初始化数据库操作异常！");
                Variable.ArenaRefreshTime = Variable.ArenaRefreshTime.AddDays(1);
            }
            catch { XTrace.WriteLine("竞技场每日可挑战次数重置刷新错误"); }

        }

        /// <summary> 竞技场每日排名奖励发放 </summary>
        private void ArenaReward()
        {
            try
            {
                if (DateTime.Now < Variable.ArenaRewardTime) return;
                RankingReward();
                Variable.ArenaRewardTime = Variable.ArenaRewardTime.AddDays(1);
            }
            catch { XTrace.WriteLine("竞技场每日排名奖励发放错误"); }
        }

        /// <summary>排名奖励</summary>
        private void RankingReward()
        {
            var list = tg_arena.GetEntityList(200);
#if DEBUG
            XTrace.WriteLine("{0}", "竞技场排名要发放的数量:" + list.Count());
#endif
            if (!list.Any()) return;
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new PushObject
                {
                    ranking = item.ranking,
                    user_id = item.user_id,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as PushObject;
                    if (_obj == null) return;
                    RankRewardSend(_obj.ranking, _obj.user_id);
                    token.Cancel();
                }, obj, token.Token);
            }
        }

        /// <summary> 排名奖励推送 </summary>
        private void RankRewardSend(int ranking, Int64 userid)
        {
            var model = Variable.BASE_ARENARANKREWARD.FirstOrDefault(m => m.ranKing == ranking);
            if (model == null) return;
            string temp = (int)GoodsType.TYPE_COIN + "_" + model.money + "|" + (int)GoodsType.TYPE_FAME + "_" + model.fame;
            (new Message()).BuildMessagesSend(userid, "竞技场排名奖励", string.Format("竞技场第{0}名奖励发放", ranking), temp);
#if DEBUG
            XTrace.WriteLine("{0}", "竞技场奖励发放当前发放排名:" + ranking + "   发放成功！！");
#endif
        }

        /// <summary>活动公告</summary>
        private void ActivityNotice()
        {
            if (DateTime.Now < Variable.NOTICETIME) return;
            (new Notice()).AddNewNotice();
            Variable.NOTICETIME = Variable.NOTICETIME.AddDays(1);
        }

        /// <summary>后台公告</summary>
        private void SystemNotice()
        {
            if (Variable.GetNoticeTime > DateTime.Now.Ticks) return;
            (new Notice()).NewNoticeAddTask();
            Variable.GetNoticeTime = DateTime.Now.AddMinutes(5).Ticks;
        }

        /// <summary>每天整点家臣体力恢复</summary>
        private void RolePowerUpdate()
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7003");
            var rule_max = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7011");
            if (rule == null || rule_max == null) return;
            var power = Convert.ToInt32(rule.value);
            var max = Convert.ToInt32(rule_max.value);
            tg_role.UpdateRolePower(power, max);
        }

        /// <summary>玩家扩展数据重置</summary>
        private void SetUserTimerUpdate()
        {
            var _s = Variable.BASE_RULE.FirstOrDefault(q => q.id == "9002");
            if (_s == null) return;
            var _n = Variable.BASE_RULE.FirstOrDefault(q => q.id == "9008");
            if (_n == null) return;
            var shot_count = Convert.ToInt32(_s.value);
            var npc_refresh_count = Convert.ToInt32(_n.value);
            var model = new tg_user_extend
            {
                daySalary = 0,
                donate = 0,
                task_role_refresh = 0,
                task_vocation_refresh = 0,
                shot_count = shot_count,
                bargain_count = 0,
                npc_refresh_count = npc_refresh_count,
                challenge_count = 1,
                power_buy_count = 0,
                task_vocation_isgo = 0,
                salary_state = (int)SalaryStateType.NO_RECEIVE,
                eloquence_count = 0,
                tea_count = 0,
                ninjutsu_count = 0,
                calculate_count = 0,
                ball_count = 0,
                game_finish_count = 0,
                game_receive = 0,
                refresh_count = 0,
                steal_fail_count = 0,
                fight_count = 0,
                fight_buy = 0,
            };
            tg_user_extend.GetEntityUpdate(model);
            tg_user_login_log.GetTimerUpdate();
            OnlineReset(model);
        }

        /// <summary>在线玩家扩展信息重置</summary>
        private void OnlineReset(tg_user_extend extend)
        {
            foreach (var player in Variable.OnlinePlayer.Values)
            {
                var session = player as TGGSession;
                if (session == null) continue;
                var ext = session.Player.UserExtend;
                ext.daySalary = extend.daySalary;
                ext.donate = extend.donate;
                ext.task_role_refresh = extend.task_role_refresh;
                ext.task_vocation_refresh = extend.task_vocation_refresh;
                ext.shot_count = extend.shot_count;
                ext.bargain_count = extend.bargain_count;
                ext.npc_refresh_count = extend.npc_refresh_count;
                ext.challenge_count = extend.challenge_count;
                ext.power_buy_count = extend.power_buy_count;
                ext.task_vocation_isgo = extend.task_vocation_isgo;
                ext.salary_state = extend.salary_state;
                ext.eloquence_count = extend.eloquence_count;
                ext.tea_count = extend.tea_count;
                ext.calculate_count = extend.calculate_count;
                ext.ninjutsu_count = extend.ninjutsu_count;
                ext.ball_count = extend.ball_count;
                ext.game_finish_count = extend.game_finish_count;
                ext.game_receive = extend.game_receive;
                ext.refresh_count = extend.refresh_count;
                ext.steal_fail_count = extend.steal_fail_count;
                ext.fight_count = extend.fight_count;
                ext.fight_buy = extend.fight_buy;
                session.Player.UserExtend = ext;
            }
        }

        #endregion

        class TaskObject
        {
            public tg_role role { get; set; }
            public Int64 user_id { get; set; }
            public List<string> list { get; set; }
        }

        class PushObject
        {
            public Int64 user_id { get; set; }

            public int ranking { get; set; }
        }

    }
}
