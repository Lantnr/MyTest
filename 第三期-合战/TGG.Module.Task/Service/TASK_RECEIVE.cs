using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;


namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 接收任务指令
    /// 开发者：李德雁
    /// </summary>
    public class TASK_RECEIVE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~TASK_RECEIVE()
        {
            Dispose();
        }

        #endregion
        //private static TASK_FINISH objInstance = null;

        //public static TASK_FINISH getInstance()
        //{
        //    if (objInstance == null) objInstance = new TASK_FINISH();
        //    return objInstance;
        //}

        //  private ConcurrentDictionary<Int64, bool> dic = new ConcurrentDictionary<long, bool>();

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
                   || mytask.user_id != session.Player.User.id || mytask.task_state != (int)TaskStateType.TYPE_UNRECEIVED)
                return null;

            var basetask = Variable.BASE_TASKMAIN.FirstOrDefault(q => q.id == mytask.task_id); //基表数据
            if (basetask == null) return null;

                return ReceiveTaskCheck(mytask, basetask.acceptId, npc);


        }

        #region 任务验证

        /// <summary>
        /// 接受任务验证
        /// </summary>
        /// <param name="task">tg_task实体</param>
        /// <param name="npcid">接取任务npcid</param>
        /// <param name="npc">前端提交npcid</param>
        /// <returns>验证后返回的数据</returns>
        private ASObject ReceiveTaskCheck(tg_task task, Int32 npcid, int npc)
        {
            try
            {
                if (task == null) return null;
                if (!Variable.OnlinePlayer.ContainsKey(task.user_id))
                    return null;

                if (npcid != npc)
                    return null;
            
                task.task_state = (int)TaskStateType.TYPE_UNFINISHED;
                tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);

                var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
                if (session == null || session.MainTask == null) return null;
                session.MainTask.task_state = (int)TaskStateType.TYPE_UNFINISHED;

                return BulidReturnData((int)ResultType.SUCCESS, task);
            }
            catch (Exception)
            {
                XTrace.WriteLine("接受任务出错");
                return null;

            }

        }
        #endregion



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
