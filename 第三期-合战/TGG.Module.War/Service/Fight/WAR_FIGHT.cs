using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 合战出阵战斗
    /// </summary>
    public class WAR_FIGHT : IDisposable
    {
        //private static WAR_FIGHT _objInstance;

        ///// <summary>WAR_FIGHT单体模式</summary>
        //public static WAR_FIGHT GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_FIGHT());
        //}
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_FIGHT()
        {
            Dispose();
        }
    
        #endregion
        /// <summary> 合战战斗 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("frontId") || !data.ContainsKey("cityid") || !data.ContainsKey("type")) return null;
            var userid = session.Player.User.id;
            var username = session.Player.User.player_name;
            var user = session.Player.User.CloneEntity();
            // var roles = data.FirstOrDefault(m => m.Key == "roles").Value as List<ASObject>;     //测试模拟数据
            // var roles = data.FirstOrDefault(m => m.Key == "roles").Value as object[];
            var sendfrontid = data.FirstOrDefault(m => m.Key == "frontId").Value.ToString();
            var baseid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "cityid").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);

            Int32 frontid;
            Int32.TryParse(sendfrontid, out frontid);
            var lines = GetSendRoles(type, user.id);
            //验证武将
            var tuple = CheckRoles(lines); if (!tuple.Item1) return null;

            if (!Variable.FightPlan.ContainsKey(userid)) return null;
            var value = Variable.FightPlan[userid];
            var splitstring = value.Split("_");
            var planid = Convert.ToInt64(splitstring[3]); //npc城攻打时为防守地形基表id
            var defenseuserid = Convert.ToInt64(splitstring[2]);
            if (type == 0)
            {
                var queueid = Convert.ToInt64(splitstring[0]);
                //出征队列
                var queue = tg_war_battle_queue.FindByid(queueid);

                if (queue == null || frontid == 0 || !lines.Any()) return null;
                var city = tg_war_city.GetEntityByBaseId(queue.end_CityId);
                //据点处于保护时间内
                if (city.guard_time > (DateTime.Now.Ticks - 621355968000000000) / 10000) return null;
                //验证坐标
                if (!Common.GetInstance().CheckPoint(lines, planid)) return null;

                //得到战斗过程 
                var fightvo = new FightProcess().GetFightProcess(planid, lines, tuple.Item2, session.Player.User.id, queue.end_CityId, frontid, queue.morale, username);
                GetFightEnd(session.Player.CloneEntity(), city, defenseuserid, fightvo.Item2, queue);
                return BuildData(fightvo.Item2);
            }
            else
            {
                var shiqi = Common.GetInstance().GetRule("32112");
                var warfightentity = new WarFight(baseid, lines, tuple.Item2, frontid, shiqi, (int)planid,username);
                var fightvo = new FightProcess().GetFightProcess(warfightentity);
                //游戏战报
                // WarReportInsert(player.User.id, 100043, cityinfo.base_id, defenseuserid, player.User.player_name);
                return BuildData(fightvo.Item2);
            }
        }

        /// <summary>
        /// 组织数据
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        private ASObject BuildData(WarFightVo vo)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                {"fight", vo},
            };
            return new ASObject(dic);
        }


        /// <summary>    
        //验证武将是否存在
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private Tuple<bool, List<tg_war_role>> CheckRoles(List<WarRolesLinesVo> lines)
        {
            //查询武将实体
            var rids = lines.Select(q => q.rid).Distinct().ToList();
            var warrolelist = tg_war_role.GetEntityListByRids(rids);
            if (warrolelist.Any(q => q.army_soldier == 0)) return Tuple.Create(false, warrolelist);
            if (warrolelist.Count != rids.Count) return Tuple.Create(false, warrolelist);
            return Tuple.Create(true, warrolelist);
        }


        /// <summary>
        /// 解析前端的数据
        /// </summary>
        /// <param name="list">武将线路集合</param>
        /// <returns></returns>
        private List<WarRolesLinesVo> GetSendRoles(int type, Int64 userid)
        {
            var key = String.Format("{0}_{1}_", userid, type);
            var keys = Variable.WarLines.Keys.Where(m => m.Contains(key)).ToList();
            var list = new List<WarRolesLinesVo>();
            foreach (var key1 in keys)
            {
                list.Add(Variable.WarLines[key1].lines);
            }
            return list;
        }

        #region 战斗结束结算

        /// <summary>
        /// 战斗结束
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cityinfo"></param>
        /// <param name="defenseuserid"></param>
        /// <param name="fightvo"></param>
        /// <param name="bq"></param>
        /// <returns></returns>
        private bool GetFightEnd(Player player, tg_war_city cityinfo, Int64 defenseuserid, WarFightVo fightvo, tg_war_battle_queue bq)
        {
            var defenseuser = tg_user.FindByid(defenseuserid);
            var own = Variable.BASE_OFFICE.FirstOrDefault(q => q.id == player.User.office); //占有度查询
            if (own == null) return false;
            var left = own.total_own - player.UserExtend.war_total_own;
            var basesize = Variable.BASE_WARCITYSIZE.FirstOrDefault(q => q.id == cityinfo.size);
            if (basesize == null) return false;
            var defenseextend = tg_user_extend.GetByUserId(defenseuserid);
            if (fightvo.result.isWin == 1) //出征成功,攻防胜利且占有度足够
            {
                //游戏战报
                WarReportInsert(player.User.id, 100042, cityinfo.base_id, defenseuserid, player.User.player_name);
                if (left >= basesize.own) //占有度足够
                {
                    var rivalUserId = cityinfo.user_id;
                    var planlist = GetCityPlanIds(cityinfo);
                    tg_war_city_plan.GetListDelete(planlist);  //删除所有的防守方案
                    player.UserExtend.war_total_own += basesize.own; //进攻玩家占有度使用增加

                    if (defenseextend.war_total_own >= basesize.own)//防守玩家占有度使用减少
                        defenseextend.war_total_own -= basesize.own;
                    //城市资源结算，城市改为进攻方
                    CityWarResourse(cityinfo, player.User.id);
                    player.UserExtend.Update();
                    player.User.merit += (int)Math.Floor(fightvo.result.rivalDieCount * 0.5);
                    GetCityChange(defenseuserid, cityinfo, player.User.id);

                    #region 删除内政策略

                    var flag = Variable.WarCityAll.Values.Count(m => m.user_id == rivalUserId) > 0;
                    if (!flag)
                    {
                        var temp = tg_war_home_tactics.GetEntityByUserid(rivalUserId);
                        if (temp != null)
                        {
                            (new Share.War()).RemoveCD((new Share.War()).GetKey(temp.end_time, rivalUserId));
                            temp.Delete();
                        }
                    }

                    #endregion
                }
                else
                {

                    CityWarResourse(cityinfo);
                    player.User.merit += (int)Math.Floor(fightvo.result.rivalDieCount * 0.5) + basesize.addMerit;
                }
                defenseuser.merit += (int)Math.Floor(fightvo.result.rivalDieCount * 0.1);

                fightvo.result.reward.Add(new RewardVo()
                {
                    goodsType = (int)GoodsType.TYPE_MERIT,
                    value = player.User.merit,
                });
            }
            else
            {
                //游戏战报
                WarReportInsert(player.User.id, 100043, cityinfo.base_id, defenseuserid, player.User.player_name);
                player.User.merit += (int)Math.Floor(fightvo.result.rivalDieCount * 0.1);
                defenseuser.merit += (int)Math.Floor(fightvo.result.rivalDieCount * 0.5);
                fightvo.result.reward.Add(new RewardVo()
                {
                    goodsType = (int)GoodsType.TYPE_MERIT,
                    value = player.User.merit,
                });
                //  new Share.War().QueueResultCity(bq);
            }
            //进攻玩家发送协议
            if (Variable.OnlinePlayer.ContainsKey(player.User.id))
            {
                player.User.Update();
                player.UserExtend.Update();
                var session = Variable.OnlinePlayer[player.User.id] as TGGSession;
                session.Player = player;
                (new Share.User()).REWARDS_API(player.User.id, fightvo.result.reward);
            }
            //防守玩家发送协议
            if (Variable.OnlinePlayer.ContainsKey(defenseuserid))
            {
                defenseuser.Update();
                var session = Variable.OnlinePlayer[defenseuserid] as TGGSession;
                session.Player.User = defenseuser;
                session.Player.UserExtend = defenseextend;
                (new Share.User()).REWARDS_API(defenseuserid, fightvo.result.reward);
            }
            return true;
        }

        /// <summary>
        /// 合战战报生成
        /// </summary>
        /// <param name="userid">进攻玩家id</param>
        /// <param name="baseid">基表id(聊天信息表)</param>
        /// <param name="cityiid">据点基表id</param>
        /// <param name="defenseuserid">防守玩家用户id</param>
        private void WarReportInsert(Int64 userid, int baseid, int cityiid, Int64 defenseuserid, string attackname)
        {
            var attackreport = new tg_war_fight_report()
            {
                user_id = userid,
                report_time = DateTime.Now,
                base_id = baseid,
                city_id = cityiid,
            };
            attackreport.Insert();
            ReportSend(attackreport, true);

            var defensereport = new tg_war_fight_report()
            {
                user_id = defenseuserid,
                report_time = DateTime.Now,
                city_id = cityiid,
                attack_user_name = attackname,

            };
            if (baseid == 100042) defensereport.base_id = 100041;
            if (baseid == 100043) defensereport.base_id = 100040;
            defensereport.Insert();
            ReportSend(defensereport, false);

        }

        /// <summary>
        /// 发送战报推送协议
        /// </summary>
        /// <param name="report"></param>
        /// <param name="isattack">是否被攻击</param>
        private void ReportSend(tg_war_fight_report report, bool isattack)
        {

            if (Variable.OnlinePlayer.ContainsKey(report.user_id))
            {
                var vo = new WarRecordInfoVo
                {
                    id = report.id,
                    warRecordId = report.base_id,
                    times = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    replaceList = isattack ? new List<string>() { report.city_id.ToString() }
                            : new List<string>() { report.city_id.ToString(), report.attack_user_name },
                };
                var dic = new Dictionary<string, object>() { { "report", vo } };
                var session = Variable.OnlinePlayer[report.user_id] as TGGSession;
                var pv = new ProtocolVo
                {
                    serialNumber = 1,
                    verificationCode = 1,
                    moduleNumber = (int)ModuleNumber.WAR,
                    commandNumber = (int)WarCommand.WAR_REPORT_PUSH,
                    sendTime = 1000,
                    serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                    status = (int)ResponseType.TYPE_SUCCESS,
                    data = new ASObject(dic),
                };
                session.SendData(pv);
            }
        }

        /// <summary>
        /// 占有度足够城市资源结算
        /// </summary>
        /// <param name="city">城市实体</param>
        /// <param name="userid">新占领的用户id</param>
        private void CityWarResourse(tg_war_city city, Int64 userid)
        {
            city.boom *= 0.2;
            city.strong *= 0.2;
            city.peace *= 0.2;
            city.res_foods = Convert.ToInt32(Math.Floor(city.res_foods * 0.5));
            city.res_funds = Convert.ToInt32(Math.Floor(city.res_funds * 0.5));
            city.res_horse = Convert.ToInt32(Math.Floor(city.res_horse * 0.5));
            city.res_kuwu = Convert.ToInt32(Math.Floor(city.res_kuwu * 0.5));
            city.res_razor = Convert.ToInt32(Math.Floor(city.res_razor * 0.5));
            city.res_soldier = Convert.ToInt32(Math.Floor(city.res_soldier * 0.5));
            city.res_morale = Convert.ToInt32(Math.Floor(city.res_morale * 0.5));
            //计算城市新的规模
            var newsize = Variable.BASE_WARCITYSIZE.FirstOrDefault(q => q.boom >= city.boom && q.strong >= city.strong && q.peace >= city.peace);
            city.size = newsize != null ? newsize.id : Variable.BASE_WARCITYSIZE.FirstOrDefault().id;
            city.user_id = userid;
            city.plan_1 = 0;
            city.plan_2 = 0;
            city.plan_3 = 0;
            city.type = (int)WarCityType.VICECITY;
            city.guard_time = (DateTime.Now.AddDays(1).Ticks - 621355968000000000) / 10000;
            //插入防守方案表
            var newplan = new tg_war_city_plan() { user_id = userid, location = 0 };
            newplan.Insert();
            city.plan_1 = newplan.id;
            city.Update();

            Variable.WarCityAll.AddOrUpdate(city.base_id, city, (k, v) => city);
            var view = view_war_city.GetEntityByBaseId(city.base_id);
            new Share.War().SendCityBuild(view);
        }

        /// <summary>
        /// 占有度不足，城市资源处理
        /// </summary>
        /// <param name="city"></param>
        private void CityWarResourse(tg_war_city city)
        {
            city.boom *= 0.2;
            city.strong *= 0.2;
            city.peace *= 0.2;
            city.res_foods = Convert.ToInt32(Math.Floor(city.res_foods * 0.5));
            city.res_funds = Convert.ToInt32(Math.Floor(city.res_funds * 0.5));
            city.res_horse = Convert.ToInt32(Math.Floor(city.res_horse * 0.5));
            city.res_kuwu = Convert.ToInt32(Math.Floor(city.res_kuwu * 0.5));
            city.res_razor = Convert.ToInt32(Math.Floor(city.res_razor * 0.5));
            city.res_soldier = Convert.ToInt32(Math.Floor(city.res_soldier * 0.5));
            city.res_morale = Convert.ToInt32(Math.Floor(city.res_morale * 0.5));
            //计算城市新的规模
            var newsize = Variable.BASE_WARCITYSIZE.FirstOrDefault(q => q.boom >= city.boom && q.strong >= city.strong && q.peace >= city.peace);

            city.size = newsize != null ? newsize.id : Variable.BASE_WARCITYSIZE.FirstOrDefault().id;
            city.guard_time = (DateTime.Now.AddDays(1).Ticks - 621355968000000000) / 10000;
            city.Update();

            Variable.WarCityAll.AddOrUpdate(city.base_id, city, (k, v) => city);
            var view = view_war_city.GetEntityByBaseId(city.base_id);
            new Share.War().SendCityBuild(view);
        }

        /// <summary>
        /// 获取防守方防守方案id集合
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        private List<Int64> GetCityPlanIds(tg_war_city city)
        {
            var list = new List<Int64>();
            if (city.plan_1 > 0) list.Add(city.plan_1);
            if (city.plan_2 > 0) list.Add(city.plan_2);
            if (city.plan_3 > 0) list.Add(city.plan_3);
            return list;
        }

        /// <summary>
        /// 据点数据更改
        /// </summary>
        /// <param name="userid">防守据点用户id</param>
        /// <param name="city">tg_war_city</param>
        /// <param name="attackuserid">攻击者id</param>
        private void GetCityChange(Int64 userid, tg_war_city city, Int64 attackuserid)
        {
            var list = new List<tg_war_role>();
            var roles = tg_war_role.GetListByCityIdAndType(city.base_id, userid, (int)WarRoleType.PLAYER);
            var npcroles = tg_war_role.GetListByCityIdAndType(city.base_id, userid, (int)WarRoleType.NPC);
            tg_war_role.GetListDeleteByIds(npcroles); //备大将删除
            //进攻玩家插入新的备大将
            var newroles = tg_war_role.GetNpcRole(city.base_id, attackuserid);
            newroles.Insert();
            new Share.War().BreakTask(city.base_id); //关闭天守阁所有线程
            new Share.War().CarryClose(city.base_id); //运输线程结束

            var citycount = Variable.WarCityAll.Values.Count(q => q.user_id == userid);
            if (citycount <= 0) //没有其他城市
            {
                //武将数据清零，状态改为空闲
                foreach (var role in roles)
                {
                    list.Add(new tg_war_role()
                    {
                        user_id = role.user_id,
                        id = role.id,
                        rid = role.rid,
                        state = (int)WarRoleStateType.IDLE,
                    });
                    new Share.War().RoleStopTask(role); //放火破坏等结束
                }
                tg_war_role.GetListUpdate(roles);

                new Share.War().PushCityAssault(userid, attackuserid);   //推送玩家最后一个城市被攻占
            }
            else //有占领其他城市
            {
                var maincity = Variable.WarCityAll.Values.FirstOrDefault(q => q.user_id == userid && q.type == (int)WarCityType.MAINCITY);
                if (maincity == null) //没有本城
                {
                    //最大的城市作为本城
                    var maxsizecity = Variable.WarCityAll.Values.OrderByDescending(q => q.size).FirstOrDefault(q => q.user_id == userid);
                    if (maxsizecity == null) return;
                    maxsizecity.type = (int)WarCityType.MAINCITY;
                    maxsizecity.Update();
                    Variable.WarCityAll.TryUpdate(maxsizecity.base_id, maxsizecity, maxsizecity);
                    maincity = maxsizecity;
                }
                //武将回归本城
                foreach (var role in roles)
                {
                    var state = role.state;
                    if (state != (int)WarRoleStateType.DESTROY && state != (int)WarRoleStateType.FIRE
                        && state != (int)WarRoleStateType.DIPLOMATIC_RELATIONS)
                    {
                        state = (int)WarRoleStateType.IDLE;
                    }
                    list.Add(new tg_war_role()
                    {
                        user_id = role.user_id,
                        id = role.id,
                        rid = role.rid,
                        state = state,
                        station = maincity.base_id,

                    });
                }
                tg_war_role.GetListUpdate(roles);

            }
        }
        #endregion

    }
}
