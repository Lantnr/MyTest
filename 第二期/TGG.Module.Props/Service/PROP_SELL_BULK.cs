using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 批量出售
    /// </summary>
    public class PROP_SELL_BULK
    {
        private static PROP_SELL_BULK _objInstance;

        /// <summary>PROP_SELL_BULK 单体模式</summary>
        public static PROP_SELL_BULK GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_SELL_BULK());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_PROP, session, data);
        }

        /// <summary>道具批量出售</summary>
        public ASObject CommandStart(int goodtype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_SELL_BULK", "道具批量出售");
#endif
            //解析数据
            var ids = data.FirstOrDefault(q => q.Key == "ids").Value as object[];
            if (ids == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            var bagids = ids.Select(Convert.ToInt64).Where(m => m > 0).ToList();

            return bagids.Any() ? BagSale(bagids, session) :
                new ASObject(Common.GetInstance().BuildData((int)ResultType.NO_DATA));
        }

        /// <summary>背包批量出售</summary>
        private ASObject BagSale(List<Int64> ids, TGGSession session)
        {
            int money = 0;
            var user = session.Player.User.CloneEntity();
            var my = Convert.ToString(user.coin);
            var list = tg_bag.GetFindByIds(ids);

            var equips = list.ToList().Where(m => m.type == (int)GoodsType.TYPE_EQUIP).ToList();
            if (equips.Any()) money += TotalMoneyEquips(equips);
            var props = list.ToList().Where(m => m.type == (int)GoodsType.TYPE_PROP).ToList();
            if (props.Any()) money += TotalMoneyProps(props);

            user.coin = tg_user.IsCoinMax(user.coin, money);
            try { user.Update(); } //处理用户的数据
            catch { return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR)); }
            Common.GetInstance().RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);

            session.Player.Bag.Surplus += list.ToList().Count();
            session.Player.Bag.BagIsFull = false;

            log.BagInsertLog(list, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL_BULK, money); //日志记录
            var model = string.Format("{0}", "出售道具:" + string.Join("_", list.ToList().Select(m => m.id)));
            (new Log()).WriteLog(user.id, (int)LogType.Delete, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL, model);
            var temp = string.Format("{0}_{1}_{2}", "原:" + my, "获:" + money, "现:" + user.coin);
            (new Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SELL, temp);

            return tg_bag.GetDeleteIds(ids) ?
                new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS)) :
                new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR));
        }

        /// <summary>道具出售总价</summary>
        private int TotalMoneyProps(IEnumerable<tg_bag> list)
        {
            return (from item in list let base_prop = Common.GetInstance().GetBaseProp(item.base_id) select Common.GetInstance().GetSellMoney(base_prop.price, item.count)).Sum();
        }

        /// <summary>装备出售总价</summary>
        private int TotalMoneyEquips(IEnumerable<tg_bag> list)
        {
            return list.Select(item => Common.GetInstance().GetBaseEquip(item.base_id)).Select(base_prop => Common.GetInstance().GetSellMoney(base_prop.sellPrice, 1)).Sum();
        }
    }
}
