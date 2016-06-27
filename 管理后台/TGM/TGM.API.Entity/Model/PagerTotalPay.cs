using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class PagerTotalPay : BaseEntity
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo Pager { get; set; }

        /// <summary>
        /// 充值记录
        /// </summary>
        public List<TotalRecordPay> Recordpay { get; set; }
    }
}
