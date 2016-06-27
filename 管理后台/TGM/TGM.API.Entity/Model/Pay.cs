using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 游戏支付记录
    /// </summary>
    public class Pay : BaseEntity
    {
        #region 属性
        /// <summary></summary>
        public Int64 id { get; set; }

        /// <summary>平台</summary>
        public Int32 sid { get; set; }

        /// <summary>玩家编号</summary>
        public Int64 player_id { get; set; }

        /// <summary>玩家账号</summary>
        public String user_code { get; set; }

        /// <summary>玩家名称</summary>
        public String player_name { get; set; }

        /// <summary>订单号</summary>
        public String order_id { get; set; }

        /// <summary>渠道</summary>
        public String channel { get; set; }

        /// <summary>充值类型(0:RMB,1:(1:1),2(1:10)...)</summary>
        public Int32 pay_type { get; set; }

        /// <summary>金额</summary>
        public Double amount { get; set; }

        /// <summary>支付状态</summary>
        public Int32 pay_state { get; set; }

        /// <summary>时间</summary>
        public Int64 createtime { get; set; }

        #endregion
    }
}
