using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 竞技场指令枚举
    /// </summary>
    public enum ArenaCommand
    {
        /// <summary>
        /// 进入竞技场
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// arena:[ArenaVo] 竞技场信息Vo
        /// </summary>
        ARENA_JOIN = 1,

        /// <summary>
        /// 增加挑战次数
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// total:[int] 总次数
        /// count:[int] 剩余次数
        /// </summary>
        ARENA_DEKARON_ADD = 2,

        /// <summary>
        /// 清除冷却
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        ARENA_REMOVE_COOLING = 3,

        /// <summary>
        /// 挑战
        /// 前端发送数据
        /// id:[double]玩家id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// fight:List[FightVo] 战斗信息
        /// </summary>
        ARENA_DEKARON = 4,

        /// <summary>
        /// 查看战斗过程
        /// 前端发送数据
        /// id:[int]战报主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// fight:List[FightVo] 战斗信息
        /// </summary>
        ARENA_FIGHT_LOOK = 5,
    }
}
