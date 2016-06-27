using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征选择目标 </summary>
    public class WAR_SELETE_TARGET : IDisposable
    {
        //private static WAR_SELETE_TARGET _objInstance;

        ///// <summary>WAR_SELETE_TARGET单体模式</summary>
        //public static WAR_SELETE_TARGET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SELETE_TARGET());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 出征选择目标 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);    //id:[double] 据点基表id (要出征的据点)

            var userid = session.Player.User.id;
            session.Player.War.WarBattle = new WarEntity();
            var city = (new Share.War()).GetWarCity(id, userid);
            if (city == null) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);

            var olist = new List<int>();//己方可操作的据点
            var rlist = new List<int>();//敌方可操作的据点
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;

            var psList = tg_war_partner.GetEntityByUserId(userid);
            if (city.guard_time < time) //自己据点不在保护时间内
                session.Player.War.WarBattle.rivalMap = (new Share.War()).GetBattleMaps(id, psList, userid, new List<int>(), ref rlist);
            session.Player.War.WarBattle.Map = (new Share.War()).GetMaps(id, psList, userid, new List<int>(), ref olist);
            session.Player.War.WarBattle.warGoCityId = id;
            session.Player.War.WarBattle.OperableCityIds = olist;
            session.Player.War.WarBattle.OperableRivalCityIds = rlist;
            return BulidData(olist, rlist);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BulidData(List<int> ids, List<int> list)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
            { "cityIds", ids },
            { "list",list} 
            };
            return new ASObject(dic);
        }
    }
}
