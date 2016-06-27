using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 本城转移
    /// </summary>
    public class WAR_UPDATE_BASE : IDisposable
    {
        //private static WAR_UPDATE_BASE _objInstance;

        ///// <summary>WAR_UPDATE_BASE单体模式</summary>
        //public static WAR_UPDATE_BASE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_UPDATE_BASE());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 本城转移 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id")) return null;

            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value); //id:[double] 要转本城据点id
            var userid = session.Player.User.id;
            var main_id = session.Player.War.WarCityId;
            var extend = session.Player.UserExtend.CloneEntity();
            var time = Common.GetInstance().CurrentTime();
            if (extend.war_base_time > time) return CommonHelper.ErrorResult(ResultType.WAR_TIME_ERROR);
            if (main_id == 0) return CommonHelper.ErrorResult(ResultType.WAR_NO_MAIN);
            var citys = Variable.WarCityAll.Values.Where(m => (m.base_id == id || m.base_id == main_id) && m.user_id == userid).ToList();
            if (citys.Count() != 2) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32096");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            extend.war_base_time = (DateTime.Now.AddMinutes(Convert.ToInt32(rule.value)).Ticks - 621355968000000000) / 10000;

            foreach (var item in citys)
            {
                item.type = item.type == (int)WarCityType.MAINCITY ?
                    (int)WarCityType.VICECITY : (int)WarCityType.MAINCITY;
                item.Update();
                (new Share.War()).SaveWarCityAll(item);
                (new Share.War()).SendCity(item.base_id, item.user_id);
            }

            extend.Update();
            session.Player.UserExtend = extend;
            session.Player.War.WarCityId = id;
            var ls = citys.Select(item => EntityToVo.ToStrongHoldShowVo(item, extend.war_base_time)).ToList();
            return Common.GetInstance().BuildData(ls);
        }
    }
}
