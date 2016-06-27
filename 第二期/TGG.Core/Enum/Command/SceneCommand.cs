namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// Enum SceneCommand
    /// 场景命令枚举
    /// </summary>
    public enum SceneCommand
    {
        /// <summary>
        /// 登陆后进入当前场景
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// id:int 场景基础 id 
        /// x:int 场景x坐标
        /// y:int 场景y坐标
        /// playerList:List[ScenePlayerVo] 场景其他玩家数据 
        /// </summary>
        LOGIN_ENTER_SCENE = 1,

        /// <summary>
        /// 进入场景
        /// 前端传递数据:
        /// id场景基础 id 
        /// x登陆时为下线时的x坐标，非登陆时为-1
        /// y登陆时为下线时的y坐标 ,非登陆时为-1
        /// 服务器响应数据:
        /// result:int
        /// playerList:Array[ScenePlayerVo] 场景其他玩家数据数据 
        /// </summary>
        ENTER_SCENE = 2,

        /// <summary>
        /// 主角移动
        /// 前端传递数据:
        /// x目标点x坐标
        /// y目标点y坐标 
        /// 服务器响应数据:
        /// result:int
        /// </summary>
        MOVING = 3,

        /*------------------------------推送------------------------*/
        /// <summary>
        /// 场景其他玩家进入当前场景
        /// 前端传递数据:
        /// 服务器响应数据:
        /// playerVo:ScenePlayerVo 场景其他玩家数据数
        /// </summary>
        PLAYER_ENTER_SCENE = 10001,
        /// <summary>
        /// 场景其他玩家移动
        /// 前端传递数据:
        /// 服务器响应数据:
        /// userId:Number 玩家id
        /// x目标点x坐标
        /// y目标点y坐标  
        /// </summary>
        PLAYER_MOVING = 10002,
        /// <summary>
        /// 场景其他玩家离开当前场景 
        /// 前端传递数据:
        /// 服务器响应数据:
        /// userId:Number 玩家id
        /// </summary>
        PLAYER_EXIT_SCENET = 10003,

        /// <summary>
        /// 推送进入当前场景 
        /// 服务器响应数据:
        /// id:int 场景基础 id 
        /// x:int 场景x坐标
        /// y:int 场景y坐标
        ///playerList[:List<ScenePlayerVo>] 场景其他玩家数据，无数据时为 null 
        /// </summary>
        PUSH_ENTER_SCENET = 10004,

        /// <summary>
        /// 推送玩家信息改变 
        /// 服务器响应数据:
        /// userId:int 用户id
        /// data:Object {key:value,key:value...}
        /// </summary>
        PUSH_PLAYER_VO = 10005,
    }
}
