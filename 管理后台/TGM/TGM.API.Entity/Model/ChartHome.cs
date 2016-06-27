using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 首页图表数据类
    /// </summary>
    public class ChartHome : BaseEntity
    {
        public ChartHome()
        {
            server = new List<int>();
            pay = new List<int>();
        }

        /// <summary>服务器数据</summary>
        public List<int> server { get; set; }

        /// <summary>充值数据</summary>
        public List<int> pay { get; set; }
    }
}
