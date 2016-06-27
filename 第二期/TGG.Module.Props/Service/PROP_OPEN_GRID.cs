using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    ///开启格子
    /// </summary>
    public class PROP_OPEN_GRID
    {
        private static PROP_OPEN_GRID ObjInstance;

        /// <summary>OPEN_GRID单体模式</summary>
        public static PROP_OPEN_GRID GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PROP_OPEN_GRID());
        }

        /// <summary>开启格子</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.PROP_OPEN_GRID()).Execute(session.Player.User.id, data);
        }
    }
}
