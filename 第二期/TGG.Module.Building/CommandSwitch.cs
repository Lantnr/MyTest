using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Building.Service;
using TGG.SocketServer;

namespace TGG.Module.Building
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
            return Switch((int)ModuleNumber.BUILDING, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.一夜))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)BuildingCommand.ENTER:
                    {
                        aso = ENTER.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.GET_WOOD:
                    {
                        aso = GET_WOOD.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.TORCH:
                    {
                        aso = TORCH.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.MAKE_BUILD:
                    {
                        aso = MAKE_BUILD.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.BUILDING:
                    {
                        aso = BUILDING.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.FIRE:
                    {
                        aso = FIRE.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.EXIT:
                    {
                        aso = EXIT.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.MOVING:
                    {
                        aso = MOVING.getInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.KILL_BOSS:
                    {
                        aso = KILL_BOSS.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)BuildingCommand.BACK_POINT:
                    {
                        aso = BACK_POINT.GetInstance().CommandStart(session, data);
                        break;
                    }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
