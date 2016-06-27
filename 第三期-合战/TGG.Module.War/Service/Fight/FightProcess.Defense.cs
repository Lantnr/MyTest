using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share.Event;

namespace TGG.Module.War.Service
{
    public partial class FightProcess
    {
        /// <summary>
        ///  防守武将战斗验证
        /// </summary>
        /// <param name="fight">战斗实体</param>
        /// <param name="rid">防守武将id</param>
        /// <param name="moves">回合实体</param>
        /// <returns>1:战斗返回结果 2:是否战斗结束</returns>
        private Tuple<int, bool> CheckDefenseRole(WarFight fight, Int64 rid, WarMovesVo moves)
        {
            var defenserole = fight.DefenseRoles.FirstOrDefault(q => q.RoleId == rid);
            if (defenserole == null) return Tuple.Create(-1, false);
            if (defenserole.type == (int)WarFightRoleType.伏兵)
            {
                return Tuple.Create((int)ResultType.SUCCESS, false);
            }

            //士气和气力值改变
            GetDefenseRoleMoraleAndQili(defenserole);

            //验证本回合是否静止
            if (defenserole.stoptimes != null && defenserole.stoptimes.Contains(moves.times))
                return Tuple.Create((int)ResultType.SUCCESS, false);
            var defenserange = fight.DefenseRange.Where(q => q.RoleId == rid).ToList();
            //验证防守范围内是否有武将
            var attrole = CheckIsAttackRole(defenserange, fight.AttackRoles);
            if (attrole == null) return Tuple.Create((int)ResultType.SUCCESS, false);
#if DEBUG
            DisplayGlobal.log.Write(string.Format("武将{0}攻击武将{1}", rid, attrole.RoleId));
            DisplayGlobal.log.Write(string.Format("武将{0}被攻击前血量{1}", attrole.RoleId, attrole.SoldierCount));
#endif
            var isend = DefenseRolesFight(defenserole, moves, attrole, fight);
            return isend ? Tuple.Create((int)ResultType.SUCCESS, true) : Tuple.Create((int)ResultType.SUCCESS, false);

        }

        /// <summary>
        /// 防守武将攻打进攻武将
        /// </summary>
        /// <param name="defrole"></param>
        /// <param name="moves"></param>
        /// <param name="attrole"></param>
        /// <param name="fight"></param>
        private bool DefenseRolesFight(DefenseRoles defrole, WarMovesVo moves, AttackRoles attrole, WarFight fight)
        {
            try
            {
                if (defrole.SoldierCount <= 0 || attrole.SoldierCount <= 0) return false;
                var condition = new List<int>();
                var rolevo = AddNewRoleVo(defrole, moves);

                if (defrole.type == (int)WarFightRoleType.伏兵) //伏兵
                {
                    condition.Add((int)WarFightCondition.DarkRoleFirstAttack);
                    WarFightRoleVoAddEffect(rolevo, defrole.RoleId, (int)EffectFaceTo.Me,
                        (int)WarFightEffectVoType.伏兵, attrole.SoldierCount, defrole.SoldierCount, 0, 0, 0);
                    defrole.isShow = true;

                }
                //是否出发地形地形效果
                var isArea = CheckArea(defrole, fight, attrole, rolevo);
                if (defrole.isFirstAttack) condition.Add((int)WarFightCondition.FirstAttack);
                condition.Add((int)WarFightCondition.Attack);
                CheckSkill(defrole, condition, fight, attrole, rolevo, WarFightSkillType.Character);
                CheckSkill(defrole, condition, fight, attrole, rolevo, WarFightSkillType.NinjaSkill);
                CheckSkill(defrole, condition, fight, attrole, rolevo, WarFightSkillType.NinjaMystery);
                CheckSkill(defrole, condition, fight, attrole, rolevo, WarFightSkillType.Skill);
                CheckSkill(defrole, condition, fight, attrole, rolevo, WarFightSkillType.Katha);
                //最后进攻
                DefenseRoleAttack(defrole, attrole, rolevo);
                var isend = CheckAttackRoleBlood(attrole, fight);
                return isend;
            }
            catch (Exception e)
            {
                XTrace.WriteLine(e.Message);
                return false;
            }


        }

