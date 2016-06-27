using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using FluorineFx;
using NewLife.Log;
using System.Linq;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 道具出售
    /// </summary>
    public class PROP_SELL
    {
        private static PROP_SELL _objInstance;

        /// <summary>PROP_SELL单体模式</summary>
        public static PROP_SELL GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_SELL());
        }

        /// <summary>道具出售</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_SELL", "道具出售");
#endif
            var propid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value.ToString());//解析数据
            var count = int.Parse(data.FirstOrDefault(q => q.Key == "count").Value.ToString());
            return BagSell(propid, count, session);
        }

        /// <summary> 道具出售验证出售类型 </summary>
        private ASObject BagSell(Int64 propid, int count, TGGSession session)
        {
            var prop = tg_bag.FindByid(propid); //查询道具信息 
            if (prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_UNKNOW));
            //装备出售走装备模块的协议
            return prop.type == (int)GoodsType.TYPE_EQUIP ? null : PropSell(prop, count, session);
        }


        /// <summary>道具出售</summary>
        private ASObject PropSell(tg_bag prop, int count, TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            var m = Convert.ToString(user.coin);
            var base_prop = Common.GetInstance().GetBaseProp(prop.base_id); //查询道具的基表数据
            if (base_prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            if (base_prop.sell != 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_UNBUY));//判断道具是否能出售
            if (prop.count < count) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_LACK));

            var money = base_prop.price * count;
            user.coin = tg_user.IsCoinMax(user.coin, money);  //处理用户的数据
            if (user.Update() <= 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR));
            session.Player.User = user;

            log.BagInsertLog(prop, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL, base_prop.price * count); //记录日志
            var model = string.Format("{0}", "出售道具:" + prop.id);//日志记录
            (new Log()).WriteLog(prop.user_id, (int)LogType.Delete, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL, model);
            var temp = string.Format("{0}_{1}_{2}", "原:" + m, "获:" + money, "现:" + user.coin);
            (new Log()).WriteLog(prop.user_id, (int)LogType.Get, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL, temp);

            prop.count -= count;
            var rewards = new List<RewardVo> { new RewardVo { goodsType = (int)GoodsType.TYPE_COIN, value = user.coin } };
            (new Bag()).BuildReward(user.id, new List<tg_bag> { prop }, rewards);

            return prop.count == 0 ? new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS))
                : new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, prop));
        }
    }
}
