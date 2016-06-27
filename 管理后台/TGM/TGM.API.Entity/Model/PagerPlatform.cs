using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 平台数据分页
    /// </summary>
    public class PagerPlatform
    {
        public PagerInfo Pager { get; set; }
        /// <summary>平台数据集合</summary>
        public List<Platform> Platforms { get; set; }

    }
}
