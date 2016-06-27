using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
   public  partial class tg_daming_log
    {
        /// <summary>根据用户Id获取用户大名令</summary>
       public static List<tg_daming_log> GetEntityByUserId(Int64 user_id = 0)
        {
            var entitylist = FindAll(new String[] { _.user_id }, new Object[] { user_id });
            return entitylist;
        }


       public static int GetListInsert(IEnumerable<tg_daming_log> logs)
       {
           var list = new EntityList<tg_daming_log>();
           list.AddRange(logs);
           return list.Insert();
       }

    }
}
