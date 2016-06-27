using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_ranking_honor
    {
        /// <summary>获取功名排名信息</summary>
        public static List<view_ranking_honor> GetEntityList(int number)
        {
            return FindAll(null, null, string.Format("top {0} *",number), 0, 0);
        }

        /// <summary>获取玩家个人排名信息</summary>
        public static view_ranking_honor GetEntityByUserId(Int64 userid)
        {
            return Find(_.id, userid);
        }

        /// <summary>获取富豪排名信息</summary>
        public static List<view_ranking_honor> GetEntityAll()
        {
            return FindAll();
        }
    }
}
