using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{

    /// <summary>
    /// 补充货物
    /// </summary>
    public class BUSINESS_GOODS_ADD
    {
        private static BUSINESS_GOODS_ADD _objInstance;

        /// <summary>BUSINESS_GOODS_ADD单体模式</summary>
        public static BUSINESS_GOODS_ADD GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_GOODS_ADD());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_GOODS_ADD()).Execute(session.Player.User.id, data);
        }
    }
}
