namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// ChatsCommand 跑商相关命令
    /// </summary>
    public enum ChatsCommand
    {
        /// <summary>
        /// 发送
        /// 前端传递数据:
        /// type:[int] 频道
        ///data:[String] 发送数据
        ///receive:[String] 个人频道接收对象(非个人时为空)
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        CHATS = 100,

        /// <summary>
        /// 玩家信息推送
        /// 服务器响应数据:
        /// type:[int] 频道
        /// id:[Double] 玩家主键Id
        /// name:[String] 玩家名称
        /// data:[String] 聊天内容信息
        /// goods:[ASObject] Vo
        /// </summary>
        CHATS_PUSH = 200,

        /// <summary>
        /// 玩家信息推送
        /// 服务器响应数据:
        ///data:List[ASObject]
        ///baseid:[int] 系统消息基表id
        ///ASObject数据:
        ///type:[int]
        ///vo:[ASObject] Vo对象 (有对象时才有此字段)
        ///id:[Double] 玩家主键id (玩家字段)
        //name:[String] 玩家名称 (玩家字段)
        ///number:[int] 数字
        /// </summary>
        CHATS_SYSTEM_PUSH = 300,

        /// <summary>
        /// 物品推送指令
        /// type:[int] 类型
        /// id:[int] 物品id
        /// count:[int] 数量
        /// </summary>
        CHATS_EXTEND = 999,

        /// <summary>
        /// 发送爬塔指令
        /// 服务器响应数据:
        /// site:[int] 爬塔层数
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        CHATS_TOWER_TEST = 1000,

        /// <summary>
        /// 系统公告发送测试
        /// 服务器响应数据:
        /// id:[int] 系统公告基表id
        /// content:[string] 内容
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        CHATS_NOTECE = 1002,
    }
}
