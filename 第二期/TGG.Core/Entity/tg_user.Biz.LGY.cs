using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user
    {
        public static bool GetUserUpdate(tg_user model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>根据用户id集合 查询用户集合</summary>
        public static List<tg_user> GetUsersByIds(string _ids)
        {
            return FindAll(string.Format("id in ({0})", _ids), null, null, 0, 0);
        }

        /// <summary>查询所有用户集合</summary>
        public static List<tg_user> GetAll()
        {
            return FindAll();
        }

        /// <summary>根据玩家名称集合 查询用户</summary>
        public static tg_user GetUserByName(string playname)
        {
            return Find(_.player_name, playname);
        }

        /// <summary>根据用户id 查询用户信息</summary>
        public static tg_user GetUsersById(Int64 id)
        {
            return Find(_.id, id);
        }

        /// <summary>根据用户id 查询用户是否存在</summary>
        public static bool UserIdIsContains(Int64 id)
        {
            return FindCount(_.id, id) > 0;
        }

        /// <summary>根据玩家名称集合 查询是否存在</summary>
        public static bool PlayNameIsContains(string playname)
        {
            return FindCount(_.player_name, playname) > 0;
        }
    }
}
