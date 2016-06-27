using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    ///  元宝消耗类型日志
    /// </summary>
    public class SingleTypeLog : BaseEntity
    {
        /// <summary>类别</summary>
        public String name { get; set; }

        /// <summary>消耗元宝</summary>
        public Int64 gold { get; set; }

        /// <summary>所占百分比</summary>
        public Double percent { get; set; }
    }
}
