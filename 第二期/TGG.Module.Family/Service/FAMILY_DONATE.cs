using System.Net.Configuration;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Reflection;
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
    /// 捐献
    /// </summary>
    public class FAMILY_DONATE
    {
        public static FAMILY_DONATE ObjInstance;

        /// <summary>
        /// FAMILY_DONATE单体模式
        /// </summary>
        public static FAMILY_DONATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_DONATE());
        }

        private int result;
        /// <summary>捐献</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.FAMILY_DONATE()).Execute(session.Player.User.id, data);
        }
    }
}
