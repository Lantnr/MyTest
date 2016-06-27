using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.Module.Consume;

namespace TGG.Module.War.Service.Fight
{
    public partial class WarFight
    {
        public WarFight(Int64 planid, List<WarRolesLinesVo> attackrolRolesVos, List<tg_war_role> attackroles, Int64 attackUserId, Int32 cityid, Int32 frontid, int morale)
        {
            #region 防守数据初始

            var cityinfo = tg_war_city.GetEntityByBaseId(cityid);
            if (cityinfo == null) { isInitSuccess = false; return; }
            if (planid != 0)
            {
                //防守地形设定
                Area = GetDefenseAreaInit(planid);
                var plan = tg_war_city_plan.GetEntityByPlanId(planid);
                //防守阵
                DefenseFrontId = (int)plan.formation;
                // 防守武将
                GetDefenseRolesInit(planid, cityinfo.res_morale);
                //初始武将防守范围
                DefenseRange = Common.GetInstance().GetDefenseRangeInit(DefenseRoles);
            }


            Area = new List<Common.MapArea>();

            if (!isInitSuccess) { isInitSuccess = false; return; }
            //城门本丸初始
            GetDoorAndCityInit(cityinfo);

            #endregion

            #region 进攻数据初始

            GetAttackRolesInit(attackrolRolesVos, attackroles, morale);
            AttackFrontId = frontid;
            #endregion

            //初始天气持续回合
            WeatherState = GetWeatherStateInit();
            //初始五常持续回合
            FiveState = GetFiveStateInit();

            //武将出手顺序初始
            AttackSort = GetRolesSort();
        }

        public bool isInitSuccess = true;

        /// <summary>防守地形设定 </summary>
        public List<Common.MapArea> Area = new List<Common.MapArea>();

        /// <summary> 防守方阵id</summary>
        public int DefenseFrontId { get; set; }

        /// <summary> 进攻方阵id</summary>
        public int AttackFrontId { get; set; }

        /// <summary> 进攻武将</summary>
        public List<AttackRoles> AttackRoles = new List<AttackRoles>();

        /// <summary>防守武将 </summary>
        public List<DefenseRoles> DefenseRoles = new List<DefenseRoles>();

        /// <summary>武将防守范围 </summary>
        public List<DefenseRange> DefenseRange = new List<DefenseRange>();

        /// <summary> 城门</summary>
        public DefenseRoles Door { get; set; }

        /// <summary> 本丸</summary>
        public DefenseRoles City { get; set; }

        /// <summary> 天气 </summary>
        public Int32 Weather { get; set; }

        /// <summary> 回合 </summary>
        public Int32 Times { get; set; }

        /// <summary>五常 </summary>
        public Int32 FiveSharp { get; set; }

        /// <summary>出手顺序 </summary>
        public List<AttackSort> AttackSort { get; set; }

        /// <summary> 天气持续回合数 </summary>
        public Int32 WeatherState { get; set; }

        /// <summary> 五常持续回合数 </summary>
        public Int32 FiveState { get; set; }

        /// <summary> 防守方总兵数 </summary>
        public Int32 DefenseSoldierCount { get; set; }

        /// <summary> 进攻方总兵数 </summary>
        public Int32 AttackSoldierCount { get; set; }
        #region 初始数据


        /// <summary>
        ///初始防守地形
        /// </summary>
        /// <param name="planid">防守方案id</param>
        /// <returns></returns>
        private List<Common.MapArea> GetDefenseAreaInit(Int64 planid)
        {
            var planset = tg_war_plan_area.GetEntityByPlanId(planid);

            //防守地形转换成战争地形
            return Area = new Common().GetMapArea(planset);
        }

        /// <summary>
        ///初始防守武将
        /// </summary>
        /// <param name="planid">防守方案id</param>
        /// <returns></returns>
        private void GetDefenseRolesInit(Int64 planid, int morale)
        {
            //城市布防的武将
            var d_roles = tg_war_city_defense.GetListByPlanId(planid);

            var rolelist = new List<tg_role>();
            //合战武将实体
            var warrolelist = tg_war_role.GetEntityListByIds(d_roles.Select(m => m.role_id).ToList());
            //非备大将武将实体
            var fightroles = warrolelist.Where(q => q.type == (int)WarRoleType.PLAYER).Select(m => m.rid).ToList();
            if (fightroles.Any())
                rolelist = tg_role.GetRoleByIds(fightroles);


            var selectroles = d_roles.Select(q =>
            {
                //合战武将实体
                var tgWarRole = warrolelist.FirstOrDefault(n => n.id == q.role_id);
                if (tgWarRole == null) { isInitSuccess = false; return null; }
                if (tgWarRole.army_soldier <= 0) return null;
                //兵种基表
                var basesoldier = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(m => m.id == tgWarRole.army_id);
                if (basesoldier == null) { isInitSuccess = false; return null; }
                //个人战武将实体
                var roleEntity = rolelist.FirstOrDefault(m => m.id == tgWarRole.rid);
                var baseid = tgWarRole.type == (int)WarRoleType.NPC ? tgWarRole.rid : roleEntity.role_id;
                //武将表基表
                var baserole = Variable.BASE_ROLE.FirstOrDefault(n => n.id == baseid);
                if (baserole == null) return null;
                return new DefenseRoles()
                    {
                        morale = morale,
                        Role = roleEntity ?? new tg_role()
                        {
                            base_brains = baserole.brains,
                            role_id = (int)tgWarRole.rid,
                        },
                        isguard = rolelist.FirstOrDefault(m => m.id == q.role_base_id) == null,
                        RoleId = q.role_id,
                        WarRole = tgWarRole,
                        X = q.point_x,
                        Y = q.point_y,
                        type = (int)WarFightRoleType.伏兵, //初始都是伏兵
                        SoldierId = tgWarRole.army_id,     //兵种基表id
                        SoldierCount = tgWarRole.army_soldier,//兵种数量即血量
                        bloodMax = tgWarRole.army_soldier,
                        BasePoint = new Common.Point(),
                        attack = basesoldier.baseAttack,  //基础进攻值
                        defense = basesoldier.baseDefence,//基础防御值
                        crit = basesoldier.crit,          //基础暴击几率
                        hits = basesoldier.hits,            //初始命中率
                        hurtRange = basesoldier.hurtRange,
                        roleName = baserole.name,
                    };
            }).ToList();
            selectroles.RemoveAll(q => q == null);
            //初始武将战争地图中的坐标
            GetBasePointInit(selectroles);
            DefenseRoles = selectroles;
            DefenseSoldierCount = selectroles.Sum(q => q.SoldierCount);
            DefenseInfluenceAdd();
            //装备初始
            EquipInit();
            //技能初始
            EffectInit();

        }

