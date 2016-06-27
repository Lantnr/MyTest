using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGG.Core.Enum.Type;
using TGG.Share.Event;

namespace TGG.Module.Socket
{
    public class Handle
    {
        static IBootstrap bootstrap;

        /// <summary>模块启动</summary>
        public void Start()
        {
            RunBootstrap();
        }
        /// <summary>停止</summary>
        public void Stop()
        {
            CloseBootstrap();
        }

        /// <summary>运行socket</summary>
        public void RunBootstrap()
        {
            bootstrap = BootstrapFactory.CreateBootstrap();
           var msg = string.Empty;
            if (!bootstrap.Initialize())
            {
                msg = "无法初始化SuperSocket ServiceEngine！请检查错误日志的详细信息!"; DisplayGlobal.log.Write(msg);
                return;
            }
            var result = bootstrap.Start();
            foreach (var server in bootstrap.AppServers)
            {
                msg = string.Format(server.State == ServerState.Running ? "- {0} 已经启动" : "- {0} 启动失败", server.Name);
            }
            switch (result)
            {
                case (StartResult.None):
                    msg = "无服务器配置，请检查你的配置!";DisplayGlobal.log.Write(msg);
                    return;
                case (StartResult.Success):
                {
                    //日志记录
                    (new Share.Log()).WriteServerLog((int)LogType.Run);
                    msg = "SuperSocket ServiceEngine已经启动!";
                    break;
                }
                case (StartResult.Failed):
                    msg = "无法启动SuperSocket ServiceEngine！请检查错误日志的详细信息!";DisplayGlobal.log.Write(msg);
                    return;

                case (StartResult.PartialSuccess):
                    msg = "一些服务器实例都成功启动，但其他人失败了！请检查错误日志的详细信息!";
                    break;
            }
            DisplayGlobal.log.Write(msg);
        }

        public void CloseBootstrap()
        {
            var msg = string.Format("服务器socket停止");
            DisplayGlobal.log.Write(msg);
            if (bootstrap == null) return;
            bootstrap.Stop();
            //日志记录
            (new Share.Log()).WriteServerLog((int)LogType.Stop);
        }

    }
}
