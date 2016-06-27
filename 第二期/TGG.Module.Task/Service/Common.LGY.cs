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
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    public partial class Common
    {

        /// <summary>随机生成npcid</summary>
        /// <param name="camp">阵营</param>
        /// <param name="rolelevel">主角等级</param>
        /// <returns>npcid</returns>
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

            //var ns = RNG.Next(1, npcids);
            //int npcid = 0;
            //foreach (var item in ns) npcid = item;
            //return npcid;
        }

        #region   守护
        /// <summary>守护任务</summary>
        public bool WatchTask(int npc, BaseTaskVocation basetask, int camp, int rolelevel, ref tg_task task)
        {
            var mysteplist = task.task_step_data.Split('_').ToList();
            var t = task.CloneEntity();
            if (mysteplist[1] != npc.ToString()) return false;

            //随机生成npcid
            var npcid = GetNpcId(camp, rolelevel);
            //npcid = 15010622;//测试
            var watch = GetWorkInfo(task.user_id);
            if (watch.WatchNpcid == 0) watch.WatchNpcid = npcid;
            WatchThreading(basetask.time * 1000, task);//开始任务
            return true;
        }

   
        /// <summary>守护线程</summary>
        public void WatchThreading(int time, tg_task newtask)
        {
# if DEBUG
            //time = 10;
#endif
            try
            {
                var token = new CancellationTokenSource();
                var task = new System.Threading.Tasks.Task(() => SpinWait.SpinUntil(() =>
                {
                    var watch = Variable.TaskInfo.FirstOrDefault(m => m.userid == newtask.user_id);
                    if (watch == null)
                    {
                        watch = new Variable.UserTaskInfo() { userid = newtask.user_id };
                        Variable.TaskInfo.Add(watch);
                    }
                    if (watch.WatchState == (int)TaskKillType.LOSE)
                    {
                        watch.WatchState = 0;
                        watch.WatchNpcid = 0;
                        watch.P_State = 0;
                        token.Cancel();
                        return true;
                    }
                    return false;
                }, time), token.Token);
                task.Start();
                task.ContinueWith(m =>
                {
                    WatchEnd(newtask.id);
                    token.Cancel();
                }, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>任务结束</summary>
        public void WatchEnd(Int64 task_id)
        {
            var newtask = tg_task.FindByid(task_id);
            if (!Variable.OnlinePlayer.ContainsKey(newtask.user_id)) return;
            var basetask = Variable.BASE_TASKVOCATION.FirstOrDefault(m => m.id == newtask.task_id);
            var watch = Variable.TaskInfo.FirstOrDefault(m => m.userid == newtask.user_id);
            if(basetask==null) return;
            newtask.task_step_data = basetask.stepCondition;
            newtask.task_state = (int)TaskStateType.TYPE_REWARD;
            newtask.Update();

            watch.WatchNpcid = 0;
            watch.WatchState = 0;
            watch.P_State = 0;
            (new Share.TGTask()).AdvancedTaskPush(newtask.user_id, newtask);

            var session = Variable.OnlinePlayer[newtask.user_id] as TGGSession;
            if (session == null)
                return;
            TASK_FIGHT_PUSH.GetInstance().CommandStart(session, null, (int)TaskWatchType.FINISH);
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
            npcid = camp == (int)CampType.East ? GetNpcId((int)CampType.West, rolelevel) : GetNpcId((int)CampType.East, rolelevel);
            //npcid = 15010622;//测试

            var kill = Variable.TaskInfo.FirstOrDefault(m => m.userid == t.user_id);
            if (kill == null)
            {
                Variable.TaskInfo.Add(GetkillTask(t.user_id, npcid));
                kill = Variable.TaskInfo.FirstOrDefault(m => m.userid == t.user_id);
            }
            if (kill.KillNpcid == 0) kill.KillNpcid = npcid;

            var watchlist = Variable.TaskInfo.Where(m => m.WatchNpcid == npcid && m.P_State == 0).ToList();
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
            var watch = Variable.TaskInfo.FirstOrDefault(m => m.userid == id);

            if (!Variable.OnlinePlayer.ContainsKey(id)) return false;
            var watchse = Variable.OnlinePlayer[id] as TGGSession;

            var kill = Variable.TaskInfo.FirstOrDefault(m => m.userid == userid);
            if (fightvo.isWin == false)
            {
                kill.KillState = (int)TaskKillType.WIN;
                watch.WatchState = (int)TaskKillType.LOSE;
                TASK_FIGHT_PUSH.GetInstance().CommandStart(watchse, fightvo, (int)TaskWatchType.LOSE);
            }
            else
            {
                kill.KillState = (int)TaskKillType.LOSE;
                kill.KillNpcid = 0;
                watch.WatchNpcid = 0;
                TASK_FIGHT_PUSH.GetInstance().CommandStart(watchse, fightvo, (int)TaskWatchType.WIN);
            }
            return true;
        }


        /// <summary>创建全局刺杀任务</summary>
        public Variable.UserTaskInfo GetkillTask(Int64 userid, int npcid)
        {
            Variable.UserTaskInfo usertask = new Variable.UserTaskInfo();
            usertask.userid = userid;
            usertask.KillNpcid = npcid;
            usertask.KillState = 0;
            return usertask;
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
            return fight.Result != ResultType.SUCCESS ? null : fight.Rfight;  //fight.Ofight;
        }

        #endregion



    }
}
