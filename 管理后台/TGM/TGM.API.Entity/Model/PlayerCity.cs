using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class PlayerCity : BaseEntity
    {
        /// <summary>据点编号</summary>
        public Int64 id { get; set; }

        /// <summary>基表编号</summary>
        public Int32 baseid { get; set; }

        /// <summary>据点名称</summary>
        public String name { get; set; }

        /// <summary>据点规模</summary>
        public String size { get; set; }

        /// <summary>军粮</summary>
        public Int32 res_foods { get; set; }

        /// <summary>军资金</summary>
        public Double res_funds { get; set; }

        /// <summary>士兵(足轻)</summary>
        public Int32 res_soldier { get; set; }

        /// <summary>铁炮</summary>
        public Int32 res_gun { get; set; }

        /// <summary>马匹</summary>
        public Int32 res_horse { get; set; }

        /// <summary>薙刀</summary>
        public Int32 res_razor { get; set; }

        /// <summary>苦无</summary>
        public Int32 res_kuwu { get; set; }

        /// <summary>士气值</summary>
        public Int32 res_morale { get; set; }

        /// <summary>治安</summary>
        public Double peace { get; set; }

        /// <summary>耐久度</summary>
        public Double strong { get; set; }

        /// <summary>繁荣度</summary>
        public Double boom { get; set; }
    }
}
