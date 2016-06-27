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

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 增加挑战次数
    /// </summary>
    public class ARENA_DEKARON_ADD
    {
        private static ARENA_DEKARON_ADD ObjInstance;

        /// <summary>ARENA_DEKARON_ADD单体模式</summary>
        public static ARENA_DEKARON_ADD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ARENA_DEKARON_ADD());
        }

        /// <summary> 增加挑战次数 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.ARENA_DEKARON_ADD()).Execute(session.Player.User.id, data);
        }
    }
}
