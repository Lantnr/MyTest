using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Family.Service;
using TGG.SocketServer;

namespace TGG.Module.Family
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
            return Switch((int)ModuleNumber.FAMILY, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.家族))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)FamilyCommand.FAMILY_JOIN: { aso = FAMILY_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_CREATE: { aso = FAMILY_CREATE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_APPLY: { aso = FAMILY_APPLY.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_UPDATE: { aso = FAMILY_UPDATE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_NOTICE_UPDATE: { aso = FAMILY_NOTICE_UPDATE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_DONATE: { aso = FAMILY_DONATE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_RECEIVE: { aso = FAMILY_RECEIVE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_EXIT: { aso = FAMILY_EXIT.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_INVITE: { aso = FAMILY_INVITE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_INVITE_REPLY: { aso = FAMILY_INVITE_REPLY.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_APPLY_PROCESS: { aso = FAMILY_APPLY_PROCESS.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_REMOVE: { aso = FAMILY_REMOVE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_DISSOLVE: { aso = FAMILY_DISSOLVE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_OFFICE: { aso = FAMILY_OFFICE.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_LOG: { aso = FAMILY_LOG.GetInstance().CommandStart(session, data); break; }
                case (int)FamilyCommand.FAMILY_APPLY_LIST: { aso = FAMILY_APPLY_LIST.GetInstance().CommandStart(session, data); break; }              
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(),GetType().Namespace);
#endif
            return aso;
        }
    }
}
