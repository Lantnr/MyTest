using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
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
    /// 创建家族
    /// </summary>
    public class FAMILY_CREATE
    {
        public static FAMILY_CREATE ObjInstance;

        /// <summary>
        /// FAMILY_CREATE单体模式
        /// </summary>
        /// <returns></returns>
        public static FAMILY_CREATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_CREATE());
        }
        private int result;
        /// <summary>创建家族</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.FAMILY_CREATE()).Execute(session.Player.User.id, data);
        }       
    }
}
