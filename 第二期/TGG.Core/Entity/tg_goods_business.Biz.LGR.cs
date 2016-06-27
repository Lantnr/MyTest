using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_goods_business 业务逻辑
    /// </summary>
    public partial class tg_goods_business
    {
        /// <summary>根据马车Id获取实体集合</summary>
        public static List<tg_goods_business> GetListEntityByCid(Int64 cid = 0)
        {
            return FindAll(new String[] { _.cid }, new Object[] { cid });
        }
    }
}
