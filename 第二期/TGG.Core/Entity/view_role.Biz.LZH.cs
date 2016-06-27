using System;
using System.Collections.Generic;
using System.Linq;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// view_role  视图查询业务操作类
    /// </summary>
    public partial class view_role
    {
        /// <summary>根据id集合获取武将数据</summary>
        public static List<RoleItem> GetRoleById(List<Int64> ids)
        {
            var roles = new List<RoleItem>();
            var listrole = GetEntity(ids);
            foreach (var id in ids)
            {
                var person = listrole.Where(m => m.id == id).ToList();
                if (person.Count <= 0) continue;
                var role = new RoleItem
                {
                    Kind = SetRole(person),
                    LifeSkill = SetRoleLifeSkill(person),
                    FightSkill = SetRoleFightSkill(person)
                };
                roles.Add(role);
            }
            return roles;
        }


        /// <summary>根据武将id集合查询武将视图信息</summary>
        public static List<view_role> GetEntity(List<Int64> lists)
        {
            var ids = string.Join(",", lists);
            return FindAll(string.Format("id in({0})", ids), null, null, 0, 0);
        }
    }
}
