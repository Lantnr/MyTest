using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 查看
    /// arlen
    /// </summary>
    public class WAR_HOME_VIEW : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        //private static WAR_HOME_VIEW _objInstance;

        ///// <summary>WAR_HOME_VIEW单体模式</summary>
        //public static WAR_HOME_VIEW GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_HOME_VIEW());
        //}

        /// <summary> 查看开始指令处理 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return new ASObject();
        }

    }
}
