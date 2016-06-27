using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 调查
    /// </summary>
    public class BUSINESS_PRICE_INFO : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_PRICE_INFO", "调查");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var tingids = data.FirstOrDefault(m => m.Key == "id").Value as object[];
            if (tingids == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var ids = tingids.Select(Convert.ToInt32).ToList();
            return Logic(ids, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(List<Int32> ids, TGGSession session)
        {
            var userinfo = session.Player.User.CloneEntity();

            if (!ids.Any()) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);    //验证前端信息

            var cost = 0;
            foreach (var item in ids)           //调查町需要消耗用户资源
            {
                var baseting = Variable.BASE_TING.FirstOrDefault(m => m.id == item);
                if (baseting == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                cost += baseting.lookCost;
            }
            var coin = userinfo.coin;
            userinfo.coin = userinfo.coin - cost;   //用户资源处理
            if (userinfo.coin < 0) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR);     //验证用户资源
            tg_user.Update(userinfo);
            //向用户推送资源更新
            session.Player.User = userinfo;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);

            TingInfoUpdate(userinfo.id, ids);         //处理町信息

            //记录金钱花费日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "C", coin, cost, userinfo.coin);
            (new Share.Log()).WriteLog(userinfo.id, (int)LogType.Use, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_PRICE_INFO, logdata);

            return CommonHelper.ErrorResult((int)ResultType.SUCCESS);
        }

        /// <summary>处理町集合信息</summary>
        private void TingInfoUpdate(Int64 userid, IEnumerable<int> tingids)
        {
            foreach (var item in tingids)
            {
                var baseting = Variable.BASE_TING.FirstOrDefault(m => m.id == item);
                var ting_enter = tg_user_ting.GetEntityByUserIdTingId(userid, item);

                if (baseting == null) continue;
                if (ting_enter == null || ting_enter.id == 0)
                {
                    var ting = new tg_user_ting()
                    {
                        user_id = userid,
                        ting_id = item,
                        state = (int)CityVisitType.VISIT,
                        area_id = baseting.areaId
                    };
                    ting.Insert();
                    InsertGoods(item, userid);  //插入tg_goods_item货物信息
                }
                else
                {
                    var num = GetBaseGoodsNum(ting_enter.ting_id);
                    //这个町还没有货物 插入货物
                    if (tg_goods_item.GetFindByTingIdUserId(ting_enter.ting_id, ting_enter.user_id) < num)
                        InsertGoods(ting_enter.ting_id, userid);
                    if (ting_enter.state == (int)CityVisitType.VISIT)       //町状态验证
                        return;

                    ting_enter.state = (int)CityVisitType.VISIT;            //更新町为已访问状态
                    ting_enter.Update();
                }
            }
        }


        /// <summary>添加货物信息</summary>
        private void InsertGoods(int tingid, Int64 userid)
        {
            var goods_string = GetBaseGoods(tingid);
            if (string.IsNullOrEmpty(goods_string)) return;

            //查询当前町基表货物信息 
            var listid = GetBaseGoodsList(goods_string);
            var _list = tg_goods_item.GetFindByGoodsIds(userid, tingid, goods_string).ToList();

            foreach (var id in listid)
            {
                var _bid = Convert.ToInt32(id);
                if (_list.Any())
                {
                    var temp = _list.Count(m => m.goods_id == _bid);
                    if (temp > 0) continue;
                }
                var basegood = Variable.BASE_GOODS.FirstOrDefault(m => m.id == _bid);
                var model = ConvertBaseGood(basegood, tingid, userid);
                model.Insert();
            }
        }

        /// <summary>获取货物id集合</summary>
        private List<int> GetBaseGoodsList(string goods)
        {
            var list = goods.Split(',').ToList();
            return list.Select(m => Convert.ToInt32(m)).ToList();
        }

        /// <summary>货物数量</summary>
        private int GetBaseGoodsNum(int tingid)
        {
            var goods_string = GetBaseGoods(tingid);
            return string.IsNullOrEmpty(goods_string) ? 0 : GetBaseGoodsList(goods_string).Count;
        }

        /// <summary>查询当前町基表货物信息</summary>
        private string GetBaseGoods(int tingid)
        {
            var baseting = Variable.BASE_TING.FirstOrDefault(m => m.id == tingid);
            if (baseting == null || baseting.goods == null) return null;
            return baseting.goods;

        }

        /// <summary>转为Entity货物</summary>
        private tg_goods_item ConvertBaseGood(BaseGoods good, int tingid, Int64 userid)
        {
            return new tg_goods_item
            {
                user_id = userid,
                ting_id = tingid,
                goods_id = good.id,
                number = good.sum,
                number_max = good.sum,
            };
        }

    }
}
