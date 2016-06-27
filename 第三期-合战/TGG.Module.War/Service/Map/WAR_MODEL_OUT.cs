using System;
using System.Collections.Generic;
using FluorineFx;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 退出模块 </summary>
    public class WAR_MODEL_OUT : IDisposable
    {
        //private static WAR_MODEL_OUT _objInstance;

        ///// <summary>WAR_OUT_MODEL单体模式</summary>
        //public static WAR_MODEL_OUT GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MODEL_OUT());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 退出模块 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            RemoveWarInUser(userid);
            session.Player.War.PlayerInCityId = 0;
            return BulidData();
        }

        private ASObject BulidData()
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            };
            return new ASObject(dic);
        }

        private void RemoveWarInUser(Int64 userid, int count = 0)
        {
            int mid; count++;
            if (count > 10) return;
            var b = Variable.WarInUser.TryRemove(userid, out mid);
            if (!b) RemoveWarInUser(userid, count);
        }
    }
}
