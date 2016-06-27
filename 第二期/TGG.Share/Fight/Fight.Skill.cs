using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Global;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Entity;
using TGG.Core.Vo.Role;
using TGG.Core.Vo.Fight;

namespace TGG.Share.Fight
{
    public partial class Fight
    {
        #region  技能公共操作

        /// <summary> 解析技能基表效果 </summary>
        /// <param name="skill">触发方的技能Vo</param>
        /// <param name="type">触发的技能类型</param>
        private bool SkillParsing(SkillVo skill, int type)
        {
            var base_skill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == skill.baseId);
            if (base_skill == null) return false;

            var role = GetShotRole();
            if (role == null) return false;
#if DEBUG
            //base_skill.energy = 2;
#endif
            if (role.angerCount < base_skill.energy) return false;//气力验证

            role.angerCount = role.angerCount - base_skill.energy;//扣除气力
            var baseEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == base_skill.id && m.level == skill.level);
            if (baseEffect == null)
            {
                XTrace.WriteLine("{0}  技能基表Id:{1}  技能等级:{2}", "技能效果基表为空", base_skill.id, skill.level);
                return false;
            }
#if DEBUG
            if (type == (int)SkillType.CHEATCODE)
                XTrace.WriteLine("{0} {1} - {2} ", "出手武将", "触发秘技", "秘技基表Id " + skill.baseId + "  效果值 " + baseEffect.effects);
            else
                XTrace.WriteLine("{0} {1} - {2}", "出手武将", "触发奥义", "奥义基表Id " + skill.baseId + "  效果值 " + baseEffect.effects);
#endif


            var effects = baseEffect.effects;     //技能效果
            SkillEffect(effects, type);           //技能效果解析

