using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 进入天守阁
    /// </summary>
    public class WAR_SKYCITY_ENTER : IDisposable
    {
        //private static WAR_SKYCITY_ENTER _objInstance;

        ///// <summary> WAR_SKYCITY_ENTER单体模式 </summary>
        //public static WAR_SKYCITY_ENTER GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_ENTER());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 进入天守阁</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_ENTER", "进入天守阁");
#endif
            var userId = session.Player.User.id;
            var cityId = session.Player.War.PlayerInCityId;  //据点基表id

            var city = (new Share.War()).GetWarCity(cityId, userId);
            if (city == null) return CommonHelper.ErrorResult((int)ResultType.WAR_CITY_NOEXIST);  //据点信息不存在

            var role = tg_war_role.GetListByCityIdAndType(city.base_id, userId, (int)WarRoleType.PLAYER);
            var listdata = new List<ASObject>();

            if (role.Any())
            {
                listdata.AddRange(role.Select(item => AMFConvert.ToASObject(EntityToVo.ToSkyCityVo(item))));
            }
            session.Player.War.Status = 1;  //玩家进入天守阁
            return new ASObject(Common.GetInstance().EnterData((int)ResultType.SUCCESS, city.interior_bar, city.levy_bar, city.train_bar, listdata));
        }
    }
}
