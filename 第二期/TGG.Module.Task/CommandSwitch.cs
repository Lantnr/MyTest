using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Task.Service;

namespace TGG.Module.Task
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch
    {
        public static CommandSwitch ObjInstance;

        /// <summary>CommandSwitch单例模式</summary>
        public static CommandSwitch GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CommandSwitch());
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGG.SocketServer.TGGSession session, ASObject data)
        {
            return Switch((int)TGG.Core.Enum.ModuleNumber.TASK, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGG.SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)TaskCommand.TASK_VOCATION_UPDATE ||
                commandNumber == (int)TaskCommand.TASK_VOCATION_ACCEPT ||
                commandNumber == (int)TaskCommand.TASK_VOCATION_BUY
                )
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.职业评定))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)TaskCommand.TASK_JOIN: { aso = TASK_JOIN.getInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_FINISH: { aso = TASK_FINISH.getInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_VOCATION_UPDATE: { aso = TASK_VOCATION_UPDATE.getInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_VOCATION_ACCEPT: { aso = TASK_VOCATION_ACCEPT.getInstance().CommandStart(session, data); break; }
                // case (int)TaskCommand.TASK_VOCATION_NOTRESET: { aso = TASK_VOCATION_NOTRESET.getInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_VOCATION_BUY: { aso = TASK_VOCATION_BUY.GetInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_CANCEL: { aso = TASK_CANCEL.GetInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_IS_FIGHT: { aso = TASK_IS_FIGHT.GetInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_FIGHT: { aso = TASK_FIGHT.GetInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_SEARCH: { aso = TASK_SEARCH.getInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_CHECK_DEFEND: { aso = TASK_CHECK_DEFEND.GetInstance().CommandStart(session, data); break; }
                case (int)TaskCommand.TASK_SELECT_DEFEND: { aso = TASK_SELECT_DEFEND.GetInstance().CommandStart(session, data); break; }
                default: break;
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