        /// <summary>
        /// 防守武将进攻
        /// </summary>
        /// <param name="defrole"></param>
        /// <param name="attrole"></param>
        /// <param name="rolevo"></param>
        /// <returns></returns>
        private void DefenseRoleAttack(DefenseRoles defrole, AttackRoles attrole, WarFightRoleVo rolevo)
        {
            //  attrole.SoldierCount -= 10;//测试数据
            var type = GetHurt(defrole, attrole);
            WarFightRoleVoAddEffect(rolevo, attrole.RoleId, (int)EffectFaceTo.Rival,
                type, defrole.SoldierCount, attrole.SoldierCount, 0, 0, 0);

        }

        /// <summary>
        /// 公式计算伤害
        /// </summary>
        /// <param name="defrole"></param>
        /// <param name="attackroles"></param>
        /// <returns></returns>
        private int GetHurt(DefenseRoles defrole, AttackRoles attackroles)
        {
            var attacktype = (int)WarFightEffectVoType.普通攻击;
            var dodge = attackroles.dodge +
                        attackroles.buffs.Where(q => q.type == (int)WarFightEffectType.Dodge).Sum(q => q.value);
            var rolehits = defrole.hits +
                           defrole.buffs.Where(q => q.type == (int)WarFightEffectType.Hits).Sum(q => q.value);
            var hits = Common.GetInstance().GetRule("32094", rolehits, dodge);
            var ishurt = new RandomSingle().IsTrue(hits);
            if (!ishurt) return (int)WarFightEffectVoType.躲避;
            // var basecap = Common.GetInstance().GetRule("32086", defrole.Role.base_captain);
            var equipattack = Common.GetInstance().GetRule("32087", defrole.EquipAddAttack);
            var total_fo = tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, defrole.Role);
            var roleattack = Common.GetInstance().GetRule("32088", total_fo, defrole.Role.base_captain);
            var soldierattack = Common.GetInstance().GetRule("32089", defrole.attack, defrole.morale, defrole.SoldierCount);
            var rivaldefense = Common.GetInstance().GetRule("32090", attackroles.EquipAddDefense);
            var addhurt = Common.GetInstance().GetRule("32091", defrole.Role.base_force);
            var redhurt = Common.GetInstance().GetRule("32092", attackroles.Role.base_brains);
            var rangesplit = defrole.hurtRange.Split("_");
            var hurtrange = RNG.Next(Convert.ToInt32(rangesplit[0]), (Convert.ToInt32(rangesplit[1])));
            var skilladdattack = defrole.buffs.Where(q => q.type == (int)WarFightEffectType.AddAttack)
                .Sum(q => q.value);
            var skilladddefense =
                attackroles.buffs.Where(q => q.type == (int)WarFightEffectType.AddDefense).Sum(q => q.value);
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
            var crit = defrole.crit +
                       defrole.buffs.Where(q => q.type == (int)WarFightEffectType.Hits).Sum(q => q.value);
            var iscrit = new RandomSingle().IsTrue(crit);
            if (iscrit)
            {
                attacktype = (int)WarFightEffectVoType.暴击;

                hurt = Common.GetInstance().GetRule("32095", hurt);
            }
            attackroles.SoldierCount -= (int)hurt;
            return attacktype;
        }

        ///  <summary>
        /// 验证是否出发地形效果
        ///  </summary>
        ///  <param name="fight"></param>
        ///  <param name="attackrole"></param>
        ///  <param name="rolevo"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool CheckArea(DefenseRoles role, WarFight fight, AttackRoles attackrole, WarFightRoleVo rolevo)
        {
            var type = (int)WarFightSkillType.Area;
            var area = fight.Area.FirstOrDefault(q => q.X == role.X && q.Y == role.Y);
            if (area == null) return false;
            var effectstring = fight.Weather == (int)WeatherType.Rain ? area.raineffect : area.suneffect;

            if (effectstring == "") return false;

            var effects = WarSkillEffect.GetEffectStringInit(effectstring, true);
            if (area.type == (int)AreaType.陷阱)
            {
                type = (int)WarFightSkillType.Trap;
            }
            DefenseRoleEffectsAdd(role, effects, attackrole, rolevo, type);
            fight.Area.Remove(area);
            return true;
        }



