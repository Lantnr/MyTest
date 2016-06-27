using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 职业任务
    /// </summary>
    //[Serializable]
    public class BaseTaskVocation : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTaskVocation CloneEntity()
        {
            return Clone() as BaseTaskVocation;
        }

        #endregion

        /// <summary>基础id </summary>
        public int id { get; set; }

        /// <summary>职业</summary>
        public int vocation { get; set; }

        /// <summary>身份 </summary>
        public string identity { get; set; }

        /// <summary>任务步骤 </summary>
        public string stepCondition { get; set; }

        /// <summary>奖励 </summary>
        public string reward { get; set; }

        /// <summary>最大奖励 </summary>
        public string rewardMax { get; set; }

        /// <summary>最高奖励条件 </summary>
        public string rewardMaxCondition { get; set; }

        /// <summary>中间奖励 </summary>
        public string rewardMedium { get; set; }

        /// <summary>中间奖励条件 </summary>
        public string rewardMediumCondition { get; set; }

        /// <summary>类型 </summary>
        public int type { get; set; }

        /// <summary>任务步骤类型 </summary>
        public int stepType { get; set; }

        /// <summary>r任务步骤初始值 </summary>
        public string stepInit { get; set; }

        /// <summary>技能要求 </summary>
        public string skillCondition { get; set; }

        /// <summary>持续时间 </summary>
        public int time { get; set; }

        /// <summary>阵营 </summary>
        public int camp { get; set; }

        /// <summary>冷却时间 </summary>
        public int coolingTime { get; set; }

        /// <summary>身份值 </summary>
        public int identifyValue { get; set; }


        /// <summary>工作奖励 </summary>
        public string workReward { get; set; }

        /// <summary>工作最大奖励 </summary>
        public string workRewardMax { get; set; }

        /// <summary>工作最高奖励条件 </summary>
        public string workRewardMaxCon { get; set; }

        /// <summary>工作中间奖励 </summary>
        public string rewardMediumCon { get; set; }

        /// <summary>工作中间奖励条件 </summary>
        public string workRewardMedium { get; set; }

        /// <summary>任务限制时间 </summary>
        public int limitTime { get; set; }



    }
}
