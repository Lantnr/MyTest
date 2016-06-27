using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_area
    {
        /// <summary>根据玩家id查询玩家商圈信息</summary>
        public static List<tg_user_area> GetAreas(Int64 userId)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userId });
        }
    }
}
