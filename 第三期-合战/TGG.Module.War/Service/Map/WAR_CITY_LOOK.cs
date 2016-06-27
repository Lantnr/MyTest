using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 据点一览 </summary>
    public class WAR_CITY_LOOK : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_CITY_LOOK()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_CITY_LOOK _objInstance;

        ///// <summary>WAR_CITY_LOOK单体模式</summary>
        //public static WAR_CITY_LOOK GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_CITY_LOOK());
        //}

        /// <summary> 据点一览 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var time = session.Player.UserExtend.war_base_time;
            var list = Variable.WarCityAll.Values.Where(m => m.user_id == userid);//tg_war_city.GetEntityByUserId(userid);
            var ls = list.Select(item => EntityToVo.ToStrongHoldShowVo(item, time)).ToList();
            return Common.GetInstance().BuildData(ls);
        }
    }
}
