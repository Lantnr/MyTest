using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征转移正式确定目标 </summary>
    public class WAR_TRANSFER_FORMAL : IDisposable
    {
        //private static WAR_TRANSFER_FORMAL _objInstance;

        ///// <summary>WAR_TRANSFER_FORMAL单体模式</summary>
        //public static WAR_TRANSFER_FORMAL GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_TRANSFER_FORMAL());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 出征转移正式确定目标 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("cid")) return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);  //出征队列主键Id
            var cid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "cid").Value); //确定的据点目标

            var userid = session.Player.User.id;
            var ids = session.Player.War.WarBattle.OperableRivalCityIds;
            var _ids = session.Player.War.WarBattle.OperableCityIds;
            if (!(ids.Contains(cid) || _ids.Contains(cid))) return CommonHelper.ErrorResult(ResultType.WAR_WAR_TARGET_ERROR);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32033");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var bq = tg_war_battle_queue.GetEntityByUseridAndId(id, userid); //tg_war_battle_queue.FindByid(id);
            if (bq == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

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

            var roles = new List<tg_war_role>();
            (new Share.War()).SettlementResourceAndSave(ref bq, ref roles); //结算一遍

            var count = roles.Sum(m => m.army_soldier);
            var use_funds = (new Share.War()).GetRule("32049", count);            //出征每分钟总消耗军资金
            var use_foods = (new Share.War()).GetRule("32048", count);            //出征每分钟总消耗兵粮
            var wartime = (new Share.War()).GetSafetyTime(Convert.ToInt32(bq.funds), Convert.ToInt32(bq.foods), use_funds, use_foods); //安全行军分钟数

            var t = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            bq.end_CityId = cid;
            bq.time = t + (time * 60 * 1000);
            bq.start_Time = t;
            bq.total_Time = wartime;
            bq.start_CityId = Convert.ToInt32(startId);

            bq.Update();
            new WAR_GO().TaskWarOk(bq);
            session.Player.War.WarBattle = new WarEntity();
            var vo = EntityToVo.ToWarGoVo(bq);
            return BulidData(vo);
        }

        /// <summary> 验证出征携带资源是否足够，是否超出据点拥有资源 </summary>
        /// <param name="funds">当前携带的军资金</param>
        /// <param name="foods">当前携带的粮食</param>
        /// <param name="time">行军时间</param>
        /// <param name="count">带兵数量</param>
        /// <param name="wartime">安全行军时间</param>
        public ResultType CheckWarCityRes(int funds, int foods, int time, int count, ref int wartime)
        {
            var use_funds = (new Share.War()).GetRule("32049", count);//出征每分钟总消耗军资金
            var use_foods = (new Share.War()).GetRule("32048", count);//出征每分钟总消耗兵粮
            var min_funds = (new Share.War()).GetRule("32050", use_funds, time);//最小携带军资金
            var min_foods = (new Share.War()).GetRule("32050", use_foods, time);//最小携带兵粮

            //if (min_funds > funds) return ResultType.WAR_REQUIREMENT_ERROR;
            //if (min_foods > foods) return ResultType.WAR_REQUIREMENT_ERROR;

            var t = (new Share.War()).GetRule("32051", funds, use_funds);
            var _t = (new Share.War()).GetRule("32051", foods, use_foods);
            wartime = t < _t ? t : _t;

            return ResultType.SUCCESS;
        }

        /// <summary> 组装数据 </summary>
        private ASObject BulidData(WarGoVo temp)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
            { "vo", temp },
            };
            return new ASObject(dic);
        }
    }
}
