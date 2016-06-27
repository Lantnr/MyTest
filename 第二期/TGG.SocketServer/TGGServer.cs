using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TGG.SocketServer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TGGServer : AppServer<TGGSession, BinaryRequestInfo>
    {
        public TGGServer() 
        :base(new DefaultReceiveFilterFactory<TGGReceiveFilter,BinaryRequestInfo>())
        { }

        internal byte[] DefaultResponse { get; set; }

        protected override void OnStopped()
        {
#if DEBUG
            Logger.Info(string.Format("{0}", "服务端停止运行"));
#endif
            this.ServerClosed();
            base.OnStopped();
        }
    }
}
