using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 进入本丸
    /// </summary>
    public class BROKEN_BASE
    {
        public static BROKEN_BASE ObjInstance;

        /// <summary>BROKEN_BASE单体模式</summary>
        public static BROKEN_BASE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BROKEN_BASE());
        }

        /// <summary> 进入本丸</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var playerdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == user.id);
            if (playerdata == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_NO_PLAYER_DATA)); //验证玩家活动数据
            if (playerdata.count < 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_YUNTI_ERROR));  //验证云梯数

            var rivalcamp = user.player_camp == (int)CampType.East ? (int)CampType.West : (int)CampType.East;             //对手阵营
            var boss = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == rivalcamp);
            if (boss == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_NO_PLAYER_DATA));   //验证Boss数据
            if (boss.GateLife > 0 || boss.GateLife > 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_BOSS_ERROR));//验证前2个Boss是否死掉

            playerdata.fame += playerdata.count;
            lock (this) { boss.BaseLife -= playerdata.count; }
            playerdata.count = 0;

            PUSH_OURS_HP.GetInstance().CommandStart(user.player_camp, rivalcamp, (int)SiegeNpcType.BASE, boss.BaseLife, playerdata.fame);
            return new ASObject(BuildData((int)ResultType.SUCCESS));
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return dic;
        }
    }
}
