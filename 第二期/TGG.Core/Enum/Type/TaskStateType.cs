namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStateType
    {
        /// <summary>
        /// 未开放
        /// </summary>
        TYPE_UNOPEN = -1,


        /// <summary>
        /// 未接受
        /// </summary>
        TYPE_UNRECEIVED = 0,

        /// <summary>
        /// 未完成
        /// </summary>
        TYPE_UNFINISHED = 1,

        /// <summary>
        /// 完成未领取奖励
        /// </summary>
        TYPE_REWARD = 2,

        /// <summary>
        /// 完成并结束
        /// </summary>
        TYPE_FINISHED = 3,
    }
}
