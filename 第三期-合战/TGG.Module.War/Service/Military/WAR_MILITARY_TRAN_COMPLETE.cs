using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary> 运输加速 </summary>
    public class WAR_MILITARY_TRAN_COMPLETE : IDisposable
    {
        //private static WAR_MILITARY_TRAN_COMPLETE _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_COMPLETE单体模式</summary>
        //public static WAR_MILITARY_TRAN_COMPLETE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_COMPLETE());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 运输加速 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_MILITARY_TRAN_COMPLETE()).Execute(session.Player.User.id, data);
        }
    }
}
