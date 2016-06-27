using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征转移确定目标提示 </summary>
    public class WAR_TRANSFER_DETERMINE_TARGET : IDisposable
    {
        //private static WAR_TRANSFER_DETERMINE_TARGET _objInstance;

        ///// <summary>WAR_TRANSFER_DETERMINE_TARGET单体模式</summary>
        //public static WAR_TRANSFER_DETERMINE_TARGET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_TRANSFER_DETERMINE_TARGET());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 出征转移确定目标提示 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("cid")) return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);  //出征队列主键Id
            var cid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "cid").Value); //确定的据点目标

            var userid = session.Player.User.id;
            var ids = session.Player.War.WarBattle.OperableRivalCityIds;
            var _ids = session.Player.War.WarBattle.OperableCityIds;
            if (!(ids.Contains(cid) || _ids.Contains(cid))) return CommonHelper.ErrorResult(ResultType.WAR_WAR_TARGET_ERROR);

            var bq = tg_war_battle_queue.GetEntityByUseridAndId(id, userid); //tg_war_battle_queue.FindByid(id);
            if (bq == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var rids = bq.rid.Split(',').Select(m => Convert.ToInt64(m)).ToList();
            if (!rids.Any()) return CommonHelper.ErrorResult(ResultType.WAR_ROLE_NOEXIST);
            var roles = tg_war_role.GetEntityListByIds(rids); //合战战武将集合
            if (!roles.Any()) return CommonHelper.ErrorResult(ResultType.WAR_ROLE_NOEXIST);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32033");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            //var mps = ids.Contains(cid) ? session.Player.War.WarBattle.rivalMap : session.Player.War.WarBattle.Map;
            var city = (new Share.War()).GetWarCity(bq.end_CityId, userid);
            var _city = (new Share.War()).GetWarCity(cid, userid);
            var mps = city != null && _city != null ? session.Player.War.WarBattle.Map : session.Player.War.WarBattle.rivalMap;

            var startId = Convert.ToString(bq.end_CityId);

            var dij = new Dijkstra(mps, startId, Convert.ToString(cid));
            dij.Find();
            var distance = dij.EndNode.MinDistance.ToString();
            var str = rule.value.Replace("distance", distance);
            var express = CommonHelper.EvalExpress(str);
            var time = Convert.ToInt32(express);

            int state = 0;
            bq = (new Share.War()).SettlementResourceByEntity(bq, ref state); //结算一遍
            var vo = EntityToVo.ToWarGoVo(bq);
            if (state == 0) { return BulidData(vo, bq.foods, bq.funds, 0, time, state); }
            var count = roles.Sum(m => m.army_soldier);
            var res = (new Share.War()).GetWarResourceEntity(roles);
            var use_funds = res.funds; //(new Share.War()).GetRule("32049", count);            //出征每分钟总消耗军资金
            var use_foods = res.foods; //(new Share.War()).GetRule("32048", count);            //出征每分钟总消耗兵粮
            var wartime = (new Share.War()).GetSafetyTime(Convert.ToInt32(bq.funds), Convert.ToInt32(bq.foods), Convert.ToInt32(use_funds), use_foods); //安全行军分钟数
            if (wartime < 0)
            {
                time = GetTime(bq, time);
                roles = (new Share.War()).ReduceResources(roles, time);
            }
            else
            {
                var t = wartime - time;
                if (t < 0)
                {
                    t = Math.Abs(t);
                    t = GetTime(bq, t);
                    bq = (new Share.War()).ReduceResources(bq, roles, wartime);
                    roles = (new Share.War()).ReduceResources(roles, t);
                }
                else
                {
                    bq = (new Share.War()).ReduceResources(bq, roles, time);
                }
            }
            count = roles.Sum(m => m.army_soldier);
            return BulidData(vo, bq.foods, bq.funds, count, time, state);
        }

        private int GetTime(tg_war_battle_queue temp, int tt)
        {
            var time = tg_war_battle_queue.GetTimeInterval(temp);  //出发地到当前的间隔时间
            var t = temp.total_Time - time - tt;                   //还有多久安全行军时间
            if (t < 0)
            {
                return Math.Abs(t); //算出欠的分钟数
            }
            return 0;
        }

        /// <summary> 组装数据 </summary>
        private ASObject BulidData(WarGoVo temp, Int64 foods, Int64 funds, int soldier, int time, int state)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
            { "vo", temp },
             { "foods", foods },
              { "funds", funds },
               { "soldier", soldier },
                { "time", time },
                 { "state", state },
                
            };
            return new ASObject(dic);
        }
    }
}
