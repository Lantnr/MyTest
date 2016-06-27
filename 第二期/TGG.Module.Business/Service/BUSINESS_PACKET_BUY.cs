using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using NewLife.Log;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 增加货物格
    /// </summary>
    public class BUSINESS_PACKET_BUY
    {
        private static BUSINESS_PACKET_BUY _objInstance;

        /// <summary>BUSINESS_PACKET_BUY单体模式</summary>
        public static BUSINESS_PACKET_BUY GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_PACKET_BUY());
        }

        /// <summary>增加货物格</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodType, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_PACKET_BUY()).Execute(session.Player.User.id, data);

        }


    }
}
