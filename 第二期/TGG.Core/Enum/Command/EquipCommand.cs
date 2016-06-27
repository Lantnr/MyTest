namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 装备指令
    /// </summary>
    public enum EquipCommand
    {
        /// <summary>
        /// 装备铸魂
        /// 前端传递数据:
        /// id:[double] 装备主键id
        /// type:[int]属性类型
        /// count:[int] 铸魂数
        /// location:[int]属性位置
        /// roleId：[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// equip:[EquipVo] 装备
        /// spirit:[int]   剩余魂数
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        EQUIP_SPIRIT = 2,

        /// <summary>
        /// 装备铸魂解锁
        /// 前端传递数据:
        /// id:[double] 装备主键id
        /// type:[int]属性类型
        /// location:[int]属性位置
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// equip:[EquipVo] 装备
        /// </summary>
        EQUIP_SPIRIT_LOCK = 3,

        /// <summary>
        /// 装备买
        /// 前端传递数据:
        /// id:[int]装备id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// equip:[EquipVo] 装备
        /// </summary>
        EQUIP_BUY = 4,

        /// <summary>
        /// 装备卖
        /// 前端传递数据:
        /// id:[double]装备主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        EQUIP_SELL = 5,

        /// <summary>
        /// 装备洗炼
        /// 前端传递数据:
        /// id:[double]装备主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        EQUIP_BAPTIZE = 6,

    }
}
