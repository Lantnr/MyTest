using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;

namespace TGG.Module.War.Service.Military
{
    /// <summary> 确定运输目标城市 </summary>
    public class WAR_MILITARY_TRAN_TARGET : IDisposable
    {
        //private static WAR_MILITARY_TRAN_TARGET _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_TARGET单体模式</summary>
        //public static WAR_MILITARY_TRAN_TARGET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_TARGET());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 确定运输目标城市 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id")) return null;
            var cityid = data.FirstOrDefault(m => m.Key == "id").Value.ToString();
            var id = Convert.ToInt32(cityid);
            if (!session.Player.War.Transport.OperableCityIds.Contains(id))
                return CommonHelper.ErrorResult(ResultType.WAR_TARGET_ERROR);
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32033");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var mps = session.Player.War.Transport.Map;
            var startId = Convert.ToString(session.Player.War.PlayerInCityId);
            var dij = new Dijkstra(mps, startId, cityid);
            dij.Find();

            var distance = dij.EndNode.MinDistance.ToString();
            var str = rule.value.Replace("distance", distance);
            var express = CommonHelper.EvalExpress(str);
            var time = Convert.ToInt32(express);
            var city = (new Share.War()).GetWarCity(id);
            var baseCity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (baseCity == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var foods = baseCity.foods - city.res_foods;
            var gun = baseCity.goods - city.res_gun;
            var horse = baseCity.goods - city.res_horse;
            var kuwu = baseCity.goods - city.res_kuwu;
            var razor = baseCity.goods - city.res_razor;

            var sX = new ResourceEntity
            {
                foods = foods < 0 ? 0 : foods,
                gun = gun < 0 ? 0 : gun,
                horse = horse < 0 ? 0 : horse,
                kuwu = kuwu < 0 ? 0 : kuwu,
                razor = razor < 0 ? 0 : razor
            };

            session.Player.War.Transport.LockCityId = id;
            session.Player.War.Transport.LockTime = time;
            session.Player.War.Transport.LockShangXian = sX;

            return BuildData(time, sX);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="time">运输总时间</param>
        /// <param name="foods">军粮</param>
        /// <param name="horse">马匹</param>
        /// <param name="gun">铁炮</param>
        /// <param name="razor">薙刀</param>
        /// <param name="kuwu">苦无</param>
        private ASObject BuildData(Int64 time, ResourceEntity res)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "time",time}, 
            { "foods",res.foods},
            { "horse",res.horse},
            { "gun",res.gun},
            { "razor",res.razor},
            { "kuwu",res.kuwu},
            };
            return new ASObject(dic);
        }
    }
}
