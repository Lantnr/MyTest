using System;
using FluorineFx;
using System.Collections.Generic;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 玩家信息推送
    /// </summary>
    public class CHATS_PUSH
    {
        public static CHATS_PUSH ObjInstance;

        /// <summary>CHATS_PUSH单体模式</summary>
        public static CHATS_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CHATS_PUSH());
        }

        /// <summary>玩家信息推送</summary>
        public void CommandStart(TGGSession session, int type, decimal id, string name, int ptype, string data, List<ASObject> vo)
        {
            var dic = new Dictionary<string, object>
            {
                {"type", type},
                {"id", id},
                {"name", name},
                {"ptype", ptype},
                {"data", data},
                {"goods", vo}
            };
            TrainingRoleEndSend(session, new ASObject(dic));
        }

        /// <summary>玩家信息推送</summary>
        public void CommandStart(Int64 userid, int type, Int64 id, string name, int ptype, string data, List<ASObject> vo)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            if (session.Player.BlackList.Contains(id)) return; //验证黑名单

            var dic = new Dictionary<string, object>
            {
                {"type", type},
                {"id", id},
                {"name", name},
                {"ptype", ptype},
                {"data", data},
                {"goods", vo}
            };
            TrainingRoleEndSend(session, new ASObject(dic));
        }

        /// <summary>推送协议</summary>
        public void TrainingRoleEndSend(TGGSession session, ASObject data)
        {
            var pv = session.InitProtocol((int)ModuleNumber.CHAT, (int)ChatsCommand.CHATS_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