        /// <summary>
        /// 防守武将势力特性更改
        /// </summary>
        private void DefenseInfluenceAdd()
        {
            //势力特性
            for (int i = 0; i < DefenseRoles.Count; i++)
            {
                var user = tg_user.FindByid(DefenseRoles[i].Role.user_id);
                if (user.player_influence == (int)InfluenceType.ZhiTian)
                {
                    if (DefenseRoles[i].SoldierId != 10001) continue;
                    DefenseRoles[i].attack += Common.GetInstance().GetRule("32103");
                }
                if (user.player_influence == (int)InfluenceType.DeChuan)
                {
                    if (DefenseRoles[i].SoldierId != 10002) continue;
                    DefenseRoles[i].attack += Common.GetInstance().GetRule("32104");
                }
                if (user.player_influence == (int)InfluenceType.WuTian)
                {
                    if (DefenseRoles[i].SoldierId != 10005) continue;
                    DefenseRoles[i].attack += Common.GetInstance().GetRule("32105");
                }
                if (user.player_influence == (int)InfluenceType.Shangshan)
                {
                    if (DefenseRoles[i].SoldierId != 10009) continue;
                    DefenseRoles[i].dodge += Common.GetInstance().GetRule("32106");
                }
                if (user.player_influence == (int)InfluenceType.DaoJin)
                {
                    if (DefenseRoles[i].SoldierId != 10003) continue;
                    DefenseRoles[i].attack += Common.GetInstance().GetRule("32107");
                    //  DefenseRoles[i].speed = Common.GetInstance().GetRule("32108");
                }
                if (user.player_influence == (int)InfluenceType.YiDa)
                {
                    if (DefenseRoles[i].SoldierId != 10007) continue;
                    DefenseRoles[i].attack += Common.GetInstance().GetRule("32109");
                }
            }
        }

        /// <summary>
        /// 初始防守武将的装备
        /// </summary>
        private void EquipInit()
        {
            var ids = new List<Int64>(); //装备主键id集合
            var equiproles = DefenseRoles.Where(q => q.isguard == false && q.Role.equip_mounts > 0).ToList();

            if (!equiproles.Any()) return; //没有武将有合战装备

            ids.AddRange(equiproles.Select(q => q.Role.equip_mounts).ToList());
            if (!ids.Any()) return;
            var equiplist = tg_bag.GetWarEquips(ids);

            for (int i = 0; i < equiproles.Count; i++)
            {
                var equip = equiplist.FirstOrDefault(q => q.id == equiproles[i].Role.equip_mounts);
                if (equip == null) continue;
                var r = GetWarRoleEquip(equip);

                equiproles[i].EquipAddAttack += r.Item1; //装备增加的攻击力
                equiproles[i].EquipAddDefense += r.Item2; //装备增加的防御力
            }

        }

        #region Arlen 2015-02-14 合战装备属性

        /// <summary>获取合战武将装备合战属性</summary>
        /// <param name="equip">合战武将装备</param>
        /// <returns>Item1:合战攻击力,Item2:合战防御力</returns>
        private Tuple<Int32, Int32> GetWarRoleEquip(tg_bag equip)
        {
            var war_attack = 0;
            var war_defense = 0;

            #region 统计合战攻击力
            if (equip.attribute1_type == (int)RoleAttributeType.ROLE_WAR_ATTACK)
            {
                war_attack += Convert.ToInt32(equip.attribute1_value + equip.attribute1_value_spirit);
            }
            if (equip.attribute2_type == (int)RoleAttributeType.ROLE_WAR_ATTACK)
            {
                war_attack += Convert.ToInt32(equip.attribute2_value + equip.attribute2_value_spirit);
            }
            if (equip.attribute3_type == (int)RoleAttributeType.ROLE_WAR_ATTACK)
            {
                war_attack += Convert.ToInt32(equip.attribute3_value + equip.attribute3_value_spirit);
            }
            #endregion

            #region 统计合战防御力
            if (equip.attribute1_type == (int)RoleAttributeType.ROLE_WAR_DEFENSE)
            {
                war_defense += Convert.ToInt32(equip.attribute1_value + equip.attribute1_value_spirit);
            }
            if (equip.attribute2_type == (int)RoleAttributeType.ROLE_WAR_DEFENSE)
            {
                war_defense += Convert.ToInt32(equip.attribute2_value + equip.attribute2_value_spirit);
            }
            if (equip.attribute3_type == (int)RoleAttributeType.ROLE_WAR_DEFENSE)
            {
                war_defense += Convert.ToInt32(equip.attribute3_value + equip.attribute3_value_spirit);
            }
            #endregion
            return Tuple.Create(war_attack, war_defense);
        }


        #endregion

