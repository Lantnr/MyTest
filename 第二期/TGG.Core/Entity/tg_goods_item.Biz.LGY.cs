using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_goods_item
    {
        /// <summary>根据用户Id获取实体</summary>
        public static List<tg_goods_item> GetEntityByTingIdAndUserId(int tingid, Int64 userid)
        {
            return FindAll(new String[] { _.ting_id, _.user_id }, new Object[] { tingid, userid });
        }
    }
}
