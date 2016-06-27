using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_game 业务逻辑类
    /// </summary>
    public partial class tg_game
    {
        /// <summary>根据用户id 查询玩家游艺园数据</summary>
        public static tg_game GetByUserId(Int64 user_id)
        {
            return Find(new String[] { _.user_id }, new Object[] { user_id });
        }
    }
}
