using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.AMF3;
using NewLife.Log;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using TGG.Core.AMF;
using TGG.Core.Queue;
using TGG.Core.Vo;
using TGG.SocketServer;
using System.Threading;

namespace TGG.Command.Socket
{
    /// <summary>
    /// 执行指令
    /// </summary>
    public class DataHandle : CommandBase<TGGSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get { return "FF-E3-32-D0"; }
        }

        /// <summary>执行指令</summary>
        public override void ExecuteCommand(TGGSession session, BinaryRequestInfo requestInfo)
        {
#if DEBUG
            //XTrace.WriteLine("接收数据:{0}", BitConverter.ToString(requestInfo.Body, 0, requestInfo.Body.Length));
            session.Logger.Debug(string.Format("接收数据:{0}", BitConverter.ToString(requestInfo.Body, 0, requestInfo.Body.Length)));
#endif
            try
            {
                var sobj = new SocketObjectData {session = session, requestInfo = requestInfo};

                var _temp = new CancellationTokenSource();
                new Task(m =>
                {
                    var model = m as SocketObjectData;

                    if(model==null) return;
                    if (model.requestInfo.Body.Length <= 0) return;
                    var stream = new MemoryStream(model.requestInfo.Body, 0, model.requestInfo.Body.Length);
                    var byteArray = new ByteArray(stream);
                    var dy = byteArray.ReadObject();
                    var retype = dy.GetType().Name;
                    //判断接收数据类型
                    ASObject obj;
                    switch (retype)
                    {
                        case "ASObject": { obj = (ASObject)dy; break; }
                        case "ProtocolVo": { obj = AMFConvert.ToASObject(dy); break; }
                        default: { return; }
                    }
                    var pv = AutoParseAsObject<ProtocolVo>.Parse(obj);
                    if (pv.className != "ProtocolVo") return;
                    ModuleQueue.GetInstance().LoadQueue(pv.moduleNumber, pv, model.session);
#if DEBUG
                    XTrace.WriteLine("Socket Handle接收数据Number:{0} - {1}", pv.moduleNumber, pv.commandNumber);
#endif            

                    _temp.Cancel();
                }, sobj, _temp.Token).Start();

                
            }
            catch (Exception ex) { XTrace.WriteException(ex); }
        }
    }

    class SocketObjectData
    {
        public TGGSession session { get; set; }
        public BinaryRequestInfo requestInfo { get; set; }
    }

}
