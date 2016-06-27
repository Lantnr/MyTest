using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Vo.War;
using TGG.Core.Common;
using System.Collections.Generic;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 进入据点
    /// </summary>
    public class WAR_CITY_IN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_CITY_IN()
        {
            Dispose();
        }
    
        #endregion
        //private static WAR_CITY_IN _objInstance;

        ///// <summary>WAR_CITY_IN单体模式</summary>
        //public static WAR_CITY_IN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_CITY_IN());
        //}

        /// <summary> 进入据点 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);//id:[double] 据点基表id
            var userid = session.Player.User.id;

            var temp = view_war_city.GetEntityByUserId(userid, id);
            if (temp == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            var city = BuildCity(temp);
            (new Share.War()).SaveWarCityAll(city);

            var roles = tg_war_role.GetPlayerWarRoleByUserId(userid, id, (int)WarRoleType.NPC); //获取备大将聚合
            var rs = (new Share.War()).ToWarRoleInfoVos(roles);
            session.Player.War.PlayerInCityId = id;

            return BulidData(temp, (int)WarCityCampType.OWN, rs);
        }

        /// <summary> 视图数据转换tg_war_city表数据 </summary>
        /// <param name="temp">view_war_city</param>
        private tg_war_city BuildCity(view_war_city temp)
        {
            return new tg_war_city
            {
                base_id = temp.base_id,
                boom = temp.boom,
                defense_id = temp.defense_id,
                destroy_time = temp.destroy_time,
                fire_time = temp.fire_time,
                guard_time = temp.guard_time,
                id = temp.id,
                interior_bar = temp.interior_bar,
                levy_bar = temp.levy_bar,
                module_number = temp.module_number,
                name = temp.name,
                own = temp.own,
                ownership_type = temp.ownership_type,
                peace = temp.peace,
                plan_1 = temp.plan_1,
                plan_2 = temp.plan_2,
                plan_3 = temp.plan_3,
                res_foods = temp.res_foods,
                res_funds = temp.res_funds,
                res_gun = temp.res_gun,
                res_horse = temp.res_horse,
                res_kuwu = temp.res_kuwu,
                res_morale = temp.res_morale,
                res_razor = temp.res_razor,
                res_soldier = temp.res_soldier,
                res_use_foods = temp.res_use_foods,
                res_use_funds = temp.res_use_funds,
                res_use_gun = temp.res_use_gun,
                res_use_horse = temp.res_use_horse,
                res_use_kuwu = temp.res_use_kuwu,
                res_use_razor = temp.res_use_razor,
                res_use_soldier = temp.res_use_soldier,
                residence = temp.residence,
                size = temp.size,
                state = temp.state,
                strong = temp.strong,
                time = temp.time,
                train_bar = temp.train_bar,
                type = temp.type,
                user_id = temp.user_id,
            };
        }

        /// <summary> 组装数据 </summary>
        private ASObject BulidData(view_war_city model, int state, List<WarRoleInfoVo> roles)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "vo",model==null?null:EntityToVo.ToWarCityVo(model,state,(int)WarCityOwnershipType.PLAYER,0)},
             { "role",roles},
            };
            return new ASObject(dic);
        }
    }
}
