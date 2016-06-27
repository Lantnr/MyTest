using System;
using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 筑城 </summary>
    public class WAR_CITY_BUILD : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_CITY_BUILD()
        {
            Dispose();
        }
    
        #endregion


        /// <summary> 筑城 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_CITY_BUILD()).Execute(session.Player.User.id, data);
        }


    }
}
