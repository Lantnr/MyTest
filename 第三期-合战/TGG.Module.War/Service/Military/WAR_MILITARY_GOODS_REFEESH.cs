using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 军需品刷新
    /// arlen
    /// </summary>
    public class WAR_MILITARY_GOODS_REFEESH : IDisposable
    {
        //private static WAR_MILITARY_GOODS_REFEESH _objInstance;

        ///// <summary>WAR_MILITARY_GOODS_REFEESH单体模式</summary>
        //public static WAR_MILITARY_GOODS_REFEESH GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_GOODS_REFEESH());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 查看开始指令处理 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_MILITARY_GOODS_REFEESH", "军需品刷新");
#endif                       
            //花费10元宝
            var user = session.Player.User;
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32038");
            if (rule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var cost = Convert.ToInt32(rule.value);
            var temp = user.gold - cost;
            if (temp < 0) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);

            var gold = user.gold;
            user.gold = temp;
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            var logdata = "Gold_" + gold + "_" + cost + "_" + user.gold;
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_MILITARY_GOODS_REFEESH, "合战", "军需品刷新", "元宝", (int)GoodsType.TYPE_GOLD, cost, Convert.ToInt64(user.gold), logdata);

            var station = session.Player.War.PlayerInCityId;//1000024;
            var list = Variable.WARGOODS.Values.Where(m => m.city_id == station);
            var list_new = RefreshGoods(list);

            return list_new.Count <= 0 ? CommonHelper.ErrorResult((int)ResultType.DATA_NULL_ERROR) :
                new ASObject(BuildData((int)ResultType.SUCCESS, list_new));
        }

        /// <summary>仓库物品信息集合</summary>
        private List<ASObject> ToWarGoods(IEnumerable<GlobalWarGoods> list)
        {
            var war_goods = new List<ASObject>();
            war_goods.AddRange(list.Select(item => AMFConvert.ToASObject(EntityToVo.ToWarGoodsVo(item))));
            return war_goods;
        }

        private Dictionary<String, Object> BuildData(int result, List<GlobalWarGoods> list)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "goods", list.Any() ? ToWarGoods(list) : null }, 
            };
            return dic;
        }

        private List<GlobalWarGoods> RefreshGoods(IEnumerable<GlobalWarGoods> list)
        {
            var list_new = new List<GlobalWarGoods>();

            foreach (var item in list)
            {
                var newgoods = item.CloneEntity();
                //货物价格表中得到浮动的数据
                var price = Variable.BASE_CITYRESOURCE.FirstOrDefault(q => q.id == item.goods_id);

                //随机生成货物买、卖的价格
                if (price != null)
                {
                    newgoods.count = price.count;
                    newgoods.goods_buy_price = RNG.NextDouble(price.min_buy, price.max_buy, 1);
                }
                var key = string.Format("{0}_{1}", newgoods.city_id, newgoods.goods_id);
                Variable.WARGOODS.AddOrUpdate(key, newgoods, (k, v) => newgoods);
                list_new.Add(newgoods);
            }

            return list_new;
        }


    }
}
