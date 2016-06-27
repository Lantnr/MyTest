using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Duplicate : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary> 进行守护者奖励发放 </summary>
        public void WatchmenReward()
        {
            try
            {
                var towers = tg_duplicate_checkpoint.GetTowers();
                if (!towers.Any()) return;
                var ids = string.Join(",", towers.Select(m => m.user_id).ToArray());
                var ul = tg_user.GetUsersByIds(ids);
                var base_towerpass = Variable.BASE_TOWERPASS;
                if (!base_towerpass.Any()) return;
                var logs=new List<tg_log_operate> ();
                foreach (var item in towers)
                {
                    var towerpass = base_towerpass.FirstOrDefault(m => m.pass == item.tower_site);
                    if (towerpass == null) continue;
                    var user = ul.FirstOrDefault(m => m.id == item.user_id);
                    user.fame = tg_user.IsFameMax(user.fame, towerpass.watchmenAward);
                    user.Update();
                    RewardsToUser(user, (int)GoodsType.TYPE_FAME);
                    logs.Add(Log(towerpass.watchmenAward, item.user_id, user.fame));
                }
                tg_log_operate.InsertLogs(logs);
            }
            catch
            {
                XTrace.WriteLine("爬塔守护者奖励定时发放错误");
            }
        }

        /// <summary>
        /// 插入主角增加体力日志
        /// </summary>
        public tg_log_operate Log(int fame, Int64 userid, int surplus)
        {

            var log = new tg_log_operate()
            {
                user_id = userid,
                module_number = (int)ModuleNumber.DUPLICATE,
                module_name = "副本",
                command_number = (int)DuplicateCommand.TOWER_CHECKPOINT_DARE_PUSH,
                command_name = "发放爬塔守护者奖励",
                type = (int)LogType.Get,
                resource_type = (int)GoodsType.TYPE_FAME,
                resource_name = "声望",
                count = fame,
                time = DateTime.Now,
                surplus = surplus,
                data = "声望_" + fame + "_" + surplus,
            };
            return log;
        }
        

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(tg_user user, int type)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user.id)) return;
            var session = Variable.OnlinePlayer[user.id] as TGGSession;
            if (session == null) return;
            session.Player.User = user;
            (new User()).REWARDS_API(type, session.Player.User);
        }
    }
}
