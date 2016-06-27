using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseTaskVocationRd : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTaskVocationRd CloneEntity()
        {
            return Clone() as BaseTaskVocationRd;
        }

        #endregion

        /// <summary> id </summary>
        public int id { get; set; }

        /// <summary> 类型 </summary>
        public int type { get; set; }

        /// <summary> 任务步骤(任务单步类型和相关id) </summary>
        public string stepCondition { get; set; }

        /// <summary> 初始任务步骤 </summary>
        public string stepInit { get; set; }
    }
}