        /// <summary>
        /// 检测防守武将，武将特性是否触发
        /// </summary>
        /// <returns></returns>
        private bool CheckSkill(DefenseRoles role, List<int> nowcondition, WarFight fight, AttackRoles attackrole, WarFightRoleVo rolevo, WarFightSkillType type)
        {
            var skills = role.skills.Where(q => q.type == type).ToList();

            foreach (var skill in skills)
            {
                var ismatch = CheckDefenseRoleCondition(role, nowcondition, fight, attackrole, skill.Condition);
                if (!ismatch) continue;
                if (type == WarFightSkillType.Katha && role.buffs.Any(q => q.type == (int)WarFightEffectType.StartKatha) &&
                     attackrole.buffs.All(q => q.type != (int)WarFightEffectType.StopKatha)) //是否立即释放奥义
                {
                    DefenseRoleEffectsAdd(role, skill.FightSkillEffects, attackrole, rolevo, (int)type);
                }

                DefenseRoleEffectsAdd(role, skill.FightSkillEffects, attackrole, rolevo, (int)type);
                return true;
            }
            return false;
        }

        /// <summary> 
        /// 防守武将技能触发
        /// </summary>
        /// <param name="role">防守武将</param>
        /// <param name="effects">技能实体</param>
        /// <param name="attackrole">进攻武将</param>
        /// <param name="rolevo">武将vo</param>
        /// <param name="skilltype"></param>
        private void DefenseRoleEffectsAdd(DefenseRoles role, List<WarSkillEffect> effects, AttackRoles attackrole, WarFightRoleVo rolevo, int skilltype)
        {
            foreach (var effect in effects)
            {
                if (effect.Probability > 0 && !new RandomSingle().IsTrue(effect.Probability)) continue; //效果触发概率
                if (effect.effectSoldier != "0")
                {
                    if (effect.effectSoldier != role.SoldierId.ToString()) return; //地形效果兵种
                }
                var target = effect.target;
                switch (effect.effectType)
                {

                    #region 增加攻击力
                    case (int)WarFightEffectType.AddAttack:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);

                            }
                            break;
                        }
                    #endregion

