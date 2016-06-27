using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    public class PROP_OPEN_GRID : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        private ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_OPEN_GRID", "开启格子");
#endif
            var player = session.Player.CloneEntity();
            var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value);

            var bagcount = Convert.ToInt32(CommonHelper.BaseRule("4001"));  //默认格子数
            var number = Convert.ToInt32(CommonHelper.BaseRule("4002"));    //上限格子数

            if (count <= 0) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);  //验证前端数据是否有误
            if (player.UserExtend.bag_count >= number) return CommonHelper.ErrorResult(ResultType.BAG_TOPLIMIT);  //验证是否达到上限
            if ((player.UserExtend.bag_count + count) > number) return CommonHelper.ErrorResult(ResultType.BAG_EXCEED_TOPLIMIT);  //验证是否开启成功时超过上限

            var _count = player.UserExtend.bag_count - bagcount;           //已开启格子数
            if (_count < 0) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);

            var base_consume = Consume(_count + 1, count);
            if (base_consume == 0) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            player.User.gold = player.User.gold - base_consume;
            if (player.User.gold < 0) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_GOLD_ERROR);

            player.UserExtend.bag_count += count;
            player.Bag.Surplus += count;
            player.Bag.BagIsFull = false;
            player.User.Update();
            player.UserExtend.Update();
            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            var temp = string.Format("{0}_{1}", "消费:" + base_consume, "开启格子数:" + count);//日志记录
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Use, (int)ModuleNumber.BAG, (int)PropCommand.PROP_OPEN_GRID, temp);

            return new ASObject(BuildData((int)ResultType.SUCCESS, player.UserExtend.bag_count));
        }

        /// <summary> 开启格子消耗 </summary>
        /// <param name="number"> 第几个开启 </param>
        /// <param name="count"> 开启的个数 </param>
        /// <returns>消耗</returns>
        private int Consume(int number, int count)
        {
            var consume = 0;
            for (var i = 0; i < count; i++)
            {
                var baseRule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "4003");
                if (baseRule == null) break;
                var temp = baseRule.value;
                temp = temp.Replace("packet", (number + i).ToString("0.00"));
                var express = CommonHelper.EvalExpress(temp);
                var money = Convert.ToInt32(express);
                consume += money;
            }
            return consume;
        }

        private Dictionary<String, Object> BuildData(int result, int count)
        {
            var dic = new Dictionary<string, object> { 
            { "result", result } ,
            {"count",count}
            };
            return dic;
        }
    }
}
