using System;
using System.Collections.Generic;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_role_life_skill 业务逻辑
    /// </summary>
    public partial class tg_role_life_skill
    {
        /// <summary>
        /// 根据武将主键id获取实体
        /// </summary>
        public static tg_role_life_skill GetEntityByRid(Int64 rid)
        {
            var exp = new WhereExpression();
            if (rid > 0) exp &= _.rid == rid;
            return Find(exp);
        }

        /// <summary>根据武将rids集合查询武将战斗技能信息</summary>
        public static List<tg_role_life_skill > GetRoleLifeSkills(List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("rid in({0})", ids), null, null, 0, 0);
        }
    }
}
