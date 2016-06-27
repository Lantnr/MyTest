using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_role_fight_skill
    {
        /// <summary> 根据id集合查询技能集合信息</summary>
        /// <param name="ids">id集合</param>
        public static List<tg_role_fight_skill> GetFindAllByIds(List<Int64> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            return FindAll(string.Format("id in ({0}) ", _ids), null, null, 0, 0);
        }
    }
}
