using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    public class RECRUIT_REFRESH : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        /// <summary> 刷新 </summary>
        private ASObject CommandStart(TGGSession session, ASObject data)
        {
            var base_rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7002"); //读取基表
            if (base_rule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var player = session.Player.CloneEntity();
            var id = player.UserExtend.id;
            var user_id = player.User.id;
            var recruit = tg_user_extend.FindByid(id);

            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (recruit.recruit_time > time)   //判断是否还在倒计时
                return new ASObject(BuildData((int)ResultType.SUCCESS, recruit.recruit_time));

            var sup_time = Convert.ToInt64(base_rule.value) * 60 * 1000;
            var end_time = time + sup_time;

            tg_role_recruit.ResetCard(user_id);
            tg_user_extend.UpdateRecruit(id, end_time);

            player.UserExtend.recruit_time = end_time;
            session.Player = player;
            TaskOpen(user_id, id, sup_time);

            return new ASObject(BuildData((int)ResultType.SUCCESS, session.Player.UserExtend.recruit_time));
        }

        private void TaskOpen(Int64 user_id, Int64 id, Int64 sup_time)
        {
            var token = new CancellationTokenSource(); //启动按钮线程
            var obj = new ReruitObject { user_id = user_id, id = id, };
            Task.Factory.StartNew(m =>
            {
                SpinWait.SpinUntil(() => false, Convert.ToInt32(m));
            }, sup_time, token.Token)
                .ContinueWith((m, n) =>
                {

                    var temp = n as ReruitObject;
                    if (temp == null) return;
#if DEBUG
                    XTrace.WriteLine(temp.id.ToString());
#endif
                    tg_user_extend.UpdateRecruit(temp.id, 0);
                    var b = Variable.OnlinePlayer.ContainsKey(temp.user_id);
                    if (!b) return;
                    var s = Variable.OnlinePlayer[user_id] as TGGSession;
                    if (s == null) return;
                    s.Player.UserExtend.recruit_time = 0;//更新session
                    token.Cancel();
                }, obj, token.Token);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, Int64 time)
        {
            var dic = new Dictionary<string, object> 
            { 
                { "result", result },
                { "time", time },
            };
            return dic;
        }

        class ReruitObject
        {
            public Int64 user_id { get; set; }
            public Int64 id { get; set; }
        }
    }
}
