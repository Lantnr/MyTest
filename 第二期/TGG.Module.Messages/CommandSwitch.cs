using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.Messages.Service;
using TGG.SocketServer;

namespace TGG.Module.Messages
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
            return Switch((int)ModuleNumber.MESSAGES, commandNumber, session, data);
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
                case (int)MessageCommand.MESSAGE_VIEW: { aso = MESSAGE_VIEW.GetInstance().CommandStart(session, data); break; }
                case (int)MessageCommand.MESSAGE_ATTACHMENT: { aso = MESSAGE_ATTACHMENT.GetInstance().CommandStart(session, data); break; }
                case (int)MessageCommand.MESSAGE_DETELE: { aso = MESSAGE_DETELE.GetInstance().CommandStart(session, data); break; }
                //case (int)MessageCommand.MESSAGE_FRIENDS: { aso = MESSAGE_FRIENDS.GetInstance().CommandStart(session, data); break; }
                //case (int)MessageCommand.MESSAGE_SEND: { aso = MESSAGE_SEND.GetInstance().CommandStart(session, data); break; }
                case (int)MessageCommand.MESSAGE_READ: { aso = MESSAGE_READ.GetInstance().CommandStart(session, data); break; }
                case (int)MessageCommand.MESSAGE_NOREAD: { aso = MESSAGE_NOREAD.GetInstance().CommandStart(session); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(),GetType().Namespace);
#endif
            return aso;
        }
    }
}
