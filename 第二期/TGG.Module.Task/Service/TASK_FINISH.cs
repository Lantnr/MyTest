using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;


namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 完成任务指令
    /// 开发者：李德雁
    /// </summary>
    public class TASK_FINISH
    {
        private static TASK_FINISH objInstance = null;

        public static TASK_FINISH getInstance()
        {
            if (objInstance == null) objInstance = new TASK_FINISH();
            return objInstance;
        }

       // private ConcurrentDictionary<Int64, bool> dic = new ConcurrentDictionary<long, bool>();

        /// <summary>
        /// 说明：任务步骤用 类型_类型id_完成值来表示。
        /// 例如1_200001_1， 1表示任务类型对话，200001表示npc的id,对话类型中0表示未完成，1表示完成
        /// 多步任务用|来分割，例如两步对话任务为 1_200001_1|1_200002_1.
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TASK_FINISH", "完成任务");
#endif
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            //type:0表示接受任务，1表示完成任务 2 表示提交任务

            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "task").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var npc = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "npcId").Value);
            var mytask = session.MainTask.CloneEntity();
            if (mytask == null || mytask.id != id || mytask.task_type != (int)TaskType.MAIN_TASK)
                return new ASObject(BulidReturnData((int)ResultType.BASE_TABLE_ERROR, null));

          
            var basetask = Variable.BASE_TASKMAIN.FirstOrDefault(q => q.id == mytask.task_id); //基表数据
            if (basetask == null) return new ASObject(BulidReturnData((int)ResultType.TASK_NO_MAINTASK, null));
            var baseindex = Variable.BASE_TASKMAIN.IndexOf(basetask);

            if (type == 0) //接收任务验证
                return ReceiveTaskCheck(mytask, basetask.acceptId, npc);
            if (type == 1) //完成任务验证
                return FinishTaskCheck(mytask, basetask.stepCondition, npc);
            if (mytask.task_state == (int)TaskStateType.TYPE_FINISHED)
                return new ASObject(BulidReturnData((int)ResultType.TASK_FINISHED, mytask));
            if (mytask.task_state != (int)TaskStateType.TYPE_REWARD)  //验证任务是否完成
                return new ASObject(BulidReturnData((int)ResultType.TASK_MAINTASK_UNFINISHED, null));
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return SubmitTaskCheck(mytask, npc, baseindex); //验证提交任务


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

        #region 任务验证

        /// <summary>
        /// 接受任务验证
        /// </summary>
        /// <param name="task">tg_task实体</param>
        /// <param name="npcid">接取任务npcid</param>
        /// <param name="npc">前端提交npcid</param>
        /// <param name="session"></param>
        /// <returns>验证后返回的数据</returns>
        private ASObject ReceiveTaskCheck(tg_task task, Int32 npcid, int npc)
        {
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                return new ASObject(BulidReturnData((int)ResultType.UNKNOW_ERROR, null));

            if (npcid != npc)
                return new ASObject(BulidReturnData((int)ResultType.TASK_NPC_FALSE, null));
            if (task.task_state != (int)TaskStateType.TYPE_UNRECEIVED)
                return new ASObject(BulidReturnData((int)ResultType.TASK_RECEIVED, null));

            task.task_state = (int)TaskStateType.TYPE_UNFINISHED;
            tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);

            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
            session.MainTask.task_state = (int)TaskStateType.TYPE_UNFINISHED;
          //  RemoveQuene(task.user_id);
            return new ASObject(BulidReturnData((int)ResultType.SUCCESS, task));
        }

        /// <summary>
        /// 任务步骤更新验证
        /// </summary>
        /// <param name="task">tg_task实体</param>
        /// <param name="basestep">基表任务条件</param>
        /// <param name="npc">前端提交npcid</param>
        /// <returns>验证后返回的数据</returns>
        private ASObject FinishTaskCheck(tg_task task, string basestep, int npc)
        {
            var tuple = new Share.TGTask().CheckTaskStep(basestep, task.task_step_data, task.user_id, npc);
            if (tuple.Item1 < 0)
                return new ASObject(BulidReturnData(tuple.Item1, null));
            Common.getInstance().TaskUpdate(task, tuple.Item2, basestep);
           // RemoveQuene(task.user_id);
            return new ASObject(BulidReturnData((int)ResultType.SUCCESS, task));
        }

        /// <summary>
        /// 提交任务验证
        /// </summary>
        /// <param name="task">tg_task实体</param>
        /// <param name="npc">前端提交的npcid</param>
        /// <param name="baseindex"></param>
        private ASObject SubmitTaskCheck(tg_task task, int npc, int baseindex)
        {
           // Thread.Sleep(5000);
            if (Variable.BASE_TASKMAIN[baseindex].finishedId != npc)
                return new ASObject(BulidReturnData((int)ResultType.TASK_NPC_FALSE, null));
            //验证任务是不是最后一个主线任务
            var tuple = CheckIsLastTask(task, baseindex);
            if (tuple.Item1) return tuple.Item2;
            //领取奖励验证
            if (!new Share.Reward().GetReward(Variable.BASE_TASKMAIN[baseindex].reward, task.user_id)) //领取奖励
                return new ASObject(BulidReturnData((int)ResultType.BAG_ISFULL_ERROR, null));

            var newtask = BuildNewTask(Variable.BASE_TASKMAIN[baseindex + 1].id, Variable.BASE_TASKMAIN[baseindex + 1].stepCondition, task.id, task.user_id);
            var tuple1 = CheckLevel(Variable.BASE_TASKMAIN[baseindex + 1].openLevel, newtask);
            if (!tuple1.Item1) return tuple1.Item2;
            IsPushCard(task.task_id, task.user_id);
            //引导任务不需要接受
            if (newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TYPE_BUSINESS)) ||
               newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.STUDY_SKILL)) ||
                newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TRAIN)))
                newtask.task_state = (int)TaskStateType.TYPE_UNFINISHED;

            tg_task.GetTaskUpdate(newtask.task_state, newtask.task_step_data, newtask.task_id, newtask.id, newtask.user_id);
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                return new ASObject(BulidReturnData((int)ResultType.UNKNOW_ERROR, null));

            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;

            session.MainTask = newtask;
           // RemoveQuene(task.user_id);
            return new ASObject(BulidReturnData((int)ResultType.SUCCESS, newtask));
        }
        #endregion

        /// <summary>
        /// 验证是否是最后一个主线任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="baseindex"></param>
        /// <returns>bool:是否最后一个任务 BaseTaskMain：新任务基表数据 ASObject：最后一个任务返回前端数据</returns>
        private Tuple<bool, ASObject> CheckIsLastTask(tg_task task, int baseindex)
        {
            if (baseindex == Variable.BASE_TASKMAIN.Count - 1) //判断是不是最后一个主线任务
            {
                task.task_state = (int)TaskStateType.TYPE_FINISHED;
                tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);
                if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                    return Tuple.Create(false, new ASObject(BulidReturnData((int)ResultType.UNKNOW_ERROR, null)));

                var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
                session.MainTask.task_state = (int)TaskStateType.TYPE_FINISHED;
                var aso = !new Share.Reward().GetReward(Variable.BASE_TASKMAIN[baseindex].reward, task.user_id) ?
                 new ASObject(BulidReturnData((int)ResultType.BAG_ISFULL_ERROR, null))
                 : new ASObject(BulidReturnData((int)ResultType.SUCCESS, null));
                //RemoveQuene(task.user_id);
                return Tuple.Create(true, aso);
            }
           // RemoveQuene(task.user_id);
            return Tuple.Create(false, new ASObject());
        }

        /// <summary>
        /// 是否向前端推送获取武将卡
        /// </summary>
        /// <param name="baseid"></param>
        private void IsPushCard(int baseid, Int64 userid)
        {
            var taskrule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "2015");
            if (taskrule != null && baseid == Convert.ToInt32(taskrule.value))
            {
                var proprule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "2016");
                if (proprule != null)
                {
                    PushGetProp(Convert.ToInt32(proprule.value), userid);
                }
            }
        }


        /// <summary>
        /// 获取升级后的等级 
        /// </summary>
        /// <param name="openlevel"></param>
        /// <param name="session"></param>
        /// <param name="newtask"></param>
        /// <returns>bool：是否达到开启等级 ASObject：等级未开启返回数据</returns>
        private Tuple<bool, ASObject> CheckLevel(int openlevel, tg_task newtask)
        {
            if (!Variable.OnlinePlayer.ContainsKey(newtask.user_id))
            {
                return Tuple.Create(false, new ASObject(BulidReturnData((int)ResultType.UNKNOW_ERROR, null)));
            }
            var session = Variable.OnlinePlayer[newtask.user_id] as TGGSession;
            var newlevle = session.Player.Role.Kind.role_level;
            if (newlevle < openlevel)  //等级验证 
            {
                newtask.task_state = (int)TaskStateType.TYPE_UNOPEN;
                tg_task.GetTaskUpdate(newtask.task_state, newtask.task_step_data, newtask.task_id, newtask.id, newtask.user_id);
                session.MainTask = newtask;
                //RemoveQuene(newtask.user_id);
                return Tuple.Create(false, new ASObject(BulidReturnData((int)ResultType.SUCCESS, newtask)));
            }
            return Tuple.Create(true, new ASObject());
        }

        /// <summary>
        /// 新手引导推送前端获取家臣卡 
        /// </summary>
        /// <param name="propid">道具id</param>
        private void PushGetProp(int propid, Int64 _userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(_userid)) return;
            var session = Variable.OnlinePlayer[_userid];
            var dic = new Dictionary<string, object>
                {
                    {"propId", propid}
                };
            var aso = new ASObject(dic);
            (new Share.TGTask()).Push(session, aso, (int)TaskCommand.WORK_ROLE_GET_PUSH);
        }


        #region 组装数据
        /// <summary> 组装新任务的数据 </summary>
        private tg_task BuildNewTask(Int32 task_id, string step, long id, Int64 userid)
        {
            var task = new tg_task()
            {
                task_type = (int)TaskType.MAIN_TASK,
                id = id,
                task_id = task_id,
                task_step_data = Common.getInstance().GetInsertValue(step),
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                user_id = userid
            };
            return task;
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidReturnData(int result, tg_task newtask)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"mainTask", newtask == null ? null : EntityToVo.ToTaskVo(newtask)}
            };
            return dic;
        }

        #endregion

    }
}
