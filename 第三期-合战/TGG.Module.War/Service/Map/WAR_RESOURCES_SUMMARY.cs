using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    public class WAR_RESOURCES_SUMMARY : IDisposable
    {
    //    private static WAR_RESOURCES_SUMMARY _objInstance;

    //    /// <summary>WAR_RESOURCES_SUMMARY单体模式</summary>
    //    public static WAR_RESOURCES_SUMMARY GetInstance()
    //    {
    //        return _objInstance ?? (_objInstance = new WAR_RESOURCES_SUMMARY());
    //    }

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 出征资源结算 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);//id:[int] 出征Vo的主键Id
            var bq = tg_war_battle_queue.GetEntityByUseridAndId(id, session.Player.User.id); //tg_war_battle_queue.FindByid(id);
            return bq == null ? CommonHelper.ErrorResult(ResultType.NO_DATA) : (new Share.War()).SettlementResource(bq);
        }
    }
}
