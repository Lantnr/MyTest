namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// MessageCommon 邮件相关命令
    /// </summary>
    public enum MessageCommand
    {
        /// <summary>
        /// 进入邮件系统所有邮件信息
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// data:[List(MessagesVo)] 当前玩家所有邮件信息
        /// </summary>
        MESSAGE_VIEW = 1,

        /// <summary>
        /// 提取附件
        /// 前端传递数据:
        /// type：［int］ 0：当前邮件 1:所有邮件
        /// id:[double]邮件主键id  (所有邮件时为空)
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        MESSAGE_ATTACHMENT = 2,

        /// <summary>
        /// 删除邮件
        /// 前端传递数据:
        /// type:[int] 0：当前邮件 1:所有邮件
        /// ids:[List<double>]邮件主键集合(全部删除时为空)
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        MESSAGE_DETELE = 3,

        /*
        /// <summary>
        /// 获取好友
        /// 前端传递数据:
        /// userid:[double]玩家主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// friend:[List[FriendVo]] 好友集合
        /// </summary>
        MESSAGE_FRIENDS = 500,

        /// <summary>
        /// 发送
        /// 前端传递数据:
        /// friend:[List[double]]好友主键id集合
        /// contents:[String] 邮件内容
        /// attachment:[String] 附件
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        MESSAGE_SEND = 600,
        */

        /// <summary>
        /// 发送
        /// 前端传递数据:
        /// id:[List[double]]邮件主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        MESSAGE_READ = 4,

        /// <summary>
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// number:[int] 未读邮件数
        /// </summary>
        MESSAGE_NOREAD = 5,

        /// <summary>
        /// 推送未读邮件数
        /// 服务器响应数据:
        /// number:[int] 未读邮件数
        /// </summary>
        MESSAGE_PUSH = 6,
    }
}
