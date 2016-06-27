using System.Threading;
using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 刷新
    /// </summary>
    public class RECRUIT_REFRESH
    {
        private static RECRUIT_REFRESH ObjInstance;

        /// <summary>
        /// RECRUIT_REFRESH单体模式
        /// </summary>
        public static RECRUIT_REFRESH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new RECRUIT_REFRESH());
        }

        /// <summary> 刷新 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.RECRUIT_REFRESH()).Execute(session.Player.User.id, data);
        }
    }
}
