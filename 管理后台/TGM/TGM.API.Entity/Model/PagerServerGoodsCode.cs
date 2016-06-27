using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡激活码分页
    /// </summary>
    public class PagerServerGoodsCode : BaseEntity
    {
        /// <summary>分页信息</summary>
        public PagerInfo Pager { get; set; }

        /// <summary>激活码数据集合</summary>
        public List<ServerGoodsCode> GoodsCode { get; set; }
    }
}
