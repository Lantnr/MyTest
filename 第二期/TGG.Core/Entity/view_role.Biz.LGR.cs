using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 武将视图
    /// </summary>
    public partial class view_role
    {
        /// <summary> 根据id集合查询武将集合信息</summary>
        /// <param name="ids">id集合</param>
        public static List<RoleItem> GetFindAllByIds(List<Int64> ids)
        {
            var result = new List<RoleItem>();
            var temp = FindViewRoleByIds(ids).ToList();
            var roles = temp.GroupBy(m => m.role_id);
            foreach (var item in roles)
            {
                var list = temp.Where(m => m.role_id == item.Key);
                result.AddRange(list.Select(entity => new RoleItem
                {
                    Kind = SetRole(list),
                    LifeSkill = SetRoleLifeSkill(list),
                    FightSkill = SetRoleFightSkill(list)
                }));
            }
            return result;
        }


        /// <summary>根据id集合查询武将集合信息</summary>
        public static EntityList<view_role> FindViewRoleByIds(List<Int64> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            return FindAll(string.Format("id in ({0}) ", _ids), null, null, 0, 0);
        }
    }
}
