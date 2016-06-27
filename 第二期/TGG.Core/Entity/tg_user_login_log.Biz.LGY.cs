using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_login_log
    {
        /// <summary>根据用户id集合 查询用户登录信息集合</summary>
        public static List<tg_user_login_log> GetLoginLogByIds(string _ids)
        {           
            return FindAll(string.Format("user_id in ({0})", _ids), null, null, 0, 0);         
        }

        /// <summary>根据用户id 查询用户登录信息</summary>
        public static tg_user_login_log GetLoginLogById(Int64 userid)
        {         
            return Find(_.user_id,userid);
        }
    }
}
