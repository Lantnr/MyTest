using System;
using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Copy
{
    /// <summary>
    /// 鼓舞士气
    /// </summary>
    public class WAR_COPY_MORALE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_COPY_MORALE()
        {
            Dispose();
        }

        #endregion

        /// <summary> 鼓舞士气 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_COPY_MORALE()).Execute(session.Player.User.id, data);
        }
    }
}
