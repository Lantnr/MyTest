using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>等级分页信息</summary>
    public class PagerServerLevel : BaseEntity
    {
        /// <summary>分页信息</summary>
        public PagerInfo Pager { get; set; }

        /// <summary>分页信息集合</summary>
        public List<LevelSpread> ServerLevels { get; set; }
    }
}
