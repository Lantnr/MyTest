using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.TaskFinish.Service
{
    public class Task_Finish
    {
        /// <summary>
        /// 完成任务指令
        /// 开发者：李德雁
        /// </summary>
        public class TASK_FINISH : IDisposable
        {
            #region IDisposable 成员
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            /// <summary>析构函数</summary>
            ~TASK_FINISH()
            {
                Dispose();
            }

            #endregion


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
                if (!data.ContainsKey("task") || !data.ContainsKey("npcId")) return null;
                var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "task").Value);
                var npc = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "npcId").Value);
                if (session.MainTask == null)
                {
                    XTrace.WriteLine("sesion中找不到任务");
                    return null;
                }
                var userid = session.Player.User.id;
                var mytask = session.MainTask.CloneEntity();

                if (mytask == null || mytask.id != id || mytask.task_type != (int)TaskType.MAIN_TASK
                    || mytask.user_id != userid ||mytask.task_state == (int)TaskStateType.TYPE_FINISHED)
                    return null;

                var basetask = Variable.BASE_TASKMAIN.FirstOrDefault(q => q.id == mytask.task_id); //基表数据
                if (basetask == null) return null;
               
                var baseindex = Variable.BASE_TASKMAIN.IndexOf(basetask);
                if (mytask.task_state != (int)TaskStateType.TYPE_REWARD)  //验证任务是否完成
                    //return new ASObject(BulidReturnData((int)ResultType.TASK_MAINTASK_UNFINISHED, null));
                    return null;

#if DEBUG
                sw.Stop();
                XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
                return SubmitTaskCheck(mytask, npc, baseindex); //验证提交任务


            }

            #region 任务验证




            /// <summary>
            /// 提交任务验证
            /// </summary>
            /// <param name="task">tg_task实体</param>
            /// <param name="npc">前端提交的npcid</param>
            /// <param name="baseindex"></param>
            private ASObject SubmitTaskCheck(tg_task task, int npc, int baseindex)
            {
                if (task == null) return null;
                if (Variable.BASE_TASKMAIN[baseindex].finishedId != npc)
                    return BulidReturnData((int)ResultType.TASK_NPC_FALSE, null);
                //验证任务是不是最后一个主线任务
                var tuple = CheckIsLastTask(task, baseindex);
                if (tuple.Item1) return tuple.Item2;
                //领取奖励验证
                var reward = new Share.Reward();
                if (!reward.GetReward(Variable.BASE_TASKMAIN[baseindex].reward, task.user_id, (int)ModuleNumber.TASK, (int)TaskCommand.TASK_FINISH)) //领取奖励
                {
                    reward.Dispose();
                    return BulidReturnData((int)ResultType.BAG_ISFULL_ERROR, null);
                }
                reward.Dispose();
                var newtask = new TGTask().BuildNewMainTask(Variable.BASE_TASKMAIN[baseindex + 1].id, Variable.BASE_TASKMAIN[baseindex + 1].stepCondition, task.id, task.user_id);
                if (newtask == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                var tuple1 = CheckLevel(Variable.BASE_TASKMAIN[baseindex + 1].openLevel, newtask);
                if (!tuple1.Item1) return tuple1.Item2;
                IsPushCard(task.task_id, task.user_id);
                //引导任务不需要接受
                if (newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TYPE_BUSINESS)) ||
                   newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.STUDY_SKILL)) ||
                    newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TRAIN)))
                    newtask.task_state = (int)TaskStateType.TYPE_UNFINISHED;
#if DEBUG
                if (newtask.task_id == 2010018)
                {
                    newtask.task_id = 2010017;
                    newtask.task_step_data = "1_200019_1|8_15020003_1_1";
                    newtask.task_state = 2;
                }
#endif
                tg_task.GetTaskUpdate(newtask.task_state, newtask.task_step_data, newtask.task_id, newtask.id, newtask.user_id);
                if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                    return BulidReturnData((int)ResultType.UNKNOW_ERROR, null);

                var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
                if (session == null) return BulidReturnData((int)ResultType.UNKNOW_ERROR, null);
                session.MainTask = newtask;
                //验证大名令主线任务是否已完成
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "2018");
                if (rule != null)
                {
                    if (newtask.task_id == Convert.ToInt32(rule.value))
                    {
                        var daming = session.Player.DamingLog;
                        var bigbag = daming.FirstOrDefault(m => m.base_id == (int)DaMingType.大礼包);
                        if (bigbag != null)
                            if (bigbag.is_finish == 1)
                                newtask = new TGTask().GetNewTask(newtask);
                    }
                }
                return BulidReturnData((int)ResultType.SUCCESS, newtask);
            }
            #endregion

            /// <summary>
            /// 验证是否是最后一个主线任务
            /// </summary>
            /// <param name="task"></param>
            /// <param name=" baseindex"></param>
            /// <returns>bool:是否最后一个任务 BaseTaskMain：新任务基表数据 ASObject：最后一个任务返回前端数据</returns>
            private Tuple<bool, ASObject> CheckIsLastTask(tg_task task, int baseindex)
            {
                if (task == null) return Tuple.Create(false, BulidReturnData((int)ResultType.UNKNOW_ERROR, null));
                if (baseindex == Variable.BASE_TASKMAIN.Count - 1) //判断是不是最后一个主线任务
                {
                    task.task_state = (int)TaskStateType.TYPE_FINISHED;
                    tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);
                    if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                        return Tuple.Create(false, BulidReturnData((int)ResultType.UNKNOW_ERROR, null));

                    var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
                    if (session == null || session.MainTask == null)
                        return Tuple.Create(false, BulidReturnData((int)ResultType.UNKNOW_ERROR, null));
                    session.MainTask.task_state = (int)TaskStateType.TYPE_FINISHED;

                    var aso = !new Share.Reward().GetReward(Variable.BASE_TASKMAIN[baseindex].reward, task.user_id,(int)ModuleNumber.TASK,(int)TaskCommand.TASK_FINISH) ?
                      BulidReturnData((int)ResultType.BAG_ISFULL_ERROR, null)
                     : BulidReturnData((int)ResultType.SUCCESS, null);

                    // RemoveQuene(task.user_id);
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
                    return Tuple.Create(false, BulidReturnData((int)ResultType.UNKNOW_ERROR, null));
                }
                var session = Variable.OnlinePlayer[newtask.user_id] as TGGSession;
                var newlevle = session.Player.Role.Kind.role_level;
                if (newlevle < openlevel)  //等级验证 
                {
                    newtask.task_state = (int)TaskStateType.TYPE_UNOPEN;
                    tg_task.GetTaskUpdate(newtask.task_state, newtask.task_step_data, newtask.task_id, newtask.id, newtask.user_id);
                    session.MainTask = newtask;
                    // RemoveQuene(newtask.user_id);
                    return Tuple.Create(false, BulidReturnData((int)ResultType.SUCCESS, newtask));
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

            /// <summary> 组装数据 </summary>
            private ASObject BulidReturnData(int result, tg_task newtask)
            {
                if (result < 0 && result != 4006)
                {
                    //  XTrace.WriteLine("任务出错。错误值{0}",result);
                    return null;
                }
                var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"mainTask", newtask == null ? null : EntityToVo.ToTaskVo(newtask)}
            };
                return new ASObject(dic);
            }
        }
    }
}
