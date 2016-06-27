using FluorineFx;
using NewLife.Log;
using System;
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
    public class CommandSwitch : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~CommandSwitch()
        {
            Dispose();
        }

        #endregion

        //public static CommandSwitch ObjInstance;
        ///// <summary>CommandSwitch单例模式 </summary>
        //public static CommandSwitch GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new CommandSwitch());
        //}

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
                case (int)UserCommand.USER_CREATE:
                    {
                        var user = new USER_CREATE();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.USER_LOGIN:
                    {
                        var user = new USER_LOGIN();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.CONSUME:
                    {
                        var user = new CONSUME();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.HEARTBEAT:
                    {
                        var user = new HEARTBEAT();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.REWARDS:
                    {
                        var user = new REWARDS();
                        user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.USER_SALARY:
                    {
                        var user = new USER_SALARY();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.USER_MODULE_ACTIVETION:
                    {
                        var user = new USER_MODULE_ACTIVETION();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.ACTIVITY_OPEN:
                    {
                        var user = new ACTIVITY_OPEN();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.OFFICIAL_UPGRADE:
                    {
                        var user = new OFFICIAL_UPGRADE();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                case (int)UserCommand.USER_GAME_UPDATE:
                    {
                        var user = new USER_GAME_UPDATE();
                        aso = user.CommandStart(session, data);
                        user.Dispose();
                        break;
                    }
                default: { aso = null; break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
