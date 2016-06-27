using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using tg_role = TGG.Core.Entity.tg_role;
using NewLife.Log;
using TGG.Core.Entity;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 讲价
    /// </summary>
    public class BUSINESS_GOODS_PRICE
    {
        private static BUSINESS_GOODS_PRICE ObjInstance;

        /// <summary>BUSINESS_GOODS_PRICE单体模式</summary>
        public static BUSINESS_GOODS_PRICE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_GOODS_PRICE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_GOODS_PRICE", "讲价");
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var count = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value);

            var player = session.Player.CloneEntity();
            player.Order.count = count;
            player.Order.isbargain = true;

            var temp_count = (new Share.Business()).GetBargainCount(player);
            if (temp_count <= 0)
                return new ASObject(BuildData((int)ResultType.BUSINESS_BARGAIN_LACK, 0, temp_count > 0 ? temp_count : 0, 0));

            var success = false;
            var cion = player.User.coin;

            if (type == (int)BusinessType.Buy) //买
            {
                var temp = player.Order.GetBuy();
                if (cion < temp) return new ASObject(BuildData((int)ResultType.BASE_PLAYER_COIN_ERROR, player.Order.buy_price_ok, temp_count, 0));

                var _price_buy = GetBargainPrice(player.UserExtend, player.Role, player.Order.GetTotalBuy(), (int)BusinessType.Buy, ref success);
                player.Order.buy_bargain = _price_buy;
                player.UserExtend.bargain_count += 1;
#if DEBUG
                player.UserExtend.bargain_count = 0;
#endif
                //var _price_buy = player.Order.buy_price_ok + Common.GetInstance().RuleData(player.User.player_vocation, player.Order.buy_price_ok);
                try
                {
                    player.UserExtend.Save();
                    temp_count -= 1;
                }
                catch { return new ASObject(BuildData((int)ResultType.DATABASE_ERROR, _price_buy, temp_count > 0 ? temp_count : 0, success ? 1 : 0)); }
                session.Player = player;
                return new ASObject(BuildData((int)ResultType.SUCCESS, player.Order.GetBuy(), temp_count > 0 ? temp_count : 0, success ? 1 : 0));
            }

            //卖
            var _price_sell = GetBargainPrice(player.UserExtend, player.Role, player.Order.GetTotalSell(), (int)BusinessType.Sell, ref success);
            player.Order.sell_bargain = _price_sell;

            player.UserExtend.bargain_count += 1;
#if DEBUG
            player.UserExtend.bargain_count = 0;
#endif
            //var _price_sell = player.Order.sell_price_ok - Common.GetInstance().RuleData(player.User.player_vocation, player.Order.sell_price_ok);
            try
            {
                player.UserExtend.Save();
                temp_count -= 1;
            }
            catch { return new ASObject(BuildData((int)ResultType.SUCCESS, _price_sell, temp_count > 0 ? temp_count : 0, success ? 1 : 0)); }
            session.Player = player;
            return new ASObject(BuildData((int)ResultType.SUCCESS, player.Order.GetSell(), temp_count > 0 ? temp_count : 0, success ? 1 : 0));
        }

        /// <summary>获取讲价后的价格</summary>
        private Int64 GetBargainPrice(tg_user_extend extend, RoleItem role, Int64 total, int type, ref bool success)
        {
            //根据概率来计算讨价是否成功
            var prob = GetEloquence(role.LifeSkill.sub_eloquence, role.LifeSkill.sub_eloquence_level);
            var rs = new RandomSingle();
            success = rs.IsTrue(prob.Eloquence);
#if DEBUG
            success = true;
#endif
            if (!success) return total;

            //判断用户家臣称号是否达成---LZH
            (new Share.Title()).IsTitleAcquire(extend, (int)TitleGetType.BARGARN_SUCCUSS);   //判断称号信息

            var calculate = GetCalculate(role.LifeSkill.sub_calculate, role.LifeSkill.sub_calculate_level);
            if (type == (int)BusinessType.Buy)
                total -= Convert.ToInt32(total * calculate.Buy / 100);
            else
                total += Convert.ToInt32(total * calculate.Sell / 100);
            return total;
        }

        /// <summary>组装数据</summary>
        private Dictionary<string, object> BuildData(int result, Int64 total, int count, int success)
        {
            return new Dictionary<string, object>()
            {
                {"result",result},
                {"price",total},
                {"count",count},
                {"success",success},
            };
        }

        /// <summary>获取算术相关数据</summary>
        public CalculateItem GetCalculate(int calculate, int level)
        {
            var cal = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == calculate && m.level == level);
            if (cal == null) return new CalculateItem(0, 0);
            var temp = cal.effect.Split('|');
            int buy = 0, sell = 0;
            foreach (var item in temp)
            {
                var v = item.Split('_');
                if (v.Length < 4) continue;
                var t = Convert.ToInt32(v[0]);
                switch (t)
                {
                    case (int)LifeSkillEffectType.INCREASE_BARGAIN_SELL_PRICE: sell = (int)(Convert.ToDouble(v[3])); break;
                    case (int)LifeSkillEffectType.REDUCE_BARGAIN_BUY_PRICE: buy = (int)(Convert.ToDouble(v[3])); break;
                }
            }
#if DEBUG
            XTrace.WriteLine("技能:{0} 等级:{1} 效果:{2} 购买货物概率:{3} 出售卖货物概率:{4}", calculate, level, cal.effect, buy, sell);
#endif
            return new CalculateItem(buy, sell);
        }

        /// <summary>获取辩才相关数据</summary>
        public EloquenceItem GetEloquence(int _eloquence, int level)
        {
            var elo = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == _eloquence && m.level == level);
            if (elo == null) return new EloquenceItem(0, 0);
            var temp = elo.effect.Split('|');
            int wisdom = 0, eloquence = 0;
            foreach (var item in temp)
            {
                var v = item.Split('_');
                if (v.Length < 4) continue;
                var t = Convert.ToInt32(v[0]);
                switch (t)
                {
                    case (int)LifeSkillEffectType.INCREASE_BRAINS: wisdom = (int)Convert.ToDouble(v[3]); break;
                    case (int)LifeSkillEffectType.BARGAIN_SUCCESS_RATE: eloquence = (int)(Convert.ToDouble(v[3])); break;
                }
            }
#if DEBUG
            eloquence += 50;
            XTrace.WriteLine("技能:{0} 等级:{1} 效果:{2} 加成值:{3} 成功率:{4}", _eloquence, level, elo.effect, wisdom, eloquence);
#endif
            return new EloquenceItem(wisdom, eloquence);
        }

        /// <summary>算术实体对象</summary>
        public class CalculateItem
        {
            public CalculateItem(int buy, int sell) { Buy = buy; Sell = sell; }
            /// <summary>购买货物概率</summary>
            public int Buy { get; set; }
            /// <summary>出售卖货物概率</summary>
            public int Sell { get; set; }
        }

        /// <summary>辩才实体对象</summary>
        public class EloquenceItem
        {
            public EloquenceItem(int wisdom, int eloquence) { Wisdom = wisdom; Eloquence = eloquence; }
            /// <summary>智谋加成值</summary>
            public int Wisdom { get; set; }
            /// <summary>讲价成功率</summary>
            public int Eloquence { get; set; }
        }

    }
}
