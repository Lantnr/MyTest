using FluorineFx;
using System;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 退出跑商指令
    /// </summary>
    public class BUSINESS_EXIT
    {
        private static BUSINESS_EXIT ObjInstance;

        /// <summary>BUSINESS_EXIT单体模式</summary>
        public static BUSINESS_EXIT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_EXIT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "BUSINESS_EXIT", "跑商退出指令");
#endif
                return Common.GetInstance().BuildData((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                session.Logger.Error(ex.Message);
                return new ASObject();
            }
        }
    }
}
