using System.Threading;
using NewLife.Log;
using System;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Queue;
using TGG.SocketServer;

namespace TGG.Module.Siege
{
    /// <summary>
    /// 模块指令类
    /// </summary>
    public class Command
    {
        public static Command ObjInstance;

        /// <summary>Command单例模式</summary>
        public static Command GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Command());
        }

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
                    session.SendData(session.InitProtocol(model, (int)ResponseType.TYPE_COMMAND_FAST, null));
                    return;
                }
                Variable.CCI.TryAdd(key, true);

                var data = CommandSwitch.GetInstance().Switch(model.Protocol.commandNumber, session, model.Protocol.data);
                var pv = session.InitProtocol(model, (int)ResponseType.TYPE_SUCCESS, data);
                session.SendData(pv);
                RemoveCommand(key);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                if (model.Session != null)
                    model.Session.CommandEnd(model);
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
