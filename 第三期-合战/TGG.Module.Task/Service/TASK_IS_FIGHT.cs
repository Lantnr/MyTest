using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary> 请求是否触发拦路 </summary>
    public class TASK_IS_FIGHT : IDisposable
    {

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~TASK_IS_FIGHT()
        {
            Dispose();
        }
    
        #endregion
        //private static TASK_IS_FIGHT _objInstance;

        ///// <summary> TASK_IS_FIGHT单体模式 </summary>
        //public static TASK_IS_FIGHT GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new TASK_IS_FIGHT());
        //}

        /// <summary> 请求是否触发拦路 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var taskid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "taskId").Value);

            var task = tg_task.FindByid(taskid);
            if (task == null || task.task_type != (int)TaskType.VOCATION_TASK)
                return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);
            if (task.task_state != (int)TaskStateType.TYPE_UNFINISHED) return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);
            if (task.is_lock == (int)GuardTaskType.TRIGGER) return new ASObject(BulidData(false));

            var stepList = task.task_step_data.Split('|');
            if (!stepList.Any()) return CommonHelper.ErrorResult((int)ResultType.TASK_STEP_NULL);
            var type = stepList[0].Split('_').First();
            if (type != Convert.ToInt32(TaskStepType.ESCORT).ToString())
                return CommonHelper.ErrorResult((int)ResultType.TASK_NO_ESCORT);

            var r = new RandomSingle();
            if (r.IsTrue(75)) return new ASObject(BulidData(true));
            task.is_lock = (int)GuardTaskType.TRIGGER;
            task.Update();
            return new ASObject(BulidData(false));
        }

        /// <summary> 组装数据 </summary>
        /// <param name="falg">是否触发</param>
        /// <returns></returns>
        private Dictionary<string, object> BulidData(bool falg)
        {
            var dic = new Dictionary<string, object> { { "state", falg ? 1 : 0 } };
            return dic;
        }

    }
}
