using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 好友指令枚举
    /// </summary>
    public enum FriendCommand
    {

        /// <summary>
        /// 获取好友
        /// result:[int] 处理结果
        /// friend:[List(FriendVo)] 好友集合
        /// </summary>
        FRIEND_JOIN = 1,

        /// <summary>
        /// 添加好友
        /// name:[String] 玩家昵称	
        /// result:[int] 处理结果
        /// </summary>			
        FRIEND_ADD = 2,

        /// <summary>
        /// 添加黑名单
        /// name:[String] 玩家昵称	
        /// result:[int] 处理结果
        /// </summary>
        FRIEND_BLACKLIST = 3,

        /// <summary>
        /// 解除黑名单
        /// id:[double] 主键id	
        /// result:[int] 处理结果
        /// </summary>
        FRIEND_REMOVE_BLACKLIST = 4,

        /// <summary>
        /// 删除好友
        /// id:[double] 主键id	
        /// result:[int] 处理结果
        /// </summary>
        FRIEND_DELETE = 5,

    }
}
