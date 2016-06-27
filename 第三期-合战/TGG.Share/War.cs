using FluorineFx;
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

namespace TGG.Share
{
    public partial class War
    {
        #region 合战副本

        /// <summary> 初始所有玩家合战副本数据 </summary>
        public void InitWarCopy()
        {
            var bingli = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32071");
            var shiqi = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32072");
            var gubingcishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32073");
            var chuzhencishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32074");
            if (bingli == null || shiqi == null || gubingcishu == null || chuzhencishu == null) return;

            var forces = Convert.ToInt32(bingli.value);
            var hire_count = Convert.ToInt32(gubingcishu.value);
            var morale = Convert.ToInt32(shiqi.value);
            var zhen_count = Convert.ToInt32(chuzhencishu.value);

            tg_war_copy.InitCopy(forces, hire_count, morale, zhen_count);
        }

        #endregion

        #region 合战武将相关公共方法


        /// <summary> 移除武将状态相关的线程 </summary>
        /// <param name="role"></param>
        public void RoleStopTask(tg_war_role role)
        {
            var key = GetKey(role.state_end_time, role.id);
            RemoveCD(key);
        }


        /// <summary> 保存武将合战数据并推送武将资源 </summary>
        /// <param name="list">武将集合</param>
        /// <param name="name">要推送的属性名称</param>
        public void SendWarRole(List<tg_war_role> list, params string[] name)
        {
            foreach (var item in list)
            {
                SendWarRole(item, name);
            }
        }

