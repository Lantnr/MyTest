using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 日志页面类
    /// </summary>
    public class PageLog : BaseEntity
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo Pager { get; set; }

        /// <summary>
        /// 统计记录
        /// </summary>
        public List<PlayerLog> Logs { get; set; }
    }
}
