using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JScript;

namespace TGG.Core.Entity
{
    public partial class tg_goods_item
    {
        /// <summary>根据用户Id获取实体</summary>
        public static IEnumerable<tg_goods_item> GetEntityList(Int64 userid, int tingid)
        {
            return FindAll(string.Format("user_id={0} and ting_id={1}", userid, tingid), null, null, 0, 0);
        }

        /// <summary>修改町货物实体</summary>
        public static void GetEntityUpdate(int tingid, int goodsid, int number, Int64 userid)
        {
            var entity = Find(new string[] { _.ting_id, _.goods_id,_.user_id }, new object[] { tingid, goodsid,userid });
            if (entity == null) return;
            entity.number = number;
            entity.Update();
        }
    }
}
