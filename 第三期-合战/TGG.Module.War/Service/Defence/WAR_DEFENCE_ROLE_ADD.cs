using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 增加防守武将
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENCE_ROLE_ADD : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_ROLE_ADD()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_ROLE_ADD _objInstance;

        ///// <summary>WAR_DEFENCE_ROLE_ADD单体模式</summary>
        //public static WAR_DEFENCE_ROLE_ADD GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_ROLE_ADD());
        //}

        /// <summary> 增加防守武将 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id")) return null;
            var roleid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);

            var cityid = session.Player.War.PlayerInCityId;
            var cityinfo = tg_war_city.GetEntityByBaseId(cityid);

            var role = tg_war_role.GetEntityById(roleid, session.Player.User.id);//FindByid(roleid);
            //查询该据点所有的防守武将
            var defenselist = tg_war_city_defense.GetEntityByCityId(cityid);

            if (defenselist == null || role == null || cityinfo == null ||
                defenselist.Where(q => q.plan_id == cityinfo.plan_1).ToList().Count >= 5)
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            //验证该位置有没有武将
            if (defenselist.Any(q => q.role_id == roleid))
                return CommonHelper.ErrorResult(ResultType.WAR_DEFENSE_LOCATION_ERROR);

            if(role.station!=session.Player.War.PlayerInCityId)
                return CommonHelper.ErrorResult(ResultType.WAR_ROLE_NOT_IN_CITY); 
            if (!CheckRoleState(role))
                return CommonHelper.ErrorResult(ResultType.WAR_ROLE_STATE_ERROR);


            if (!SavaData(cityinfo.plan_1, cityinfo.plan_2, cityinfo.plan_3, roleid, role.type, cityid, defenselist))
                return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
            return CommonHelper.SuccessResult();
        }



        /// <summary>
        /// 验证该武将是否在该城市中
        /// </summary>
        /// <param name="role">合战武将实体</param>
        /// <returns></returns>
        private bool CheckRoleState(tg_war_role role)
        {
            if (role == null) return false;
            if (!tg_war_role.RoleIsIdle(role)) return false;
            role.state = (int)WarRoleStateType.DEFENSE;
            role.Update();
            new Share.War().SendWarRole(role, "state");
            return true;
        }

        /// <summary>
        /// 保存防守武将数据
        /// </summary>
        /// <param name="planid1">防守方案1id</param>
        /// <param name="planid2">防守方案2id</param>
        /// <param name="planid3">防守方案3id</param>
        /// <param name="roleid">合战武将id</param>
        /// <param name="type">武将类型</param>
        /// <param name="cityid">据点基表id</param>
        /// <param name="roles">已经布置的武将集合</param>
        /// <returns></returns>
        private Boolean SavaData(Int64 planid1, Int64 planid2, Int64 planid3, Int64 roleid, Int32 type, Int32 cityid, List<tg_war_city_defense> roles)
        {
            var list = new List<tg_war_city_defense>();
            var entity = new tg_war_city_defense()
            {
                role_id = roleid,
                type = type,
                city_id = cityid,
                plan_id = planid1
            };
            GetBasePoint(entity, roles);

            list.Add(entity);
            if (planid2 != 0)
            {
                var entity2 = entity.CloneEntity();
                entity2.plan_id = planid2;
                list.Add(entity2);
            }
            if (planid3 != 0)
            {
                var entity3 = entity.CloneEntity();
                entity3.plan_id = planid3;
                list.Add(entity3);
            }
            return tg_war_city_defense.GetListInsert(list) > 0;
        }

        /// <summary>
        /// 获取默认坐标，5个位置获取一个没有武将的坐标
        /// </summary>
        /// <param name="entity">武将实体</param>
        /// <param name="roles">所有武将</param>
        /// <returns></returns>
        private void GetBasePoint(tg_war_city_defense entity, List<tg_war_city_defense> roles)
        {
            #region
            if (!roles.Any(q => q.point_x == 1 && q.point_y == 3)) { entity.point_x = 1; entity.point_y = 3; return; }
            if (!roles.Any(q => q.point_x == 1 && q.point_y == 4)) { entity.point_x = 1; entity.point_y = 4; return; }
            if (!roles.Any(q => q.point_x == 1 && q.point_y == 5)) { entity.point_x = 1; entity.point_y = 5; return; }
            if (!roles.Any(q => q.point_x == 0 && q.point_y == 3)) { entity.point_x = 0; entity.point_y = 3; return; }
            if (!roles.Any(q => q.point_x == 0 && q.point_y == 5)) { entity.point_x = 0; entity.point_y = 5; }

            #endregion
        }


        private Int32 GetRoleBaseId(tg_war_role role)
        {
            return role.type == (int)WarRoleType.NPC ?
                Convert.ToInt32(role.rid)
               : tg_role.FindByid(role.rid).role_id;
        }
    }
}