        #region 初始技能
        /// <summary>
        /// 技能初始
        /// </summary>
        public void EffectInit()
        {
            var ids = new List<Int64>(); //忍者众主键id集合

            //合战奥义
            var mysteryroles = DefenseRoles.Where(q => q.isguard == false && q.Role.art_war_mystery > 0).ToList();
            //合战秘技
            var cheatroles = DefenseRoles.Where(q => q.isguard == false && q.Role.art_war_cheat_code > 0).ToList();
            //忍者众秘技
            var cheatroles1 = DefenseRoles.Where(q => q.isguard == false && q.Role.art_ninja_cheat_code1 > 0).ToList();
            var cheatroles2 = DefenseRoles.Where(q => q.isguard == false && q.Role.art_ninja_cheat_code2 > 0).ToList();
            var cheatroles3 = DefenseRoles.Where(q => q.isguard == false && q.Role.art_ninja_cheat_code3 > 0).ToList();
            //忍者众奥义
            var ninjaroles = DefenseRoles.Where(q => q.isguard == false && q.Role.art_ninja_mystery > 0).ToList();
            //武将特性
            var characterroles1 = DefenseRoles.Where(q => q.isguard == false && q.Role.character1 > 0).ToList();
            var characterroles2 = DefenseRoles.Where(q => q.isguard == false && q.Role.character2 > 0).ToList();
            var characterroles3 = DefenseRoles.Where(q => q.isguard == false && q.Role.character3 > 0).ToList();
            if (mysteryroles.Any()) ids.AddRange(mysteryroles.Select(q => q.Role.art_war_mystery).ToList());
            if (cheatroles.Any()) ids.AddRange(cheatroles.Select(q => q.Role.art_war_cheat_code).ToList());
            if (cheatroles1.Any()) ids.AddRange(cheatroles1.Select(q => q.Role.art_ninja_cheat_code1).ToList());
            if (cheatroles2.Any()) ids.AddRange(cheatroles2.Select(q => q.Role.art_ninja_cheat_code2).ToList());
            if (cheatroles3.Any()) ids.AddRange(cheatroles3.Select(q => q.Role.art_ninja_cheat_code3).ToList());
            if (ninjaroles.Any()) ids.AddRange(ninjaroles.Select(q => q.Role.art_ninja_mystery).ToList());

            var skilllist = tg_role_war_skill.GetListByIds(ids);

            GetDefensrolesSkill(mysteryroles, skilllist, "art_war_mystery");
            GetDefensrolesSkill(cheatroles, skilllist, "art_war_cheat_code");
            GetNinjaSkill(cheatroles1, skilllist, "art_ninja_cheat_code1");
            GetNinjaSkill(cheatroles2, skilllist, "art_ninja_cheat_code2");
            GetNinjaSkill(cheatroles3, skilllist, "art_ninja_cheat_code3");
            GetNinjaSkill(ninjaroles, skilllist, "art_ninja_mystery");
            GetDefensrolesSkill(characterroles1, null, "character1");
            GetDefensrolesSkill(characterroles2, null, "character2");
            GetDefensrolesSkill(characterroles3, null, "character3");

        }

