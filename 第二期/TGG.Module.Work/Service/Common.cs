using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        /// <summary> 将dynamic对象转换成ASObject对象 </summary>
        public List<ASObject> ConvertListASObject(dynamic list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                model = EntityToVo.ToVocationTaskVo(item);
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }


        /// <summary> 获取奖励字符串 </summary>
        public string GetRewardString(string maxc, string maxr, string mc, string mr, string reward, RoleItem role)
        {
            role = role.CloneEntity();
            if (TaskCommon.CheckRewardCondition(maxc, role))
                return maxr;
            return TaskCommon.CheckRewardCondition(mc, role)
                ? mr : reward;
        }

        public void TaskUpdate(tg_task task, string newstep, string basestep)
        {
            task.task_step_data = newstep;
            if (newstep == basestep)
                task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
        }

        /// <summary>
        /// 获取玩家工作信息，如果没有该玩家数据，默认插入一条数据
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public Variable.UserTaskInfo GetWorkInfo(Int64 userid)
        {
            var info = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
            if (info != null) return info;
            var newone = new Variable.UserTaskInfo(userid);
            Variable.WorkInfo.Add(newone);
            return newone;
        }


        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            return new Dictionary<string, object>
            {
                {"result", result},
                {"workVo", newtask == null ? null : EntityToVo.ToVocationTaskVo(newtask)}
            };
        }

        /// <summary>
        /// 初始化同类型工作任务
        /// </summary>
        /// <param name="task"></param>
        public tg_task WorkTasksInit(tg_task task)
        {
            var basetask = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == task.task_id);
            if (basetask == null) return task;
            var task_id = basetask.id;
            var step = basetask.stepInit;
            var step_type = basetask.stepType;
            if (basetask.stepType == (int)TaskStepType.FIGHTING_CONTINUOUS ||
                basetask.stepType == (int)TaskStepType.SEARCH_GOODS
                || basetask.stepType == (int)TaskStepType.ESCORT || basetask.stepType == (int)TaskStepType.RUMOR
                || basetask.stepType == (int)TaskStepType.FIRE || basetask.stepType == (int)TaskStepType.BREAK
                || basetask.stepType == (int)TaskStepType.SEll_WINE ||
                basetask.stepType == (int)TaskStepType.ASSASSINATION
                || basetask.stepType == (int)TaskStepType.GUARD || basetask.stepType == (int)TaskStepType.ARREST_RUMOR
                || basetask.stepType == (int)TaskStepType.ARREST_FIRE ||
                basetask.stepType == (int)TaskStepType.ARREST_BREAK
                || basetask.stepType == (int)TaskStepType.ARREST_SEll_WINE ||
                basetask.stepType == (int)TaskStepType.STAND_GUARD)
            {
                var basetasks = Variable.BASE_TASKVOCATION.Where(q => q.stepType == basetask.stepType).ToList();

                var newtask = RandomTask(basetasks, basetask.stepType);
                if (newtask == null) return task;
                task_id = newtask.id;
                step = newtask.stepInit;
                step_type = newtask.stepType;
            }
            task = new TGTask().BuildSpecialTask(step, step_type, task_id, task.user_id, task.task_base_identify, (int)TaskType.WORK_TASK);
            var a = true;
            Variable.CD.TryRemove(string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, task.user_id, task.id), out a);
            return task;
        }



        /// <summary>
        /// 随机在同类型任务中取出一个
        /// </summary>
        /// <param name="entitytasks">任务实体</param>
        /// <param name="basetasks"></param>
        /// <param name="steptype"></param>
        /// <returns></returns>
        public static BaseTaskVocation RandomTask(IEnumerable<BaseTaskVocation> basetasks, int steptype)
        {
            var s_task = basetasks.Where(q => q.stepType == steptype).ToList();
            if (!s_task.Any()) return null;
            var indextask = RNG.Next(0, s_task.Count - 1);
            return s_task[indextask];

        }


        #region 筹措资金任务 (商人)

        /// <summary>筹措资金任务线程</summary>
        public void RaiseTaskThread(tg_task task)
        {
            var time = task.task_endtime - task.task_starttime;
#if DEBUG
            //time = 300000;
#endif
            var token = new CancellationTokenSource();

            Object obj = new WorkTaskObject { user_id = task.user_id, tid = task.id, time = time };

            System.Threading.Tasks.Task.Factory.StartNew(m =>
            {
                var t = m as WorkTaskObject;
                if (t == null) return;
                var key = t.GetKey();
                Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                SpinWait.SpinUntil(() =>
                {
                    var b = Convert.ToBoolean(Variable.CD[key]);
                    return b;
                }, Convert.ToInt32(t.time));
            }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var t = n as WorkTaskObject;
                    if (t == null) return;

                    var tid = t.tid;
                    var _task = tg_task.FindByid(tid);

                    var entity = (new Share.TGTask()).Ananalyze(_task.task_step_data);
                    if (entity == null) return;
                    if (entity.total > entity.count)//未完成
                    {
                        entity.count = 0;
                        _task.task_state = (int)TaskStateType.TYPE_UNRECEIVED;
                        _task.task_step_data = entity.GetRaiseStepData();
                        _task.Save();
                        (new Share.Work()).AdvancedWorkPush(_task.user_id, _task);
                    }
                    else
                    {
                        //发放奖励
                        _task.task_state = (int)TaskStateType.TYPE_REWARD;
                        _task.Save();
                        (new Share.Work()).AdvancedWorkPush(_task.user_id, _task);
                    }

                    //移除全局变量
                    var key = t.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);

                    token.Cancel();
                }, obj, token.Token);
        }

        class WorkTaskObject
        {
            public Int64 user_id { get; set; }
            public Int64 tid { get; set; }
            public Int64 time { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, user_id, tid);
            }

        }


        #endregion


        /// <summary>
        /// 清空任务时间
        /// </summary>
        /// <param name="userid"></param>
        public void ClearTime(Int64 userid)
        {
            var g_work = GetWorkInfo(userid);
            g_work.LimitTime = 0;

        }

        /// <summary>任务倒计时线程</summary>
        public void LimitTimeThreading(int time, tg_task worktask)
        {

            if (worktask.task_step_type == (int)TaskStepType.RAISE_COIN) return;
            try
            {
                time = time * 1000;
                var obj = new WorkObject { user_id = worktask.user_id, task_id = worktask.id, time = time, };
                var token = new CancellationTokenSource();

                Task.Factory.StartNew(m =>
                {
                    var wo = m as WorkObject;
                    if (wo == null) return;
                    SpinWait.SpinUntil(() =>
                    {
                        var g_work = GetWorkInfo(wo.user_id);
                        return g_work.LimitTime == 0;
                    }, wo.time);
                }, obj, token.Token).ContinueWith((m, n) =>
                {
                    var wo = n as WorkObject;
                    if (wo == null) { token.Cancel(); return; }
                    TimeEnd(Convert.ToInt64(wo.task_id));
                    token.Cancel();
                }, obj, token.Token);

            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }



        /// <summary>
        /// 倒计时结束 任务初始
        /// </summary>
        private void TimeEnd(Int64 task_id)
        {
            var task = tg_task.FindByid(task_id);
            Variable.CD.AddOrUpdate(string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, task.user_id, task.id), true, (m, n) => true);
            var mainid = task.id;
            var WorkInfo = WorkTasksInit(task);
            WorkInfo.id = mainid;
            WorkInfo.Update();
            (new Share.Work()).AdvancedWorkPush(task.user_id, WorkInfo);//推送协议
        }

        class WorkObject
        {
            public Int64 user_id { get; set; }
            public Int64 task_id { get; set; }

            public Int32 time { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, user_id, task_id);
            }
        }
    }
}
