using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Threading;
using FluorineFx;
using FluorineFx.Context;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Role;
using TGG.Core.Vo.Task;
using TGG.Share;
using TGG.SocketServer;

using System.Transactions;


namespace TGG.Module.Task.Service
{
    public partial class Common
    {

        #region 组装数据
        public Dictionary<String, Object> BuildData(object result, List<tg_task> list_task, int count, int hasReset)
        {
            var dic = new Dictionary<string, object>();
            var maintask = list_task.FirstOrDefault(q => q.task_type == (int)TaskType.MAIN_TASK
                && q.task_state != (int)TaskStateType.TYPE_FINISHED);
            var dailytask = list_task.Where(q => q.task_type == (int)TaskType.VOCATION_TASK).ToList();
            dic.Add("result", result);
            dic.Add("mainTask", maintask != null ? EntityToVo.ToTaskVo(maintask) : null);
            dic.Add("vocationTask", ConvertListASObject(dailytask, "VocationTask"));
            dic.Add("count", count);
            dic.Add("hasReset", hasReset);
            return dic;
        }

        public ASObject BuildVoctionData(object result, List<tg_task> list_task, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"vocationTask",list_task!=null? ConvertListASObject(list_task, "VocationTask"):null},
                {"count", count}
            };
            return new ASObject(dic);
        }

        /// <summary> 将dynamic对象转换成ASObject对象 </summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "VocationTask": model = EntityToVo.ToVocationTaskVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }
        #endregion

        #region 公共方法




        /// <summary>
        /// 获取高级评定信息，如果没有该玩家数据，默认插入一条数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Variable.UserTaskInfo GetWorkInfo(Int64 userid)
        {
            var info = Variable.TaskInfo.FirstOrDefault(q => q.userid == userid);
            if (info != null) return info;
            var newone = new Variable.UserTaskInfo(userid);
            Variable.TaskInfo.Add(newone);
            return newone;
        }


        /// <summary> 组装新的任务 </summary>
        public tg_task BuildNewVocationTask(string step, int taskid, long userid, int identify, int stepType)
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

        /// <summary> 根据用户身份返回用户的新的职业任务 </summary>
        public List<tg_task> GetNewVocationTasks(int identity, long userid, int vocation)
        {
            try
            {
                var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == vocation);
                if (baseidentify == null) return null;
                var mytask = new List<tg_task>();
                var baseTaskVocation = Variable.BASE_TASKVOCATION.LastOrDefault(q => q.vocation == vocation);
                if (baseTaskVocation != null && identity == baseTaskVocation.id) return mytask;//大名没有职业任务
                if (identity == baseidentify.id) //本职业第一个身份,3个职业任务
                {
                    var list = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString()))
                        .Where(q => q.type == 1).ToList();
                    if (!list.Any()) return mytask;
                    var leftcount = list.Count >= 3 ? 3 : list.Count;
                    var indexlist = RNG.Next(0, list.Count - 1, leftcount);
                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity, list[item].stepType)));
                }
                else
                {
                    var list = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains((identity - 1).ToString()))
                       .Where(q => q.type == 1).ToList();
                    if (!list.Any()) return mytask;
                    var leftcount = list.Count >= 3 ? 3 : list.Count;
                    var indexlist = RNG.Next(0, list.Count - 1, leftcount);//随机三个任务

                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity - 1, list[item].stepType)));
                    var taskids = mytask.Select(q => q.task_id).ToList();
                    var list1 = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString()))
                        .Where(q => q.type == 1 && !taskids.Contains(q.id)).ToList();
                    leftcount = list1.Count >= 3 ? 3 : list1.Count;
                    indexlist = RNG.Next(0, list1.Count - 1, leftcount);
                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list1[item].stepInit, list1[item].id, userid, identity, list1[item].stepType)));
                }
                return mytask;
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        /// <summary>
        /// 主线任务步骤插入
        /// </summary>
        /// <param name="value">步骤字符串 例如1_200001_1|3_9010001_1</param>
        /// <returns>处理后的字符串1_200001_0|3_9010001_0</returns>
        public string GetInsertValue(string value)
        {
            //1.把每个步骤切割开，存在arr数组中
            string newvalue = "";
            var arr = SplitTaskToList(value);
            //对每个任务步骤处理
            foreach (var item in arr)
            {
                var type = item.Split('_').ToList();
                var steptype = Convert.ToInt32(type[0]);
                if (steptype == (int)TaskStepType.NPC_FIGHT_TIMES)//有次数要求的npc
                    type[3] = "0";
                if (steptype == (int)TaskStepType.TYPE_DIALOG || steptype == (int)TaskStepType.TYPE_BUSINESS)//跑商
                    type[2] = "0";
                if (steptype == (int)TaskStepType.STUDY_SKILL || steptype == (int)TaskStepType.TRAIN)//有次数要求的npc
                    type[1] = "0";
                newvalue += string.Join("_", type);
                newvalue += "|";//任务完成数归零
            }
            newvalue = newvalue.Remove(newvalue.Length - 1);
            return newvalue;
        }

        /// <summary> 任务步骤切割 </summary>
        public List<string> SplitTaskToList(string data)
        {
            var mylist = new List<string>();
            if (data.Contains('|'))
            {
                var steplist = data.Split('|');
                mylist.AddRange(steplist);
            }
            else
                mylist.Add(data);
            return mylist;
        }


        /// <summary> 任务数据更新 </summary>
        public void TaskUpdate(tg_task task, string newstep, string basestep)
        {
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id)) return;
            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
            if (session == null) return;
            task.task_step_data = newstep;
            if (newstep == basestep)
                task.task_state = (int)TaskStateType.TYPE_REWARD;
            tg_task.GetTaskUpdate(task.task_state, task.task_step_data, task.task_id, task.id, task.user_id);
            if (task.task_type == (int)TaskType.MAIN_TASK)
                session.MainTask = task;
        }


        public void SendPv(TGGSession session, ASObject aso, int commandnumber)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.TASK, commandnumber);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }


        /// <summary> 获取奖励字符串 </summary>
        public string GetRewardString(BaseTaskVocation baseinfo, TGGSession session)
        {
            if (CheckRewardCondition(baseinfo.rewardMaxCondition, session.Player.Role.CloneEntity()))
                return baseinfo.rewardMax;
            return CheckRewardCondition(baseinfo.rewardMediumCondition, session.Player.Role)
                ? baseinfo.rewardMedium : baseinfo.reward;
        }

        /// <summary> 验证奖励标准 </summary>
        public bool CheckRewardCondition(string rewardcondition, RoleItem roleitem)
        {
            var list = rewardcondition.Split('|').ToList();
            foreach (var item in list)
            {
                var type = Convert.ToInt32(item.Split('_')[0]);
                var type1 = Convert.ToInt32(item.Split('_')[1]);
                var value = Convert.ToInt32(item.Split('_')[2]);
                if (type == 0)
                {
                    switch (type1)
                    {
                        #region 技能验证

                        case (int)LifeSkillType.ARTILLERY: //铁炮
                            {
                                if (roleitem.LifeSkill.sub_artillery_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ARCHER: //弓术
                            {
                                if (roleitem.LifeSkill.sub_archer_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ASHIGARU: //足轻
                            {
                                if (roleitem.LifeSkill.sub_ashigaru_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.BUILD: //建筑
                            {
                                if (roleitem.LifeSkill.sub_build_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.CALCULATE: //算术
                            {
                                if (roleitem.LifeSkill.sub_calculate_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.CRAFT: //艺术
                            {
                                if (roleitem.LifeSkill.sub_craft_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ELOQUENCE: //辩才
                            {
                                if (roleitem.LifeSkill.sub_eloquence_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.EQUESTRIAN: //马术
                            {
                                if (roleitem.LifeSkill.sub_equestrian_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ETIQUETTE: //礼法
                            {
                                if (roleitem.LifeSkill.sub_etiquette_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MARTIAL: //武艺
                            {
                                if (roleitem.LifeSkill.sub_martial_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MEDICAL: //医术
                            {
                                if (roleitem.LifeSkill.sub_medical_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MINE: //矿山
                            {
                                if (roleitem.LifeSkill.sub_mine_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.NINJITSU: //忍术
                            {
                                if (roleitem.LifeSkill.sub_ninjitsu_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.RECLAIMED: //开垦
                            {
                                if (roleitem.LifeSkill.sub_reclaimed_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.TACTICAL: //军学
                            {
                                if (roleitem.LifeSkill.sub_tactical_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.TEA: //茶道
                            {
                                if (roleitem.LifeSkill.sub_tea_level < value)
                                    return false;
                            }
                            break;
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

        #endregion

        /// <summary>
        ///高级评定任务初始
        /// </summary>
        /// <param name="taksinfo"></param>
        /// <returns></returns>
        public IEnumerable<tg_task> SpecialTasksInit(view_user_role_task taksinfo)
        {
            var entitytasks = new List<tg_task>();
            var userid = taksinfo.user_id;
            var identify = taksinfo.role_identity;
            var basetask = Variable.BASE_TASKVOCATION.Where(q => q.type == 2 && q.vocation == taksinfo.player_vocation && Convert.ToInt32(q.identity) <= taksinfo.role_identity).ToList();
            if (!basetask.Any()) return null;
            var clonelist = new List<BaseTaskVocation>();
            basetask.ForEach(q => clonelist.Add(q.CloneEntity()));
            lock (clonelist)
            {
                foreach (var item in clonelist)
                {
                    var model = item.CloneEntity();

                    var step = model.stepInit;
                    var steptype = model.stepType;
                    var taskid = model.id;
                    if (model.stepType == (int)TaskStepType.FIGHTING_CONTINUOUS ||
                        model.stepType == (int)TaskStepType.SEARCH_GOODS
                        || model.stepType == (int)TaskStepType.ESCORT || model.stepType == (int)TaskStepType.RUMOR
                        || model.stepType == (int)TaskStepType.FIRE || model.stepType == (int)TaskStepType.BREAK
                        || model.stepType == (int)TaskStepType.SEll_WINE ||
                        model.stepType == (int)TaskStepType.ASSASSINATION
                        || model.stepType == (int)TaskStepType.GUARD ||
                        model.stepType == (int)TaskStepType.ARREST_RUMOR
                        || model.stepType == (int)TaskStepType.ARREST_FIRE ||
                        model.stepType == (int)TaskStepType.ARREST_BREAK
                        || model.stepType == (int)TaskStepType.ARREST_SEll_WINE ||
                        model.stepType == (int)TaskStepType.STAND_GUARD)
                    {
                        var steptypelist = entitytasks.Select(q => q.task_step_type).ToList();
                        var randomtask = new Upgrade().RandomTask(steptypelist, clonelist, model.stepType);
                        if (randomtask == null) continue;
                        step = randomtask.stepInit;
                        steptype = randomtask.stepType;
                        taskid = randomtask.id;
                        entitytasks.Add(new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify,
                            (int)TaskType.VOCATION_TASK));
                        continue;
                    }
                    entitytasks.Add(new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify, (int)TaskType.VOCATION_TASK));
                }
                clonelist.Clear();
            }
            return entitytasks;
        }




    }
}
