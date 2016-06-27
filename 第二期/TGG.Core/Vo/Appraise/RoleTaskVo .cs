using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Appraise
{
    public class RoleTaskVo : BaseVo
    {
        /// <summary> 主键id </summary>
        public double id { get; set; }

        /// <summary>/ * 任务基础id  </summary>
        public int baseId { get; set; }

        /// <summary> * 家臣主键id</summary>
        public double roleId { get; set; }

        /// <summary> * 任务开始时间</summary>
        public double beginTime { get; set; }

        /// <summary> * 任务结束时间</summary>
        public double time { get; set; }

        /// <summary>任务状态 TaskStateType </summary>
        public int state { get; set; }

    }
}
