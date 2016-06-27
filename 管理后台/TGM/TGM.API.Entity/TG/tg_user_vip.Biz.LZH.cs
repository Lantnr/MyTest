using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_vip
    {
        /// <summary>根据玩家id查询玩家VIP信息</summary>
        public static tg_user_vip GetByUserId(Int64 userId)
        {
            return Find(new String[] { _.user_id }, new Object[] { userId });
        }
    }
}
