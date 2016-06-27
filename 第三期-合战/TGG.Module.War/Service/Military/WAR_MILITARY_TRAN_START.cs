using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Vo.War;
using TGG.SocketServer;
using TGG.Core.Entity;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Common;
using System.Threading;
using NewLife.Log;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 开始运输
    /// </summary>
    public class WAR_MILITARY_TRAN_START : IDisposable
    {
        //private static WAR_MILITARY_TRAN_START _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_START单体模式</summary>
        //public static WAR_MILITARY_TRAN_START GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_START());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 开始运输 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.War.WAR_MILITARY_TRAN_START()).Execute(session.Player.User.id, data);
        }
    }
}
