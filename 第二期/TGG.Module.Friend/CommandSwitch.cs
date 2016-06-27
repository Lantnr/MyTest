using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Friend.Service;
using TGG.SocketServer;

namespace TGG.Module.Friend
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
            return Switch((int)ModuleNumber.FRIEND, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.好友))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)FriendCommand.FRIEND_ADD: { aso = FRIEND_ADD.GetInstance().CommandStart(session, data); break; }
                case (int)FriendCommand.FRIEND_BLACKLIST: { aso = FRIEND_BLACKLIST.GetInstance().CommandStart(session, data); break; }
                case (int)FriendCommand.FRIEND_JOIN: { aso = FRIEND_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)FriendCommand.FRIEND_REMOVE_BLACKLIST: { aso = FRIEND_DELETE.GetInstance().CommandStart(session, data); break; }
                case (int)FriendCommand.FRIEND_DELETE: { aso = FRIEND_DELETE.GetInstance().CommandStart(session, data); break; }

            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