            if (baseEffect.isQuickAttack == 1) //1为当前回合攻击  0为当前回合不攻击
                Attack(role, baseEffect.attackRange == (int)EffectRangeType.ALL);
            RemoveBuff(false);
            return true;
        }

        /// <summary> 技能效果 </summary>
        /// <param name="effects">技能效果</param>
        /// <param name="type">技能类型</param>
        private void SkillEffect(string effects, int type)
        {
#if DEBUG
            //effects = "10_1_1_2_50";
#endif
            //bool flag = true;
            var data = effects.Split('|');                 //拆分技能效果
            foreach (var item in data)
            {
                var skill = BuildSkillEffects(item);       //组装战斗技能效果实体
                if (skill == null) continue;
                var list = IsAttack ? GetTargetRole(skill, attack_matrix, defense_matrix)
                    : GetTargetRole(skill, defense_matrix, attack_matrix);
                if (!list.Any()) continue;
                EffectType(list, skill);

                if (type == (int)SkillType.CARD) continue;
                var movesvo = move.CloneDeepEntity();
                //movesvo.hitIds = list.Select(m => Convert.ToDouble(m.id)).ToList();
                InitRoleDamage();
                //if (!flag) type = (int)SkillType.CHEATCODE;
                BuildMovesVo(movesvo, type);
                //flag = false;
            }
        }

        /// <summary> 组装技能效果实体 </summary>
        /// <param name="item">要解析的字符串</param>
        /// <returns>FightSkillEffects</returns>
        private FightSkillEffects BuildSkillEffects(string item)
        {
            var array = item.Split('_');
            if (array.Length < 5 || array.Length > 6) return null;
            var skill = new FightSkillEffects();
            skill.type = Convert.ToInt32(array[0]);                  //技能效果类型
            skill.target = Convert.ToInt32(array[1]);                //技能效果目标 (1=本方 2=敌方)
            skill.range = Convert.ToInt32(array[2]);                 //技能效果范围 (1=单体 2=全体)
            skill.round = Convert.ToInt32(array[3]);                 //技能效果回合数
            skill.values = Convert.ToDouble(array[4]);               //技能效果值
            if (array.Length == 6)                                   //几率类型效果
                skill.robabilityValues = Convert.ToDouble(array[5]); //技能效果几率
            if (skill.robabilityValues <= 0) skill.robabilityValues = 100;
            return skill;
        }

        /// <summary> 获取技能目标武将 </summary>
        /// <param name="skill">技能</param>
        /// <param name="p">触发方的阵形</param>
        /// <param name="_p">触发方敌对的阵形</param>
        /// <returns></returns>
        private List<FightRole> GetTargetRole(FightSkillEffects skill, FightPersonal p, FightPersonal _p)
        {
            bool flag;
            switch (skill.target)                           //目标类型  1己方 2敌方
            {
                case (int)FightTargetType.OWN: { flag = true; break; }    //己方
                case (int)FightTargetType.ENEMY: { flag = false; break; } //敌方
                default: { return new List<FightRole>(); }
            }
            var tp = flag ? p : _p;

            switch (skill.range)                                                 //效果范围  1单人 2多人
            {
                case (int)EffectRangeType.SINGLE: { return GetTargetRole(tp, false, flag); }//单人
                case (int)EffectRangeType.ALL: { return GetTargetRole(tp, true, flag); }    //多人
                default: { return new List<FightRole>(); }
            }
        }

        /// <summary>效果类型</summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void EffectType(List<FightRole> list, FightSkillEffects skill)
        {
            if (!list.Any()) return;
            switch (skill.type) //技能效果类型
            {
                case (int)FightingSkillType.INCREASE_ATTACK: { RoleAddAttack(list, skill); break; } //增加攻击
                case (int)FightingSkillType.REDUCE_ATTACK: { RoleReduceAttack(list, skill); break; }//减少攻击
                case (int)FightingSkillType.INCREASE_DEFENSE: { RoleAddDefense(list, skill); break; }//增加防御
                case (int)FightingSkillType.REDUCE_DEFENSE: { RoleReduceDefense(list, skill); break; }//减少防御
                case (int)FightingSkillType.KMOWING_PROBABILITY: { RoleAddCritProbability(list, skill); break; }//会心几率
                case (int)FightingSkillType.INCREASE_KMOWING: { RoleAddCritKmowing(list, skill); break; }//增加会心效果
                case (int)FightingSkillType.DUCK_PROBABILITY: { RoleAddDodgeProbability(list, skill); break; }//闪避率
                case (int)FightingSkillType.IGNORE_DUCK_PROBABILITY: { RoleProbability(list, skill); break; }//无视闪避几率
                case (int)FightingSkillType.INCREASES_DAMAGE_PERCENTAGE: { RoleAddHurtIncrease(list, skill); break; }//增加伤害百分比
                case (int)FightingSkillType.REDUCE_DAMAGE_PERCENTAGE: { RoleAddHurtReduce(list, skill); break; }//降低伤害百分比
                case (int)FightingSkillType.DIZZINESS: { RoleDiziness(list, skill); break; }//眩晕
                case (int)FightingSkillType.SEAL: { RoleSeal(list, skill); break; }//封印
                case (int)FightingSkillType.INCREASES_STRENGTH: { RoleAddAngerCount(list, skill); break; }//增加气力
                case (int)FightingSkillType.REDUCE_STRENGTH: { RoleReduceAngerCount(list, skill); break; }//减少气力
                case (int)FightingSkillType.INCREASES_MYSTERY_PROBABILITY: { RoleAddMysteryProbability(list, skill); break; }//增加奥义触发几率
                case (int)FightingSkillType.INCREASES_YINCOUNT:
                    {
                        if (!IsTrue(skill.robabilityValues)) return;
                        AddYinCount(list, skill); break;
                    }//增加印计数
                case (int)FightingSkillType.REDUCE__YINCOUNT:
                    {
                        if (!IsTrue(skill.robabilityValues)) return;
                        ReduceYinCount(list, skill); break;
                    }//减少印计数
                default: { break; }
            }
        }

        /// <summary>无视闪避几率 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleProbability(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.IGNORE_DUCK_PROBABILITY);
                }

                var count = m.IgnoreDuck + skill.values;
                if (count < 0) { skill.values = m.IgnoreDuck; m.IgnoreDuck = 0; }
                else m.IgnoreDuck = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.IGNORE_DUCK_PROBABILITY, skill.values, true);
            }
        }

        /// <summary>增加武将奥义触发几率 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddMysteryProbability(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.INCREASES_MYSTERY_PROBABILITY);
                }
                var count = m.mystery_probability + skill.values;
                if (count < 0) { skill.values = m.mystery_probability; m.mystery_probability = 0; }
                else m.mystery_probability = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASES_MYSTERY_PROBABILITY, skill.values, true);
            }
        }


        /// <summary>增加武将会心几率 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddCritProbability(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.KMOWING_PROBABILITY);
                }
                var count = m.critProbability + skill.values;
                if (count < 0) { skill.values = m.critProbability; m.critProbability = 0; }
                else m.critProbability = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.KMOWING_PROBABILITY, skill.values, true);
            }
        }

        /// <summary>增加武将会心效果 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddCritKmowing(List<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.INCREASE_KMOWING);
                }

                var count = m.critAddition + skill.values;
                if (count < 0) { skill.values = m.critAddition; m.critAddition = 0; }
                else m.critAddition = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASE_KMOWING, skill.values, true);
            }
        }

        /// <summary>增加武将攻击 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddAttack(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.INCREASE_ATTACK);
                }

                var count = m.attack + skill.values;
                if (count < 0) { skill.values = m.attack; m.attack = 0; }
                else m.attack = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASE_ATTACK, skill.values, true);
            }
        }

        /// <summary>减少武将攻击 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill">技能效果</param>
        private void RoleReduceAttack(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.REDUCE_ATTACK);
                }
                var count = m.attack - skill.values;
                if (count < 0) { skill.values = m.attack; m.attack = 0; }
                else m.attack = count;
                AddRoleBuff(m, skill.round, (int)FightingSkillType.REDUCE_ATTACK, skill.values, true);
            }
        }

        /// <summary>增加武将防御 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddDefense(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.INCREASE_DEFENSE);
                }
                m.defense = m.defense + skill.values;
                if (m.defense < 0) { m.defense = 0; skill.values = 0; }
                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASE_DEFENSE, skill.values, true);
            }
        }

        /// <summary>减少武将防御 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleReduceDefense(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.REDUCE_DEFENSE);
                }
                var count = m.defense - skill.values;
                if (count < 0) { skill.values = m.defense; m.defense = 0; }
                else m.defense = count;
                AddRoleBuff(m, skill.round, (int)FightingSkillType.REDUCE_DEFENSE, skill.values, false);
            }
        }

        /// <summary>增加武将闪避几率 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddDodgeProbability(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.DUCK_PROBABILITY);
                }

                var count = m.dodgeProbability + skill.values;
                if (count < 0) { skill.values = m.dodgeProbability; m.dodgeProbability = 0; }
                else m.dodgeProbability = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.DUCK_PROBABILITY, skill.values, true);
            }
        }

        /// <summary>增加武将增伤 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddHurtIncrease(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.INCREASES_DAMAGE_PERCENTAGE);
                }

                var count = m.hurtIncrease + skill.values;
                if (count < 0) { skill.values = m.hurtIncrease; m.hurtIncrease = 0; }
                else m.hurtIncrease = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASES_DAMAGE_PERCENTAGE, skill.values, true);
            }
        }

        /// <summary>增加武将减伤 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddHurtReduce(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (skill.round > 0 && skill.round != 99999)
                {
                    if (!IsTrue(skill.robabilityValues)) continue;
                    RemoveRoleBuff(m, (int)FightingSkillType.REDUCE_DAMAGE_PERCENTAGE);
                }

                var count = m.hurtReduce + skill.values;
                if (count < 0) { skill.values = m.hurtReduce; m.hurtReduce = 0; }
                else m.hurtReduce = count;

                AddRoleBuff(m, skill.round, (int)FightingSkillType.REDUCE_DAMAGE_PERCENTAGE, skill.values, true);
            }
        }

        /// <summary>增加武将气力 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleAddAngerCount(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
