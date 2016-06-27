using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_arena_ranking
    {
        /// <summary>根据排名集合查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<view_arena_ranking> GetFindByRanking(List<int> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            var where = string.Format(" ranking in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>获取竞技场排名信息</summary>
        public static List<view_arena_ranking> GetEntityList(int number)
        {
            return FindAll(null, " ranking asc ", string.Format(" top {0} *", number), 0, 0);
        }
    }
}
