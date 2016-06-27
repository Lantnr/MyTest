using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class tg_role_fight_skill
    {
        /// <summary>根据武将主键id查询武将战斗技能信息</summary>
        public static List<tg_role_fight_skill> GetListByRid(Int64 rid)
        {
            return FindAll(string.Format("rid={0} and skill_level>=0", rid), null, null, 0, 0);
        }
    }
}
