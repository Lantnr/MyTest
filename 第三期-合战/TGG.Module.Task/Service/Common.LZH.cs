using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Common.Randoms;
using TGG.Core.Global;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 任务部分公共方法类
    /// </summary>
    public partial class Common
    {
        #region 谣言任务

        /// <summary>谣言类任务</summary>
        /// <param name="player">玩家信息 player</param>
        /// <param name="task">任务信息 task</param>
        /// <param name="finishstep">任务步骤 finishstep</param>
        /// <param name="npcid">对话npcid </param>
        /// <returns>ASObject</returns>
        public ASObject RumorTask(Player player, tg_task task, string finishstep, int npcid)
        {
            var myscene = Scene.GetSceneInfo((int)ModuleNumber.PRISON, player.User.id);
            if (myscene != null) CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_PRISON);    //验证是否在监狱中

            //验证对话npc是否正确
            if (!CheckNpc(task.task_step_data, npcid)) return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);
            //验证谣言任务是否成功
            var istrue = (new Share.TGTask()).IsTaskSuccess(player.Role, task.task_step_type);

            var gtask = Variable.TaskInfo.FirstOrDefault(m => m.userid == player.User.id);   //查询全局变量信息
            if (istrue)       //成功  更新任务
            {
                if (!CheckRumorTask(task, finishstep)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (gtask != null) UpdateFailCount(task.task_step_type, gtask);    //更新失败次数
            }
            else
            {
                if (gtask == null) { Variable.TaskInfo.Add(CreatRumorTask(task)); }
                else
                { UpdateAndPush(task, gtask, player.User.id, player.User.player_name, player.Scene.scene_id); }   //更新失败次数并验证是否入狱
            }
            return new ASObject(BulidData((int)ResultType.SUCCESS, task));
        }

        /// <summary>验证对话npc信息</summary>
        private bool CheckNpc(string stepdate, int npcid)
        {
            if (!stepdate.Contains("|") || !stepdate.Contains("_")) return false;
            var step = stepdate.Split("|")[0];
            var npc = step.Split("_")[1];
            return npc == npcid.ToString();
        }

        /// <summary>更新谣言任务</summary>
        private bool CheckRumorTask(tg_task task, string finishstep)
        {
            task.task_step_data = finishstep;
            task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
            return true;
        }

        /// <summary>初始化全局变量谣言失败次数</summary>
        private void UpdateFailCount(int type, Variable.UserTaskInfo taskinfo)
        {
            switch (type)
            {
                case (int)TaskStepType.RUMOR: taskinfo.RumorFailCount = 0; break;
                case (int)TaskStepType.FIRE: taskinfo.FireFailCount = 0; break;
                case (int)TaskStepType.BREAK: taskinfo.BreakFailCount = 0; break;
                case (int)TaskStepType.SEll_WINE: taskinfo.SellFailCount = 0; break;
            }
        }

        /// <summary>更新失败次数并验证入狱</summary>
        private void UpdateAndPush(tg_task task, Variable.UserTaskInfo taskinfo, Int64 userid, string name, Int64 senceid)
        {
            switch (task.task_step_type)
            {
                case (int)TaskStepType.RUMOR: taskinfo.RumorFailCount += 1;
                    CheckIsInPrison(taskinfo.RumorFailCount, userid, name, senceid, task.task_step_type);
                    break;
                case (int)TaskStepType.FIRE: taskinfo.FireFailCount += 1;
                    CheckIsInPrison(taskinfo.FireFailCount, userid, name, senceid, task.task_step_type);
                    break;
                case (int)TaskStepType.BREAK: taskinfo.BreakFailCount += 1;
                    CheckIsInPrison(taskinfo.BreakFailCount, userid, name, senceid, task.task_step_type);
                    break;
                case (int)TaskStepType.SEll_WINE: taskinfo.SellFailCount += 1;
                    CheckIsInPrison(taskinfo.SellFailCount, userid, name, senceid, task.task_step_type);
                    break;
            }
        }

        /// <summary>创建全局谣言任务</summary>
        private Variable.UserTaskInfo CreatRumorTask(tg_task task)
        {
            var rumor = new Variable.UserTaskInfo
            {
                userid = task.user_id,
            };
            switch (task.task_step_type)
            {
                case (int)TaskStepType.RUMOR: rumor.RumorFailCount = 1; break;
                case (int)TaskStepType.FIRE: rumor.FireFailCount = 1; break;
                case (int)TaskStepType.BREAK: rumor.BreakFailCount = 1; break;
                case (int)TaskStepType.SEll_WINE: rumor.SellFailCount = 1; break;
            }
            return rumor;
        }

        /// <summary>验证进入监狱并推送公告</summary>
        private void CheckIsInPrison(int failcount, Int64 userid, string name, Int64 senceid, int type)
        {
            if (failcount <= 2) return;
            var ruserid = GetArrestUser(senceid, type);
            if (ruserid != 0)
            {
                new Share.Prison().PutInPrison(userid);
                NoticePush(userid, name, ruserid, type);     //推送系统公告
            }
        }

        /// <summary>查询戒严玩家信息</summary>
        /// <param name="senceid">场景id</param>
        /// <param name="taskType">谣言任务taskType</param>
        private Int64 GetArrestUser(Int64 senceid, int taskType)
        {
            var rusers = new List<Int64>();
            switch (taskType)          //布谣任务类型
            {
                case (int)TaskStepType.RUMOR:
                    rusers = Variable.TaskInfo.Where(m => m.ArrestRumorSceneId == senceid).Select(m => m.userid).ToList();
                    break;
                case (int)TaskStepType.FIRE:
                    rusers = Variable.TaskInfo.Where(m => m.ArrestFireSceneId == senceid).Select(m => m.userid).ToList();
                    break;
                case (int)TaskStepType.BREAK:
                    rusers = Variable.TaskInfo.Where(m => m.ArrestBreakSceneId == senceid).Select(m => m.userid).ToList();
                    break;
                case (int)TaskStepType.SEll_WINE:
                    rusers = Variable.TaskInfo.Where(m => m.ArrestSellSceneId == senceid).Select(m => m.userid).ToList();
                    break;
            }
            return !rusers.Any() ? 0 : RandomUserId(rusers);
        }

        /// <summary>随机一个戒严玩家id</summary>
        private Int64 RandomUserId(List<Int64> ruser)
        {
            var number = RNG.Next(0, ruser.Count - 1);
            return ruser[number];
        }

        /// <summary>推送公告</summary>
        private void NoticePush(Int64 userid, string name, Int64 ruserid, int type)
        {
            var chat = new Share.Chat();
            var ruser = tg_user.GetUsersById(ruserid);       //查询对手信息
            if (ruser == null) return;
            var aso = new List<ASObject>
            {
                chat.BuildData((int) ChatsASObjectType.PLAYERS, null, userid, name, null),
                chat.BuildData((int) ChatsASObjectType.PLAYERS, null, ruserid, ruser.player_name, null)
            };

            var baseid = 0;
            switch (type)      //验证推送任务的公告
            {
                case (int)TaskStepType.RUMOR: baseid = 100016; break;
                case (int)TaskStepType.FIRE: baseid = 100017; break;
                case (int)TaskStepType.BREAK: baseid = 100018; break;
                case (int)TaskStepType.SEll_WINE: baseid = 100019; break;
            }

            var list = Variable.OnlinePlayer.Keys;
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var model = item;
                System.Threading.Tasks.Task.Factory.StartNew(m =>
                {
                    var userId = Convert.ToInt64(model);
                    if (!Variable.OnlinePlayer.ContainsKey(userId)) return;
                    var session = Variable.OnlinePlayer[userId];
                    new Share.Chat().SystemChatSend(session, aso, baseid);
                    token.Cancel();
                }, model, token.Token);
            }
        }

        #endregion

        #region 戒严任务

        /// <summary>戒严类任务</summary>
        /// <param name="time">基表戒严时间</param>
        /// <param name="finishstep">任务步骤</param>
        /// <param name="task">任务信息</param>
        /// <param name="getnpc">npcid</param>
        /// <param name="sceneid">场景id</param>
        /// <returns>ASObject</returns>
        public ASObject ArrestTask(int time, string finishstep, tg_task task, int getnpc, long sceneid)
        {
            var btask = Variable.TaskInfo.FirstOrDefault(m => m.userid == task.user_id);
            if (btask == null)
                Variable.TaskInfo.Add(CreatArrestTask(task.user_id));

            if (!CheckNpc(task.task_step_data, getnpc))
                return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);

            if (!CheckArrest(task, sceneid))
                return CommonHelper.ErrorResult((int)ResultType.TASK_START_NOW);

            NewArrestStart(time, task, finishstep);       //开启戒严线程
            return new ASObject(BulidData((int)ResultType.SUCCESS, task));
        }

        /// <summary>创建全局戒严任务</summary>
        private Variable.UserTaskInfo CreatArrestTask(Int64 userid)
        {
            return new Variable.UserTaskInfo { userid = userid, };
        }

        /// <summary>验证戒严任务  更新戒严场景sceneid</summary>
        private bool CheckArrest(tg_task task, Int64 senceid)
        {
            var btask = Variable.TaskInfo.FirstOrDefault(m => m.userid == task.user_id);
            if (btask == null) return false;

            switch (task.task_step_type)      //验证任务并根据戒严类型  更新戒严场景id
            {
                case (int)TaskStepType.ARREST_RUMOR:
                    if (btask.ArrestRumorSceneId != 0) return false; btask.ArrestRumorSceneId = senceid; break;
                case (int)TaskStepType.ARREST_FIRE:
                    if (btask.ArrestFireSceneId != 0) return false; btask.ArrestFireSceneId = senceid; break;
                case (int)TaskStepType.ARREST_BREAK:
                    if (btask.ArrestBreakSceneId != 0) return false; btask.ArrestBreakSceneId = senceid; break;
                case (int)TaskStepType.ARREST_SEll_WINE:
                    if (btask.ArrestSellSceneId != 0) return false; btask.ArrestSellSceneId = senceid; break;
                default: return false;
            }
            return true;
        }

        /// <summary>开启戒严线程</summary>
        private void NewArrestStart(decimal time, tg_task mytask, string finishstep)
        {
            try
            {
                var token = new CancellationTokenSource();
                var task = new System.Threading.Tasks.Task(() => SpinWait.SpinUntil(() =>
                {
                    var taskinfo = Variable.TaskInfo.FirstOrDefault(q => q.userid == mytask.user_id);
                    if (taskinfo == null) { token.Cancel(); return true; }
                    switch (mytask.task_step_type)     //根据对应类型验证是否取消线程
                    {
                        case (int)TaskStepType.ARREST_RUMOR: if (taskinfo.ArrestRumorSceneId == 0) { token.Cancel(); return true; } break;
                        case (int)TaskStepType.ARREST_FIRE: if (taskinfo.ArrestFireSceneId == 0) { token.Cancel(); return true; } break;
                        case (int)TaskStepType.ARREST_BREAK: if (taskinfo.ArrestBreakSceneId == 0) { token.Cancel(); return true; } break;
                        case (int)TaskStepType.ARREST_SEll_WINE: if (taskinfo.ArrestSellSceneId == 0) { token.Cancel(); return true; } break;
                    }
                    return false;

                }, (int)time * 1000), token.Token);
                task.Start();
                task.ContinueWith(m =>
                {
                    TaskUpdateAndSend(mytask, finishstep);
                    token.Cancel();
                }, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>完成任务并推送</summary>
        private void TaskUpdateAndSend(tg_task task, string finishstep)
        {
            var taskinfo = Variable.TaskInfo.FirstOrDefault(q => q.userid == task.user_id);
            if (taskinfo == null) return;

            //更新对应任务戒严场景id
            switch (task.task_step_type)
            {
                case (int)TaskStepType.ARREST_RUMOR: taskinfo.ArrestRumorSceneId = 0; break;
                case (int)TaskStepType.ARREST_FIRE: taskinfo.ArrestFireSceneId = 0; break;
                case (int)TaskStepType.ARREST_BREAK: taskinfo.ArrestBreakSceneId = 0; break;
                case (int)TaskStepType.ARREST_SEll_WINE: taskinfo.ArrestSellSceneId = 0; break;
            }
            task.task_step_data = finishstep;
            task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
            (new Share.TGTask()).AdvancedTaskPush(task.user_id, task);    //推送协议
        }

        #endregion
    }
}
