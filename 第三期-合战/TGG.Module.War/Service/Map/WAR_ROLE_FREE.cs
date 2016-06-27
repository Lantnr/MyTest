using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    public class WAR_ROLE_FREE : IDisposable
    {
        //private static WAR_ROLE_FREE _objInstance;

        ///// <summary>WAR_ROLE_FREE单体模式</summary>
        //public static WAR_ROLE_FREE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_ROLE_FREE());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 解救（武将一览） </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return new TGG.Module.Consume.War.WAR_ROLE_FREE().Execute(session.Player.User.id, data);
        }
    }
}
