namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// Enum DuplicateCommand.
    /// 副本指令枚举
    /// </summary>
    public enum DuplicateCommand
    {
        #region 猎魂系统指令

        /// <summary>
        /// 进入名塔
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// top:[Lis（ShotVo）] 前3名
        /// oneself:ShotVo
        /// count:[int] 剩余闯关次数
        /// </summary>
        TOWER_SHOT_ENTER = 1,

        /// <summary>
        /// 关卡结束
        /// 前端传递数据:
        /// score:[int] 当前分数
        /// type:[int] 关卡
        /// isexit:[int] 0:继续 1:退出
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// count:[int] 剩余闯关次数
        /// </summary>  
        TOWER_SHOT_FUNISH = 2,

        #endregion

        #region 利塔

        /// <summary>
        /// 进入利塔
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// reset:[int] 重置次数
        /// top:[SharpVo] 通关最快者
        /// oneself:[SharpVo] 我的关卡排行
        /// total:[double]扫荡总时间
        /// </summary>
        TOWER_SHARP_ENTER = 3100,

        /// <summary>
        /// 属性加成道具
        /// 前端传递数据:
        /// id:[double]道具主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        TOWER_SHARP_BUFF = 3500,
        #endregion

        #region 爬塔
        /// <summary>
        /// 进入爬塔
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// towerpass：[TowerPassVo ]关卡信息
        /// count:[int] 剩余闯关次数
        /// watchmen:[UserInfoVo] 守护者信息
        /// challenge:[int] 0:有权挑战守护者  1：无权挑战守护者
        /// </summary>
        TOWER_CHECKPOINT_ENTER = 3,

        /// <summary>
        /// 翻将
        /// 前端传递数据:
        /// type:[int] 0：使用翻将次数  1：使用元宝
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// towerpass：[TowerPassVo ]关卡信息
        /// </summary>
        TOWER_CHECKPOINT_NPC_REFRESH = 4,

        /// <summary>
        /// 挑战怪物
        /// 前端传递数据:
        /// npcid：[double]怪物id
        /// type：[int] 0:挑战怪物   1：挑战守护者
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        TOWER_CHECKPOINT_DARE = 5,

        /// <summary>
        /// 辩才小游戏
        /// 前端传递数据:
        /// type：[int] 卡牌类型
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// type：[int] npc卡牌类型
        /// userBlood：[int]玩家气血值
        /// npcBlood:[int] npc气血值
        /// </summary>
        TOWER_CHECKPOINT_ELOQUENCE_GAME = 6,

        /// <summary>
        /// 忍术小游戏开始
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// position:[int] 色子出现位置
        /// xing:[string] 星星字段
        /// </summary>
        TOWER_CHECKPOINT_NINJUTSU_GAME_START = 7,

        /// <summary>
        /// 忍术小游戏结束
        /// 前端传递数据:
        /// position:[int] 选择盅的位置
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// position:[int] 下一局色子出现位置
        /// xing:[string] 星星字段
        /// 通关结束推送:1:通关成功 2:通关失败   (未通关不推送)
        /// </summary>
        TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT = 8,

        /// <summary>
        /// 算术游戏开始
        /// 前端传递数据:
        /// values：[list[int]] 数字结果集合
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// xing:[string] 星星字段
        /// 通关结束推送:1:通关成功 2:通关失败   (未通关不推送)
        /// </summary>
        TOWER_CHECKPOINT_CALCULATE_GAME = 9,

        /// <summary>
        /// 推送挑战结果
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// type:[int] 1：挑战胜利；2:挑战失败
        /// blood:[int] 剩余血量
        /// challenge:[int] 0:有权挑战守护者  1：无权挑战守护者
        /// watchmen:[UserInfoVo] 守护者信息
        /// towerpass：[TowerPassVo ]关卡信息
        /// </summary>
        TOWER_CHECKPOINT_DARE_PUSH = 12,

        /// <summary>
        /// 花月茶道进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// npcTea:[int] 关主茶席值
        /// userTea:[int] 玩家茶席值
        /// photoStateList:Array 牌的所有状态
        /// </summary>
        TOWER_CHECKPOINT_TEA_GAME_ENTER = 10,

        /// <summary>
        /// 花月茶道翻牌
        /// 前端传递数据:
        /// loc：要查看的牌位置(1-30)
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// npcTea:[int] 关主茶席值
        /// userTea:[int] 玩家茶席值
        /// loc:int;  要查看的牌位置
        /// userPhoto:用户图案
        /// npcLoc:int; 电脑翻出的牌位置
        /// npcPhoto:电脑图案
        /// 一方血量值为0：推送  通关则成功，不通关则失败
        /// </summary>
        TOWER_CHECKPOINT_TEA_GAME_FLOP = 11,

        /// <summary>
        /// 算术游戏进入
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// xing:[string] 星星字段
        /// </summary>
        TOWER_CHECKPOINT_CALCULATE_GAME_JOIN = 13,

        /// <summary>
        /// 进入辩才小游戏
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// userBlood：[int]玩家气血值
        /// npcBlood:[int] npc气血值
        /// </summary>
        TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER = 14,

        /// <summary>
        /// 点击下一关
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// towerpass：[TowerPassVo ]关卡信息
        /// count:[int] 剩余闯关次数
        /// watchmen:[UserInfoVo] 守护者信息
        /// </summary>
        TOWER_CHECKPOINT_NEXT_PASS = 15,
        #endregion
    }
}
