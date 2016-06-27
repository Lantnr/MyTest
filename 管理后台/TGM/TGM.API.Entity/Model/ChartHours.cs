using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 小时图表
    /// </summary>
    public class ChartHours : BaseEntity
    {
        /// <summary>在线图表</summary>
        public RecordHours online { get; set; }
        /// <summary>登陆图表</summary>
        public RecordHours login { get; set; }

        /// <summary>注册图表</summary>
        public RecordHours register { get; set; }

        
    }
}
