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
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 修行解锁
    /// </summary>
    public class TRAIN_ROLE_LOCK : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_LOCK", "修行解锁");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return new ASObject((new Family()).BuilData((int)ResultType.BASE_PLAYER_OFFLINE_ERROR));
            return CommandStart(session);
        }

        /// <summary>修行解锁</summary>
        public ASObject CommandStart(TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            var user_extend = session.Player.UserExtend.CloneEntity();

            var vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == session.Player.Vip.vip_level);
            if (vip == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            if (user_extend.train_bar >= vip.trainBar) return CommonHelper.ErrorResult((int)ResultType.TRAINROLE_BAR_ENOUGH);

            var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17001");
            var rule2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17002");
            var rule3 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17003");
            if (rule1 == null || rule2 == null || rule3 == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var bar = Convert.ToInt32(rule1.value);
            var maxbar = Convert.ToInt32(rule2.value);
            var cost = 0;
            if (user_extend.train_bar >= maxbar) return CommonHelper.ErrorResult((int)ResultType.TRAINROLE_BAR_ENOUGH);
            if (user_extend.train_bar < bar + 1)
            {
                cost = Convert.ToInt32(rule3.value);
                if (user.coin < cost) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR);
                CreateLog(user.id, user.coin, user.gold, cost, 0);
                user.coin = user.coin - cost;
                if (!tg_user.GetUserUpdate(user)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                (new Share.RoleTrain()).RewardsToUser(session, user, (int)GoodsType.TYPE_COIN); //推送消耗丁银
            }
            else
            {
                cost = RuleData(user_extend.train_bar);
                if (user.gold < cost) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);
                CreateLog(user.id, user.coin, user.gold, cost, 1);
                user.gold = user.gold - cost;
                if (!tg_user.GetUserUpdate(user)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                (new Share.RoleTrain()).RewardsToUser(session, user, (int)GoodsType.TYPE_GOLD);//推送消耗金币                
            }
            user_extend.train_bar += 1;
            if (!tg_user_extend.GetUpdate(user_extend)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            session.Player.UserExtend = user_extend;
            return new ASObject(BuilData((int)ResultType.SUCCESS, user_extend.train_bar));
        }

        private void CreateLog(Int64 userid, Int64 coin, int gold, int cost, int type)
        {
            string logdata = "";
            if (type == 0)
            {
                var _coin = coin;
                coin -= cost;
                //日志
                logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", _coin, cost, coin);

            }
            else
            {
                var _gold = gold;
                gold -= cost;
                //日志
                logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", _gold, cost, gold);
            }
            (new Share.Log()).WriteLog(userid, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_LOCK, logdata);
        }

        public int RuleData(int bar)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "17004");
            if (rule == null) return 0;
            var rulevalue = rule.value.Replace("bar", bar.ToString("0.00"));
            var express = CommonHelper.EvalExpress(rulevalue);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        private ASObject Error(int error)
        {
            return new ASObject(BuilData(error, 0));
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, int bar)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"bar", bar},
            };
            return dic;
        }
    }
}
