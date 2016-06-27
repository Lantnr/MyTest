using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;
using TGG.Core.Entity;

namespace TGG.Share
{
    public partial class User : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region 物品更新接口


        /// <summary>奖励更新 API</summary>
        public void REWARDS_API(int goodsType, tg_user user)
        {
            CommandSend(user.id, RewardsASObject(goodsType, user, null, null, null));
        }
        /// <summary>奖励更新 API</summary>
        public void REWARDS_API(IEnumerable<int> goodsTypeList, tg_user user)
        {
            CommandSend(user.id, RewardsASObject(goodsTypeList, user, null, null, null));
        }
        /// <summary>奖励更新 API</summary>
        public void REWARDS_API(tg_user user, IEnumerable<int> goodsTypeList, List<RewardVo> list)
        {
            CommandSend(user.id, RewardsASObject(goodsTypeList, user, null, null, null, list));
        }

        /// <summary>Common奖励更新 API</summary>
        /// <param name="session">The session.</param>
        /// <param name="list">更新物品RewardVo集合</param>
        public void REWARDS_API(Int64 user_id, List<RewardVo> list)
        {
            CommandSend(user_id, RewardsASObject(list));
        }

        /// <summary>物品更新指令</summary>
        private void CommandSend(Int64 user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "REWARDS", "物品更新");
#endif
            var b = Variable.OnlinePlayer.ContainsKey(user_id);
            if (!b) return;
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;

            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = (int)ModuleNumber.USER,
                commandNumber = (int)UserCommand.REWARDS,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }


        #endregion

        #region 公共方法

        public bool UseCoin(TGGSession session, Int64 cost)
        {
            //var player = session.Player;
            //var _c
            //player.User.coin
            return false;
        }

        /// <summary>
        /// 防沉迷系统推送
        /// </summary>
        /// <param name="userid"></param>
        public void UserLoginTimeCheck()
        {
            foreach (var userid in Variable.OnlinePlayer.Keys)
            {
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) { continue; }
                if (session.Player.UserExtend.fcm == (int)FCMType.None) continue;
                var loginentity = tg_user_login_log.GetEntityByUserId(userid);
                var now = DateTime.Now.Ticks;
                var _time = (now - 621355968000000000) / 10000;
                var onlinetime = (now - loginentity.login_time) / 600000000; //玩家本次登陆在线时长
                var logintime = (loginentity.login_time_longer_day + onlinetime); //今日在线分钟数
                if (logintime <= 0) continue;
#if DEBUG
                if (logintime >= 1 && logintime < 2) { SendPv(userid, logintime * 60); continue; } //测试数据
                if (logintime >= 10 && logintime < 11) { SendPv(userid, logintime * 60); continue; } //测试数据
#endif
                if (logintime >= 60 && logintime < 61) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 120 && logintime < 121) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 180 && logintime < 181)
                {
                    SendPv(userid, 0);
                    loginentity.login_open_time = _time + 5 * 60 * 60 * 1000;
                    loginentity.Update();
                    UserLeave(userid);
                    return;
                }
                if (logintime >= 240 && logintime < 241) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 300 && logintime < 301) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 360 && logintime < 361)
                {
                    SendPv(userid, 0);
                    loginentity.login_open_time = _time + 5 * 60 * 60 * 1000;
                    loginentity.Update();
                    UserLeave(userid);
                    return;
                }

                if (logintime >= 420 && logintime < 421) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 480 && logintime < 481) { SendPv(userid, logintime * 60); continue; }
                if (logintime >= 540 && logintime < 541)
                {
                    SendPv(userid, 0);
                    loginentity.login_open_time = _time + 5 * 60 * 60 * 1000;
                    loginentity.Update();
                    UserLeave(userid);
                }
            }

        }

        /// <summary>
        /// 防沉迷期间登陆，玩家踢下线
        /// </summary>
        /// <param name="userid"></param>
        private void UserLeave(Int64 userid)
        {
            try
            {

                var token = new CancellationTokenSource();
                Task.Factory.StartNew(() => SpinWait.SpinUntil(() => false, 10 * 1000), token.Token)
                    .ContinueWith((m, n) =>
                    {
                        var user_id = Convert.ToInt64(n);
                        if (!Variable.OnlinePlayer.ContainsKey(user_id)) { token.Cancel(); return; }
                        var session = Variable.OnlinePlayer[user_id] as TGGSession;
                        if (session == null) { token.Cancel(); return; }
                        session.Close();
                        token.Cancel();
                    }, userid, token.Token);
                /* 
                var user_id = userid;
                if (!Variable.OnlinePlayer.ContainsKey(user_id)) { return; }
                var session = Variable.OnlinePlayer[user_id] as TGGSession;
                if (session == null) { return; }
                session.Close();
                 *  * */
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public tg_user_login_log GetFCM(Int64 user_id)
        {
            var entity = tg_user_login_log.GetEntityByUserId(user_id);
            if (entity == null) return null;
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (time < entity.login_open_time)
                UserLeave(user_id);

            return entity;
        }

        /// <summary>
        /// 推送防沉迷协议
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="time">onlineTime:Number 在线时间(s)，0 表示下线时间</param>
        private void SendPv(Int64 userid, Int64 time)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var dic = new Dictionary<string, object>()
            {
                {"onlineTime", time}
            };
            var aso = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.USER, (int)UserCommand.PUSH_ONLINE_TIME, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);


        }

        #endregion

        #region 私有方法

        /// <summary>构建奖励更新数据</summary>
        /// <param name="goodsType">物品类型</param>
        /// <param name="model">玩家</param>
        /// <param name="increases">新增物品对象集合</param>
        /// <param name="decreases">更新物品对象集合</param>
        /// <param name="delete">删除物品ID集合</param>
        private ASObject RewardsASObject(int goodsType, tg_user model, List<ASObject> increases, List<ASObject> decreases, List<double> delete)
        {
            var dic = new Dictionary<string, object>();
            var aso_list = new List<ASObject>
            {
                AMFConvert.ToASObject(BulidReward( goodsType,model, increases, decreases, delete))
            };
            dic.Add("rewards", aso_list);
            return new ASObject(dic);
        }

        /// <summary>构建奖励更新数据</summary>
        /// <param name="list">奖励物品Vo集合</param>
        private ASObject RewardsASObject(IEnumerable<RewardVo> list)
        {
            var dic = new Dictionary<string, object>();
            var aso_list = list.Select(AMFConvert.ToASObject).ToList();
            dic.Add("rewards", aso_list);
            return new ASObject(dic);
        }

        /// <summary>构建奖励更新数据</summary>
        /// <param name="list">物品类型集合</param>
        /// <param name="model">玩家</param>
        /// <param name="increases">新增物品对象集合</param>
        /// <param name="decreases">更新物品对象集合</param>
        /// <param name="delete">删除物品ID集合</param>
        private ASObject RewardsASObject(IEnumerable<int> list, tg_user model, List<ASObject> increases, List<ASObject> decreases, List<double> delete)
        {
            var dic = new Dictionary<string, object>();
            var aso_list = list.Select(item => AMFConvert.ToASObject(BulidReward(item, model, increases, decreases, delete))).ToList();
            dic.Add("rewards", aso_list);
            return new ASObject(dic);
        }

        /// <summary>构建奖励更新数据</summary>
        /// <param name="list">物品类型集合</param>
        /// <param name="model">玩家</param>
        /// <param name="increases">新增物品对象集合</param>
        /// <param name="decreases">更新物品对象集合</param>
        /// <param name="delete">删除物品ID集合</param>
        /// <param name="list_rv">奖励物品Vo集合</param>
        private ASObject RewardsASObject(IEnumerable<int> list, tg_user model, List<ASObject> increases, List<ASObject> decreases, List<double> delete,
            IEnumerable<RewardVo> list_rv)
        {
            var dic = new Dictionary<string, object>();
            var aso_list = new List<ASObject>();
            aso_list.AddRange(list_rv.Select(AMFConvert.ToASObject).ToList());
            aso_list.AddRange(list.Select(item => AMFConvert.ToASObject(BulidReward(item, model, increases, decreases, delete))));
            dic.Add("rewards", aso_list);
            return new ASObject(dic);
        }

        /// <summary>组织物品跟新数据</summary>
        /// <param name="model">玩家</param>
        /// <param name="goodsType">物品类型</param>
        /// <param name="increases">新增物品对象集合</param>
        /// <param name="decreases">更新物品对象集合</param>
        /// <param name="delete">删除物品ID集合</param>
        /// <returns></returns>
        private RewardVo BulidReward(int goodsType, tg_user model, List<ASObject> increases, List<ASObject> decreases, List<double> delete)
        {
            var reward = new RewardVo();
            switch (goodsType)
            {
                #region
                case (int)GoodsType.TYPE_COIN: { reward = new RewardVo { goodsType = goodsType, value = model.coin }; break; }
                case (int)GoodsType.TYPE_COUPON: { reward = new RewardVo { goodsType = goodsType, value = model.coupon }; break; }
                case (int)GoodsType.TYPE_GOLD: { reward = new RewardVo { goodsType = goodsType, value = model.gold }; break; }
                case (int)GoodsType.TYPE_RMB: { reward = new RewardVo { goodsType = goodsType, value = model.rmb, }; break; }
                case (int)GoodsType.TYPE_PROP: { reward = new RewardVo { goodsType = goodsType, increases = increases, decreases = decreases }; break; }
                case (int)GoodsType.TYPE_EQUIP: { reward = new RewardVo { goodsType = goodsType, increases = increases, decreases = decreases }; break; }
                case (int)GoodsType.TYPE_SPIRIT: { reward = new RewardVo { goodsType = goodsType, value = model.spirit }; break; }
                case (int)GoodsType.TYPE_FAME: { reward = new RewardVo { goodsType = goodsType, value = model.fame }; break; }
                case (int)GoodsType.TYPE_DONATE: { reward = new RewardVo { goodsType = goodsType, value = model.donate }; break; }
                case (int)GoodsType.TYPE_MERIT: { reward = new RewardVo { goodsType = goodsType, value = model.merit }; break; }
                #endregion
            }
            return reward;
        }


        #endregion

        #region 摇钱树推送
        public void ShakeMoneyPush()
        {
            var users = tg_user.FindAll().ToList();
            foreach (var item in users)
            {

                var token = new CancellationTokenSource();
                var obj = new ShakeMoneyObject()
                {
                    userid = item.id,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as ShakeMoneyObject;
                    if(_obj==null)return;
                    ShakeMoneyPush(_obj.userid);
                    token.Cancel();
                },obj,token.Token);
            }
        }

        public void ShakeMoneyPush(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            //向在线玩家推送数据
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;          
            var dic = new Dictionary<string, object> { { "count", 0 } };
            var aso = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.USER, (int)UserCommand.USER_SHAKE_MONEY_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>
        /// 线程对象
        /// </summary>
        class ShakeMoneyObject
        {
            public Int64 userid { get; set; }

        }
        #endregion
    }

    public class CoinItem
    {
        /// <summary>
        /// 当前金钱
        /// </summary>
        public Int64 Current { get; set; }

        /// <summary>
        /// 花费/获取金钱
        /// </summary>
        public Int64 Cost { get; set; }

        /// <summary>
        /// 最终剩余金钱
        /// </summary>
        public Int64 Surplus { get; set; }
    }
}
