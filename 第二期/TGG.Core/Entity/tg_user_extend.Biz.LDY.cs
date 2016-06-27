using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_extend
    {
        /// <summary>根据userid 获取实体</summary>
        public static int GetTaskReflashUpdate()
        {
            return Update(string.Format("task_vocation_refresh={0}", 0), null);
        }

        /// <summary>
        /// 家臣任务刷新次数重置
        /// </summary>
        public static int GetReflashReset()
        {
            return Update(string.Format("[task_role_refresh]={0}", 0), null);
        }

    }
}
