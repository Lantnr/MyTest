using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 据点规模提升
    /// </summary>
    public class WAR_UPDATE_CITY : IDisposable
    {
        //private static WAR_UPDATE_CITY _objInstance;

        ///// <summary>WAR_UPDATE_CITY单体模式</summary>
        //public static WAR_UPDATE_CITY GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_UPDATE_CITY());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 据点规模提升 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);  //id:[double] 据点基表id
            var userid = session.Player.User.id;
            var off = session.Player.User.office;
            var extend = session.Player.UserExtend;

            var city = (new Share.War()).GetWarCity(id, userid);//tg_war_city.GetEntityByBaseId(id);
            if (city == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            if (city.size >= 5) return CommonHelper.ErrorResult(ResultType.WAR_SIZE_MAX);

            var size = city.size + 1;
            var basesize = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == size);
            var baseoffice = Variable.BASE_OFFICE.FirstOrDefault(m => m.id == off);
            if (basesize == null || baseoffice == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var count = extend.war_total_own + basesize.own - city.own;
            if (count > baseoffice.total_own) return CommonHelper.ErrorResult(ResultType.WAR_NOT_OWN);

            if (basesize.boom > city.boom) return CommonHelper.ErrorResult(ResultType.WAR_BOOM_DEFICIENCY);
            if (basesize.strong > city.strong) return CommonHelper.ErrorResult(ResultType.WAR_STRONG_DEFICIENCY);
            if (basesize.peace > city.peace) return CommonHelper.ErrorResult(ResultType.WAR_PEACE_DEFICIENCY);

            city.size = size;
            city.own = basesize.own;
            extend.war_total_own = count;
            city.Update();
            extend.Update();
            session.Player.UserExtend = extend;
            (new Share.War()).SaveWarCityAll(city);

            var temp = view_war_city.GetEntityById(city.id);
            (new Share.War()).SendCityBuild(temp);
            return Common.GetInstance().BulidData(temp, (int)WarCityCampType.OWN);
        }
    }
}