                    #region 增加防御力
                    case (int)WarFightEffectType.AddDefense:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加命中
                    case (int)WarFightEffectType.Hits:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加气力
                    case (int)WarFightEffectType.QiLi:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加兵力
                    case (int)WarFightEffectType.SoldierCount:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                role.SoldierCount += effect.effectValue;
                                if (role.SoldierCount > role.bloodMax) role.SoldierCount = role.bloodMax;
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (attackrole.SoldierCount > role.bloodMax) attackrole.SoldierCount = attackrole.bloodMax;
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加先手值
                    case (int)WarFightEffectType.FirstAttack:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加暴击率
                    case (int)WarFightEffectType.BaoJi:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加躲避率
                    case (int)WarFightEffectType.Dodge:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 增加士气
                    case (int)WarFightEffectType.ShiQi:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                if (effect.times > 1) DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 免疫僧兵兵种特性
                    case (int)WarFightEffectType.CanceCheatroles:
                        {
                            if (effect.target == (int)EffectFaceTo.Me)
                            {
                                DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            else
                            {
                                if (effect.times > 1) AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            }
                            break;
                        }
                    #endregion

                    #region 停止释放奥义
                    case (int)WarFightEffectType.StopKatha:
                        {
                            AttackRoleAddNewBuff(attackrole, effect.effectType, effect.effectValue, skilltype, effect.times);
                            break;
                        }
                    #endregion

                    #region 立即释放奥义
                    case (int)WarFightEffectType.StartKatha:
                        {
                            DefenseRoleAddNewBuff(role, effect.effectType, effect.effectValue, skilltype, effect.times);

                            break;
                        }
                    #endregion


                }
                if (skilltype != (int)WarFightSkillType.Area)
                {
                    //vo添加状态
                    if (effect.target == (int)EffectFaceTo.Me) //对自己
                    {
                        if (effect.times == 1) WarFightRoleVoAddEffect(rolevo, role.RoleId, target,
                        SkillTypeToEffectType(skilltype), role.SoldierCount, attackrole.SoldierCount, effect.id, effect.effectType, effect.effectValue);
                        else
                        {
                            //添加buff
                            WarFightRoleVoAddBuff(rolevo, role.RoleId, effect.effectType, effect.effectValue);
                        }
                    }
                }

            }


        }

        /// <summary>
        /// 将实体技能类型转换成vo技能类型
        /// </summary>
        /// <param name="skilltype"></param>
        /// <returns></returns>
        private int SkillTypeToEffectType(int skilltype)
        {
            switch (skilltype)
            {
                case (int)WarFightSkillType.NinjaSkill:
                case (int)WarFightSkillType.NinjaMystery:
                    {
                        return (int)WarFightEffectVoType.忍者众;
                    }
                case (int)WarFightSkillType.Trap:
                    return (int)WarFightEffectVoType.陷阱;

            }
            return (int)WarFightEffectVoType.技能;

        }

        /// <summary>
        /// 防守武将增加buff
        /// </summary>
        /// <param name="role"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="usertype"></param>
        /// <param name="times"></param>
        private void DefenseRoleAddNewBuff(DefenseRoles role, int type, int value, int usertype, int times)
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
            repeatbuff = buff.usertype == (int)WarFightSkillType.Area ?
                role.buffs.FirstOrDefault(q => q.type == buff.type && q.usertype == buff.usertype) :
                role.buffs.FirstOrDefault(q => q.type == buff.type);

            if (repeatbuff == null)
                role.buffs.Add(buff);
            else
            {
                if (repeatbuff.value > buff.value) return;
                role.buffs.Remove(repeatbuff);
                role.buffs.Add(buff);
            }
        }


        /// <summary>
        /// 防守武将技能释放条件验证
        /// </summary>
        /// <param name="role">防守武将</param>
        /// <param name="nowcondition"></param>
        /// <param name="fight"></param>
        /// <param name="attrole"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private bool CheckDefenseRoleCondition(DefenseRoles role, List<int> nowcondition, WarFight fight, AttackRoles attrole, List<string> conditions)
        {
            var ismatch = true;
            foreach (var item in conditions)
            {
                var condition = Convert.ToInt32(item);
                #region 技能条件验证
                switch (condition)
                {
                    case (int)WarFightCondition.Attack: if (nowcondition.Contains(condition)) ismatch = false; break;
                    case (int)WarFightCondition.Rain: { if (fight.Weather != condition)ismatch = false; } break;
                    case (int)WarFightCondition.Attacked: { if (nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.FirstAttack: { if (nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.DarkRoleFirstAttack: { if (nowcondition.Contains(condition))ismatch = false; } break;
                    case (int)WarFightCondition.FiveSame: { if (fight.FiveSharp != condition)ismatch = false; } break;
                    case (int)WarFightCondition.SkillAttack: { if (nowcondition.Contains(condition))ismatch = false; } break;
                    // case (int)WarFightCondition.BloodLess: { if (role.SoldierCount > skill.effectBaseInfo.)ismatch = false; } break;
                    case (int)WarFightCondition.CharmLessMe: { if (role.Role.base_charm < attrole.Role.base_charm)ismatch = false; } break;
                    case (int)WarFightCondition.ForceLessMe: { if (role.Role.base_force < attrole.Role.base_force)ismatch = false; } break;
                    case (int)WarFightCondition.BrainLessMe: { if (role.Role.base_brains < attrole.Role.base_brains)ismatch = false; } break;
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
                            if (nowcondition.Contains(condition)) ismatch = false;
                        } break;

                }
                #endregion
            }
            return ismatch;
        }

        #region 士气和气力
        /// <summary>
        /// 获取回合开始时的士气和气力
        /// </summary>
        /// <param name="role"></param>
        private void GetDefenseRoleMoraleAndQili(DefenseRoles role)
        {
            role.morale -= 20;
            if (role.morale <= 0) role.morale = 0;
            role.qili += 10;
            if (role.qili > 100) role.qili = 100;
        }
        #endregion
    }
}
