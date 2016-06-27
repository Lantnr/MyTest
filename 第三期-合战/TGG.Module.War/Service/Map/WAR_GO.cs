using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征 </summary>
    public class WAR_GO : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        //private static WAR_GO _objInstance;

        ///// <summary>WAR_GO单体模式</summary>
        //public static WAR_GO GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_GO());
        //}

        /// <summary> 出征 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            Variable.RemoveLine(session.Player.User.id);//移除所有路线缓存
            if (!data.ContainsKey("rids") || !data.ContainsKey("funds") || !data.ContainsKey("foods")) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            var rids = (data["rids"] as object[]).Select(Convert.ToInt64).ToList();
            var funds = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "funds").Value);
            var foods = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "foods").Value);
            if (!rids.Any()) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var roles = tg_war_role.GetEntityListByIds(rids); //合战战武将集合
            if (!CheckWarRole(roles)) return CommonHelper.ErrorResult(ResultType.WAR_ROLE_STATE_ERROR); //验证武将状态是空闲且类型不是备大将且都带有兵

            var ids = roles.Select(m => m.rid).ToList();
            var rs = tg_role.GetFindAllByIds(ids);   //个人战武将集合
            var lists = rs.Select(item => item.CloneEntity()).ToList();//为插入日志而克隆武将

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32047");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var power = Convert.ToInt32(rule.value);
            var b = rs.Count(m => (tg_role.GetTotalPower(m) < power)) > 0;
            if (b) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_POWER_ERROR); //验证体力

            var userid = session.Player.User.id;
            var time = session.Player.War.WarBattle.LockTime;
            var cid = session.Player.War.WarBattle.warGoCityId;
            var rcid = session.Player.War.WarBattle.LockCityId;
            var lcid = session.Player.War.WarBattle.lastCityId;
            if (cid == 0 || rcid == 0 || lcid == 0) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);

            var city = (new Share.War()).GetWarCity(cid, userid);
            var _city = (new Share.War()).GetWarCity(rcid);
            if (city == null) CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);
            if (_city != null) if (_city.guard_time > Common.GetInstance().CurrentTime())
                    return CommonHelper.ErrorResult(ResultType.WAR_RIVAL_CITY_GUARD_TIME);

            var res = (new Share.War()).GetWarResourceEntity(roles);

            var carry_funds = (new Share.War()).GetRule("32050", res.funds, time);//最小携带军资金
            var carry_foods = (new Share.War()).GetRule("32050", res.foods, time);//最小携带兵粮

            if (carry_funds > funds) return CommonHelper.ErrorResult(ResultType.WAR_REQUIREMENT_FUNDS_ERROR);
            if (carry_foods > foods) return CommonHelper.ErrorResult(ResultType.WAR_REQUIREMENT_FOODS_ERROR);
            if (city.res_funds < funds) return CommonHelper.ErrorResult(ResultType.WAR_RES_FUNDS_ERROR);
            if (city.res_foods < foods) return CommonHelper.ErrorResult(ResultType.WAR_RES_FOODS_ERROR);

            var wartime = (new Share.War()).GetSafetyTime(funds, foods, Convert.ToInt32(res.funds), res.foods); //安全行军分钟数

            city.res_foods -= foods;
            city.res_funds -= funds; //扣除要携带的资源
            city = ReduceWarCityRes(roles, city);
            var bq = BuildBattleQueue(rids, cid, rcid, lcid, foods, funds, city.res_morale, time, wartime, userid);
            if (!tg_war_battle_queue.InitBattleQueue(bq, city)) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
            (new Share.War()).SaveWarCityAll(city);

            UpdateWarRoleState(roles, (int)WarRoleStateType.DISPATCH); //合战武将状态更新为出战        
            rs.ForEach(m => (new Share.Role()).PowerUpdateAndSend(m, power, m.user_id)); //扣除武将体力 推送
            //插入消耗体力日志
            foreach (var item in lists)
            {
                (new Share.Role()).LogInsert(item, power, ModuleNumber.WAR, (int)WarCommand.WAR_GO, "合战", "出征");
            }

            TaskWarOk(bq);   //开启线程  队列到达是结算一次资源

            session.Player.War.WarBattle = new WarEntity(); //初始化出征相关变量
            var vo = EntityToVo.ToWarGoVo(bq);
            return BulidData(vo);
        }

        /// <summary> 武将集合配置的兵扣除对应城市的资源 </summary>
        /// <param name="list">武将集合</param>
        /// <param name="city">据点信息</param>
        private tg_war_city ReduceWarCityRes(IEnumerable<tg_war_role> list, tg_war_city city)
        {
            return list.Aggregate(city, (current, item) => ReduceWarCityRes(item, current));
        }

        /// <summary> 武将配置的兵扣除对应城市的资源 </summary>
        /// <param name="temp">武将信息</param>
        /// <param name="city">据点信息</param>
        private tg_war_city ReduceWarCityRes(tg_war_role temp, tg_war_city city)
        {
            city.res_foods -= temp.army_foods;
            city.res_use_foods -= temp.army_foods;
            if (city.res_foods < 0) city.res_foods = 0;
            if (city.res_use_foods < 0) city.res_use_foods = 0;

            city.res_funds -= temp.army_funds;
            city.res_use_funds -= temp.army_funds;
            if (city.res_funds < 0) city.res_funds = 0;
            if (city.res_use_funds < 0) city.res_use_funds = 0;

            city.res_gun -= temp.army_gun;
            city.res_use_gun -= temp.army_gun;
            if (city.res_gun < 0) city.res_gun = 0;
            if (city.res_use_gun < 0) city.res_use_gun = 0;

            city.res_horse -= temp.army_horse;
            city.res_use_horse -= temp.army_horse;
            if (city.res_horse < 0) city.res_horse = 0;
            if (city.res_use_horse < 0) city.res_use_horse = 0;

            city.res_kuwu -= temp.army_kuwu;
            city.res_use_kuwu -= temp.army_kuwu;
            if (city.res_kuwu < 0) city.res_kuwu = 0;
            if (city.res_use_kuwu < 0) city.res_use_kuwu = 0;

            city.res_razor -= temp.army_razor;
            city.res_use_razor -= temp.army_razor;
            if (city.res_razor < 0) city.res_razor = 0;
            if (city.res_use_razor < 0) city.res_use_razor = 0;

            city.res_soldier -= temp.army_soldier;
            city.res_use_soldier -= temp.army_soldier;
            if (city.res_soldier < 0) city.res_soldier = 0;
            if (city.res_use_soldier < 0) city.res_use_soldier = 0;
            return city;
        }

        /// <summary> 推送出征到达结算 </summary>
        /// <param name="temp">合战出征队列实体</param>
        public void TaskWarOk(tg_war_battle_queue temp)
        {
            try
            {
                var time = temp.time - temp.start_Time;
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    SpinWait.SpinUntil(() =>
                    {
                        return false;
                    }, Convert.ToInt32(m));
                }, time, token.Token)
           .ContinueWith((m, n) =>
           {
               var model = n as tg_war_battle_queue;
               if (model == null) return;
               (new Share.War()).SettlementResourceAndPush(model);
               if (!Variable.OnlinePlayer.ContainsKey(model.user_id)) return;
               var session = Variable.OnlinePlayer[model.user_id];
               var vo = EntityToVo.ToWarGoVo(model);
               var data = BulidData(vo);
               var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_WAR_OK, (int)ResponseType.TYPE_SUCCESS, data);
               session.SendData(pv);
               token.Cancel();
           }, temp, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 合战武将状态更新为出战 </summary>
        /// <param name="list"></param>
        /// <param name="state"></param>
        private void UpdateWarRoleState(IEnumerable<tg_war_role> list, int state)
        {
            foreach (var item in list)
            {
                item.state = state;
                item.Update();
                (new Share.War()).SendWarRole(item, "state");
            }
        }

        /// <summary> 组装出征行军队列实体 </summary>
        /// <param name="rids">出征武将集合</param>
        /// <param name="startCityId">发出据点Id</param>
        /// <param name="endCityId">目标据点Id</param>
        /// <param name="foods">携带兵粮</param>
        /// <param name="funds">携带军资金</param>
        /// <param name="morale">出征锁定士气</param>
        /// <param name="time">到达总时间（分钟）</param>
        /// <param name="warTime">安全行军时间</param>
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        private tg_war_battle_queue BuildBattleQueue(IEnumerable<long> rids, int startCityId, int endCityId, int lastCityId, Int64 foods, Int64 funds, int morale, int time, int warTime, Int64 userid)
        {
            var t = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var temp = new tg_war_battle_queue
            {
                foods = foods,
                funds = funds,
                morale = morale,
                start_Time = t,
                total_Time = warTime,
                end_CityId = endCityId,
                last_CityId = lastCityId,
                start_CityId = startCityId,
                rid = string.Join(",", rids),
                time = t + (time * 60 * 1000),
                user_id = userid,
            };
            return temp;
        }

        /// <summary> 检查武将状态是否全空闲同时验证是否不包含备大将且都配置有兵</summary>
        private static bool CheckWarRole(IEnumerable<tg_war_role> list)
        {
            return !(list.Count(m => m.state != (int)WarRoleStateType.IDLE || m.type == (int)WarRoleType.NPC || m.army_soldier <= 0) > 0);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="temp">出征Vo</param>
        /// <returns></returns>
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
