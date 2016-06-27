using FluorineFx;
using System;
using System.Collections.Generic;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.User;
using System.Linq;
using tg_bag = TGG.Core.Entity.tg_bag;
using TGG.SocketServer;
using TGG.Core.Common.Util;
using NewLife.Log;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 用户系统公共方法类
    /// </summary>
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        /// <summary>获取远程客户端IP</summary>
        public string GetRemoteIP(TGGSession session)
        {
            try
            {
                return session.RemoteEndPoint.ToString();
            }
            catch { return "127.0.0.1"; }
        }


        #region 数据组装

        public Dictionary<String, Object> BuildDataUser(int result, Player player)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"userInfoVo", player != null ? AMFConvert.ToASObject(EntityToVo.ToUserInfoVo(player)) : null},
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(Player player, object module, object cmd, object data)
        {
            var dic = new Dictionary<string, object>
            {
                {"userinfo", AMFConvert.ToASObject(EntityToVo.ToUserInfoVo(player))},
                {"module", module},
                {"cmd", cmd},
                {"data", data}
            };
            return dic;
        }

        public Dictionary<String, Object> BuildData(int result, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"count", count},
            };
            return dic;
        }

        #endregion

        #region
        public List<int> GetUserOpenModule(Int64 user_id)
        {
            var list = new List<int>();
            var base_module = Variable.BASE_MODULEOPEN.Select(m => m.id).ToList();
            list.AddRange(base_module);

            //var user_module = tg_module_open.GetListByUserId(user_id).Select(m => m.module).ToList();
            //list.AddRange(user_module);

            return list;
        }

        #endregion

        #region 获取玩家信息

        /// <summary>获取玩家信息</summary>
        public Player GetPlayer(tg_user user, Int32 isAdult)
        {
            try
            {


                var user_extend = tg_user_extend.GetEntityByUserId(user.id) ?? InitUserExtend(user); //还未完成
                user_extend.fcm = isAdult;
                tg_user_extend.Save(user_extend);

                var scene = view_scene_user.GetSceneByUserid(user.id);
                var bag = InitBag(user, user_extend);
                var family = tg_family_member.GetEntityById(user.id) ?? new tg_family_member { fid = 0 };
                var blacklist = tg_friends.GetEntityListByUserIdAndState(user.id).Select(m => m.friend_id).ToList();

                var entity = new RoleItem { Kind = tg_role.GetEntityByUserId(user.id) ?? InitRole(user) };
                entity.LifeSkill = tg_role_life_skill.GetFindByRid(entity.Kind.id) ?? InitLifeSkill(entity.Kind.id);
                entity.FightSkill = tg_role_fight_skill.GetRoleSkillByRid(entity.Kind.id);

                var area = tg_user_area.GetEntityByUserId(user.id).ToList();
                if (!area.Any()) area = InitArea(user.id);
                var vip = tg_user_vip.GetEntityByUserId(user.id) ?? InitVip(user.id);
                var daminglog = tg_daming_log.GetEntityByUserId(user.id);
                if (!daminglog.Any())
                {
                    daminglog = InitDamingLog(user.id);
                }
                var war = GetWarItem(user.id);
                var player = new Player
                {
                    User = user,
                    UserExtend = user_extend,
                    Scene = scene,
                    Role = entity,
                    Bag = bag,
                    Family = family,
                    BlackList = blacklist,
                    BusinessArea = area,
                    Vip = vip,
                    DamingLog = daminglog,
                    War = war,
                };
                player.moduleIds = GetUserOpenModule(player.User.id);
                return player;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return null;
            }
        }

        /// <summary>获取玩家合战信息</summary>
        public WarItem GetWarItem(Int64 userid)
        {
            var warcity = tg_war_city.GetMainCityEntityByUserId(userid);
            var war = new WarItem
            {
                // Allys = new List<tg_war_partner>(),
                WarCityId = warcity == null ? 0 : warcity.base_id
            };
            return war;
        }

        /// <summary>获取玩家战斗信息</summary>
        public FightItem GetFight(Player player)
        {
            var personal = tg_fight_personal.GetFindByUserId(player.User.id) ?? InitFightPersonal(player);
            var fight = new FightItem
            {
                Personal = personal,
                Type = FightType.ONE_SIDE,
                BoutCount = 0,
                Rival = 0,
            };
            return fight;
        }

        #region Init 初始化

        public tg_user_vip InitVip(Int64 user_id)
        {
            var level = 0;
            var rule_vip = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1015");
            if (rule_vip != null) level = Convert.ToInt32(rule_vip.value);

            var base_vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == level);
            if (base_vip == null) return new tg_user_vip();

            var vip = new tg_user_vip
            {
                user_id = user_id,
                vip_gold = 0,
                power = base_vip.power,
                bargain = base_vip.bargain,
                buy = base_vip.buy,
                arena_buy = 0,
                arena_cd = 0,
                train_home = base_vip.trainHome,
                vip_level = base_vip.level,
                fight = base_vip.fight,
                car = base_vip.car,
            };

            vip.Save();
            return vip;
        }

        /// <summary>初始大名令表 </summary>
        public List<tg_daming_log> InitDamingLog(Int64 user_id)
        {
            var baselist = Variable.BASE_DAMING.Select(q => q.id).ToList();
            var logs = baselist.Select(i => new tg_daming_log()
                {
                    base_id = i,
                    user_id = user_id
                }).ToList();
            tg_daming_log.GetListInsert(logs);
            return logs;
        }

        /// <summary>初始化默认跑商商圈</summary>
        public List<tg_user_area> InitArea(Int64 user_id)
        {
            var _r = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1012");
            if (_r == null) return null;
            var temp = _r.value;
            var list = new List<tg_user_area>();

            if (temp.Contains(','))
            {
                var list_area = temp.Split(',');
                foreach (var item in list_area)
                {
                    var entity = new tg_user_area() { user_id = user_id, area_id = Convert.ToInt32(item) };
                    entity.Save();
                    list.Add(entity);
                }
            }
            else
            {
                var entity = new tg_user_area() { user_id = user_id, area_id = Convert.ToInt32(temp) };
                entity.Save();
                list.Add(entity);
            }
            return list;
        }

        /// <summary>初始化背包数据</summary>
        public BagItem InitBag(tg_user user, tg_user_extend extend)
        {
            var bag = new BagItem { BagIsFull = false, Surplus = 0, };

            var count = tg_bag.GetFindCount(user.id);
            bag.Surplus = extend.bag_count - count;
            bag.BagIsFull = bag.Surplus <= 0;
            return bag;
        }

        /// <summary>初始用户扩展信息</summary>
        public tg_fight_personal InitFightPersonal(Player player)
        {
            if (player.User == null || player.Role.Kind == null) return null;
            var model = new tg_fight_personal
            {
                user_id = player.User.id,
                matrix1_rid = player.Role.Kind.id,
            };
            model.Save();
            return model;
        }

        /// <summary>初始用户武将信息</summary>
        public tg_role InitRole(tg_user user)
        {
            var base_role_0 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1003");
            var base_role_1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1004");
            if (base_role_0 == null || base_role_1 == null) return null;
            var roleid = user.player_sex == (int)SexType.Male ? int.Parse(base_role_0.value) : int.Parse(base_role_1.value);
            var base_role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == roleid);
            if (base_role == null) return null;
            var identify = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == user.player_vocation);

            var model = new tg_role
            {
                user_id = user.id,
                role_id = roleid,
                role_level = 1,
                role_state = (int)RoleStateType.PROTAGONIST,
                base_captain = base_role.captain,
                base_force = base_role.force,
                base_brains = base_role.brains,
                base_charm = base_role.charm,
                base_govern = base_role.govern,
                power = base_role.power,
                att_life = base_role.life,
                role_identity = identify == null ? 0 : identify.id,
            };
            model.Save();
            return model;
        }

        /// <summary>初始用户扩展信息</summary>
        public tg_user_extend InitUserExtend(tg_user user)
        {
            if (user == null || user.id <= 0) return null;
            var _b = Variable.BASE_RULE.FirstOrDefault(q => q.id == "4001");
            var _t = Variable.BASE_RULE.FirstOrDefault(q => q.id == "17001");
            var _s = Variable.BASE_RULE.FirstOrDefault(q => q.id == "9002");
            var _p = Variable.BASE_RULE.FirstOrDefault(q => q.id == "31013");
            if (_b == null || _t == null || _s == null || _p == null) return new tg_user_extend();
            var base_bag = Convert.ToInt32(_b.value);
            var base_train_bar = Convert.ToInt32(_t.value);
            var shot_count = Convert.ToInt32(_s.value);
            var packet = Convert.ToInt32(_p.value);
            var model = new tg_user_extend { user_id = user.id, bag_count = base_bag, train_bar = base_train_bar, shot_count = shot_count, fusion_prop_packet = packet };
            model.Save();
            return model;
        }

        /// <summary>初始用户武将生活技能信息</summary>
        public tg_role_life_skill InitLifeSkill(Int64 rid)
        {
            if (rid == 0) return null;
            var model = new tg_role_life_skill
            {
                rid = rid,
                sub_archer = CommonHelper.EnumLifeType(LifeSkillType.ARCHER),
                sub_artillery = CommonHelper.EnumLifeType(LifeSkillType.ARTILLERY),
                sub_ashigaru = CommonHelper.EnumLifeType(LifeSkillType.ASHIGARU),
                sub_build = CommonHelper.EnumLifeType(LifeSkillType.BUILD),
                sub_calculate = CommonHelper.EnumLifeType(LifeSkillType.CALCULATE),
                sub_craft = CommonHelper.EnumLifeType(LifeSkillType.CRAFT),
                sub_eloquence = CommonHelper.EnumLifeType(LifeSkillType.ELOQUENCE),
                sub_equestrian = CommonHelper.EnumLifeType(LifeSkillType.EQUESTRIAN),
                sub_etiquette = CommonHelper.EnumLifeType(LifeSkillType.ETIQUETTE),
                sub_martial = CommonHelper.EnumLifeType(LifeSkillType.MARTIAL),
                sub_medical = CommonHelper.EnumLifeType(LifeSkillType.MEDICAL),
                sub_mine = CommonHelper.EnumLifeType(LifeSkillType.MINE),
                sub_ninjitsu = CommonHelper.EnumLifeType(LifeSkillType.NINJITSU),
                sub_reclaimed = CommonHelper.EnumLifeType(LifeSkillType.RECLAIMED),
                sub_tactical = CommonHelper.EnumLifeType(LifeSkillType.TACTICAL),
                sub_tea = CommonHelper.EnumLifeType(LifeSkillType.TEA)
            };
            model.Save();
            return model;
        }



        #endregion



        #endregion

    }
}
