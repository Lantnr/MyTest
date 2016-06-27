using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 游艺园枚举
    /// </summary>
    public enum GameCommand
    {
        /// <summary>
        /// 进入游戏
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// games:[YouYiyuanVo] 游戏信息
        /// total:[int]本周最高闯关总数
        /// </summary>
        GAMES_JOIN = 1,

        /// <summary>
        /// 猎魂游戏
        /// 前端传递数据:
        /// score:[int] 当前分数
        /// pass:[int] 关卡
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// count：[int]闯关次数
        /// </summary>
        GAMES_SHOT_FINISH = 2,

        /// <summary>
        /// 辩才游戏进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        GAMES_ELOQUENCE_ENTER = 3,

        /// <summary>
        /// 辩才游戏开始
        /// 前端传递数据:
        /// cardtype：[int] 卡牌类型
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// type：[int] npc卡牌类型
        /// userBlood：[int]玩家气血值
        /// npcBlood:[int] npc气血值
        /// </summary>
        GAMES_ELOQUENCE_START = 4,

        /// <summary>
        /// 花月茶道进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// pass:[int] 闯关次数
        /// photoStateList:Array 牌的所有状态
        /// </summary>
        GAMES_TEA_ENTER = 5,

        /// <summary>
        /// 花月茶道开始
        /// 前端传递数据:
        /// loc：要查看的牌位置(1-30)
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// npcTea:[int] 关主茶席值
        /// userTea:[int] 玩家茶席值
        /// loc:int;  要查看的牌位置
        /// userPhoto:用户图案
        /// npcLoc:int; 电脑翻出的牌位置
        /// npcPhoto:电脑图案
        /// </summary>
        GAMES_TEA_START = 6,

        /// <summary>
        /// 忍术游戏进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// position:[int] 色子出现位置
        /// </summary>
        GAMES_NINJUTSU_ENTER = 7,

        /// <summary>
        /// 忍术游戏开始
        /// 前端传递数据:
        /// position：[int] 选择盅的位置
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        GAMES_NINJUTSU_START = 8,

        /// <summary>
        /// 算术游戏开始
        /// 前端传递数据:
        /// values：[list[int]] 数字结果集合
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        GAMES_CALCULATE_START = 9,

        /// <summary>
        /// 领取每日完成游戏次数的奖励
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        GAMES_RECEIVE = 10,

        /// <summary>
        /// 推送游戏结果
        /// 前端传递数据:
        /// 服务器响应数据:
        /// type：[int] 0:失败  1：胜利
        /// pass:[int] 闯关次数
        /// gametype：[int] 游戏类型
        /// </summary>
        GAMES_PUSH = 11,

        /// <summary>
        /// 猎魂游戏进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        GAMES_SHOT_ENTER = 12,

        /// <summary>
        /// 算术游戏进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        GAMES_CALCULATE_ENTER = 13,

        /// <summary>
        /// 每日完成度推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// state：[int] 未领取：0 可领取：1 已领取：2
        /// </summary>
        GAMES_REWARD_PUSH = 14,

        /// <summary>
        /// 退出小游戏
        /// 前端传递数据:
        /// gametype：[int] 游戏类型
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        GAME_EXIT = 15,

        /// <summary>
        /// 选择游戏模式
        /// 前端传递数据:
        /// type:[int] 0：闯关模式  1：练习模式
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        GAMES_SELECT = 16,
    }
}
