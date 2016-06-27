using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// Enum FamilyCommand.
    /// 家族指令枚举
    /// </summary>
    public enum FamilyCommand
    {
        /// <summary>
        /// 进入家族
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// family:[FamilyVo]  自己家族信息
        /// familylist：[List<FamilyListVo >]  其他家族信息集合
        /// </summary>
        FAMILY_JOIN = 1,

        /// <summary>
        /// 创建家族
        /// 前端传递数据:
        /// name:[String] 名称
        /// notice:[String] 公告
        /// 服务器响应数据:
        /// result:int 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_CREATE = 2,

        /// <summary>
        /// 申请加入
        /// 前端传递数据:
        /// id:[double] 家族主键Id
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FAMILY_APPLY = 3,

        /// <summary>
        /// 修改族徽
        /// 前端传递数据:
        /// baseId:[double] 族徽基表Id
        /// 服务器响应数据:
        /// result:int 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_UPDATE = 4,

        /// <summary>
        /// 修改家族公告
        /// 前端传递数据:
        /// notice:[String] 公告
        /// 服务器响应数据:
        /// result:int 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_NOTICE_UPDATE = 5,

        /// <summary>
        /// 捐献
        /// 前端传递数据:
        /// count:[int] 金钱数
        /// 服务器响应数据:
        /// result:int 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_DONATE = 6,

        /// <summary>
        /// 领取
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FAMILY_RECEIVE = 7,

        /// <summary>
        /// 退出家族
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FAMILY_EXIT = 8,

        /// <summary>
        /// 邀请
        /// 前端传递数据:
        /// palyername:[String] 名称
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        FAMILY_INVITE = 9,

        /// <summary>
        /// 接收邀请
        /// 前端传递数据:
        /// 服务器响应数据:
        /// invitereceive:[InviteReceiveVo]  邀请信息
        /// </summary>
        FAMILY_INVITE_RECEIVE = 10,

        /// <summary>
        /// 响应邀请
        /// 前端传递数据:
        /// state:[int]0:同意,1:拒绝
        /// id:[double] 家族主键Id
        /// userid:[double]邀请人id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_INVITE_REPLY = 11,

        /// <summary>
        /// 申请处理
        /// 前端传递数据:
        /// type:[int]  0:接受 1:拒绝
        /// userid:[double] 申请人id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// applylist:[List<FamilyApplyVo>] 申请列表信息
        /// </summary>
        FAMILY_APPLY_PROCESS = 12,

        /// <summary>
        /// 踢出家族
        /// 前端传递数据:
        /// userid:[double]家族成员id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_REMOVE = 13,

        /// <summary>
        /// 解散家族
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        FAMILY_DISSOLVE = 14,

        /// <summary>
        /// 任职
        /// 前端传递数据:
        /// baseId:[double] 家族职位基表Id
        /// userid:[double]家族成员id
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// family:[FamilyVo] 家族信息
        /// </summary>
        FAMILY_OFFICE = 15,

        /// <summary>
        /// 日志
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// log:[List(FamilyLogVo)] 日志信息
        /// </summary>
        FAMILY_LOG = 16,

        /// <summary>
        /// 申请列表
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// applylist:[FamilyApplyVo] 申请列表信息
        /// </summary>
        FAMILY_APPLY_LIST = 17,

        /// <summary>
        /// 日志推送
        /// 前端传递数据:
        /// 服务器响应数据:
        /// log:[FamilyLogVo] 日志信息
        /// </summary>
        FAMILY_LOG_PUSH = 18,

        /// <summary>
        /// 推送玩家离开家族
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        FAMILY_LEAVE_PUSH = 19,

        /// <summary>
        /// 推送玩家拒绝加入家族
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 处理结果
        /// </summary>
        FAMILY_REFUSE_PUSH = 20,
    }
}
