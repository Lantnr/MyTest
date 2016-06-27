using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Entity;
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
                foreach (var item in towers)
                {
                    var towerpass = base_towerpass.FirstOrDefault(m => m.pass == item.tower_site);
                    if (towerpass == null) continue;
                    var user = ul.FirstOrDefault(m => m.id == item.user_id);
                    user.fame = tg_user.IsFameMax(user.fame, towerpass.watchmenAward);
                    user.Update();
                    RewardsToUser(user, (int)GoodsType.TYPE_FAME);


                }
            }
            catch
            {
                XTrace.WriteLine("爬塔守护者奖励定时发放错误");
            }
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
