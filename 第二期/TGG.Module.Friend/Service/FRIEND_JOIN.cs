using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Friend.Service
{
    /// <summary>
    /// 获取好友指令
    /// Author:arlen xiao
    /// </summary>
    public class FRIEND_JOIN
    {
        private static FRIEND_JOIN ObjInstance;

        /// <summary>FRIEND_JOIN单体模式</summary>
        public static FRIEND_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FRIEND_JOIN());
        }

        /// <summary>获取好友</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FRIEND_JOIN", "获取好友指令");
#endif
            var list = tg_friends.GetFindListByUserId(session.Player.User.id);
#if DEBUG
            XTrace.WriteLine("好友集合大小 {0}", list.Count);
#endif
            if (!list.Any()) return new ASObject(Common.GetInstance().BuildData((int)ResultType.NO_DATA, new List<view_user_role_friend>()));
            var view = view_user_role_friend.GetFindByFriendList(list);

            var list_friend = Common.GetInstance().CheckPlayerOnline(view);//Variable.OnlinePlayer
            GetBlackList(session, view);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, list_friend));
        }

        /// <summary>获取黑名单</summary>
        private void GetBlackList(TGGSession session, IEnumerable<view_user_role_friend> list)
        {
            foreach (var item in list.Where(item => item.friend_state == (int)FriendStateType.BLACKLIST))
            {
                if (!session.Player.BlackList.Contains(item.friend_id))
                    session.Player.BlackList.Add(item.friend_id);
            }
        }
    }
}
