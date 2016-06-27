using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_role_life_skill
    {
        /// <summary>根据武将主键id查询生活技能信息</summary>
        public static tg_role_life_skill GetLifeByRid(Int64 rid)
        {
            return Find(new String[] { _.rid }, new Object[] { rid });
        }

    }
}
