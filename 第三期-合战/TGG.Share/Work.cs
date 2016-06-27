using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Work
    {
        /// <summary>高级任务推送</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="task">推送任务实体</param>
        public void AdvancedWorkPush(Int64 user_id, tg_task task)
        {
            var token = new CancellationTokenSource();
            Object obj = new TaskObject { user_id = user_id, task = task };
            System.Threading.Tasks.Task.Factory.StartNew(m =>
            {
                var entity = m as TaskObject;
                if (entity == null) return;
                if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
                var session = Variable.OnlinePlayer[entity.user_id] as TGGSession;
                if (session == null) return;
                var data = new ASObject(BulidData((int)ResultType.SUCCESS, entity.task));
                Push(session, data, (int)WorkCommand.WORK_PUSH_UPDATE);
                token.Cancel();
            }, obj, token.Token);
        }


        class TaskObject
        {
            public Int64 user_id { get; set; }
            public tg_task task { get; set; }
        }

        private void Push(TGGSession session, ASObject data, int commandNumber)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = (int)ModuleNumber.WORK,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            return new Dictionary<string, object>
            {
                {"result", result},
                {"workVo", newtask == null ? null : EntityToVo.ToVocationTaskVo(newtask)}
            };
        }
    }
}
