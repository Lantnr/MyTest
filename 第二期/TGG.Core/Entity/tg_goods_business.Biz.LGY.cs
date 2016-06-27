using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_goods_business
    {
        /// <summary>根据马车集合获取跑商实体集合</summary>
        public static List<tg_goods_business> GetEntity(IEnumerable<tg_car> list)
        {
            var ids = string.Join(",", list.Select(m => m.id).ToArray());
            return FindAll(string.Format("cid in({0})", ids), null, null, 0, 0);
        }

        public static tg_goods_business GetByGoodsidAndUserid(Int64 userid, Int64 goodid, Int64 carid)
        {

            return Find(new string[] { _.goods_id, _.user_id,_.cid }, new object[] { goodid, userid ,carid});
        }
    }
}
