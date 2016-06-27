using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Task;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 任务公共部分方法
    /// arlen
    /// </summary>
    public partial class Common
    {

        /// <summary>筹措资金任务(商人)线程</summary>
        public void RaiseTaskThread(tg_task task)
        {
            var time = task.task_endtime - task.task_starttime;
#if DEBUG
            //time = 300000;
#endif
            var token = new CancellationTokenSource();
            System.Threading.Tasks.Task.Factory.StartNew(m =>
            {
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, Convert.ToInt32(m));
            }, time, token.Token)
                .ContinueWith((m, n) =>
                {
                    var tid = Convert.ToInt64(n);
                    var _task = tg_task.FindByid(tid);

                    var entity = (new Share.TGTask()).Ananalyze(_task.task_step_data);
                    if (entity == null) return;
                    if (entity.total > entity.count)//未完成
                    {
                        entity.count = 0;
                        _task.task_state = (int)TaskStateType.TYPE_UNRECEIVED;
                        _task.task_step_data = entity.GetRaiseStepData();
                        _task.Save();
                        new Share.TGTask().AdvancedTaskPush(_task.user_id, _task);
                    }
                    else
                    {
                        //发放奖励
                        _task.task_state = (int)TaskStateType.TYPE_REWARD;
                        PushReward(_task);
                        _task.Delete();
                    }

                    token.Cancel();
                }, task.id, token.Token);
        }


        /// <summary>发放奖励</summary>
        public void PushReward(tg_task task)
        {
            var userid = task.user_id;
            var _base = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == task.task_id);
            if (_base == null) return;
            if (task.task_state != (int)TaskStateType.TYPE_REWARD) return;

            var b = Variable.OnlinePlayer.ContainsKey(task.user_id);
            if (!b) { return; }
            var session = Variable.OnlinePlayer[task.user_id] as TGGSession;
            var rewardstring = GetRewardString(_base, session);   //领取奖励
            new Share.Reward().GetReward(rewardstring, userid);
        }

        /// <summary>登陆重启任务线程</summary>
        public void RaiseTaskRecovery()
        {

            var list = tg_task.GetThreadRaiseTask();
            if (!list.Any()) return;
            foreach (var item in list)
            {
                Int64 timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                item.task_starttime = timeStamp;
                RaiseTaskThread(item);
            }


        }


    }

}
