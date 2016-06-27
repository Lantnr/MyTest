namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// TitleCommand 家臣称号相关指令
    /// </summary>
    public enum TitleCommand
    {
        /// <summary>
        /// 称号信息
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// value:[list[int]] 次数集合
        /// titles:[List[TitleVo]] 所有已达成称号信息
        /// </summary>
        ROLE_TITLE_JOIN = 1,

        /// <summary>
        /// 称号装备
        /// 前端传递数据:
        /// titleId:[double]称号主键id
        /// roleId:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// title:[TitleVo] 当前称号信息
        /// </summary>
        ROLE_TITLE_LOAD = 2,

        /// <summary>
        /// 称号卸载
        /// 前端传递数据:
        /// titleId:[double]称号主键id
        /// roleId:[double]武将主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// title:[TitleVo] 当前称号信息
        /// </summary>
        ROLE_TITLE_UNLOAD = 3,

        /// <summary>
        /// 称号解锁
        /// 前端传递数据:
        /// titleId:[double]称号主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// title:[TitleVo] 当前称号信息
        /// </summary>
        ROLE_TITLE_PACKET_BUY = 4,

        /// <summary>
        /// 称号装备
        /// 前端传递数据:
        /// id:[int]称号基表id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// count：[int] 次数
        /// title:[TitleVo] 当前称号信息
        /// </summary>
        ROLE_TITLE_SELECT = 5,
    }
}
