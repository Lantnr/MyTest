using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Core.Common
{
    public class TaskCommon
    {
        /// <summary> 验证奖励标准 </summary>
        public static bool CheckRewardCondition(string rewardcondition, RoleItem roleitem)
        {
            var list = rewardcondition.Split('|').ToList();
            foreach (var item in list)
            {
                var type = Convert.ToInt32(item.Split('_')[0]); //0:技能 1：属性
                var type1 = Convert.ToInt32(item.Split('_')[1]);
                var value = Convert.ToInt32(item.Split('_')[2]);  
                if (type == 0)
                {
                    switch (type1)
                    {
                        #region 技能验证
                        //铁炮
                        case (int)LifeSkillType.ARTILLERY: { if (roleitem.LifeSkill.sub_artillery_level < value) return false; } break;
                        //弓术
                        case (int)LifeSkillType.ARCHER: { if (roleitem.LifeSkill.sub_archer_level < value)return false; } break;
                        //足轻
                        case (int)LifeSkillType.ASHIGARU: { if (roleitem.LifeSkill.sub_ashigaru_level < value) return false; } break;
                        //建筑
                        case (int)LifeSkillType.BUILD: { if (roleitem.LifeSkill.sub_build_level < value) return false; } break;
                        //算术
                        case (int)LifeSkillType.CALCULATE: { if (roleitem.LifeSkill.sub_calculate_level < value) return false; } break;
                        //艺术
                        case (int)LifeSkillType.CRAFT: { if (roleitem.LifeSkill.sub_craft_level < value) return false; } break;
                        //辩才
                        case (int)LifeSkillType.ELOQUENCE: { if (roleitem.LifeSkill.sub_eloquence_level < value)return false; } break;
                        //马术
                        case (int)LifeSkillType.EQUESTRIAN: { if (roleitem.LifeSkill.sub_equestrian_level < value) return false; } break;
                        //礼法
                        case (int)LifeSkillType.ETIQUETTE: { if (roleitem.LifeSkill.sub_etiquette_level < value) return false; } break;
                        //武艺
                        case (int)LifeSkillType.MARTIAL: { if (roleitem.LifeSkill.sub_martial_level < value)return false; } break;
                        //医术
                        case (int)LifeSkillType.MEDICAL: { if (roleitem.LifeSkill.sub_medical_level < value)  return false; } break;
                        //矿山
                        case (int)LifeSkillType.MINE: { if (roleitem.LifeSkill.sub_mine_level < value) return false; } break;
                        //忍术
                        case (int)LifeSkillType.NINJITSU: { if (roleitem.LifeSkill.sub_ninjitsu_level < value) return false; } break;
                        //开垦
                        case (int)LifeSkillType.RECLAIMED: { if (roleitem.LifeSkill.sub_reclaimed_level < value) return false; } break;
                        //军学
                        case (int)LifeSkillType.TACTICAL: { if (roleitem.LifeSkill.sub_tactical_level < value) return false; } break;
                        //茶道
                        case (int)LifeSkillType.TEA: { if (roleitem.LifeSkill.sub_tea_level < value)return false; } break;
                        #endregion
                    }
                }
                if (type != 1) continue;
                #region 属性验证
                switch (type1)
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN: //统帅
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, roleitem.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_FORCE: //武力
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, roleitem.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_BRAINS: //智谋
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, roleitem.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_CHARM: //魅力
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, roleitem.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_GOVERN: //政务
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, roleitem.Kind) < value)
                                return false;
                        }
                        break;
                }
                #endregion

            }
            return true;

        }

        /// <summary> 根据用户身份返回用户的新的职业任务 </summary>
        public static List<tg_task> GetNewVocationTasks(int identity, long userid, int vocation)
        {
            var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == vocation);
            if (baseidentify == null) return null;
            var mytask = new List<tg_task>();
            var basetask = Variable.BASE_TASKVOCATION.Where(q => q.type == 1).ToList();
            if (!basetask.Any()) return null;
            var baseTaskVocation = Variable.BASE_TASKVOCATION.LastOrDefault(q => q.vocation == vocation);
            if (baseTaskVocation != null && identity == baseTaskVocation.id) return mytask;//大名没有职业任务
            if (identity == baseidentify.id) //本职业第一个身份,3个职业任务
            {
                var list = basetask.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString())).ToList();
                if (!list.Any()) return mytask;
                var leftcount = list.Count >= 3 ? 3 : basetask.Count;
                var indexlist = RNG.Next(0, list.Count - 1, leftcount);
                mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity, list[item].stepType)));
            }
            else
            {
                var list = basetask.FindAll(q => q.identity.Split(',').ToList().Contains((identity - 1).ToString())).ToList();
                if (!list.Any()) return mytask;
                var leftcount = list.Count >= 3 ? 3 : list.Count;
                var indexlist = RNG.Next(0, list.Count - 1, leftcount);//随机三个任务
                var oldtask = indexlist.Select(item => basetask[item]).ToList();
                mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity - 1, list[item].stepType)));
                var list1 = basetask.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString())).ToList();
                list1 = list1.SkipWhile(oldtask.Contains).ToList();  //基表中跳过已经接的任务
                var leftcount1 = list1.Count >= 3 ? 3 : list1.Count;
                var indexlist1 = RNG.Next(0, list1.Count - 1, leftcount1);
                mytask.AddRange(indexlist1.Select(item => BuildNewVocationTask(list1[item].stepInit, list1[item].id, userid, identity, list1[item].stepType)));
            }
            return mytask;


        }

        /// <summary> 组装新的任务 </summary>
        public static tg_task BuildNewVocationTask(string step, int taskid, long userid, int identify, int stepType)
        {
            var newtask = new tg_task
            {
                task_type = (int)TaskType.VOCATION_TASK,
                task_id = taskid,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_step_data = step,
                task_base_identify = identify,
                user_id = userid,
                task_step_type = stepType,
            };
            return newtask;
        }


    }
}
