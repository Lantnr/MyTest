using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 武将宅购买挑战次数
    /// </summary>
    public class TRAIN_HOME_FIGHT_BUY : IConsume
    {
        public ASObject Execute(long userId, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userId)) return Error((int)ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[userId] as TGGSession;

            if (session == null) return Error((int)ResultType.CHAT_NO_ONLINE);
            return CommandStart(session, data);
        }

        /// <summary>武将宅购买挑战次数</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_FIGHT_BUY", "武将宅购买挑战次数");
#endif
                var ext = session.Player.UserExtend;

                var f = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17020");
                var b = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17021");
                var cost = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17023");
                if (f == null || b == null || cost == null) return Error((int)ResultType.BASE_TABLE_ERROR);

                var fcount = Convert.ToInt32(f.value);
                if (ext.fight_count < fcount) return Error((int)ResultType.TRAIN_HOME_FIGHT_ERROR);    //挑战次数未用完

                var bcount = Convert.ToInt32(b.value);
                if (ext.fight_buy >= bcount) return Error((int)ResultType.TRAIN_HOME_BUY_LACK);

                return BuyFight(session.Player.User.id, cost.value, f.value);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>购买挑战次数</summary>
        private ASObject BuyFight(Int64 userid, string rule, string fcount)
        {
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return Error((int)ResultType.CHAT_NO_ONLINE);

            var player = session.Player.CloneEntity();

            var cost = CostGold(rule, player.UserExtend.fight_buy + 1);  
            var gold = player.User.gold;
            if (player.User.gold < cost) return Error((int)ResultType.BASE_PLAYER_GOLD_ERROR);
            player.User.gold -= cost;

            player.User.Update();
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, player.User);

            player.UserExtend.fight_count = 0;
            player.UserExtend.fight_buy++;
            player.UserExtend.Update();
            session.Player = player;

            var logdata = string.Format("{0}_{1}_{2}_{3}", "GoldCost", gold, cost, player.User.gold);   //记录元宝花费日志
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_FIGHT_BUY, logdata);

            var lfight = Convert.ToInt32(fcount) - player.UserExtend.fight_count;
            return BuildData((int)ResultType.SUCCESS, lfight);
        }

        /// <summary>计算购买挑战次数消耗元宝</summary>
        private int CostGold(string rulevalue, int count)
        {
            var temp = rulevalue;
            temp = temp.Replace("n", count.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        /// <summary>返回前端信息</summary>
        private ASObject BuildData(int result, int lfight)
        {
            return new ASObject(new Dictionary<string, object> { { "fightCount", lfight }, { "result", result } });
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
