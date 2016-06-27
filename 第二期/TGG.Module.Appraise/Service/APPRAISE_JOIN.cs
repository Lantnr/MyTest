using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Configuration;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Appraise.Service
{
    /// <summary>
    /// 进入家臣评定
    /// 开发者：李德雁
    /// </summary>
    public class APPRAISE_JOIN
    {
        private static APPRAISE_JOIN _objInstance;

        /// <summary>APPRAISE_JOIN 单体模式</summary>
        public static APPRAISE_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new APPRAISE_JOIN());
        }

        /// <summary> 进入家臣任务评定</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("进入家臣评定指令{0}——{1}", session.Player.User.player_name, "APPRAISE_JOIN");
#endif
            var dic = new Dictionary<string, object>();
            var tasks = tg_task.GetTaskQueryByType(session.Player.User.id,(int)TaskType.ROLE_TASK).OrderByDescending(q => q.id).ToList(); //按照主键id倒序排列 新接的任务在前
            var now  =(DateTime.Now.Ticks - 621355968000000000) / 10000;
            foreach (var task in tasks.Where(q => q.task_state != (int)TaskStateType.TYPE_UNRECEIVED
                && q.task_state != (int)TaskStateType.TYPE_FINISHED)) //未接受或已完成的任务不做特殊处理
            {
                if (task.task_endtime != 0 && task.task_endtime <= now)  //到达时间小于等于当前时间
                    Common.GetInstance().AppraiseEnd(task);
                else
                {
                    var times = task.task_endtime - now;
                    Common.GetInstance().NewTaskStart(times, task);
                }
            }
#if DEBUG
            XTrace.WriteLine("玩家的家臣任务数{0}，剩余刷新数{1}", tasks.Count, 2 - session.Player.UserExtend.task_role_refresh);
#endif
            var reflashcount = 2 - session.Player.UserExtend.task_role_refresh;
            dic.Add("result", (int)ResultType.SUCCESS);
            dic.Add("task", tasks.Any() ? Common.GetInstance().ConvertListAsObject(tasks) : null);
            dic.Add("count", reflashcount);
            return new ASObject(dic);
        }
    }
}
