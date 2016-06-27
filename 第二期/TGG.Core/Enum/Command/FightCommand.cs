namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 
    /// Enum FightCommand.
    /// 战斗指令枚举
    /// </summary>
    public enum FightCommand
    {
        /// <summary>
        /// 进入个人战设置
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// matrix:[MatrixVo] 阵Vo
        /// </summary>
        FIGHT_PERSONAL_JOIN=1,

        /// <summary>
        /// 阵武将选择
        /// 前端传递数据:
        /// roleId:[double]
        /// location:[当前武将位置]
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FIGHT_PERSONAL_ROLE_SELECT = 2,

        /// <summary>
        /// 战斗
        /// 前端传递数据:
        /// module:[int] ModuleNumber 模块号
        /// cmd:[int] 命令号
        /// 服务器响应数据:
        /// result:int 处理结果
        /// rewards:Array 奖励 RewardVo 数组
        /// data:[FightVo] 战斗结果信息
        /// module:[int] ModuleNumber 模块号
        /// cmd:[int] 命令号
        /// </summary>
        FIGHT_PERSONAL_ENTER = 3,

        /// <summary>
        /// 拉取印数据
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        ///yin:[YinVo] 印Vo
        /// </summary>
        YIN_JION = 4,

        /// <summary>
        /// 印升级
        /// 前端传递数据:
        /// id:[double] 印id主键
        /// 服务器响应数据:
        /// result:int 处理结果
        /// yin:[YinVo] 印Vo
        /// </summary>
        YIN_UPGRADE = 5,

        /// <summary>
        /// 阵武将丢弃
        /// 前端传递数据:
        /// roleId:[double] 武将主键ID
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FIGHT_PERSONAL_ROLE_DELETE = 6,

        /// <summary>
        /// 阵武将交换
        /// 前端传递数据:
        /// roleId:[double] 武将主键ID
        /// location:[int] 当前武将位置
        /// roleIdOrg:[double] 原来位置武将
        /// newLocation:[int] 新的阵位置
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FIGHT_PERSONAL_ROLE_CHANGE = 7,

        /// <summary>
        /// 阵印选择
        /// 前端传递数据:
        /// id:[double] 印主键
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FIGHT_PERSONAL_YIN_SELECT = 8,

        /// <summary>
        /// 战斗道具拾取
        /// 前端传递数据:
        /// type:0:丢弃 1：入包
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FIGHT_PROP_PICKUP=9,

        /// <summary>
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// type:[int]0:没有掉落 1：有掉落
        /// reward:List<RewardVo> 战斗掉落物品
        /// </summary>
        FIGHT_PROP =10,
    }
}
