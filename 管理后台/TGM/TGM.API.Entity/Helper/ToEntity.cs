using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Model;
using GoodsType = TGG.Core.Enum.Type.GoodsType;

namespace TGM.API.Entity.Helper
{
    /// <summary>
    /// 实体对象转换类
    /// </summary>
    public class ToEntity
    {
        /// <summary>User 实体转换</summary>
        public static User ToUser(tgm_role model)
        {
            return new User
            {
                id = model.id,
                pid = model.pid,
                role = model.role,
                name = model.name,
                createtime = model.createtime,
                token = model.Platform.token,
                platform_name = model.Platform.name,
            };
        }

        /// <summary>Server 实体转换</summary>
        public static Server ToServer(tgm_server model)
        {
            var server = new Server
            {
                id = model.id,
                pid = model.pid,
                ip = model.ip,
                name = model.name,
                port_policy = model.port_policy,
                port_server = model.port_server,
                connect_string = model.connect_string,
                createtime = model.createtime,
                platform_name = model.Platform.name,
                tg_pay = model.tg_pay,
                tg_route = model.tg_route,
                game_domain = model.game_domain,
                game_pay = model.game_pay,
                server_open = Convert.ToDateTime(model.server_open).ToString("yyyy-MM-dd HH:mm:ss"),
                test_url = model.test_url,
                server_state = model.server_state,
            };
            switch (model.server_state)
            {
                case (int)ServerOpenState.未启服: server.state_name = "未启服"; break;
                case (int)ServerOpenState.停服: server.state_name = "停服"; break;
                case (int)ServerOpenState.测试: server.state_name = "测试"; break;
                case (int)ServerOpenState.启服: server.state_name = "启服"; break;
            }
            return server;
        }

        /// <summary>Email 实体转换</summary>
        public static Email ToEmail(tg_messages model)
        {
            return new Email()
            {
                id = model.id,
                title = model.title,
                content = model.contents,
                createtime = model.create_time,
            };
        }

        /// <summary>Notice 实体转换</summary>
        public static Notice ToNotice(tgm_notice model)
        {
            return new Notice()
            {
                id = model.id,
                content = model.content,
                start_time = DateTime.FromBinary(long.Parse(Convert.ToString(model.start_time))),
                end_time = DateTime.FromBinary(long.Parse(Convert.ToString(model.end_time))),
                pid = model.pid,
                sid = model.sid,

            };
        }

        /// <summary>Platform 实体转换</summary>
        public static Platform ToPlatform(tgm_platform model)
        {
            return new Platform
            {
                id = model.id,
                name = model.name,
                token = model.token,
                encrypt = model.encrypt,
                createtime = model.createtime,
            };
        }

        /// <summary>GmManage 实体转换</summary>
        public static GmManage ToGmManage(tgm_gm model)
        {
            return new GmManage()
            {
                id = model.id,
                pid = model.pid,
                sid = model.sid,
                player_id = model.player_id,
                state = model.state,
                player_name = model.player_name,
                player_code = model.player_code,
                platform_name = model.platform_name,
                server_name = model.server_name,
                limit_time = model.limit_time,
                describe = model.describe,
                createtime = DateTime.FromBinary(long.Parse(Convert.ToString(model.createtime))),
                operate = model.operate,
            };
        }


        /// <summary>Resource 实体转换</summary>
        public static Resource ToResource(tgm_resource model, string goods)
        {
            return new Resource()
            {
                id = model.id,
                time = DateTime.FromBinary(long.Parse(Convert.ToString(model.time))),
                type = model.type,
                state = model.state,
                content = model.content,
                sid = model.sid,
                pid = model.pid,
                goods = goods,
                operation = model.operation,
                player = string.Format("账号:{0} 角色名:{1}", model.user_code,model.player_name)
            };
        }



