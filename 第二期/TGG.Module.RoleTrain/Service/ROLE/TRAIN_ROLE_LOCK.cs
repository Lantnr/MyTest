using FluorineFx;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 修行解锁
    /// </summary>
    public class TRAIN_ROLE_LOCK
    {
        private static TRAIN_ROLE_LOCK ObjInstance;

        /// <summary> TRAIN_ROLE_LOCK单体模式 </summary>
        public static TRAIN_ROLE_LOCK GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_LOCK());
        }

        /// <summary>修行解锁</summary>
        public ASObject CommandStart(int goodsType, TGGSession session, ASObject data)
        {
            return (new Consume.TRAIN_ROLE_LOCK()).Execute(session.Player.User.id, data);
        }
    }
}
