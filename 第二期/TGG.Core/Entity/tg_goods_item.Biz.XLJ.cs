using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 货物表逻辑类
    /// </summary>
    public partial class tg_goods_item
    {
        /// <summary>
        /// 
        /// </summary>
        public static EntityList<tg_goods_item> GetFindByGoodsIds(Int64 userid, int tingid, string ids)
        {
            var exp = new WhereExpression();
            if (userid > 0) exp &= _.user_id == userid;
            if (tingid > 0) exp &= _.ting_id == tingid;
            if (!string.IsNullOrEmpty(ids)) exp &= _.goods_id.In(ids);
            return FindAll(exp, null, null, 0, 0);
        }
    }
}
