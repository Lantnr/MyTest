using FluorineFx;
using System.Collections.Generic;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 系统信息推送
    /// </summary>
    public class CHATS_SYSTEM_PUSH
    {
        public static CHATS_SYSTEM_PUSH ObjInstance;

        /// <summary>CHATS_SYSTEM_PUSH单体模式</summary>
        public static CHATS_SYSTEM_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CHATS_SYSTEM_PUSH());
        }

        /// <summary>系统信息推送</summary>
        public void CommandStart(TGGSession session, dynamic data, int baseid)
        {
            var dic = new Dictionary<string, object> { { "data", data }, { "baseid", baseid } };
            TrainingRoleEndSend(session, new ASObject(dic));
        }

        /// <summary>组装数据[ASObject]</summary>
        public ASObject BuildData(int type, dynamic vo, dynamic id, dynamic name, dynamic number)
        {
            var dic = new Dictionary<string, object> { { "type", type } };
            switch (type)
            {

                case (int)ChatsASObjectType.PLAYERS:
                    {
                        dic.Add("id", id);
                        dic.Add("name", name);
                        break;
                    }
                case (int)ChatsASObjectType.GOODS: { dic.Add("vo", vo); break; }
                case (int)ChatsASObjectType.NUMBER: { dic.Add("number", number); break; }
            }
            return new ASObject(dic);
        }

        /// <summary>推送协议</summary>
        public void TrainingRoleEndSend(TGGSession session, ASObject data)
        {
            var pv = session.InitProtocol((int)ModuleNumber.CHAT, (int)ChatsCommand.CHATS_SYSTEM_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
