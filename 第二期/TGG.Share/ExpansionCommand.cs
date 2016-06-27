using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 扩展指令共享类
    /// </summary>
    public class ExpansionCommand : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region GM暗号

        /// <summary> 检测是否GM暗号 </summary>
        /// <param name="data"></param>
        public bool IsGmContent(string data)
        {
            var d = data.Split('|');
            try
            {
                switch (d[0])
                {
                    case "sysnotice_dy": { return SendNotice(d); }
                    case "sysmessage_dy": { return SendMessage(d); }
                    case "sysbuffpower_dy": { return SendBuffPower(d); }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        /// <summary> 发送邮件 </summary>
        private bool SendMessage(string[] data)
        {
            if (data.Count() < 5) return false;
            var attachment = "";
            var where = data[1];
            var title = data[2];
            var contents = data[3];
            for (int i = 4; i < data.Count(); i++)
            {
                attachment += data[i] + "|";
            }
            attachment = attachment.TrimEnd('|');
            (new Message()).MessagesSendAll(where, title, contents, attachment);
            return true;
        }

        /// <summary> 发送公告 </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SendNotice(string[] data)
        {
            if (data.Count() < 2) return false;
            var content = "";
            var id = Convert.ToInt32(data[1]);
            if (data.Count() == 3) content = data[2];
            new Notice().TrainingPlayer(id, content);
            return true;
        }

        private bool SendBuffPower(string[] data)
        {
            if (data.Count() < 3) return false;
            var values = Convert.ToInt32(data[1]);
            var type = data[2];
            BuffPowerSendAll(type, values);
            return true;
        }

        /// <summary> 发送玩家Buff体力 </summary>
        /// <param name="type">要发送的目标</param>
        /// <param name="values">要加的体力值</param>
        public void BuffPowerSendAll(string type, int values)
        {
            switch (type)
            {
                case "0": { SendOnlinePlayer(values); return; }    //在线玩家
                case "1": { SendAllLeadRole(values); return; }     //所有玩家
                default: { SendLeadRole(values, Convert.ToInt64(type)); return; }//指定玩家Id
            }
        }

        /// <summary> 更新推送在线玩家主角武将体力 </summary>
        /// <param name="values">体力值</param>
        private void SendOnlinePlayer(int values)
        {
            var list = Variable.OnlinePlayer.Keys.ToList();
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var userid = Convert.ToInt64(n);
                    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                    var session = Variable.OnlinePlayer[userid] as TGGSession;
                    var role = session.Player.Role.Kind;
                    if (role == null) return;
                    var power = role.buff_power + values;
                    if (power > 50) power = 50;
                    role.buff_power = power;
                    role.Update();
                    new RoleAttUpdate().RoleUpdatePush(role, userid, new List<string>() { "power", "rolepower" });
                    var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                    (new Notice()).SendNotice(role.user_id, 0, data);
                }, item, token.Token);
            }
        }

        private void SendAllLeadRole(int values)
        {
            var list = tg_role.GetAllLeadRole();
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var role = n as tg_role;
                    if (role == null) return;
                    var power = role.buff_power + values;
                    if (power > 50) power = 50;
                    role.buff_power = power;
                    role.Update();
                    if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
                    new RoleAttUpdate().RoleUpdatePush(role, role.user_id, new List<string>() { "power", "rolepower" });
                    var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                    (new Notice()).SendNotice(role.user_id, 0, data);
                }, item, token.Token);
            }
        }

        private void SendLeadRole(int values, Int64 userid)
        {
            if (Variable.OnlinePlayer.ContainsKey(userid))
            {
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                var role = session.Player.Role.Kind;
                if (role == null) return;
                var power = role.buff_power + values;
                if (power > 50) power = 50;
                role.buff_power = power;
                role.Update();
                new RoleAttUpdate().RoleUpdatePush(role, userid, new List<string>() { "power", "rolepower" });
                var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                (new Notice()).SendNotice(role.user_id, 0, data);
            }
            else
            {
                var role = tg_role.GetEntityByUserId(userid);
                if (role == null) return;
                var power = role.buff_power + values;
                if (power > 50) power = 50;
                role.buff_power = power;
                role.Update();
            }
        }

        #endregion

        #region 游艺园每周奖励

        /// <summary> 每周奖励 </summary>
        public void SendYouYiYuanReward()
        {
            var list = view_ranking_game.GetRewardList(10);
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new TaskObject { Userid = item.id, Ranking = item.ranks };
                Task.Factory.StartNew(n =>
                {
                    var temp = n as TaskObject;
                    if (temp == null) return;
                    var reward = Variable.BASE_YOUYIYUANREWARD.FirstOrDefault(m => m.ranking == temp.Ranking);
                    if (reward == null) return; if (reward.reward == "") return;
                    (new Message()).BuildMessagesSend(temp.Userid, "游艺园每周奖励", "游艺园第" + temp.Ranking + "名奖励发放", reward.reward);
                    token.Cancel();

                }, obj, token.Token);
            }
        }

        private class TaskObject
        {
            public Int64 Ranking { get; set; }

            public Int64 Userid { get; set; }
        }

        #endregion
    }
}
