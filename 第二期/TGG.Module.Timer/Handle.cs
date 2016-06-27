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

        /// <summary>构造函数</summary>
        public Handle()
        {
            (new Common()).Init();
            InitTimer();
            //不应该在这里初始化
            InitRoleHire();
            InitNoticeTask();
        }

        /// <summary>模块启动</summary>
        public void Start(int count)
        {
            DisplayGlobal.log.Write(string.Format("进入定时模块 当前计数{0}", count));
            CommonHelper.GetCDTSTM(GetType().Namespace, count, true);
            (new Share.Business()).InitGoods();
            _globalTimer.Enabled = true;
        }

        /// <summary>停止</summary>
        public void Stop(int count)
        {
            DisplayGlobal.log.Write(string.Format("退出定时模块 当前计数{0}", count));
            CommonHelper.GetCDTSTMRemove(GetType().Namespace, count);
            _globalTimer.Enabled = false;       //是否执行System.Timers.Timer.Elapsed事件;   
        }

        /// <summary>初始化全局定时器</summary>
        private void InitTimer()
        {
            _globalTimer.Interval = 5000;
            _globalTimer.Elapsed += ExecuteTimer;//到达时间的时候执行事件;
            _globalTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true);  
            _globalTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;   
        }

        /// <summary>全局定时执行方法</summary>
        private void ExecuteTimer(object source, ElapsedEventArgs e)
        {
            (new Common()).ExecuteTimer();
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







    }

}
