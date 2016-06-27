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
    /// 防守方案设定清空
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENSE_PLAN_CLEAR : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENSE_PLAN_CLEAR()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENSE_PLAN_CLEAR _objInstance;

        ///// <summary>WAR_DEFENSE_PLAN_CLEAR单体模式</summary>
        //public static WAR_DEFENSE_PLAN_CLEAR GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENSE_PLAN_CLEAR());
        //}

        /// <summary> 防守方案设定清空 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //获取前端数据
            var tuple = GetFrontData(data);
            if (!tuple.Item1) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var planid = tuple.Item2;
            var planinlist = view_city_defense_plan.GetEntityByPlanId(planid);

            if (!planinlist.Any())
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

            SaveData(planid);
            return CommonHelper.SuccessResult();
        }

        /// <summary>
        /// 获取前端数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Tuple<bool, Int64> GetFrontData(ASObject data)
        {
            //获取方案id
            if (!data.ContainsKey("planid")) return Tuple.Create(false, 0L);
            var sendid = data.FirstOrDefault(q => q.Key == "planid").Value.ToString();
            if (sendid == "") return Tuple.Create(false, 0L);
            Int64 planid = 0;
            if (!Int64.TryParse(sendid, out planid))
                return Tuple.Create(false, 0L);
            return Tuple.Create(true, planid);
        }


        /// <summary>
        /// 保存防守方案地形数据
        /// </summary>
        /// <param name="planid">防守方案id</param>
        /// <returns></returns>
        private void SaveData(Int64 planid)
        {
            //删除防守地形数据
            tg_war_plan_area.GetAreaSetDelete(planid);
            //添加新的数据
            tg_war_plan_area.Insert(new tg_war_plan_area { plan_id = planid, });
            var roles = tg_war_city_defense.GetListByPlanId(planid);
            for (int i = 0; i < roles.Count; i++)
            {
                GetBasePoint(i, roles[i]);
            }
            //武将的位置归为默认位置
            tg_war_city_defense.GetListUpdate(roles);
        }

        /// <summary>
        /// 获取默认位置
        /// </summary>
        /// <param name="location">默认位置</param>
        /// <param name="entity">防守武将实体</param>
        private void GetBasePoint(int location, tg_war_city_defense entity)
        {
            #region 根据位置设定坐标
            switch (location)
            {
                case 1: { entity.point_x = 1; entity.point_y = 3; } break;
                case 2: { entity.point_x = 1; entity.point_y = 4; } break;
                case 3: { entity.point_x = 1; entity.point_y = 5; } break;
                case 4: { entity.point_x = 0; entity.point_y = 3; } break;
                case 5: { entity.point_x = 0; entity.point_y = 5; } break;
            }
            #endregion

        }

    }
}
