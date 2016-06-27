using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 修改防守方案消费
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENCE_PLAN_COST : IDisposable  
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_PLAN_COST()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_PLAN_COST _objInstance;

        ///// <summary>WAR_DEFENCE_PLAN_COST单体模式</summary>
        //public static WAR_DEFENCE_PLAN_COST GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_PLAN_COST());
        //}

        /// <summary> 修改防守方案消费 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("planId")) return null;
            var getplan = data.FirstOrDefault(q => q.Key == "planId").Value.ToString();
            Int64 planid = 0; Int64.TryParse(getplan, out planid);
            var planinfo = tg_war_city_plan.GetEntityByPlanId(planid,session.Player.User.id);

            if (planinfo == null) return
                   CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

            //不是第一次修改
            if (planinfo.is_update == 1)
            {
                var city = session.Player.War.City.CloneEntity();
                if (!CheckFunds(city)) return CommonHelper.ErrorResult(ResultType.WAR_RES_FUNDS_ERROR);
                session.Player.War.City = city;

            }
            //第一次修改
            planinfo.is_update = 1;
            planinfo.Update();
             
            return BuildData(planinfo);


        }

        /// <summary>
        /// 验证消耗的军资金
        /// </summary>
        /// <returns></returns>
        private bool CheckFunds(tg_war_city city)
        {
            var costfunds = Common.GetInstance().GetRule("32042");
            if (city.res_funds < costfunds) return false;
            city.res_funds -= costfunds;
            city.Update();
            Variable.WarCityAll.TryUpdate(city.base_id, city, city);

            new Share.War().SendCity(city.base_id, city.user_id);
            return true;
        }

  
        /// <summary>
        /// 组装数据
        /// </summary>
        /// <returns></returns>
        private ASObject BuildData(tg_war_city_plan plan)
        {
            //组装防守方案vo
            var vo = new WarDefencePlanVo()
               {
                   id = plan.id,
                   frontId = (int)plan.formation,
                   location = plan.location,
                   isChoose = plan.is_choose,
                   isInit = plan.is_update,
                   roles = ConverDefenseRoles(plan.id),
                   listarea = ConverDefenseAreas(plan.id),

               };
            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                {"warDefencePlanVo", vo}
            };
            return new ASObject(dic);
        }


        private List<ASObject> ConverDefenseAreas(Int64 planid)
        {
            var area = tg_war_plan_area.GetEntityByPlanId(planid);
            return !area.Any() ? null : area.Select(set => AMFConvert.ToASObject(EntityToVo.ToAreaSetVo(set))).ToList();
        }

        private List<ASObject> ConverDefenseRoles(Int64 planid)
        {
            var roles = tg_war_city_defense.GetListByPlanId(planid);
            return !roles.Any() ? null : roles.Select(set => AMFConvert.ToASObject(EntityToVo.ToWarDefenceRoleVo(set))).ToList();
        }
    }
}
