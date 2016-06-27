using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.RoleTrain.Service;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain
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
            return Switch((int)ModuleNumber.ROLETRAIN, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            switch (commandNumber)
            {
                case (int)RoleTrainCommand.TRAIN_TEA_INSIGHT:
                case (int)RoleTrainCommand.TRAIN_HOME_JOIN:
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_STEAL:
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH:
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_FIGHT:
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_TEA:
                case (int)RoleTrainCommand.TRAIN_HOME_LEVEL_SELECT:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.武将宅))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
                case (int)RoleTrainCommand.TRAIN_ROLE_UNSELECT:
                case (int)RoleTrainCommand.TRAIN_ROLE_SELECT:
                case (int)RoleTrainCommand.TRAIN_ROLE_START:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.修行))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
                default:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.自宅))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)RoleTrainCommand.TRAIN_INHERIT_ROLE_SELECT: { aso = TRAIN_INHERIT_ROLE_SELECT.getInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_INHERIT_ROLE: { aso = TRAIN_INHERIT_ROLE.getInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_ROLE_START: { aso = TRAIN_ROLE_START.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_ROLE_SELECT: { aso = TRAIN_ROLE_SELECT.GetInstance().CommandStart(session, data); break; }
                //case (int)RoleTrainCommand.TRAIN_ROLE_LOCK: { aso = TRAIN_ROLE_LOCK.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_ROLE_UNSELECT: { aso = TRAIN_ROLE_UNSELECT.GetInstance().CommandStart(session, data); break; }
                //case (int)RoleTrainCommand.TRAIN_ROLE_ACCELERATE: { aso = TRAIN_ROLE_ACCELERATE.GetInstance().CommandStart(session, data); break; }

                case (int)RoleTrainCommand.TRAIN_HOME_LEVEL_SELECT: { aso = TRAIN_HOME_LEVEL_SELECT.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_TEA: { aso = TRAIN_HOME_NPC_TEA.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_FIGHT: { aso = TRAIN_HOME_NPC_FIGHT.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH: { aso = TRAIN_HOME_NPC_REFRESH.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_STEAL: { aso = TRAIN_HOME_NPC_STEAL.GetInstance().CommandStart(session, data); break; }
                case (int)RoleTrainCommand.TRAIN_HOME_JOIN: { aso = TRAIN_HOME_JOIN.GetInstance().CommandStart(session, data); break; }

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
