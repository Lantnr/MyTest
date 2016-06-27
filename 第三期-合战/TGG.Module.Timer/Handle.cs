using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Vo.Role;
using TGG.Share.Event;
using TGG.SocketServer;
using TGG.Share;

namespace TGG.Module.Timer
{
    /// <summary>
    /// 定时模块
    /// </summary>
    public class Handle
    {

        private static Handle ObjInstance;

        /// <summary>Handle单例模式</summary>
        public static Handle GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Handle());
        }

        /// <summary>全局定时器</summary>
        System.Timers.Timer _globalTimer = new System.Timers.Timer();

        /// <summary>货物定时器</summary>
        System.Timers.Timer _goodsTimer = new System.Timers.Timer();

        /// <summary>体力定时器</summary>
        System.Timers.Timer _powerTimer = new System.Timers.Timer();

        /// <summary>24点定时器</summary>
        System.Timers.Timer _timer_24 = new System.Timers.Timer();

        /// <summary>合战定时器</summary>
        System.Timers.Timer _war_timer = new System.Timers.Timer();

        /// <summary>活动定时器</summary>
        System.Timers.Timer _activity_timer = new System.Timers.Timer();

        /// <summary>构造函数</summary>
        public Handle()
        {
            var common = new Common();
            common.Init();
            common.Dispose();
            InitTimer();
            //不应该在这里初始化
            InitRoleHire();
            InitNoticeTask();
            InitWarCityAll();
            InitWarCarryTask();
            InitWarRoleTask();
            InitUsers();
        }

        /// <summary>模块启动</summary>
        public void Start(int count)
        {
            DisplayGlobal.log.Write(string.Format("进入定时模块 当前计数{0}", count));
            CommonHelper.GetCDTSTM(GetType().Namespace, count, true);
            var bus = new Business();
            var war = new Share.War();
            bus.InitGoods();
            war.RefreshWarGoods();
            bus.Dispose();
            war.Dispose();
            _globalTimer.Enabled = true;
            _goodsTimer.Enabled = true;
            _powerTimer.Enabled = true;
            _timer_24.Enabled = true;
            _war_timer.Enabled = true;
            _activity_timer.Enabled = true;
        }

        /// <summary>停止</summary>
        public void Stop(int count)
        {
            DisplayGlobal.log.Write(string.Format("退出定时模块 当前计数{0}", count));
            CommonHelper.GetCDTSTMRemove(GetType().Namespace, count);
            _globalTimer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
            _goodsTimer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
            _powerTimer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
            _timer_24.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
            _war_timer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
            _activity_timer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
        }

        /// <summary>初始化全局定时器</summary>
        private void InitTimer()
        {
            _globalTimer.Interval = 10000;
            _globalTimer.Elapsed += ExecuteTimer;//到达时间的时候执行事件;
            _globalTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _globalTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   

            //货物定时器
            _goodsTimer.Interval = 30000;
            _goodsTimer.Elapsed += GoodsExecuteTimer;//到达时间的时候执行事件;
            _goodsTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _goodsTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   

            //体力定时器
            _powerTimer.Interval = 60000;
            _powerTimer.Elapsed += PowerExecuteTimer;//到达时间的时候执行事件;
            _powerTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _powerTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   

            //24点定时器
            _timer_24.Interval = 60000;
            _timer_24.Elapsed += ExecuteTimer_24;//到达时间的时候执行事件;
            _timer_24.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _timer_24.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   

            //合战定时器
            _war_timer.Interval = 300000;
            _war_timer.Elapsed += WarExecuteTimer;//到达时间的时候执行事件;
            _war_timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _war_timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   

            //活动定时器
            _activity_timer.Interval = 30000;
            _activity_timer.Elapsed += ActivityExecuteTimer;//到达时间的时候执行事件;
            _activity_timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _activity_timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   
        }

        /// <summary>全局定时执行方法</summary>
        private void ExecuteTimer(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("全局定时执行方法");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.ExecuteTimer();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }

        /// <summary>货物定时器方法</summary>
        private void GoodsExecuteTimer(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("货物定时器方法");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.CheckGoods();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }


        /// <summary>体力定时器方法</summary>
        private void PowerExecuteTimer(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("体力定时器方法");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.CheckPlayerPower();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }

        /// <summary>24点定时器方法</summary>
        private void ExecuteTimer_24(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("24点定时器");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.FixedTimer();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }

        /// <summary>合战定时器方法</summary>
        private void WarExecuteTimer(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("合战定时器");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.WarTimer();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }


        /// <summary>活动定时器方法</summary>
        private void ActivityExecuteTimer(object source, ElapsedEventArgs e)
        {
#if DEBUG
            XTrace.WriteLine("活动定时器");
            var sw = Stopwatch.StartNew();
#endif
            var common = new Common();
            common.ActivityTimer();
            common.Dispose();
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
        }

        /// <summary> 初始化将雇佣过期的玩家的数据 </summary>
        private void InitRoleHire()
        {
            new Share.Role().InitHire();
        }

        /// <summary> 初始化公告线程 重新加入线程 </summary>
        private void InitNoticeTask()
        {
            (new Share.Notice()).InitNoticeTask();
        }

        /// <summary> 初始化全局合战据点集合 </summary>
        private void InitWarCityAll()
        {
            (new Share.War()).InitWarCity();
        }

        /// <summary> 初始化合战运输队列 </summary>
        private void InitWarCarryTask()
        {
            (new Share.War()).AddCarryTask();
        }

        /// <summary> 初始化合战武将状态 </summary>
        private void InitWarRoleTask()
        {
            (new Share.War()).AddAllWarRoleTask();
        }

        /// <summary> 初始化玩家状态信息 </summary>
        private void InitUsers()
        {
            var now = DateTime.Now.Ticks;
            var user = tg_user.FindAll().ToList();
            var frozens = user.Where(m => m.state == (int)UserStateType.Frozen && m.state_end_time <= now).ToList();
            if (!frozens.Any()) return;

            foreach (var item in frozens)
            {
                item.state = (int)UserStateType.Normal;
                item.state_end_time = 0;
                item.Save();
            }
        }
    }

}
