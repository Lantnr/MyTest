using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 拉取防守方案
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENCE_PLAN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_PLAN()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_PLAN _objInstance;

        ///// <summary>WAR_DEFENCE_PLAN单体模式</summary>
        //public static WAR_DEFENCE_PLAN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_PLAN());
        //}

        /// <summary> 拉取防守方案 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var id = session.Player.War.PlayerInCityId;
            if (id <= 0) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            //  var list = view_city_defense_plan.GetEntityByUserIdAndCityId(id, session.Player.User.id);
            var city = tg_war_city.GetEntityByBaseId(id);
            if (city == null) return null;
            var ids = new List<Int64>();
            if (city.plan_1 != 0)
                ids.Add(city.plan_1);
            if (city.plan_2 != 0)
                ids.Add(city.plan_2);
            if (city.plan_3 != 0)
                ids.Add(city.plan_3);
            var plan = tg_war_city_plan.GetListByCityId(ids);

            return BuildData(plan);

        }


        private ASObject BuildData(List<tg_war_city_plan> plans)
        {
            var listvo = new List<WarDefencePlanVo>();
            foreach (var plan in plans)
            {
                listvo.Add(new WarDefencePlanVo()
                {
                    id = plan.id,
                    frontId = (int)plan.formation,
                    location = plan.location,
                    isChoose = plan.is_choose,
                    isInit = plan.is_update,
                    roles = ConverDefenseRoles(plan.id),
                    listarea = Common.GetInstance().ConverDefenseAreas(plan.id),

                });
            }
            var data = listvo.Select(AMFConvert.ToASObject).ToList();

            var dic = new Dictionary<string, object>() { { "list", data } };
            return new ASObject(dic);

        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list">玩家防守方案实体</param>
        /// <returns></returns>
        private ASObject BuildData(List<view_city_defense_plan> list)
        {
            //组装防守方案vo
            var listvo = list.GroupBy(q => q.plan_id).Select(
               q => new WarDefencePlanVo()
               {
                   id = q.Key,
                   frontId = (int)list.Where(m => m.plan_id == q.Key).FirstOrDefault().formation,
                   location = list.Where(m => m.plan_id == q.Key).FirstOrDefault().location,
                   isChoose = list.Where(m => m.plan_id == q.Key).FirstOrDefault().is_choose,
                   isInit = list.Where(m => m.plan_id == q.Key).FirstOrDefault().is_update,
                   roles = ConverDefenseRoles(list.Where(m => m.plan_id == q.Key && m.role_id != 0).ToList()),
                   listarea = ConverDefenseAreas(list.Where(m => m.plan_id == q.Key && m.base_id != 0).ToList()),
               }).ToList();
            var data = listvo.Select(AMFConvert.ToASObject).ToList();

            var dic = new Dictionary<string, object>() { { "list", data } };
            return new ASObject(dic);
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<ASObject> ConverDefenseRoles(List<view_city_defense_plan> list)
        {
            return !list.Any() ? null : list.Select(set => AMFConvert.ToASObject(EntityToVo.ToWarDefenceRoleVo(set))).ToList();
        }

        private List<ASObject> ConverDefenseRoles(Int64 planid)
        {
            var roles = tg_war_city_defense.GetListByPlanId(planid);
            return !roles.Any() ? null : roles.Select(set => AMFConvert.ToASObject(EntityToVo.ToWarDefenceRoleVo(set))).ToList();
        }



        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<ASObject> ConverDefenseAreas(List<view_city_defense_plan> list)
        {
            return !list.Any() ? null : list.Select(set => AMFConvert.ToASObject(EntityToVo.ToAreaSetVo(set))).ToList();
        }


    }
}
