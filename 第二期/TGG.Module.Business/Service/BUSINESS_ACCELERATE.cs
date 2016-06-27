using System;
using System.Collections.Generic;
using System.Globalization;
using FluorineFx;
using System.Linq;
using FluorineFx.Configuration;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using tg_user = TGG.Core.Entity.tg_user;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 跑商加速
    /// </summary>
    public class BUSINESS_ACCELERATE
    {
        private static BUSINESS_ACCELERATE _objInstance;

        /// <summary>BUSINESS_ACCELERATE单体模式</summary>
        public static BUSINESS_ACCELERATE GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_ACCELERATE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        /// <summary>跑商加速</summary>
        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_ACCELERATE()).Execute(session.Player.User.id,data);
        }


    }
}
