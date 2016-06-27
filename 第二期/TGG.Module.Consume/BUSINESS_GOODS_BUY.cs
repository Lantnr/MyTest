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
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.Share.Event;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 货物交易
    /// </summary>
    public class BUSINESS_GOODS_BUY : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_GOODS_BUY", "交易");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var count = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value);
            return Logic(type, count, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(Int32 type, Int32 count, TGGSession session)
        {
            //验证商圈
            if (!(new Share.Business()).IsArea(session.Player.Order.current_ting_base_id, session.Player.BusinessArea))
            {
                return CommonHelper.ErrorResult(ResultType.BUSINESS_TIME_OPEN_ERROR);
            }

            var player = session.Player.CloneEntity();

            var gb = tg_goods_business.GetByGoodsidAndUserid(player.User.id, player.Order.goods_id, player.Order.car_mainid);

            var sell_max = gb == null ? 0 : gb.goods_number;//卖货物的最大数量
            var goods_max = player.Order.buy_count_max; //买货物的最大数量

            if (!CheckCount(count, goods_max, type, sell_max))//货物数量验证
                return CommonHelper.ErrorResult((int)ResultType.BUSINESS_GOODS_COUNT_FULL);
            player.Order.count = count;
#if DEBUG
            DisplayGlobal.log.Write("货物交易货物数量:" + count);
#endif
            //买货物
            if (type == 0)
            {
                if (!CheckPacket(player.Order.car_mainid, gb, count)) //验证格子数
                    return CommonHelper.ErrorResult((int)ResultType.BUSINESS_GRID_LACK);

                if (!CheckBuyMoney(session, player)) //验证金钱
                    return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR);

                gb = BuyGoodsSave(gb, player.Order, player.User.id);

                var _count = player.Order.buy_count_max - player.Order.count;
                tg_goods_item.GetEntityUpdate(player.Order.ting_base_id, player.Order.goods_id, _count, player.User.id); //町货物更新

                return new ASObject(BuildData((int)ResultType.SUCCESS, CovertToVo(gb, player.Order.sell_price_ok, player.Order.buy_price_ok))); ;
            }

            //卖货物
            if (gb == null) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_GOODS_LACK); //货物存在验证
            if (!CheckSellMoney(session, player)) //验证金钱消耗
                return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            if (!SellGoodsSave(count, ref gb)) //保存货物数据
                return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            //任务判断
            (new Share.Business()).CheckTaskSell(player.User.id, player.Order);
            (new Share.Business()).CheckWorkTaskSell(player.User.id, player.Order);

            return new ASObject(BuildData((int)ResultType.SUCCESS, CovertToVo(gb, player.Order.sell_price_ok, gb.price)));
        }

        /// <summary>马车格子数验证</summary>
        private bool CheckPacket(Int64 cid, tg_goods_business goods, int count)
        {
            var packetnum = 0;
            var businessgoods = tg_goods_business.GetListEntityByCid(cid); //马车上已有货物
            if (businessgoods == null) return false;
            if (goods != null)
            {
                var newgoodsinfo = businessgoods.FirstOrDefault(q => q.goods_id == goods.goods_id);
                if (newgoodsinfo != null)
                    newgoodsinfo.goods_number += count; //货物叠加
            }
            else
                packetnum++; //新货物则格子+1
            foreach (var item in businessgoods)
            {
                var mun = RuleConvert.GetPacket();
                packetnum += Convert.ToInt32(Math.Ceiling((double)item.goods_number / mun));
            }
            var maxnum = tg_car.GetEntityById(cid).packet;
            return packetnum <= maxnum;
        }

        /// <summary>货物数量验证</summary>
        private bool CheckCount(int number, int goodsmax, int type, int sellmax)
        {
            if (type == 0)
                return number <= goodsmax;
            return number <= sellmax;
        }

        /// <summary>买货物金钱验证</summary>
        private bool CheckBuyMoney(TGGSession session, Player player)
        {
            var org = session.Player.User.coin;
            var cost = player.Order.GetBuy();//购买税
            var _coin = player.User.coin - cost;
            if (_coin < 0) return false;
            player.User.coin = _coin;
            if (tg_user.Update(player.User) == 0) return false;

            //日志
            var data = string.Format("{0}_{1}_{2}_{3}", "B", org, cost, _coin);
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Use, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_GOODS_BUY, data);

            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
            return true;
        }

        /// <summary>保存购买的货物数据</summary>
        private tg_goods_business BuyGoodsSave(tg_goods_business gb, BusinessOrder order, Int64 userid)
        {
            if (gb == null) //之前没有购买此货物
            {
                gb = BuildEntity(order, order.count, userid);
                gb.Insert();
            }
            else
            {
                var totalmoney = gb.goods_number * gb.price + order.GetTotalBuy(); //买货物总消费
                gb.goods_number += order.count;
                gb.price = Convert.ToInt32(totalmoney / gb.goods_number); //买货物的价格为平均价
                gb.Update();
            }
            return gb;
        }

        /// <summary>保存买的货物数据</summary>
        private bool SellGoodsSave(int count, ref tg_goods_business goodsbusiness)
        {
            if (goodsbusiness == null) return false;

            if (goodsbusiness.goods_number == count) //卖完所有
            {
                tg_goods_business.Delete(new String[] { tg_goods_business._.id }, new Object[] { goodsbusiness.id });
                goodsbusiness.goods_number = 0;
            }
            else
            {
                goodsbusiness.goods_number -= count;
                if (tg_goods_business.Update(goodsbusiness) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>卖货物金钱验证</summary>
        private bool CheckSellMoney(TGGSession session, Player player)
        {
            var org = session.Player.User.coin;
            Int64 cost = player.Order.GetSell();//购买税
            var _coin = player.User.coin + cost;
            player.User.coin = _coin;
            if (tg_user.Update(player.User) == 0) return false;

            //日志
            var data = string.Format("{0}_{1}_{2}_{3}", "S", org, cost, _coin);
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Get, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_GOODS_BUY, data);

            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);

            //检查筹措资金任务
            (new Share.Business()).CheckTaskRaise(player.User.id, cost);
            (new Share.Business()).CheckWorkTaskRaise(player.User.id, cost);

            return true;
        }

        /// <summary>组装跑商货物实体</summary>
        private tg_goods_business BuildEntity(BusinessOrder order, int numb, Int64 userid)
        {
            return new tg_goods_business()
            {
                goods_id = order.goods_id,
                cid = order.car_mainid,
                goods_number = numb,
                price = order.buy_price_ok,
                ting_id = order.ting_base_id,
                user_id = userid
            };
        }

        /// <summary>实体装换成vo</summary>
        private BusinessGoodsVo CovertToVo(tg_goods_business entitydata, int sellprice, int buyprice)
        {
            return new BusinessGoodsVo()
            {
                priceSell = sellprice,
                id = entitydata.id,
                baseId = (int)entitydata.goods_id,
                priceBuy = buyprice,
                count = entitydata.goods_number
            };
        }

        /// <summary>组装数据</summary>
        private Dictionary<string, object> BuildData(int result, BusinessGoodsVo goodsvo)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",result },
                {"goods",goodsvo ?? null },
            };
            return dic;
        }

    }
}
