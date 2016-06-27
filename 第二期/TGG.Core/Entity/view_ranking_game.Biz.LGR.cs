using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_ranking_game
    {
        /// <summary> 获取实体集合 </summary>
        /// <param name="ranking">小于等于此参数的排名</param>
        /// <param name="userid">顺便要查找的用户id</param>
        public static List<view_ranking_game> GetEntityList(int ranking, Int64 userid)
        {
            return FindAll(string.Format("ranks<= {0} or id={1}", ranking, userid), null, null, 0, 0);
        }

        /// <summary> 发放周奖励实体集合 </summary>
        /// <param name="ranking">小于等于此参数的排名</param>
        public static List<view_ranking_game> GetRewardList(int ranking)
        {
            return FindAll(string.Format("ranks<= {0} and week_max_pass>0", ranking), null, null, 0, 0);
        }
    }
}
