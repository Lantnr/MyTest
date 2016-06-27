namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 任务步骤类型枚举
    /// </summary>
    public enum TaskStepType
    {
        #region 任务类型
        /// <summary> 与npc交谈 </summary>
        TYPE_DIALOG = 1,

        /// <summary> 通关副本 </summary>
        TYPE_GATES = 2,

        /// <summary> 派送物品 </summary>
        TYPE_SEND = 3,

        /// <summary> 击杀怪物</summary>
        TYPE_KILL = 4,

        /// <summary> 跑商总值</summary>
        TYPE_BUSINESS = 5,

        /// <summary> 战斗力达到某值 </summary>
        TYPE_FIGHT_VALUE = 6,

        /// <summary>爬塔次数 </summary>
        TYPE_TOWER_VALUE = 7,

        /// <summary> npc战斗（有次数和胜败两个条件）</summary>
        NPC_FIGHT_TIMES = 8,

        /// <summary>任意战斗计数 </summary>
        FREE_FIGHT_TIMES = 9,

        /// <summary>点击查看信息 </summary>
        CLICK = 10,

        /// <summary>学习技能 </summary>
        STUDY_SKILL = 11,

        /// <summary>修行 </summary>
        TRAIN = 12,

        /// <summary>监督筑城 </summary>
        BUILD = 101,

        /// <summary>对话和送信 </summary>
        SEND = 401,

        /// <summary>有一个npc可以快速完成 </summary>
        FASTNPC = 402,

        /// <summary>有一个npc可以快速完成 </summary>
        NPCCOUNTS = 301,

        /// <summary> 护送 </summary>
        ESCORT = 201,

        /// <summary> 连续战斗</summary>
        FIGHTING_CONTINUOUS = 202,

        /// <summary>守护 </summary>
        GUARD = 203,

        /// <summary>刺杀</summary>
        ASSASSINATION = 302,

        /// <summary> 搜寻物品 </summary>
        SEARCH_GOODS = 303,

        /// <summary>布谣</summary>
        RUMOR = 304,

        /// <summary>纵火</summary>
        FIRE = 305,

        /// <summary>破坏</summary>
        BREAK = 306,

        /// <summary>售酒</summary>
        SEll_WINE = 307,

        /// <summary>戒严布谣</summary>
        ARREST_RUMOR = 103,

        /// <summary>戒严纵火</summary>
        ARREST_FIRE = 104,

        /// <summary>戒严破坏</summary>
        ARREST_BREAK =105 ,

        /// <summary>戒严售酒</summary>
        ARREST_SEll_WINE = 106,

        /// <summary> 买卖类 </summary>
        BUSINESS = 403,

        /// <summary>挣钱 </summary>
        RAISE_COIN = 404,

        /// <summary>站岗 </summary>
        STAND_GUARD = 102,
        #endregion
    }
}
