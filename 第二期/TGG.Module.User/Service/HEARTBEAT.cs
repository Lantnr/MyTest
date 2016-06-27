using FluorineFx;
using NewLife.Log;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 心跳保持连接类
    /// </summary>
    public class HEARTBEAT
    {
        private static HEARTBEAT ObjInstance;

        /// <summary>HEARTBEAT单例模式</summary>
        public static HEARTBEAT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new HEARTBEAT());
        }

        /// <summary>心跳保持连接</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "HEARTBEAT", "心跳");
#endif           
            return new ASObject();
        }
    }
}
