using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Home
{
    /// <summary>
    /// 进入内政策略
    /// </summary>
    public class WAR_HOME_TACTICS_JOIN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_HOME_TACTICS_JOIN()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_HOME_TACTICS_JOIN _objInstance;

        ///// <summary>WAR_HOME_TACTICS_JOIN单体模式</summary>
        //public static WAR_HOME_TACTICS_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_HOME_TACTICS_JOIN());
        //}

        /// <summary> 进入内政策略 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var temp = tg_war_home_tactics.GetEntityByUseridAndTime(userid);
            return Common.GetInstance().BuildData(temp);
        }
    }
}
