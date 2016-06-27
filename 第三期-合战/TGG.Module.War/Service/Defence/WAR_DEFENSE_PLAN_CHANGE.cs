using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Net;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 防守方案选用状态更改
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENSE_PLAN_CHANGE : IDisposable  
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENSE_PLAN_CHANGE()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENSE_PLAN_CHANGE _objInstance;

        ///// <summary>WAR_DEFENSE_PLAN_CHANGE单体模式</summary>
        //public static WAR_DEFENSE_PLAN_CHANGE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENSE_PLAN_CHANGE());
        //}

        /// <summary> 防守方案选用状态更改 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //type:0弃用 1选用
            var tuple = GetFrontData(data);
            if (!tuple.Item1) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var plan = tg_war_city_plan.GetEntityByPlanId(tuple.Item3,session.Player.User.id);
            var type = tuple.Item2;
            if (plan == null || type > 1 || type < 0)
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            if ((type == 0 && plan.is_choose != 1) || (type == 1 && plan.is_choose != 0))
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            plan.is_choose = type;
            plan.Save();
            return CommonHelper.SuccessResult();
        }

        /// <summary>
        /// 获取前端数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Tuple<bool, Int32, Int64> GetFrontData(ASObject data)
        {
            if (!data.ContainsKey("planId") || !data.ContainsKey("type"))
                return Tuple.Create(false, 0, 0L);
            var sendplanid = data.FirstOrDefault(q => q.Key == "planId").Value.ToString();
            var sendtype = data.FirstOrDefault(q => q.Key == "type").Value.ToString();
            Int64 planid = 0;
            Int32 type = 0;
            if (!Int64.TryParse(sendplanid, out planid) || !Int32.TryParse(sendtype, out type))
            {
                return Tuple.Create(false, 0, 0L);
            }
            return Tuple.Create(true, type, planid);
        }
    }



}
