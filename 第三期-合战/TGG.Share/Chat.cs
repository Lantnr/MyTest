using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Chat : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region 公共方法

        /// <summary>家族公告推送</summary>
        /// <param name="uid">玩家ID</param>
        /// <param name="fid">玩家家族ID</param>
        /// <param name="name">玩家名称</param>
        /// <param name="data">推送的文本</param>
        public void FamilyPush(Int64 uid, Int64 fid, string name, string data)
        {
            var list = Variable.OnlinePlayer.Select(m => m.Value as TGGSession)
                .Where(m => m.Player.Family.fid == fid).ToList();//同家族

            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new FamilyPushObject
                {
                    session = item,
                    user_id = uid,
                    player_name = name,
                    data = data,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as FamilyPushObject;
                    if (_obj == null) return;
                    PlayerChatSend(_obj.session, (int)ChatsType.CHATS_FAMILY, _obj.user_id, _obj.player_name, (int)ChatsPushType.NO_PRIVATE, _obj.data, null);
                    token.Cancel();
                }, obj, token.Token);
            }
        }

        /// <summary>玩家信息推送</summary>
        public void PlayerChatSend(Int64 userid, int type, Int64 id, string name, int ptype, string data, List<ASObject> vo)
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
            TrainingRoleEndSend(session, new ASObject(dic), (int)ChatsCommand.CHATS_PUSH);
        }



        /// <summary>玩家信息推送</summary>
        public void PlayerChatSend(TGGSession session, int type, decimal id, string name, int ptype, string data, List<ASObject> vo)
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
            TrainingRoleEndSend(session, new ASObject(dic), (int)ChatsCommand.CHATS_PUSH);
        }

        /// <summary>系统信息推送</summary>
        public void SystemChatSend(TGGSession session, dynamic data, int baseid)
        {
            var dic = new Dictionary<string, object> { { "data", data }, { "baseid", baseid } };
            TrainingRoleEndSend(session, new ASObject(dic), (int)ChatsCommand.CHATS_SYSTEM_PUSH);
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

        #endregion

        #region 私有方法

        /// <summary>推送协议</summary>
        private void TrainingRoleEndSend(TGGSession session, ASObject data, int commandnumber)
        {
            var pv = session.InitProtocol((int)ModuleNumber.CHAT, commandnumber, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary>
        /// 线程对象
        /// </summary>
        class FamilyPushObject
        {
            public TGGSession session { get; set; }

            public Int64 user_id { get; set; }

            public String player_name { get; set; }

            public String data { get; set; }
        }

        #endregion
    }
}
