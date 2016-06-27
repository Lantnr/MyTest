using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 大名令指令枚举
    /// </summary>
    public enum GuideCommand
    {
        /// <summary>
        /// 进入页面
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// data:  List[DaMingLingVo]玩家引导完成数据
        /// </summary>
        ENTER = 1,

        /// <summary>
        /// 领取奖励
        /// 前端传递数据:
        /// id:doule 主键id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        REWARD = 2,

        /// <summary>
        /// 完成度推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// data:  DaMingLingVo玩家单个引导完成数据
        /// </summary>
        PUSH = 10001,
    }
}
