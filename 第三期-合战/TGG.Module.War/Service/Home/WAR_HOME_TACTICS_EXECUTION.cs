using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Home
{
    /// <summary>
    /// 执行策略
    /// </summary>
    public class WAR_HOME_TACTICS_EXECUTION : IDisposable
    {
        //private static WAR_HOME_TACTICS_EXECUTION _objInstance;

        ///// <summary>WAR_HOME_TACTICS_EXECUTION单体模式</summary>
        //public static WAR_HOME_TACTICS_EXECUTION GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_HOME_TACTICS_EXECUTION());
        //}
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_HOME_TACTICS_EXECUTION()
        {
            Dispose();
        }
    
        #endregion

        /// <summary> 执行策略 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var _type = GetData(data, "type");
            var _ids = GetData(data, "ids");
            var _time = GetData(data, "time");
            if (_type == null || _ids == null || _time == null) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

            var money = 0;
            var power = 3;
            var type = Convert.ToInt32(_type);
            var ids = (_ids as object[]).Select(Convert.ToInt32).ToList();
            var base_list = Variable.BASE_CELUE.Where(m => m.type == type);
            var bases = ids.Select(id => base_list.FirstOrDefault(m => m.id == id)).ToList();
            if (bases.Contains(null)) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            foreach (var temp in bases)
            {
                power -= temp.power;
                money += temp.money;
            }

            var time = Convert.ToInt32(_time);
            var number = GetRuleValue(time);
            money = money * number;

            var user = session.Player.User.CloneEntity();
            if (user.coin < money) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_COIN_ERROR);
            if (power < 0) return CommonHelper.ErrorResult(ResultType.WAR_NOT_POWER);

            var t = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var _t = time * 60 * 60 * 1000;
            var model = tg_war_home_tactics.GetEntityByUserid(user.id) ?? new tg_war_home_tactics { user_id = user.id };
            if (model.end_time > t) return CommonHelper.ErrorResult(ResultType.WAR_NOT_TIME_ERROR);

            model.type = type;
            model.count = power;
            model.money = money;
            model.effect = string.Join("_", ids);
            model.end_time = t + _t;
            model.total_time = time;
            model.Save();

            var coin = user.coin;
            user.coin -= money;
            user.Update();
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, user);
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, money, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_HOME_TACTICS_EXECUTION, "合战", "执行策略", "金钱", (int)GoodsType.TYPE_COIN, money, user.coin, logdata);
            
            session.Player.User = user;
            PushOk(user.id, model.end_time);
            return Common.GetInstance().BuildData(model);
        }

        private int GetRuleValue(int number)
        {
            string id;
            switch (number)
            {
                case 1: { id = "32059"; break; }
                case 2: { id = "32060"; break; }
                case 3: { id = "32061"; break; }
                default: { return 0; }
            }
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return rule == null ? 0 : Convert.ToInt32(rule.value);
        }

        /// <summary> 推送策略过期 </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        private void PushOk(Int64 userid, Int64 endTime)
        {
            var key = (new Share.War()).GetKey(endTime, userid);
            var t = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var time = endTime - t;
#if DEBUG
            time = 10000;
#endif

            Variable.CD.AddOrUpdate(key, false, (k, v) => false);
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(m =>
            {
                SpinWait.SpinUntil(() =>
                {
                    if (Variable.CD.ContainsKey(key))
                    {
                        var b = Variable.CD[key];
                        if (!b) return false;
                        return true;
                    }
                    token.Cancel();
                    return true;
                }, Convert.ToInt32(m));
            }, time, token.Token)
             .ContinueWith((m, n) =>
                {
                    (new Share.War()).RemoveCD(key);
                    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                    var session = Variable.OnlinePlayer[userid] as TGGSession;
                    var data = CommonHelper.SuccessResult();
                    var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_WAR_TACTICS_OK, (int)ResponseType.TYPE_SUCCESS, data);
                    session.SendData(pv);
                    token.Cancel();
                }, userid, token.Token);
        }

        private static object GetData(ASObject data, string key)
        {
            return data.ContainsKey(key) ? data[key] : null;
        }
    }
}
