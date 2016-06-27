using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_car
    {
        /// <summary>根据玩家id查询玩家马车数量</summary>
        public static int GetCarCount(Int64 userId)
        {
            return FindCount(new String[] { _.user_id }, new Object[] { userId });
        }
    }
}
