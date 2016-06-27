using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// 监狱指令枚举类
    /// </summary>
    public enum PrisonCommand
    {
        /// <summary>
        /// 检测是否在监狱中
        /// 前端传递数据:
        /// 服务器响应数据:
        ///* result:int 处理结果
        ///* time:double 服刑结束时间
        ///* count:int 留言剩余次数
        ///* playerList监狱中的玩家 List<ScenePlayerVo>集合
        /// </summary>
        CHECK = 1,

        /// <summary> 
        /// 留言
        /// 前端传递数据:
        ///  msg:String 留言
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        MESSAGE = 2,

        /// <summary> 
        /// 获取留言
        /// 前端传递数据:
        /// page:int 页数,从0开始
        /// 服务器响应数据:
        /// result:int 处理结果
        /// list: List<MessageVo>  集合
        /// </summary>
        MESSAGE_PAGE = 3,

        /// <summary>
        ///  越狱
        /// 前端传递数据:
        /// 服务器响应数据:
        ///* result:int 处理结果
        /// </summary>
        BREAK = 4,

        /// <summary>
        /// 玩家在监狱移动移动
        /// 前端传递数据:
        ///x:int 目标点x坐标
        /// y:int 目标点y坐标 
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        MOVING = 5,

        /*  ------------------推送--------------------------*/

        /// <summary> 
        /// 进入监狱
        ///* result:int 处理结果
        ///* time:Number 服刑结束时间
        ///* count:int 留言剩余次数
        ///* playerList:Array 监狱中的玩家 ScenePlayerVo 数组
        /// </summary>
        PUSH_NETER = 10001,

        /// <summary> 
        /// 场景其他玩家进入当前监狱
        /// playerVo:ScenePlayerVo 场景其他玩家数据数据
        /// </summary>
        PUSH_PLAYER_ENTER = 10003,

        /// <summary> 
        /// 场景其他玩家移动
        ///* userId:Number 玩家id
        ///* x:int 目标点x坐标
        ///* y:int 目标点y坐标  
        /// </summary>
        PUSH_PLAYER_MOVING = 10004,

        /// <summary> 
        /// 场景其他玩家离开监狱
        /// userId:Number 玩家id
        /// </summary>
        PUSH_PLAYER_EXIT = 10005,

    }
}

