using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Task;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 职业任务更新
    /// 开发者：李德雁
    /// </summary>
    public class TASK_VOCATION_UPDATE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~TASK_VOCATION_UPDATE()
        {
            Dispose();
        }

        #endregion

        //private static TASK_VOCATION_UPDATE ObjInstance = null;

        ///// <summary> TASK_VOCATION_UPDATE单体模式 </summary>
        //public static TASK_VOCATION_UPDATE getInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new TASK_VOCATION_UPDATE());
        //}

        //  private ConcurrentDictionary<Int64, bool> dic = new ConcurrentDictionary<long, bool>();

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            // if(!CheckQueue(userid))return null;
            if (!data.ContainsKey("type") || !data.ContainsKey("taskId")) return null;
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var npc = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "npcId").Value);
            var step = data.FirstOrDefault(q => q.Key == "data").Value;
            var mainid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "taskId").Value);
            var state = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "state").Value);
            var taskinfo = tg_task.FindByid(mainid);
            if (taskinfo == null || taskinfo.task_type != (int)TaskType.VOCATION_TASK)
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));
            var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == taskinfo.task_id);
            if (baseinfo == null) return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));
            var tasktype = baseinfo.type;
            var steptype = Convert.ToInt32(baseinfo.stepCondition.Split(new[] { '|', '_' })[0]);
            if (type == 1)
            {
                if (taskinfo.task_state != (int)TaskStateType.TYPE_UNFINISHED)
                    return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));
                #region 任务步骤验证与更新
                switch (steptype)
                {
                    case (int)TaskStepType.BUILD: //监督建筑
                    case (int)TaskStepType.ESCORT://护送
                    case (int)TaskStepType.CLICK: //点击任意玩家
                    case (int)TaskStepType.TYPE_DIALOG:
                        {
                            var tuple = new Share.TGTask().CheckTaskStep(baseinfo.stepCondition, taskinfo.task_step_data, userid, npc);
                            if (tuple.Item1 < 0)
                                return new ASObject(BulidData(tuple.Item1, null));
                            Common.getInstance().TaskUpdate(taskinfo, tuple.Item2, baseinfo.stepCondition);
                        }
                        break;
                    case (int)TaskStepType.SEND: //从小事赚起
                        if (!CheckDiaAndSend(baseinfo.stepCondition, npc, taskinfo))
                            return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        break;
                    case (int)TaskStepType.FASTNPC: //对玩家对话，有一个玩家可以快速完成
                        if (!CheckFastDialogue(baseinfo.stepCondition, npc, taskinfo))
                            return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        break;
                    case (int)TaskStepType.NPCCOUNTS://与多个npc对话，达到总次数
                        if (!CheckTwoNpc(baseinfo.stepCondition, npc, taskinfo))
                            return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        break;
                    case (int)TaskStepType.STAND_GUARD://站岗
                        {
                            if (!CheckStandGuard(baseinfo.time, baseinfo.stepCondition, taskinfo, npc, session.Player.Scene.scene_id, session.Player.User.player_camp))
                                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        }
                        break;
                    case (int)TaskStepType.ASSASSINATION://刺杀
                        {
                            if (!Common.getInstance().KillTask(npc, baseinfo, session.Player.User.player_camp, session.Player.Role.Kind.role_level, ref taskinfo))
                                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        }
                        break;
                    case (int)TaskStepType.FIGHTING_CONTINUOUS://连续战斗
                        return Common.ObjInstance.ContinueFight(session, taskinfo, baseinfo.stepCondition, npc);
                    case (int)TaskStepType.GUARD://守护
                        {
                            if (!Common.getInstance().WatchTask(npc, baseinfo, session.Player.User.player_camp, session.Player.Role.Kind.role_level, ref taskinfo))
                                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        }
                        break;
                    //谣言类
                    case (int)TaskStepType.RUMOR:
                    case (int)TaskStepType.FIRE:
                    case (int)TaskStepType.BREAK:
                    case (int)TaskStepType.SEll_WINE:
                        {
                            return Common.getInstance().RumorTask(session.Player, taskinfo, baseinfo.stepCondition, npc);
                        }
                    //戒严类
                    case (int)TaskStepType.ARREST_RUMOR:
                    case (int)TaskStepType.ARREST_FIRE:
                    case (int)TaskStepType.ARREST_BREAK:
                    case (int)TaskStepType.ARREST_SEll_WINE:
                        {
                            return Common.getInstance().ArrestTask(baseinfo.time, baseinfo.stepCondition, taskinfo, npc, session.Player.Scene.scene_id);
                        }
                    case (int)TaskStepType.DUOMAOMAO:
                        return Common.getInstance().DuoMaoMaoTask(npc, state, baseinfo, taskinfo);
                }
                #endregion
                return new ASObject(BulidData((int)ResultType.SUCCESS, taskinfo));
            }
            if (type != 2) return new ASObject(BulidData((int)ResultType.FRONT_DATA_ERROR, null));
            if (taskinfo.task_state != (int)TaskStateType.TYPE_REWARD)  //验证任务是否完成
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UNFINISH, null));
            var rewardstring = "";
            switch (steptype)
            {
                case (int)TaskStepType.FIGHTING_CONTINUOUS:
                    {
                        var tuple = Common.getInstance().GetContinueReward(userid, taskinfo.task_step_data, baseinfo.id);
                        if (tuple.Item1 < 0)
                            return new ASObject(BulidData(tuple.Item1, null));
                        rewardstring = tuple.Item2;
                    }
                    break;
                case (int)TaskStepType.DUOMAOMAO:
                    rewardstring = Common.getInstance().GetRewardString(userid, baseinfo);
                    break;
                default: rewardstring = Common.getInstance().GetRewardString(baseinfo, session); break; //领取奖励
            }
            if (rewardstring != "") //连续战斗没有胜利没有奖励
            {
                if (!new Share.Reward().GetReward(rewardstring, userid, (int)ModuleNumber.TASK, (int)TaskCommand.TASK_VOCATION_UPDATE))
                    return new ASObject(BulidData((int)ResultType.REWARD_FALSE, null));
            }
            taskinfo.Delete();
            //大名令更新
            var damingtype = tasktype == 1 ? (int)DaMingType.评定 : (int)DaMingType.高级评定;
            new Share.DaMing().CheckDaMing(userid, damingtype);
            //评定主线任务统计
            new Share.TGTask().VocationAdd(userid, tasktype);
            ////日志记录 格式：任务_任务主键id_任务id|奖励字符串
            //var logdata = string.Format("T_{0}_{1}%{2}", taskinfo.id, taskinfo.task_id, rewardstring);
            //new Share.Log().WriteLog(userid, (int)LogType.Delete, (int)ModuleNumber.TASK, (int)TaskCommand.TASK_VOCATION_UPDATE, "任务", "身份更新", "身份", 0, 0, 0, logdata);
            return new ASObject(BulidData((int)ResultType.SUCCESS, null));
        }

        #region 任务验证
        /// <summary>
        /// 从小事赚起 任务验证
        ///任务步骤值：401|1_200059_1|1_200067_1|4_6_00_2|3_6_00_200066_2
        /// / </summary>
        private bool CheckDiaAndSend(string basestep, int npc, tg_task task)
        {
            var mysteplist = new TGTask().SplitTaskToList(task.task_step_data);
            var basesteplist = new TGTask().SplitTaskToList(basestep);
            var diff = mysteplist.Except(basesteplist).ToList();  //找出未完成的任务
            if (!diff.Any()) return false;
            for (var i = 0; i < diff.Count; i++)
            {
                var splitdiff = diff[i].Split('_').ToList();
                if (Convert.ToInt32(splitdiff[0]) != Convert.ToInt32(TaskStepType.TYPE_DIALOG)) continue;
                if (splitdiff[1] != npc.ToString() || splitdiff[2] != "0") continue;
                var index = mysteplist.IndexOf(diff[i]);
                //任务更新
                if (!UpdateStep(mysteplist, index, 2, basesteplist)) return false;
                if (!UpdateStep(mysteplist, mysteplist.Count - 2, 3, basesteplist)) return false;
                if (!UpdateStep(mysteplist, mysteplist.Count - 1, 4, basesteplist)) return false;
                var newstep = string.Join("|", mysteplist);
                Common.getInstance().TaskUpdate(task, newstep, basestep);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 快速对话  任务验证
        /// 任务步骤格式：402_200012|1_200078_1|1_200059_1|1_200012_1
        /// 初始值：402_200012|1_200078_0|1_200059_0|1_200012_0
        /// </summary>
        private bool CheckFastDialogue(string basestep, int npc, tg_task task)
        {
            var mysteplist = new TGTask().SplitTaskToList(task.task_step_data);
            var fastnpc = mysteplist[0].Split('_')[1];
            if (fastnpc == npc.ToString()) //快速完成
            {
                Common.getInstance().TaskUpdate(task, basestep, basestep);
                return true;
            }
            var basesteplist = new TGTask().SplitTaskToList(basestep);
            var diff = mysteplist.Except(basesteplist).ToList();  //找出未完成的任务
            if (!diff.Any()) return false;
            for (int i = 0; i < diff.Count; i++)
            {
                var splitdiff = diff[i].Split('_').ToList();
                if (Convert.ToInt32(splitdiff[0]) != Convert.ToInt32(TaskStepType.TYPE_DIALOG)) continue;
                if (splitdiff[1] != npc.ToString() || splitdiff[2] != "0") continue;
                int index = mysteplist.IndexOf(diff[i]);
                //任务更新
                if (!UpdateStep(mysteplist, index, 2, basesteplist)) return false;
                var newstep = string.Join("|", mysteplist);
                Common.getInstance().TaskUpdate(task, newstep, basestep);
                return true;
            }
            return true;
        }

        /// <summary>多个npc完成总对话次数 任务验证 </summary>
        private bool CheckTwoNpc(string basestep, int npc, tg_task task)
        {
            var steplist = task.task_step_data.Split('_');
            var npccheck = false;
            for (var i = 0; i < steplist.Count(); i++)
            {
                if (steplist[i] != npc.ToString()) continue;
                npccheck = true;
                break;
            }
            if (npccheck == false) return false;
            var countindex = steplist.Count() - 1;
            var count = Convert.ToInt32(steplist[countindex]);
            var needcount = Convert.ToInt32(basestep.Split('_')[countindex]);
            count++;
            if (count > needcount) return false;
            steplist[3] = count.ToString();
            var newstep = string.Join("_", steplist);
            Common.getInstance().TaskUpdate(task, newstep, basestep);
            return true;
        }


        /// <summary>
        /// 站岗验证
        /// </summary>
        private bool CheckStandGuard(int time, string finishstep, tg_task task, int getnpc, long sceneid, int camp)
        {
            var npc = task.task_step_data.Split(new[] { '_', '|' })[3];
            if (npc != getnpc.ToString()) return false;
            var v_task = Common.getInstance().GetWorkInfo(task.user_id);
            if (v_task.GuardSceneId > 0) return false; //已经有守护城市
            v_task.GuardSceneId = sceneid;
            v_task.GuardCamp = camp;
            NewTaskStart(time, task, finishstep);
            return true;
        }
        #endregion


        /// <summary>任务单步步骤更新</summary>
        private bool UpdateStep(List<string> mysteplist, int stepindex, int updateindex, List<string> basesteplist)
        {
            if (mysteplist.Count - 1 < stepindex || basesteplist.Count - 1 < stepindex) return false;//验证数组长度
            var needaddstep = mysteplist[stepindex];
            var baseneedstep = basesteplist[stepindex];
            var mystep = needaddstep.Split('_');
            if (mystep.Count() - 1 < updateindex) return false;//验证数组长度
            var count = Convert.ToInt32(mystep[updateindex]);
            var listtask = baseneedstep.Split('_').ToList();
            if (listtask.Count() - 1 < updateindex) return false;//验证数组长度
            var needcount = Convert.ToInt32(listtask[updateindex]);
            count++;
            if (count > needcount) return false;
            mystep[updateindex] = count.ToString();
            needaddstep = string.Join("_", mystep);
            mysteplist[stepindex] = needaddstep;
            return true;
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            // if (result < 0) return null;
            var taskvo = new VocationTaskVo();
            var dic = new Dictionary<string, object>();
            if (newtask != null)
                taskvo = EntityToVo.ToVocationTaskVo(newtask);
            dic.Add("result", result);
            dic.Add("taskVo", newtask == null ? null : taskvo);
            return dic;
        }

        /// <summary>
        /// 开启新线程
        /// </summary>
        /// <param name="time"></param>
        /// <param name="mytask"></param>
        /// <param name="finishstep"></param>
        public void NewTaskStart(Int64 time, tg_task mytask, string finishstep)
        {
            try
            {
                var token = new CancellationTokenSource();
                var task = new System.Threading.Tasks.Task(() => SpinWait.SpinUntil(() =>
                {
                    var taskinfo = Common.getInstance().GetWorkInfo(mytask.user_id);
                    if (taskinfo.GuardSceneId == 0)
                    {
                        token.Cancel();
                        return true;
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

        /// <summary>
        /// 完成任务并推送
        /// </summary>
        /// <param name="task"></param>
        /// <param name="finishstep"></param>
        private void TaskUpdateAndSend(tg_task task, string finishstep)
        {
            var v_task = Common.getInstance().GetWorkInfo(task.user_id);
            if (v_task != null) v_task.GuardSceneId = 0;
            task.task_step_data = finishstep;
            task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
            (new Share.TGTask()).AdvancedTaskPush(task.user_id, task);//推送协议
        }

        //private bool CheckQueue(Int64 userid)
        //{
        //    if (dic.ContainsKey(userid)) return false;
        //    dic.TryAdd(userid, true);
        //    return true;
        //}

        //private void RemoveQuene(Int64 userid)
        //{
        //    Boolean tryremove;
        //    dic.TryRemove(userid, out tryremove);
        //}

    }
}
