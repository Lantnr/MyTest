using System;
using System.Collections.Generic;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 外交同盟
    /// </summary>
    public class WAR_DIPLOMACY_JOIN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DIPLOMACY_JOIN()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DIPLOMACY_JOIN _objInstance;

        ///// <summary>WAR_DIPLOMACY_JOIN单体模式</summary>
        //public static WAR_DIPLOMACY_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DIPLOMACY_JOIN());
        //}

        /// <summary> 外交同盟 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var list = view_war_partner.GetEntityByUserId(userid).ToList();
            return BulidData(list);
        }

        /// <summary> 组装数据 </summary>
        public ASObject BulidData(List<view_war_partner> list)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "list",Common.GetInstance().ToDiplomacyVos(list)},
            };
            return new ASObject(dic);
        }
    }
}
