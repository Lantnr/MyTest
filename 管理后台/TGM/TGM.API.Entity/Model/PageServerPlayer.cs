using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 服务器玩家分页信息
    /// </summary>
    public class PageServerPlayer : BaseEntity
    {
        public PagerInfo Pager { get; set; }

        /// <summary>玩家信息集合</summary>
        public List<ServerPlayer> Players { get; set; }
    }
}
