using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 购买的议价次数
    /// </summary>
    public class BUSINESS_BUY_BARGAIN
    {
        private static BUSINESS_BUY_BARGAIN _objInstance;

        /// <summary>BUSINESS_BUY_BARGAIN单体模式</summary>
        public static BUSINESS_BUY_BARGAIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_BUY_BARGAIN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_BUY_BARGAIN()).Execute(session.Player.User.id, data);
        }
    }
}
