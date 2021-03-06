﻿using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Siege.Service;
using TGG.SocketServer;

namespace TGG.Module.Siege
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
            return Switch((int)ModuleNumber.SIEGE, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.美浓))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);

            var aso = new ASObject();
           
            //指令匹配
            switch (commandNumber)
            {
                case (int)SiegeCommand.ENTER: { aso = ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.MAKE_LADDER: { aso = MAKE_LADDER.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.ENTER_ENTRY_POINT: { aso = ENTER_ENTRY_POINT.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.DEFEND: { aso = DEFEND.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.ATTACK_BOSS: { aso = ATTACK_BOSS.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.BROKEN_BASE: { aso = BROKEN_BASE.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.EXIT: { aso = EXIT.GetInstance().CommandStart(session); break; }
                case (int)SiegeCommand.MOVING: { aso = MOVING.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.ATTACK_GATE: { aso = ATTACK_GATE.GetInstance().CommandStart(session, data); break; }
                case (int)SiegeCommand.GO_BACK: { aso = GO_BACK.GetInstance().CommandStart(session, data); break; }
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
