using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using NewLife.Log;
using System;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 调查
    /// </summary>
    public class BUSINESS_PRICE_INFO
    {
        private static BUSINESS_PRICE_INFO ObjInstance;

        /// <summary>BUSINESS_PRICE_INFO单体模式</summary>
        public static BUSINESS_PRICE_INFO GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_PRICE_INFO());
        }

        /// <summary>调查</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_COIN, session, data);
        }

        public ASObject CommandStart(int goodType, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_PRICE_INFO()).Execute(session.Player.User.id, data);
        }

    }
}
