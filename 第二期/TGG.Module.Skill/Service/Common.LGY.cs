using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Role;
using TGG.Share;
using TGG.SocketServer;
using Task = System.Threading.Tasks.Task;

namespace TGG.Module.Skill.Service
{
    public partial class Common
    {
        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilData(int result, Int64 rid)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            dic.Add("role", rid != 0 ? RoleInfo(rid) : null);
            return dic;
        }

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }


        /// <summary>创建生活技能修炼线程</summary>
        public void ThreadingUpgrade(Int64 user_id, Int64 rid, Int64 costtime)
        {
            try
            {
                var token = new CancellationTokenSource();
                //# if DEBUG
                //                costtime = 5000;
                //#endif
                Object obj = new LifeObject { user_id = user_id, rid = rid };
                Task.Factory.StartNew(m =>
                {
                    var t = m as LifeObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(costtime));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var lo = n as LifeObject;
                    if (lo == null) { token.Cancel(); return; }
                    SKILL_LIFE_PUSH.GetInstance().LifePush(lo.user_id, lo.rid);
                    //移除全局变量
                    var key = lo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public class LifeObject
        {
            public Int64 user_id { get; set; }
            public Int64 rid { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.LifeSkill, user_id, rid);
            }
        }

        /// <summary>切割学习技能</summary>
        public List<int> SkillBeforePracticeSplit(string condition)
        {
            var list = new List<int>();
            if (condition.Length <= 0) return list;
            if (condition.Contains('|'))
            {
                var ids = condition.Split('|');
                list.AddRange(ids.Select(item => Convert.ToInt32(item)));
            }
            else
                list.Add(Convert.ToInt32(condition));
            return list;
        }

        /// <summary>反射武将公共方法</summary>
        public RoleInfoVo RoleInfo(Int64 rid)
        {
            return (new Share.Role()).BuildRole(rid);
        }

        /// <summary>切割生活技能效果</summary>
        public List<SkillEffect> SkillEffectSplit(BaseLifeSkillEffect next_effect)
        {
            var list_effect = new List<SkillEffect>();
            if (next_effect.effect.Length <= 0) return null;
            var skilleffect = next_effect.effect;
            if (skilleffect.Length <= 0) return null;
            if (skilleffect.Contains('|'))
            {
                var effect = skilleffect.Split('|');
                foreach (var item in effect)
                {
                    var se = new SkillEffect();
                    var values = item.Split('_');
                    se.type = Convert.ToInt32(values[0]);
                    se.value = Convert.ToDouble(values[3]);
                    list_effect.Add(se);
                }
            }
            else
            {
                var se = new SkillEffect();
                var values = skilleffect.Split('_');
                se.type = Convert.ToInt32(values[0]);
                se.value = Convert.ToDouble(values[3]);
                list_effect.Add(se);
            }
            return list_effect;
        }

        /// <summary>获取技能类型</summary>
        public int GetSkillType(int id)
        {
            var base_skill = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == id);
            if (base_skill == null) return 0;
            return base_skill.type;
        }

        /// <summary>是否已在学习其他技能 </summary>
        public int SkillStudying(RoleItem roleitem)
        {
            #region
            if (roleitem.LifeSkill.sub_archer_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_archer;
            if (roleitem.LifeSkill.sub_artillery_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_artillery;
            if (roleitem.LifeSkill.sub_ashigaru_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_ashigaru;
            if (roleitem.LifeSkill.sub_build_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_build;
            if (roleitem.LifeSkill.sub_calculate_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_calculate;
            if (roleitem.LifeSkill.sub_craft_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_craft;
            if (roleitem.LifeSkill.sub_eloquence_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_eloquence;
            if (roleitem.LifeSkill.sub_equestrian_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_equestrian;
            if (roleitem.LifeSkill.sub_etiquette_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_etiquette;
            if (roleitem.LifeSkill.sub_martial_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_martial;
            if (roleitem.LifeSkill.sub_medical_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_medical;
            if (roleitem.LifeSkill.sub_mine_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_mine;
            if (roleitem.LifeSkill.sub_ninjitsu_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_ninjitsu;
            if (roleitem.LifeSkill.sub_reclaimed_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_reclaimed;
            if (roleitem.LifeSkill.sub_tactical_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_tactical;
            if (roleitem.LifeSkill.sub_tea_state == (int)SkillLearnType.STUDYING) return roleitem.LifeSkill.sub_tea;
            return 0;
            #endregion
        }


        /// <summary>获取技能等级和时间</summary>
        public SkillLevelAndTime GetSkillLevel(int type, RoleItem roleitem)
        {
            #region
            switch (type)
            {
                case (int)LifeSkillType.ASHIGARU:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_ashigaru_level, roleitem.LifeSkill.sub_ashigaru_time);
                case (int)LifeSkillType.ARTILLERY:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_artillery_level, roleitem.LifeSkill.sub_artillery_time);
                case (int)LifeSkillType.ARCHER:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_archer_level, roleitem.LifeSkill.sub_archer_time);
                case (int)LifeSkillType.BUILD:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_build_level, roleitem.LifeSkill.sub_build_time);
                case (int)LifeSkillType.CALCULATE:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_calculate_level, roleitem.LifeSkill.sub_calculate_time);
                case (int)LifeSkillType.CRAFT:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_craft_level, roleitem.LifeSkill.sub_craft_time);
                case (int)LifeSkillType.ELOQUENCE:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_eloquence_level, roleitem.LifeSkill.sub_eloquence_time);
                case (int)LifeSkillType.EQUESTRIAN:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_equestrian_level, roleitem.LifeSkill.sub_equestrian_time);
                case (int)LifeSkillType.ETIQUETTE:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_etiquette_level, roleitem.LifeSkill.sub_etiquette_time);
                case (int)LifeSkillType.MARTIAL:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_martial_level, roleitem.LifeSkill.sub_martial_time);
                case (int)LifeSkillType.MEDICAL:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_medical_level, roleitem.LifeSkill.sub_medical_time);
                case (int)LifeSkillType.MINE:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_mine_level, roleitem.LifeSkill.sub_mine_time);
                case (int)LifeSkillType.NINJITSU:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_ninjitsu_level, roleitem.LifeSkill.sub_ninjitsu_time);
                case (int)LifeSkillType.RECLAIMED:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_reclaimed_level, roleitem.LifeSkill.sub_reclaimed_time);
                case (int)LifeSkillType.TACTICAL:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_tactical_level, roleitem.LifeSkill.sub_tactical_time);
                case (int)LifeSkillType.TEA:
                    return GetLevelAndTime(roleitem.LifeSkill.sub_tea_level, roleitem.LifeSkill.sub_tea_time);
            }
            return null;
            #endregion
        }

        public class SkillLevelAndTime
        {
            /// <summary> 等级 </summary>
            public int level { get; set; }

            /// <summary> 时间 </summary>
            public Int64 time { get; set; }
        }

        /// <summary>生活技能重新加入线程</summary>
        public void RoleLifeSkillRecovery()
        {
            #region
            var list_user = tg_user.FindAll().ToList();
            var list = tg_role.FindAll().ToList();
            if(!list_user.Any()||!list.Any())return;
            var ids = list.Select(m => m.id).ToList();
            if (!ids.Any()) return;
            var view_roles = view_role.GetRoleById(ids);
            if (!view_roles.Any()) return;
            foreach (var user in list_user)
            {
                var time = CurrentMs() + 5000;//+5000毫秒预热
                var roles = view_roles.Where(m => m.Kind.user_id == user.id);
                foreach (var item in roles)
                {
                    var baseid = SkillStudying(item);
                    if (baseid > 0)
                    {
                        var base_life = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
                        if (base_life == null) return;
                        var slt = GetSkillLevel(base_life.type, item);
                        if (slt.time < time)
                        {
                            var base_effect =
                                Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(
                                    m => m.level == slt.level && m.skillid == baseid);
                            var next_effect =
                                Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(
                                    m => base_effect != null && m.id == base_effect.nextId);
                            if (next_effect != null)
                            {
                                //var roleitem = item;
                                var life = tg_role_life_skill.GetEntityByRid(item.Kind.id);
                                var role = tg_role.GetEntityById(item.Kind.id);
                                //生活技能升级处理
                                life = GetRoleLifeSkill(base_life.type, life, next_effect.level);
                                life = SkillStateChange(base_life, life);
                                var value = SkillEffectSplit(next_effect);
                                role = SkillEffectIncrease(role, value);
                                life.Update();
                                role.Update();
                            }
                        }
                        else
                        {
                            var _time = slt.time - CurrentMs();
                            if (_time < 0) continue;
                            ThreadingUpgrade(item.Kind.user_id, item.Kind.id, _time);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>更改后置技能状态</summary>
        /// <returns>生活技能实体</returns>
        public tg_role_life_skill SkillStateChange(BaseLifeSkill baselife, tg_role_life_skill life)
        {
            var ids = SkillBeforePracticeSplit(baselife.studypostposition);//后置技能id集合
            ids = GetNoShoolIds(ids, life);
            if (ids.Count > 0)
            {
                var st = GetSkillLevel(baselife.type, life);
                life = SkillStateChange(ids, life, st.level);
            }
            return life;
        }

        /// <summary> 技能学习加速消耗的元宝 </summary>
        /// <param name="time">到达时间</param>
        /// <returns>消耗元宝</returns>
        public int Consume(decimal time)
        {
            var minute = (double)((time - CurrentTime()) / 1000 / 60);
            if (minute <= 0) return 0;
            var rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "16001");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("minute", minute.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        /// <summary>生活技能等级和时间 </summary>
        public SkillLevelAndTime GetLevelAndTime(int level, Int64 time)
        {
            return new SkillLevelAndTime { level = level, time = time };
        }

        /// <summary>技能效果类</summary>
        public class SkillEffect
        {
            /// <summary> 类型 </summary>
            public int type { get; set; }

            /// <summary> 值 </summary>
            public double value { get; set; }
        }


        #region arlen 2014-09-23 update

        /// <summary>是否已在学习其他技能 </summary>
        public int SkillStudying(tg_role_life_skill life)
        {
            #region
            if (life.sub_archer_state == (int)SkillLearnType.STUDYING) return life.sub_archer;
            if (life.sub_artillery_state == (int)SkillLearnType.STUDYING) return life.sub_artillery;
            if (life.sub_ashigaru_state == (int)SkillLearnType.STUDYING) return life.sub_ashigaru;
            if (life.sub_build_state == (int)SkillLearnType.STUDYING) return life.sub_build;
            if (life.sub_calculate_state == (int)SkillLearnType.STUDYING) return life.sub_calculate;
            if (life.sub_craft_state == (int)SkillLearnType.STUDYING) return life.sub_craft;
            if (life.sub_eloquence_state == (int)SkillLearnType.STUDYING) return life.sub_eloquence;
            if (life.sub_equestrian_state == (int)SkillLearnType.STUDYING) return life.sub_equestrian;
            if (life.sub_etiquette_state == (int)SkillLearnType.STUDYING) return life.sub_etiquette;
            if (life.sub_martial_state == (int)SkillLearnType.STUDYING) return life.sub_martial;
            if (life.sub_medical_state == (int)SkillLearnType.STUDYING) return life.sub_medical;
            if (life.sub_mine_state == (int)SkillLearnType.STUDYING) return life.sub_mine;
            if (life.sub_ninjitsu_state == (int)SkillLearnType.STUDYING) return life.sub_ninjitsu;
            if (life.sub_reclaimed_state == (int)SkillLearnType.STUDYING) return life.sub_reclaimed;
            if (life.sub_tactical_state == (int)SkillLearnType.STUDYING) return life.sub_tactical;
            if (life.sub_tea_state == (int)SkillLearnType.STUDYING) return life.sub_tea;
            #endregion
            return 0;
        }


        /// <summary>获取技能等级和时间</summary>
        public SkillLevelAndTime GetSkillLevel(int type, tg_role_life_skill life)
        {
            Int32 level = 0; Int64 time = 0;
            switch (type)
            {               
                #region
                case (int)LifeSkillType.ASHIGARU: { level = life.sub_ashigaru_level; time = life.sub_ashigaru_time; break; }
                case (int)LifeSkillType.ARTILLERY: { level = life.sub_artillery_level; time = life.sub_artillery_time; break; }
                case (int)LifeSkillType.ARCHER: { level = life.sub_archer_level; time = life.sub_archer_time; break; }
                case (int)LifeSkillType.BUILD: { level = life.sub_build_level; time = life.sub_build_time; break; }
                case (int)LifeSkillType.CALCULATE: { level = life.sub_calculate_level; time = life.sub_calculate_time; break; }
                case (int)LifeSkillType.CRAFT: { level = life.sub_craft_level; time = life.sub_craft_time; break; }
                case (int)LifeSkillType.ELOQUENCE: { level = life.sub_eloquence_level; time = life.sub_eloquence_time; break; }
                case (int)LifeSkillType.EQUESTRIAN: { level = life.sub_equestrian_level; time = life.sub_equestrian_time; break; }
                case (int)LifeSkillType.ETIQUETTE: { level = life.sub_etiquette_level; time = life.sub_etiquette_time; break; }
                case (int)LifeSkillType.MARTIAL: { level = life.sub_martial_level; time = life.sub_martial_time; break; }
                case (int)LifeSkillType.MEDICAL: { level = life.sub_medical_level; time = life.sub_medical_time; break; }
                case (int)LifeSkillType.MINE: { level = life.sub_mine_level; time = life.sub_mine_time; break; }
                case (int)LifeSkillType.NINJITSU: { level = life.sub_ninjitsu_level; time = life.sub_ninjitsu_time; break; }
                case (int)LifeSkillType.RECLAIMED: { level = life.sub_reclaimed_level; time = life.sub_reclaimed_time; break; }
                case (int)LifeSkillType.TACTICAL: { level = life.sub_tactical_level; time = life.sub_tactical_time; break; }
                case (int)LifeSkillType.TEA: { level = life.sub_tea_level; time = life.sub_tea_time; break; }
                #endregion
            }
            return new SkillLevelAndTime { level = level, time = time };
        }

        /// <summary>生活技能锻炼完处理</summary>
        public tg_role_life_skill GetRoleLifeSkill(int life_type, tg_role_life_skill life, int level)
        {
            var type = (int)SkillLearnType.LEARNED;
            switch (life_type)
            {
                #region
                case (int)LifeSkillType.ASHIGARU:
                    {
                        life.sub_ashigaru_time = 0; life.sub_ashigaru_level = level;
                        life.sub_ashigaru_state = type; break;
                    }

                case (int)LifeSkillType.ARTILLERY:
                    {
                        life.sub_artillery_time = 0; life.sub_artillery_level = level;
                        life.sub_artillery_state = type; break;
                    }
                case (int)LifeSkillType.ARCHER:
                    {
                        life.sub_archer_time = 0; life.sub_archer_level = level;
                        life.sub_archer_state = type; break;
                    }
                case (int)LifeSkillType.BUILD:
                    {
                        life.sub_build_time = 0; life.sub_build_level = level;
                        life.sub_build_state = type; break;
                    }
                case (int)LifeSkillType.CALCULATE:
                    {
                        life.sub_calculate_time = 0; life.sub_calculate_level = level;
                        life.sub_calculate_state = type; break;
                    }
                case (int)LifeSkillType.CRAFT:
                    {
                        life.sub_craft_time = 0; life.sub_craft_level = level;
                        life.sub_craft_state = type; break;
                    }
                case (int)LifeSkillType.ELOQUENCE:
                    {

                        life.sub_eloquence_time = 0; life.sub_eloquence_level = level;
                        life.sub_eloquence_state = type; break;
                    }
                case (int)LifeSkillType.EQUESTRIAN:
                    {
                        life.sub_equestrian_time = 0; life.sub_equestrian_level = level;
                        life.sub_equestrian_state = type; break;
                    }
                case (int)LifeSkillType.ETIQUETTE:
                    {
                        life.sub_etiquette_time = 0; life.sub_etiquette_level = level;
                        life.sub_etiquette_state = type; break;
                    }
                case (int)LifeSkillType.MARTIAL:
                    {
                        life.sub_martial_time = 0; life.sub_martial_level = level;
                        life.sub_martial_state = type; break;
                    }
                case (int)LifeSkillType.MEDICAL:
                    {
                        life.sub_medical_time = 0; life.sub_medical_level = level;
                        life.sub_medical_state = type; break;
                    }
                case (int)LifeSkillType.MINE:
                    {
                        life.sub_mine_time = 0; life.sub_mine_level = level;
                        life.sub_mine_state = type; break;
                    }
                case (int)LifeSkillType.NINJITSU:
                    {
                        life.sub_ninjitsu_time = 0; life.sub_ninjitsu_level = level;
                        life.sub_ninjitsu_state = type; break;
                    }
                case (int)LifeSkillType.RECLAIMED:
                    {
                        life.sub_reclaimed_time = 0; life.sub_reclaimed_level = level;
                        life.sub_reclaimed_state = type; break;
                    }
                case (int)LifeSkillType.TACTICAL:
                    {
                        life.sub_tactical_time = 0; life.sub_tactical_level = level;
                        life.sub_tactical_state = type; break;
                    }
                case (int)LifeSkillType.TEA:
                    {
                        life.sub_tea_time = 0; life.sub_tea_level = level;
                        life.sub_tea_state = type; break;
                    }
                #endregion
            }
            //life.Update();
            return life;
        }

        /// <summary>升级后增加技能效果</summary>
        public tg_role SkillEffectIncrease(tg_role role, IEnumerable<SkillEffect> n_effect)
        {
             var mn = (int) ModuleNumber.EQUIP;
            var sc = (int)SkillCommand.SKILL_LIFE_PUSH;
            foreach (var item in n_effect)
            {
                switch (item.type)
                {
                    #region
                    case (int)LifeSkillEffectType.INCREASE_BRAINS:  //智谋
                        {
                            var brain = role.base_brains_life;
                            role.base_brains_life += item.value;
                            RoleAttrituteLog(role.user_id, item.type, item.value, brain, role.base_brains_life, mn, sc);
                            break;
                        }
                    case (int)LifeSkillEffectType.INCREASE_CAPTAIN://统率
                        {
                            var captain = role.base_captain_life;
                            role.base_captain_life += item.value;
                            RoleAttrituteLog(role.user_id, item.type, item.value, captain, role.base_captain_life, mn, sc);
                            break;
                        }
                    case (int)LifeSkillEffectType.INCREASE_CHARM://魅力
                        {
                            var charm = role.base_charm_life;
                            role.base_charm_life += item.value;
                            RoleAttrituteLog(role.user_id, item.type, item.value, charm, role.base_charm_life, mn, sc);
                            break;
                        }
                    case (int)LifeSkillEffectType.INCREASE_FORCE://武力
                        {
                            var force = role.base_force_life;
                            role.base_force_life += item.value;
                            RoleAttrituteLog(role.user_id, item.type, item.value, force, role.base_force_life, mn, sc);
                            break;
                        }
                    case (int)LifeSkillEffectType.INCREASE_GOVERN://政务
                        {
                            var govern = role.base_govern_life;
                            role.base_govern_life += item.value;
                            RoleAttrituteLog(role.user_id, item.type, item.value, govern, role.base_govern_life, mn, sc);
                            break;
                        }
                    case (int)LifeSkillEffectType.LIFE_INCREASE:
                        {
                            var life = role.att_life;
                            role.att_life += Convert.ToInt32(item.value);
                            RoleAttrituteLog(role.user_id, item.type, item.value, life, role.att_life, mn, sc);
                        }
                        break;
                    #endregion
                }
            }
            return role;
        }

        /// <summary>武将属性变动添加日志 </summary>
        /// <param name="userid">玩家id</param>
        /// <param name="type">属性类型</param>
        /// <param name="add">增加的值</param>
        /// <param name="ov">武将增加前的属性值</param>
        /// <param name="nv">武将增加后的属性值</param>
        /// <param name="ModuleNumber">模块号</param>
        /// <param name="command">指令号</param>
        private void RoleAttrituteLog(Int64 userid, int type, double add, double ov, double nv, int ModuleNumber, int command)
        {
            string logdata = "";
            switch (type)
            {
                #region
                case (int)LifeSkillEffectType.INCREASE_BRAINS:  //智谋
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleBrains", ov, add, nv);
                        break;
                    }
                case (int)LifeSkillEffectType.INCREASE_CAPTAIN://统率
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCaptain", ov, add, nv);
                        break;
                    }
                case (int)LifeSkillEffectType.INCREASE_CHARM://魅力
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCharm", ov, add, nv);
                        break;
                    }
                case (int)LifeSkillEffectType.INCREASE_FORCE://武力
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleForce", ov, add, nv);
                        break;
                    }
                case (int)LifeSkillEffectType.INCREASE_GOVERN://政务
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleGovern", ov, add, nv);
                        break;
                    }
                case (int)LifeSkillEffectType.LIFE_INCREASE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleLife", ov, add, nv);
                        break;
                    }                                               
                #endregion
            }
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, ModuleNumber, command, logdata);// (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_LOCK, logdata);
        }

        /// <summary>把技能学习状态标志为可学</summary>
        public tg_role_life_skill SkillStateChange(IEnumerable<int> ids, tg_role_life_skill life, int level)
        {
            var list_lifeskillid = SkillStudied(life);
            foreach (var baseid in ids)
            {
                var temp = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
                var type = temp != null ? temp.type : 0;
                if (!IsStudied(temp, temp.studyCondition, list_lifeskillid, life)) continue;
                switch (type)
                {
                    #region
                    case (int)LifeSkillType.ASHIGARU: life.sub_ashigaru_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.ARTILLERY: life.sub_artillery_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.ARCHER: life.sub_archer_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.BUILD: life.sub_build_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.CALCULATE: life.sub_calculate_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.CRAFT: life.sub_craft_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.ELOQUENCE: life.sub_eloquence_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.EQUESTRIAN: life.sub_equestrian_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.ETIQUETTE: life.sub_etiquette_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.MARTIAL: life.sub_martial_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.MEDICAL: life.sub_medical_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.MINE: life.sub_mine_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.NINJITSU: life.sub_ninjitsu_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.RECLAIMED: life.sub_reclaimed_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.TACTICAL: life.sub_tactical_state = (int)SkillLearnType.TOLEARN; break;
                    case (int)LifeSkillType.TEA: life.sub_tea_state = (int)SkillLearnType.TOLEARN; break;
                    #endregion
                }
            }
            return life;
        }

        /// <summary>判断技能状态是否未学</summary>
        public bool IsTolearn(int state)
        {
            if (state == (int)SkillLearnType.NOSCHOOL)
                return true;
            return false;
        }

        /// <summary>获取未学技能id集合</summary>
        public List<int> GetNoShoolIds(IEnumerable<int> ids, tg_role_life_skill life)
        {
            List<int> _ids = new List<int>();
            foreach (var baseid in ids)
            {
                var temp = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
                var type = temp != null ? temp.type : 0;
                switch (type)
                {
                    #region
                    case (int)LifeSkillType.ASHIGARU: if (IsTolearn(life.sub_ashigaru_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.ARTILLERY: if (IsTolearn(life.sub_artillery_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.ARCHER: if (IsTolearn(life.sub_archer_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.BUILD: if (IsTolearn(life.sub_build_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.CALCULATE: if (IsTolearn(life.sub_calculate_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.CRAFT: if (IsTolearn(life.sub_craft_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.ELOQUENCE: if (IsTolearn(life.sub_eloquence_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.EQUESTRIAN: if (IsTolearn(life.sub_equestrian_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.ETIQUETTE: if (IsTolearn(life.sub_etiquette_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.MARTIAL: if (IsTolearn(life.sub_martial_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.MEDICAL: if (IsTolearn(life.sub_medical_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.MINE: if (IsTolearn(life.sub_mine_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.NINJITSU: if (IsTolearn(life.sub_ninjitsu_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.RECLAIMED: if (IsTolearn(life.sub_reclaimed_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.TACTICAL: if (IsTolearn(life.sub_tactical_state)) { _ids.Add(baseid); } break;
                    case (int)LifeSkillType.TEA: if (IsTolearn(life.sub_tea_state)) { _ids.Add(baseid); } break;
                    #endregion
                }
            }
            return _ids;
        }


        /// <summary>判断前置技能是否学习及前置技能等级是否足够</summary>
        public bool IsStudied(BaseLifeSkill base_life, string condition, ICollection<int> list, tg_role_life_skill life)
        {
            if (condition.Length == 0) return true;
            var ids = SkillBeforePracticeSplit(condition);//前置技能id集合           
            if (ids.Count == 0) return false;
            foreach (var item in ids)
            {
                var id = list.FirstOrDefault(m => m == item);
                if (id > 0)
                {
                    var type = GetSkillType(id);
                    var st = GetSkillLevel(type, life);
                    if (st.level < base_life.conditionLevel) return false;
                }
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 生活技能学习升级处理
        /// </summary>
        public void SkillChange(Int64 userid)
        {
            var list = tg_role.GetFindAllByUserId(userid);
            if(!list.Any())return;
            //var ids = list.Select(m => m.id).ToList();
            //if (!ids.Any()) return;
            //var roles = view_role.GetRoleById(ids);
            //if (!roles.Any()) return;
            foreach (var item in list)
            {
                var life = tg_role_life_skill.GetEntityByRid(item.id);
                var _ids = SkillNoStudy(life);
                if (_ids.Any())
                {
                    foreach (var baseid in _ids)
                    {
                        var base_life = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
                        if (base_life == null) continue;

                        var studyids = SkillStudied(life);
                        if (IsStudied(base_life, base_life.studyCondition, studyids, life))
                        {
                            life = StateChange(life, baseid);
                            life.Update();
                        }

                    }
                }
            }

        }

        /// <summary>
        /// 生活技能学习升级处理
        /// </summary>
        public void SkillChange()
        {
            var list_user = tg_user.FindAll().ToList();
            var list_role = tg_role.FindAll().ToList();
            if (!list_user.Any() || !list_role.Any()) return;
            var ids = list_role.Select(m => m.id).ToList();
            if (!ids.Any()) return;
            var view_roles = view_role.GetRoleById(ids);
            if (!view_roles.Any()) return;
            foreach (var user in list_user)
            {
                var roles = view_roles.Where(m => m.Kind.user_id == user.id).ToList();
                foreach (var item in roles)
                {
                    var _ids = SkillNoStudy(item.LifeSkill);
                    if (_ids.Any())
                    {
                        foreach (var baseid in _ids)
                        {
                            var base_life = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
                            if (base_life == null) continue;

                            var studyids = SkillStudied(item.LifeSkill);
                            if (IsStudied(base_life, base_life.studyCondition, studyids, item.LifeSkill))
                            {
                                item.LifeSkill = StateChange(item.LifeSkill, baseid);
                                item.LifeSkill.Update();
                            }

                        }
                    }
                }
            }

        }


        /// <summary>获取未学习的技能id集合</summary>
        public List<int> SkillNoStudy(tg_role_life_skill life)
        {
            var ids = new List<int>();
            #region
            if (life.sub_archer_level == 0 && life.sub_archer_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_archer);
            if (life.sub_artillery_level == 0 && life.sub_artillery_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_artillery);
            if (life.sub_ashigaru_level == 0 && life.sub_ashigaru_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_ashigaru);
            if (life.sub_build_level == 0 && life.sub_build_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_build);
            if (life.sub_calculate_level == 0 && life.sub_calculate_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_calculate);
            if (life.sub_craft_level == 0 && life.sub_craft_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_craft);
            if (life.sub_eloquence_level == 0 && life.sub_eloquence_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_eloquence);
            if (life.sub_equestrian_level == 0 && life.sub_equestrian_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_equestrian);
            if (life.sub_etiquette_level == 0 && life.sub_etiquette_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_etiquette);
            if (life.sub_martial_level == 0 && life.sub_martial_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_martial);
            if (life.sub_medical_level == 0 && life.sub_medical_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_medical);
            if (life.sub_mine_level == 0 && life.sub_mine_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_mine);
            if (life.sub_ninjitsu_level == 0 && life.sub_ninjitsu_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_ninjitsu);
            if (life.sub_reclaimed_level == 0 && life.sub_reclaimed_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_reclaimed);
            if (life.sub_tactical_level == 0 && life.sub_tactical_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_tactical);
            if (life.sub_tea_level == 0 && life.sub_tea_state == (int)SkillLearnType.NOSCHOOL) ids.Add(life.sub_tea);
            #endregion

            return ids;
        }

        /// <summary> 组装已学习技能id</summary>
        public List<int> SkillStudied(tg_role_life_skill life)
        {
            var list = new List<int>();
            #region
            if (life.sub_archer_level > 0) { list.Add(life.sub_archer); }
            if (life.sub_artillery_level > 0) { list.Add(life.sub_artillery); }
            if (life.sub_ashigaru_level > 0) { list.Add(life.sub_ashigaru); }
            if (life.sub_build_level > 0) { list.Add(life.sub_build); }
            if (life.sub_calculate_level > 0) { list.Add(life.sub_calculate); }
            if (life.sub_craft_level > 0) { list.Add(life.sub_craft); }
            if (life.sub_eloquence_level > 0) { list.Add(life.sub_eloquence); }
            if (life.sub_equestrian_level > 0) { list.Add(life.sub_equestrian); }
            if (life.sub_etiquette_level > 0) { list.Add(life.sub_etiquette); }
            if (life.sub_martial_level > 0) { list.Add(life.sub_martial); }
            if (life.sub_medical_level > 0) { list.Add(life.sub_medical); }
            if (life.sub_mine_level > 0) { list.Add(life.sub_mine); }
            if (life.sub_ninjitsu_level > 0) { list.Add(life.sub_ninjitsu); }
            if (life.sub_reclaimed_level > 0) { list.Add(life.sub_reclaimed); }
            if (life.sub_tactical_level > 0) { list.Add(life.sub_tactical); }
            if (life.sub_tea_level > 0) { list.Add(life.sub_tea); }
            return list;
            #endregion
        }

        public tg_role_life_skill StateChange(tg_role_life_skill life, int baseid)
        {
            #region
            var type = (int)SkillLearnType.TOLEARN;
            var base_life = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
            if (base_life == null) return life;
            switch (base_life.type)
            {
                case (int)LifeSkillType.ASHIGARU: life.sub_ashigaru_state = type; break;
                case (int)LifeSkillType.ARTILLERY: life.sub_artillery_state = type; break;
                case (int)LifeSkillType.ARCHER: life.sub_archer_state = type; break;
                case (int)LifeSkillType.BUILD: life.sub_build_state = type; break;
                case (int)LifeSkillType.CALCULATE: life.sub_calculate_state = type; break;
                case (int)LifeSkillType.CRAFT: life.sub_craft_state = type; break;
                case (int)LifeSkillType.ELOQUENCE: life.sub_eloquence_state = type; break;
                case (int)LifeSkillType.EQUESTRIAN: life.sub_equestrian_state = type; break;
                case (int)LifeSkillType.ETIQUETTE: life.sub_etiquette_state = type; break;
                case (int)LifeSkillType.MARTIAL: life.sub_martial_state = type; break;
                case (int)LifeSkillType.MEDICAL: life.sub_medical_state = type; break;
                case (int)LifeSkillType.MINE: life.sub_mine_state = type; break;
                case (int)LifeSkillType.NINJITSU: life.sub_ninjitsu_state = type; break;
                case (int)LifeSkillType.RECLAIMED: life.sub_reclaimed_state = type; break;
                case (int)LifeSkillType.TACTICAL: life.sub_tactical_state = type; break;
                case (int)LifeSkillType.TEA: life.sub_tea_state = type; break;
            }
            return life;
            #endregion
        }
        #endregion
    }
}
