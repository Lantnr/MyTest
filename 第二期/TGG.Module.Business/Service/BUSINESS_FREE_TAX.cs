﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 免税
    /// </summary>
    public class BUSINESS_FREE_TAX
    {

        private static BUSINESS_FREE_TAX _objInstance;

        /// <summary>BUSINESS_FREE_TAX单体模式</summary>
        public static BUSINESS_FREE_TAX GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_FREE_TAX());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_FREE_TAX()).Execute(session.Player.User.id, data);
        }

    }
}
