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
    public class WORK_JOIN
    {
        private static WORK_JOIN _objInstance;

        /// <summary> WORK_JOIN单体模式 </summary>s
        public static WORK_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new WORK_JOIN());
        }
        object lockObj = new object();
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}登陆时查询工作任务", "WORK_JOIN", session.Player.User.player_name);
#endif
            var userid = session.Player.User.id;
            Int64 time = 0;
            var usertask = tg_task.GetTaskQueryByType(userid, (int)TaskType.WORK_TASK);
            var worktask = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
            var now = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            lock (lockObj)
            {
                for (int i = 0; i < usertask.Count; i++)
                {
                    if (usertask[i].task_state == (int)TaskStateType.TYPE_UNRECEIVED && usertask[i].task_endtime != 0)
                    {
                        usertask[i].task_endtime = 0;
                        usertask[i].Update();
                        continue;
                    }
                    if (usertask[i].task_state == (int)TaskStateType.TYPE_UNFINISHED && usertask[i].task_endtime <= now)
                    {
                        var id = usertask[i].id;
                         usertask[i] = Common.GetInstance().WorkTasksInit(usertask[i]);
                        usertask[i].id = id;
                        usertask[i].Update();
                    }
                }
            }
            if (worktask != null && usertask.Any())
            {
                time = worktask.CoolingTime;
            }
            return BuildData((int)ResultType.SUCCESS, usertask, time);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        private ASObject BuildData(object result, IEnumerable<tg_task> list_task, Int64 time)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"workVo",list_task.Any()? Common.GetInstance().ConvertListASObject(list_task):null},
                {"time",time}
            };
            return new ASObject(dic);
        }
    }
}
