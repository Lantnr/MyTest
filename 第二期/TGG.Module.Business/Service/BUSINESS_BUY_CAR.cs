using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using NewLife.Reflection;
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
    /// 购买马车指令
    /// arlen 
    /// 2014-10-15
    /// </summary>
    public class BUSINESS_BUY_CAR
    {
        private static BUSINESS_BUY_CAR _objInstance;

        /// <summary>BUSINESS_BUY_CAR单体模式</summary>
        public static BUSINESS_BUY_CAR GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_BUY_CAR());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            return (new Consume.BUSINESS_BUY_CAR()).Execute(session.Player.User.id, data);
        }
    }
}
