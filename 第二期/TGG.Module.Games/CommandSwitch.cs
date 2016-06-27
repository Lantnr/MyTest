using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Module.Games.Service;
using TGG.SocketServer;
using TGG.Core.Enum.Command;

namespace TGG.Module.Games
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
            return Switch((int)ModuleNumber.GAMES, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)GameCommand.GAMES_JOIN: { aso = GAMES_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_SHOT_FINISH: { aso = GAMES_SHOT_FINISH.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_ELOQUENCE_ENTER: { aso = GAMES_ELOQUENCE_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_ELOQUENCE_START: { aso = GAMES_ELOQUENCE_START.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_TEA_ENTER: { aso = GAMES_TEA_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_TEA_START: { aso = GAMES_TEA_START.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_NINJUTSU_ENTER: { aso = GAMES_NINJUTSU_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_NINJUTSU_START: { aso = GAMES_NINJUTSU_START.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_CALCULATE_START: { aso = GAMES_CALCULATE_START.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_RECEIVE: { aso = GAMES_RECEIVE.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_SHOT_ENTER: { aso = GAMES_SHOT_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_CALCULATE_ENTER: { aso = GAMES_CALCULATE_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAME_EXIT: { aso = GAME_EXIT.GetInstance().CommandStart(session, data); break; }
                case (int)GameCommand.GAMES_SELECT: { aso = GAMES_SELECT.GetInstance().CommandStart(session, data); break; }               
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
