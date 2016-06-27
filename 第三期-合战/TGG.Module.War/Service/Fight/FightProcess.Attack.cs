using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using NewLife.Reflection;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share;

namespace TGG.Module.War.Service
{
    public partial class FightProcess
    {
        /// <summary>
        ///  进攻武将战斗验证
        /// </summary>
        /// <param name="fight">战斗实体</param>
        /// <param name="rid">防守武将id</param>
        /// <param name="moves">回合实体</param>
        /// <returns>1:战斗返回结果 2:是否战斗结束 true结束</returns>
        private Tuple<int, bool> CheckAttackRole(WarFight fight, Int64 rid, WarMovesVo moves)
        {
            var attackrole = fight.AttackRoles.FirstOrDefault(q => q.RoleId == rid);
            if (attackrole == null) return Tuple.Create(-1, false);
            GetAttackRoleMoraleAndQili(attackrole);
            var tuple = CheckIsAttack(fight, rid, moves);
            if (!tuple) //没有防守武将
            {
                if (RolesMove(moves, fight, rid)) //武将移动成功
                {
                    if (fight.City.SoldierCount == 0) return Tuple.Create((int)ResultType.SUCCESS, true);
                    //验证是否有伏兵 

                    var darkroles = CheckDarkRole(fight.DefenseRoles.Where(q => q.type == 4 && q.isShow == false).ToList(),
                        attackrole, fight.DefenseRange.Where(q => q.type == 4).ToList());
                    foreach (var darkrole in darkroles)
                    {
                        var isend = DefenseRolesFight(darkrole, moves, attackrole, fight);
                        if (isend) return Tuple.Create((int)ResultType.SUCCESS, true);
                    }
                }
            }
            else
            {
                if (fight.City.SoldierCount == 0) return Tuple.Create((int)ResultType.SUCCESS, true);
            }

            return Tuple.Create((int)ResultType.SUCCESS, false);
        }


