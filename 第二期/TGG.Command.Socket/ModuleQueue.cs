using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using TGG.Core.Enum;
using TGG.Core.Global;
using TGG.Core.Queue;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Command.Socket
{
    /// <summary>模块队列类</summary>
    public class ModuleQueue
    {
        private static ModuleQueue ObjInstance;

        /// <summary>ModuleQueue单体模式</summary>
        public static ModuleQueue GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ModuleQueue());
        }

        /// <summary>加载模块到队列</summary>
        /// <param name="moduleNumber">模块号</param>
        /// <param name="protocol">协议</param>
        /// <param name="session">会话</param>
        public void LoadQueue(int moduleNumber, ProtocolVo protocol, TGGSession session)
        {
            //枚举反射
            var fields_enum = typeof(ModuleNumber).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var item_enum in fields_enum)
            {
                var value = Convert.ToInt32(item_enum.GetValue(null));
                if (value != moduleNumber) continue;
                var firstOrDefault = Variable.LMMQ.FirstOrDefault(m => m.Name == item_enum.Name);
                if (firstOrDefault == null) continue;
                var v = firstOrDefault.Value;
                var fields_queue = typeof(Variable).GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (var q in from item_queue in fields_queue let queue = item_queue.GetValue(null) where v == item_queue.Name select queue as ConcurrentQueue<dynamic>)
                {
                    if (q != null) q.Enqueue(new ProtocolQueue { Protocol = protocol, Session = session });
                    return;
                }
            }
        }
    }
}
