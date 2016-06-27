using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_user_extend
    {
        /// <summary>获取所有有雇佣的玩家扩展信息</summary>
        public static List<tg_user_extend> GetAllUserHire()
        {
            return FindAll(String.Format("hire_time>{0}", 0), null, null, 0, 0);
        }
    }
}
