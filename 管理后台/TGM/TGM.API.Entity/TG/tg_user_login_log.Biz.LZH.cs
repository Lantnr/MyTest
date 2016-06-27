using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_login_log
    {
        /// <summary>查询玩家登陆信息</summary>
        public static tg_user_login_log GetLoginLog(Int64 userId)
        {
            return Find(new String[] { _.user_id }, new Object[] { userId });
        }
    }
}
