using System.Linq;
using FluorineFx;
using FluorineFx.Util;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 进入町
    /// </summary>
    public class BUSINESS_TING_ENTER
    {
        private static BUSINESS_TING_ENTER ObjInstance;

        /// <summary>BUSINESS_TING_ENTER单体模式</summary>
        public static BUSINESS_TING_ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_TING_ENTER());
        }

        /// <summary>进入町指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine(string.Format("{0}:{1}", "BUSINESS_TING_ENTER", "进入町"));
#endif
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());

            if (!tg_car.GetStopTing(session.Player.User.id,id))
                return CommonHelper.ErrorResult((int) ResultType.BUSINESS_RUN_TING_ERROR);
            var userinfo = session.Player.User.CloneEntity();
            session.Player.Order.ting_base_id = id;   //更新Session中Order信息

            return new ASObject(Common.GetInstance().EnterTing(userinfo.id, id));
        }
    }
}