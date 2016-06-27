using System;
using System.Collections.Generic;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_bag 业务逻辑类操作
    /// </summary>
    public partial class tg_bag
    {
        /// <summary> 根据装备主键id获取实体 </summary>
        public static tg_bag GetEntityByEquipId(Int64 id)
        {
            return Find(new String[] { _.id }, new Object[] { id });
        }

        /// <summary>根据装备主键id,删除装备信息</summary>
        public static bool DeleteEquips(List<Int64> equips)
        {
            try
            {
                var ids = string.Join(",", equips.ToArray());
                var where = string.Format("id in (0,{0})", ids);
                return Delete(where) > 0;
            }
            catch
            { return false; }
        }

        /// <summary>根据用户id，装备state，物品类型type 查询玩家所有已穿戴装备</summary>
        public static List<tg_bag> GetEquipByUserIdStateType(Int64 userid, int state, int type)
        {
            return FindAll(new String[] { _.user_id, _.state, _.type }, new Object[] { userid, state, type });
        }
    }
}
