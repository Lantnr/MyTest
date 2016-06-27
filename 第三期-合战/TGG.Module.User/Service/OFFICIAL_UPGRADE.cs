using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary> 官职升级 </summary>
    public class OFFICIAL_UPGRADE:IDisposable
    {
         #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~OFFICIAL_UPGRADE()
        {
            Dispose();
        }
    
        #endregion

        //private static OFFICIAL_UPGRADE ObjInstance;
        ///// <summary>OFFICIAL_UPGRADE单例模式</summary>
        //public static OFFICIAL_UPGRADE GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new OFFICIAL_UPGRADE());
        //}
        /// <summary>官职升级</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.OFFICIAL_UPGRADE()).Execute(session.Player.User.id, data);
        }
    }
}
