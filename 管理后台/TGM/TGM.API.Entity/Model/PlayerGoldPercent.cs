using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 元宝消耗明细
    /// </summary>
    public class PlayerGoldPercent : BaseEntity
    {
        public PlayerGoldPercent()
        {
            ListLogs = new List<SingleTypeLog>();
        }

        /// <summary>总消耗元宝</summary>
        public Int64 total_gold { get; set; }

        /// <summary>开始时间</summary>
        public String start_time { get; set; }

        /// <summary>日志集合</summary>
        public List<SingleTypeLog> ListLogs { get; set; }
    }
}
