using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_friends
    {
        /// <summary>根据用户Id获取实体集合</summary>
        public static IEnumerable<tg_friends> GetEntityListByUserId(Int64 userid)
        {
            return FindAll(string.Format("user_id={0}", userid), null, null, 0, 0);
        }

        /// <summary>双方是否有一方在黑名单</summary>
        public static int GetCountByUserId(Int64 userid, Int64 rivalid)
        {
            return FindCount(string.Format(" friend_state={0} and ((user_id={1} and friend_id={2}) or (user_id={3} and friend_id={4})) ", (int)FriendStateType.BLACKLIST, userid, rivalid, rivalid, userid), null, null, 0, 0);
        }

        /// <summary>根据用户Id获取黑名单好友集合</summary>
        public static IEnumerable<tg_friends> GetEntityListByUserIdAndState(Int64 userid)
        {
            return FindAll(string.Format(" friend_state={0} and user_id={1}", (int)FriendStateType.BLACKLIST, userid), null, null, 0, 0);
        }
    }
}