        /// <summary>
        /// 防守武将技能初始
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="skilllist"></param>
        /// <param name="skilltype"></param>
        private void GetDefensrolesSkill(List<DefenseRoles> roles, List<tg_role_war_skill> skilllist, string skilltype)
        {
            WarFightSkillType type = new WarFightSkillType();
            for (int i = 0; i < roles.Count; i++)
            {
                var basedata = new BaseHeZhanSkillEffect();

                #region 获取效果基表
                switch (skilltype)
                {
                    case "character1"://武将特性1
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == roles[i].Role.character1);
                            type = WarFightSkillType.Character;
                        } break;
                    case "character2"://武将特性2
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == roles[i].Role.character2);
                            type = WarFightSkillType.Character;
                        } break;
                    case "character3"://武将特性3
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == roles[i].Role.character3);
                            type = WarFightSkillType.Character;
                        } break;
                    case "art_war_mystery": //合战奥义
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_war_mystery);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.Katha;
                        } break;
                    case "art_war_cheat_code"://合战秘技
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_war_cheat_code);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.Skill;
                        } break;
                    case "art_ninja_cheat_code1"://忍者众秘技1
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code1);
                            if (warskill == null) continue;
                            var basedata1 = Variable.BASE_FIGHTSKILL.FirstOrDefault(q => q.id == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_cheat_code2"://忍者众秘技2
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code2);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_cheat_code3"://忍者众秘技3
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code3);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_mystery"://忍者众奥义
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_mystery);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaMystery;
                        } break;

                }
                #endregion

                if (basedata == null) continue;
                //只有“装备时”这个触发条件  需要直接初始
                var isOneCondition = !basedata.condition.Contains("|");
                if (isOneCondition)
                {
                    var condition = Convert.ToInt32(basedata.condition);
                    if (condition == (int)WarFightCondition.OtherUse) continue;
                    if (condition == (int)WarFightCondition.Equip)
                    {
                        GetEffectInit(roles[i], WarSkillEffect.GetEffectStringInit(basedata.effects, false));
                        continue;
                    }
                }
                GetEffectInit(roles[i], basedata, type);
            }
        }

        /// <summary>
        /// 初始忍者众技能
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="skilllist"></param>
        /// <param name="skilltype"></param>
        private void GetNinjaSkill(List<DefenseRoles> roles, List<tg_role_war_skill> skilllist, string skilltype)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                var basedata = new BaseFightSkillEffect();
                var warskill = new tg_role_war_skill();
                #region 获取效果基表
                switch (skilltype)
                {
                    case "art_ninja_cheat_code1"://忍者众秘技1
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code1);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_cheat_code2"://忍者众秘技2
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code2);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_cheat_code3"://忍者众秘技3
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code3);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_mystery"://忍者众奥义
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_mystery);
                            if (warskill == null) continue;

                        } break;

                }
                #endregion
                basedata = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                WarFightSkillType type = WarFightSkillType.NinjaSkill;
                if (basedata == null) continue;
                //只有“装备时”这个触发条件  需要直接初始
                var isOneCondition = !basedata.condition.Contains("|");
                if (isOneCondition)
                {
                    var condition = Convert.ToInt32(basedata.condition);
                    if (condition == (int)WarFightCondition.OtherUse) continue;
                    if (condition == (int)WarFightCondition.Equip)
                    {
                        GetEffectInit(roles[i], WarSkillEffect.GetEffectStringInit(basedata.effects, false));
                        continue;
                    }
                }
                GetEffectInit(roles[i], null, type);
            }


        }
        #endregion

        /// <summary>
        /// 直接装备的技能效果初始
        /// </summary>
        /// <param name="role"></param>
        /// <param name="effects"></param>
        private void GetEffectInit(DefenseRoles role, List<WarSkillEffect> effects)
        {

            foreach (var effect in effects)
            {
                switch (effect.effectType)
                {
                    //增加攻击力
                    case (int)WarFightEffectType.AddAttack: role.attack += effect.effectValue; break;
                    //增加防御力
                    case (int)WarFightEffectType.AddDefense: role.defense += effect.effectValue; break;
                    //增加命中
                    case (int)WarFightEffectType.Hits: role.hits += effect.effectValue; break;
                    //增加气力
                    case (int)WarFightEffectType.QiLi: role.qili += effect.effectValue; break;
                    //增加兵力
                    case (int)WarFightEffectType.SoldierCount: role.SoldierCount += effect.effectValue; break;
                    //增加先手值
                    case (int)WarFightEffectType.FirstAttack: role.AttackSort += effect.effectValue; break;
                    //增加暴击率
                    case (int)WarFightEffectType.BaoJi: role.crit += effect.effectValue; break;
                    //增加躲避率
                    case (int)WarFightEffectType.Dodge: role.dodge += effect.effectValue; break;
                    //增加士气
                    case (int)WarFightEffectType.ShiQi: role.morale += effect.effectValue; break; //增加士气
                    case (int)WarFightEffectType.SoldierMax://增加血量上限
                        {
                            role.SoldierCount += effect.effectValue;
                            role.bloodMax += effect.effectValue; break;
                        }

                }
            }

        }

        /// <summary>
        /// 初始防守武将技能
        /// </summary>
        /// <param name="role"></param>
        /// <param name="baseeffect"></param>
        /// <param name="type"></param>
        private void GetEffectInit(DefenseRoles role, BaseHeZhanSkillEffect baseeffect, WarFightSkillType type)
        {
            var effect = WarSkillEffect.GetEffectStringInit(baseeffect.effects, baseeffect.skillid);
            if (effect == null) isInitSuccess = false;
            if (role.skills == null)
                role.skills = new List<WarFightSkill>();

            role.skills.Add(new WarFightSkill()
            {
                effectBaseInfo = baseeffect,
                FightSkillEffects = effect,
                type = type,
                Condition = baseeffect.condition.Split("_").ToList(),
            });

        }
        /// <summary>
        /// 初始进攻武将技能
        /// </summary>
        /// <param name="role"></param>
        /// <param name="baseeffect"></param>
        /// <param name="type"></param>
        private void GetEffectInit(AttackRoles role, BaseHeZhanSkillEffect baseeffect, WarFightSkillType type)
        {
            var effect = WarSkillEffect.GetEffectStringInit(baseeffect.effects, baseeffect.skillid);
            if (effect == null) isInitSuccess = false;
            if (role.skills == null)
                role.skills = new List<WarFightSkill>();
            role.skills.Add(new WarFightSkill()
            {
                effectBaseInfo = baseeffect,
                FightSkillEffects = effect,
                type = type,
                Condition = baseeffect.condition.Split("|").ToList(),
            });

        }


        /// <summary>
        ///初始进攻武将
        /// </summary>
        /// <param name="roles">武将vo集合</param>
        /// <returns></returns>
        private void GetAttackRolesInit(IEnumerable<WarRolesLinesVo> roles, List<tg_war_role> warrolelist, int morale)
        {
            var baseroles = tg_role.GetRoleByIds(warrolelist.Select(q => q.rid).ToList());
            var selectroles = warrolelist.Select(q =>
            {
                var role = baseroles.FirstOrDefault(m => m.id == q.rid);
                var tgrolelines = roles.FirstOrDefault(m => m.rid == q.rid).lines;
                var basesoldier = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(m => m.id == q.army_id);

                if (role == null || !tgrolelines.Any() || basesoldier == null)
                {
                    isInitSuccess = false;
                    return null;
                }
                var baserole = Variable.BASE_ROLE.FirstOrDefault(n => n.id == role.role_id);
                if (baserole == null) return null;
                return new AttackRoles()
                {
                    morale = morale,
                    RoleId = q.id,
                    Role = role,
                    WarRole = q,
                    Lines = tgrolelines,
                    SoldierId = q.army_id,
                    SoldierCount = q.army_soldier,
                    bloodMax = q.army_soldier,
                    speed = basesoldier.speed,
                    X = tgrolelines[0].x,
                    Y = tgrolelines[0].y,
                    AttackRange = Common.GetInstance().GetAttackRangeInit(q.army_id, tgrolelines[0].x, tgrolelines[0].y),
                    attack = basesoldier.baseAttack,  //兵种基础进攻值
                    defense = basesoldier.baseDefence,//兵种基础防御值
                    crit = basesoldier.crit,          //基础暴击几率
                    hits = basesoldier.hits,            //初始命中率
                    hurtRange = basesoldier.hurtRange,
                    roleName = baserole.name
                };
            }).ToList();
            AttackRoles = selectroles;
            AttackSoldierCount = selectroles.Sum(q => q.SoldierCount);
            AttackInfluence();
            //技能
            AttackEffectInit();
            //装备
            AttackRolesEquipInit();
        }

        /// <summary>
        /// 进攻武将势力特性
        /// </summary>
        private void AttackInfluence()
        {
            //势力特性
            for (int i = 0; i < AttackRoles.Count; i++)
            {
                var user = tg_user.FindByid(AttackRoles[i].Role.user_id);
                if (user.player_influence == (int)InfluenceType.ZhiTian)
                {
                    if (AttackRoles[i].SoldierId != 10001) continue;
                    AttackRoles[i].attack += Common.GetInstance().GetRule("32103");
                }
                if (user.player_influence == (int)InfluenceType.DeChuan)
                {
                    if (AttackRoles[i].SoldierId != 10002) continue;
                    AttackRoles[i].attack += Common.GetInstance().GetRule("32104");
                }
                if (user.player_influence == (int)InfluenceType.WuTian)
                {
                    if (AttackRoles[i].SoldierId != 10005) continue;
                    AttackRoles[i].attack += Common.GetInstance().GetRule("32105");
                }
                if (user.player_influence == (int)InfluenceType.Shangshan)
                {
                    if (AttackRoles[i].SoldierId != 10009) continue;
                    AttackRoles[i].dodge += Common.GetInstance().GetRule("32106");
                }
                if (user.player_influence == (int)InfluenceType.DaoJin)
                {
                    if (AttackRoles[i].SoldierId != 10003) continue;
                    AttackRoles[i].attack += Common.GetInstance().GetRule("32107");
                    AttackRoles[i].speed = Common.GetInstance().GetRule("32108");
                }
                if (user.player_influence == (int)InfluenceType.YiDa)
                {
                    if (AttackRoles[i].SoldierId != 10007) continue;
                    AttackRoles[i].attack += Common.GetInstance().GetRule("32109");
                }
            }

        }

        /// <summary>
        /// 初始防守武将的装备
        /// </summary>
        private void AttackRolesEquipInit()
        {
            var ids = new List<Int64>(); //装备主键id集合
            var equiproles = AttackRoles.Where(q => q.Role.equip_mounts > 0).ToList();

            if (!equiproles.Any()) return; //没有武将有合战装备

            ids.AddRange(equiproles.Select(q => q.Role.equip_mounts).ToList());
            if (!ids.Any()) return;
            var equiplist = tg_bag.GetWarEquips(ids);

            for (int i = 0; i < equiproles.Count; i++)
            {
                var equip = equiplist.FirstOrDefault(q => q.id == equiproles[i].Role.equip_mounts);
                if (equip == null) continue;
                var r = GetWarRoleEquip(equip);

                equiproles[i].EquipAddAttack += r.Item1; //装备增加的攻击力
                equiproles[i].EquipAddDefense += r.Item2; //装备增加的防御力
            }

        }

        #region 初始技能
        /// <summary>
        /// 技能初始
        /// </summary>
        private void AttackEffectInit()
        {
            var ids = new List<Int64>(); //忍者众主键id集合

            //合战奥义
            var mysteryroles = AttackRoles.Where(q => q.Role.art_war_mystery > 0).ToList();
            //合战秘技
            var cheatroles = AttackRoles.Where(q => q.Role.art_war_cheat_code > 0).ToList();
            //忍者众秘技
            var cheatroles1 = AttackRoles.Where(q => q.Role.art_ninja_cheat_code1 > 0).ToList();
            var cheatroles2 = AttackRoles.Where(q => q.Role.art_ninja_cheat_code2 > 0).ToList();
            var cheatroles3 = AttackRoles.Where(q => q.Role.art_ninja_cheat_code3 > 0).ToList();
            //忍者众奥义
            var ninjaroles = AttackRoles.Where(q => q.Role.art_ninja_mystery > 0).ToList();
            //武将特性
            var characterroles1 = AttackRoles.Where(q => q.Role.character1 > 0).ToList();
            var characterroles2 = AttackRoles.Where(q => q.Role.character2 > 0).ToList();
            var characterroles3 = AttackRoles.Where(q => q.Role.character3 > 0).ToList();
            if (mysteryroles.Any()) ids.AddRange(mysteryroles.Select(q => q.Role.art_war_mystery).ToList());
            if (cheatroles.Any()) ids.AddRange(cheatroles.Select(q => q.Role.art_war_cheat_code).ToList());
            if (cheatroles1.Any()) ids.AddRange(cheatroles1.Select(q => q.Role.art_ninja_cheat_code1).ToList());
            if (cheatroles2.Any()) ids.AddRange(cheatroles2.Select(q => q.Role.art_ninja_cheat_code2).ToList());
            if (cheatroles3.Any()) ids.AddRange(cheatroles3.Select(q => q.Role.art_ninja_cheat_code3).ToList());
            if (ninjaroles.Any()) ids.AddRange(ninjaroles.Select(q => q.Role.art_ninja_mystery).ToList());

            var skilllist = tg_role_war_skill.GetListByIds(ids);

            GetAttackRolesSkill(mysteryroles, skilllist, "art_war_mystery");
            GetAttackRolesSkill(cheatroles, skilllist, "art_war_cheat_code");
            GetAttackNinjaSkill(cheatroles1, skilllist, "art_ninja_cheat_code1");
            GetAttackNinjaSkill(cheatroles2, skilllist, "art_ninja_cheat_code2");
            GetAttackNinjaSkill(cheatroles3, skilllist, "art_ninja_cheat_code3");
            GetAttackNinjaSkill(ninjaroles, skilllist, "art_ninja_mystery");
            GetAttackRolesSkill(characterroles1, null, "character1");
            GetAttackRolesSkill(characterroles2, null, "character2");
            GetAttackRolesSkill(characterroles3, null, "character3");

        }

        /// <summary>
        /// 初始忍者众技能
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="skilllist"></param>
        /// <param name="skilltype"></param>
        private void GetAttackNinjaSkill(List<AttackRoles> roles, List<tg_role_war_skill> skilllist, string skilltype)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                var basedata = new BaseFightSkillEffect();
                var warskill = new tg_role_war_skill();
                #region 获取效果基表
                switch (skilltype)
                {
                    case "art_ninja_cheat_code1"://忍者众秘技1
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code1);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_cheat_code2"://忍者众秘技2
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code2);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_cheat_code3"://忍者众秘技3
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code3);
                            if (warskill == null) continue;

                        } break;
                    case "art_ninja_mystery"://忍者众奥义
                        {
                            warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_mystery);
                            if (warskill == null) continue;

                        } break;

                }
                #endregion
                basedata = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                WarFightSkillType type = WarFightSkillType.NinjaSkill;
                if (basedata == null) continue;
                //只有“装备时”这个触发条件  需要直接初始
                var isOneCondition = !basedata.condition.Contains("|");
                if (isOneCondition)
                {
                    var condition = Convert.ToInt32(basedata.condition);
                    if (condition == (int)WarFightCondition.OtherUse) continue;
                    if (condition == (int)WarFightCondition.Equip)
                    {
                        GetAttackRolesEffectInit(roles[i], WarSkillEffect.GetEffectStringInit(basedata.effects, false));
                        continue;
                    }
                }
                GetEffectInit(roles[i], null, type);
            }


        }

        /// <summary>
        /// 防守武将技能初始
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="skilllist"></param>
        /// <param name="skilltype"></param>
        private void GetAttackRolesSkill(List<AttackRoles> roles, List<tg_role_war_skill> skilllist, string skilltype)
        {
            WarFightSkillType type = new WarFightSkillType();
            for (int i = 0; i < roles.Count; i++)
            {
                var basedata = new BaseHeZhanSkillEffect();

                #region 获取效果基表
                switch (skilltype)
                {
                    case "character1"://武将特性1
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.id == roles[i].Role.character1);
                            type = WarFightSkillType.Character;
                        } break;
                    case "character2"://武将特性1
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.id == roles[i].Role.character2);
                            type = WarFightSkillType.Character;
                        } break;
                    case "character3"://武将特性1
                        {
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.id == roles[i].Role.character3);
                            type = WarFightSkillType.Character;
                        } break;
                    case "art_war_mystery": //合战奥义
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_war_mystery);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.Katha;
                        } break;
                    case "art_war_cheat_code"://合战秘技
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_war_cheat_code);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.Skill;
                        } break;
                    case "art_ninja_cheat_code1"://忍者众秘技1
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code1);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_cheat_code2"://忍者众秘技2
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code2);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_cheat_code3"://忍者众秘技3
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_cheat_code3);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaSkill;
                        } break;
                    case "art_ninja_mystery"://忍者众奥义
                        {
                            var warskill = skilllist.FirstOrDefault(q => q.id == roles[i].Role.art_ninja_mystery);
                            if (warskill == null) continue;
                            basedata = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(q => q.skillid == warskill.war_skill_id);
                            type = WarFightSkillType.NinjaMystery;
                        } break;
                }
                #endregion

                if (basedata == null) continue;
                //只有“装备时”这个触发条件  需要直接初始
                var isOneCondition = !basedata.condition.Contains("|");
                if (isOneCondition)
                {
                    var condition = Convert.ToInt32(basedata.condition);
                    if (condition == (int)WarFightCondition.OtherUse) continue;
                    if (condition == (int)WarFightCondition.Equip)
                    {
                        GetAttackRolesEffectInit(roles[i],
                            WarSkillEffect.GetEffectStringInit(basedata.effects, false));
                        continue;
                    }
                }
                GetEffectInit(roles[i], basedata, type);
            }
        }


        #endregion

        /// <summary>
        /// 直接装备的技能效果初始
        /// </summary>
        /// <param name="role"></param>
        /// <param name="effects"></param>
        private void GetAttackRolesEffectInit(AttackRoles role, List<WarSkillEffect> effects)
        {

            foreach (var effect in effects)
            {
                switch (effect.effectType)
                {
                    //增加攻击力
                    case (int)WarFightEffectType.AddAttack: role.attack += effect.effectValue; break;
                    //增加防御力
                    case (int)WarFightEffectType.AddDefense: role.defense += effect.effectValue; break;
                    //增加命中
                    case (int)WarFightEffectType.Hits: role.hits += effect.effectValue; break;
                    //增加气力
                    case (int)WarFightEffectType.QiLi: role.qili += effect.effectValue; break;
                    //增加兵力
                    case (int)WarFightEffectType.SoldierCount: role.SoldierCount += effect.effectValue; break;
                    //增加先手值
                    case (int)WarFightEffectType.FirstAttack: role.AttackSort += effect.effectValue; break;
                    //增加暴击率
                    case (int)WarFightEffectType.BaoJi: role.crit += effect.effectValue; break;
                    //增加躲避率
                    case (int)WarFightEffectType.Dodge: role.dodge += effect.effectValue; break;
                    //增加士气
                    case (int)WarFightEffectType.ShiQi: role.morale += effect.effectValue; break;
                    case (int)WarFightEffectType.SoldierMax://增加血量上限
                        {
                            role.SoldierCount += effect.effectValue;
                            role.bloodMax += effect.effectValue; break;
                        }

                }
            }

        }

        /// <summary>
        /// 给每个武将初始一个默认位置
        /// </summary>
        /// <param name="roles">防守武将实体集合</param>
        /// <returns></returns>
        private void GetBasePointInit(List<DefenseRoles> roles)
        {
            foreach (var role in roles)
            {
                //本身就设置在默认位置则不用设置
                if ((role.X == 1 && role.Y == 3) || (role.X == 1 && role.Y == 4) || (role.X == 1 && role.Y == 5)
                    || (role.X == 0 && role.Y == 3) || (role.X == 0 && role.Y == 5))
                {
                    role.BasePoint.X = role.X;
                    role.BasePoint.Y = role.Y;
                }
                if (!roles.Any(q => q.X == 1 && q.Y == 3))
                {
                    role.BasePoint.X = 1;
                    role.BasePoint.Y = 3;
                    continue;
                }
                if (!roles.Any(q => q.X == 1 && q.Y == 4))
                {
                    role.BasePoint.X = 1;
                    role.BasePoint.Y = 4;
                    continue;
                }
                if (!roles.Any(q => q.X == 1 && q.Y == 5))
                {
                    role.BasePoint.X = 1;
                    role.BasePoint.Y = 5;
                    continue;
                }
                if (!roles.Any(q => q.X == 0 && q.Y == 3))
                {
                    role.BasePoint.X = 0;
                    role.BasePoint.Y = 3;
                    continue;
                }
                if (!roles.Any(q => q.X == 0 && q.Y == 5))
                {
                    role.BasePoint.X = 0;
                    role.BasePoint.Y = 5;
                }
            }
        }


        /// <summary>
        /// 初始进攻顺序
        /// </summary>
        private List<AttackSort> GetRolesSort()
        {
            var sort = new List<AttackSort>();
            for (var i = 0; i < DefenseRoles.Count; i++)
            {
                //备大将先攻值为0
                if (DefenseRoles[i].isguard)
                    sort.Add(new AttackSort() { RoleId = DefenseRoles[i].RoleId, value = 0, type = 2 });
                else
                {
                    DefenseRoles[i].AttackSort = Convert.ToInt32(DefenseRoles[i].Role.base_govern + DefenseRoles[i].Role.base_brains);
                    sort.Add(new AttackSort()
                    {
                        RoleId = DefenseRoles[i].RoleId,
                        value = DefenseRoles[i].AttackSort,
                        type = 2
                    });
                }

            }

            for (var i = 0; i < AttackRoles.Count; i++)
            {
                AttackRoles[i].AttackSort = Convert.ToInt32(AttackRoles[i].Role.base_govern + AttackRoles[i].Role.base_brains);
                sort.Add(new AttackSort()
                {
                    RoleId = AttackRoles[i].RoleId,
                    value = AttackRoles[i].AttackSort,
                    type = 1
                });
            }
            return sort.OrderByDescending(q => q.value).ToList();

        }

        /// <summary> 城门本丸初始</summary>
        private void GetDoorAndCityInit(tg_war_city cityinfo)
        {
            Door = new DefenseRoles();
            City = new DefenseRoles();
            Door.type = (int)WarFightRoleType.城门;
            City.type = (int)WarFightRoleType.本丸;

            if (cityinfo == null)
            {
                isInitSuccess = false;
                return;
            }
            var basesize = Variable.BASE_WARCITYSIZE.FirstOrDefault(q => q.id == cityinfo.size);
            if (basesize == null)
            {
                isInitSuccess = false; return;
            }
            Door.SoldierCount = Common.GetInstance().GetRule("32056", basesize.strong);
            Door.RoleId = RNG.Next();
            City.RoleId = RNG.Next();
            City.SoldierCount = basesize.blood;
            City.X = 0;
            City.Y = 4;

        }

        /// <summary>
        /// 初始天气持续回合数
        /// </summary>
        /// <returns></returns>
        private int GetWeatherStateInit()
        {
            return Common.GetInstance().GetRule("32057");
        }

        /// <summary>
        /// 初始五常持续回合数
        /// </summary>
        /// <returns></returns>
        private int GetFiveStateInit()
        {
            return Common.GetInstance().GetRule("32058");
        }

        #endregion



    }

    /// <summary>
    /// 进攻出手顺序
    /// </summary>
    public class AttackSort
    {
        /// <summary>武将主键id </summary>
        public Int64 RoleId { get; set; }

        /// <summary>先攻值 </summary>
        public Int32 value { get; set; }

        /// <summary>武将类型 1：进攻 2：防守</summary>
        public Int64 type { get; set; }

        /// <summary>武将类型 0:未死 1：死亡</summary>
        public bool isDie { get; set; }

    }

    /// <summary>
    /// 进攻武将类
    /// </summary>
    public class AttackRoles : Common.Point
    {
        /// <summary>武将主键id </summary>
        public Int64 RoleId { get; set; }

        /// <summary> 合战武将实体 </summary>
        public tg_war_role WarRole { get; set; }

        /// <summary>武将实体 </summary>
        public tg_role Role { get; set; }

        /// <summary>进攻线路集合 </summary>
        public List<PointVo> Lines = new List<PointVo>();

        /// <summary>进攻线路索引</summary>
        public Int32 Index { get; set; }

        /// <summary>先攻值 </summary>
        public Int32 AttackSort { get; set; }

        /// <summary>是否在战斗 </summary>
        public bool isfight { get; set; }

        /// <summary>武将兵种基表id </summary>
        public Int32 SoldierId { get; set; }

        private Int32 _SoldierCount;

        /// <summary>武将带兵数量(等于血量) </summary>
        public Int32 SoldierCount
        {
            get { return _SoldierCount; }
            set
            {
                _SoldierCount = value;
                if (value < 0) _SoldierCount = 0;
            }
        }

        /// <summary>武将行走的速度 </summary>
        public Int32 speed { get; set; }

        /// <summary>是否在攻打城门 </summary>
        public bool isFightDoor { get; set; }

        /// <summary>是否在攻打本丸 </summary>
        public bool isFightCity { get; set; }

        /// <summary>进攻范围</summary>
        public List<Common.Point> AttackRange = new List<Common.Point>();

        /// <summary>士气 </summary>
        public int morale { get; set; }

        /// <summary> 武将技能 </summary>
        public List<WarFightSkill> skills = new List<WarFightSkill>();

        /// <summary> 武将攻击力 </summary>
        public Int64 attack { get; set; }

        /// <summary> 武将防御力 </summary>
        public Int64 defense { get; set; }

        /// <summary> 暴击几率 </summary>
        public double crit { get; set; }

        /// <summary> 命中率 </summary>
        public double hits { get; set; }

        /// <summary> 躲避 </summary>
        public double dodge { get; set; }

        /// <summary> 气力 </summary>
        public double qili { get; set; }

        /// <summary> buff </summary>
        public List<WarFightSkillBuff> buffs = new List<WarFightSkillBuff>();

        /// <summary> 是不是第一次战斗 </summary>
        public bool isFirstAttack = new bool();

        /// <summary> 血量上限 </summary>
        public int bloodMax { get; set; }

        /// <summary> 装备增加的攻击力 </summary>
        public int EquipAddAttack { get; set; }

        /// <summary> 装备增加的防御力 </summary>
        public int EquipAddDefense { get; set; }

        /// <summary> 技能增加的攻击力 </summary>
        public int skillAddAttack { get; set; }

        /// <summary> 伤害变量值 </summary>
        public string hurtRange { get; set; }

        /// <summary> 武将名字 </summary>
        public string roleName { get; set; }

    }


    /// <summary>
    /// 防守武将类
    /// </summary>
    public class DefenseRoles : Common.Point
    {
        /// <summary>武将主键id </summary>
        public Int64 RoleId { get; set; }

        /// <summary>武将类型 0:武将 1:备大将 2:本丸  3：城门 4:伏兵</summary>
        public Int32 type { get; set; }

        /// <summary> 合战武将实体 </summary>
        public tg_war_role WarRole { get; set; }

        /// <summary>武将实体 </summary>
        public tg_role Role { get; set; }

        /// <summary>武将默认坐标 </summary>
        public Common.Point BasePoint { get; set; }

        /// <summary>武将兵种基表id </summary>
        public Int32 SoldierId { get; set; }

        /// <summary>武将带兵数量(等于血量) </summary>
        private Int32 _SoldierCount;

        /// <summary>武将带兵数量(等于血量) </summary>
        public Int32 SoldierCount
        {
            get { return _SoldierCount; }
            set
            {
                _SoldierCount = value;
                if (value < 0) _SoldierCount = 0;
            }
        }

        /// <summary>先攻值 </summary>
        public Int32 AttackSort { get; set; }

        /// <summary>是否是备大将 </summary>
        public bool isguard { get; set; }

        /// <summary>停止回合集合 </summary>
        public List<int> stoptimes { get; set; }


        private int _morale;
        /// <summary>士气 </summary>
        public int morale
        {
            get { return _morale; }
            set { _morale = value; if (_morale < 0) _morale = 0; }
        }

        /// <summary> 武将技能 </summary>
        public List<WarFightSkill> skills = new List<WarFightSkill>();

        /// <summary> 兵种攻击力 </summary>
        public Int64 attack { get; set; }

        /// <summary> 兵种防御力 </summary>
        public Int64 defense { get; set; }

        /// <summary> 暴击几率 </summary>
        public double crit { get; set; }

        /// <summary> 命中率 </summary>
        public double hits { get; set; }

        /// <summary> 气力 </summary>
        private double _qili;

        public double qili
        {
            get { return _qili; }
            set { _qili = value; if (value < 0) _qili = 0; }
        }

        /// <summary> 躲避 </summary>
        public double dodge { get; set; }

        /// <summary> buff </summary>
        public List<WarFightSkillBuff> buffs = new List<WarFightSkillBuff>();


        /// <summary> 是不是第一次战斗 </summary>
        public bool isFirstAttack = new bool();

        /// <summary> 血量上限 </summary>
        public int bloodMax { get; set; }

        /// <summary> 伏兵是否出现 </summary>
        public bool isShow { get; set; }

        /// <summary> 装备增加的攻击力 </summary>
        public int EquipAddAttack { get; set; }

        /// <summary> 装备增加的防御力 </summary>
        public int EquipAddDefense { get; set; }

        /// <summary> 技能增加的攻击力 </summary>
        public int skillAddAttack { get; set; }

        /// <summary> 伤害变量值 </summary>
        public string hurtRange { get; set; }

        /// <summary> 是否是npc 0:否 1：是 </summary>
        public int isNpc { get; set; }

        /// <summary> 武将名字 </summary>
        public string roleName { get; set; }

    }

    /// <summary>
    /// 防守范围
    /// </summary>
    public class DefenseRange : Common.Point
    {
        /// <summary>武将主键id </summary>
        public Int64 RoleId { get; set; }

        /// <summary>武将类型</summary>
        public int type { get; set; }

    }

    /// <summary>
    /// 合战技能效果
    ///1_2_2_1_6:类型_目标_范围_回合数_值
    ///1_1_2_1_0_30:类型_目标_范围_回合数_值_几率
    /// </summary>
    public class WarSkillEffect
    {
        /// <summary>
        /// 初始技能效果
        /// </summary>
        /// <param name="effectstring"></param>
        /// <returns></returns>
        public static List<WarSkillEffect> GetEffectStringInit(string effectstring, bool isareaeffect)
        {
            var list = new List<WarSkillEffect>();
            var splitstring = effectstring.Split("|").ToList();
            foreach (var effect in splitstring)
            {
                var splitvalue = effect.Split("_").ToList();
                if (splitvalue.Count < 5) continue;
                var oneeffect = new WarSkillEffect()
                {
                    effectType = Convert.ToInt32(splitvalue[0]),
                    target = Convert.ToInt32(splitvalue[1]),
                    range = Convert.ToInt32(splitvalue[2]),
                    times = Convert.ToInt32(splitvalue[3]),
                    effectValue = Convert.ToInt32(splitvalue[4]),
                };
                if (isareaeffect) //地形效果
                {
                    oneeffect.effectSoldier = splitvalue[5];
                }
                else
                {
                    if (splitvalue.Count == 6) oneeffect.Probability = Convert.ToInt32(splitvalue[5]);
                }

                list.Add(oneeffect);
            }
            return list;
        }

        /// <summary>
        /// 初始合战技能效果
        /// </summary>
        /// <param name="effectstring"></param>
        /// <param name="effectid"></param>
        /// <returns></returns>
        public static List<WarSkillEffect> GetEffectStringInit(string effectstring, int effectid)
        {
            var list = new List<WarSkillEffect>();
            var splitstring = effectstring.Split("|").ToList();
            foreach (var effect in splitstring)
            {
                var splitvalue = effect.Split("_").ToList();
                if (splitvalue.Count < 5) return null;
                var oneeffect = new WarSkillEffect()
                {
                    id = effectid,
                    effectType = Convert.ToInt32(splitvalue[0]),
                    target = Convert.ToInt32(splitvalue[1]),
                    range = Convert.ToInt32(splitvalue[2]),
                    times = Convert.ToInt32(splitvalue[3]),
                    effectValue = Convert.ToInt32(splitvalue[4]),
                };

                if (splitvalue.Count == 6) oneeffect.Probability = Convert.ToInt32(splitvalue[5]);
                list.Add(oneeffect);
            }
            return list;
        }


        /// <summary> 技能效果基表id </summary>
        public int id { get; set; }

        /// <summary> 效果类型 </summary>
        public int effectType { get; set; }

        /// <summary> 目标1=本方 2=敌方 </summary>
        public int target { get; set; }

        /// <summary> 范围 1=单人 2=全体</summary>
        public int range { get; set; }

        /// <summary> 持续回合数 1:当前回合 </summary>
        public int times { get; set; }

        /// <summary> 效果值 </summary>
        public int effectValue { get; set; }

        /// <summary> 概率 </summary>
        public int Probability { get; set; }

        /// <summary> 作用兵种 </summary>
        public string effectSoldier { get; set; }


    }


    /// <summary>
    /// 合战战斗技能类
    /// </summary>
    public class WarFightSkill
    {
        /// <summary> 1.合战秘技2.合战奥义 3.武将特性 4忍者众技能 5 NinjaMystery 6 地形</summary>
        public WarFightSkillType type { get; set; }

        /// <summary> 技能效果</summary>
        public List<WarSkillEffect> FightSkillEffects { get; set; }

        /// <summary> 技能效果基础信息 </summary>
        public BaseHeZhanSkillEffect effectBaseInfo { get; set; }

        /// <summary>触发条件</summary>
        public List<string> Condition { get; set; }
    }

    /// <summary>
    /// 合战buff类
    /// </summary>
    public class WarFightSkillBuff
    {
        /// <summary>
        /// buff类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 效果值 
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 持续回合数
        /// </summary>
        public int times { get; set; }

        /// <summary>
        /// 释放类型 WarFightSkillType类型
        /// </summary>
        public int usertype { get; set; }


        /// <summary>
        /// 基表id 用于检测相同地形中buff时使用
        /// </summary>
        public int baseid { get; set; }


    }
}

