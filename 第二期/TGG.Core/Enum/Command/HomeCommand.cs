namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 自宅指令
    /// </summary>
    public enum HomeCommand
    {
        #region 福禄树——基本资源产出
        /// <summary>
        /// 进入俸禄树
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// tree:[List[TreeVo]] 所有果子集合
        /// </summary>  
        HOME_TREE_ENTER = 1000,

        /// <summary>
        /// 摘取
        /// 前端传递数据:
        /// treeid:[double] 摘取果子主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// newtree:TreeVo 摘取后新果子
        /// </summary>  
        HOME_TREE_GET = 1100,

        #endregion

        #region 九五至尊鼎——卡片合成

        /// <summary>
        /// 进入卡位
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// build:[List[CardVo]] 正在练卡的卡牌
        /// </summary>  
        HOME_CARD_ENTER = 2000,

        /// <summary>
        /// 进入练卡
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// cards:[List[CardVo]] 卡牌集合
        /// </summary>  
        HOME_CARD_MAKE = 2100,

        /// <summary>
        /// 合成
        /// 前端传递数据:
        /// cardid:[double] 卡牌主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// build:[CardVo] 正在练卡的卡牌
        /// </summary>  
        HOME_CARD_BUILD = 2200,

        /// <summary>
        /// 取消合成
        /// 前端传递数据:
        /// cardid:[double] 卡牌主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// new:[List[PropVo]]  玩家练卡新材料集合
        /// update:[List[PropVo]]  玩家练卡更新材料集合
        /// card:CardVo
        /// </summary>  
        HOME_CARD_UNBUILD = 2300,

        /// <summary>
        /// 取卡
        /// 前端传递数据:
        /// cardid:[double] 卡牌主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// card:[CardVo] 卡牌
        /// </summary>  
        HOME_CARD_GET = 2400,

        /// <summary>
        /// 进入材料盒
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// prop:[List[PropVo]]  玩家材料集合
        /// </summary>  
        HOME_CARD_JOIN = 2900,

        #endregion

        #region 炼金屋——研究特产和工艺品
        /// <summary>
        /// 进入炼金屋
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///formula:[List[FormulaVo]]配方集合
        /// </summary>  
        HOME_ALCHEMY_ENTER = 3000,

        /// <summary>
        /// 研发
        /// 前端传递数据:
        /// id:[double]配方主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///formula:[List[FormulaVo]]配方集合
        /// </summary>  
        HOME_ALCHEMY_DEVELOPE = 3100,

        /// <summary>
        /// 加速
        /// 前端传递数据:
        /// id:[double]配方主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///formula:[List[FormulaVo]]配方集合
        /// </summary>  
        HOME_ALCHEMY_ACCELERATE = 3200,

        #endregion

        #region 工坊——女仆生产
        /// <summary>
        /// 进入工坊
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// maid:[List[MaidVo]]女仆Vo
        /// equip:[List[EquipVo]] 女仆装备
        /// order:[List[OrderVo]] 订单集合
        /// level:[int] 工坊等级
        /// </summary>  
        HOME_WORKSHOP_ENTER = 4000,

        /// <summary>
        /// 开始订单
        /// 前端传递数据:
        /// id:[double] 订单id主键
        /// maidid:[int] 当前女仆主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// order:[OrderVo]订单信息

        /// </summary>  
        HOME_WORKSHOP_ORDER_START = 4100,

        /// <summary>
        /// 订单变更
        /// 前端传递数据:
        /// id:[double] 订单id主键
        /// maidid:[int] 当前女仆主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// order:[OrderVo]订单信息
        /// </summary>  
        HOME_WORKSHOP_ORDER_UPDATE = 4200,

        /// <summary>
        /// 订单加速
        /// 前端传递数据:
        /// id:[double] 订单id主键
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>  
        HOME_WORKSHOP_ORDER_ACCELERATE = 4300,

        /// <summary>
        /// 女仆装备穿戴
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>  
        HOME_WORKSHOP_MAID_LOAD = 4700,

        /// <summary>
        /// 女仆装备卸载
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>  
        HOME_WORKSHOP_MAID_UNLOAD = 4710,

        /// <summary>
        /// 订单结束推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// goods:[BusinessGoodsVo] 货物信息
        /// </summary>  
        HOME_WORKSHOP_ORDER_END = 4800,

        /// <summary>
        /// 工坊升级
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// level:[int] 工坊等级
        /// </summary>  
        HOME_WORKSHOP_UPGRADE = 4400,

        /// <summary>
        /// 女仆选择
        /// 前端传递数据:
        /// maidid:[int] 当前女仆主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// order:[List[OrderVo]]订单信息
        /// </summary>  
        HOME_WORKSHOP_MAID_SELECT = 4500,
        #endregion

        #region 健身房——武将锻炼、培养

        /// <summary>
        /// 进入健身馆
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// gym:GymVo 当前健身馆信息
        /// </summary>  
        HOME_GYM_ENTER = 5000,

        /// <summary>
        /// 升级
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// gym:GymVo 当前健身馆信息
        /// </summary>  
        HOME_GYM_UPGRADE = 5100,

        /// <summary>
        /// 开始修炼
        /// 前端传递数据:
        /// roleid:[double] 武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 当前武将信息
        /// </summary>  
        HOME_GYM_EXP_START = 5200,

        /// <summary>
        /// 经验领取
        /// 前端传递数据:
        /// roleid:[double] 武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 当前武将信息
        /// exp:[int] 修炼获得经验值
        /// </summary>  
        HOME_GYM_EXP_GET = 5300,

        /// <summary>
        /// 经验全部领取
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// roles:[List[RoleInfoVo]] 当前武将信息
        /// exp:[List[ASObject]]ASObject[id,exp]
        /// </summary>  
        HOME_GYM_EXP_GET_ALL = 5400,

        /// <summary>
        /// 分配经验
        /// 前端传递数据:
        /// roleid:[double] 武将主键id
        /// exp:[int] 分配经验
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 当前武将信息
        /// gym:GymVo 当前健身馆信息
        /// </summary>  
        HOME_GYM_EXP_ASSIGN = 5500,

        /// <summary>
        /// 取消修炼
        /// 前端传递数据:
        /// roleid:[double] 武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 当前武将信息
        /// </summary>  
        HOME_GYM_EXP_CANCEL = 5600,

        #endregion

        #region 招待所——武将刷新点
        /// <summary>
        /// 进入招待所
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///favorite:[int] 好友度
        /// hostel:[List<HostelVo>] 招待所武将Vo集合
        /// activity:[int] 选择活跃度
        ///</summary>  
        HOME_HOSTEL_ENTER = 6000,

        /// <summary>
        /// 活跃度
        /// 前端传递数据:
        /// lock:[int] 0:未锁定 1:锁定
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///role:[HostelVo] 招募武将
        ///next:[int] 下一次活跃度
        /// </summary>  
        HOME_HOSTEL_ACTIVITY = 6001,

        /// <summary>
        /// 招募武将
        ///   前端传递数据:
        /// role:[double] 招募武将id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///role:[RoleInfoVo] 招募武将信息
        /// </summary>
        HOME_HOSTEL_RECRUIT = 6100,

        /// <summary>
        /// 一键寻贤
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///role:[List<RoleInfoVo>] 招募武将信息
        ///</summary>
        HOME_HOSTEL_FIND_MANUAL = 6200,

        /// <summary>
        /// 自动寻贤
        /// 前端传递数据:
        /// level:[int] 招募武将等级
        ///count:[int] 招募次数
        ///lock:[int] 0:未锁定 1:锁定
        /// 服务器响应数据:
        ///  result:[int] 返回状态值
        ///role: [HostelVo]寻贤到武将
        ///next:[int] 下一次活跃度
        ///favorite:[int] 好友度
        ///out:[List<HostelVo>] 送客武将
        ///</summary>
        HOME_HOSTEL_FIND_AUTO = 6300,

        /// <summary>
        /// 一键送客
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///favorite:[int] 好友度
        ///</summary>
        HOME_HOSTEL_GO_MANUAL = 6400,

        /// <summary>
        /// 兑换武将
        /// 前端传递数据:
        /// roleid:[double] 兑换武将id主键
        ///type:[int] 0:好友度 1:丁银
        /// 服务器响应数据:
        ///  result:[int] 返回状态值
        ///role:[RoleInfoVo] 招募武将信息
        ///favorite:[int] 好友度
        ///type:[int] 0:好友度 1:丁银
        ///</summary>
        HOME_HOSTEL_CONVERT = 6500,

        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        ///activity:[int] 活跃度
        HOME_HOSTEL_LOCK = 6600,
        #endregion

        #region 行政大厅
        #endregion

        #region 秘术修炼

        /// <summary>
        /// 点亮星图
        /// 前端传递数据:
        /// role:[double]当前武将主键id
        /// starLv:[int] 星级
        /// mapLv:[int] 星图等级
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 当前武将信息
        /// </summary>  
        HOME_CABALIST_LIT = 8000,
        #endregion
    }
}
