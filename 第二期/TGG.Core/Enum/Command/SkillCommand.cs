using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// SkillCommand 技能相关命令
    /// </summary>
    public enum SkillCommand
    {
        /// <summary>
        /// 生活技能锻炼
        /// 前端传递数据:
        /// id:[double] 武将主键Id
        /// baseId:[int] 技能基表Id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_LIFE_STUDY = 1,

        /// <summary>
        /// 生活技能学习推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_LIFE_PUSH = 3,

        /// <summary>
        /// 战斗技能学习
        /// 前端传递数据:
        /// id:[int] 技能基表Id
        /// roleId:[double] 武将主键Id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_FIGHT_STUDY = 4,

        /// <summary>
        /// 战斗技能升级
        /// 前端传递数据:
        /// id:[int] 战斗技能主键Id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_FIGHT_UPGRADE = 5,

        /// <summary>
        ///  战斗技能升级推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_FIGHT_PUSH = 6,

        /// <summary>
        /// 生活技能学习加速
        /// 前端传递数据:
        /// id:[double] 武将主键Id
        /// baseId:[int] 技能基表Id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_LIFE_ACCELERATE = 7,

        /// <summary>
        /// 战斗技能学习升级加速
        /// 前端传递数据:
        /// id:[int] 战斗技能主键Id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// role:[RoleVo] 武将信息Vo
        /// </summary>
        SKILL_FIGHT_ACCELERATE = 8
    }
}
