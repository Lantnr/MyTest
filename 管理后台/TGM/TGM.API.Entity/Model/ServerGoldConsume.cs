using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>服务器元宝数据列表</summary>
    public class ServerGoldConsume : BaseEntity
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>服务器sid</summary>
        public Int32 sid { get; set; }

        /// <summary>充值金额</summary>
        public Int32 recharge_count { get; set; }

        /// <summary>充值人数</summary>
        public Int32 recharge_people { get; set; }

        /// <summary>元宝消耗数量</summary>
        public Int32 consume { get; set; }

        /// <summary>消费充值比例</summary>
        public Double percent { get; set; }

        /// <summary>日期</summary>
        public String createtime { get; set; }

    }
}
