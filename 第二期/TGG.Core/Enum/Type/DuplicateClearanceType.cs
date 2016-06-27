namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 通关类型枚举
    /// </summary>
    public enum DuplicateClearanceType
    {
        /// <summary>
        /// 还未闯关
        /// </summary>
        CLEARANCE_UNBEGIN = 0,

        /// <summary>
        /// 通关成功
        /// </summary>
        CLEARANCE_SUCCESS = 1,

        /// <summary>
        /// 通关失败
        /// </summary>
        CLEARANCE_FAIL = 2,

        /// <summary>
        /// 闯关中
        /// </summary>
        CLEARANCE_FIGHTING = 3,
    }
}
