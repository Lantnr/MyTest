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
    /// 添加黑名单指令
    /// Author:arlen xiao
    /// </summary>
    public class FRIEND_BLACKLIST
    {
        private static FRIEND_BLACKLIST ObjInstance;

        /// <summary>FRIEND_BLACKLIST单体模式</summary>
        public static FRIEND_BLACKLIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FRIEND_BLACKLIST());
        }

        /// <summary>添加黑名单指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FRIEND_BLACKLIST", "添加黑名单指令");
#endif
            var name = data.FirstOrDefault(q => q.Key == "name").Value.ToString();
            var friend = tg_user.GetUserByName(name);
            if (friend == null) return ResultError((int)ResultType.FRIEND_NO_DATA_ERROR);
            if (friend.player_name == session.Player.User.player_name) return ResultError((int)ResultType.FRIEND_ON_ONESELF);
            //if (!Variable.OnlinePlayer.ContainsKey(friend.id)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRIEND_OFFONLINE_ERROR));

            var count = tg_friends.GetFindCountByState(session.Player.User.id, (int)FriendStateType.FRIEND);
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "19002");
            if (rule == null) return ResultError((int)ResultType.BASE_TABLE_ERROR);
            var _max = Convert.ToInt32(rule.value);
            if (count >= _max) return ResultError((int)ResultType.FRIEND_MAX_ERROR);

            var entity = tg_friends.GetFindEntity(session.Player.User.id, friend.id);
            if (entity != null)
            {
                if (entity.friend_state == (int)FriendStateType.BLACKLIST)
                    return ResultError((int)ResultType.FRIEND_BLACKLIST_EXIST);
            }
            else
            {
                entity = new tg_friends()
                    {
                        user_id = session.Player.User.id,
                        friend_id = friend.id,
                    };
            }
#if DEBUG
            XTrace.WriteLine("id  {0} user_id {1} friend_id {2} friend_state {3}", entity.id, entity.user_id, entity.friend_id, entity.friend_state);
#endif

            entity.friend_state = (int)FriendStateType.BLACKLIST;
#if DEBUG
            XTrace.WriteLine("更新状态后 id  {0} user_id {1} friend_id {2} friend_state {3}", entity.id, entity.user_id, entity.friend_id, entity.friend_state);
#endif
            try
            {
                entity.Save();
                if (!session.Player.BlackList.Contains(entity.friend_id))
                    session.Player.BlackList.Add(entity.friend_id);
            }
            catch { return ResultError((int)ResultType.DATABASE_ERROR); }
            var model = view_user_role_friend.GetFindById(entity.id);
            var view_friend = Common.GetInstance().CheckSingleOnline(model);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, view_friend));
        }

        /// <summary>错误结果返回</summary>
        private ASObject ResultError(Int32 type)
        {
            return new ASObject(Common.GetInstance().BuildData(type));
        }
    }
}
