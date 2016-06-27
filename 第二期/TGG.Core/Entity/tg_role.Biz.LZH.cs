using System;
using System.Collections.Generic;
using TGG.Core.Enum.Type;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_role
    {
        /// <summary>根据武将主键id获取实体</summary>
        public static tg_role GetEntityById(Int64 id)
        {
            return Find(new String[] { _.id }, new Object[] { id });
        }

        /// <summary>获取所有主角武将</summary>
        public static EntityList<tg_role> GetAllLeadRole()
        {
            return FindAll(new String[] { _.role_state }, new Object[] { (int)RoleStateType.PROTAGONIST });
        }

        /// <summary>根据用户id 武将状态获取主角武将信息</summary>
        public static tg_role GetEntityByUserId(Int64 userid)
        {
            return Find(new String[] { _.user_id, _.role_state }, new Object[] { userid, (int)RoleStateType.PROTAGONIST });
        }

        /// <summary>根据武将实体更新武将信息</summary>
        public static bool UpdateByRole(tg_role model)
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

        /// <summary>根据武将ids 查询武将信息集合</summary>
        public static List<tg_role> GetRoleByIds(List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("id in({0})", ids), null, null, 0, 0);
        }
    }
}
