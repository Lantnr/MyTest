using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class TotalRecordPay : BaseEntity
    {
        /// <summary>平台名称</summary>
        public String platform { get; set; }

        /// <summary>服务器名称</summary>
        public String server { get; set; }

        /// <summary>玩家名称</summary>
        public String playername { get; set; }

        /// <summary> 充值金额</summary>
        public int paytotal { get; set; }

        /// <summary>充值次数</summary>
        public int count { get; set; }

        /// <summary>最近一次充值时间</summary>
        public String paytime { get; set; }
        //public DateTime paytime { get; set; }

        /// <summary>最近一次登陆时间</summary>
        public String logintime { get; set; }
    }
}
