using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家扩展类  
    /// </summary>
    public partial class tg_user_extend
    {
        /// <summary>更新用户拓展实体</summary>
        public static bool GetUpdate(tg_user_extend model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch { return false; }
        }
    }
}
