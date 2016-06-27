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
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using Task = System.Threading.Tasks.Task;

namespace TGG.Module.Appraise.Service
{
    /// <summary>
    /// 开始评定
    /// 开发者：李德雁
    /// </summary>
    public class APPRAISE_START
    {
        private static APPRAISE_START _objInstance;

        private int result;
        /// <summary>APPRAISE_START 单体模式</summary>
        public static APPRAISE_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new APPRAISE_START());
        }

        /// <summary> 开始评定</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("开始评定{0}——{1}", session.Player.User.player_name, "APPRAISE_START");
#endif
            result = (int)ResultType.SUCCESS;
            var rid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "role").Value);
            var taskmain = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "task").Value);
            var roleinfo = tg_role.FindByid(rid);
            var mytask = tg_task.FindByid(taskmain);
            if (!CheckRole(roleinfo, session.Player.User.id)) return new ASObject(BuildData(null));   //验证武将

            if (!CheckIdentify(session, roleinfo)) //身份验证
                return new ASObject(BuildData(null));

            if (!CheckTask(mytask)) return new ASObject(BuildData(null)); //验证任务
            var baseinfo = Variable.BASE_APPRAISE.FirstOrDefault(q => q.id == mytask.task_id);
            if (baseinfo == null)
            {
                result = (int)ResultType.BASE_TABLE_ERROR;
                return new ASObject(BuildData(null));
            }
#if DEBUG
            XTrace.WriteLine("武将{0}开始任务前的体力{1}", roleinfo.id, roleinfo.power);
#endif
            if (!CheckPower(roleinfo, session)) return new ASObject(BuildData(null));
#if DEBUG
            XTrace.WriteLine("武将{0}开始任务后的体力{1}", roleinfo.id, roleinfo.power);
#endif
            GetTaskChange(mytask, roleinfo, baseinfo);
#if DEBUG
            XTrace.WriteLine("任务的开始时间{0}和结束时间{1}", mytask.task_starttime, mytask.task_endtime);
#endif

            return new ASObject(BuildData(mytask));
        }

        private ASObject BuildData(tg_task task)
        {
            var dic = new Dictionary<string, object>()
           {
               {"result", result},
               {"task", task == null ? null : EntityToVo.ToRoleVo(task)}
           };
            return new ASObject(dic);
        }

        /// <summary>
        /// 验证武将的数据
        /// </summary>
        private bool CheckRole(tg_role role, Int64 userid)
        {
            if (role == null)
            {
                result = (int)ResultType.APPRAISE_ROLE_LACK;
                return false;
            }
            var tasks = tg_task.GetRoleTaskAccept(userid);   //正在做的家臣任务
            var count = tasks.Where(q => q.rid == role.id).ToList().Count;
            if (count <= 0) return true;
            result = (int)ResultType.APPRAISE_ROLE_TASKING;
            return false;
        }

        /// <summary>
        /// 验证任务数据
        /// </summary>
        private bool CheckTask(tg_task task)
        {
            if (task == null)
            {
                result = (int)ResultType.APPRAISE_TASK_LACK;
                return false;
            }
            if (task.task_state == (int)TaskStateType.TYPE_UNRECEIVED) return true;
            result = (int)ResultType.APPRAISE_TASK_STATEWRONG;
            return false;
        }

        /// <summary> 验证体力</summary>
        private bool CheckPower(tg_role role, TGGSession session)
        {
            var userid = session.Player.User.id;
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower >= power)
            {
                new Share.Role().PowerUpdateAndSend(role, power, userid);
                return true;
            }
            result = (int)ResultType.BASE_ROLE_POWER_ERROR;
            return false;
        }

        /// <summary>
        /// 验证武将身份
        /// </summary>
        /// <param name="session"></param>
        /// <param name="roleinfo"></param>
        /// <returns></returns>
        private bool CheckIdentify(TGGSession session, tg_role roleinfo)
        {
            var lastidentify = Variable.BASE_IDENTITY.LastOrDefault(q => q.vocation == session.Player.User.player_vocation);
            if (lastidentify != null && lastidentify.id == session.Player.Role.Kind.role_identity) return true;
            var myidentifybase = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == session.Player.Role.Kind.role_identity);
            var roleidentifybase = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == roleinfo.role_identity);
            if (roleidentifybase != null && myidentifybase != null && myidentifybase.value > roleidentifybase.value) return true;
            result = (int)ResultType.APPRAISE_IDENTIFY_FULL;
            return false;
        }

        /// <summary> 任务实体更新</summary>
        private void GetTaskChange(tg_task task, tg_role role, BaseAppraise baseinfo)
        {
            var roleitem = view_role.GetFindRoleById(role.id);
            task.rid = role.id;
            task.task_starttime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            task.task_state = (int)TaskStateType.TYPE_UNFINISHED;
            var temp = baseinfo.expression;
            temp = temp.Replace("tea", roleitem.LifeSkill.sub_tea_level.ToString("0.00"));
            temp = temp.Replace("medical", roleitem.LifeSkill.sub_medical_level.ToString("0.00"));
            temp = temp.Replace("ninjitsu", roleitem.LifeSkill.sub_ninjitsu_level.ToString("0.00"));
            temp = temp.Replace("calculate", roleitem.LifeSkill.sub_calculate_level.ToString("0.00"));
            temp = temp.Replace("eloquence", roleitem.LifeSkill.sub_eloquence_level.ToString("0.00"));
            temp = temp.Replace("martial", roleitem.LifeSkill.sub_martial_level.ToString("0.00"));
            temp = temp.Replace("craft", roleitem.LifeSkill.sub_craft_level.ToString("0.00"));
            temp = temp.Replace("etiquette", roleitem.LifeSkill.sub_etiquette_level.ToString("0.00"));
            temp = temp.Replace("reclaimed", roleitem.LifeSkill.sub_reclaimed_level.ToString("0.00"));
            temp = temp.Replace("build", roleitem.LifeSkill.sub_build_level.ToString("0.00"));
            temp = temp.Replace("mine", roleitem.LifeSkill.sub_mine_level.ToString("0.00"));
            temp = temp.Replace("tactical", roleitem.LifeSkill.sub_tactical_level.ToString("0.00"));
            temp = temp.Replace("ashigaru", roleitem.LifeSkill.sub_ashigaru_level.ToString("0.00"));
            temp = temp.Replace("equestrian", roleitem.LifeSkill.sub_equestrian_level.ToString("0.00"));
            temp = temp.Replace("archer", roleitem.LifeSkill.sub_archer_level.ToString("0.00"));
            temp = temp.Replace("artillery", roleitem.LifeSkill.sub_artillery_level.ToString("0.00"));
            temp = temp.Replace("captain", tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role).ToString("0.00"));
            temp = temp.Replace("force", tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role).ToString("0.00"));
            temp = temp.Replace("brains", tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role).ToString("0.00"));
            temp = temp.Replace("charm", tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role).ToString("0.00"));
            temp = temp.Replace("govern", tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role).ToString("0.00"));

            var express = CommonHelper.EvalExpress(temp);
            var reducetime = Convert.ToInt32(express);
            var times = (baseinfo.time - reducetime) * 1000;
            task.task_endtime = task.task_starttime + times;
            task.Update();
            Common.GetInstance().NewTaskStart(times, task);
        }


    }
}
