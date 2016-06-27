using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.Task;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    /// <summary>
    /// 工作更新和领取奖励
    /// </summary>
    public class WORK_UPDATE
    {
        private static WORK_UPDATE ObjInstance;

        /// <summary> WORK_UPDATE单体模式 </summary>
        public static WORK_UPDATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new WORK_UPDATE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var npc = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "npcId").Value);
            var step = data.FirstOrDefault(q => q.Key == "data").Value;
            var mainid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "taskId").Value);
            var rewardstring = "";
            var WorkInfo = tg_task.FindByid(mainid);

            if (WorkInfo == null || WorkInfo.task_type != (int)TaskType.WORK_TASK || WorkInfo.task_state == (int)TaskStateType.TYPE_UNRECEIVED)
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));

            var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == WorkInfo.task_id);
            if (baseinfo == null) return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));
            var steptype = Convert.ToInt32(baseinfo.stepCondition.Split(new[] { '|', '_' })[0]);
            if (type == 1)
            {
                if (WorkInfo.task_state != (int)TaskStateType.TYPE_UNFINISHED)
                    return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null));
                #region 任务步骤验证与更新
                switch (steptype)
                {
                    case (int)TaskStepType.STAND_GUARD://站岗
                        {
                            if (!CheckStandGuard(baseinfo.time, baseinfo.stepCondition, WorkInfo, npc, session.Player.Scene.scene_id, session.Player.User.player_camp))
                                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        }
                        break;
                    case (int)TaskStepType.ASSASSINATION://刺杀
                        {
                            if (!KillTask(npc, baseinfo, session.Player.User.player_camp, session.Player.Role.Kind.role_level, ref WorkInfo))
                                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null));
                        }
                        break;
                    case (int)TaskStepType.FIGHTING_CONTINUOUS://连续战斗
                        {
                            return ContinueFight(session, WorkInfo, baseinfo.stepCondition, npc);
                        }
                    case (int)TaskStepType.GUARD://守护
                        {
                            var tuple = WatchTask(npc, baseinfo.time, session.Player.User.player_camp, session.Player.Role.Kind.role_level, WorkInfo);
                            if (!tuple.Item2) return new ASObject(BulidData((int)tuple.Item1, null));
                        }
                        break;
                    //谣言类
                    case (int)TaskStepType.RUMOR:
                    case (int)TaskStepType.FIRE:
                    case (int)TaskStepType.BREAK:
                    case (int)TaskStepType.SEll_WINE:
                        {
                            return Common.GetInstance().RumorTask(session.Player, WorkInfo, baseinfo.stepCondition, npc);
                        }
                    //戒严类
                    case (int)TaskStepType.ARREST_RUMOR:
                    case (int)TaskStepType.ARREST_FIRE:
                    case (int)TaskStepType.ARREST_BREAK:
                    case (int)TaskStepType.ARREST_SEll_WINE:
                        {
                            return Common.GetInstance().ArrestTask(baseinfo.time, baseinfo.stepCondition, WorkInfo, npc, session.Player.Scene.scene_id);
                        }
                    case (int)TaskStepType.ESCORT://护送
                        {
                            var tuple = new Share.TGTask().CheckTaskStep(baseinfo.stepCondition, WorkInfo.task_step_data, userid, npc);
                            if (tuple.Item1 < 0)
                                return new ASObject(BulidData(tuple.Item1, null));
                            Common.GetInstance().TaskUpdate(WorkInfo, tuple.Item2, baseinfo.stepCondition);
                        }
                        break;
                }
                #endregion
                return new ASObject(BulidData((int)ResultType.SUCCESS, WorkInfo));
            }
            if (type != 2) return new ASObject(BulidData((int)ResultType.FRONT_DATA_ERROR, null));
            if (WorkInfo.task_state != (int)TaskStateType.TYPE_REWARD)  //验证任务是否完成
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UNFINISH, null));

            switch (steptype)
            {
                case (int)TaskStepType.FIGHTING_CONTINUOUS:
                    {
                        var tuple = GetContinueReward(userid, WorkInfo.task_step_data, baseinfo.id);
                        if (tuple.Item1 < 0)
                            return new ASObject(BulidData(tuple.Item1, null));
                        rewardstring = tuple.Item2;
                    } break;
                default:
                    {
                        var maxc = baseinfo.workRewardMaxCon;
                        var maxr = baseinfo.workRewardMax;
                        var mediac = baseinfo.rewardMediumCon;
                        var mediar = baseinfo.workRewardMedium;
                        var reward = baseinfo.workReward;
                        rewardstring = Common.GetInstance().GetRewardString(maxc, maxr, mediac, mediar, reward, session.Player.Role);   //领取奖励
                    } break;
            }
            if (rewardstring != "") //连续战斗没有胜利没有奖励
            {
                if (!new Share.Reward().GetReward(rewardstring, userid))
                    return new ASObject(BulidData((int)ResultType.REWARD_FALSE, null));
            }
            new Share.DaMing().CheckDaMing(userid, (int)DaMingType.工作);
            //格式：任务_任务主键id_任务id|奖励字符串
            var logdata = string.Format("T_{0}_{1}%{2}", WorkInfo.id, WorkInfo.task_id, rewardstring);
            new Share.Log().WriteLog(userid, (int)LogType.Delete, (int)ModuleNumber.WORK, (int)WorkCommand.WORK_UPDATE, logdata);
            WorkInfo = Common.GetInstance().WorkTasksInit(WorkInfo);
            WorkInfo.id = mainid;
            WorkInfo.Update();
            Common.GetInstance().ClearTime(WorkInfo.user_id);
            return new ASObject(BulidData((int)ResultType.SUCCESS, WorkInfo));
        }

        #region 任务验证

        /// <summary>
        /// 站岗验证
        /// </summary>
        /// <param name="time"></param>
        /// <param name="finishstep"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private bool CheckStandGuard(int time, string finishstep, tg_task task, int getnpc, long sceneid, int camp)
        {
            var npc = task.task_step_data.Split(new[] { '_', '|' }).ToList();
            if (npc.Count <= 4) return false;
            if (npc[3] != getnpc.ToString()) return false;
            var v_task = Common.GetInstance().GetWorkInfo(task.user_id);
            if (v_task.GuardSceneId > 0) return false;
            v_task.GuardSceneId = sceneid;
            v_task.GuardCamp = camp;
            NewTaskStart(time, task, finishstep);
            return true;
        }
        #endregion

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            var taskvo = new VocationTaskVo();
            var dic = new Dictionary<string, object>();
            if (newtask != null)
                taskvo = EntityToVo.ToVocationTaskVo(newtask);
            dic.Add("result", result);
            dic.Add("workVo", newtask == null ? null : taskvo);
            return dic;
        }

        /// <summary>
        /// 开启新线程
        /// </summary>
        /// <param name="time"></param>
        /// <param name="mytask"></param>
        /// <param name="finishstep"></param>
        public void NewTaskStart(decimal time, tg_task mytask, string finishstep)
        {
#if DEBUG
            time = 10;
#endif
            try
            {
                var temp = new WatchObject()
                {
                    task_id = mytask.id,
                    time = (int)time,
                    user_id = mytask.user_id,
                    step_string = finishstep
                };
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var t = m as WatchObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        if (!Variable.CD.ContainsKey(key))
                        {
                            token.Cancel();
                            return true;
                        }
                        if (Convert.ToBoolean(Variable.CD[key]))
                        {
                            token.Cancel();
                            return true;
                        }
                        var w = Common.GetInstance().GetWorkInfo(t.user_id);
                        if (w.GuardSceneId == 0)
                        {
                            token.Cancel();
                            return true;
                        }
                        return false;
                    }, temp.time * 1000);
                }, temp, token.Token).ContinueWith((m, n) =>
                {
                    var wo = n as WatchObject;
                    if (wo == null) { token.Cancel(); return; }
                    TaskUpdateAndSend(wo.task_id, wo.step_string);
                    var key = wo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, temp, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>
        /// 完成任务并推送
        /// </summary>
        /// <param name="taskid">任务主键id</param>
        /// <param name="finishstep"></param>
        private void TaskUpdateAndSend(Int64 taskid, string finishstep)
        {
            var task = tg_task.FindByid(taskid);
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id)) return;
            var v_task = Variable.WorkInfo.FirstOrDefault(q => q.userid == task.user_id);
            if (v_task != null) v_task.GuardSceneId = 0;
            task.task_step_data = finishstep;
            task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
            (new Share.Work()).AdvancedWorkPush(task.user_id, task);//推送协议
        }

        #region   守护
        /// <summary>守护任务</summary>
        private Tuple<ResultType, bool> WatchTask(int npc, int time, int camp, int rolelevel, tg_task task)
        {
            var userid = task.user_id;
            var mysteplist = task.task_step_data.Split('_').ToList();
            if (mysteplist[1] != npc.ToString())
                return Tuple.Create(ResultType.TASK_VOCATION_UPDATEWRONG, false);
            //随机生成npcid
            var npcid = GetNpcId(camp, rolelevel);
            //npcid = 15010001;//测试
            var watch = Common.GetInstance().GetWorkInfo(userid);
            if (watch.WatchNpcid == 0) watch.WatchNpcid = npcid;

            WatchThreading(time * 1000, task);//开始任务
            return Tuple.Create(ResultType.SUCCESS, true);
        }



        /// <summary>守护线程</summary>
        public void WatchThreading(int time, tg_task newtask)
        {
            try
            {
                var obj = new WatchObject { user_id = newtask.user_id, task_id = newtask.id, time = time, };
                var token = new CancellationTokenSource();

                Task.Factory.StartNew(m =>
                {
                    var wo = m as WatchObject;
                    if (wo == null) return;
                    var key = wo.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        if (!Variable.CD.ContainsKey(key))
                        {
                            token.Cancel();
                            return true;
                        }
                        if (Convert.ToBoolean(Variable.CD[key]))
                        {
                            token.Cancel();
                            return true;
                        }
                        var watch = Common.GetInstance().GetWorkInfo(wo.user_id);
                        if (watch.WatchState == (int)TaskKillType.LOSE)
                        {
                            watch.WatchState = 0;
                            watch.WatchNpcid = 0;
                            watch.P_State = 0;
                            token.Cancel();
                            return true;
                        }
                        return false;
                    }, wo.time);
                }, obj, token.Token).ContinueWith((m, n) =>
                {
                    var wo = n as WatchObject;
                    if (wo == null) { token.Cancel(); return; }
                    WatchEnd(Convert.ToInt64(wo.task_id));
                    //移除全局变量
                    var key = wo.GetKey();
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

        /// <summary>任务结束</summary>
        public void WatchEnd(Int64 task_id)
        {
            var task = tg_task.FindByid(task_id);
            if (!Variable.OnlinePlayer.ContainsKey(task.user_id)) return;
            var basetask = Variable.BASE_TASKVOCATION.FirstOrDefault(m => m.id == task.task_id);
            var watch = Variable.WorkInfo.FirstOrDefault(m => m.userid == task.user_id);
            if (basetask == null || watch == null) return;
            task.task_step_data = basetask.stepCondition;
            task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();

            watch.WatchNpcid = 0;
            watch.WatchState = 0;
            watch.P_State = 0;
            (new Share.Work()).AdvancedWorkPush(task.user_id, task);
            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
            if (session == null) return;
            WORK_FIGHT_PUSH.GetInstance().CommandStart(session, null, (int)TaskWatchType.FINISH);
        }

        class WatchObject
        {
            public Int64 user_id { get; set; }
            public Int64 task_id { get; set; }

            public Int32 time { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, user_id, task_id);
            }

            public string step_string { get; set; }
        }
        #endregion

        #region  刺杀任务
        /// <summary>刺杀结果</summary>
        public bool KillTask(int npc, BaseTaskVocation basetask, int camp, int rolelevel, ref tg_task task)
        {
            int result = 0; var t = task.CloneEntity();
            var mysteplist = task.task_step_data.Split('_').ToList();
            if (mysteplist[1] != npc.ToString()) return false;
            int npcid = 0;
            npcid = camp == (int)CampType.East ? GetNpcId((int)CampType.West, rolelevel) : GetNpcId((int)CampType.East, rolelevel);//npcid = 15010001;//测试

            var kill = Common.GetInstance().GetWorkInfo(task.user_id);
            kill.KillNpcid = npcid;
            var watchlist = Variable.WorkInfo.Where(m => m.WatchNpcid == npcid && m.P_State == 0).ToList();
            if (watchlist.Any()) //判断有没有守护玩家
            {
                if (kill.KillState == (int)TaskKillType.WIN)//判断是否已挑战了守护玩家
                    result = NpcChallenge(task.user_id, npcid, FightType.NPC_MONSTER);
                else
                {
                    if (!KillWatch(watchlist, task.user_id)) return false;
                    return true;
                }
            }
            else
                result = NpcChallenge(task.user_id, npcid, FightType.NPC_MONSTER);

            if (result == (int)FightResultType.WIN)
            {
                task.task_step_data = basetask.stepCondition;
                task.task_state = (int)TaskStateType.TYPE_REWARD;
                task.Update();
            }
            kill.KillNpcid = 0;
            kill.KillState = 0;
            return true;
        }

        /// <summary>刺杀守护者</summary>
        /// <param name="watchlist">守护玩家列表</param>
        public bool KillWatch(List<Variable.UserTaskInfo> watchlist, Int64 userid)
        {
            var id = GetWatchId(watchlist);
            if (id == 0) return false;
            var fightvo = UserChallenge(userid, id, FightType.ONE_SIDE);
            var watch = Variable.WorkInfo.FirstOrDefault(m => m.userid == id);

            if (!Variable.OnlinePlayer.ContainsKey(id)) return false;
            var watchse = Variable.OnlinePlayer[id] as TGGSession;

            var kill = Variable.WorkInfo.FirstOrDefault(m => m.userid == userid);
            if (fightvo.isWin == false)
            {
                kill.KillState = (int)TaskKillType.WIN;
                watch.WatchState = (int)TaskKillType.LOSE;
                WORK_FIGHT_PUSH.GetInstance().CommandStart(watchse, fightvo, (int)TaskWatchType.LOSE);
            }
            else
            {
                kill.KillState = (int)TaskKillType.LOSE;
                kill.KillNpcid = 0;
                watch.WatchNpcid = 0;
                WORK_FIGHT_PUSH.GetInstance().CommandStart(watchse, fightvo, (int)TaskWatchType.WIN);
            }
            return true;
        }


        /// <summary> 随机刷新一个守护者玩家 </summary>
        public Int64 GetWatchId(List<Variable.UserTaskInfo> watchlist)
        {
            var index = RNG.Next(0, watchlist.Count() - 1);
            watchlist[index].P_State = 1;
            return watchlist[index].userid;
        }

        /// <summary>得到战斗结果</summary>
        private int NpcChallenge(Int64 userid, Int64 rivalid, FightType type)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, rivalid, type, 0, false, true);
            new Share.Fight.Fight().Dispose();
            if (fight.Result != ResultType.SUCCESS)
                return Convert.ToInt32(fight.Result);
            return fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE;
        }

        /// <summary>得到战斗vo</summary>
        private FightVo UserChallenge(Int64 userid, Int64 rivalid, FightType type)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, rivalid, type, 0, false, true, true);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS ? null : fight.Rfight;
        }

        public int GetNpcId(int camp, int rolelevel)
        {
            var base_npc = Variable.BASE_NPCMONSTER.Where(m => m.camp == camp).ToList();
            var list = new List<string>();
            //var npcids = new List<Int32>();
            var npclist = new List<BaseNpcMonster>();
            foreach (var item in base_npc)
            {
                if (list.Count() == 0)
                {
                    list.Add(item.levelLimit);
                }
                var l = list.FirstOrDefault(m => m == item.levelLimit);
                if (l == null)
                    list.Add(item.levelLimit);
            }
            foreach (var lm in list)
            {
                var sp = lm.Split('_');
                if (rolelevel >= Convert.ToInt32(sp[0]) && rolelevel <= Convert.ToInt32(sp[1]))
                {
                    npclist = base_npc.Where(m => m.levelLimit == lm).ToList();
                    //npcids = npclist.Select(m => m.id).ToList();
                    break;
                }
            }
            var index = RNG.Next(0, npclist.Count() - 1);
            return npclist[index].id;
        }
        #endregion

        /// <summary> 连续战斗 </summary>
        /// <param name="session"></param>
        /// <param name="task"></param>
        /// <param name="btask"></param>
        /// <param name="npc"></param>
        /// <returns></returns>
        public ASObject ContinueFight(TGGSession session, tg_task task, string btask, int npc)
        {
            var step = task.task_step_data.Split("|");
            var bstep = btask.Split("|"); //步骤条件
            if (step.Count() != bstep.Count() || step.Count() < 3)
                return CommonHelper.ErrorResult(ResultType.TASK_STEP_ERROR);

            var ftep = step[2]; //战斗步骤
            var bftep = bstep[2]; //战斗步骤
            var s = ftep.Split('_');
            if (s[0] != Convert.ToInt32(TaskStepType.NPC_FIGHT_TIMES).ToString())
                return CommonHelper.ErrorResult(ResultType.TASK_STEP_ERROR);
            ftep = GetNewFightString(session, s);
            if (ftep.IndexOf("false", StringComparison.Ordinal) > -1)
            {
                step[2] = ftep.Replace("false", "");
                task.task_step_data = string.Join("|", step);
                task.task_state = (int)TaskStateType.TYPE_REWARD;
                task.Update();
                return new ASObject(BulidData((int)ResultType.SUCCESS, task));
            }
            var _ftep = ftep.Substring(0, ftep.Length - 1);
            var _bftep = bftep.Substring(0, bftep.Length - 1);
            if (_ftep == _bftep)
            {
                var tftep = bstep[1]; //对话步骤
                step[1] = tftep;
                task.task_state = (int)TaskStateType.TYPE_REWARD;
            }
            step[2] = ftep;
            task.task_step_data = string.Join("|", step);
            task.Update();
            return new ASObject(BulidData((int)ResultType.SUCCESS, task));
        }

        /// <summary> 获取更新后战斗步骤 </summary>
        /// <param name="session"></param>
        /// <param name="s1"></param>
        /// <returns></returns>
        private string GetNewFightString(TGGSession session, string[] s1)
        {
            var userid = session.Player.User.id;
            var level = session.Player.Role.Kind.role_level;
            var hp = session.Player.Role.Kind.att_life;

            //var taskInfo = Common.GetInstance().GetWorkInfo(userid);
            //taskInfo.RoleHp = hp;

            var taskInfo = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
            if (taskInfo == null)
            {
                taskInfo = new Variable.UserTaskInfo() { RoleHp = hp, userid = userid };
                Variable.WorkInfo.Add(taskInfo);
            }
            else
            {
                if (taskInfo.RoleHp == -99999) taskInfo.RoleHp = hp;
            }

            var npc = Variable.BASE_NPCARMY.Where(m => m.type == (int)TaskFightType.CONTINUE_FIGHT && m.level <= level)
       .OrderByDescending(m => m.level).FirstOrDefault();
            if (npc == null) return "";

            var life = taskInfo.RoleHp;
            var result = NpcChallenge(userid, npc.id, ref life);
            if (result < 0) return "";
            if (result == (int)FightResultType.LOSE)
            {
                taskInfo.RoleHp = 0;
                s1[2] = (Convert.ToInt32(s1[2]) + 1).ToString();
                return "false" + GetString(s1, true);
            }
            taskInfo.RoleHp = life;
            s1[2] = (Convert.ToInt32(s1[2]) + 1).ToString();
            s1[3] = s1[2];
            return GetString(s1, true);
        }

        private string GetString(IEnumerable<string> s1, bool flag)
        {
            var str = flag ? s1.Aggregate("", (current, item) => current + (item + "_")) :
                 s1.Aggregate("", (current, item) => current + (item + "|"));
            str = str.Substring(0, str.Length - 1);
            return str;
        }

        /// <summary> 进入战斗 </summary>
        /// <param name="userid"></param>
        /// <param name="npcid">rivalid 包括npc</param>
        /// <param name="rolelife"></param>
        private int NpcChallenge(Int64 userid, int npcid, ref Int64 rolelife)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, npcid, FightType.CONTINUOUS, rolelife, false, true);
            new Share.Fight.Fight().Dispose();
            if (fight.Result != ResultType.SUCCESS)
                return Convert.ToInt32(fight.Result);
            rolelife = fight.PlayHp;
            return fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE;
        }

        /// <summary> 连续战斗领取奖励 </summary>
        public Tuple<int, string> GetContinueReward(Int64 userid, string step, Int32 baseid)
        {
            var stepdata = new TGTask.TaskStep().GetStepData(step);
            if (stepdata == null) return Tuple.Create((int)ResultType.TASK_STEP_ERROR, "");
            var s = stepdata.FirstOrDefault(q => q.Type == (int)TaskStepType.NPC_FIGHT_TIMES);
            if (s == null) return Tuple.Create((int)ResultType.TASK_STEP_ERROR, "");

            var reward = GetReward(baseid, s.FinishValue1);
            if (reward == null) return Tuple.Create((int)ResultType.REWARD_FALSE, "");
            var pd = Common.GetInstance().GetWorkInfo(userid);
            pd.RoleHp = -99999;
            return Tuple.Create((int)ResultType.SUCCESS, reward);
        }

        /// <summary> 连续战斗任务获取奖励字符串 </summary>
        /// <param name="baseid"></param>
        /// <param name="count">连胜次数</param>
        /// <returns></returns>
        private string GetReward(int baseid, int count)
        {
            var btask = Variable.BASE_TASKVOCATION.FirstOrDefault(m => m.id == baseid);
            if (btask == null) return "Error";
            switch (count)
            {
                case 0: { return ""; }
                case 1: { return btask.workReward; }
                case 2: { return btask.workRewardMedium; }
                case 3: { return btask.workRewardMax; }
                default: { return ""; }
            }
        }
    }
}