#if DEBUG
                XTrace.WriteLine("{0} {1} {2}", "原本气力 " + m.angerCount, "增加气力 " + skill.values, "剩余气力 " + m.angerCount + skill.values);
#endif
                if (!IsTrue(skill.robabilityValues)) continue;
                m.angerCount = m.angerCount + Convert.ToInt32(skill.values);
                AddRoleBuff(m, skill.round, (int)FightingSkillType.INCREASES_STRENGTH, skill.values, true);
            }
        }

        /// <summary>减少武将气力 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleReduceAngerCount(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            foreach (var m in list)
            {
                if (!IsTrue(skill.robabilityValues)) continue;
                var count = m.angerCount - Convert.ToInt32(skill.values);
                if (count < 0) { skill.values = m.angerCount; m.angerCount = 0; }
                AddRoleBuff(m, skill.round, (int)FightingSkillType.REDUCE_STRENGTH, skill.values, false);
#if DEBUG
                XTrace.WriteLine("{0} {1} {2}", "原本气力 " + m.angerCount, "减少气力 " + skill.values, "剩余气力 " + count);
#endif
            }
        }

        ///  <summary> 眩晕武将 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleDiziness(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            if (skill.round < 0) return;
            foreach (var m in list)
            {
                if (!IsTrue(skill.robabilityValues)) continue;  //是否眩晕
                AddRoleBuff(m, skill.round, (int)FightingSkillType.DIZZINESS, true);
            }
        }

        /// <summary> 封印武将 </summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void RoleSeal(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            if (skill.round < 0) return;
            foreach (var m in list)
            {
                if (!IsTrue(skill.robabilityValues)) continue;  //是否封印
                AddRoleBuff(m, skill.round, (int)FightingSkillType.SEAL, true);
            }
        }

        /// <summary>增加印计数</summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void AddYinCount(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            var role = list.First();
            var userid = role.user_id;
            var value = Convert.ToInt32(skill.values);

            if (!dic_yincount.ContainsKey(userid))
                dic_yincount.Add(userid, value);
            else
            {
                dic_yincount[userid] += value;
#if DEBUG
                XTrace.WriteLine("{0} {1}", "增加印计数 " + value, "当前印计数 " + dic_yincount[userid]);
#endif
            }
            GetYinCount(move);
            AddRoleBuff(role, skill.round, (int)FightingSkillType.INCREASES_YINCOUNT, value, true);
        }

        /// <summary>减少印计数</summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void ReduceYinCount(IEnumerable<FightRole> list, FightSkillEffects skill)
        {
            var role = list.First();
            var userid = role.user_id;
            var value = Convert.ToInt32(skill.values);

            if (!dic_yincount.ContainsKey(userid))
                dic_yincount.Add(userid, 0);
            else
            {
                var yincount = dic_yincount[userid];
                var count = yincount - value;
                dic_yincount[userid] = count < 0 ? 0 : count;
#if DEBUG
                XTrace.WriteLine("{0} {1} {2}", "原本印计数 " + yincount, "减少印计数 " + value, "剩余印计数 " + count);
#endif
            }
            GetYinCount(move);
            AddRoleBuff(role, skill.round, (int)FightingSkillType.REDUCE__YINCOUNT, value, false);
        }


        /// <summary> 增加武将Buff</summary>
        /// <param name="role">武将</param>
        /// <param name="round">影响回合数</param>
        /// <param name="type">技能类型</param>
        /// <param name="flag">true:增加 false:减少</param>
        private void AddRoleBuff(FightRole role, int round, int type, bool flag)
        {
            AddRoleBuff(role, round, type, 0, flag);
        }

        /// <summary> 增加武将Buff</summary>
        /// <param name="role">武将</param>
        /// <param name="round">影响回合数</param>
        /// <param name="type">技能类型</param>
        /// <param name="values">技能效果值</param>
        /// <param name="flag">true:增加 false:减少</param>
        private void AddRoleBuff(FightRole role, int round, int type, double values, bool flag)
        {
            if (round < 0) return;
            switch (round)
            {
                case 0:
                    {
                        var roleAtt = new FightRoleBuff { id = role.id, round = 0, type = type, values = -values, state = 0 };
                        list_buff.Add(roleAtt); break;
                    }
                case 99999: { role.foreverBuffVos.Add(new BuffVo { type = type, buffValue = values }); return; }
                default:
                    {
                        var roleAtt = new FightRoleBuff { id = role.id, round = Round + (round - 1), type = type, values = -values, state = 0 };
                        list_buff.Add(roleAtt); break;
                    }
            }
            role.buffVos2.Add(new BuffVo { type = type, buffValue = flag ? values : -values });
#if DEBUG
            XTrace.WriteLine(string.Format("{0} {1} {2} - {3}", "增加  武将 " + role.id + " BUFF ", " 类型 " + type, " 过期回合数 " + (Round + (round - 1)), " 效果值 " + values));
#endif
        }

        /// <summary> 获取目标武将 </summary>
        /// <param name="p">受影响方的阵形</param>
        /// <param name="flag">目标范围 true:全体 flase:单体</param>
        /// <param name="istarget">是否己方为目标</param>
        private List<FightRole> GetTargetRole(FightPersonal p, bool flag, bool istarget)
        {
            if (!istarget) return GetRoleOrRoleAll(p, flag);
            if (flag) return GetRoleOrRoleAll(p, true);
            var list = new List<FightRole> { GetShotRole() };
            return list;
        }

        /// <summary> 获取武将</summary>
        /// <param name="p">要获取方的阵</param>
        /// <param name="flag">获取单个还是全体  true:全体  false:单体</param>
        private List<FightRole> GetRoleOrRoleAll(FightPersonal p, bool flag)
        {
            var list = new List<FightRole>();
            if (!flag)
            {
                var role = GetFrontRole(p);
                if (role != null)
                    list.Add(role);
            }
            else
                list = FindRoleList(p.user_id);
            //foreach (var item in list)//清除伤害
            //{
            //    item.damage = 0;
            //}
            return list;
        }

        /// <summary>根据用户id获取所有武将</summary>
        /// <param name="userid">用户id</param>
        private List<FightRole> FindRoleList(decimal userid)
        {
            return IsFirst(userid) ? list_attack_role.Where(m => m != null && m.hp > 0).ToList() :
                list_defense_role.Where(m => m != null && m.hp > 0).ToList();
        }
        #endregion
    }
}
