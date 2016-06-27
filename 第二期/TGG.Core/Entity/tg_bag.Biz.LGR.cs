using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_bag
    {
        /// <summary>根据id获取道具</summary>
        public static tg_bag GetFindById(Int64 id)
        {
            return Find(new String[] { _.id }, new Object[] { id });
        }

        /// <summary>根据用户id 与基表id集合查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByUserIdAndBaseIds(Int64 userid, List<int> ids)
        {
            string _ids = string.Join(",", ids.ToArray());
            string where = string.Format(" user_id={0} and base_id in (0,{1})", userid, _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>根据主键id集合查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByIds(List<int> ids)
        {
            string _ids = string.Join(",", ids.ToArray());
            string where = string.Format(" id in (0,{0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>根据用户id 与基表id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByUserIdAndBaseId(Int64 userid, int baseid)
        {
            return FindAll(new String[] { _.user_id, _.base_id }, new Object[] { userid, baseid });
        }

        /// <summary>根据用户id 查找道具数量</summary>
        public static int FindCount(Int64 userid)
        {
            return FindCount(new String[] { _.user_id }, new Object[] { userid });
        }

        public static int UpdateCount(tg_bag model)
        {
            var setClause = string.Format("count={0}", model.count);
            var whereClause = string.Format(" id={0}", model.id);
            return Update(setClause, whereClause);
        }
    }
}
