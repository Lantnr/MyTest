using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_arena
    {
        /// <summary>根据用户id获取竞技场玩家信息</summary>
        public static tg_arena FindByUserId(Int64 userid)
        {
            return Find(new String[] { _.user_id }, new Object[] { userid });
        }

        /// <summary>获取竞技场排名信息</summary>
        public static List<tg_arena> GetEntityList(int number)
        {
            return FindAll(string.Format(" ranking<={0}", number), null, null, 0, 0);
        }

        /// <summary>更新竞技场所有玩家可挑战次数信息</summary>
        public static bool UpdateArenaCount(int count)
        {
            try
            {
                tg_arena.Update(new string[] { _.count, _.buy_count }, new object[] { count, 0 }, null, null);
                return true;
            }
            catch
            { return false; }
        }
    }
}
