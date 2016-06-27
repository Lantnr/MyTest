using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 职业任务接受
    /// </summary>
    public class TASK_VOCATION_ACCEPT : IDisposable
    {

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~TASK_VOCATION_ACCEPT()
        {
            Dispose();
        }
    
        #endregion
        //public static TASK_VOCATION_ACCEPT objInstance = null;

        ///// <summary> TASK_VOCATION_ACCEPT单体模式 </summary>
        //public static TASK_VOCATION_ACCEPT getInstance()
        //{
        //    return objInstance ?? (objInstance = new TASK_VOCATION_ACCEPT());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {

#if DEBUG
            XTrace.WriteLine("{0}:{1}职业评定领取", "TASK_VOCATION_ACCEPT", session.Player.User.player_name);
#endif
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "task").Value);
            var userid = session.Player.User.id;
            var mytask = tg_task.FindByid(id);
            if (mytask == null || mytask.task_state != (int)TaskStateType.TYPE_UNRECEIVED || mytask.task_type != (int)TaskType.VOCATION_TASK)
                return BuildData((int)ResultType.TASK_VOCATION_NOTASK);
            var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == mytask.task_id);
            if (baseinfo == null) return BuildData((int)ResultType.TASK_VOCATION_NOTASK); //体力判断

            //技能验证
            if (!CheckSkill(baseinfo, session.Player.Role)) return BuildData((int)ResultType.BASE_ROLE_SKILL_LEVEL_ERROR);
            if (!PowerOperate(session.Player.Role.Kind, userid)) return BuildData((int)ResultType.BASE_ROLE_POWER_ERROR);
            mytask.task_state = (int)TaskStateType.TYPE_UNFINISHED;

            if (mytask.task_step_type == (int)TaskStepType.RAISE_COIN)
            {
                var time = 0;
                var t = Variable.BASE_TASKVOCATION.FirstOrDefault(m => m.id == mytask.task_id);
                if (t == null) time = 0;
                else time = t.time * 1000;

                Int64 timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                var starttime = timeStamp;
                var stoptime = timeStamp + time;
                mytask.task_starttime = starttime;
                mytask.task_endtime = stoptime;

            }

            mytask.Update();

            //接受任务启动线程
            TaskThreadStart(mytask);

            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary>启动任务线程</summary>
        /// <param name="task">任务实体</param>
        private void TaskThreadStart(tg_task task)
        {
            switch (task.task_step_type)
            {
                case (int)TaskStepType.RAISE_COIN:
                    {
                        Common.getInstance().RaiseTaskThread(task);
                        (new Share.TGTask()).AdvancedTaskPush(task.user_id, task);
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
            (new Share.Role()).LogInsert(r, power, ModuleNumber.TASK, (int)TaskCommand.TASK_VOCATION_ACCEPT, "任务", "接受任务");
            return true;
        }

        /// <summary>组装数据 </summary>
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result}
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
                if (basetask.skillCondition == "") return true;
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


    }
}
