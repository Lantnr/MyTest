using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Net;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Module.War.Service.Defence;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Attack
{
    /// <summary>
    /// 进攻数据拉取
    /// </summary>
    public class WAR_FIGHT_SET : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_FIGHT_SET()
        {
            Dispose();
        }
    
        #endregion
        //private static WAR_FIGHT_SET _objInstance;

        ///// <summary>WAR_FIGHT_SET</summary>
        //public static WAR_FIGHT_SET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_FIGHT_SET());
        //}

        /// <summary> 进攻数据拉取</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id")) return null;
            var userid = session.Player.User.id;
            var type = 0;
            var baseid = 0;
            var queueid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var queue = tg_war_battle_queue.GetEntityByUseridAndId(queueid, userid);//FindByid(queueid);
            if (queue == null)
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var value = "";
            var tuple = GetPlanId(queue.end_CityId);

            var planid = tuple.Item1;
            var defenseuserid = tuple.Item2;
            if (planid == 0 && defenseuserid == 0) //npc城市
            {
                if (Variable.WarNpcDefenseInfo.ContainsKey(queue.end_CityId))
                    baseid = Variable.WarNpcDefenseInfo[queue.end_CityId];

                //队列id_进攻玩家id_防守玩家id_防守地形基表id
                value = string.Format("{0}_{1}_{2}_{3}", queueid, userid, defenseuserid, baseid);
                Variable.FightPlan.AddOrUpdate(userid, value, (k, v) => value);
                type = 1;
            }
            else
            {
                //队列id_进攻玩家id_防守玩家id_防守方案id
                value = string.Format("{0}_{1}_{2}_{3}", queueid, userid, defenseuserid, planid);
            }
            var listfront = tg_war_formation.GetEntityByUserId(userid);
            var ids = listfront.Select(q => q.base_id).ToList();
            Variable.FightPlan.AddOrUpdate(userid, value, (k, v) => value);
            return BuildData(planid, type, baseid, ids);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="planid">防守方案id</param>
        /// <returns></returns>
        private ASObject BuildData(Int64 planid, int type, int baseid, List<int> frontids)
        {

            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                 {"type",type},
                 {"baseArea",baseid},
                 {"listFront",frontids},
                {"areaSetVo",planid==0?null: ConverDefenseAreas(planid)}
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 获取城市防守地形
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns>1:planid 2:userid</returns>
        private Tuple<Int64, Int64> GetPlanId(Int32 cityid)
        {
            var cityinfo = tg_war_city.GetEntityByBaseId(cityid);

            if (cityinfo == null) return Tuple.Create(0L, 0L);
            var list = new List<Int64>();
            if (cityinfo.plan_1 != 0) list.Add(cityinfo.plan_1);
            if (cityinfo.plan_2 != 0) list.Add(cityinfo.plan_2);
            if (cityinfo.plan_3 != 0) list.Add(cityinfo.plan_3);
            var plan1info = tg_war_city_plan.GetListByCityId(list);
            if (plan1info == null) return Tuple.Create(0L, cityinfo.user_id);
            if (plan1info.All(q => q.is_choose != 1)) return Tuple.Create(0L, cityinfo.user_id);
            var planinfo = plan1info.Where(q => q.is_choose == 1).ToList();
            var index = RNG.Next(0, planinfo.Count - 1);
            return Tuple.Create(plan1info[index].id, cityinfo.user_id);
        }

        /// <summary>
        /// 组装防守地形数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public List<ASObject> ConverDefenseAreas(Int64 planid)
        {
            var area = tg_war_plan_area.GetEntityByPlanId(planid);
            if (!area.Any()) return null;
            area = area.Where(q => q.type != (int)AreaType.陷阱).ToList();
            return area.Select(set => AMFConvert.ToASObject(EntityToVo.ToAreaSetVo(set))).ToList();
        }

    }
}
