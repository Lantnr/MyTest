using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 开启商圈
    /// </summary>
    public class BUSINESS_AREA_OPEN
    {
        private static BUSINESS_AREA_OPEN _objInstance;

        /// <summary>BUSINESS_AREA_OPEN单体模式</summary>
        public static BUSINESS_AREA_OPEN GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_AREA_OPEN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_AREA_OPEN()).Execute(session.Player.User.id, data);
        }
    }
}
