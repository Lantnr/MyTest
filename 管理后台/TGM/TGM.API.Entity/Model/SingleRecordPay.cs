using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class SingleRecordPay : BaseEntity
    {
        /// <summary>平台名称</summary>
        public String platform { get; set; }

        /// <summary>服务器名称</summary>
        public String server { get; set; }

        /// <summary>玩家名称</summary>
        public String playername { get; set; }

        /// <summary> 充值金额</summary>
        public double pay { get; set; }

        /// <summary>充值元宝</summary>
        public int gold { get; set; }

        /// <summary>订单号</summary>
        public String order { get; set; }

        /// <summary>充值时间</summary>
        public string paytime { get; set; }
        //public DateTime paytime { get; set; }
    }
}
