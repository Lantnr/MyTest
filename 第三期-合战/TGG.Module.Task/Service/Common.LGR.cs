using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    public partial class Common
    {


        #region 组装数据

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            return new Dictionary<string, object>
            {
                {"result", result},
                {"taskVo", newtask == null ? null : EntityToVo.ToVocationTaskVo(newtask)}
            };
        }

        #endregion

        #region 连续战斗

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

            if (string.IsNullOrEmpty(ftep) || string.IsNullOrEmpty(bftep)) 
                return CommonHelper.ErrorResult(ResultType.NO_DATA);

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
            var taskInfo = Variable.TaskInfo.FirstOrDefault(m => m.userid == userid);
            if (taskInfo == null)
            {
                taskInfo = new Variable.UserTaskInfo() { RoleHp = hp, userid = userid };
                Variable.TaskInfo.Add(taskInfo);
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
           // new Share.Fight.Fight().Dispose();
            if (fight.Result != ResultType.SUCCESS)
                return Convert.ToInt32(fight.Result);
            rolelife = fight.PlayHp;
            return fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE;
        }

        /// <summary> 连续战斗领取奖励 </summary>
        public Tuple<int, string> GetContinueReward(Int64 userid, string step, Int32 baseid)
        {
            var stepdata = new TGTask.TaskStep().GetStepData(step);
            var s = stepdata.FirstOrDefault(q => q.Type == (int)TaskStepType.NPC_FIGHT_TIMES);
            if (s == null) return Tuple.Create((int)ResultType.TASK_STEP_ERROR, "");

            var reward = GetReward(baseid, s.FinishValue1);
            if (reward == null) return Tuple.Create((int)ResultType.REWARD_FALSE, "");
            var pd = GetWorkInfo(userid);
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
            if (btask == null) return null;
            switch (count)
            {
                case 0: { return ""; }
                case 1: { return btask.reward; }
                case 2: { return btask.rewardMedium; }
                case 3: { return btask.rewardMax; }
                default: { return ""; }
            }
        }

        #endregion

    }
}
