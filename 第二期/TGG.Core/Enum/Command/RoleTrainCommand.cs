using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    public enum RoleTrainCommand
    {

        /// <summary>
        /// 武将传承
        /// 前端数据
        ///left:[double] 传承武将
        ///right:[double] 继承武将
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// left:[RoleInfoVo] 传承武将信息
        /// right:[RoleInfoVo] 继承武将信息
        /// </summary>
        TRAIN_INHERIT_ROLE = 1,

        /// <summary>
        /// 武将传承选择后数值变化
        /// 前端数据
        /// left:[double] 传承武将
        /// right:[double] 继承武将
        /// 服务端响应数据
        /// result:[int] 处理结果
        ///data:[List] 
        /// </summary>
        TRAIN_INHERIT_ROLE_SELECT = 14,

        /// <summary>
        /// 选取家臣
        /// 前端数据
        /// id:[double] 武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// </summary>
        TRAIN_ROLE_SELECT = 2,

        /// <summary>
        /// 开始修行
        /// 前端数据
        /// id:[double] 武将主键Id
        /// attribute:[int] 修炼属性
        /// type:[int] 修炼类型
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        TRAIN_ROLE_START = 3,

        /// <summary>
        /// 修行加速
        /// 前端数据
        /// id:[double] 武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        TRAIN_ROLE_ACCELERATE = 4,

        /// <summary>
        /// 取消家臣
        /// 前端数据
        /// id:[double] 武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// </summary>
        TRAIN_ROLE_UNSELECT = 5,

        /// <summary>
        /// 修行解锁
        /// 前端数据
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// bar:[int] 当前总数量
        /// </summary>
        TRAIN_ROLE_LOCK = 6,

        /// <summary>
        /// 修行完成推送
        /// 前端数据
        /// 服务端响应数据
        /// role:[RoleInfoVo] 武将信息
        /// </summary>
        TRAIN_ROLE_PUSH = 7,

        /// <summary>
        /// 武将难度选择
        /// 前端数据
        /// lv:[int] 难度
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// npc:[List[NpcMonsterVo]] NPC武将集合
        /// </summary>
        TRAIN_HOME_LEVEL_SELECT = 8,


        /// <summary>
        /// 购买挑战次数
        /// 前端数据
        /// id:[int] NPC武将主键Id
        /// 服务端响应数据
        /// fightCount:[int] 剩余挑战次数
        /// result:[int] 处理结果
        /// </summary>
        TRAIN_HOME_FIGHT_BUY = 9,

        /// <summary>
        /// 茶道
        /// 前端数据
        /// id:[int] NPC武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// spirit:[int] 获得的魂数
        /// npcSpirit:[int] npc剩余魂数
        /// </summary>
        TRAIN_HOME_NPC_TEA = 10,

        /// <summary>
        /// 挑战
        /// 前端数据
        /// id:[int] NPC武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// type:[int] 0:挑战失败；1：挑战胜利
        /// </summary>
        TRAIN_HOME_NPC_FIGHT = 11,

        /// <summary>
        /// 武将刷新
        /// 前端数据
        /// lv:[int] 难度
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// npc:[List[NpcMonsterVo]] NPC武将集合
        /// </summary>
        TRAIN_HOME_NPC_REFRESH = 12,

        /// <summary>
        /// 偷窃
        /// 前端数据
        /// id:[int] NPC武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// type:[int] 0:偷窃失败；1：偷窃胜利
        /// </summary>
        TRAIN_HOME_NPC_STEAL = 13,

        /// <summary>
        /// 武将点将加载
        /// 前端数据
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// refresh:[int] 刷新次数
        /// fightCount:[int] 剩余挑战次数
        /// buyCount:[int] 剩余购买次数
        /// </summary>
        TRAIN_HOME_JOIN = 16,

        /// <summary>
        /// 茶道顿悟
        /// 前端数据
        /// id:[int] NPC武将主键Id
        /// 服务端响应数据
        /// result:[int] 处理结果
        /// spirit:[int] 获得的魂数
        /// npcSpirit:[int] npc剩余魂数
        /// </summary>
        TRAIN_TEA_INSIGHT = 17,

    }
}
