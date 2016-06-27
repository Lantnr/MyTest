using System;
namespace TGG.Core.Base
{
    /// <summary>
    /// 主线任务基表
    /// </summary>
    //[Serializable]
    public class BaseTaskMain : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTaskMain CloneEntity()
        {
            return Clone() as BaseTaskMain;
        }

        #endregion

        #region

        /// <summary>主线任务id </summary>
        public int id { get; set; }

        /// <summary>接取等级</summary>
        public int openLevel { get; set; }

        /// <summary>接取声望</summary>
        public int reputation { get; set; }

        /// <summary>接取 Npc id</summary>
        public int acceptId { get; set; }

        /// <summary>完成Npc id</summary>
        public int finishedId { get; set; }

        /// <summary>任务步骤(任务单步类型和相关id)</summary>
        public string stepCondition { get; set; }

        /// <summary>奖励</summary>
        public string reward { get; set; }

        #endregion
    }
}
