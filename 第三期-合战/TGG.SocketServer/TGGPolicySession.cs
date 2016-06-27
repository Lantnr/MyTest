using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.SocketServer
{
    public class TGGPolicySession : AppSession<TGGPolicySession>
    {
        protected override void OnSessionStarted()
        {
            var policy = "<cross-domain-policy><site-control permitted-cross-domain-policies=\"all\"/><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0";
            var buffer = Encoding.UTF8.GetBytes(policy.ToCharArray());
            base.Send(buffer, 0, buffer.Length);
            base.OnSessionStarted();
        }
    }
}
