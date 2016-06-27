using System.Transactions;
using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将抽取
    /// </summary>
    public class RECRUIT_GET
    {
        private static RECRUIT_GET _objInstance;

        /// <summary>
        /// RECRUIT_GET单体模式
        /// </summary>
        public static RECRUIT_GET GetInstance()
        {
            return _objInstance ?? (_objInstance = new RECRUIT_GET());
        }

        /// <summary> 武将抽取 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.RECRUIT_GET()).Execute(session.Player.User.id, data);
        }
    }
}
