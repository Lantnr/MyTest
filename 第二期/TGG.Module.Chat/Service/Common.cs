using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

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
                    CHATS_PUSH.GetInstance().CommandStart(_obj.session, (int)ChatsType.CHATS_FAMILY, _obj.user_id, _obj.player_name, (int)ChatsPushType.NO_PRIVATE, _obj.data, null);

                }, obj, token.Token);
            }
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

    }
}
