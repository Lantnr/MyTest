using System.Threading;
using FluorineFx;
using NewLife.Log;
using System;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Queue;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.TaskFinish
{
    /// <summary>
    /// 模块指令类
    /// </summary>
    public class Command : IDisposable
    {

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~Command()
        {
            Dispose();
        }

        #endregion

        //public static Command ObjInstance;
        ///// <summary>Command单例模式</summary>
        //public static Command GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new Command());
        //}

        /// <summary>指令业务</summary>
        public void Service(ProtocolQueue model)
        {
#if DEBUG
            XTrace.WriteLine("commandNumber:{0}", model.Protocol.commandNumber);
#endif
            try
            {
                if (model.Session == null) return;
                TGGSession session = model.Session;

                var key = string.Format("{0}_{1}_{2}", session.Player.User.id, model.Protocol.moduleNumber, model.Protocol.commandNumber);
                if (Variable.CCI.ContainsKey(key))
                {
                    //session.SendData(session.InitProtocol(model, (int)ResponseType.TYPE_COMMAND_FAST, null));
                    return;
                }
                Variable.CCI.TryAdd(key, true);

                var cs = new CommandSwitch();
                var data = cs.Switch(model.Protocol.commandNumber, session, model.Protocol.data);
                cs.Dispose();
                if (data == null) { XTrace.WriteLine("指令错误:{0}", key); RemoveCommand(key); return; }
                var pv = session.InitProtocol(model, (int)ResponseType.TYPE_SUCCESS, data);
                session.SendData(pv);
                RemoveCommand(key);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                if (model.Session != null) model.Session.CommandEnd(model);
            }
        }

        /// <summary>移除模块</summary>
        /// <param name="key">key</param>
        private bool RemoveCommand(string key)
        {
            //Thread.Sleep(300);
            bool b;
            return Variable.CCI.TryRemove(key, out b);
        }
    }
}
