using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_task
    {
        /// <summary>
        /// 根据用户id查询用户所有的任务
        /// </summary>
        public static List<tg_task> GetEntityById(Int64 userid = 0)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userid });
        }

        /// <summary>
        /// 根据用户id查询用户所有的任务
        /// </summary>
        public static tg_task GetEntityByStepType(Int64 userid, int steptype)
        {
            return Find(new String[] { _.user_id, _.task_step_type }, new Object[] { userid, steptype });
        }
        ///// <summary>
        ///// 根据用户id查询用户为开放的主线任务
        ///// </summary>
        //public static tg_task GetMainTask(Int64 userid)
        //{
        //    return Find(new String[] { _.user_id, _.task_type, _.task_state }, new Object[] { userid, (int)TaskType.MAIN_TASK, (int)TaskStateType.TYPE_UNOPEN });
        //}

        /// <summary>
        /// 查询用户所有的特殊职业任务
        /// </summary>
        public static List<tg_task> GetSpecialVocTask(Int64 userid)
        {
            return FindAll(new String[] { _.user_id, _.is_special, _.task_type }, new Object[] { userid, 1, (int)TaskType.VOCATION_TASK });
        }

        /// <summary> 根据用户删除职业任务</summary>
        public static void GetVocationTaskDel(Int64 userid, int isspecial)
        {
            Delete(new string[] { _.task_type, _.user_id, _.is_special }, new object[] { TaskType.VOCATION_TASK, userid, isspecial });
        }

        /// <summary> 删除所有的职业任务</summary>
        public static void GetVocationTaskDel()
        {
            Delete(new string[] { _.task_type }, new object[] { TaskType.VOCATION_TASK });
        }

        /// <summary>
        /// 查询所有待领奖的职业任务
        /// </summary>
        public static List<tg_task> GetFinishVocatinTask(Int64 userid)
        {
            return FindAll(new String[] { _.user_id, _.task_state, _.task_type },
                new Object[] { userid, TaskStateType.TYPE_REWARD, TaskType.VOCATION_TASK });
        }

        /// <summary>
        /// 查询所有用户待领奖的职业任务
        /// </summary>
        public static List<tg_task> GetFinishVocatinTask()
        {
            return FindAll(new String[] { _.task_state, _.task_type },
                new Object[] { TaskStateType.TYPE_REWARD, TaskType.VOCATION_TASK });
        }

        /// <summary>批量插入任务数据</summary>
        public static int GetListInsert(List<tg_task> tasks)
        {
            var list = new EntityList<tg_task>();
            list.AddRange(tasks);
            return list.Insert();
        }

        /// <summary>批量更新任务数据</summary>
        public static int GetListSave(IEnumerable<tg_task> tasks)
        {
            var list = new EntityList<tg_task>();
            list.AddRange(tasks);
            return list.Save();
        }

        /// <summary>根据任务类型查询玩家任务集合 </summary>
        public static List<tg_task> GetTaskQueryByType(Int64 userid, int type)
        {
            return FindAll(new string[] { _.task_type, _.user_id }, new object[] { type, userid });
        }

        /// <summary>查询玩家正在做的家臣任务 </summary>
        public static List<tg_task> GetRoleTaskAccept(Int64 userid)
        {
            return FindAll(new string[] { _.task_type, _.task_state, _.user_id }, new object[] { TaskType.ROLE_TASK, (int)TaskStateType.TYPE_UNFINISHED, userid });
        }

        /// <summary> 删除已经完成家臣任务</summary>
        public static void GetRoleTaskDel()
        {
            Delete(string.Format("task_state !={0} and task_type ={1}", (int)TaskStateType.TYPE_UNFINISHED, (int)TaskType.ROLE_TASK));
        }

        /// <summary> 根据用户id 任务基表id 查询用户指定任务</summary>
        public static bool GetEntityByUserIdTaskId(Int64 userid, int taskid)
        {
            return FindCount(string.Format("task_id>{0} and user_id={1}", taskid, userid), null, null, 0, 0) > 0;
        }

        /// <summary> 根据用户id 任务基表id 查询用户指定任务</summary>
        public static tg_task GetRoleTaskByRoleId(Int64 userid, Int64 rid)
        {
            return Find(new string[] { _.task_type, _.rid, _.user_id }, new object[] { TaskType.ROLE_TASK, rid, userid });
        }

        /// <summary> 删除已经完成家臣任务</summary>
        public static void GetTaskUpdate(int state, string step, int taskid, Int64 id, Int64 userid)
        {
            Update(string.Format("task_state={0},task_step_data='{1}',task_id={2}", state, step, taskid),
             string.Format("id ={0} and user_id ={1}", id, userid));
        }


    }
}
