using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_prison_messages
    {
        /// <summary>
        /// 根据时间查询玩家今日留言次数
        /// </summary>
        public static int GetUserMessageCount(double date, Int64 userid)
        {
            return FindCount(string.Format("writetime>={0} and user_id={1}", date, userid), null, "*", 0, 0);
        }

        public static List<tg_prison_messages> GetUserMessages(int index, int count)
        {
            return FindAll(null, "id desc", "*", index * count, count);
        }
    }
}
