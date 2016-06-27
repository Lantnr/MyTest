using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.XML;

namespace TGGUnitTest
{
    [TestClass]
    public class UnitTest_XLJ
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new NEval().Eval("(75-1)+70");
            Console.WriteLine(a);
        }

        [TestMethod]
        public void TestWarRole()
        {
            Util.Init();//加载基础实体
            var vo = new List<view_war_role>();
            var user_id = 1;
            var station = 10008;
            var list = view_war_role.GetFindRole(user_id, station);
            foreach (var item in list)
            {
                var entity = item;
                if (entity.type == 1) //备大将
                {
                    //
                    var ide = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == (int)VocationType.Roles);
                    entity.role_id = Convert.ToInt32(entity.rid);
                    entity.role_identity = ide != null ? ide.soldier : 1038;
                    entity.role_state = 5;
                    entity.power = 0;
                    var role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == entity.role_id);
                    entity.player_name = role != null ? role.name : "--";
                }
                else
                {
                    if (item.role_state != 1) //不是主角武将
                    {
                        var role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == entity.role_id);
                        entity.player_name = role != null ? role.name : "--";

                    }
                }
                vo.Add(entity);
            }
            Console.WriteLine(vo.Count);
        }


        [TestMethod]
        public void TestWarGoods()
        {
            Util.Init();//加载基础实体
            (new TGG.Share.War()).RefreshWarGoods();
            var station = 1000024;
            //var list = Variable.WARGOODS.Where(m => m.city_id == station).ToList();
            var list = Variable.WARGOODS.Values.Where(m => m.city_id == station).ToList();

            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestWarGoodsBuy()
        {
            Util.Init();//加载基础实体
            (new TGG.Share.War()).RefreshWarGoods();


            var goods_id = 1;
            var goods_count = 1000;
            var station = 1000024;//session.War.CityId;

            var goods = Variable.WARGOODS.Values.FirstOrDefault(m => m.city_id == station && m.goods_id == goods_id);
            //if (goods == null) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            //判断资源消耗
            //if (goods_count > goods.count) return CommonHelper.ErrorResult((int)ResultType.WAR_GOODS_COUNT_ERROR);
            //var city = session.War.City;
            //var res_cost = goods_count * goods.goods_buy_price;
            //var temp = city.res_funds - res_cost;
            //if (temp < 0) return CommonHelper.ErrorResult((int)ResultType.WAR_RES_FUNDS_ERROR);

            //city.res_funds = temp;
            //session.War.City = city;
            //资源更新推送
            //tg_war_city.UpdateByFunds(city);
            //Common.GetInstance().SendCity(city.base_id, city.user_id);
            //数量更新

            var g = goods.CloneEntity();

            var _c = g.count - goods_count;
            g.count = _c;
            var key = string.Format("{0}_{1}", g.city_id, g.goods_id);
            Variable.WARGOODS.AddOrUpdate(key, g, (k, v) => g);

            var goods_01 = Variable.WARGOODS.Values.FirstOrDefault(m => m.city_id == station && m.goods_id == goods_id);

            Console.WriteLine();
        }


        [TestMethod]
        public void TestWarGoodsRefresh()
        {
            Util.Init();//加载基础实体
            (new TGG.Share.War()).RefreshWarGoods();


            var station = 1000024;//session.War.CityId;

            var list = Variable.WARGOODS.Values.Where(m => m.city_id == station);
            var test = list.ToList();
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
            var list01 = Variable.WARGOODS.Values.Where(m => m.city_id == station);

            Console.WriteLine();
        }

    }
}
