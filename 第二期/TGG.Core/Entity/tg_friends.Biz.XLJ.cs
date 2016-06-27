using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 好友逻辑类
    /// </summary>
    public partial class tg_friends
    {
        /// <summary>根据用户Id获取实体集合</summary>
        public static tg_friends GetFindEntity(Int64 userid, Int64 friendid)
        {
            return Find(new String[] { _.user_id, _.friend_id }, new Object[] { userid, friendid });
        }

        /// <summary>根据用户Id获取实体集合</summary>
        public static List<tg_friends> GetFindListByUserId(Int64 userid)
        {
            var exp = new WhereExpression();
            exp &= _.user_id == userid;
            return FindAll(exp, null, null, 0, 0);
        }
        /// <summary>根据用户Id和状态获取实体集合</summary>
        public static List<tg_friends> GetFindListByState(Int64 userid, int state)
        {
            var exp = new WhereExpression();
            exp &= _.user_id == userid;
            exp &= _.friend_state == state;
            return FindAll(exp, null, null, 0, 0);
        }

        /// <summary>获取对应好友状态数量</summary>
        public static int GetFindCountByState(Int64 userid, int state)
        {
            var exp = new WhereExpression();
            exp &= _.user_id == userid;
            exp &= _.friend_state == state;
            return FindCount(exp, null, null, 0, 0);
        }

    }
}
