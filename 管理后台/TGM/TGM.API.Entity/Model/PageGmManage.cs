using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// Gm记录分页类
    /// </summary>
    public class PageGmManage : BaseEntity
    {
        public PagerInfo Pager { get; set; }

        /// <summary>Gm记录集合</summary>
        public List<GmManage> Gms { get; set; }
    }
}
