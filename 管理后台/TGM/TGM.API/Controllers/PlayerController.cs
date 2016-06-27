using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using FluorineFx.Json;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class PlayerController : ControllerBase
    {
        //Post api/Player?token={token}&name={name}&pid={pid}&sid={sid}&type={type}&value={value}
        /// <summary>玩家详细信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="name">用户名称</param>
        /// <param name="pid">平台pid</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="type">查询类型</param>
        /// <param name="value">查询值</param>
        public PlayerDetailed PostDetailed(String token, String name, Int32 pid, Int32 sid, Int32 type, String value)
        {
            if (!IsToken(token)) return new PlayerDetailed() { result = -1, message = "令牌不存在" };

            tgm_platform.SetDbConnName(tgm_connection);
            var platform = tgm_platform.FindByid(pid);
            if (platform == null) return new PlayerDetailed() { result = -1, message = "平台信息不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null || server.pid != pid) return new PlayerDetailed() { result = -1, message = " 服务器信息不存在" };

            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.GetFindEntity(name);
            if (user.role != 10000)
            {
                if (user.pid != pid) return new PlayerDetailed() { result = -1, message = "没有权限操作该平台信息" };
            }

            SN = server.name;
            tg_user.SetDbConnName(db_connection);
            var player = type == 1 ? tg_user.GetEntityByCode(value) : tg_user.GetEntityByName(value);
            if (player == null) return new PlayerDetailed() { result = -1, message = "玩家信息不存在" };

            tg_role.SetDbConnName(db_connection);
            var role = tg_role.GetRoleByUserId(player.id);
            if (role == null) return new PlayerDetailed() { result = -1, message = "玩家主角信息不存在" };

            tg_user_vip.SetDbConnName(db_connection);
            var vip = tg_user_vip.GetByUserId(player.id);
            if (vip == null) return new PlayerDetailed() { result = -1, message = "玩家VIP信息不存在" };

            tg_car.SetDbConnName(db_connection);
            var cars = tg_car.GetCarCount(player.id);
            tg_user_login_log.SetDbConnName(db_connection);
            var log = tg_user_login_log.GetLoginLog(player.id);
            if (log == null) return new PlayerDetailed() { result = -1, message = "登陆日志信息不存在" };

            var areas = GetAreaName(player.id);
            var vocation = GetVocation(role.role_id, player.player_vocation);
            var identity = GetIdentity(role.role_identity);
            var office = GetOffice(player.office);
            var data = ToEntity.ToPlayerDetailed(player, role, areas, log.login_state, cars, vip.vip_level, identity, vocation, office);

            data = BuildData(data, player.id);
            data.sid = server.id;
            return data;
        }

        #region 玩家详细信息
        /// <summary>组装玩家信息</summary>
        private PlayerDetailed BuildData(PlayerDetailed data, Int64 playerId)
        {
            var citys = PlayerCitys(playerId);
            data.Citys = citys;
            var bags = PlayerBags(playerId);
            data.Bags = bags;

            var roles = PlayerRoles(playerId, citys);    //武将集合
            data.Roles = roles;

            data.result = 1;
            return data;
        }

        /// <summary>查询玩家武将信息</summary>
        private List<PlayerRoles> PlayerRoles(Int64 userId, List<PlayerCity> citys)
        {
            var list = new List<PlayerRoles>();
            tg_role.SetDbConnName(db_connection);

            var roles = tg_role.QueryRolesByUserId(userId);
            if (!roles.Any()) return list;

            foreach (var item in roles)
            {
                var name = Helper.FixedResources.BASE_ROLE.FirstOrDefault(m => m.id == item.role_id);
                if (name == null) continue;
                var n = name.name;
                var identity = Helper.FixedResources.BASE_IDENTITY.FirstOrDefault(m => m.id == item.role_identity);
                if (identity == null) continue;
                var i = identity.name;
                list.Add(ToEntity.ToPlayerRoles(item, n, i, name.grade));
            }

            //组装武将其他信息
            var rids = list.Select(m => m.id).ToList();
            tg_war_role.SetDbConnName(db_connection);
            var wroles = tg_war_role.GetWarRolesByIds(rids);

            if (!wroles.Any()) return list;
            foreach (var item in list)
            {
                var w = wroles.FirstOrDefault(m => m.rid == item.id);
                if (w == null) continue;
                if (w.station != 0)
                {
                    var c = citys.FirstOrDefault(m => m.baseid == w.station);
                    if (c != null) item.cityname = c.name;
                }
                item.war_status = State(w.state);
            }
            return list;
        }

        /// <summary>武将状态</summary>
        private String State(int warstate)
        {
            switch (warstate)
            {
                case 0: return "空闲";
                case 1: return "开垦";
                case 2: return "采矿";
                case 3: return "增筑";
                case 4: return "治安";
                case 5: return "建设";
                case 6: return "徵兵";
                case 7: return "训练";
                case 8: return "防守";
                case 9: return "破坏";
                case 10: return "放火";
                case 11: return "建交";
                case 12: return "出战";
                case 13: return "俘虏";
            }
            return "";
        }

        /// <summary>查询玩家背包信息</summary>
        private List<PlayerBag> PlayerBags(Int64 userId)
        {
            var list = new List<PlayerBag>();
            tg_bag.SetDbConnName(db_connection);

            var bags = tg_bag.QueryBagByUserId(userId);
            if (!bags.Any()) return list;

            foreach (var item in bags)
            {
                switch (item.type)
                {
                    case 7:   //装备表
                        {
                            var equip = Helper.FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == item.base_id);
                            if (equip == null) continue;
                            list.Add(ToEntity.ToPlayerBag(item, equip.name, equip.useLevel, equip.grade));
                        } break;
                    case 12:  //熔炼表
                        {
                            var fusion = Helper.FixedResources.BASE_FUSION.FirstOrDefault(m => m.id == item.base_id);
                            if (fusion == null) continue;
                            list.Add(ToEntity.ToPlayerBag(item, fusion.name, fusion.useLevel, fusion.grade));
                        } break;
                    default:  //道具表
                        {
                            var prop = Helper.FixedResources.BASE_PROP.FirstOrDefault(m => m.id == item.base_id);
                            if (prop == null) continue;
                            list.Add(ToEntity.ToPlayerBag(item, prop.name, prop.useLevel, prop.grade));
                        }
                        break;
                }
            }
            return list;
        }

        /// <summary>查询玩家据点信息</summary>
        private List<PlayerCity> PlayerCitys(Int64 userId)
        {
            var list = new List<PlayerCity>();
            tg_war_city.SetDbConnName(db_connection);

            var city = tg_war_city.GetCityByUserId(userId);
            if (!city.Any()) return list;

            list.AddRange(city.Select(ToEntity.ToPlayerCity));
            return list;
        }

        /// <summary>获取商圈信息</summary>
        private List<String> GetAreaName(Int64 playerId)
        {
            tg_user_area.SetDbConnName(db_connection);
            var areas = tg_user_area.GetAreas(playerId);
            var names = new List<String>();
            if (!areas.Any()) return names;
            foreach (var item in areas)
            {
                var area = Helper.FixedResources.BASE_BUSSINESS_AREA.FirstOrDefault(m => m.id == item.area_id);
                if (area == null) continue;
                names.Add(area.name);
            }
            return names;
        }

        /// <summary>查询身份</summary>
        private String GetIdentity(int id)
        {
            var rule = Helper.FixedResources.BASE_IDENTITY.FirstOrDefault(m => m.id == id);
            if (rule == null) return " ";
            var identity = rule.name;
            return identity;
        }

        /// <summary>查询职业信息</summary>
        private String GetVocation(int id, int vid)
        {
            var rule = Helper.FixedResources.BASE_ROLE.FirstOrDefault(m => m.id == id && m.jobType == vid);
            if (rule == null) return " ";
            var vocation = rule.name;
            return vocation;
        }

        /// <summary>查询官职信息</summary>
        private String GetOffice(int id)
        {
            var rule = Helper.FixedResources.BASE_OFFICE.FirstOrDefault(m => m.id == id);
            if (rule == null) return "";
            var office = rule.officeName;
            return office;
        }

        #endregion

        //Post api/Player?token={token}&sid={sid}&rid={rid}
        /// <summary>武将技能信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="rid">武将主键id</param>
        public RoleSkill PostSkills(String token, Int32 sid, Int64 rid)
        {
            if (!IsToken(token)) return new RoleSkill() { result = -1, message = "令牌不存在" };

            var server = tgm_server.FindByid(sid);
            if (server == null) return new RoleSkill() { result = -1, message = " 服务器信息不存在" };
            SN = server.name;

            tg_role_fight_skill.SetDbConnName(db_connection);
            var fskills = tg_role_fight_skill.GetListByRid(rid);
            tg_role_life_skill.SetDbConnName(db_connection);
            var lskills = tg_role_life_skill.GetLifeByRid(rid);
            if (lskills == null) return new RoleSkill() { result = -1, message = "武将生活技能信息错误" };

            var listf = new List<RoleFightSkill>();
            if (fskills.Any())
            {
                foreach (var item in fskills)
                {
                    var message = Helper.FixedResources.BASE_FIGHT_SKILL.FirstOrDefault(m => m.id == item.skill_id);
                    if (message == null) continue;
                    var name = message.name;
                    listf.Add(ToEntity.ToRoleFightSkills(item, name));
                }
            }
            var l = ToEntity.ToRoleLifeSkills(lskills);
            return new RoleSkill() { result = 1, FightSkill = listf, LifeSkill = l };
        }

        //Post api/Player?token={token}&sid={sid}&rid={rid}&name={name}
        /// <summary>武将装备信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="rid">武将主键id</param>
        /// /// <param name="name">name</param>
        public List<PlayerBag> PostEquips(String token, Int32 sid, Int64 rid, String name)
        {
            if (!IsToken(token)) return new List<PlayerBag>();

            var server = tgm_server.FindByid(sid);
            if (server == null) return new List<PlayerBag>();
            SN = server.name;

            tg_role.SetDbConnName(db_connection);
            var role = tg_role.FindByid(rid);
            if (role == null) return new List<PlayerBag>();
            return SingleRoleEquips(role);
        }

        /// <summary>武将装备信息</summary>
        private List<PlayerBag> SingleRoleEquips(tg_role role)
        {
            var ids = new List<Int64>();
            var equips = new List<PlayerBag>();
            if (role.equip_armor != 0) ids.Add(role.equip_armor);
            if (role.equip_barbarian != 0) ids.Add(role.equip_barbarian);
            if (role.equip_book != 0) ids.Add(role.equip_book);
            if (role.equip_craft != 0) ids.Add(role.equip_craft);
            if (role.equip_gem != 0) ids.Add(role.equip_gem);
            if (role.equip_mounts != 0) ids.Add(role.equip_mounts);
            if (role.equip_tea != 0) ids.Add(role.equip_tea);
            if (role.equip_weapon != 0) ids.Add(role.equip_weapon);

            if (!ids.Any()) return equips;
            tg_bag.SetDbConnName(db_connection);
            var list = tg_bag.GetBagByIds(ids).ToList();
            if (!list.Any()) return equips;

            foreach (var item in list)
            {
                var be = Helper.FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == item.base_id);
                if (be == null) continue;
                equips.Add(ToEntity.ToPlayerBag(item, be.name, be.useLevel, be.grade));
            }
            return equips;
        }

        //Post api/Player?token={token}&sid={sid}&playerId={playerId}&type={type}&name={name}&index={index}&size={size}
        /// <summary>游戏记录</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="playerId">玩家主键id</param>
        /// <param name="type">查询类型</param>
        /// <param name="name">name</param>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面数量</param>
        public PageLog PostLogs(String token, Int32 sid, Int64 playerId, Int32 type, String name, Int32 index = 1, Int32 size = 10)
        {
            var lg = new PageLog();
            var list = new List<PlayerLog>();
            if (!IsToken(token)) return new PageLog() { result = -1, message = "令牌不存在" };  //验证会话

            if (playerId == 0) return new PageLog() { result = -1, message = "请输入玩家信息" };
            if (sid == 0) return new PageLog() { result = -1, message = "请选择服务器" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PageLog() { result = -1, message = "服务器信息不存在" };

            SN = server.name;
            tg_log_operate.SetDbConnName(db_connection);

            int count;
            var entitys = tg_log_operate.GetLogEntity(playerId, type, index, size, out count).ToList();

            list.AddRange(entitys.Select(ToEntity.ToPlayerLog));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };
            lg.result = 1;
            lg.Pager = pager;
            lg.Logs = list;
            return lg;
        }

        //Post api/Player?token={token}&sid={sid}&rid={rid}&name={name}&role={role}
        /// <summary>武将属性信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务区sid</param>
        /// <param name="rid">武将主键rid</param>
        /// <param name="name">用户</param>
        /// <param name="role">权限</param>
        /// <returns></returns>
        public PlayerRoles PostRole(String token, Int32 sid, Int64 rid, String name, String role)
        {
            if (!IsToken(token)) return new PlayerRoles() { result = -1, message = "令牌不存在" };  //验证会话

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PlayerRoles() { result = -1, message = "服务器信息不存在" };
            SN = server.name;

            tg_role.SetDbConnName(db_connection);
            var userRole = tg_role.FindByid(rid);
            if (userRole == null) return new PlayerRoles() { result = -1, message = "武将信息不存在，查询失败" };

            var rule = Helper.FixedResources.BASE_ROLE.FirstOrDefault(m => m.id == userRole.role_id);
            if (rule == null) return new PlayerRoles() { result = -1, message = "查询武将基表信息失败" };
            var n = rule.name;
            var identity = Helper.FixedResources.BASE_IDENTITY.FirstOrDefault(m => m.id == userRole.role_identity);
            if (identity == null) return new PlayerRoles() { result = -1, message = "查询身份基表信息失败" }; ;
            var i = identity.name;
            var roleInfo = ToEntity.ToPlayerRoles(userRole, n, i, rule.grade);
            roleInfo.result = 1;
            return roleInfo;
        }

        //Post api/Player?token={token}&sid={sid}&playerId={playerId}&role={role}
        /// <summary>元宝消耗明细</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="playerId">玩家id</param>
        /// <param name="role">权限</param>
        public PlayerGoldPercent PostPercent(String token, Int32 sid, Int64 playerId, Int32 role)
        {
            if (!IsToken(token)) return new PlayerGoldPercent() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PlayerGoldPercent() { result = -1, message = "服务器信息不存在" };

            SN = server.name;
            tg_user.SetDbConnName(db_connection);
            var player = tg_user.FindByid(playerId);
            if (player == null) return new PlayerGoldPercent() { result = -1, message = "玩家信息不存在，查询失败" };

            tg_log_operate.SetDbConnName(db_connection);
            var logs = tg_log_operate.GetUserGoldLogs(playerId);
            if (!logs.Any()) return new PlayerGoldPercent() { result = -1, message = "查询不到元宝消耗记录" };

            var percent = new PlayerGoldPercent();
            var last = logs.LastOrDefault();
            if (last == null) return new PlayerGoldPercent() { result = 1, message = "数据错误" };

            percent.start_time = Convert.ToDateTime(last.time).ToString("yyyy-MM-dd");
            percent.total_gold = logs.Select(m => m.count).Sum();
            percent.ListLogs = ListLogs(logs, percent.total_gold);
            percent.result = 1;
            return percent;
        }

        /// <summary>分组处理元宝消耗信息</summary>
        private List<SingleTypeLog> ListLogs(List<tg_log_operate> logs, Int64 count)
        {
            var names = logs.GroupBy(m => m.command_name).ToList();
            var lists = new List<SingleTypeLog>();
            foreach (var name in names)
            {
                var list = logs.Where(m => m.command_name == name.Key).ToList();
                if (!list.Any()) continue;

                var log = new SingleTypeLog();
                if (list.Count == 1)
                {
                    log.name = list[0].command_name;
                    log.gold = list[0].count;
                    log.percent = Math.Round((Convert.ToDouble(log.gold) / Convert.ToDouble(count)) * 100, 2);
                }
                else
                {
                    var first = list[0];
                    log.gold = list.Select(m => m.count).Sum();
                    log.name = first.command_name;
                    log.percent = Math.Round((Convert.ToDouble(log.gold) / Convert.ToDouble(count)) * 100, 2);
                }
                lists.Add(log);
            }
            return lists;
        }
    }
}
