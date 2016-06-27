using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Role.Service;
using TGG.SocketServer;

namespace TGG.Module.Role
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
            return Switch((int)ModuleNumber.ROLE, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)RoleCommand.RECRUIT_JOIN ||
                commandNumber == (int)RoleCommand.RECRUIT_GET ||
                commandNumber == (int)RoleCommand.RECRUIT_REFRESH)
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.酒馆))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)RoleCommand.ROLE_JOIN: { aso = ROLE_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_EXILE: { aso = ROLE_EXILE.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_SKILL_SELECT: { aso = ROLE_SKILL_SELECT.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_EQUIP_LOAD: { aso = ROLE_EQUIP_LOAD.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_EQUIP_UNLOAD: { aso = ROLE_EQUIP_UNLOAD.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_ATTRIBUTE: { aso = ROLE_ATTRIBUTE.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.RECRUIT_JOIN: { aso = RECRUIT_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.RECRUIT_GET: { aso = RECRUIT_GET.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.RECRUIT_REFRESH: { aso = RECRUIT_REFRESH.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.SELECT_GENRE: { aso = SELECT_GENRE.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.ROLE_HIRE: { aso = ROLE_HIRE.GetInstance().CommandStart(session, data); break; }
                case (int)RoleCommand.QUERY_PLAYER_ROLE: { aso = QUERY_PLAYER_ROLE.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
