using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
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
    /// 
    /// </summary>
    public class BUSINESS_PACKET_BUY : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_PACKET_BUY", "增加货物格");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "carId").Value.ToString());
            var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value.ToString());
            return Logic(id, count, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(Int64 id, Int32 count, TGGSession session)
        {
            if (count == 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            var user = session.Player.User.CloneEntity();
            var car = tg_car.GetEntityById(id);   //根据马车主键id 查询当前马车信息
            if (car == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);     //验证数据库数据

            var lpackets = Variable.BASE_RULE.FirstOrDefault(m => m.id == "3006");       //马车格子上限
            var _packet = Variable.BASE_RULE.FirstOrDefault(m => m.id == "3009");        //默认开启格子数
            if (_packet == null || lpackets == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR); //验证基表数据
            if (car.packet >= Convert.ToInt32(lpackets.value)) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_PACKET_FULL);    //验证是否达到上限

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "3008");
            if (rule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR); //验证基表数据

            var costgold = CostGold(car, Convert.ToInt32(_packet.value), count, rule.value);
            if (costgold == 0) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);  //验证基表数据
            var gold = user.gold;
            if (user.gold < costgold) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);//验证玩家金钱
            user.gold = user.gold - costgold;

            car.packet = car.packet + count;
            if (car.Update() <= 0 || user.Update() <= 0) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            //向用户推送更新
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            //记录元宝花费日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "G", gold, costgold, user.gold);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_PACKET_BUY, logdata);
            log.GoldInsertLog(count, user.id, (int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_PACKET_BUY);    //玩家开启格子消费记录


            return new ASObject(PacketBuildData((int)ResultType.SUCCESS, car.packet));
        }

        /// <summary>增加马车格子数据组装</summary>
        private Dictionary<String, Object> PacketBuildData(int result, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result}, 
                {"count", count}
            };
            return dic;
        }

        /// <summary> 开启货物格消耗的金币 </summary>
        /// <param name="car">马车数据</param>
        /// <param name="_packet">基表默认开启格子数</param>
        /// <param name="count">开启数量</param>
        /// <param name="rule">计算公式</param>
        /// <returns>消耗金币数</returns>
        private int CostGold(tg_car car, int _packet, int count, string rule)
        {
            var total = 0;
            var buypacket = car.packet - _packet; //当前金币开启的格子数

            for (var i = 1; i <= count; i++)
            {
                buypacket += 1;
                var temp = rule;
                temp = temp.Replace("packet", buypacket.ToString("0.00"));
                var express = CommonHelper.EvalExpress(temp);
                total += Convert.ToInt32(express);
            }
            return total;
        }

    }
}
