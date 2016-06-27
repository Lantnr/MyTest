using System;

namespace TGG.Core.Vo.Role
{
    /// <summary>
    /// 武将雇佣Vo
    /// </summary>
    public class HomeHireVo : BaseVo
    {
        /// <summary> id </summary>
        public Int64 id { get; set; }

        /// <summary> 基础id  </summary>
        public Int64 baseId { get; set; }

        /// <summary> 是否启用保镖   0:不启用，1:启用 </summary>
        public int state { get; set; }

        /// <summary> 保镖结束时间 </summary>
        public Int64 time { get; set; }

    }
}
