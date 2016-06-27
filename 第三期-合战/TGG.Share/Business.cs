using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Share
{
    /// <summary>
    /// 跑商共享类
    /// </summary>
    public partial class Business : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        private readonly object syncRoot = new object();

        /// <summary>加载全局货物价格</summary>
        public void InitGoods()
        {
            Variable.GOODS.Clear();
            var count_ting = Variable.BASE_TING.Count;
            var count_goods = Variable.BASE_GOODS.Count;
            for (var i = 0; i < count_ting; i++)
            {
                var goods = new GlobalGoods().CloneEntity();
                var listgoodsid = Variable.BASE_TING[i].goods.Split(',').Select(int.Parse).ToList(); //町中货物集合
                goods.ting_id = Variable.BASE_TING[i].id;
                for (var j = 0; j < count_goods; j++)
                {
                    lock (syncRoot)
                    {
                        var newgoods = goods.CloneEntity();
                        newgoods.goods_id = Variable.BASE_GOODS[j].id;
                        //货物价格表中得到浮动的数据
                        var goodsprice =
                            Variable.BASE_GOODSPRICE.FirstOrDefault(
                                q => q.areaId == Variable.BASE_TING[i].areaId && q.goodsId == Variable.BASE_GOODS[j].id);
                        //随机生成货物买、卖的价格
                        if (listgoodsid.Contains(Variable.BASE_GOODS[j].id))
                        {
                            if (goodsprice != null)
                                newgoods.goods_buy_price = RNG.Next(goodsprice.min_buy, goodsprice.max_buy);
                        }
                        if (goodsprice != null)
                            newgoods.goods_sell_price = RNG.Next(goodsprice.min_sell, goodsprice.max_sell);
                        Variable.GOODS.Add(newgoods);
                    }
                }
            }

            tg_user_ting.GetStateUpdate();
            tg_goods_item.GetGoodsNumber();
        }

        /// <summary>货物刷新</summary>
        public void RefreshGoods()
        {
            var count_ting = Variable.BASE_TING.Count;
            var count_goods = Variable.BASE_GOODS.Count;
            for (var i = 0; i < count_ting; i++)
            {
                var goods = new GlobalGoods().CloneEntity();
                var listgoodsid = Variable.BASE_TING[i].goods.Split(',').Select(int.Parse).ToList(); //町中货物集合
                goods.ting_id = Variable.BASE_TING[i].id;
                for (var j = 0; j < count_goods; j++)
                {
                    lock (syncRoot)
                    {
                        var newgoods = goods.CloneEntity();
                        newgoods.goods_id = Variable.BASE_GOODS[j].id;
                        //货物价格表中得到浮动的数据
                        var goodsprice =
                            Variable.BASE_GOODSPRICE.FirstOrDefault(
                                q => q.areaId == Variable.BASE_TING[i].areaId && q.goodsId == Variable.BASE_GOODS[j].id);
                        //随机生成货物买、卖的价格
                        if (listgoodsid.Contains(Variable.BASE_GOODS[j].id))
                        {
                            if (goodsprice != null)
                                newgoods.goods_buy_price = RNG.Next(goodsprice.min_buy, goodsprice.max_buy);
                        }
                        if (goodsprice != null)
                            newgoods.goods_sell_price = RNG.Next(goodsprice.min_sell, goodsprice.max_sell);
                        var globle =
                            Variable.GOODS.FirstOrDefault(
                                q => q.ting_id == newgoods.ting_id && q.goods_id == newgoods.goods_id);
                        if (globle == null) continue;
                        globle.goods_buy_price = newgoods.goods_buy_price;
                        globle.goods_sell_price = newgoods.goods_sell_price;
                    }
                }
            }
            tg_user_ting.GetStateUpdate();
            tg_goods_item.GetGoodsNumber();
        }


        /// <summary>获取剩余讲价次数</summary>
        public int GetBargainCount(Player player)
        {
            try
            {
                if (player.Role.Kind == null) return 0;
                if (player.Role.Kind.base_charm <= 0) return 0;
                var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3010"); //获取基表数据
                if (baserule == null) return 0;
                var temp = baserule.value;
                var charm = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, player.Role.Kind), 2);
                temp = temp.Replace("agile", charm.ToString("0.00")); //魅力
                var express = CommonHelper.EvalExpress(temp);
                var maxcount = Convert.ToInt32(express);
                return maxcount - player.UserExtend.bargain_count;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return 0;
            }
        }

        #region 买卖类型任务--（商人）

        /// <summary>出售货物时检查买卖类型任务</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="order">货物实体</param>
        public void CheckTaskSell(Int64 user_id, BusinessOrder order)
        {
            var _task = tg_task.GetUnBusinessTask(user_id);
            if (_task == null) return;

            //解析
            var entity = (new TGTask()).Ananalyze(_task.task_step_data);
            if (entity == null) return;
            //判断是否是任务出售町
            if (entity.ting != order.current_ting_base_id || entity.goods != order.goods_id) return;
            var _c = order.count + entity.count;
            if (entity.total > _c)
                entity.count = _c;
            else
            {
                entity.count = entity.total;
                _task.task_state = (int)TaskStateType.TYPE_REWARD;
            }

            var step_data = entity.GetBusinessStepData();
            _task.task_step_data = step_data;
            _task.Save();

            //任务推送
            (new TGTask()).AdvancedTaskPush(user_id, _task);
        }


        /// <summary>出售货物时检查买卖类型任务</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="order">货物实体</param>
        public void CheckWorkTaskSell(Int64 user_id, BusinessOrder order)
        {
            var _task = tg_task.GetWorkUnBusinessTask(user_id);
            if (_task == null) return;

            //解析
            var entity = (new Share.TGTask()).Ananalyze(_task.task_step_data);
            if (entity == null) return;
            //判断是否是任务出售町
            if (entity.ting != order.current_ting_base_id || entity.goods != order.goods_id) return;
            var _c = order.count + entity.count;
            if (entity.total > _c)
                entity.count = _c;
            else
            {
                entity.count = entity.total;
                _task.task_state = (int)TaskStateType.TYPE_REWARD;
            }

            var step_data = entity.GetBusinessStepData();
            _task.task_step_data = step_data;
            _task.Save();

            //任务推送
            (new Work()).AdvancedWorkPush(user_id, _task);
        }

        #endregion

        #region 筹措资金

        /// <summary>检查筹措资金任务</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="coin">跑商获取金钱</param>
        public void CheckTaskRaise(Int64 user_id, Int64 coin)
        {
            var _task = tg_task.GetUnRaiseTask(user_id);
            if (_task == null) return;

            //解析
            var entity = (new TGTask()).Ananalyze(_task.task_step_data);
            if (entity == null) return;
            Int64 _c = coin + entity.count;
            if (entity.total > _c)
                entity.count = Convert.ToInt32(_c);
            else
            {
                entity.count = entity.total;
                _task.task_state = (int)TaskStateType.TYPE_REWARD;
            }

            var step_data = entity.GetRaiseStepData();
            _task.task_step_data = step_data;
            _task.Save();

            //任务推送
            (new TGTask()).AdvancedTaskPush(user_id, _task);
        }

        /// <summary>检查筹措资金任务</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="coin">跑商获取金钱</param>
        public void CheckWorkTaskRaise(Int64 user_id, Int64 coin)
        {
            var _task = tg_task.GetWorkUnRaiseTask(user_id);
            if (_task == null) return;

            //解析
            var entity = (new TGTask()).Ananalyze(_task.task_step_data);
            if (entity == null) return;
            var _c = coin + entity.count;
            if (entity.total > _c)
                entity.count = Convert.ToInt32(_c);
            else
            {
                entity.count = entity.total;
                _task.task_state = (int)TaskStateType.TYPE_REWARD;
            }

            var step_data = entity.GetRaiseStepData();
            _task.task_step_data = step_data;
            _task.Save();

            //任务推送
            (new Work()).AdvancedWorkPush(user_id, _task);
        }

        #endregion

        /// <summary>是否是自己所属商圈</summary>
        /// <returns></returns>
        public bool IsArea(int ting, List<tg_user_area> areas)
        {
            var _area = Variable.BASE_TING.FirstOrDefault(m => m.id == ting);
            if (_area == null) return false;
            var count = areas.Count(m => m.area_id == _area.areaId);
            return count > 0;
        }

    }
}
