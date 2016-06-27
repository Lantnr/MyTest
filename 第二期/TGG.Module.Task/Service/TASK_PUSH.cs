using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share.Event;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 任务推送
    /// </summary>
    public class TASK_PUSH
    {
        public static TASK_PUSH objInstance = null;

        /// <summary>
        /// TASK_PUSH单体模式
        /// </summary>
        /// <returns></returns>
        public static TASK_PUSH getInstance()
        {
            if (objInstance == null) objInstance = new TASK_PUSH();
            return objInstance;
        }


        /// <summary> 日常任务每日刷新 </summary>
        public void VocationTaskUpdate()
        {
#if DEBUG
            var stopwatch = new Stopwatch(); //creates and start the instance of Stopwatch
            stopwatch.Start();
#endif
            var newtasks = new List<tg_task>();
            GetReward();
            var userroles = view_user_role_task.GetAll();
            Variable.TaskInfo.Clear();
            foreach (var usernewtasks in userroles)
            {
                var basetasks = Common.getInstance().
                    GetNewVocationTasks(usernewtasks.role_identity, usernewtasks.user_id, usernewtasks.player_vocation).ToList();
                newtasks.AddRange(basetasks);
                var s_tasks = Common.getInstance().SpecialTasksInit(usernewtasks);
                if (s_tasks != null)
                    newtasks.AddRange(s_tasks);
            }

            tg_task.GetListInsert(newtasks);
            #region 在线则推送数据
            foreach (var item in Variable.OnlinePlayer.Keys)
            {
                var item1 = item;
                var dic = new Dictionary<string, object>
                {
                    {"1", Common.getInstance().ConvertListASObject(newtasks.Where(q=>q.user_id==item1), "VocationTask")}
                };
                var aso = new ASObject(dic);
                var session = Variable.OnlinePlayer[item1] as TGGSession;
                //推送数据
                if (session == null) continue;
                var pv = session.InitProtocol((int)ModuleNumber.TASK,
                    (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
            }
            #endregion

#if DEBUG
            var timespan = stopwatch.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());

#endif
        }

        #region 并行计算测试
        public void VocationTaskUpdate1()
        {
#if DEBUG
            var stopwatch = new Stopwatch(); //creates and start the instance of Stopwatch
            stopwatch.Start();
#endif

            var newtasks = new List<tg_task>();
            GetReward();
            var userroles = view_user_role_task.GetAll();
            Variable.TaskInfo.Clear();
            Parallel.ForEach(userroles, (i, j) => GetTaskData(i, newtasks));

            tg_task.GetListInsert(newtasks);
            #region 在线则推送数据

            Parallel.ForEach(Variable.OnlinePlayer.Keys, (i, j) => GetTaskPush(i, newtasks));

            #endregion
#if DEBUG
            var timespan = stopwatch.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());
#endif

        }

        /// <summary>
        /// 获取用户任务数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newtasks"></param>
        private void GetTaskPush(Int64 item, List<tg_task> newtasks)
        {
            var item1 = item;
            var dic = new Dictionary<string, object>
                {
                    {"1", Common.getInstance().ConvertListASObject(newtasks.Where(q=>q.user_id==item1), "VocationTask")}
                };
            var aso = new ASObject(dic);
            var session = Variable.OnlinePlayer[item1] as TGGSession;
            //推送数据
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);

        }

        /// <summary>
        /// 推送在线玩家数据
        /// </summary>
        /// <param name="usernewtasks"></param>
        /// <param name="newtasks"></param>
        private void GetTaskData(view_user_role_task usernewtasks, List<tg_task> newtasks)
        {
            var basetasks = Common.getInstance().
                 GetNewVocationTasks(usernewtasks.role_identity, usernewtasks.user_id, usernewtasks.player_vocation).ToList();
            newtasks.AddRange(basetasks);
            var s_tasks = Common.getInstance().SpecialTasksInit(usernewtasks);
            if (s_tasks != null)
                newtasks.AddRange(s_tasks);
        }

        #endregion

        /// <summary> 已经完成的任务领取奖励 </summary>
        private void GetReward()
        {
            try
            {
                var finishtasks = tg_task.GetFinishVocatinTask();
                foreach (var finishtask in finishtasks)
                {
                    var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == finishtask.task_id);
                    var userid = finishtask.user_id;
                    if (Variable.OnlinePlayer.ContainsKey(userid))
                    {
                        var session = Variable.OnlinePlayer[userid] as TGGSession;
                        if (baseinfo == null) continue;
                        var rewardstring = Common.getInstance().GetRewardString(baseinfo, session);
                        new Share.Reward().GetReward(rewardstring, userid);
                    }
                    else//玩家不在线
                    {
                        if (baseinfo != null) new Share.Reward().GetRewardNotOnline(baseinfo.reward, finishtask.user_id);
                    }
                }
                tg_task.GetVocationTaskDel();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
