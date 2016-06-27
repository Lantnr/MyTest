namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// PropCommand 道具相关命令
    /// </summary>
    public enum PropCommand
    {
        /// <summary>
        /// 道具数据
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// prop: [List(PropVo)] 当前玩家所有道具
        /// equip:[List<EquipVo>] 当前玩家背包装备集合
        /// </summary>
        PROP_JOIN = 1,

        /// <summary>
        /// 道具使用
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// count:[int] 使用数量
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// prop: [PropVo] 当前玩家道具信息
        /// 返回更新用户信息(USER_INFO)
        /// </summary>
        PROP_USE = 2,

        /// <summary>
        /// 道具出售
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// type:类型(道具大类)
        /// count:[int] 使用数量
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// prop: [PropVo] 当前玩家道具信息
        /// 道具出售后直接删除
        /// 返回更新用户信息(USER_INFO)
        /// </summary>
        PROP_SELL = 3,

        /// <summary>
        /// 批量出售
        /// 前端传递数据:
        /// propids: [List<id>] 当前批量出售道具主键id集合
        /// equipids: [List<id>] 当前批量出售主键装备id集合
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// 返回更新用户信息(USER_INFO)
        /// </summary>
        PROP_SELL_BULK = 4,

        /// <summary>
        /// 开启背包格子
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        PROP_OPEN_GRID = 5,

        /// <summary>
        /// 拾取道具
        /// 前端传递数据:
        /// type:类型 0:丢弃  1:入包
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        PROP_PICKUP = 6,

        /// <summary>
        /// 道具丢弃
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        PROP_DISCARD = 7,

        /// <summary>
        /// 道具合成
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        PROP_SYNTHETIC = 8,

        /// <summary>
        /// 家臣使用
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:RoleInfoVo 武将信息
        /// </summary>
        PROP_ROLE_USE=9,

        /// <summary>
        /// 流派技能书使用
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// rid:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// data:List[ASObject] 
        /// </summary>
        PROP_GENRE_USE = 10,
    }
}
