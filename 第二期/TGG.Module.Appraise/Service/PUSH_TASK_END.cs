using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Appraise.Service
{
    /// <summary>
    /// 评定结束
    /// 开发者：李德雁
    /// </summary>
    public class PUSH_TASK_END
    {
        private static PUSH_TASK_END _objInstance;

        /// <summary>PUSH_TASK_END 单体模式</summary>
        public static PUSH_TASK_END GetInstance()
        {
            return _objInstance ?? (_objInstance = new PUSH_TASK_END());
        }

        /// <summary> 评定结束</summary>
        public void CommandStart(tg_task task)
        {
#if DEBUG
            XTrace.WriteLine("任务{0}评定结束{1}--{2}", task.id, task.user_id, "PUSH_TASK_END");
#endif
            Common.GetInstance().AppraiseEnd(task);
            if (Variable.OnlinePlayer.ContainsKey(task.user_id))
            {
                var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
                if (session == null) return;
#if DEBUG
                XTrace.WriteLine("玩家{0}在线，向前端推送任务结束{0}", task.id);
#endif
                var dic = new Dictionary<string, object>() { { "task", EntityToVo.ToRoleVo(task) } };
                var aso = new ASObject(dic);
                var pv = session.InitProtocol((int)ModuleNumber.APPRAISE,
                          (int)AppraiseCommand.PUSH_TASK_END, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
            }
        }






    }
}
