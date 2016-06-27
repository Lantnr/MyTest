using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 任务项
    /// </summary>
    [Serializable]
    public class TaskItem
    {
        public TaskItem()
        {
            Result =FightResultType.DEFAULT;
        }

        /// <summary>任务主键id</summary>
        public int Id { get; set; }

        /// <summary>任务目标对象</summary>
        public int Target { get; set; }

        /// <summary>任务步骤类型</summary>
        public TaskStepType Type { get; set; }

        /// <summary>任务步骤类型</summary>
        public TaskType TasktType { get; set; }

        /// <summary>任务结果</summary>
        public FightResultType Result { get; set; }

     
    }

}
