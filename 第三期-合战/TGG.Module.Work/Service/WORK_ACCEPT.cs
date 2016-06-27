using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    /// <summary>
    /// 工作任务接受
    /// </summary>
    public class WORK_ACCEPT : IDisposable
    {
        //public static WORK_ACCEPT ObjInstance;

        ///// <summary> WORK_ACCEPT单体模式 </summary>
        //public static WORK_ACCEPT GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new WORK_ACCEPT());
        //}
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WORK_ACCEPT()
        {
            Dispose();
        }

        #endregion
        public ASObject CommandStart(TGGSession session, ASObject data)
        {

#if DEBUG
            XTrace.WriteLine("{0}:{1}工作任务领取", "WORK_ACCEPT", session.Player.User.player_name);
#endif
            if (!data.ContainsKey("id")) return null;
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var userid = session.Player.User.id;
            var mytask = tg_task.GetEntityByIdAndUserId(id, userid);
            if (tg_task.GetEntityByUserIdAndType(userid, (int)TaskStateType.TYPE_UNRECEIVED, (int)TaskType.WORK_TASK)) return null;
            var now = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (mytask == null || mytask.task_state != (int)TaskStateType.TYPE_UNRECEIVED || mytask.task_type != (int)TaskType.WORK_TASK)
                return BuildData((int)ResultType.TASK_VOCATION_NOTASK, 0, null);
            var usertask = tg_task.GetTaskQueryByType(userid, (int)TaskType.WORK_TASK);
            if (usertask.Any(q => q.task_state == (int)TaskStateType.TYPE_REWARD || q.task_state == (int)TaskStateType.TYPE_UNFINISHED))
            {
                return BuildData((int)ResultType.WORK_IS_STARTED, 0, null);
            }
            var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == mytask.task_id);
            if (baseinfo == null) return BuildData((int)ResultType.TASK_VOCATION_NOTASK, 0, null);
            //技能验证
            if (!CheckSkill(baseinfo, session.Player.Role)) return BuildData((int)ResultType.TASK_SKILL_LACK, 0, null);
            //验证体力
            if (!PowerOperate(session.Player.Role.Kind, userid)) return BuildData((int)ResultType.BASE_ROLE_POWER_ERROR, 0, null);
            //验证冷却时间
            if (!CheckTime(mytask.task_coolingtime)) return BuildData((int)ResultType.WORK_TIME_WRONG, 0, null);
            mytask.task_starttime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            mytask.task_coolingtime = now + baseinfo.coolingTime * 1000;
            mytask.task_endtime = now + baseinfo.limitTime * 1000;

            TaskThreadStart(mytask);//接受任务启动线程

            Common.GetInstance().LimitTimeThreading(baseinfo.limitTime, mytask);
            mytask.task_state = (int)TaskStateType.TYPE_UNFINISHED;
            mytask.Update();
            return BuildData((int)ResultType.SUCCESS, mytask);
        }

        /// <summary>启动任务线程</summary>
        /// <param name="task">任务实体</param>
        private void TaskThreadStart(tg_task task)
        {
            switch (task.task_step_type)
            {
                case (int)TaskStepType.RAISE_COIN:
                    {

                        Common.GetInstance().RaiseTaskThread(task);
                        (new Share.Work()).AdvancedWorkPush(task.user_id, task);
                        break;
                    }
            }
        }


        /// <summary>体力操作</summary>
        private bool PowerOperate(tg_role role, Int64 userid)
        {
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            var r = role.CloneEntity();
            new Share.Role().PowerUpdateAndSend(role, power, userid);
            (new Share.Role()).LogInsert(r, power, ModuleNumber.WORK, (int)WorkCommand.WORK_ACCEPT, "工作", "接受工作  ");
            return true;
        }

        /// <summary>组装数据 </summary>
        private ASObject BuildData(int result, Int64 time, tg_task task)
        {
            var vo = task == null ? null : EntityToVo.ToVocationTaskVo(task);
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
               
                {"workVo",vo}
            };
            return new ASObject(dic);
        }

        private ASObject BuildData(int result, tg_task task)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"workVo",task==null?null:EntityToVo.ToVocationTaskVo(task)}
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 技能验证
        /// </summary>
        /// <param name="basetask"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool CheckSkill(BaseTaskVocation basetask, RoleItem role)
        {
            try
            {
                if (basetask.stepCondition == "") return true;
                var sc = basetask.skillCondition.Split('_');
                var type = Convert.ToInt32(sc[0]); //0：技能 1属性
                var type1 = Convert.ToInt32(sc[1]); //技能或者属性类型
                var value = Convert.ToInt32(sc[2]); //技能或者属性值
                if (type == 0)
                {
                    #region 技能
                    switch (type1)
                    {
                        case (int)LifeSkillType.ARTILLERY:
                            return role.LifeSkill.sub_artillery_level >= value;
                        case (int)LifeSkillType.ARCHER:
                            return role.LifeSkill.sub_archer_level >= value;
                        case (int)LifeSkillType.ASHIGARU:
                            return role.LifeSkill.sub_ashigaru_level >= value;
                        case (int)LifeSkillType.BUILD:
                            return role.LifeSkill.sub_build_level >= value;
                        case (int)LifeSkillType.CALCULATE:
                            return role.LifeSkill.sub_calculate_level >= value;
                        case (int)LifeSkillType.CRAFT:
                            return role.LifeSkill.sub_craft_level >= value;
                        case (int)LifeSkillType.ELOQUENCE:
                            return role.LifeSkill.sub_eloquence_level >= value;
                        case (int)LifeSkillType.EQUESTRIAN:
                            return role.LifeSkill.sub_equestrian_level >= value;
                        case (int)LifeSkillType.ETIQUETTE:
                            return role.LifeSkill.sub_etiquette_level >= value;
                        case (int)LifeSkillType.MARTIAL:
                            return role.LifeSkill.sub_martial_level >= value;
                        case (int)LifeSkillType.MEDICAL:
                            return role.LifeSkill.sub_medical_level >= value;
                        case (int)LifeSkillType.MINE:
                            return role.LifeSkill.sub_mine_level >= value;
                        case (int)LifeSkillType.NINJITSU:
                            return role.LifeSkill.sub_ninjitsu_level >= value;
                        case (int)LifeSkillType.RECLAIMED:
                            return role.LifeSkill.sub_reclaimed_level >= value;
                        case (int)LifeSkillType.TACTICAL:
                            return role.LifeSkill.sub_tactical_level >= value;
                        case (int)LifeSkillType.TEA:
                            return role.LifeSkill.sub_tea_level >= value;
                    }
                    #endregion
                }
                if (type != 1) return true;

                #region 属性
                switch (type1)
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role.Kind) >= value;
                    case (int)RoleAttributeType.ROLE_FORCE: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role.Kind) >= value;
                    case (int)RoleAttributeType.ROLE_BRAINS: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role.Kind) >= value;
                    case (int)RoleAttributeType.ROLE_CHARM: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role.Kind) >= value;
                    case (int)RoleAttributeType.ROLE_GOVERN: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role.Kind) >= value;
                }
                #endregion

                return true;

            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return false;
            }
        }

        /// <summary>
        /// 工作冷却时间
        /// </summary>
        /// <param name="newtime"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool CheckTime(Int64 newtime)
        {
            //var workinfo = Common.GetInstance().GetWorkInfo(userid);
            var now = ((DateTime.Now.Ticks - 621355968000000000) / 10000);

            // workinfo.CoolingTime = now + newtime * 1000;
            return now > newtime;
        }



    }
}

