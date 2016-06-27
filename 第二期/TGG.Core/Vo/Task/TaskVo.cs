using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Task
{
    /// <summary>
    /// 主线任务
    /// </summary>
    public class TaskVo : BaseVo
    {
        /// <summary>主键</summary>
        public double id { get; set; }

        /// <summary>基础id </summary>
        public int baseId { get; set; }

        /// <summary>任务状态 TaskStateType </summary>
        public int state { get; set; }

        /// <summary>
        /// 当前步骤多个条件完成情况，
        /// 格式：类型_数值|类型_相关id_数值|类型_物品类型_相关id_数值，
        /// 如：7_value|4_100001_value|6_6_100001_value
        /// </summary>
        public string stepData { get; set; }

    }
}
