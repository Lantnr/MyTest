namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 角色武将指令枚举
    /// </summary>
    public enum RoleCommand
    {
        /// <summary>
        /// 武将信息
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// roles: List[RoleInfoVo] 所有武将信息
        /// bar:[int] 当前武将修炼栏数量
        /// homeHireVo:[HomeHireVo]保镖vo
        /// powerCount:[int] 购买体力次数
        /// </summary>
        ROLE_JOIN = 1,

        /// <summary>
        /// 武将放逐
        /// 前端传递数据:
        /// roleId:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        ROLE_EXILE = 2,

        /// <summary>
        /// 秘技/奥义选择
        /// 前端传递数据:
        /// skillId:[double]技能主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        ROLE_SKILL_SELECT = 4,

        /// <summary>
        /// 武将装备穿戴
        /// 前端传递数据:
        /// equipId:[double]装备主键id
        /// roleId:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// equip: [EquipVo] 原来装备信息,没有就为空
        /// </summary>
        ROLE_EQUIP_LOAD = 5,

        /// <summary>
        /// 武将装备卸载
        /// 前端传递数据:
        /// equip:装备主键id
        /// roleId:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        ROLE_EQUIP_UNLOAD = 6,

        /// <summary>
        /// 武将属性保存
        /// 前端传递数据:
        /// att:[List[int]]加成值
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// growAddCount:[int] 剩余点数
        /// </summary>
        ROLE_ATTRIBUTE = 7,

        #region 招募武将指令枚举

        /// <summary>
        /// 进入酒馆
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[List[RoleInfoVo]] 酒馆武将信息
        /// </summary>
        RECRUIT_JOIN = 8,

        /// <summary>
        /// 武将抽取
        /// 前端传递数据:
        /// type:[int] 抽取类型 0:一次 1:10次
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[List[RoleInfoVo]] 获得武将信息
        /// </summary>
        RECRUIT_GET = 9,

        /// <summary>
        /// 刷新
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[List[RoleInfoVo]] 获得武将信息
        /// </summary>
        RECRUIT_REFRESH = 10,

        #endregion

        /// <summary>
        /// 武将推送
        /// 服务器响应数据:
        /// type:[int] 1:主角升级
        /// role:RecruitVo 推送的武将
        /// experience:[int]获取的经验数
        /// growAddCount:[int] 增加的可分配的属性点
        /// </summary>
        ROLE_PUSH = 11,

        ///推送武将属性更新
        /// 服务器响应数据:
        ///id:[double]武将主键id
        ///data:List<ASObject> 
        ///ASObject数据类型
        ///name:[string]武将更新属性名字
        ///value:[int]武将属性数值
        ROLE_UPDATE = 12,

        /// <summary>
        /// 选择流派或忍者众
        /// 前端传递数据:
        /// id:[double]武将主键id
        /// type:[int]流派类型或忍者
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        SELECT_GENRE = 13,

        /// <summary>
        /// 主角武将选择流派或忍者众
        /// 前端传递数据:
        /// typeA:[int] 流派类型
        /// typeB:[int] 忍者众类型
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        SELECT_GENRE_ROLE = 14,

        /// <summary>
        ///主将购买体力
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// count:[int] 用户购买次数
        /// </summary>
        POWER_BUY = 15,

        /// <summary>
        ///雇佣
        ///  前端传递数据:
        /// id:[double] 武将基表id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// homeHireVo:[HomeHireVo]保镖vo
        /// </summary>
        ROLE_HIRE = 16,

        /// <summary>
        ///雇佣结束推送
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        ROLE_HIRE_END = 17,

        /// <summary>
        /// 查看玩家信息
        /// 前端传递数据:
        /// userId:[int] 要查看的玩家id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// name：[string] 玩家名称
        /// camp：[int] 玩家阵营
        /// influence：[int] 玩家势力
        /// equips：List[EquipVo] 玩家所有武将身上穿戴的装备
        /// roles: List[RoleInfoVo] 所有武将信息
        /// </summary>
        QUERY_PLAYER_ROLE = 18,

        /// <summary>
        /// 武将体力购买
        /// 前端传递数据:
        /// id:武将主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        ROLE_POWER_BUY =19,
    }
}