        /// <summary> 合战武将属性推送 </summary>
        /// <param name="role">合战武将实体</param>
        /// <param name="name">要推送的属性</param>
        public void SendWarRole(tg_war_role role, params string[] name)
        {
            if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            var dic = new Dictionary<string, object>();

            #region 要推送的属性

            foreach (var item in name)
            {
                switch (item)
                {
                    case "state":
                        {
                            dic.Add("state", role.state);
                            break;
                        }
                    case "station":
                        {
                            dic.Add("station", role.station);
                            break;
                        }
                    case "armyCount":
                        {
                            dic.Add("armyCount", role.army_soldier);
                            break;
                        }
                    case "armyBaseId":
                        {
                            dic.Add("armyBaseId", role.army_id);
                            break;
                        }
                    case "stateEndTime":
                        {
                            dic.Add("stateEndTime", role.state_end_time);
                            break;
                        }
                }
            }

            #endregion

            var data = BulidData(role.id, role.type, new ASObject(dic));
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_ROLE_UPDATE, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary> 起服时  将不是空闲的武将重新开启线程 </summary>
        public void AddAllWarRoleTask()
        {
            var ids = GetState();
            var list = tg_war_role.GetWarRoleByState(ids);
            if (!list.Any()) return;
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            foreach (var item in list)
            {
                var t = item.state_end_time - time;
                if (t <= 5000)
                {
                    item.state = (int)WarRoleStateType.IDLE;
                    item.state_end_time = 0;
                    item.Update();
                    SendWarRole(item, "state"); //推送武将合战状态更变
                }
                else
                    TaskRoleState(item);
            }
        }

        private static List<int> GetState()
        {
            return new List<int>
            {
                (int)WarRoleStateType.FIRE,
                (int)WarRoleStateType.DESTROY,
                (int)WarRoleStateType.PRISONERS,
                (int)WarRoleStateType.DIPLOMATIC_RELATIONS,
            };
        }

        /// <summary> 将合战武将改成空闲状态 </summary>
        /// <param name="role">合战武将</param>
        public void TaskRoleState(tg_war_role role)
        {
            try
            {
                var key = GetKey(role.state_end_time, role.id);
                var t = role.state_end_time - (DateTime.Now.Ticks - 621355968000000000) / 10000;
                if (t < 5000) { UpdateStateByIdle(role); return; }

                Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    SpinWait.SpinUntil(() =>
                    {
                        if (Variable.CD.ContainsKey(key))
                        {
                            var b = Variable.CD[key];
                            if (!b) return false;
                        }
                        else { UpdateStateByIdle(role); }
                        token.Cancel();
                        RemoveCD(key);
                        return true;
                    }, Convert.ToInt32(m));
                }, t, token.Token)
                .ContinueWith((m, n) =>
                {
                    RemoveCD(key);
                    var temp = tg_war_role.GetEntityById(Convert.ToInt64(n));
                    if (temp == null) return;
                    UpdateStateByIdle(role);
                    token.Cancel();
                }, role.id, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public string GetKey(Int64 time, Int64 roleid)
        {
            return string.Format("{0}_{1}", time, roleid);
        }

        private void UpdateStateByIdle(tg_war_role role)
        {
            role.state = (int)WarRoleStateType.IDLE;
            role.state_end_time = 0;
            role.Update();
            SendWarRole(role, "state", "stateEndTime"); //推送武将合战状态更变
        }

        #endregion

        #region 出征相关

        /// <summary> 出征消耗 </summary>
        /// <param name="list">出征武将集合</param>
        /// <param name="temp">出征队列信息</param>
        /// <param name="state">是否有士兵扣除</param>
        public List<tg_war_role> ReduceResources(List<tg_war_role> list, ref tg_war_battle_queue temp, ref bool state)
        {
            var res = GetWarResourceEntity(list);
            var use_funds = res.funds;
            var use_foods = res.foods;

            var number = Convert.ToInt32((temp.time - temp.start_Time) / 60 / 1000); //行军时间
            for (int i = 0; i < number; i++)
            {
                var foods = temp.foods - use_foods;
                var funds = temp.funds - use_funds;
                if (foods < 0 || funds < 0) //当前分钟资源不足
                {
                    var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32053");//资源不足时每只部队每分钟减少兵数量。
                    if (rule == null) return new List<tg_war_role>();
                    var c = Convert.ToInt32(rule.value);
                    state = true;
                    list = ReduceWarRoleArmy(list, c);
                }
                else
                {
                    temp.foods = foods;
                    temp.funds = Convert.ToInt32(funds);
                }
            }
            temp.total_Time -= number;
            if (temp.total_Time < 0) temp.total_Time = 0;

            return list;
        }

        /// <summary> 获取出征每分钟消耗 </summary>
        public ResourceEntity GetWarResourceEntity(IEnumerable<tg_war_role> roles)
        {
            var model = new ResourceEntity();
            foreach (var role in roles)
            {
                model.funds += GetRule("32049", role.army_soldier);            //出征每分钟总消耗军资金
                model.foods += GetRule("32048", role.army_soldier);            //出征每分钟总消耗兵粮
            }
            return model;
        }

        /// <summary> 计算固定规则表公式 </summary>
        /// <param name="id">固定规则表的Id</param>
        /// <param name="count">带兵数量 或 id:32050用到的（每分钟总消耗）</param>
        /// <param name="time">id:32050 用到的 到达总时间</param>
        public int GetRule(string id, object count, int time = 0)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;
            var str = "";
            switch (id)
            {
                case "32048"://出征每分钟消耗兵粮  (count：兵数)
                case "32049"://出征每分钟消耗军资金  (count：兵数)
                    {
                        str = rule.value.Replace("count", Convert.ToString(count));
                        break;
                    }
                case "32050"://出征携带资源最小值   (每分钟总消耗(value)*到达时间（minute：分钟）)
                    {
                        str = rule.value.Replace("minute", Convert.ToString(time));
                        str = str.Replace("value", Convert.ToString(count));
                        break;
                    }
                case "32051"://出征行军时间（单项资源计算 分钟） (sum:携带的资源，value：每分钟总消耗)
                    {
                        str = rule.value.Replace("sum", Convert.ToString(count));
                        str = str.Replace("value", Convert.ToString(time));
                        break;
                    }
                default: { return 0; }
            }
            var express = CommonHelper.EvalExpress(str);
            return Convert.ToInt32(express);

        }

        /// <summary> 扣除武将身上配的兵与配置兵对应的资源 </summary>
        /// <param name="list">武将集合</param>
        /// <param name="count">扣除的兵数</param>
        public List<tg_war_role> ReduceWarRoleArmy(List<tg_war_role> list, int count)
        {
            foreach (var item in list)
            {
                var rs = BuildResource(item.army_id, count);
                ReduceWarRoleArmy(item, rs);
            }
            return list;
        }

        /// <summary> 扣除武将资源 </summary>
        /// <param name="role">武将信息</param>
        ///  <param name="rs">要扣除的资源数实体</param>
        public tg_war_role ReduceWarRoleArmy(tg_war_role role, ResourceEntity rs)
        {
            role.army_foods -= rs.foods;
            if (role.army_foods < 0) role.army_foods = 0;

            role.army_funds -= rs.funds;
            if (role.army_funds < 0) role.army_funds = 0;

            role.army_gun -= rs.gun;
            if (role.army_gun < 0) role.army_gun = 0;

            role.army_horse -= rs.horse;
            if (role.army_horse < 0) role.army_horse = 0;

            role.army_kuwu -= rs.kuwu;
            if (role.army_kuwu < 0) role.army_kuwu = 0;

            role.army_razor -= rs.razor;
            if (role.army_razor < 0) role.army_razor = 0;

            role.army_soldier -= rs.soldier;
            if (role.army_soldier < 0) role.army_soldier = 0;
            return role;
        }

        /// <summary> 获取兵种消耗资源数 </summary>
        /// <param name="armyid">兵种基表编号</param>
        /// <param name="count">兵数量</param>
        public ResourceEntity BuildResource(int armyid, int count)
        {
            var rs = new ResourceEntity();
            var baseArmy = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(m => m.id == armyid);
            if (baseArmy == null) return rs;
            var str = baseArmy.cost.Split('|');
            foreach (var item in str)
            {
                var s = item.Split('_');
                if (s.Count() != 2) return rs;
                var type = Convert.ToInt32(s[0]);
                var number = Convert.ToInt32(s[1]) * count;
                switch (type)
                {
                    case (int)WarResourseType.兵粮: { rs.foods += number; break; }
                    case (int)WarResourseType.军资金: { rs.funds += number; break; }
                    case (int)WarResourseType.苦无: { rs.kuwu += number; break; }
                    case (int)WarResourseType.马匹: { rs.horse += number; break; }
                    case (int)WarResourseType.铁炮: { rs.gun += number; break; }
                    case (int)WarResourseType.薙刀: { rs.razor += number; break; }
                    case (int)WarResourseType.足轻: { rs.soldier += number; break; }
                }
            }
            return rs;
        }

        #endregion

        #region MyRegion

        #endregion

        #region 出征队列返回出发据点或本城

        /// <summary> 出征队列返回出发据点或本城 </summary>
        /// <param name="bq">出征队列实体</param>
        public void QueueResultCity(tg_war_battle_queue bq)
        {
            var city = GetWarCity(bq.start_CityId, bq.user_id);
            if (city == null)
            {
                city = Variable.WarCityAll.Values.FirstOrDefault(m => m.user_id == bq.user_id && m.type == (int)WarCityType.MAINCITY);
                if (city == null) return;
            }
            var rids = bq.rid.Split(',').Select(m => Convert.ToInt64(m)).ToList();
            if (!rids.Any()) return;
            var roles = tg_war_role.GetEntityListByIds(rids); //合战战武将集合
            if (!roles.Any()) return;
            bq.Delete();
            RolesResultCity(roles, city.base_id);
        }

        /// <summary> 清除武将身上资源并返回城市 </summary>
        /// <param name="roles"></param>
        /// <param name="cityid"></param>
        public void RolesInitResultCity(IEnumerable<tg_war_role> roles, int cityid)
        {
            foreach (var item in roles)
            {
                item.state = (int)WarRoleStateType.IDLE;
                item.army_foods = 0;
                item.army_funds = 0;
                item.army_gun = 0;
                item.army_horse = 0;
                item.army_kuwu = 0;
                item.army_morale = 0;
                item.army_razor = 0;
                item.army_soldier = 0;
                item.station = cityid;
                item.Update();
                SendWarRole(item, "state", "station", "armyCount"); //推送武将合战状态更变
            }
        }

        /// <summary> 武将返回城市 </summary>
        /// <param name="roles">武将集合</param>
        /// <param name="cityid">要返回的城市</param>
        private void RolesResultCity(IEnumerable<tg_war_role> roles, int cityid)
        {
            foreach (var item in roles)
            {
                item.state = (int)WarRoleStateType.IDLE;
                item.station = cityid;
                item.Update();
                SendWarRole(item, "state", "station"); //推送武将合战状态更变
            }
        }

        #endregion

        #region 运输相关

        /// <summary> 起服的时候将运输队列线程挂回去 </summary>
        public void AddCarryTask()
        {
            var list = tg_war_carry.GetEntityListByRun(); //获取还在运行状态的运输队列
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            foreach (var item in list)
            {
                var t = item.time - time;
                if (t < 10000)
                    CarryComplete(item, true); //运输队列正常完成
                else
                    WaitCarryComplete(item);   //重新开启线程
            }
        }

        /// <summary> 运输完成 </summary>
        /// <param name="temp">运输队列实体</param>
        /// <param name="falg">true:是正常完成   false:目标据点被攻占</param>
        public void CarryComplete(tg_war_carry temp, bool falg)
        {
            RemoveCD(Convert.ToString(temp.time));
            var city = GetWarCity(falg ? temp.end_cityId : temp.start_cityId);
            if (city == null) return;
            AddCityRes(city, temp); //将马车上的资源加到目标据点中
            SendCity(city.base_id, city.user_id); //推送据点信息
            InitCarry(temp);
            SendCarry(temp);
        }

        /// <summary> 运输中止（目标据点被占据情况） </summary>
        public void CarryClose(tg_war_carry temp, int count = 0)
        {
            RemoveCD(Convert.ToString(temp.time));
            if (tg_war_carry.DeleteByCityId(temp.start_cityId, temp.user_id)) return;
            if (count > 5) return;
            count++;
            CarryClose(temp, count);
        }

        /// <summary> 将指定城市的相关运输队列中止 </summary>
        /// <param name="cityid">目标 或 出发城市Id</param>
        public void CarryClose(int cityid)
        {
            var list = tg_war_carry.GetEntityListByCityId(cityid);
            var startList = list.Where(m => m.start_cityId == cityid).ToList();
            var endList = list.Where(m => m.end_cityId == cityid).ToList();
            foreach (var item in startList)
            {
                CarryComplete(item, false);//运输目标据点被占据
            }
            foreach (var item in endList)
            {
                CarryClose(item);//出发地被占据
            }
        }

        /// <summary> 等待运输队列完成 </summary>
        /// <param name="model">运输队列实体</param>
        public void WaitCarryComplete(tg_war_carry model)
        {
            try
            {
                var time = Convert.ToInt32(model.time - (DateTime.Now.Ticks - 621355968000000000) / 10000); //线程时间
                Variable.CD.AddOrUpdate(Convert.ToString(model.time), false, (k, v) => false);
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as tg_war_carry;
                    var key = Convert.ToString(temp.time);
                    SpinWait.SpinUntil(() =>
                    {
                        if (!Variable.CD.ContainsKey(key))
                        {
                            token.Cancel();
                            return true;
                        }

                        var b = Variable.CD[key];
                        if (!b) return false;
                        CarryComplete(temp, true); //运输队列正常完成 加速时的
                        token.Cancel();
                        return true;

                    }, time);
                }, model, token.Token)
                .ContinueWith((m, n) =>
                {
                    var temp = n as tg_war_carry;
                    CarryComplete(temp, true); //运输队列正常完成
                    token.Cancel();
                }, model, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 初始化运输队列 </summary>
        /// <param name="carry">运输实体</param>
        private void InitCarry(tg_war_carry carry)
        {
            carry.time = 0;
            carry.res_gun = 0;
            carry.res_kuwu = 0;
            carry.res_horse = 0;
            carry.res_razor = 0;
            carry.res_foods = 0;
            carry.end_cityId = 0;
            carry.state = (int)WarCarryStateType.IDLE;
            carry.Update();
        }

        /// <summary> 移除线程控制 </summary>
        /// <param name="key">控制器的key</param>
        /// <param name="count">第几次移除</param>
        public void RemoveCD(string key, int count = 0)
        {
            bool falg; count++;
            if (!Variable.CD.ContainsKey(key)) return;
            if (count > 10) return;
            var b = Variable.CD.TryRemove(key, out falg);
            if (!b) RemoveCD(key, count);
        }

        /// <summary> 将资源加到据点中 </summary>
        /// <param name="city"></param>
        /// <param name="carry"></param>
        private void AddCityRes(tg_war_city city, tg_war_carry carry)
        {
            var size = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (size == null) return;
            city.res_gun += carry.res_gun;
            if (city.res_gun > size.goods) city.res_gun = size.goods;
            city.res_kuwu += carry.res_kuwu;
            if (city.res_kuwu > size.goods) city.res_kuwu = size.goods;
            city.res_horse += carry.res_horse;
            if (city.res_horse > size.goods) city.res_horse = size.goods;
            city.res_razor += carry.res_razor;
            if (city.res_razor > size.goods) city.res_razor = size.goods;
            city.res_foods += carry.res_foods;
            if (city.res_foods > size.foods) city.res_foods = size.goods;
            city.Update();
            (new Share.War()).SaveWarCityAll(city);
        }

        #endregion

        #region 地图构建

        /// <summary> 获取可选取的己方据点Id </summary>
        /// <param name="cityId">所在据点Id</param>
        /// <param name="psList">同盟集合</param>
        /// <param name="userid">用户Id</param>
        /// <param name="idsList">当前经过了的据点</param>
        /// <param name="idls">自己能选自己的据点</param>
        public List<Map> GetMaps(int cityId, List<tg_war_partner> psList, Int64 userid, List<int> idsList, ref List<int> idls)
        {
            var ids = new List<int>();
            var map = new List<Map>();
            var cityids = GetWarCityIds(cityId);
            if (!cityids.Any()) return new List<Map>();
            ids.AddRange(idsList);
            ids.Add(cityId);

            foreach (var id in cityids)
            {
                if (ids.Contains(id)) continue;
                if (Variable.WarCityAll.ContainsKey(id))
                {
                    var city = Variable.WarCityAll[id];
                    if (city.user_id == userid)
                    {
                        if (!idls.Contains(id)) idls.Add(id); //自己的据点
                    }
                    else
                        if (!IsExistPartner(psList, city.user_id)) continue;
                }
                else
                {
                    var baseCity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == id);
                    if (baseCity == null) continue;
                    if (baseCity.type != (int)WarLandType.SPACE) continue;
                }
                map.AddRange(BuildMap(cityId, id));
                map.AddRange(GetMaps(id, psList, userid, ids, ref idls));
            }
            return map;
        }

        /// <summary> 获取可选取的敌方据点Id (排除保护期内的)</summary>
        /// <param name="cityId">所在据点Id</param>
        /// <param name="psList">同盟集合</param>
        /// <param name="userid">用户Id</param>
        /// <param name="idsList">当前经过了的据点</param>
        /// <param name="idls">自己能攻击的据点</param>
        public List<Map> GetBattleMaps(int cityId, List<tg_war_partner> psList, Int64 userid, List<int> idsList, ref List<int> idls)
        {
            var ids = new List<int>();
            var map = new List<Map>();
            var cityids = GetWarCityIds(cityId);     //获取该据点的子据点集合
            if (!cityids.Any()) return new List<Map>();
            ids.AddRange(idsList);    //将当前走过的点加到新集合中
            ids.Add(cityId);          //将当前的点加到走过的点的集合中
            foreach (var id in cityids)
            {
                if (ids.Contains(id)) continue;  //如果这个点走过  那么就跳出
                map.AddRange(BuildMap(cityId, id));
                if (!Variable.WarCityAll.ContainsKey(id)) //验证全局据点信息中是否存在该据点
                {
                    var baseCity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == id);
                    if (baseCity == null) continue;
                    if (baseCity.type == (int)WarLandType.NPCCITY)
                    {
                        if (!idls.Contains(id)) idls.Add(id); continue;//此处为能攻击的NPC据点Id 
                    }
                }
                else
                {
                    var city = Variable.WarCityAll[id];
                    if (city.user_id != userid) //如果当前据点不是自己的据点
                    {
                        if (!IsExistPartner(psList, city.user_id)) //如果当前据点不是盟友的据点
                        {
                            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                            if (city.guard_time > time) continue;    //该敌方据点是否还在保护时间内
                            if (!idls.Contains(id)) idls.Add(id); //此处为能攻击的据点Id
                            continue;
                        }
                    }
                }
                map.AddRange(GetBattleMaps(id, psList, userid, ids, ref idls));//继续往下检索  直到找到能攻击的据点
            }
            return map;
        }

