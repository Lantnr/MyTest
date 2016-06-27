using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Business;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 货物交易
    /// </summary>
    public class BUSINESS_GOODS_BUY
    {

        private static BUSINESS_GOODS_BUY ObjInstance;

        /// <summary>BUSINESS_GOODS_BUY单体模式</summary>
        public static BUSINESS_GOODS_BUY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_GOODS_BUY());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_GOODS_BUY()).Execute(session.Player.User.id, data);
        }



    }
}
