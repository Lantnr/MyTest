using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.User.Service;
using TGG.SocketServer;

namespace TGG.Module.User
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch
    {
        public static CommandSwitch ObjInstance;

        /// <summary>CommandSwitch单例模式 </summary>
        public static CommandSwitch GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CommandSwitch());
        }

        /// <summary>用户指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.USER, commandNumber, session, data);
        }

        /// <summary>用户指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)UserCommand.USER_SALARY)
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.俸禄))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)UserCommand.USER_CREATE: { aso = USER_CREATE.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.USER_LOGIN: { aso = USER_LOGIN.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.CONSUME: { aso = CONSUME.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.HEARTBEAT: { aso = HEARTBEAT.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.REWARDS: { REWARDS.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.USER_SALARY: { aso = USER_SALARY.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.USER_MODULE_ACTIVETION: { aso = USER_MODULE_ACTIVETION.GetInstance().CommandStart(session, data); break; }
                case (int)UserCommand.ACTIVITY_OPEN: { aso = ACTIVITY_OPEN.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
