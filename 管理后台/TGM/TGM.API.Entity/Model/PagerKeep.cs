using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 留存分页类
    /// </summary>
    public class PagerKeep : BaseEntity
    {
        public PagerKeep()
        {
            Keeps = new List<RecordKeep>();
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo Pager { get; set; }

        /// <summary>
        /// 游戏服务器留存统计记录
        /// </summary>
        public List<RecordKeep> Keeps { get; set; }

    }
}