        /// <summary>PlayerDetailed 实体转换</summary>
        public static PlayerDetailed ToPlayerDetailed(tg_user model, tg_role role, List<String> areas, int loginstate, int cars, int viplevel, string identity, string vocation, string office)
        {
            var player = new PlayerDetailed()
            {
                id = model.id,
                rid = role.id,
                code = model.user_code,
                name = model.player_name,
                vocation = vocation,
                login_state = loginstate == 0 ? "不在线" : "在线",
                vip = viplevel,
                identity = identity,
                level = role.role_level,
                office = office,
                gold = model.gold,
                coin = model.coin,
                spirit = model.spirit,
                fame = model.fame,
                merit = model.merit,
                honor = role.total_honor,
                cars = cars,
                Areas = areas,
            };
            return player;
        }

        /// <summary>PlayerRoles 实体转换</summary>
        public static PlayerRoles ToPlayerRoles(tg_role model, string name, string identity, int grade)
        {
            var role = new PlayerRoles()
            {
                id = model.id,
                name = name,
                roleid = model.role_id,
                state = model.role_state,
                level = model.role_level,
                identity = identity,
                equip_weapon = model.equip_weapon,
                equip_barbarian = model.equip_barbarian,
                equip_mounts = model.equip_mounts,
                equip_armor = model.equip_armor,
                equip_gem = model.equip_gem,
                equip_tea = model.equip_tea,
                equip_craft = model.equip_craft,
                equip_book = model.equip_book,
                captain = tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, model),
                force = tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, model),
                brains = tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, model),
                govern = tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, model),
                charm = tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, model),
            };
            switch (grade)
            {
                case 1: role.quality = "白"; break;
                case 2: role.quality = "蓝"; break;
                case 3: role.quality = "紫"; break;
                case 4: role.quality = "橙"; break;
                case 5: role.quality = "红"; break;
            }
            return role;
        }

        /// <summary>RoleFightSkills 实体转换</summary>
        public static RoleFightSkill ToRoleFightSkills(tg_role_fight_skill model, string name)
        {
            var skill = new RoleFightSkill()
            {
                id = model.id,
                name = name,
                baseid = model.skill_id,
                level = model.skill_level,
                genre = model.skill_genre,
            };
            switch (model.skill_genre)
            {
                case 1: skill.genreName = "流派通用"; break;
                case 2: skill.genreName = "宝藏院流"; break;
                case 3: skill.genreName = "佐分利流"; break;
                case 4: skill.genreName = "二天一流"; break;
                case 5: skill.genreName = "吉冈流"; break;
                case 6: skill.genreName = "林崎梦想流"; break;
                case 7: skill.genreName = "柳生新阴流"; break;
                case 8: skill.genreName = "体舍流"; break;
                case 9: skill.genreName = "香取神道流"; break;
                case 10: skill.genreName = "新阴流"; break;
                case 11: skill.genreName = "严流"; break;
                case 12: skill.genreName = "一刀流"; break;
                case 13: skill.genreName = "钵屋众"; break;
                case 14: skill.genreName = "风魔众"; break;
                case 15: skill.genreName = "根来众"; break;
                case 16: skill.genreName = "户隐众"; break;
                case 17: skill.genreName = "甲贺众"; break;
                case 18: skill.genreName = "山潜众"; break;
                case 19: skill.genreName = "透波众"; break;
                case 20: skill.genreName = "轩猿众"; break;
                case 21: skill.genreName = "伊贺众"; break;
                case 22: skill.genreName = "忍者众通用"; break;
            }
            return skill;
        }

        /// <summary>RoleLifeSkill 实体转换</summary>
        public static RoleLifeSkill ToRoleLifeSkills(tg_role_life_skill model)
        {
            return new RoleLifeSkill()
            {
                id = model.id,
                sub_tea = model.sub_tea,
                sub_calculate = model.sub_calculate,
                sub_build = model.sub_build,
                sub_eloquence = model.sub_eloquence,
                sub_equestrian = model.sub_equestrian,
                sub_reclaimed = model.sub_reclaimed,
                sub_ashigaru = model.sub_ashigaru,
                sub_artillery = model.sub_artillery,
                sub_mine = model.sub_mine,
                sub_craft = model.sub_craft,
                sub_archer = model.sub_archer,
                sub_etiquette = model.sub_etiquette,
                sub_martial = model.sub_martial,
                sub_tactical = model.sub_tactical,
                sub_medical = model.sub_medical,
                sub_ninjitsu = model.sub_ninjitsu,
                sub_tea_level = model.sub_tea_level,
                sub_calculate_level = model.sub_calculate_level,
                sub_build_level = model.sub_build_level,
                sub_eloquence_level = model.sub_eloquence_level,
                sub_equestrian_level = model.sub_equestrian_level,
                sub_reclaimed_level = model.sub_reclaimed_level,
                sub_ashigaru_level = model.sub_ashigaru_level,
                sub_artillery_level = model.sub_artillery_level,
                sub_mine_level = model.sub_mine_level,
                sub_craft_level = model.sub_craft_level,
                sub_archer_level = model.sub_archer_level,
                sub_etiquette_level = model.sub_etiquette_level,
                sub_martial_level = model.sub_martial_level,
                sub_tactical_level = model.sub_tactical_level,
                sub_medical_level = model.sub_medical_level,
                sub_ninjitsu_level = model.sub_ninjitsu_level,
            };
        }

        /// <summary>PlayerBag 实体转换</summary>
        public static PlayerBag ToPlayerBag(tg_bag model, string name, int useLevel, int grade)
        {
            var bag = new PlayerBag()
            {
                id = model.id,
                name = name,
                baseid = model.base_id,
                count = model.count,
                level = useLevel,
                type = model.type,
                equip_type = model.equip_type,
                attribute1_type = model.attribute1_type,
                attribute1_value_spirit = model.attribute1_value_spirit,
                attribute2_type = model.attribute2_type,
                attribute2_value_spirit = model.attribute2_value_spirit,
                attribute3_type = model.attribute3_type,
                attribute3_value_spirit = model.attribute3_value_spirit,
            };
            if (bag.type == 7)
            {
                switch (bag.equip_type)
                {
                    case 1: bag.position = "武器"; break;
                    case 2: bag.position = "铠甲"; break;
                    case 3: bag.position = "坐骑"; break;
                    case 4: bag.position = "茶器"; break;
                    case 5: bag.position = "书籍"; break;
                    case 6: bag.position = "南蛮物"; break;
                    case 7: bag.position = "艺术品"; break;
                    case 8: bag.position = "珠宝"; break;
                }
                if (model.attribute1_type != 0) { bag.attribute1_name = Name(model.attribute1_type); }
                if (model.attribute2_type != 0) { bag.attribute2_name = Name(model.attribute2_type); }
                if (model.attribute3_type != 0) { bag.attribute3_name = Name(model.attribute3_type); }
            }
            switch (grade)
            {
                case 1: bag.quality = "绿"; break;
                case 2: bag.quality = "蓝"; break;
                case 3: bag.quality = "紫"; break;
                case 4: bag.quality = "橙"; break;
                case 5: bag.quality = "红"; break;
            }
            return bag;
        }

        private static string Name(int type)
        {
            switch (type)
            {
                case 1: return "统率";
                case 2: return "武力";
                case 3: return "智谋";
                case 4: return "政务";
                case 5: return "魅力";
                case 6: return "攻击力";
                case 7: return "增伤";
                case 8: return "防御力";
                case 9: return "减伤";
                case 10: return "生命值";
            }
            return "";
        }

        /// <summary>PlayerCity 实体转换</summary>
        public static PlayerCity ToPlayerCity(tg_war_city model)
        {
            var city = new PlayerCity()
            {
                id = model.id,
                baseid = model.base_id,
                name = model.name,
                res_foods = model.res_foods,
                res_funds = model.res_funds,
                res_soldier = model.res_soldier,
                res_gun = model.res_gun,
                res_horse = model.res_horse,
                res_razor = model.res_razor,
                res_kuwu = model.res_kuwu,
                res_morale = model.res_morale,
                peace = model.peace,
                strong = model.strong,
                boom = model.boom,
            };
            switch (model.size)
            {
                case 1: city.size = "村"; break;
                case 2: city.size = "小城"; break;
                case 3: city.size = "大城"; break;
                case 4: city.size = "中城"; break;
                case 5: city.size = "巨城"; break;
            }
            return city;
        }

        /// <summary>TotalRecordPay 实体转换</summary>
        public static TotalRecordPay ToTotalRecordPay(int count, string platform, string server, double paytotal, Int64 logtime, string playername, Int64 paytime)
        {
            return new TotalRecordPay()
            {
                platform = platform,
                server = server,
                playername = playername,
                paytotal = Convert.ToInt32(paytotal),
                count = count,
                paytime = Convert.ToString(DateTime.FromBinary(long.Parse(Convert.ToString(paytime)))),
                logintime = Convert.ToString(DateTime.FromBinary(long.Parse(Convert.ToString(logtime)))),
            };
        }

        /// <summary>SingleRecordPay 实体转换</summary>
        public static SingleRecordPay ToSingleRecordPay(string platform, string server, tgm_record_pay model)
        {
            return new SingleRecordPay()
            {
                platform = platform,
                server = server,
                playername = model.player_name,
                pay = model.money,
                gold = model.amount,
                paytime = Convert.ToString(DateTime.FromBinary(long.Parse(Convert.ToString(model.createtime)))),
                order = model.order_id,
            };
        }

        /// <summary>PlayerLog 实体转换</summary>
        public static PlayerLog ToPlayerLog(tg_log_operate model)
        {
            return new PlayerLog()
            {
                id = model.id,
                module_number = model.module_number,
                module_name = model.module_name,
                command_number = model.command_number,
                command_name = model.command_name,
                changes_type = model.type,
                resources_type = model.resource_type,
                resources_name = model.resource_name,
                count = model.count,
                surplus = model.surplus,
                time = Convert.ToDateTime(model.time).ToString("yyyy-MM-dd HH:mm:ss tt"),
            };
        }
        #region RecordServer 实体转换

        /// <summary>RecordServer 实体转换</summary>
        public static RecordServer ToRecordServer(tgm_record_server model)
        {
            var d = DateTime.Now.Ticks - model.createtime;
            var days = Convert.ToInt32(new TimeSpan(d).TotalDays);
            var time = new DateTime(model.createtime).ToString("yyyy-MM-dd HH:mm:ss");
            var apru = 0.0;//当日总收入/付费人数
            if (model.pay_number != 0 && model.pay_taday != 0)
            {
                apru = Math.Round(Convert.ToDouble(model.pay_taday) / Convert.ToDouble(model.pay_number), 2);
            }
            var cost_rate = 0.00;//每天的总消耗的元宝数量/单服每天充值的元宝数量*100%
            if (model.taday_cost != 0 && model.pay_taday != 0)
            {
                cost_rate = Math.Round(Convert.ToDouble(model.taday_cost) / Convert.ToDouble(model.pay_taday), 2);
            }
            return new RecordServer
            {
                pid = model.pid,
                sid = model.sid,
                server_name = model.server_name,
                offline = model.offline,
                online = model.online,
                history_online = model.history_online,
                register = model.register,
                register_total = model.register_total,
                taday_login = model.taday_login,
                taday_online = model.taday_online,
                pay_count = model.pay_count,
                pay_number = model.pay_number,
                pay_taday = model.pay_taday,
                pay_total = model.pay_total,
                pay_month = model.pay_month,
                createtime = time,
                total_days = days,
                apru = apru,
                taday_cost = model.taday_cost,
                cost_rate = cost_rate,
            };
        }

        /// <summary>RecordServer 实体转换</summary>
        public static RecordServer ToRecordServer(tgm_record_day model)
        {
            var d = DateTime.Now.Ticks - model.createtime;
            var days = Convert.ToInt32(new TimeSpan(d).TotalDays);
            var time = new DateTime(model.createtime).ToString("yyyy-MM-dd HH:mm:ss");
            var apru = 0.0;//当日总收入/付费人数
            if (model.pay_number != 0 && model.pay_taday != 0)
            {
                apru = Math.Round(Convert.ToDouble(model.pay_taday) / Convert.ToDouble(model.pay_number), 2);
            }
            var cost_rate = 0.00;//每天的总消耗的元宝数量/单服每天充值的元宝数量
            if (model.taday_cost != 0 && model.pay_taday != 0)
            {
                cost_rate = Math.Round(Convert.ToDouble(model.taday_cost) / Convert.ToDouble(model.pay_taday), 2);
            }
            return new RecordServer
            {
                pid = model.pid,
                sid = model.sid,
                server_name = model.server_name,
                offline = model.offline,
                online = model.online,
                history_online = model.history_online,
                register = model.register,
                register_total = model.register_total,
                taday_login = model.taday_login,
                taday_online = model.taday_online,
                pay_count = model.pay_count,
                pay_number = model.pay_number,
                pay_taday = model.pay_taday,
                pay_total = model.pay_total,
                pay_month = model.pay_month,
                createtime = time,
                total_days = days,
                apru = apru,
                taday_cost = model.taday_cost,
                cost_rate = cost_rate,
            };
        }

        /// <summary>RecordServer 实体转换</summary>
        public static RecordServer ToRecordServer(tgm_record_hours model)
        {
            var d = DateTime.Now.Ticks - model.createtime;
            var days = Convert.ToInt32(new TimeSpan(d).TotalDays);
            var time = new DateTime(model.createtime).ToString("yyyy-MM-dd HH:mm:ss");
            var apru = 0.0;//当日总收入/付费人数
            if (model.pay_number != 0 && model.pay_taday != 0)
            {
                apru = Math.Round(Convert.ToDouble(model.pay_taday) / Convert.ToDouble(model.pay_number), 2);
            }
            var cost_rate = 0.00;//每天的总消耗的元宝数量/单服每天充值的元宝数量
            if (model.taday_cost != 0 && model.pay_taday != 0)
            {
                cost_rate = Math.Round(Convert.ToDouble(model.taday_cost) / Convert.ToDouble(model.pay_taday), 2);
            }
            return new RecordServer
            {
                pid = model.pid,
                sid = model.sid,
                server_name = model.server_name,
                offline = model.offline,
                online = model.online,
                history_online = model.history_online,
                register = model.register,
                register_total = model.register_total,
                taday_login = model.taday_login,
                taday_online = model.taday_online,
                pay_count = model.pay_count,
                pay_number = model.pay_number,
                pay_taday = model.pay_taday,
                pay_total = model.pay_total,
                pay_month = model.pay_month,
                createtime = time,
                total_days = days,
                apru = apru,
                taday_cost = model.taday_cost,
                cost_rate = cost_rate,
            };
        }

        #endregion

        /// <summary>ToIdentitySpread 实体转换</summary>
        public static IdentitySpread ToIdentitySpread(report_identity_day model)
        {
            return new IdentitySpread()
            {
                id = model.id,
                identity1_count = model.identity1_count,
                identity2_count = model.identity2_count,
                identity3_count = model.identity3_count,
                identity4_count = model.identity4_count,
                identity5_count = model.identity5_count,
                identity6_count = model.identity6_count,
                identity7_count = model.identity7_count,
                createtime = new DateTime(model.createtime).ToString("yyyy-MM-dd"),
            };
        }

        /// <summary>ToLevelSpread 实体转换</summary>
        public static LevelSpread ToLevelSpread(report_level_day model)
        {
            return new LevelSpread()
            {
                id = model.id,
                stage1_count = model.stage1_count,
                stage2_count = model.stage2_count,
                stage3_count = model.stage3_count,
                stage4_count = model.stage4_count,
                stage5_count = model.stage5_count,
                stage6_count = model.stage6_count,
                stage7_count = model.stage7_count,
                stage8_count = model.stage8_count,
                total_count = model.total_count,
                percent1 = Math.Round((Convert.ToDouble(model.stage1_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent2 = Math.Round((Convert.ToDouble(model.stage2_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent3 = Math.Round((Convert.ToDouble(model.stage3_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent4 = Math.Round((Convert.ToDouble(model.stage4_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent5 = Math.Round((Convert.ToDouble(model.stage5_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent6 = Math.Round((Convert.ToDouble(model.stage6_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent7 = Math.Round((Convert.ToDouble(model.stage7_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                percent8 = Math.Round((Convert.ToDouble(model.stage8_count) / Convert.ToDouble(model.total_count)) * 100, 2),
                createtime = new DateTime(model.createtime).ToString("yyyy-MM-dd"),
            };
        }

        /// <summary>ToServerGoldConsume 实体转换</summary>
        public static ServerGoldConsume ToServerGoldConsume(tgm_record_day model)
        {
            var serverGold = new ServerGoldConsume()
            {
                id = model.id,
                sid = model.sid,
                recharge_count = model.pay_taday,
                recharge_people = model.pay_number,
                consume = model.taday_cost,
                createtime = new DateTime(model.createtime).ToString("yyyy-MM-dd"),
            };
            if (model.pay_taday == 0 || model.taday_cost == 0)
            {
                serverGold.percent = 0;
            }
            else
            {
                serverGold.percent = Math.Round((Convert.ToDouble(model.taday_cost) / Convert.ToDouble(model.pay_taday)) * 100, 2);
            }
            return serverGold;
        }

        /// <summary>RecordKeep 实体转换</summary>
        public static RecordKeep ToRecordKeep(tgm_record_keep model)
        {
            var time = new DateTime(model.createtime).ToString("yyyy-MM-dd HH:mm:ss");
            return new RecordKeep
            {
                id = model.id,
                pid = model.pid,
                sid = model.sid,
                server_name = model.server_name,
                keep_1 = model.keep_1,
                keep_3 = model.keep_3,
                keep_5 = model.keep_5,
                keep_7 = model.keep_7,
                keep_30 = model.keep_30,
                login_30 = model.login_30,
                createtime = time,
            };
        }

        /// <summary>ServerPlayer 实体转换</summary>
        public static ServerPlayer ToServerPlayer(view_player_detail model, string identity)
        {
            return new ServerPlayer()
            {
                id = model.id,
                code = model.user_code,
                name = model.player_name,
                level = model.role_level,
                vip = model.vip_level,
                identity = identity,
                coin = model.coin,
                gold = model.gold,
                vip_gold = model.vip_gold,
                login_time = new DateTime(model.login_time).ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        /// <summary>ToServerGoodsCode 实体转换</summary>
        public static ServerGoodsCode ToServerGoodsCode(tgm_goods_code model)
        {
            var goodscode = new ServerGoodsCode()
            {
                id = model.id,
                platform_name = model.platform_name,
                card_key = model.card_key,
                kind = model.kind,
                type = model.GoodsType.name
            };
            return goodscode;
        }

        /// <summary>ToServerCodeLog 实体转换</summary>
        public static ServerCodeLog ToServerCodeLog(tgm_give_log model)
        {
            return new ServerCodeLog()
            {
                id = model.id,
                platform_name = model.platform_name,
                server_name = model.server_name,
                kind = model.kind,
                type = model.GoodsType.name,
                time = Convert.ToDateTime(model.createtime).ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        /// <summary>ToReportCode 实体转换</summary>
        public static ReportCode ToReportCode(tgm_goods_code model)
        {
            var report = new ReportCode()
            {
                平台名称 = model.platform_name,
                福利卡类型 = model.GoodsType.name,
                激活码 = model.card_key,
                生成序号 = model.kind,

            };
            return report;
        }

        /// <summary>ToGoodsType 实体转换</summary>
        public static Model.GoodsType ToGoodsType(tgm_goods_type model)
        {
            return new Model.GoodsType()
            {
                id = model.id,
                type_id = model.type_id,
                name = model.name,
            };
        }
    }
}
