using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_daming_log
    {
        /// <summary>更新大名引导信息</summary>
        public static void UpdateDaMing(tg_daming_log data)
        {
            var _set = string.Format("user_finish={0},is_reward={1},is_finish={2}", data.user_finish, data.is_reward, data.is_finish);
            var _where = string.Format("id={0}", data.id);

            Update(_set, _where);
        }
    }
}

