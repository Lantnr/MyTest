namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 排名指令枚举
    /// </summary>
    public enum RankingsCommand
    {
        #region 武将排名

        /// <summary>
        /// 武将排名界面
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// oneself:MainstayRankingVo 自己的排名MainstayRankingVo数据
        /// list:[List(MainstayRankingVo)]  武将排名MainstayRankingVo数据100个
        /// </summary>
        ROLE_RANKINGS = 1,

        /// <summary>
        /// 武将信息
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// mianstay:RoleInfoVo 武将信息RoleInfoVo
        /// </summary>
        ROLE_DETAIL = 2,

        #endregion

        #region 跑商排名
        /// <summary>
        /// 跑商排名界面
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int    返回状态值
        /// oneself:BusinessRankingsVo 自己的排名BusinessRankingsVo数据
        /// list:[List(BusinessRankingsVo)]   所有跑商排名BusinessRankingsVo数据100个
        /// </summary>
        BUSINESS_RANKINGS = 501,

        /// <summary>
        /// 玩家信息
        /// 前端传递数据:
        /// rankings:int 角色在跑商排名榜的排名
        /// 服务器响应数据:
        /// result:int    返回状态值
        /// list:[List(BusinessCarVo)]  角色的所有马车BusinessCarVo数据
        /// </summary>
        BUSINESS_RANKINGS_INFO = 502,

        #endregion

        #region  卡牌排行榜

        /// <summary>
        /// 卡牌排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// oneself:CardNPCRecordVo当前玩家NPC战绩
        /// top100:[List(CardRecordVo)] NPC对战前100名
        /// </summary>
        CARD_RANKING_RANKINGS = 2100,

        #endregion

        #region  副本排行榜

        /// <summary>
        /// 名塔排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// oneself:ShotVo当前玩家打气球战绩
        /// top:[List(ShotVo)] 打气球前100名
        /// </summary>
        TOWER_SHOT_RANKINGS = 3100,

        /// <summary>
        /// 色塔排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// oneself:MaidVo当前玩家最高女仆排名
        /// top:[List(MaidVo)] 色塔排名前100名
        /// </summary>
        TOWER_MAID_RANKINGS = 3200,

        /// <summary>
        /// 利塔排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// oneself:SharpVo当前玩家最高女仆排名
        /// top:[List(SharpVo)] 色塔排名前100名
        /// </summary>
        TOWER_SHARP_RANKINGS = 3300,

        /// <summary>
        /// 竞技场排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// ranks:List[RankingVo]排名前200名
        /// </summary>
        ARENA_RANKING_LIST = 4100,

        #endregion

        #region 功名、富豪排行榜

        /// <summary>
        /// 功名排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// ranks:List[UserRankingVo]排名前10名
        /// </summary>
        RANKING_HONOR_LIST = 4200,

        /// <summary>
        /// 富豪排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// ranks:List[UserRankingVo]排名前10名
        /// </summary>
        RANKING_COIN_LIST = 4300,

        #endregion

        /// <summary>
        /// 闯关排行榜
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// ranks:List[UserRankingVo]排名前10名
        /// </summary>
        RANKING_YOUYIYUAN_LIST = 4400,

    }
}
