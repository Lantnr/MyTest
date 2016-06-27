using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 选择地图
    /// </summary>
    public class WAR_MILITARY_TRAN_MAP : IDisposable
    {
        //private static WAR_MILITARY_TRAN_MAP _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_MAP单体模式</summary>
        //public static WAR_MILITARY_TRAN_MAP GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_MAP());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 选择地图 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var idss = new List<int>();
            var cityId = session.Player.War.PlayerInCityId;
            if (cityId == 0) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var b = tg_war_carry.IsExistByCityId(cityId, time); //是否有空闲的运输队列
            if (!b) return CommonHelper.ErrorResult(ResultType.WAR_CITY_QUEUE_FULL);

            var userid = session.Player.User.id;
            var psList = tg_war_partner.GetEntityByUserId(userid);
            var maps = (new Share.War()).GetMaps(cityId, psList, userid, new List<int>(), ref idss);
            session.Player.War.Transport.OperableCityIds = idss;
            session.Player.War.Transport.Map = maps;
            return BuildData(idss);
        }

        private ASObject BuildData(List<int> ids)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "cityIds",ids},
            };
            return new ASObject(dic);
        }
    }
}
