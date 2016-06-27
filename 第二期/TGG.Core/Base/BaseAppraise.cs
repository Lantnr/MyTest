using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 评定基表
    /// </summary>
    ///[Serializable]
    public class BaseAppraise : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseAppraise CloneEntity()
        {
            return Clone() as BaseAppraise;
        }

        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }

        /// <summary>体力消耗 </summary>
        public int power { get; set; }

        /// <summary>默认时间 </summary>
        public int time { get; set; }

        /// <summary>减少消耗公式 </summary>
        public string expression { get; set; }

        /// <summary>相关技能 </summary>
        public string skill { get; set; }

        /// <summary>一阶奖励条件 </summary>
        public string rewardACondition { get; set; }

        /// <summary>一阶奖励 </summary>
        public string rewardA { get; set; }

        /// <summary> 二阶奖励条件</summary>
        public string rewardBCondition { get; set; }

        /// <summary> 二阶奖励</summary>
        public string rewardB { get; set; }

        /// <summary> 三阶奖励条件</summary>
        public string rewardCCondition { get; set; }

        /// <summary> 三阶奖励</summary>
        public string rewardC { get; set; }
    }
}
