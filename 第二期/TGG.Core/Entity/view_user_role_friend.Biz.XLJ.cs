using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 逻辑部分类
    /// </summary>
    public partial class view_user_role_friend
    {
        /// <summary>获取好友集合</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<view_user_role_friend> GetFindByFriendList(List<tg_friends> list)
        {
            var exp = new WhereExpression();
            var item = list.FirstOrDefault();
            Int64 user_id = 0;
            if (item != null) user_id = item.user_id;
            exp &= _.user_id == user_id;
            exp &= _.friend_id.In(list.Select(m => m.friend_id));
            return FindAll(exp, null, null, 0, 0);
        }

        /// <summary>获取好友信息</summary>
        public static view_user_role_friend GetFindById(Double id)
        {
            //var exp = new WhereExpression();
            //exp &= _.uid.In(list.Select(m => m.friend_id));
            return Find(_.id, id);
        }

    }
}
