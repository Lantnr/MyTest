using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class FamilyLogVo : BaseVo
    {

        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 家族id
        /// </summary>
        public int familyid { get; set; }

        /// <summary>
        /// 玩家编号
        /// </summary>
        public int userid { get; set; }

        /// <summary>
        /// 玩家名字
        /// </summary>
        public String userName { get; set; }

        /// <summary>
        /// 日志基表id
        /// </summary>
        public int logBaseId { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public int logType { get; set; }

        /// <summary>
        /// 事件时间
        /// </summary>
        public double time { get; set; }
    }
}
