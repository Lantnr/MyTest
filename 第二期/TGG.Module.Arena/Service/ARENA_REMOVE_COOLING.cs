using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 清除冷却
    /// </summary>
    public class ARENA_REMOVE_COOLING
    {
        private static ARENA_REMOVE_COOLING ObjInstance;

        /// <summary>ARENA_REMOVE_COOLING单体模式</summary>
        public static ARENA_REMOVE_COOLING GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ARENA_REMOVE_COOLING());
        }

        /// <summary> 清除冷却 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.ARENA_REMOVE_COOLING()).Execute(session.Player.User.id, data);
        }
    }
}
