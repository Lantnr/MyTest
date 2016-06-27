using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Appraise.Service
{
    public class TASK_BUY
    {
        private static TASK_BUY _objInstance;

        /// <summary>TASK_BUY 单体模式</summary>
        public static TASK_BUY GetInstance()
        {
            return _objInstance ?? (_objInstance = new TASK_BUY());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {

#if DEBUG
            XTrace.WriteLine("{0}家臣任务购买--{1}", session.Player.User.player_name, "TASK_BUY");
#endif
            var user = session.Player.User.CloneEntity();
            var userextend = session.Player.UserExtend.CloneEntity();

            if (!CheckMoney(user, userextend.task_role_refresh, session)) //金钱验证
                return BuildData(userextend.task_role_refresh == 0 ? (int)ResultType.BASE_PLAYER_COIN_ERROR : (int)ResultType.BASE_PLAYER_GOLD_ERROR, null, 0);
            var newtasks = new Share.TGTask().RandomTask(session.Player.User.id);
            if (!newtasks.Any()) return BuildData((int)ResultType.APPRAISE_REFLASH_WRONG, null, 0);
            var list = new EntityList<tg_task>();
            list.AddRange(newtasks);
            list.Insert();
#if DEBUG
            XTrace.WriteLine("购买的任务数{0}", newtasks.Count);
#endif
            //更新用户和用户扩展
            userextend.task_role_refresh++;
            userextend.Update();
            session.Player.UserExtend = userextend;
            return BuildData((int)ResultType.SUCCESS, newtasks, 2 - userextend.task_role_refresh);
        }

        /// <summary> 金钱消耗验证 </summary>
        private bool CheckMoney(tg_user user, int count, TGGSession session)
        {
#if DEBUG
            XTrace.WriteLine("{0}玩家第{1}次刷新", session.Player.User.player_name, count);
#endif
            if (count == 0) //第一次刷新
            {
                var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "21002");
                if (baserule == null) return false;
                var costcoin = Convert.ToInt64(baserule.value);
#if DEBUG
                XTrace.WriteLine("{0}玩家第刷新前的铜币{1}", session.Player.User.player_name, session.Player.User.coin);
#endif
                if (user.coin < costcoin) return false;
                user.coin -= costcoin;
                user.Update();
                session.Player.User.coin = user.coin;
#if DEBUG
                XTrace.WriteLine("{0}玩家第刷新后的铜币{1}", session.Player.User.player_name, session.Player.User.coin);
#endif
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
                return true;
            }
            if (count != 1) return false;
#if DEBUG
            XTrace.WriteLine("{0}玩家第刷新前的金币{1}", session.Player.User.player_name, session.Player.User.gold);
#endif
            //第二次刷新
            var baserule1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "21003");
            if (baserule1 == null) return false;
            int costgold = Convert.ToInt32(baserule1.value);
            if (user.gold < costgold) return false;
            user.gold -= costgold;
            user.Update();
            session.Player.User.gold = user.gold;
#if DEBUG
            XTrace.WriteLine("{0}玩家第刷新后的金币{1}", session.Player.User.player_name, session.Player.User.gold);
#endif
            log.GoldInsertLog(costgold, user.id, (int)ModuleNumber.APPRAISE, (int)AppraiseCommand.TASK_BUY);  //金币消费记录
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
            return true;
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, IEnumerable<tg_task> tasks, int count)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                 {"count", count},
                {"task", tasks!=null ? Common.GetInstance().ConvertListAsObject(tasks) : null}
            };
            return new ASObject(dic);
        }
    }
}
