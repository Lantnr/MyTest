using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_prison
    {
        /// <summary>
        /// 根据用户id查询
        /// </summary>
        public static tg_prison GetPrisonByUserId(Int64 userid)
        {
            return Find(new String[] { _.user_id }, new Object[] { userid });
        }

        /// <summary>
        /// 根据用户id查询
        /// </summary>
        public static bool GetCountByUserId(Int64 userid)
        {
            return FindCount(new String[] { _.user_id }, new Object[] { userid }) > 0;
        }

        /// <summary>
        /// 查询监狱里所有的用户
        /// </summary>
        public static List<tg_prison> GetPrisonByUserId(IEnumerable<long> ids)
        {
            string _ids = string.Join(",", ids.ToArray());
            string where = string.Format(" user_id in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }
    }
}
