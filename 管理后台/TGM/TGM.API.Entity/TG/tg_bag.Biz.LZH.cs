using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_bag
    {
        /// <summary>根据玩家id查询玩家背包信息</summary>
        public static List<tg_bag> QueryBagByUserId(Int64 userId)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userId });
        }

        /// <summary>根据ids集合查询合战武将信息</summary>
        public static List<tg_bag> GetBagByIds(List<Int64> bid)
        {
            var ids = string.Join(",", bid);
            return FindAll(string.Format("id in({0})", ids), null, null, 0, 0);
        }
    }
}
