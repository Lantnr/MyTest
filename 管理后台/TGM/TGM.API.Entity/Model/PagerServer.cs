using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 服务器分页类
    /// </summary>
    public class PagerServer : BaseEntity
    {
        public PagerInfo Pager { get; set; }
        /// <summary>服务器集合</summary>
        public List<Server> Servers { get; set; }
    }
}
