using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Net;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Prison.Service
{
    public class MESSAGE
    {
        private static MESSAGE objInstance = null;

        /// <summary> MESSAGE单体模式 </summary>
        public static MESSAGE getInstance()
        {
            return objInstance ?? (objInstance = new MESSAGE());
        }
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_COIN, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}监狱留言", "MESSAGE", session.Player.User.player_name);
#endif
            var message = data.FirstOrDefault(q => q.Key == "msg").Value.ToString();
            var user = session.Player.User.CloneEntity();
            if (string.IsNullOrEmpty(message)) return BuildData((int)ResultType.PRISON_MESSAGE_EMPTY, 0);
            var prison = tg_prison.GetPrisonByUserId(user.id);
            if (prison == null) //验证是否在狱中
                return BuildData((int)ResultType.PRISON_OUT_ERROR, 0);
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var doubledate = date.Ticks ;
            var count = tg_prison_messages.GetUserMessageCount(doubledate, user.id); //今日留言次数
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "25001");
            if (baseinfo == null) return Common.GetInstance().BuildData((int)ResultType.PRISON_BASERULE_ERROR);
            var maxcount = Convert.ToInt32(baseinfo.value);
            if (maxcount <= count) return BuildData((int)ResultType.PRISON_MESSAGE_FULL, 0); //今日留言达到上限
            if (!CheckMoney(user, session)) return BuildData((int)ResultType.BASE_PLAYER_COIN_ERROR, 0); //金钱验证
            var msg = new tg_prison_messages()
            {
                play_name = session.Player.User.player_name,
                user_id = user.id,
                message = message,
                writetime = DateTime.Now.Ticks,
            };
            msg.Insert();
            count++;
            return BuildData((int)ResultType.SUCCESS, count);
        }

        private ASObject BuildData(int result, int count)
        {
            var dic = new Dictionary<string, object>()
            {
                { "result", result },
                {"count",count}
            };
            return new ASObject(dic);
        }


        /// <summary> 金钱消耗验证 </summary>
        private bool CheckMoney(tg_user user, TGGSession session)
        {
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "25005");
            if (baserule == null) return false;
            var costcoin = Convert.ToInt64(baserule.value);
#if DEBUG
            XTrace.WriteLine("{0}玩家留言前的铜币{1}", session.Player.User.player_name, session.Player.User.coin);
#endif
            var leftcoin = user.coin - costcoin;
            if (leftcoin < 0) return false;
            user.coin = leftcoin;
            user.Update();
            session.Player.User = user;
#if DEBUG
            XTrace.WriteLine("{0}玩家留言后的铜币{1}", session.Player.User.player_name, session.Player.User.coin);
#endif
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
            return true;
        }
    }
}
