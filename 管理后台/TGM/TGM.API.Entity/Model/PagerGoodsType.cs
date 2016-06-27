using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡类型分页
    /// </summary>
    public class PagerGoodsType : BaseEntity
    {
        /// <summary>分页信息</summary>
        public PagerInfo Pager { get; set; }

        /// <summary>福利卡类型数据集合</summary>
        public List<GoodsType> GoodsType { get; set; }
    }
}
