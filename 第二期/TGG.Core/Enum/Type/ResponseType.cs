namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// ResponseType Socket通讯服务端返回结果类型
    /// </summary>
    public enum ResponseType
    {
        /// <summary>成功 </summary>
        TYPE_SUCCESS = 0,

        /// <summary>无效的验证码(返回后忽略响应)</summary>
        TYPE_INVALIDATE_VERIFICATION_CODE = 1,

        /// <summary>无操作权限(返回后忽略响应) </summary>
        TYPE_NO_PERMISSIONS = 2,

        /// <summary>解析错误(返回后忽略响应) </summary>
        TYPE_RESOLVE_ERROR = 3,

        /// <summary>指令请求太快</summary>
        TYPE_COMMAND_FAST = 4,

        /// <summary>登陆人数过多</summary>
        TYPE_USER_RESTRICTION = 100,

        /// <summary>未知错误</summary>
        TYPE_UNKNOW_ERROR = 200,
    }
}
