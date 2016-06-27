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

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 领取
    /// </summary>
    public class FAMILY_RECEIVE
    {
        private static FAMILY_RECEIVE ObjInstance;

        /// <summary>
        /// FAMILY_RECEIVE单体模式
        /// </summary>
        public static FAMILY_RECEIVE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_RECEIVE());
        }

        /// <summary>领取</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.FAMILY_RECEIVE()).Execute(session.Player.User.id, data);
        }


    }
}
