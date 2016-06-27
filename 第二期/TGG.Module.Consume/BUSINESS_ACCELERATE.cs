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
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 跑商加速
    /// </summary>
    public class BUSINESS_ACCELERATE : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_ACCELERATE", "跑商加速");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var car_id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "carId").Value);//马车主键id
            return Logic(car_id, session);
        }

        /// <summary>加速逻辑操作</summary>
        /// <param name="carid">马车主键id</param>
        /// <param name="session">session</param>
        private ASObject Logic(Int64 carid, TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            var car = tg_car.FindByid(carid);
            if (car == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);     //验证数据库数据是否存在
            if (car.state != (int)CarStatusType.RUNNING) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_CAR_LACK_RUNNING);      //跑商马车状态验证
            var ticks = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (car.time < ticks) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_TIME_UNFINISHED); //跑商时间验证

            var gold = Consume(car.time);
            if (gold == 0) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            user.gold = user.gold - gold;
            if (user.gold < 0) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);//用户金钱验证

            var key = string.Format("{0}_{1}_{2}", (int)CDType.Businss, car.user_id, car.id);
            var b = Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            if (!b)
            {
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            }

            tg_user.Update(user);
            //推送并进行session记录
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
            car = tg_car.FindByid(carid);
            var goods = tg_goods_business.GetListEntityByCid(carid);   //马车上货物
            log.GoldInsertLog(gold, user.id, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_ACCELERATE); //玩家消费金币记录
            return new ASObject(BuildData((int)ResultType.SUCCESS, car, goods));
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, tg_car car, List<tg_goods_business> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {
                    "carVo",
                    EntityToVo.ToBusinessCarVo(car, list.Select(EntityToVo.ToBusinessGoodsVo).ToList())
                }
            };
            return new ASObject(dic);
        }

        /// <summary>用户加速消耗的元宝数计算</summary>
        /// <param name="time">跑商到达时间</param>
        /// <returns>需要消耗的元宝数</returns>
        private int Consume(decimal time)
        {
            var ticks = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var minute = Convert.ToDouble(((time - ticks) / 1000 / 60));
            var rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3005");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("minute", minute.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }
    }
}
