using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_role
    {
        /// <summary>根据用户id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<tg_role> GetFindAllByUserId(Int64 user_id)
        {
            return FindAll(_.user_id, user_id);
        }

        /// <summary>根据武将id 查询武将信息</summary>
        public static tg_role GetRoreByUserid(Int64 userid, int roleid)
        {
            return Find(new string[] { _.user_id, _.role_id }, new object[] { userid, roleid });
        }

        /// <summary> 获取所有主将信息 </summary>
        public static List<tg_role> GetMainRoles()
        {
            return FindAll(new string[] { _.role_state }, new object[] { (int)RoleStateType.PROTAGONIST });
        }

        /// <summary> 根据id集合查询武将集合信息</summary>
        /// <param name="ids">id集合</param>
        public static List<tg_role> GetFindAllByIds(List<Int64> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            return FindAll(string.Format("id in ({0}) ", _ids), null, null, 0, 0);
        }

        /// <summary> 修改武将信息 </summary>
        public static bool RoleUpdate(List<tg_role> list)
        {
            var roles = new EntityList<tg_role>();
            roles.AddRange(list);
            return roles.Save() > 0;
        }
    }
}
