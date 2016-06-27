using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Global;
using TGG.Core.Enum.Type;
using TGG.Core.Common.Util;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 任务部分类
    /// </summary>
    public partial class tg_task
    {

        #region Init Data
        /// <summary>初始化任务</summary>
        /// <param name="user_id">玩家id</param>
        public static tg_task InitTask(Int64 user_id, int identify, int vocation)
        {
            try
            {
                InitMainTask(user_id);
                InitRoleTask(user_id);
                VocationTaskInit(user_id, identify, vocation);
                return null;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// 初始主线任务数据
        /// </summary>
        /// <param name="user_id"></param>
        private static void InitMainTask(Int64 user_id)
        {
            var basedata = Variable.BASE_TASKMAIN.FirstOrDefault();
            if (basedata == null) return;
            var task = BuildMainTask(basedata, user_id);
            task.Save();
        }

        /// <summary>
        /// 初始家臣任务数据
        /// </summary>
        /// <param name="user_id"></param>
        private static void InitRoleTask(Int64 user_id)
        {
            var listtaks = new List<tg_task>();
            var list = Variable.BASE_APPRAISE.ToList();
            var basedata = Variable.BASE_RULE.FirstOrDefault(q => q.id == "21001");
            if (basedata == null) return;
            var basecount = Convert.ToInt32(basedata.value); //任务数量
            if (list.Count < basecount) return;
            var indexlist = RNG.Next(0, list.Count - 1, basecount);
            listtaks.AddRange(indexlist.Select(item => BuildNewRoleTask(list[item].id, user_id)));
            GetListInsert(listtaks);
        }

        /// <summary>
        /// 初始化职业任务
        /// </summary>
        private static void VocationTaskInit(Int64 userid, int identify, int vocation)
        {
            var newtasks = TaskCommon.GetNewVocationTasks(identify, userid, vocation);
            tg_task.GetListInsert(newtasks);
        }

        #endregion

        #region  初始任务实体数据
        private static tg_task BuildNewRoleTask(int baseid, Int64 userid)
        {
            return new tg_task()
            {
                task_id = baseid,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_type = (int)TaskType.ROLE_TASK,
                user_id = userid,
            };
        }

        private static tg_task BuildMainTask(BaseTaskMain basedata, Int64 user_id)
        {
            return new tg_task()
            {
                task_id = basedata.id,
                user_id = user_id,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_step_data = CommonHelper.ToNewTask(basedata.stepCondition),
                task_type = (int)TaskType.MAIN_TASK,
            };
        }
        #endregion

        #region 逻辑方法
        /// <summary>未完成的跑商买卖任务</summary>
        /// <param name="userid">玩家ID</param>
        public static tg_task GetUnBusinessTask(Int64 userid)
        {
            return Find(new String[] { _.user_id, _.task_type, _.task_state, _.task_step_type },
                new Object[] { userid, (int)TaskType.VOCATION_TASK, (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStepType.BUSINESS });
        }

        /// <summary>未完成的跑商买卖任务</summary>
        /// <param name="userid">玩家ID</param>
        public static tg_task GetWorkUnBusinessTask(Int64 userid)
        {
            return Find(new String[] { _.user_id, _.task_type, _.task_state, _.task_step_type },
                new Object[] { userid, (int)TaskType.WORK_TASK, (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStepType.BUSINESS });
        }

        /// <summary>是否有未完成跑商买卖任务</summary>
        /// <param name="userid">玩家ID</param>
        public static bool IsUnBusinessTask(Int64 userid)
        {
            return FindCount(new String[] { _.user_id, _.task_type, _.task_state, _.task_step_type },
                new Object[] { userid, (int)TaskType.VOCATION_TASK, (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStepType.BUSINESS }) > 0;
        }

        /// <summary>未完成的跑商筹措任务</summary>
        /// <param name="userid">玩家ID</param>
        public static tg_task GetUnRaiseTask(Int64 userid)
        {
            return Find(new String[] { _.user_id, _.task_type, _.task_state, _.task_step_type },
                new Object[] { userid, (int)TaskType.VOCATION_TASK, (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStepType.RAISE_COIN });
        }

        /// <summary>工作未完成的跑商筹措任务</summary>
        /// <param name="userid">玩家ID</param>
        public static tg_task GetWorkUnRaiseTask(Int64 userid)
        {
            return Find(new String[] { _.user_id, _.task_type, _.task_state, _.task_step_type },
                new Object[] { userid, (int)TaskType.WORK_TASK, (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStepType.RAISE_COIN });
        }


        /// <summary>线程恢复筹措任务</summary>
        /// <param name="userid">玩家ID</param>
        public static List<tg_task> GetThreadRaiseTask()
        {
            Int64 timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var state = new List<int> { (int)TaskStateType.TYPE_UNFINISHED, (int)TaskStateType.TYPE_REWARD };
            var exp = new WhereExpression();
            //if (userid > 0) exp &= _.user_id == userid;
            exp &= _.task_type == (int)TaskType.VOCATION_TASK;
            exp &= _.task_step_type == (int)TaskStepType.RAISE_COIN;
            exp &= _.task_endtime > timeStamp;
            exp &= _.task_state.In(state);

            return FindAll(exp, null, null, 0, 0);

        }

        #endregion

    }
}
