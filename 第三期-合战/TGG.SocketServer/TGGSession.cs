using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System.Collections;
using System.Reflection;
using NewLife.Log;

namespace TGG.SocketServer
{
    /// <summary>
    /// TGG Session会话类
    /// </summary>
    public partial class TGGSession : AppSession<TGGSession, BinaryRequestInfo>
    {
        public TGGSession() { }

        public new TGGServer AppServer
        {
            get { return (TGGServer)base.AppServer; }
        }

        /// <summary>未知请求</summary>
        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            XTrace.WriteLine("{0}==>{1}", "未知请求", requestInfo.Body);
            base.HandleUnknownRequest(requestInfo);
        }
        /// <summary>异常</summary>
        protected override void HandleException(Exception e)
        {
            XTrace.WriteLine("{0}==>{1}", "异常", e.Message);
            base.HandleException(e);
        }

        /// <summary>新连接</summary>
        protected override void OnSessionStarted()
        {
            this.SessionStartedExtend();
            base.OnSessionStarted();
        }
        /// <summary>连接关闭</summary>
        protected override void OnSessionClosed(CloseReason reason)
        {
            ClearProcess();
            base.OnSessionClosed(reason);
        }

        public override void Close()
        {
            ClearProcess();
            base.Close();
        }

        /// <summary>会话结束前清空处理</summary>
        void ClearProcess()
        {
            if (!IsCloseExtend)
            {
                IsCloseExtend = true;
                SessionClosedExtend();
            }
        }
    }
}
