using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.Share.Event;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    ///     任务推送
    /// </summary>
    public class TASK_PUSH
    {
        public static TASK_PUSH objInstance;

        /// <summary>
        ///     TASK_PUSH单体模式
        /// </summary>
        /// <returns></returns>
        public static TASK_PUSH getInstance()
        {
            if (objInstance == null) objInstance = new TASK_PUSH();
            return objInstance;
        }

        /// <summary> 日常任务每日刷新 </summary>
        public void VocationTaskUpdate()
        {
#if DEBUG
            var stopwatch = new Stopwatch(); //creates and start the instance of Stopwatch
            stopwatch.Start();
#endif
            var newtasks = new List<tg_task>();
            GetReward(); //已经完成的评定领取奖励
            tg_task.GetRoleTaskDel(); //删除除了还在做的所有的家臣任务
            var userroles = view_user_role_task.GetAll();
            Variable.TaskInfo.Clear();
            foreach (var usernewtasks in userroles)
            {
                var basetasks = Common.getInstance().
                    GetNewVocationTasks(usernewtasks.role_identity, usernewtasks.user_id, usernewtasks.player_vocation)
                    .ToList();
                newtasks.AddRange(basetasks);
                var s_tasks = Common.getInstance().SpecialTasksInit(usernewtasks);
                if (s_tasks != null)
                    newtasks.AddRange(s_tasks);
                newtasks.AddRange(new TGTask().RandomTask(usernewtasks.user_id, (int)TaskType.ROLE_TASK));
                newtasks.AddRange(new TGTask().RandomTask(usernewtasks.user_id, (int)TaskType.ROLE_EXPERIENCE));
            }

            GetListInsert(newtasks);
            newtasks.Clear();

            #region 在线则推送数据

            foreach (var item in Variable.OnlinePlayer.Keys)
            {
                var item1 = item;
                var usertask = tg_task.GetEntityById(item1);
                if (!userroles.Any()) return;
                var dic = new Dictionary<string, object>
                {
                    {
                        "1",
                        Common.getInstance()
                            .ConvertListASObject(
                                usertask.Where(q => q.user_id == item1 && q.task_type == (int) TaskType.VOCATION_TASK),
                                "VocationTask")
                    }
                };
                var aso = new ASObject(dic);

                var session = Variable.OnlinePlayer[item1] as TGGSession;
                //推送评定数据
                if (session == null) continue;
                var pv = session.InitProtocol((int)ModuleNumber.TASK,
                    (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
                dic = new Dictionary<string, object>
                {
                    {
                        "task",
                        ConvertListAsObject(
                            usertask.Where(
                                q => q.user_id == item1 && q.task_state == (int) TaskStateType.TYPE_UNRECEIVED
                                     && q.task_type == (int) TaskType.ROLE_TASK||q.task_type==(int)TaskType.ROLE_EXPERIENCE))
                    },
                    {"count", 2}
                };
                aso = new ASObject(dic);
                //推送数据家臣评定
                pv = session.InitProtocol((int)ModuleNumber.APPRAISE,
                    (int)AppraiseCommand.PUSH_TASK_REFRESH, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
            }

            #endregion

#if DEBUG
            var timespan = stopwatch.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());

#endif
        }

        /// <summary>
        ///批量插入数据
        /// </summary>
        /// <param name="newtasks"></param>
        private void GetListInsert(List<tg_task> newtasks)
        {
            // tg_task.GetListInsert(newtasks);
            //var batchSize = 400;
            //var count = newtasks.Count;
            //var list = new List<tg_task>();
            //for (var i = 0; i < count; i += batchSize)
            //{
            //    for (var j = i; j < i + batchSize && j < count; j++)
            //    {
            //        list.Add(newtasks[j]);
            //    }
            //    SqlBulkCopyInsert(list);
            //    list.Clear();
            //}

            SqlBulkCopyInsert(newtasks);
        }

        /// <summary>
        ///     使用SqlBulkCopy方式插入数据
        /// </summary>
        /// <returns></returns>
        private static void SqlBulkCopyInsert<T>(IList<T> list)
        {
            var ltt = new ListToDataTable();
            var dataTable = ltt.ConvertTo(list);
            ltt.Dispose();
            var sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            sqlcon.Open();
            var sqlBulkCopy = new SqlBulkCopy(sqlcon);
            sqlBulkCopy.DestinationTableName = "tg_task";

            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.ColumnMappings.Add("task_id", "task_id");
                sqlBulkCopy.ColumnMappings.Add("id", "id");
                sqlBulkCopy.ColumnMappings.Add("user_id", "user_id");
                sqlBulkCopy.ColumnMappings.Add("task_type", "task_type");
                sqlBulkCopy.ColumnMappings.Add("task_state", "task_state");
                sqlBulkCopy.ColumnMappings.Add("task_step_type", "task_step_type");
                sqlBulkCopy.ColumnMappings.Add("task_step_data", "task_step_data");
                sqlBulkCopy.ColumnMappings.Add("task_starttime", "task_starttime");
                sqlBulkCopy.ColumnMappings.Add("task_endtime", "task_endtime");
                sqlBulkCopy.ColumnMappings.Add("rid", "rid");
                sqlBulkCopy.ColumnMappings.Add("task_base_identify", "task_base_identify");
                sqlBulkCopy.ColumnMappings.Add("is_lock", "is_lock");
                sqlBulkCopy.ColumnMappings.Add("is_special", "is_special");
                sqlBulkCopy.ColumnMappings.Add("task_coolingtime", "task_coolingtime");
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
            sqlcon.Close();
            sqlcon.Dispose();
        }



        /// <summary> 已经完成的任务领取奖励 </summary>
        private void GetReward()
        {
            try
            {
                var finishtasks = tg_task.GetFinishVocatinTask();
                foreach (var finishtask in finishtasks)
                {
                    var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == finishtask.task_id);
                    var userid = finishtask.user_id;
                    if (Variable.OnlinePlayer.ContainsKey(userid))
                    {
                        var session = Variable.OnlinePlayer[userid] as TGGSession;
                        if (baseinfo == null) continue;
                        var rewardstring = Common.getInstance().GetRewardString(baseinfo, session);
                        new Reward().GetReward(rewardstring, userid, (int)ModuleNumber.TASK,
                            (int)TaskCommand.TASK_PUSH);
                    }
                    else //玩家不在线
                    {
                        if (baseinfo != null)
                            new Reward().GetRewardNotOnline(baseinfo.reward, finishtask.user_id, (int)ModuleNumber.TASK,
                                (int)TaskCommand.TASK_PUSH);
                        var user = tg_user.FindByid(userid);
                        var role = tg_role.GetEntityByUserId(userid);
                        if (user == null || role == null)
                            return;
                    }
                }
                tg_task.GetVocationTaskDel();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public List<ASObject> ConvertListAsObject(dynamic list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model = EntityToVo.ToRoleVo(item);
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        #region 并行计算测试

        public void VocationTaskUpdate1()
        {
#if DEBUG
            var stopwatch = new Stopwatch(); //creates and start the instance of Stopwatch
            stopwatch.Start();
#endif

            var newtasks = new List<tg_task>();
            GetReward();
            var userroles = view_user_role_task.GetAll();
            Variable.TaskInfo.Clear();
            Parallel.ForEach(userroles, (i, j) => GetTaskData(i, newtasks));

            tg_task.GetListInsert(newtasks);

            #region 在线则推送数据

            Parallel.ForEach(Variable.OnlinePlayer.Keys, (i, j) => GetTaskPush(i, newtasks));

            #endregion

#if DEBUG
            var timespan = stopwatch.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());
#endif
        }

        /// <summary>
        ///     获取用户任务数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newtasks"></param>
        private void GetTaskPush(Int64 item, List<tg_task> newtasks)
        {
            var item1 = item;
            var dic = new Dictionary<string, object>
            {
                {"1", Common.getInstance().ConvertListASObject(newtasks.Where(q => q.user_id == item1), "VocationTask")}
            };
            var aso = new ASObject(dic);
            var session = Variable.OnlinePlayer[item1] as TGGSession;
            //推送数据
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>
        ///     推送在线玩家数据
        /// </summary>
        /// <param name="usernewtasks"></param>
        /// <param name="newtasks"></param>
        private void GetTaskData(view_user_role_task usernewtasks, List<tg_task> newtasks)
        {
            var basetasks = Common.getInstance().
                GetNewVocationTasks(usernewtasks.role_identity, usernewtasks.user_id, usernewtasks.player_vocation)
                .ToList();
            newtasks.AddRange(basetasks);
            var s_tasks = Common.getInstance().SpecialTasksInit(usernewtasks);
            if (s_tasks != null)
                newtasks.AddRange(s_tasks);
        }

        #endregion
    }
}