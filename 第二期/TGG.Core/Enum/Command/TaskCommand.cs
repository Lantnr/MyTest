namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// Enum TaskCommand
    /// 任务指令枚举
    /// </summary>
    public enum TaskCommand
    {
        /// <summary>
        /// 登录时查询任务
        /// 前端传递数据:
        /// 服务器响应数据:
        /// *result:[int] 返回状态值
        /// *mainTask: TaskVo 当前用户主线任务
        /// *vocationTask: [List<VocationTaskVo>] 职业任务集合
        /// *count:剩余次数
        /// </summary>
        TASK_JOIN = 1,

        /// <summary>
        /// 完成任务
        /// 前端传递数据:
        /// *type:int 0:接受任务 1:完成任务 2:提交任务
        /// *npcId:int 对话 npc 基础 id
        /// *taskid:[double] 用户任务主键id
        /// 服务器响应数据:
        /// *result:[int] 返回状态值
        /// *mainTask: TaskVo 当前用户主线任务
        /// </summary>
        TASK_FINISH = 2,

        /// <summary>
        /// * 职业任务更新提交
        /// 前端传递数据:
        // * type:int 1:更新任务数据 2:提交任务(领取奖励)
        // * npcId:int npc 基础 id，没有相关 npc 为 0
        // * data:String 与任务步骤格式相同
        // * taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// *result:[int] 返回状态值
        /// *taskVo: VocationTaskVo 职业任务
        /// </summary>
        TASK_VOCATION_UPDATE = 3,

        /// <summary>
        /// 职业任务刷新
        /// 前端传递数据:
        /// * result:[int] 返回状态值
        /// * vocationTask: [List<VocationTaskVo>] 职业任务集合
        /// </summary>
        TASK_VOCATION_REFRESH = 6,

        /// <summary>
        /// 职业任务接受
        /// 前端传递数据:
        /// task:[double] 任务主键id
        /// 服务器响应数据:
        /// * result:[int] 返回状态值  
        /// </summary>
        TASK_VOCATION_ACCEPT = 5,

        /// <summary>
        /// 职业任务刷新
        /// 前端传递数据:
        /// 服务器响应数据:
        /// * result:[int] 返回状态值
        /// * vocationTask: [List<VocationTaskVo>] 职业任务集合
        /// * count:剩余次数         
        /// </summary>
        TASK_VOCATION_BUY = 4,

        /// <summary>
        /// 宝物搜寻职业任务
        /// 前端传递数据:
        /// 服务器响应数据:
        /// * result:[int] 返回状态值
        /// taskVo:VocationTaskVo
        /// </summary>
        TASK_SEARCH = 7,

        /// <summary>
        /// 检测是否触发站岗
        /// 前端传递数据:
        /// id:int 场景id
        /// 服务器响应数据:
        ///  result:[int] 返回状态值
        ///  type:int 0:未触发，1:触发
        /// </summary>
        TASK_CHECK_DEFEND = 8,

        /// <summary>
        ///  触发站岗选择
        /// 前端传递数据:
        /// id:int 场景id
        /// type:int 0:强行进入，1:花钱
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        TASK_SELECT_DEFEND = 9,

        /// <summary>
        /// 任务推送
        /// 前端传递数据:
        /// 服务器响应数据:
        ///*0:[TaskVo]当前主线任务
        ///*1:List<VocationTaskVo> 职业任务数组
        /// </summary>
        TASK_PUSH = 10001,

        /// <summary>
        /// 职业任务更新推送
        /// task:VocationTaskVo职业任务vo
        /// </summary>
        TASK_PUSH_UPDATE = 10002,

        /// <summary>
        /// 职业任务删除推送
        /// id:[double]任务主键id
        /// </summary>
        TASK_PUSH_DELETE = 10003,


        /// <summary>
        /// 取消守护任务
        /// 前端传递数据:
        /// 服务器响应数据:
        /// * result:[int] 返回状态值  
        TASK_CANCEL = 4001,

        /// <summary>
        /// 战斗推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// type：[int] 战斗结果   0：失败  1：成功
        /// fight:[FightVo] 战斗Vo
        /// </summary>
        TASK_FIGHT_PUSH = 10004,

        /// <summary>
        /// 请求是否触发拦路
        /// 前端传递数据:
        /// taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// state:0 否  1.是
        /// </summary>
        TASK_IS_FIGHT = 2001,

        /// <summary>
        /// 请求战斗
        /// 前端传递数据:
        /// taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        TASK_FIGHT = 3001,

        /// <summary>
        /// 领取家臣道具任务推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// propId：[int] 道具基表id
        /// </summary>
        WORK_ROLE_GET_PUSH = 100010,
    }
}
