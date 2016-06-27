using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 请求战斗
    /// </summary>
    public class TASK_FIGHT
    {
        private static TASK_FIGHT _objInstance;

        /// <summary> TASK_FIGHT单体模式 </summary>
        public static TASK_FIGHT GetInstance()
        {
            return _objInstance ?? (_objInstance = new TASK_FIGHT());
        }

        /// <summary> 请求战斗 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var taskid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "taskId").Value);
            var task = tg_task.FindByid(taskid);
            if (task == null || task.task_type != (int)TaskType.VOCATION_TASK)
                return CommonHelper.ErrorResult((int)ResultType.TASK_VOCATION_NOTASK);
            var level = session.Player.Role.Kind.role_level;
            var userid = session.Player.User.id;
            var result = ConvoyTaskFightResult(userid, level);
            switch (result)
            {
                case (int)FightResultType.WIN: { return TaskFightSucceed(task);/*玩家胜利，则进入城市进入城市，与指定NPC对完话*/ }
                case (int)FightResultType.LOSE: { return TaskFightLose(session, task); /*返回战斗失败玩家战斗失败，则任务失败，系统提示玩家任务失败，任务随之删除*/}
                default: { return CommonHelper.ErrorResult(result); }
            }
        }

        /// <summary> 护送任务战斗结果 </summary>
        private int ConvoyTaskFightResult(Int64 userid, int level)
        {
            var npc = Variable.BASE_NPCARMY.Where(m => m.type == (int)TaskFightType.ESCORT && m.level <= level)
                .OrderByDescending(m => m.level).FirstOrDefault();
            if (npc == null) return (int)ResultType.BASE_TABLE_ERROR;
            return FightStart(userid, npc.id, FightType.NPC_FIGHT_ARMY);
        }

        /// <summary> 任务战斗成功 </summary>
        /// <returns></returns>
        private ASObject TaskFightSucceed(tg_task newtask)
        {
            newtask.is_lock = (int)GuardTaskType.TRIGGER;
            newtask.Update();
            return new ASObject(BulidData());
        }

        /// <summary> 任务失败 </summary>
        private ASObject TaskFightLose(TGGSession session, tg_task newtask)
        {
            newtask.Delete();
            SendProtocol(session, newtask.id);
            return new ASObject(BulidData());
        }

        /// <summary> 推送协议 </summary>
        /// <param name="session">session</param>
        /// <param name="id">战斗Vo</param>
        public void SendProtocol(TGGSession session, Int64 id)
        {
            var aso = new ASObject(BulidData(id));
            var pv = session.InitProtocol((int)ModuleNumber.TASK, (int)TaskCommand.TASK_PUSH_DELETE, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(Int64 id)
        {
            return new Dictionary<string, object>
            {
                 {"result", (int)ResultType.SUCCESS},
                {"taskId", id},
            };
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData()
        {
            return new Dictionary<string, object>
            {
                {"result", (int)ResultType.SUCCESS},
            };
        }

        /// <summary>开始战斗 </summary>
        public int FightStart(Int64 userid, int baseid, FightType fighttype)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, baseid, fighttype, 0, false, true);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS ? (int)fight.Result : fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE;
        }
    }
}
