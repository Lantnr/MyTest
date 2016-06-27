using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    public enum WorkCommand
    {
        /// <summary>
        /// 拉取工作
        /// </summary>
        WORK_JOIN = 1,

        /// <summary>
        /// * 工作任务更新提交
        /// 前端传递数据:
        // * type:int 1:更新任务数据 2:提交任务(领取奖励)
        // * npcId:int npc 基础 id，没有相关 npc 为 0
        // * data:String 与任务步骤格式相同
        // * taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// *result:[int] 返回状态值
        /// *taskVo: WorkVo 工作任务
        /// </summary>
        WORK_UPDATE = 3,

        /// <summary>
        /// 工作任务接受
        /// 前端传递数据:
        /// task:[double] 任务主键id
        /// 服务器响应数据:
        /// * result:[int] 返回状态值  
        /// </summary>
        WORK_ACCEPT = 5,

        /// <summary>
        /// 工作任务放弃
        /// 前端传递数据:
        /// task:[double] 任务主键id
        /// 服务器响应数据:
        /// * result:[int] 返回状态值  
        /// </summary>
        WORK_DROP = 10,


        /// <summary>
        /// 任务推送
        /// 前端传递数据:
        /// 服务器响应数据:
        ///*0:[TaskVo]当前主线任务
        ///*1:List<WorkVo> 工作任务数组
        /// </summary>
        WORK_PUSH = 10001,

        /// <summary>
        /// 工作任务更新推送
        /// task:WorkVo工作任务vo
        /// </summary> 
        WORK_PUSH_UPDATE = 10002,

        /// <summary>
        /// 工作任务删除推送
        /// id:[double]任务主键id
        /// </summary>
        WORK_PUSH_DELETE = 10003,

        /// <summary>
        /// 取消守护任务
        /// 前端传递数据:
        /// 服务器响应数据:
        /// * result:[int] 返回状态值  
        WORK_CANCEL = 4001,

        /// <summary>
        /// 战斗推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// type：[int] 战斗结果   0：失败  1：成功
        /// fight:[FightVo] 战斗Vo
        /// </summary>
        WORK_FIGHT_PUSH = 10004,

        /// <summary>
        /// 请求是否触发拦路
        /// 前端传递数据:
        /// taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// state:0 否  1.是
        /// </summary>
        WORK_IS_FIGHT = 2001,

        /// <summary>
        /// 请求战斗
        /// 前端传递数据:
        /// taskId:[double] 用户任务id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        WORK_FIGHT = 3001,

        /// <summary>
        /// 宝物搜寻工作任务
        /// 前端传递数据:
        /// 服务器响应数据:
        /// * result:[int] 返回状态值
        /// taskVo:WorkVo
        /// </summary>
        WORK_SEARCH = 7,

        /// <summary>
        ///  触发站岗选择
        /// 前端传递数据:
        /// id:int 场景id
        /// type:int 0:强行进入，1:花钱
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        WORK_SELECT_DEFEND = 9,

        /// <summary>
        /// 检测是否触发站岗
        /// 前端传递数据:
        /// id:int 场景id
        /// 服务器响应数据:
        ///  result:[int] 返回状态值
        ///  type:int 0:未触发，1:触发
        /// </summary>
        WORK_CHECK_DEFEND = 8,



    }
}
