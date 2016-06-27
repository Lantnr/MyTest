using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using System.Linq;
using System.Threading;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share.Event;
using TGG.SocketServer;
using tg_user_login_log = TGG.Core.Entity.tg_user_login_log;
using tg_user = TGG.Core.Entity.tg_user;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 玩家登陆指令
    /// </summary>
    public class USER_LOGIN
    {
        private static USER_LOGIN ObjInstance;


        /// <summary>LOGIN单例模式</summary>
        public static USER_LOGIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new USER_LOGIN());
        }

        /// <summary>玩家登陆指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "LOGIN", "玩家登陆指令");
#endif
#if DEBUG
            XTrace.WriteLine("------------   登陆前  {0}     ------------", Variable.OnlinePlayer.Count);
#endif
            var userName = data.FirstOrDefault(q => q.Key == "userName").Value.ToString();
            if (string.IsNullOrEmpty(userName)) return CommonHelper.ErrorResult((int)ResultType.USER_SUBMIT_ERROR);
            if (!CommonHelper.CheckActivation(userName)) return CommonHelper.ErrorResult((int)ResultType.ACTIVATION_ERROR);

            //DisplayGlobal.log.Write("登陆账号:" + userName);

            var user = tg_user.Find(string.Format("user_code='{0}'", userName));
            if (user == null) return new ASObject(BuildData((int)ResultType.NO_DATA, null, 0));
            var b = Variable.OnlinePlayer.ContainsKey(user.id);
#if DEBUG
            XTrace.WriteLine("OnlinePlayer:{0} {1}", user.id, b);
#endif
            if (b)
            {
                var user_id = Convert.ToInt64(user.id);
                var s = Variable.OnlinePlayer[user_id] as TGGSession;
                if (s != null) s.Close();
            }
            session.Player = Common.GetInstance().GetPlayer(user);
            session.Fight = Common.GetInstance().GetFight(session.Player);

            Variable.OnlinePlayer.AddOrUpdate(user.id, session, (k, v) => session);
            tg_user_login_log.LoginLog(user.id, Common.GetInstance().GetRemoteIP(session));
            ReLoadTask(session);
            Int64 opentime = 0;
            if (CommonHelper.CheckOpenTime())
            {
                if (session.Player.UserExtend.fcm == (int)FCMType.Yes)
                {
                    var fcm = new Share.User().GetFCM(user.id);
                    if (fcm != null)
                    {
                        session.Player.onlinetime = fcm.login_time_longer_day * 60;
                        opentime = fcm.login_open_time;
                    }
                }
            }
            return new ASObject(BuildData((int)ResultType.SUCCESS, session.Player, opentime));
        }

        /// <summary>返回注册</summary>
        public Dictionary<String, Object> BuildData(int result, Player player, Int64 time)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"userInfoVo", player != null ? AMFConvert.ToASObject(EntityToVo.ToUserInfoVo(player)) : null},
                {"influence", tg_user.FindCampInfluence()}
            };
            return dic;
        }

        /// <summary>登陆重新加载当前账号未完成任务 >10s</summary>
        private void ReLoadTask(TGGSession session)
        {
            CarRecovery(session.Player.User.id);
            CarTask(session.Player.User.id);
        }

        /// <summary>跑商马车重新加入线程</summary>
        private void CarRecovery(Int64 user_id)
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Business", "Common");
                obje.LoginCar(user_id);
            }
            catch { }

        }

        private void CarTask(Int64 user_id)
        {
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var endtime = time + 5000;
            var list_car = tg_car.GetEntityListByState((int)CarStatusType.RUNNING, user_id, endtime);
            if (!list_car.Any()) return;
            foreach (var item in list_car)
            {
                //结束全局线程
                var key = string.Format("{0}_{1}_{2}", (int)CDType.Businss, item.user_id, item.id);
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);

                tg_car.GetTaskCarUpdate(item.id);
            }
        }

    }
}
