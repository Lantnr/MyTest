using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Business
{
    /// <summary>
    /// BusinessCarVo 马车vo
    /// </summary>
    public class BusinessCarVo : BaseVo
    {
        /// <summary>主键</summary>
        public double id { get; set; }

        /// <summary>基表 id </summary>
        public int baseId { get; set; }

        /// <summary>马车状态 CarStateType 0:停止，1:运行中</summary>
        public int state { get; set; }

        /// <summary>运行时到达目的地的时间戳 ms</summary>		
        public double time { get; set; }

        /// <summary>目的地城市基础id </summary>
        public int destinationId { get; set; }

        /// <summary>停靠地基础id  </summary>		
        public int stopId { get; set; }

        /// <summary>马车当前已开格子数量 </summary>
        public int volume { get; set; }

        /// <summary>武将 id </summary>
        public double generalId { get; set; }

        /// <summary>当前货物 vo</summary>
        public List<BusinessGoodsVo> goods { get; set; }
    }
}
