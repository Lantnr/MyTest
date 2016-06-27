using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{

    public class WORK_DROP : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WORK_DROP()
        {
            Dispose();
        }
    
        #endregion
        //public static WORK_DROP ObjInstance;

        ///// <summary> WORK_DROP单体模式 </summary>
        //public static WORK_DROP GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new WORK_DROP());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {

#if DEBUG
            XTrace.WriteLine("{0}:{1}工作任务放弃", "WORK_DROP", session.Player.User.player_name);
#endif
            if (!data.ContainsKey("id")) return null;
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var task = tg_task.GetEntityByIdAndUserId(id, session.Player.User.id); //tg_task.FindByid(id);

            if (task == null || task.task_type != (int)TaskType.WORK_TASK)
                return BuildData((int)ResultType.TASK_VOCATION_NOTASK, null);

            if (task.task_step_type == (int)TaskStepType.RAISE_COIN)
            {
                var key = string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, task.user_id, task.id);
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            }
            task = Common.GetInstance().WorkTasksInit(task);
            task.id = id;
            task.Update();
            Common.GetInstance().ClearTime(task.user_id);
            return BuildData((int)ResultType.SUCCESS, task);
        }


        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private ASObject BuildData(int result, tg_task task)
        {
            return new ASObject(new Dictionary<string, object>()
          {
              {"result",result},
              {"workVo",task==null?null:EntityToVo.ToVocationTaskVo(task)}

          });


        }
    }
}
