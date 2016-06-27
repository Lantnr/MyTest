using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_war_role
    {
        /// <summary>根据ids集合查询合战武将信息</summary>
        public static List<tg_war_role> GetWarRolesByIds(List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("rid in({0})", ids), null, null, 0, 0);
        }
    }
}
