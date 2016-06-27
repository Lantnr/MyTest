using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 服务器元宝充值消耗分页
    /// </summary>
    public class PagerServerGold : BaseEntity
    {
        /// <summary>分页信息</summary>
        public PagerInfo Pager { get; set; }

        /// <summary>服务器元宝数据集合</summary>
        public List<ServerGoldConsume> GoldConsumes { get; set; }
    }
}
