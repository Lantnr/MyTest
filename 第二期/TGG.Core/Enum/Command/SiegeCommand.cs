using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary> 美浓活动指令枚举 </summary>
    public enum SiegeCommand
    {
        /// <summary>
        /// 进入活动
        ///  前端提交数据：
        /// 服务器响应数据：
        ///result:int 处理结果
        ///ladder:int 当前云梯
        ///dataA:Object 东军
        ///dataB:Object 西军
        ///playerList:List[ScenePlayerVo] 场景中的玩家 ScenePlayerVo 数组
        ///Object为:[gateHp:Number 城门血量 npcHp:Number 大将血量 coreHp:Number 本丸血量]
        ///</summary>
        ENTER = 1,

        /// <summary>
        ///  制造云梯
        /// 前端提交数据：
        /// type:int 1:制造云梯 2:破坏城门
        /// 服务器响应数据：
        ///result:int 处理结果
        ///isSuccess:Boolean 是否成功
        ///ladder:int 当前云梯
        ///</summary>
        MAKE_LADDER = 2,

        /// <summary>
        ///  进入传递点
        /// 前端提交数据：
        /// 服务器响应数据：
        ///result:int 处理结果
        ///</summary>
        ENTER_ENTRY_POINT = 3,

        /// <summary>
        ///  进入传递点
        /// 前端提交数据：
        /// type:int 1:防守 2:退出防守
        /// 服务器响应数据：
        /// result:int 处理结果
        ///</summary>
        DEFEND = 4,

        /// <summary>
        ///  攻大将
        /// 前端提交数据：
        /// type:int 1:防守 2:退出防守
        /// 服务器响应数据：
        /// result:int 处理结果
        /// ladder:int 当前云梯
        /// fight:[FightVo] 战斗Vo
        ///</summary>
        ATTACK_BOSS = 5,

        /// <summary>
        ///  进入本丸
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:int 处理结果
        ///</summary>
        BROKEN_BASE = 6,

        /// <summary>
        ///  退出美浓攻略
        /// 前端提交数据：
        /// 服务器响应数据：
        /// 推送登录进入场景
        ///</summary>
        EXIT = 7,

        /// <summary>
        /// 主角移动
        /// 前端提交数据：
        /// x:int 目标点x坐标
        /// y:int 目标点y坐标
        /// 服务器响应数据：
        /// result:int 处理结果
        ///</summary>
        MOVING = 8,

        /// <summary>
        /// 破坏城门
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:int 处理结果
        ///</summary>
        ATTACK_GATE = 9,

        /// <summary>
        /// 回到出生点
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:int 处理结果
        ///</summary>
        GO_BACK = 10,

        /// <summary>
        /// 其他玩家进入美浓攻略
        /// 前端提交数据：
        /// 服务器响应数据：
        /// playerVo:ScenePlayerVo 监狱其他玩家数据数据
        ///</summary>
        PUSH_PLAYER_ENTER = 10001,

        /// <summary>
        /// 美浓攻略其他玩家移动
        /// 前端提交数据：
        /// 服务器响应数据：
        /// userId:Number 玩家id
        /// x:int 目标点x坐标
        /// y:int 目标点y坐标  
        ///</summary>
        PUSH_PLAYER_MOVING = 10002,

        /// <summary>
        /// 其他玩家离开美浓攻略
        /// 前端提交数据：
        /// 服务器响应数据：
        /// userId:Number 玩家id
        ///</summary>
        PUSH_PLAYER_EXIT = 10003,

        /// <summary>
        /// 推送我方攻城hp数据
        /// 前端提交数据：
        /// 服务器响应数据：
        /// flag:Boolean 是否东军
        /// type:int 1:城门 2:大将 3:本丸
        /// hp:Number hp数据
        ///</summary>
        PUSH_OURS_HP = 10004,

        /// <summary>
        /// 推送玩家位置跳转
        /// 前端提交数据：
        /// 服务器响应数据：
        /// userId:Number 玩家id
        /// type:int 1:传送点 2:出生点
        ///</summary>
        PUSH_PLAYER_POS = 10005,

        /// <summary>
        /// 推送活动结束
        /// 前端提交数据：
        /// 服务器响应数据：
        /// type:int 1:东军胜 2:东军败 3:平
        /// rewards:Array 奖励数组 RewardVo 
        ///</summary>
        PUSH_END = 10006,

        /// <summary>
        /// 推送攻击城门结果
        /// 前端提交数据：
        /// 服务器响应数据：
        /// isSuccess:Boolean 是否成功
        /// ladder:int 当前云梯
        ///</summary>
        PUSH_GATE_RESULT = 10007,

        /// <summary>
        /// 推送玩家匹配战斗
        /// 前端提交数据：
        /// 服务器响应数据：
        /// fight:[FightVo] 战斗Vo
        ///</summary>
        PUSH_FIGHT = 10008,
    }
}
