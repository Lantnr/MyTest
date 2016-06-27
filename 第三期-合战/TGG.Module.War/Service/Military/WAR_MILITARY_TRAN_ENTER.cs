using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary> 进入运输功能 </summary>
    public class WAR_MILITARY_TRAN_ENTER : IDisposable
    {
        //private static WAR_MILITARY_TRAN_ENTER _objInstance;

        ///// <summary>WAR_MILITARY_TRAN_ENTER单体模式</summary>
        //public static WAR_MILITARY_TRAN_ENTER GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_TRAN_ENTER());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 进入运输功能 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var cityid = session.Player.War.PlayerInCityId;
            if (cityid == 0) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);

            var l = new List<WarCarryVo>();
            var userid = session.Player.User.id;
            var ls = tg_war_carry.GetEntityListByCityId(cityid, userid);
            var list = ls.Where(m => m.time > 0).ToList(); //不是空闲队列
            if (list.Any()) { l = ToCarryVos(list); }

            return BuildData(l, ls.Count());
        }

        /// <summary> 组装数据 </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private ASObject BuildData(List<WarCarryVo> list, int count)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
             { "count", count },
            { "carry",list},
            };
            return new ASObject(dic);
        }

        public List<WarCarryVo> ToCarryVos(List<tg_war_carry> list)
        {
            return list.Select(EntityToVo.ToCarryVo).ToList();
        }
    }
}
