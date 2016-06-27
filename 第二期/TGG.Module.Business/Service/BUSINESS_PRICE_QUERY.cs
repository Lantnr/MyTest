using System.Linq;
using FluorineFx;
using FluorineFx.Util;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 市价情报
    /// </summary>
    public class BUSINESS_PRICE_QUERY
    {
        private static BUSINESS_PRICE_QUERY ObjInstance;

        /// <summary>BUSINESS_PRICE_QUERY单体模式</summary>
        public static BUSINESS_PRICE_QUERY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_PRICE_QUERY());
        }

        /// <summary>市价情报指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_PRICE_QUERY", "市价情报");
#endif
            var areaid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);

            var player = session.Player.User.CloneEntity();
            var tings = tg_user_ting.GetEntityQueryedTing(player.id, areaid);
            var tingsIds = tings.Select(m => m.ting_id).ToList();
            return new ASObject(Common.GetInstance().BuildIds((int)ResultType.SUCCESS, tingsIds));
        }
    }
}
