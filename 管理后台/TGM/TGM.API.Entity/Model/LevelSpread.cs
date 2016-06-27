using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 等级分布信息
    /// </summary>
    public class LevelSpread : BaseEntity
    {
        /// <summary>编号</summary>
        public Int32 id { get; set; }

        /// <summary>第一阶段等级人数 1~20级</summary>
        public Int32 stage1_count { get; set; }

        /// <summary>第一阶段所占百分比</summary>
        public double percent1 { get; set; }

        /// <summary>第二阶段等级人数 21~30级</summary>
        public Int32 stage2_count { get; set; }

        /// <summary>第二阶所占百分比</summary>
        public double percent2 { get; set; }

        /// <summary>第三阶段等级人数 31~35级</summary>
        public Int32 stage3_count { get; set; }

        /// <summary>第三阶所占百分比</summary>
        public double percent3 { get; set; }

        /// <summary>第四阶段等级人数 36~40级</summary>
        public Int32 stage4_count { get; set; }

        /// <summary>第四阶所占百分比</summary>
        public double percent4 { get; set; }

        /// <summary>第五阶段等级人数 41~45级</summary>
        public Int32 stage5_count { get; set; }

        /// <summary>第五阶所占百分比</summary>
        public double percent5 { get; set; }

        /// <summary>第六阶段等级人数 46~50级</summary>
        public Int32 stage6_count { get; set; }

        /// <summary>第六阶所占百分比</summary>
        public double percent6 { get; set; }

        /// <summary>第七阶段等级人数 51~55级</summary>
        public Int32 stage7_count { get; set; }

        /// <summary>第七阶所占百分比</summary>
        public double percent7 { get; set; }

        /// <summary>第八阶段等级人数 56~60级</summary>
        public Int32 stage8_count { get; set; }

        /// <summary>第八阶所占百分比</summary>
        public double percent8 { get; set; }

        /// <summary>总人数</summary>
        public Int32 total_count { get; set; }

        /// <summary>日期</summary>
        public String createtime { get; set; }
    }
}
