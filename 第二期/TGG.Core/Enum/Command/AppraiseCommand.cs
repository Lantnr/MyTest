using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    public enum AppraiseCommand
    {
        /// <summary>
        /// 进入评定
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///task:[List<RoleTaskVo>] 家臣任务集合（前五条为最新的任务）
        ///count:[int]刷新次数
        ///</summary>
        APPRAISE_JOIN = 1,

        /// <summary>
        /// 评定开始
        /// 前端提交数据：
        /// role:[double]家臣主将id
        ///task:[int]家臣任务主键id
        /// 服务器响应数据：
        /// result:[int] 处理结果
        ///task:<RoleTaskVo> 家臣任务
        ///推送武将属性更新
        ///</summary>
        APPRAISE_START = 2,

        /// <summary>
        /// 任务购买
        /// 服务器响应数据：
        /// result:[int] 处理结果
        /// count:[int]购买次数
        /// task:[List<RoleTaskVo>] 家臣任务集合
        /// </summary>
        TASK_BUY = 3,

      
        /// <summary>
        /// 任务刷新
        /// 服务器响应数据：
        /// result:[int] 处理结果
        /// task:[List<RoleTaskVo>] 家臣任务集合
        /// </summary>
        TASK_REFLASH = 4,

        /********  推送数据  ********/

        /// <summary>
        /// 评定结束
        ///task:[RoleTaskVo] 家臣任务
        /// </summary>
        PUSH_TASK_END = 10001,

        /// <summary>
        /// 每日刷新
        ///task:[RoleTaskVo] 家臣任务
        /// count:[int] 刷新次数
        /// </summary>
        PUSH_TASK_REFRESH = 10002,

        /// <summary>
        /// 任务删除推送
        ///id:[int] 任务主键id 
        /// </summary>
        PUSH_DELETE = 10003,
    }
}
