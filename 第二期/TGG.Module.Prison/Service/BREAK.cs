using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Prison.Service
{
    /// <summary>
    /// 越狱
    /// </summary>
    public class BREAK
    {
        private static BREAK ObjInstance;

        public static BREAK GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BREAK());
        }

        /// <summary> BREAK单体模式 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_COIN, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}越狱", "BREAK", session.Player.User.player_name);
#endif
            var user = session.Player.User.CloneEntity();
            var userid = user.id;
            var prison = tg_prison.GetPrisonByUserId(userid);
            if (prison == null)
                return Common.GetInstance().BuildData((int)ResultType.PRISON_OUT_ERROR);
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "25002"); //基表中每分钟消耗金币数
            if (baserule == null) return Common.GetInstance().BuildData((int)ResultType.PRISON_BASERULE_ERROR);
            var cost = Convert.ToInt64(baserule.value);
            var gold = Convert.ToInt32(Math.Ceiling((double)(prison.prison_time - (DateTime.Now.Ticks - 621355968000000000) / 10000) / 60000) * cost);
            if (gold < 0)
                return Common.GetInstance().BuildData((int)ResultType.PRISON_TIMEOUT);
            if (!CheckMoney(user, gold))
                return Common.GetInstance().BuildData((int)ResultType.BASE_PLAYER_GOLD_ERROR);
            prison.Delete();
            //推送玩家离开监狱
            new Share.Prison().PusuLeavePrison(userid);
            //推送玩家进入场景 
            new Share.Prison().PushMyEnterScene(userid);
            //向其他玩家推送玩家进入场景
            new Share.Prison().PushEnterScene(userid);
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS);

        }

        /// <summary> 金钱消耗验证 </summary>
        private bool CheckMoney(tg_user user, int costgold)
        {
#if DEBUG
            XTrace.WriteLine("{0}玩家越狱前的金币{1}", user.player_name, user.gold);
#endif
            var leftgold = user.gold - costgold;
            var userid = user.id;
            if (leftgold < 0) return false;
            user.gold = Convert.ToInt32(leftgold);
            user.Update();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return false;
            session.Player.User = user;
#if DEBUG
            XTrace.WriteLine("{0}玩家越狱后的金币{1}", session.Player.User.player_name, session.Player.User.gold);
#endif
            log.GoldInsertLog(costgold, user.id, (int)ModuleNumber.PRISON, (int)PrisonCommand.BREAK);//金钱消耗记录
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
            return true;
        }
    }
}
