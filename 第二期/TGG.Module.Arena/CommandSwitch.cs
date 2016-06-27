using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Module.Arena.Service;
using TGG.SocketServer;

namespace TGG.Module.Arena
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
            return Switch((int)ModuleNumber.ARENA, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.竞技场))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)TGG.Core.Enum.Command.ArenaCommand.ARENA_JOIN: { aso = ARENA_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)TGG.Core.Enum.Command.ArenaCommand.ARENA_FIGHT_LOOK: { aso = ARENA_FIGHT_LOOK.GetInstance().CommandStart(session, data); break; }
                case (int)TGG.Core.Enum.Command.ArenaCommand.ARENA_DEKARON: { aso = ARENA_DEKARON.GetInstance().CommandStart(session, data); break; }
                case (int)TGG.Core.Enum.Command.ArenaCommand.ARENA_DEKARON_ADD: { aso = ARENA_DEKARON_ADD.GetInstance().CommandStart(session, data); break; }
                case (int)TGG.Core.Enum.Command.ArenaCommand.ARENA_REMOVE_COOLING: { aso = ARENA_REMOVE_COOLING.GetInstance().CommandStart(session, data); break; }
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
