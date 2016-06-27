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
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 修行加速
    /// </summary>
    public class TRAIN_ROLE_ACCELERATE : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_ACCELERATE", "修行加速");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var rid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            return CommandStart(rid, session);
        }

        /// <summary> 修行加速</summary>
        public ASObject CommandStart(Int64 rid, TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            var roletrain = tg_train_role.GetEntityByRid(rid);
            if (roletrain.state != (int)RoleTrainStatusType.TRAINING) //修炼状态验证
                return CommonHelper.ErrorResult((int)ResultType.TRAINROLE_LACK_TRAINING);

            if (roletrain.time < CurrentTime()) //时间验证
                return CommonHelper.ErrorResult((int)ResultType.TRAINROLE_TIME_FINISHED);

            var gold = user.gold;
            var cost = Consume(roletrain.time);
            user.gold = user.gold - cost;

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, cost, user.gold);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_ACCELERATE, logdata);

            if (user.gold < 0) //用户金钱验证
                return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);
            if (!tg_user.GetUserUpdate(user))
                return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            (new RoleTrain()).RewardsToUser(session, user, (int)GoodsType.TYPE_GOLD);

            var key = string.Format("{0}_{1}_{2}", (int)CDType.RoleTrain, user.id, rid);//为打断线程加入全局变量cd
            Variable.CD.AddOrUpdate(key, true, (k, v) => true);

            return new ASObject((new RoleTrain())
                    .BuilData((int)ResultType.SUCCESS, (new Share.Role()).BuildRole(rid))); //, null, roletrain)));
        }

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 修行加速消耗的元宝
        /// </summary>
        /// <param name="time">修行到达时间</param>
        /// <returns></returns>
        private int Consume(decimal time)
        {
            var minute = (double)((time - CurrentTime()) / 1000 / 60);
            if (minute <= 0) return 0;
            var rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "17005");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("minute", minute.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        private ASObject Error(int result)
        {

            return new ASObject((new RoleTrain()).BuilData(result, null));
        }
    }
}
