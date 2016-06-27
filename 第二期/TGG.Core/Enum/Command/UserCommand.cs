namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// UserCommand 用户信息相关命令
    /// </summary>
    public enum UserCommand
    {
        /// <summary>
        /// 登录服务器 
        /// 前端传递数据:
        /// userName:String 用户名
        /// server:String 服务器标识
        /// 服务器响应数据:
        /// result:int 处理结果
        /// userInfoVo:UserInfoVo 用户信息，未创建角色为 null
        /// </summary>
        USER_LOGIN = 1,

        /// <summary>
        /// 前端传递数据:
        /// 创建角色
        /// userName:String 用户名
        /// server:String 服务器标识
        /// sex:int 性别   SexType 1:男，2：女，3：未知
        /// vocation:int 职业 VocationType 1:法师
        /// playerName:String 角色名
        /// 服务器响应数据:
        /// result:int 处理结果
        /// userInfoVo:UserInfoVo 用户信息
        /// </summary>
        USER_CREATE = 2,

        /// <summary>
        /// 模块激活指令
        /// 前端传递数据:
        /// id:[int] 模块号
        /// 服务器响应数据:
        /// result:int 处理结果
        /// </summary>
        USER_MODULE_ACTIVETION = 3,

        /// <summary>
        /// 身份俸禄领取指令
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:int 处理结果
        /// state:int 领取状态 0:未领取 1:领取
        /// </summary>
        USER_SALARY = 4,

        /// <summary>
        /// 活动开放请求
        /// listvo:ActivityOpenVo集合
        /// </summary>
        ACTIVITY_OPEN = 6,

        /// <summary>
        /// 消费
        /// 前端传递数据:
        /// goodsType:int GoodsType 物品类型
        /// module:int ModuleNumber 模块号
        /// cmd:int 命令号
        /// data:Object 业务数据
        /// 服务器响应数据:
        /// result:int 处理结果
        /// module:int ModuleNumber 模块号
        /// cmd:int 命令号
        /// data:Object 业务数据
        /// </summary>
        CONSUME = 100,

        /**********************************************推送**********************************************/
        /// <summary>
        /// 奖励更新，包括增加和减少
        /// 服务器响应数据:
        /// rewards:Array 奖励 RewardVo 数组
        /// </summary>
        REWARDS = 10001,

        /// <summary>
        /// 心跳保持连接
        /// </summary>
        HEARTBEAT = 99999,

        /// <summary>
        /// 防沉迷推送
        /// </summary>
        PUSH_ONLINE_TIME = 10002,

        /// <summary>
        /// VIP推送
        /// </summary>
        USER_VIP_PUSH = 5,

        /// <summary>
        /// 活动推送
        /// id:功能开放表id
        /// state:0:活动开始 1：活动结束
        /// </summary>
        ACTIVITY_PUSH = 10003,
    }
}
