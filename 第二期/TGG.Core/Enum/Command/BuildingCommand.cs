using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 一夜墨俣活动指令
    /// </summary>
    public enum BuildingCommand
    {
        /// <summary>
        /// 进入活动
        /// 服务器响应数据：
        /// result:[int] 处理结果
        /// playerList活动的玩家 List<ScenePlayerVo>集合
        /// dataA:Object 东军
        ///dataB:Object 西军
        ///Object为:[cityHp:Number 城池耐久 npcHp:Number 大将血量 ]
        ///wood:[int] 木材
        ///torch:[int] 火把
        ///baseBuild:[int]建材
        ///fame:[int]声望

        ///</summary>
        ENTER = 1,

        /// <summary>
        /// 收集木材
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///type:[int]0：失败 1：成功
        ///count:[int] 木材数量
        ///</summary>
        GET_WOOD = 2,

        /// <summary>
        /// 收集火把
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///type:[int]0：失败 1：成功
        ///count:[int] 火把数量
        ///</summary>
        TORCH = 3,

        /// <summary>
        /// 击杀boss
        /// 服务器响应数据：
        /// result:[int] 处理结果
        /// result:int 处理结果
        /// fight:[FightVo] 战斗Vo
        ///</summary>
        KILL_BOSS = 4,

        /// <summary>
        /// 制造建材
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///type:[int]0：失败 1：成功
        ///baseBuild:[int] 建材
        /// wood:[int] 木材
        ///</summary>
        MAKE_BUILD = 5,

        /// <summary>
        /// 筑城
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///type:[int]0：失败 1：成功
        ///baseBuild:[int] 建材
        /// fame:[int] 声望
        /// durability:[int] 城池耐久度
        ///</summary>
        BUILDING = 6,

        /// <summary>
        /// 放火
        /// 前端提交数据：
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///type:[int]0：失败 1：成功
        ///torch:[int] 火把
        /// fame:[int] 声望
        ///durability:[int] 城池耐久度
        ///</summary>
        FIRE = 7,

        /// <summary>
        /// 离开活动
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///</summary>
        EXIT = 8,

        /// <summary>
        /// 人物移动
        /// 前端提交数据：
        ///x:[int]目标点x坐标
        ///y:[int]目标点y坐标
        /// result:[int] 处理结果
        ///</summary>
        MOVING = 9,

        /// <summary>
        /// 回到出生点
        /// 前端提交数据：
        /// result:[int] 处理结果
        ///</summary>
        BACK_POINT = 10,



        /*---------------------------推送---------------------------- */
        /// <summary>
        /// BOSS血量
        /// type:[int]1:我方。 2.对方
        /// blood:[int] boss血量
        /// </summary>
        BOOS_BLOOD = 10001,

        /// <summary>
        /// 活动结束
        /// type:[int]0：失败 1：成功
        /// count:[int]奖励声望
        /// </summary>
        END = 10002,

        /// <summary>
        /// 玩家移动推送
        /// userId:[int] 玩家id
        ///  x:int 目标点x坐标
        ///  y:int 目标点y坐标
        /// </summary>
        PUSH_PLAYER_MOVING = 10003,

        /// <summary> 
        /// 场景其他玩家进入活动
        /// playerVo:ScenePlayerVo 场景其他玩家数据数据
        /// </summary>
        PUSH_PLAYER_ENTER = 10004,


        /// <summary> 
        /// 场景其他玩家退出活动
        /// userId:Number 玩家id
        /// </summary>
        PUSH_PLAYER_EXIT = 10005,

        /// <summary> 
        /// 场景其他玩家坐标改变
        /// playerVo:ScenePlayerVo 场景其他玩家数据数据
        /// </summary>
        PUSH_PLAYER_CHANGE = 10006,

        /// <summary>
        /// 城池耐久度	
        /// type:[int]1:东军 2.西军	
        /// durability:[int] 城池耐久度			
        /// </summary>
        CITY_DURABILITY = 10007,


    }
}
