using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 移除防守武将
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENCE_ROLE_REMOVE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_ROLE_REMOVE()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_ROLE_REMOVE _objInstance;

        ///// <summary>WAR_DEFENCE_ROLE_REMOVE单体模式</summary>
        //public static WAR_DEFENCE_ROLE_REMOVE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_ROLE_REMOVE());
        //}

        /// <summary> 移除防守武将 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id")) return null;
            var roleid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var cityid = session.Player.War.PlayerInCityId;
            var defenseinfo = tg_war_city_defense.GetByCityIdIdAndRoleId(cityid, roleid);
            if (defenseinfo == null || !defenseinfo.Any())
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            //删除数据库中数据
            tg_war_city_defense.GetCityRoleDelete(cityid, roleid);
            var role = tg_war_role.GetEntityById(roleid, session.Player.User.id);//FindByid(roleid);
            role.state = (int)WarRoleStateType.IDLE;
            role.Update();
            new Share.War().SendWarRole(role, "state");
            return CommonHelper.SuccessResult();

        }
    }
}
