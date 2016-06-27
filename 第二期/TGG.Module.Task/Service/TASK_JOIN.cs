using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 登陆时查询任务
    /// 开发者：李德雁
    /// </summary>
    public class TASK_JOIN
    {
        public static TASK_JOIN objInstance = null;

        /// <summary> TASK_JOIN单体模式 </summary>
        public static TASK_JOIN getInstance()
        {
            return objInstance ?? (objInstance = new TASK_JOIN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}登陆时查询任务", "TASK_JOIN", session.Player.User.player_name);
#endif
            var count = 2 - session.Player.UserExtend.task_vocation_refresh;
            var userid = session.Player.User.id;
            var usertask = tg_task.GetEntityById(userid);
            var maintask = usertask.FirstOrDefault(q => q.task_type == (int)TaskType.MAIN_TASK);
            if (maintask == null)
                return new ASObject(Common.getInstance().BuildData((int)ResultType.TASK_NO_MAINTASK, usertask, count, session.Player.UserExtend.task_vocation_isgo));
            session.MainTask = maintask;
            return new ASObject(Common.getInstance().BuildData((int)ResultType.SUCCESS, usertask, count, session.Player.UserExtend.task_vocation_isgo));
        }

    }
}
