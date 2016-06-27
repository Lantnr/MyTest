using System.Diagnostics;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.Chat.Service;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Chat
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
            return Switch((int)ModuleNumber.CHAT, commandNumber, session, data);
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
                case (int)ChatsCommand.CHATS: { aso = CHATS.GetInstance().CommandStart(session, data); break; }
                case (int)ChatsCommand.CHATS_EXTEND: { aso = CHATS_EXTEND.GetInstance().CommandStart(session, data); break; }
                case (int)ChatsCommand.CHATS_TOWER_TEST: { aso = CHATS_TOWER_TEST.GetInstance().CommandStart(session, data); break; }
                //case (int)ChatsCommand.CHATS_TOWER_COUNT: { aso = CHATS_TOWER_COUNT.GetInstance().CommandStart(session, data); break; }
                //case (int)ChatsCommand.CHATS_SYSTEM_PUSH: { aso = Service.CHATS_SYSTEM_PUSH.getInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
