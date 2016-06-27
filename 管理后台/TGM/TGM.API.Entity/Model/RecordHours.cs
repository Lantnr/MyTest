using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 小时记录
    /// </summary>
    public class RecordHours
    {
        /// <summary>数据集合</summary>
        public List<Int32> data { get; set; }

        /// <summary>最高</summary>
        public Int32 best { get; set; }

        /// <summary>平均</summary>
        public Double average { get; set; }

        /// <summary>总数</summary>
        public Double total { get; set; }
    }
}
