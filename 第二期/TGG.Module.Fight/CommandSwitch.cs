using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Fight.Service;
using TGG.SocketServer;

namespace TGG.Module.Fight
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
            return Switch((int)ModuleNumber.FIGHT, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)FightCommand.FIGHT_PERSONAL_ENTER ||
                commandNumber == (int)FightCommand.FIGHT_PROP ||
                commandNumber == (int)FightCommand.FIGHT_PROP_PICKUP
                )
            {
                //不需要等级验证
            }
            else
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.布阵))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)FightCommand.FIGHT_PERSONAL_ENTER: { aso = FIGHT_PERSONAL_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PROP_PICKUP: { aso = FIGHT_PROP_PICKUP.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PROP: { aso = FIGHT_PROP.GetInstance().CommandStart(session, data); break; }

                case (int)FightCommand.FIGHT_PERSONAL_JOIN: { aso = FIGHT_PERSONAL_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PERSONAL_ROLE_SELECT: { aso = FIGHT_PERSONAL_ROLE_SELECT.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PERSONAL_ROLE_DELETE: { aso = FIGHT_PERSONAL_ROLE_DELETE.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PERSONAL_ROLE_CHANGE: { aso = FIGHT_PERSONAL_ROLE_CHANGE.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.YIN_JION: { aso = YIN_JION.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.YIN_UPGRADE: { aso = YIN_UPGRADE.GetInstance().CommandStart(session, data); break; }
                case (int)FightCommand.FIGHT_PERSONAL_YIN_SELECT: { aso = FIGHT_PERSONAL_YIN_SELECT.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