        private List<DefenseRoles> GetDefenseRoles(List<DefenseRoles> roles)
        {
            var newroles = new List<DefenseRoles>();
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].type == 4 && roles[i].isShow == false) continue;
                newroles.Add(roles[i]);
            }
            return newroles;

        }

        /// <summary>
        /// 验证武将是否需要攻击
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="rid"></param>
        /// <param name="moves"></param>
        /// <returns>1.是否需要攻击 2.是否战斗结束</returns>
        private bool CheckIsAttack(WarFight fight, Int64 rid, WarMovesVo moves)
        {
            var attackrole = fight.AttackRoles.FirstOrDefault(q => q.RoleId == rid);
            if (attackrole == null) return false;

            if (attackrole.isFightDoor) //是否攻打城门
            {
                AttackDoorFight(moves, attackrole, fight);
                return true; ;
            }
            if (attackrole.isFightCity) //是否攻打本丸
            {
                if (AttackHomeFight(moves, attackrole, fight))
                    return true;
            }

            //验证进攻武将攻击范围内是否有防守武将 
            var defrole = CheckIsDefenseRole(GetDefenseRoles(fight.DefenseRoles), attackrole, fight.Door, fight.City);
            if (defrole != null) //有防守武将
            {
                if (defrole.type == (int)WarFightRoleType.武将)
                {
                    AttackRolesFight(attackrole, moves, defrole, fight);
                }
                if (attackrole.isFightDoor) //是否攻打城门
                {
                    AttackDoorFight(moves, attackrole, fight);
                }
                if (attackrole.isFightCity) //是否攻打本丸
                {
                    AttackHomeFight(moves, attackrole, fight);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 进攻武将攻打防守武将
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="attackrole"></param>
        /// <param name="denfenserole"></param>
        /// <param name="fight"></param>
        private void AttackRolesFight(AttackRoles attackrole, WarMovesVo moves, DefenseRoles denfenserole, WarFight fight)
        {
            try
            {
                var condition = new List<int>();
                var rolevo = AddNewRoleVo(attackrole, moves);
                //是否出发地形地形效果
                // var isArea = CheckArea(defrole, fight, attrole, rolevo);
                if (attackrole.isFirstAttack) condition.Add((int)WarFightCondition.FirstAttack);
                condition.Add((int)WarFightCondition.Attack);
                CheckAttackRoleSkill(attackrole, condition, fight, denfenserole, rolevo, WarFightSkillType.Character);
                CheckAttackRoleSkill(attackrole, condition, fight, denfenserole, rolevo, WarFightSkillType.NinjaSkill);
                CheckAttackRoleSkill(attackrole, condition, fight, denfenserole, rolevo, WarFightSkillType.NinjaMystery);
                if (CheckAttackRoleSkill(attackrole, condition, fight, denfenserole, rolevo, WarFightSkillType.Skill)) //秘技释放以后气力值清零
                    attackrole.qili = 0;

                CheckAttackRoleSkill(attackrole, condition, fight, denfenserole, rolevo, WarFightSkillType.Katha);

                AttackRoleAttack(attackrole, denfenserole, rolevo);
                CheckDefenseRoleBlood(denfenserole, fight);
            }
            catch (Exception e)
            {
                XTrace.WriteLine("进攻武将技能出错{0}", e.Message);
                throw;
            }



        }

        /// <summary>
        /// 攻打城门
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="attackrole"></param>
        /// <param name="fight"></param>
        private void AttackDoorFight(WarMovesVo moves, AttackRoles attackrole, WarFight fight)
        {
            if (fight.Door.SoldierCount == 0) return;
            var type = GetHurt(attackrole, fight.Door);
            if (fight.Door.SoldierCount <= 0)
            {
                fight.Door.SoldierCount = 0;
                for (int i = 0; i < fight.AttackRoles.Count; i++)
                {

                    fight.AttackRoles[i].AttackRange = Common.GetInstance().Sacked(attackrole.X, attackrole.Y);
                    if (fight.AttackRoles[i].isFightDoor)
                    {
                        fight.AttackRoles[i].isFightDoor = false;
                    }
                }
            }

            var rolevo = AddNewRoleVo(attackrole, moves);
            WarFightRoleVoAddEffect(rolevo, fight.Door.RoleId, (int)EffectFaceTo.Rival,
               type, attackrole.SoldierCount, fight.Door.SoldierCount, 0, 0, 0);
            //防守武将回归
            DefenseRolesBack(fight, moves);
        }

        /// <summary>
        /// 空闲武将回归本丸
        /// </summary>
        /// <param name="fight"></param>
        private void DefenseRolesBack(WarFight fight, WarMovesVo moves)
        {
            if (!fight.DefenseRoles.Any()) return;
            for (int i = 0; i < fight.DefenseRoles.Count; i++)
            {
                if (fight.DefenseRoles[i].SoldierCount <= 0) continue;
                //已经在回归位置
                if ((fight.DefenseRoles[i].X == 1 && fight.DefenseRoles[i].Y == 4) || (fight.DefenseRoles[i].X == 1 && fight.DefenseRoles[i].Y == 3)
                    || (fight.DefenseRoles[i].X == 1 && fight.DefenseRoles[i].Y == 5) || (fight.DefenseRoles[i].X == 0 && fight.DefenseRoles[i].Y == 3)
                    || (fight.DefenseRoles[i].X == 0 && fight.DefenseRoles[i].Y == 5))
                    continue;
                #region 回归本丸
                if (!fight.DefenseRoles.Any(q => q.X == 1 && q.Y == 3) &&
                       !fight.AttackRoles.Any(q => q.X == 1 && q.Y == 3))
                {
                    fight.DefenseRoles[i].X = 1; fight.DefenseRoles[i].Y = 3;
                    fight.DefenseRoles[i].buffs.Add(new WarFightSkillBuff()
                    {
                        type = (int)WarFightEffectType.StopAction,
                        times = 2
                    });
                    //fight.DefenseRoles[i].type = (int)WarFightRoleType.武将;
                    var rolevo = AddNewRoleVo(fight.DefenseRoles[i], moves);
                    WarFightRoleVoAddEffect(rolevo, fight.DefenseRoles[i].RoleId, (int)EffectFaceTo.Me, (int)WarFightEffectVoType.伏兵, fight.DefenseRoles[i].SoldierCount, 0, 0, 0, 0);
                    continue;
                }
                if (!fight.DefenseRoles.Any(q => q.X == 1 && q.Y == 4) &&
                   !fight.AttackRoles.Any(q => q.X == 1 && q.Y == 4))
                {
                    fight.DefenseRoles[i].X = 1; fight.DefenseRoles[i].Y = 4;
                    fight.DefenseRoles[i].buffs.Add(new WarFightSkillBuff()
                    {
                        type = (int)WarFightEffectType.StopAction,
                        times = 2
                    });
                    //fight.DefenseRoles[i].type = (int)WarFightRoleType.武将;
                    fight.DefenseRoles[i].isShow = true;
                    var rolevo = AddNewRoleVo(fight.DefenseRoles[i], moves);
                    WarFightRoleVoAddEffect(rolevo, fight.DefenseRoles[i].RoleId, (int)EffectFaceTo.Me, (int)WarFightEffectVoType.伏兵, fight.DefenseRoles[i].SoldierCount, 0, 0, 0, 0);
                    continue;
                }
                if (!fight.DefenseRoles.Any(q => q.X == 1 && q.Y == 5) &&
                   !fight.AttackRoles.Any(q => q.X == 1 && q.Y == 5))
                {
                    fight.DefenseRoles[i].X = 1; fight.DefenseRoles[i].Y = 5;
                    fight.DefenseRoles[i].buffs.Add(new WarFightSkillBuff()
                    {
                        type = (int)WarFightEffectType.StopAction,
                        times = 2
                    });
                    // fight.DefenseRoles[i].type = (int)WarFightRoleType.武将;
                    fight.DefenseRoles[i].isShow = true;
                    var rolevo = AddNewRoleVo(fight.DefenseRoles[i], moves);
                    WarFightRoleVoAddEffect(rolevo, fight.DefenseRoles[i].RoleId, (int)EffectFaceTo.Me, (int)WarFightEffectVoType.伏兵, fight.DefenseRoles[i].SoldierCount, 0, 0, 0, 0);
                    continue;
                }
                if (!fight.DefenseRoles.Any(q => q.X == 0 && q.Y == 3) &&
                   !fight.AttackRoles.Any(q => q.X == 0 && q.Y == 3))
                {
                    fight.DefenseRoles[i].X = 0; fight.DefenseRoles[i].Y = 3;
                    fight.DefenseRoles[i].buffs.Add(new WarFightSkillBuff()
                    {
                        type = (int)WarFightEffectType.StopAction,
                        times = 2
                    });
                    // fight.DefenseRoles[i].type = (int)WarFightRoleType.武将;
                    fight.DefenseRoles[i].isShow = true;
                    var rolevo = AddNewRoleVo(fight.DefenseRoles[i], moves);
                    WarFightRoleVoAddEffect(rolevo, fight.DefenseRoles[i].RoleId, (int)EffectFaceTo.Me, (int)WarFightEffectVoType.伏兵, fight.DefenseRoles[i].SoldierCount, 0, 0, 0, 0);
                    continue;
                }
                if (!fight.DefenseRoles.Any(q => q.X == 0 && q.Y == 5) &&
                   !fight.AttackRoles.Any(q => q.X == 0 && q.Y == 5))
                {
                    fight.DefenseRoles[i].X = 0; fight.DefenseRoles[i].Y = 5;
                    fight.DefenseRoles[i].buffs.Add(new WarFightSkillBuff()
                    {
                        type = (int)WarFightEffectType.StopAction,
                        times = 2
                    });
                    //fight.DefenseRoles[i].type = (int)WarFightRoleType.武将;
                    fight.DefenseRoles[i].isShow = true;
                    var rolevo = AddNewRoleVo(fight.DefenseRoles[i], moves);
                    WarFightRoleVoAddEffect(rolevo, fight.DefenseRoles[i].RoleId, (int)EffectFaceTo.Me, (int)WarFightEffectVoType.伏兵, fight.DefenseRoles[i].SoldierCount, 0, 0, 0, 0);
                    continue;
                }
                #endregion

                fight.DefenseRange.RemoveAll(q => q.RoleId == fight.DefenseRoles[i].RoleId);

            }
            //防守范围重新初始
            fight.DefenseRange = Common.GetInstance().GetDefenseRangeInit(fight.DefenseRoles);
        }

        /// <summary>
        /// 进攻武将进攻
        /// </summary>
        /// <param name="defrole"></param>
        /// <param name="attrole"></param>
        /// <param name="rolevo"></param>
        /// <returns></returns>
        private void AttackRoleAttack(AttackRoles attrole, DefenseRoles defrole, WarFightRoleVo rolevo)
        {
            // defrole.SoldierCount -= 10;//测试数据
            var type = GetHurt(attrole, defrole);
            //var type = (int)WarFightEffectType.普通攻击;
            WarFightRoleVoAddEffect(rolevo, defrole.RoleId, (int)EffectFaceTo.Rival, type, attrole.SoldierCount, defrole.SoldierCount, 0, 0, 0);


        }

        /// <summary>
        /// 伤害计算
        /// </summary>
        /// <param name="attrole"></param>
        /// <param name="defenserole"></param>
        /// <returns></returns>
        private int GetHurt(AttackRoles attrole, DefenseRoles defenserole)
        {
            var attacktype = (int)WarFightEffectVoType.普通攻击;
            var dodge = defenserole.dodge + defenserole.buffs.Where(q => q.type == (int)WarFightEffectType.Dodge).Sum(q => q.value);
            var rolehits = attrole.hits + attrole.buffs.Where(q => q.type == (int)WarFightEffectType.Hits).Sum(q => q.value);
            var hits = Common.GetInstance().GetRule("32094", rolehits, dodge);
            var ishurt = new RandomSingle().IsTrue(hits);
            if (!ishurt) return (int)WarFightEffectVoType.躲避;
            //var basecap = Common.GetInstance().GetRule("32086", attrole.Role.base_captain);

            var equipattack = Common.GetInstance().GetRule("32087", attrole.EquipAddAttack);
            var total_fo = tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, attrole.Role);
            var roleattack = Common.GetInstance().GetRule("32088", total_fo, attrole.Role.base_captain);
            var soldierattack = Common.GetInstance().GetRule("32089", attrole.attack, attrole.morale, attrole.SoldierCount);

            var rivaldefense = Common.GetInstance().GetRule("32090", defenserole.EquipAddDefense);

            var addhurt = Common.GetInstance().GetRule("32091", attrole.Role.base_force);
            var redhurt = Common.GetInstance().GetRule("32092", defenserole.Role == null ? 0 : defenserole.Role.base_brains);
            var rangesplit = attrole.hurtRange.Split("_");
            var hurtrange = RNG.Next(Convert.ToInt32(rangesplit[0]), (Convert.ToInt32(rangesplit[1])));
            var skilladdattack = attrole.buffs.Where(q => q.type == (int)WarFightEffectType.AddAttack).Sum(q => q.value);
            var skilladddefense = defenserole.buffs.Where(q => q.type == (int)WarFightEffectType.AddDefense).Sum(q => q.value);
            var list = new List<Double>()
            {
                equipattack,
                roleattack,
                soldierattack,
                rivaldefense,
                addhurt,
                redhurt,
               skilladdattack,
               skilladddefense,
                hurtrange,
              
            };
            var hurt = Common.GetInstance().GetRule("32093", list);
            var crit = attrole.crit + attrole.buffs.Where(q => q.type == (int)WarFightEffectType.Hits).Sum(q => q.value);
            var iscrit = new RandomSingle().IsTrue(crit);
            if (iscrit)
            {
                attacktype = (int)WarFightEffectVoType.暴击;
                hurt = Common.GetInstance().GetRule("32095", hurt);
            }
            defenserole.SoldierCount -= (int)hurt;
            return attacktype;
        }

        /// <summary>
        /// 攻打本丸
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="attackrole"></param>
        /// <param name="fight"></param>
        private bool AttackHomeFight(WarMovesVo moves, AttackRoles attackrole, WarFight fight)
        {
            if (fight.City.SoldierCount == 0) return true;
            var type = GetHurt(attackrole, fight.City);
            if (fight.City.SoldierCount < 0)
            {
                fight.City.SoldierCount = 0;
                return true;
            }
            var rolevo = AddNewRoleVo(attackrole, moves);

            WarFightRoleVoAddEffect(rolevo, fight.City.RoleId, (int)EffectFaceTo.Rival,
              type, attackrole.SoldierCount, fight.City.SoldierCount, 0, 0, 0);
            return false;
        }


        /// <summary>
        /// 进攻武将技能释放条件验证
        /// </summary>
        /// <param name="role"></param>
        /// <param name="nowcondition"></param>
        /// <param name="fight"></param>
        /// <param name="defenserole"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private bool CheckAttackRoleCondition(AttackRoles role, List<int> nowcondition, WarFight fight, DefenseRoles defenserole, WarFightSkill skill)
        {
            var ismatch = true;
            foreach (var item in skill.Condition)
            {
                var condition = Convert.ToInt32(item);

                #region 技能条件验证
                switch (condition)
                {
                    case (int)WarFightCondition.Attack:
                        {
                            if (nowcondition.Contains(condition)) ismatch = false;
                        } break;
                    case (int)WarFightCondition.Rain: { if (fight.Weather != condition)ismatch = false; } break;
                    case (int)WarFightCondition.Attacked: { if (!nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.FirstAttack: { if (!nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.DarkRoleFirstAttack: { if (!nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.FiveSame: { if (fight.FiveSharp != condition)ismatch = false; } break;
                    case (int)WarFightCondition.SkillAttack: { if (!nowcondition.Contains(condition))ismatch = false; } break;

                    case (int)WarFightCondition.BloodLess: { if (role.SoldierCount > skill.effectBaseInfo.SoldierCount)ismatch = false; } break;

                    case (int)WarFightCondition.CharmLessMe: { if (role.Role.base_charm < defenserole.Role.base_charm)ismatch = false; } break;
                    case (int)WarFightCondition.ForceLessMe: { if (role.Role.base_force < defenserole.Role.base_force)ismatch = false; } break;
                    case (int)WarFightCondition.BrainLessMe: { if (role.Role.base_brains < defenserole.Role.base_brains)ismatch = false; } break;
                    case (int)WarFightCondition.AreaSlope:
                        {
                            if (!fight.Area.Any(q => q.type == (int)AreaType.高坡 && q.X == role.X && role.Y == q.Y))
                                ismatch = false;
                        } break;
                    case (int)WarFightCondition.AreaRiver:
                        {
                            if (!fight.Area.Any(q => q.type == (int)AreaType.河滩 && q.X == role.X && role.Y == q.Y))
                                ismatch = false;
                        } break;

                    case (int)WarFightCondition.AreaSwamp:
                        {
                            if (!fight.Area.Any(q => q.type == (int)AreaType.沼泽 && q.X == role.X && role.Y == q.Y))
                                ismatch = false;
                        } break;
                    case (int)WarFightCondition.AreaForest:
                        {
                            if (!fight.Area.Any(q => q.type == (int)AreaType.密林 && q.X == role.X && role.Y == q.Y))
                                ismatch = false;
                        } break;
                    case (int)WarFightCondition.FancyAttack:
                        {
                            if (!nowcondition.Contains(condition)) ismatch = false;

                        } break;
                    case (int)WarFightCondition.QiLi:
                        {
                            if (nowcondition.Contains(condition)
                                || role.qili < skill.effectBaseInfo.energy) ismatch = false;
                        } break;
                #endregion
                }
            }
            return ismatch;
        }

        /// <summary>
        /// 防守武将增加buff
        /// </summary>
        /// <param name="role"></param>
        /// <param name="usertype"></param>
        /// <param name="times"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private void AttackRoleAddNewBuff(AttackRoles role, int type, int value, int usertype, int times)
        {
            var buff = new WarFightSkillBuff()
            {
                type = type,
                value = value,
                times = times,
                usertype = usertype
            };
            var repeatbuff = new WarFightSkillBuff();
            //检测是否已经有该buff效果
            repeatbuff = role.buffs.FirstOrDefault(q => q.type == buff.type && q.usertype == buff.usertype);
            if (repeatbuff == null) //没有相同类型的buff
                role.buffs.Add(buff);
            else
            {
                if (repeatbuff.value > buff.value) return;
                role.buffs.Remove(repeatbuff);
                role.buffs.Add(buff);
            }
        }

        /// <summary>
        /// 检测进攻武将，武将特性是否触发
        /// </summary>
        /// <returns></returns>
        private bool CheckAttackRoleSkill(AttackRoles role, List<int> nowcondition, WarFight fight, DefenseRoles defenserole, WarFightRoleVo rolevo, WarFightSkillType type)
        {
            var skills = role.skills.Where(q => q.type == type).ToList();

            foreach (var skill in skills)
            {
                var ismatch = CheckAttackRoleCondition(role, nowcondition, fight, defenserole, skill);
                if (!ismatch) continue;
                if (type == WarFightSkillType.Katha && role.buffs.Any(q => q.type == (int)WarFightEffectType.StartKatha) &&
                     role.buffs.All(q => q.type != (int)WarFightEffectType.StopKatha)) //是否立即释放奥义
                {
                    AttackRoleEffectsAdd(role, skill.FightSkillEffects, defenserole, rolevo, (int)type);
                    return true;
                }
                AttackRoleEffectsAdd(role, skill.FightSkillEffects, defenserole, rolevo, (int)type);
                if (type == WarFightSkillType.Skill) //气力清零
                    role.qili -= 0;
                return true;
            }
            return false;
        }

        /// <summary> 
        /// 进攻武将技能触发
        /// </summary>
        /// <param name="attackrole">进攻武将</param>
        /// <param name="effects">技能实体</param>
        /// <param name="defenserole">防守武将</param>
        /// <param name="rolevo">武将vo</param>
        /// <param name="skilltype"></param>
        private void AttackRoleEffectsAdd(AttackRoles attackrole, List<WarSkillEffect> effects, DefenseRoles defenserole, WarFightRoleVo rolevo, int skilltype)
        {
            //WarFightRoleVoAddEffect(rolevo, attackrole.RoleId, 0, SkillTypeToEffectType(skilltype), attackrole.SoldierCount, defenserole.SoldierCount,
            //    effects[0].id, 0, 0);   //vo增加效果
            foreach (var effect in effects)
            {
                if (effect.Probability > 0 && !new RandomSingle().IsTrue(effect.Probability)) continue; //效果触发概率

                if (effect.times == 0) //永久增加
                {
                    #region 永久效果

                    switch (effect.effectType)
                    {
                        #region 增加攻击力

                        case (int)WarFightEffectType.AddAttack:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.attack += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.attack += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加防御力

                        case (int)WarFightEffectType.AddDefense:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.defense += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.defense += effect.effectValue;

                                }
                                break;
                            }

                        #endregion

                        #region 增加命中

                        case (int)WarFightEffectType.Hits:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.hits += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.hits += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加气力

                        case (int)WarFightEffectType.QiLi:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.qili += effect.effectValue;

                                }
                                else
                                {
                                    defenserole.qili += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加兵力

                        case (int)WarFightEffectType.SoldierCount:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.SoldierCount += effect.effectValue;
                                    if (attackrole.SoldierCount > attackrole.bloodMax)
                                        attackrole.SoldierCount = attackrole.bloodMax;
                                }
                                else
                                {
                                    if (attackrole.SoldierCount > attackrole.bloodMax)
                                        attackrole.SoldierCount = attackrole.bloodMax;
                                    if (effect.times > 1)
                                        DefenseRoleAddNewBuff(defenserole, effect.effectType, effect.effectValue, skilltype,
                                            effect.times);
                                }
                                break;
                            }

                        #endregion

                        #region 增加先手值

                        case (int)WarFightEffectType.FirstAttack:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.AttackSort += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.AttackSort += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加暴击率

                        case (int)WarFightEffectType.BaoJi:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.crit += effect.effectValue;

                                }
                                else
                                {
                                    defenserole.crit += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加躲避率

                        case (int)WarFightEffectType.Dodge:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {

                                    attackrole.dodge += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.dodge += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                        #region 增加士气

                        case (int)WarFightEffectType.ShiQi:
                            {
                                if (effect.target == (int)EffectFaceTo.Me)
                                {
                                    attackrole.morale += effect.effectValue;
                                }
                                else
                                {
                                    defenserole.morale += effect.effectValue;
                                }
                                break;
                            }

                        #endregion

                    }

                    #endregion

                    WarFightRoleVoAddEffect(rolevo, attackrole.RoleId, effect.target, SkillTypeToEffectType(skilltype),
                           attackrole.SoldierCount, defenserole.SoldierCount, 0, effect.effectType, effect.effectValue);       //vo增加效果
                    continue;
                }
                if (effect.target == (int)EffectFaceTo.Me)
                {
                    AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times); //实体增加效果数值数据

                    WarFightRoleVoAddEffect(rolevo, attackrole.RoleId, effect.target, SkillTypeToEffectType(skilltype),
                        attackrole.SoldierCount, defenserole.SoldierCount, 0, effect.effectType, effect.effectValue);       //vo增加效果

                    if (effect.times > 1) //持续超过1回合
                        //添加buff
                        WarFightRoleVoAddBuff(rolevo, attackrole.RoleId, effect.effectType, effect.effectValue);
                }
                else
                {
                    DefenseRoleAddNewBuff(defenserole, effect.effectType, effect.effectValue, skilltype, effect.times); //实体添加buff
                    WarFightRoleVoAddEffect(rolevo, defenserole.RoleId, effect.target, SkillTypeToEffectType(skilltype),
                        attackrole.SoldierCount, defenserole.SoldierCount, 0, effect.effectType, effect.effectValue);   //vo增加效果
                    if (effect.times > 1) //
                        //添加buff
                        WarFightRoleVoAddBuff(rolevo, defenserole.RoleId, effect.effectType, effect.effectValue);

                }
            }


        }

        /// <summary>
        /// 获取回合开始时的士气和气力
        /// </summary>
        /// <param name="role"></param>
        private void GetAttackRoleMoraleAndQili(AttackRoles role)
        {
            role.morale -= 20;
            if (role.morale <= 0) role.morale = 0;
            role.qili += 10;
            if (role.qili > 100) role.qili = 100;
        }
    }
}
