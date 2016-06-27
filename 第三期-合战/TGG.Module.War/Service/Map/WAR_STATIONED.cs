using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 入驻 </summary>
    public class WAR_STATIONED : IDisposable
    {
        //private static WAR_STATIONED _objInstance;

        ///// <summary>WAR_STATIONED单体模式</summary>
        //public static WAR_STATIONED GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_STATIONED());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 入驻 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);//id:[int] 出征Vo的主键Id
            var userid = session.Player.User.id;
            var bq = tg_war_battle_queue.GetEntityByUseridAndId(id, userid);//FindByid(id);
            if (bq == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            if (bq.time > Common.GetInstance().CurrentTime())
                return CommonHelper.ErrorResult(ResultType.WAR_NOT_ARRIVE);

            var city = (new Share.War()).GetWarCity(bq.end_CityId, userid);
            if (city == null) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);

            var rids = bq.rid.Split(',').Select(m => Convert.ToInt64(m)).ToList();
            if (!rids.Any()) return CommonHelper.ErrorResult(ResultType.WAR_ROLE_NOEXIST);
            var roles = tg_war_role.GetEntityListByIds(rids); //合战战武将集合
            if (!roles.Any()) return CommonHelper.ErrorResult(ResultType.WAR_ROLE_NOEXIST);

            var res = new ResourceEntity();
            res = (new Share.War()).BuildResourceEntity(roles);
            res = (new Share.War()).BuildResourceEntity(bq, res);
            city = (new Share.War()).AddCityResources(res, city);

            bq.Delete();
            city.Update();

            (new Share.War()).SaveWarCityAll(city);
            (new Share.War()).RolesInitResultCity(roles, bq.end_CityId);
            (new Share.War()).SendCity(city.base_id, city.user_id);
            return CommonHelper.SuccessResult();
        }
    }
}
