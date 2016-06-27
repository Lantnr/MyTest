using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将雇佣
    /// </summary>
    public class ROLE_HIRE
    {
        private static ROLE_HIRE _objInstance;

        /// <summary>ROLE_HIRE单体模式</summary>
        public static ROLE_HIRE GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_HIRE());
        }

        /// <summary> 武将雇佣 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.ROLE_HIRE()).Execute(session.Player.User.id, data);
        }
    }
}
