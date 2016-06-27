using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Global;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Queue;
using NewLife.Log;
using TGG.Core.XML;
using TGG.Share.Event;

namespace TGG.Module.Messages
{
    /// <summary>
    /// 模块处理类
    /// </summary>
    public class Handle
    {
        public static Handle ObjInstance;

        /// <summary>Handle单例模式</summary>
        public static Handle GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Handle());
        }

        CancellationTokenSource _token;

        /// <summary>模块启动</summary>
        public void Start(int count)
        {
#if DEBUG
            XTrace.WriteLine("进入邮件模块 当前计数{0}", count);
#endif
            DisplayGlobal.log.Write(string.Format("进入邮件模块 当前计数{0}", count));

            _token = new CancellationTokenSource();
            var _module = CommonHelper.GetCDTSTM(this.GetType().Namespace, count, true);
            ConcurrentQueueExtensions.Clear(Variable.GMQ_MESSAGES);
            try
            {
                Task.Factory.StartNew(m =>
                {
                    var key = m as XmlModule;
                    if (Variable.CDTSTM.Count <= 0 || key == null) return;
                    while (Variable.CDTSTM[key])
                    {
                      if (Variable.GMQ_MESSAGES.Count > 0)
                      {
                          dynamic data;
                          Variable.GMQ_MESSAGES.TryDequeue(out data);
                          //Command.GetInstance().Service(data);
                          var obj = data as ProtocolQueue;
                          var _temp = new CancellationTokenSource();
                          Task.Factory.StartNew(n =>
                          {
                              var model = n as ProtocolQueue;
                              Command.GetInstance().Service(model);
                              _temp.Cancel();
                          }, obj, _temp.Token);
                      }
                      //else
                      Thread.Sleep(1);
                  }
                  CommonHelper.GetCDTSTMRemove(this.GetType().Namespace, count);
                  _token.Cancel();
              },_module, _token.Token);
            }
            catch
            {
                Start(count);
            }
        }

        /// <summary>停止</summary>
        public void Stop(int count)
        {
#if DEBUG
            XTrace.WriteLine("退出邮件模块 当前计数{0}", count);
#endif
            DisplayGlobal.log.Write(string.Format("退出邮件模块 当前计数{0}", count));

            CommonHelper.GetCDTSTMUpdate(this.GetType().Namespace, count);
        }
    }
}
