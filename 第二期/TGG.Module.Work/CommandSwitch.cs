using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Work.Service;
using TGG.SocketServer;

namespace TGG.Module.Work
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
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.WORK, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)WorkCommand.WORK_UPDATE||
                commandNumber == (int)WorkCommand.WORK_ACCEPT||
                commandNumber == (int)WorkCommand.WORK_DROP
                )
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.工作))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)WorkCommand.WORK_JOIN: { aso = WORK_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_UPDATE: { aso = WORK_UPDATE.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_ACCEPT: { aso = WORK_ACCEPT.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_CANCEL: { aso = WORK_CANCEL.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_IS_FIGHT: { aso = WORK_IS_FIGHT.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_FIGHT: { aso = WORK_FIGHT.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_SEARCH: { aso = WORK_SEARCH.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_DROP: { aso = WORK_DROP.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_SELECT_DEFEND: { aso = WORK_SELECT_DEFEND.GetInstance().CommandStart(session, data); break; }
                case (int)WorkCommand.WORK_CHECK_DEFEND: { aso = WORK_CHECK_DEFEND.GetInstance().CommandStart(session, data); break; }

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
