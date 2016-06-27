using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Copy
{
    /// <summary>
    /// 雇兵
    /// </summary>
    public class WAR_COPY_HIRE_SOLDIERS : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_COPY_HIRE_SOLDIERS()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_COPY_HIRE_SOLDIERS _objInstance;

        ///// <summary>WAR_COPY_HIRE_SOLDIERS单体模式</summary>
        //public static WAR_COPY_HIRE_SOLDIERS GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_COPY_HIRE_SOLDIERS());
        //}

        /// <summary> 雇兵 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_COPY_HIRE_SOLDIERS()).Execute(session.Player.User.id, data);
        }
    }
}
