using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class PagerPay : BaseEntity
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo Pager { get; set; }

        /// <summary>
        /// 统计记录
        /// </summary>
        public List<SingleRecordPay> Recordpay { get; set; }
    }
}
