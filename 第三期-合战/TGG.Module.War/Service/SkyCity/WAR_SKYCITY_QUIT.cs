using System;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 退出天守阁
    /// </summary>
    public class WAR_SKYCITY_QUIT:IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        //private static WAR_SKYCITY_QUIT _objInstance;

        ///// <summary> WAR_SKYCITY_QUIT单体模式 </summary>
        //public static WAR_SKYCITY_QUIT GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_QUIT());
        //}

        /// <summary> 退出天守阁</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_QUIT", "退出天守阁");
#endif
            session.Player.War.Status = 0;  //玩家退出天守阁
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
        }
    }
}
