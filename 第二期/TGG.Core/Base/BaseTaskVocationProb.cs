using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary> 职业任务概率基表基础 </summary>
    //[Serializable]
    public class BaseTaskVocationProb : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTaskVocationProb CloneEntity()
        {
            return Clone() as BaseTaskVocationProb;
        }

        #endregion

        /// <summary> 类型 </summary>
        public int type { get; set; }

        /// <summary>概率 </summary>
        public int prob { get; set; }

        /// <summary> 能力值 </summary>
        public int value { get; set; }

        /// <summary> 技能或属性 </summary>
        public string skillOrAtt { get; set; }

        /// <summary>概率2 </summary>
        public int prob2 { get; set; }

        /// <summary> 能力值2 </summary>
        public int value2 { get; set; }

        /// <summary> 技能或属性2</summary>
        public string skillOrAtt2 { get; set; }
    }
}
