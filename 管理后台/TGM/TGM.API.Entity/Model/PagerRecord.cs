using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 统计分页
    /// </summary>
    public class PagerRecord : BaseEntity
    {
        public PagerRecord()
        {
            RecordServers = new List<RecordServer>();
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo Pager { get; set; }

        /// <summary>
        /// 游戏服务器统计记录
        /// </summary>
        public List<RecordServer> RecordServers { get; set; }

        /// <summary>
        /// 总充值
        /// </summary>
        public Int32 total { get; set; }

        /// <summary>
        /// 本月充值
        /// </summary>
        public Int32 month_total { get; set; }
    }
}
