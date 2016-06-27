using System;
using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 解锁
    /// </summary>
    public class WAR_SKYCITY_UNLOCK:IDisposable
    {
        //private static WAR_SKYCITY_UNLOCK _objInstance;

        ///// <summary> WAR_SKYCITY_UNLOCK单体模式 </summary>
        //public static WAR_SKYCITY_UNLOCK GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_UNLOCK());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 解锁</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_SKYCITY_UNLOCK()).Execute(session.Player.User.id, data);
        }
    }
}
