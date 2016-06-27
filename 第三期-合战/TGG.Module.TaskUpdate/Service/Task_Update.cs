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

namespace TGG.Module.TaskUpdate.Service
{
    /// <summary>
    /// 完成任务指令
    /// 开发者：李德雁
    /// </summary>
    public class Task_Update
    {

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~Task_Update()
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
            XTrace.WriteLine("{0}:{1}", "TASK_FINISH", "任务更新");
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
            var mytask = session.MainTask.CloneEntity();
            if (mytask == null || mytask.id != id || mytask.task_type != (int)TaskType.MAIN_TASK
                || mytask.user_id != session.Player.User.id)
                return null;

            var basetask = Variable.BASE_TASKMAIN.FirstOrDefault(q => q.id == mytask.task_id); //基表数据
            if (basetask == null) return null;


#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif

            return FinishTaskCheck(mytask, basetask.stepCondition, npc);


        }


        #region 任务验证

        /// <summary>
        /// 任务步骤更新验证
        /// </summary>
        /// <param name="task">tg_task实体</param>
        /// <param name="basestep">基表任务条件</param>
        /// <param name="npc">前端提交npcid</param>
        /// <returns>验证后返回的数据</returns>
        private ASObject FinishTaskCheck(tg_task task, string basestep, int npc)
        {
            string step = task.task_step_data;
            Int64 userid = task.user_id;
            var tuple = new Share.TGTask().CheckTaskStep(basestep, step, userid, npc);

            if (tuple.Item1 < 0)
            {
                XTrace.WriteLine("未catch,任务更新出错,错误值{0}", tuple.Item1);
                return BulidReturnData(tuple.Item1, null);
            }

            TaskUpdate(task, tuple.Item2, basestep);

            return BulidReturnData((int)ResultType.SUCCESS, task);
        }

        /// <summary> 任务数据更新 </summary>
        private void TaskUpdate(tg_task task, string newstep, string basestep)
        {
            if (task == null) return;
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id)) return;
            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
            if (session == null || session.MainTask == null) return;
            task.task_step_data = newstep;
            if (newstep == basestep)
                task.task_state = (int)TaskStateType.TYPE_REWARD;
            tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);
            if (task.task_type == (int)TaskType.MAIN_TASK)
            {
                session.MainTask.task_state = task.task_state;
                session.MainTask.task_step_data = task.task_step_data;
                session.MainTask.task_id = task.task_id;
            }
        }

        #endregion



        /// <summary> 组装数据 </summary>
        private ASObject BulidReturnData(int result, tg_task newtask)
        {
            if (result < 0 && result != 4006)
            {
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
