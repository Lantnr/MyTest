namespace TGG.Core.Queue
{
    /// <summary>
    /// Class ProtocolQueue.
    /// 协议队列
    /// </summary>
    public class ProtocolQueue
    {
        /// <summary>
        /// Vo协议
        /// </summary>
        public TGG.Core.Vo.ProtocolVo Protocol { get; set; }
        /// <summary>
        /// socket Session会话
        /// </summary>
        public dynamic Session { get; set; }
    }
}
