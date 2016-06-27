using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家实体操作类
    /// </summary>
    public partial class tg_user
    {
        /// <summary>根据玩家账号查询玩家信息</summary>
        public static tg_user GetEntityByCode(string code)
        {
            return Find(new String[] { _.user_code }, new Object[] { code });
        }

        /// <summary>根据玩家名称查询玩家信息</summary>
        public static tg_user GetEntityByName(string name)
        {
            return Find(new String[] { _.player_name }, new Object[] { name });
        }
    }
}
