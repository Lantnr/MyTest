using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 运输开锁
    /// </summary>
    public class WAR_MILITARY_TRAN_LOCK : IDisposable
    {
        //private static WAR_MILITARY_TRAN_LOCK _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_LOCK单体模式</summary>
        //public static WAR_MILITARY_TRAN_LOCK GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_LOCK());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 运输开锁 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_MILITARY_TRAN_LOCK()).Execute(session.Player.User.id, data);
        }
    }
}
