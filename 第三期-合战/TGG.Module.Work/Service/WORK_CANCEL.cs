using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    /// <summary>
    /// 取消守护任务
    /// </summary>
    public class WORK_CANCEL : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WORK_CANCEL()
        {
            Dispose();
        }
    
        #endregion
        //public static WORK_CANCEL objInstance = null;

        ///// <summary> WORK_CANCEL单体模式 </summary>
        //public static WORK_CANCEL GetInstance()
        //{
        //    return objInstance ?? (objInstance = new WORK_CANCEL());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TASK_CANCEL", "任务取消");
#endif
            if (!data.ContainsKey("id")) return null;
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);        //取消任务 主键id
            if (id == 0) return new ASObject(BuildData((int)ResultType.FRONT_DATA_ERROR, 0));

            var task = tg_task.GetEntityByIdAndUserId(id, session.Player.User.id);// tg_task.FindByid(id);
            if (task == null || task.task_type != (int)TaskType.WORK_TASK)
                return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);   //验证职业任务是否存在

            var WorkInfo = Variable.WorkInfo.FirstOrDefault(m => m.userid == session.Player.User.id);
            if (WorkInfo == null) return new ASObject(BuildData((int)ResultType.SUCCESS, id));

            //根据任务类型更新对应全局变量任务
            switch (task.task_step_type)
            {
                case (int)TaskStepType.GUARD: WorkInfo.WatchState = (int)TaskKillType.LOSE; break;
                case (int)TaskStepType.ARREST_RUMOR: WorkInfo.ArrestRumorSceneId = 0; break;
                case (int)TaskStepType.ARREST_FIRE: WorkInfo.ArrestFireSceneId = 0; break;
                case (int)TaskStepType.ARREST_BREAK: WorkInfo.ArrestBreakSceneId = 0; break;
                case (int)TaskStepType.ARREST_SEll_WINE: WorkInfo.ArrestSellSceneId = 0; break;
                case (int)TaskStepType.STAND_GUARD: WorkInfo.GuardSceneId = 0; break;
            }
            return new ASObject(BuildData((int)ResultType.SUCCESS, id));
        }

        /// <summary>组装数据</summary>
        private Dictionary<String, Object> BuildData(int result, Int64 id)
        {
            return new Dictionary<string, object> { { "result", result }, { "id", id } };
        }
    }
}