        /// <summary> 组装图型实体 </summary>
        /// <param name="startId">节点1</param>
        /// <param name="endId">节点2</param>
        public List<Map> BuildMap(int startId, int endId)
        {
            var start = Convert.ToString(startId);
            var end = Convert.ToString(endId);
            var weight = CityDistance(startId, endId);
            var list = new List<Map>()
            {
                 new Map
            {
                N1 = start,
                N2 = end,
                Weight =weight,
            }, new Map
            {
                N1 =end,
                N2 = start,
                Weight = weight,
            }
            };
            return list;
        }

        /// <summary>计算据点间的距离</summary>
        public Int32 CityDistance(int startId, int endId)
        {
            try
            {
                var startCity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == startId);
                var endCity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == endId);
                if (startCity == null || endCity == null) return 0;
                var startPoint = startCity.point;
                var endPoint = endCity.point;
                var coorPointX = Math.Abs(int.Parse(startPoint.Split(',')[0]) - int.Parse(endPoint.Split(',')[0]));
                var coorPointY = Math.Abs(int.Parse(startPoint.Split(',')[1]) - int.Parse(endPoint.Split(',')[1]));
                var distance = Math.Sqrt(coorPointX * coorPointX + coorPointY * coorPointY);
                return Convert.ToInt32(distance);
            }
            catch (Exception ex) { XTrace.WriteException(ex); return 100; }
        }

        #endregion

        #region 资源消耗

        /// <summary> 据点资源消耗 </summary>
        public void ResourceConsumption()
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32055");
            if (rule == null) return;
            var count = Convert.ToInt32(rule.value); //据点内一个足轻军资金、兵粮各个的消耗

            var list = Variable.WarCityAll.Values.Where(m => m.ownership_type != (int)WarCityOwnershipType.NPC);

            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as tg_war_city;
                    var city = CityConsumption(temp, count);
                    city.Update();
                    SaveWarCityAll(city);
                    var b = Variable.OnlinePlayer.ContainsKey(city.user_id);
                    if (!b) return;
                    var session = Variable.OnlinePlayer[city.user_id] as TGGSession;
                    if (session == null) return;
                    if (session.Player.War.PlayerInCityId == city.base_id)
                        SendCity(city.base_id, city.user_id);
                    token.Cancel();
                }, item, token.Token);
            }
        }

        /// <summary> 结算出征队列的资源并推送资源结算 </summary>
        public void SettlementResourceAndPush(tg_war_battle_queue model)
        {
            var data = SettlementResource(model);
            if (!Variable.OnlinePlayer.ContainsKey(model.user_id)) return;
            var session = Variable.OnlinePlayer[model.user_id];
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_RESOURCES_SUMMARY, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary> 结算出征队列的资源 </summary>
        public ASObject SettlementResource(tg_war_battle_queue model)
        {
            int state = 0;
            model = SettlementResourceByEntity(model, ref state);
            if (model == null) return CommonHelper.ErrorResult(ResultType.DATA_NULL_ERROR);
            var vo = EntityToVo.ToWarGoVo(model);
            return BulidData(vo, state);
        }

        public tg_war_battle_queue SettlementResourceByEntity(tg_war_battle_queue model, ref int state)
        {
            state = 1;  //是否删除  0:是 1：否
            var roles = new List<tg_war_role>();
            var flag = SettlementResourceByEntity(ref model, ref roles);

            if (!flag) return model;
            if (roles.Sum(b => b.army_soldier) <= 0) //已经没士兵存在
            {
                var r = roles.FirstOrDefault();
                if (r == null) return null;
                var city = GetWarCity(r.station, r.user_id) ??
                Variable.WarCityAll.Values.FirstOrDefault(q => q.user_id == model.user_id && q.type == (int)WarCityType.MAINCITY); //如果出发据点查询不到 将 这些武将放到主据点中
                if (city == null) return null;
                UpdateWarRoleState(roles, (int)WarRoleStateType.IDLE, Convert.ToInt32(city.base_id), "armyCount", "state", "station");
                model.Delete();
                state = 0;
            }
            else
            {
                SendWarRole(roles, "armyCount");
            }
            return model;
        }

        public void SettlementResourceAndSave(ref tg_war_battle_queue model, ref List<tg_war_role> roles)
        {
            //var roles = new List<tg_war_role>();
            var flag = SettlementResourceByEntity(ref model, ref roles);
            if (!flag) return;
            if (roles.Sum(b => b.army_soldier) <= 0) //已经没士兵存在
            {
                var r = roles.FirstOrDefault();
                if (r == null) return;
                var city = GetWarCity(r.station, r.user_id) ??
                           Variable.WarCityAll.Values.FirstOrDefault(q => q.user_id == r.user_id && q.type == (int)WarCityType.MAINCITY); //如果出发据点查询不到 将 这些武将放到主据点中
                if (city == null) return;
                UpdateWarRoleState(roles, (int)WarRoleStateType.IDLE, Convert.ToInt32(city.base_id), "armyCount", "state", "station");
                model.Delete();
            }
            else
            {
                UpdateWarRole(roles, "armyCount");
            }
        }

        /// <summary> 结算出征队列的资源 </summary>
        /// <param name="model"></param>
        /// <param name="roles"></param>
        /// <returns>是否减少武将的兵</returns>
        public bool SettlementResourceByEntity(ref tg_war_battle_queue model, ref List<tg_war_role> roles)
        {
            model.time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var ids = model.rid.Split(',').Select(q => Convert.ToInt64(q)).ToList();
            roles = tg_war_role.GetEntityListByIds(ids);
            if (!roles.Any()) return false;
            var count = roles.Sum(m => m.army_soldier);           //总带兵数
            if (count <= 0) { return true; }

            var time = tg_war_battle_queue.GetTimeInterval(model);  //出发地到当前的间隔时间
            var t = model.total_Time - time;                        //还有多久安全行军时间
            if (t < 0)
            {
                var tt = Math.Abs(t); //算出欠的分钟数
                model = ReduceResources(model, roles, model.total_Time);
                roles = ReduceResources(roles, tt);
                return true;
            }
            model = ReduceResources(model, roles, time);
            return false;
        }

        /// <summary> 还在安全时间内时资源扣除 </summary>
        /// <param name="model">出征队列</param>
        /// <param name="count">带兵数量</param>
        /// <param name="time">消耗的分钟数</param>
        public tg_war_battle_queue ReduceResources(tg_war_battle_queue model, List<tg_war_role> list, int time)
        {
            var res = GetWarResourceEntity(list);
            var use_funds = Convert.ToInt32(res.funds); //GetRule("32049", count);  //出征每分钟总消耗军资金
            var use_foods = res.foods; //GetRule("32048", count);  //出征每分钟总消耗兵粮
            var funds = use_funds * time;
            var foods = use_foods * time;
            model.foods -= foods;
            model.funds -= funds;
            return model;
        }

        /// <summary> 扣除武将身上的士兵 </summary>
        /// <param name="roles"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<tg_war_role> ReduceResources(List<tg_war_role> roles, int time)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32053");//资源不足时每只部队每分钟减少兵数量。
            if (rule == null) return new List<tg_war_role>();
            var c = Convert.ToInt32(rule.value) * time;
            return ReduceWarRoleArmy(roles, c);
        }

        private void UpdateWarRole(IEnumerable<tg_war_role> list, params string[] name)
        {
            foreach (var item in list)
            {
                item.Update();
                SendWarRole(item, name);
            }
        }

        private void UpdateWarRoleState(IEnumerable<tg_war_role> list, int state, int station, params string[] name)
        {
            foreach (var item in list)
            {
                item.state = state;
                item.station = station;
                item.Update();
                SendWarRole(item, name);
            }
        }

        /// <summary> 组装数据 </summary>
        /// <param name="temp">出征Vo</param>
        /// <param name="state">//是否删除  0:是 1：否</param>
        /// <returns></returns>
        private ASObject BulidData(WarGoVo temp, int state)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
             { "state", state },//是否删除  0:是 1：否
            { "vo", temp },
            };
            return new ASObject(dic);
        }

        /// <summary> 城市消耗 </summary>
        /// <param name="city">要消耗的城市</param>
        /// <param name="count">每个兵要消耗的资源数量</param>
        public tg_war_city CityConsumption(tg_war_city city, double count)
        {
            var total = Convert.ToInt32(city.res_soldier * count);                  //该据点要消耗的资源数量
            var foods = Convert.ToInt32(city.res_foods - city.res_use_foods);       //空闲军粮
            var funds = Convert.ToInt32(city.res_funds - city.res_use_funds);       //空闲军资金

            if (total <= foods && total <= funds) //资源足够的情况
            {
                city.res_foods -= total;
                city.res_funds -= total;
                if (city.res_foods < 0) city.res_foods = 0;
                if (city.res_funds < 0) city.res_funds = 0;
            }
            else
            {
                var minNumber = foods < funds ? foods : funds; //最小资源
                var sol_count = Convert.ToInt32(minNumber / count); //资源只够维持的兵数
                var res_count = Convert.ToInt32(count * sol_count); //需要扣掉的置闲资源
                city.res_foods = city.res_foods - res_count;        //扣除sol_count个兵的资源消耗
                city.res_funds = city.res_funds - res_count;
                if (city.res_foods < 0) city.res_foods = 0;
                if (city.res_funds < 0) city.res_funds = 0;

                var soldier = city.res_soldier - sol_count;             //计算出当前要扣除的士兵
                var kongxian = city.res_soldier - city.res_use_soldier; //空闲的士兵
                if (kongxian < 0)
                {
                    city.res_soldier = 0; city.res_use_soldier = 0;
                    return city;
                }
                if (soldier <= kongxian)  //空闲人口够扣的时候
                {
                    city.res_soldier -= soldier;
                    return city;
                }

                city.res_soldier -= kongxian;                            //减去空闲的兵 
                var sc = soldier - kongxian;                             //计算出要减去的配置兵数
                var roles = tg_war_role.GetEntityListByUserId(city.user_id, city.base_id).ToList()
                    .Where(m => m.state != (int)WarRoleStateType.DISPATCH && m.army_soldier > 0).ToList(); //该据点的所有武将信息

                if (!roles.Any()) return city;
                var rs = roles.Where(m => m.state != (int)WarRoleStateType.DEFENSE).OrderBy(m => m.army_soldier).ToList(); //不是防守状态的武将集合
                sc = CityConsumption(rs, sc, city);
                if (sc > 0)
                {
                    var rr = roles.Where(m => m.state == (int)WarRoleStateType.DEFENSE).OrderBy(m => m.army_soldier).ToList(); //防守状态的武将集合
                    sc = CityConsumption(rr, sc, city);
                }
            }
            return city;
        }

        private int CityConsumption(IEnumerable<tg_war_role> rs, int sc, tg_war_city city)
        {
            foreach (var item in rs)
            {
                if (sc <= 0) continue;
                if (item.army_soldier < sc)
                {
                    sc = sc - item.army_soldier;//还要扣的足轻
                    var res = BuildResource(item.army_id, item.army_soldier);
                    ReduceWarRoleArmy(item, res);
                    city.res_soldier -= res.soldier;
                    city.res_use_soldier -= res.soldier;
                    res.soldier = 0;
                    city = ReduceCityUseResources(res, city);
                }
                else
                {
                    var res = BuildResource(item.army_id, sc);
                    ReduceWarRoleArmy(item, res);
                    city.res_soldier -= res.soldier;
                    city.res_use_soldier -= res.soldier;
                    res.soldier = 0;
                    sc = 0;
                    city = ReduceCityUseResources(res, city);
                }

                item.Update();
                SendWarRole(item, "armyCount");
            }
            return sc;
        }


        /// <summary> 将占用资源释放出来 </summary>
        /// <param name="entity">要释放的资源数</param>
        /// <param name="city">据点信息</param>
        public tg_war_city ReduceCityUseResources(ResourceEntity entity, tg_war_city city)
        {
            city.res_use_foods -= entity.foods;
            if (city.res_use_foods < 0) city.res_use_foods = 0;
            city.res_use_funds -= entity.funds;
            if (city.res_use_funds < 0) city.res_use_funds = 0;
            city.res_use_gun -= entity.gun;
            if (city.res_use_gun < 0) city.res_use_gun = 0;
            city.res_use_kuwu -= entity.kuwu;
            if (city.res_use_kuwu < 0) city.res_use_kuwu = 0;
            city.res_use_horse -= entity.horse;
            if (city.res_use_horse < 0) city.res_use_horse = 0;
            city.res_use_razor -= entity.razor;
            if (city.res_use_razor < 0) city.res_use_razor = 0;
            city.res_use_soldier -= entity.soldier;
            if (city.res_use_soldier < 0) city.res_use_soldier = 0;
            return city;
        }

        #endregion

        #region 换算资源实体

        public ResourceEntity BuildResourceEntity(IEnumerable<tg_war_role> list)
        {
            var res = new ResourceEntity();
            return list.Aggregate(res, (current, item) => BuildResourceEntity(item, current));
        }

        public ResourceEntity BuildResourceEntity(tg_war_role temp, ResourceEntity res)
        {
            res.funds += temp.army_funds;
            res.foods += temp.army_foods;
            res.gun += temp.army_gun;
            res.horse += temp.army_horse;
            res.kuwu += temp.army_kuwu;
            res.razor += temp.army_razor;
            res.soldier += temp.army_soldier;
            return res;
        }

        public ResourceEntity BuildResourceEntity(tg_war_battle_queue temp, ResourceEntity res)
        {
            res.funds += temp.funds;
            res.foods += Convert.ToInt32(temp.foods);
            return res;
        }

        #endregion

        #region 破坏城市处理线程

        public void CityDestructionAddTask()
        {
            var list = tg_war_city.FindAll();
            foreach (var city in list)
            {
                DestructionTask(city);
            }
        }

        public void DestructionTask(tg_war_city city)
        {
            try
            {
                if (city.destroy_time == 0) return;
                var t = city.destroy_time - (DateTime.Now.Ticks - 621355968000000000) / 10000;
                if (t < 5000) { UpdateDestructionRes(city); return; }

                var key = GetKey(city.base_id, city.destroy_time);
                Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    SpinWait.SpinUntil(() =>
                    {
                        if (Variable.CD.ContainsKey(key))
                        {
                            var b = Variable.CD[key];
                            if (!b) return false;
                        }
                        RemoveCD(key);
                        token.Cancel();
                        return true;
                    }, Convert.ToInt32(m));
                }, t, token.Token)
                .ContinueWith((m, n) =>
                {
                    RemoveCD(key);
                    var model = n as tg_war_city;
                    if (model == null) return;
                    var temp = (new War()).GetWarCity(model.base_id, model.user_id);
                    if (temp == null) return;
                    UpdateDestructionRes(temp);
                    token.Cancel();
                }, city, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        //private string GetKey(Int64 cityId, Int64 time)
        //{
        //    return string.Format("{0}_{1}", cityId, time);
        //}

        private void UpdateDestructionRes(tg_war_city city)
        {
            city.boom += city.destroy_boom;
            city.peace += city.destroy_peace;
            city.strong += city.destroy_strong;

            city.destroy_boom = 0;
            city.destroy_peace = 0;
            city.destroy_strong = 0;

            city.destroy_time = 0;

            city.Update();
            (new Share.War()).SaveWarCityAll(city);
            (new Share.War()).SendCity(city.base_id, city.user_id);
        }

        #endregion


        /// <summary> 获取安全行军时间 </summary>
        /// <param name="funds">军资金</param>
        /// <param name="foods">兵粮</param>
        /// <param name="useFunds">每分钟总消耗军资金</param>
        /// <param name="useFoods">每分钟总消耗兵粮</param>
        public int GetSafetyTime(object funds, int foods, int useFunds, int useFoods)
        {
            var t = GetRule("32051", funds, useFunds);
            var _t = GetRule("32051", foods, useFoods);
            return t < _t ? t : _t;
        }

        /// <summary> 将资源加到据点中 </summary>
        /// <param name="entity">资源</param>
        /// <param name="city">据点信息</param>
        public tg_war_city AddCityResources(ResourceEntity entity, tg_war_city city)
        {
            //var size = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            //if (size == null) return null;
            city.res_foods += entity.foods;
            //if (city.res_funds > size.foods) city.res_foods = size.foods;
            city.res_funds += entity.funds;
            //if (city.res_funds > size.funds) city.res_funds = size.funds;
            city.res_gun += entity.gun;
            //if (city.res_gun > size.goods) city.res_gun = size.goods;
            city.res_kuwu += entity.kuwu;
            //if (city.res_kuwu > size.goods) city.res_kuwu = size.goods;
            city.res_horse += entity.horse;
            //if (city.res_horse > size.goods) city.res_horse = size.goods;
            city.res_razor += entity.razor;
            //if (city.res_razor > size.goods) city.res_razor = size.goods;
            city.res_soldier += entity.soldier;
            return city;
        }

        /// <summary> 获取据点信息 </summary>
        /// <param name="cityId">要获取的据点基表Id</param>
        public tg_war_city GetWarCity(Int64 cityId)
        {
            return Variable.WarCityAll.ContainsKey(cityId) ? Variable.WarCityAll[cityId] : null;
        }

        /// <summary> 获取据点信息 </summary>
        /// <param name="cityId">要获取的据点基表Id</param>
        public tg_war_city GetWarCity(Int64 cityId, Int64 userid)
        {
            if (!Variable.WarCityAll.ContainsKey(cityId)) return null;
            var city = Variable.WarCityAll[cityId];
            return city.user_id != userid ? null : city;
        }

        /// <summary> 获取据点信息 </summary>
        /// <param name="type">指定类型  WarCityOwnershipType 枚举</param>
        /// <param name="time">指定时间内</param>
        public List<tg_war_city> GetWarCityListByType(int type, Int64 time)
        {
            return Variable.WarCityAll.Values.Where(m => m.guard_time > time && m.ownership_type == type).ToList();
        }

        /// <summary> 移除全局内存的据点信息 </summary>
        /// <param name="cityid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool RemovesWarCity(int cityid, int count = 0)
        {
            tg_war_city c;
            var b = Variable.WarCityAll.TryRemove(cityid, out c);
            if (count >= 5) return b;
            count++;
            return RemovesWarCity(cityid, count);
        }

        /// <summary> 将据点信息更新全局 </summary>
        /// <param name="city">据点信息</param>
        public void SaveWarCityAll(tg_war_city city)
        {
            Variable.WarCityAll.AddOrUpdate(city.base_id, city, (k, v) => city);
        }

        /// <summary> 推送空闲队列Id </summary>
        public void SendCarry(tg_war_carry temp)
        {
            if (!Variable.OnlinePlayer.ContainsKey(temp.user_id)) return;
            var session = Variable.OnlinePlayer[temp.user_id] as TGGSession;
            var data = BulidData(temp.id);
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_TRAN_OK, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary> 将据点推送给其他玩家 </summary>
        /// <param name="model">该据点信息</param>
        public void SendCityBuild(view_war_city model)
        {
            var uids = Variable.WarInUser.Where(m => m.Value == model.module_number && m.Key != model.user_id).Select(m => m.Key).ToList();
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var list = tg_war_partner.GetEntityByUserIdAndTime(model.user_id, time);

            foreach (var item in uids)
            {
                var obj = new ObjectTask()
                {
                    uid = item,
                    city = model,
                    part = list.FirstOrDefault(m => m.partner_id == item),

                };
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    int state;
                    long t = 0;
                    var temp = m as ObjectTask;
                    if (temp == null) return;

                    if (!Variable.OnlinePlayer.ContainsKey(temp.uid)) return;
                    var session = Variable.OnlinePlayer[temp.uid] as TGGSession;

                    var p = temp.part;
                    if (p != null)
                    {
                        if (p.state == (int)WarPertnerType.ALLIANCE_IN)
                        {
                            t = p.time;
                            state = (int)WarCityCampType.PARTNER;
                        }
                        else state = (int)WarCityCampType.ENEMY;
                    }
                    else state = (int)WarCityCampType.ENEMY;

                    var vo = EntityToVo.ToWarCityVo(temp.city, state, (int)WarCityOwnershipType.PLAYER, t);
                    var data = BulidData(new List<WarCityVo> { vo });
                    var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_CITY, (int)ResponseType.TYPE_SUCCESS, data);
                    session.SendData(pv);
                    token.Cancel();
                }, obj, token.Token);
            }
        }

        /// <summary> List[tg_war_role] TO  List[WarRoleInfoVo] </summary>
        /// <param name="ls">List[tg_war_role]</param>
        /// <returns></returns>
        public List<WarRoleInfoVo> ToWarRoleInfoVos(List<tg_war_role> ls)
        {
            return ls.Select(EntityToVo.ToWarRoleInfoVo).ToList();
        }

        /// <summary> 是否盟友 </summary>
        public bool IsExistPartner(List<tg_war_partner> list, Int64 partnersid)
        {
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            return list.Where(n => n.time > time).Select(n => n.partner_id).Contains(partnersid);
        }

        /// <summary> 获取据点的子据点 </summary>
        /// <param name="id">要获取的据点基表Id</param>
        public List<int> GetWarCityIds(int id)
        {
            var baseCity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == id);
            return baseCity == null ? new List<int>() : GetWarCityIds(baseCity.tooltip);
        }

        /// <summary>切割字符串 获取子据点Id集合 </summary>
        /// <param name="str">要切割的字符串</param>
        public List<int> GetWarCityIds(string str)
        {
            if (str == string.Empty) return new List<int>();
            var temps = str.Split('|');
            return temps.Select(item => Convert.ToInt32(item)).ToList();
        }

        /// <summary> 组装数据 </summary>
        /// <param name="list">据点Vo信息集合</param>
        public ASObject BulidData(List<WarCityVo> list)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "vo",list},
            };
            return new ASObject(dic);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="id">运输队列主键Id</param>
        public ASObject BulidData(Int64 id)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "id",id},
            };
            return new ASObject(dic);
        }

        /// <summary> 初始化全局合战据点集合 </summary>
        public void InitWarCity()
        {
            var citys = tg_war_city.FindAll().ToList();
            foreach (var item in citys)
            {
                Variable.WarCityAll.AddOrUpdate(item.base_id, item, (k, v) => item);
            }
        }


        /// <summary> 组装数据 </summary>
        private ASObject BulidData(Int64 id, int type, ASObject data)
        {
            var dic = new Dictionary<string, object> { 
            { "id", id },
            { "roleType", type },
            { "data",data} 
            };
            return new ASObject(dic);
        }

        class ObjectTask
        {
            /// <summary> 用户id </summary>
            public Int64 uid { get; set; }

            /// <summary> 外交实体 </summary>
            public tg_war_partner part { get; set; }

            /// <summary> 据点数据 </summary>
            public view_war_city city { get; set; }
        }
    }
}
