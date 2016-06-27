using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家跑商商圈表部分业务辑类
    /// </summary>
    public partial class tg_user_area
    {
        /// <summary>根据用户Id获取商圈集合</summary>
        public static EntityList<tg_user_area> GetEntityByUserId(Int64 user_id = 0)
        {
            var entity = FindAll(new String[] { _.user_id }, new Object[] { user_id });
            return entity;
        }
    }
}
