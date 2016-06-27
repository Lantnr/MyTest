using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 
    /// </summary>
    public class BUSINESS_BUY_CAR : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_BUY_CAR", "购买马车");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            return Logic(session);
        }

        private ASObject Logic(TGGSession session)
        {
            var player = session.Player.CloneEntity();

            if (player.Vip.car <= 0)
                return CommonHelper.ErrorResult((int)ResultType.BUSINESS_VIP_CAR_ERROR);
            var rbp = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1003");
            var rp = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3009");
            var base_car = Variable.BASE_PART.FirstOrDefault(m => m.vip == 1);
            if (rp == null || rbp == null || base_car == null)
                return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var birthplace = Convert.ToInt32(rbp.value);    //马车出生地
            var packet = Convert.ToInt32(rp.value);         //默认马车开启车厢数量

            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3012"); //获取基表数据
            if (baserule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var cost = Convert.ToInt32(baserule.value);
            player.User.gold -= cost;

            var car = new tg_car
            {
                car_id = base_car.id,
                distance = 0,
                packet = packet,
                rid = 0,
                speed = base_car.speed,
                start_ting_id = birthplace,
                state = (int)CarStatusType.STOP,
                stop_ting_id = 0,
                time = 0,
                user_id = player.User.id,
            };
            car.Save();

            player.Vip.car = 0;
            player.Vip.Save();
            player.User.Save();
            session.Player = player;
            //元宝更新推送
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            return new ASObject(BuildData((int)ResultType.SUCCESS, car));
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, tg_car car)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {
                    "carVo",
                    EntityToVo.ToBusinessCarVo(car, new List<BusinessGoodsVo>())
                }
            };
            return new ASObject(dic);
        }


    }
}
