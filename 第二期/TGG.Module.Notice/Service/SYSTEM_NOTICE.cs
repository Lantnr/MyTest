using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Notice.Service
{
    /// <summary>
    /// 系统公告
    /// </summary>
    public class SYSTEM_NOTICE
    {
        private static SYSTEM_NOTICE _objInstance;

        /// <summary>SYSTEM_NOTICE单体模式</summary>
        public static SYSTEM_NOTICE GetInstance()
        {
            return _objInstance ?? (_objInstance = new SYSTEM_NOTICE());
        }

        #region 公告推送

        /// <summary>系统公告推送</summary>
        public void CommandStart(List<tg_system_notice> list)
        {
            try
            {
                var l = list.OrderBy(m => m.start_time).ThenBy(n => n.level);
                foreach (var item in l)
                {
                    var token = new CancellationTokenSource();
                    Task.Factory.StartNew(m =>
                    {
                        var notice = m as tg_system_notice;
                        if (notice == null) { token.Cancel(); return; }
                        var time = notice.end_time - DateTime.Now.Ticks;  //计算当前至结束的时间间隔
                        if (!UpdateState(notice, time)) return;

                        SpinWait.SpinUntil(() =>
                        {
                            if (notice.start_time > DateTime.Now.Ticks) return false; //验证公告时间是否到达
                            if (notice.end_time <= DateTime.Now.Ticks) //验证关闭公告时间是否到达
                            {
#if DEBUG
                                XTrace.WriteLine("{0}", "系统公告关闭！");
#endif
                                notice.state = (int)NoticeStateType.OBSOLETE;
                                notice.Update();
                                token.Cancel();
                                return true;
                            }
                            notice.start_time = DateTime.Now.Ticks + (notice.time_interval * 1000 * 10000);
                            new Share.Notice().TrainingPlayer(notice.base_Id, notice.content);
                            return false;
                        }, -1);
                    }, item, token.Token);
                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        private bool UpdateState(tg_system_notice notice, Int64 time)
        {
            if (time <= 1000)
                notice.state = (int)NoticeStateType.OBSOLETE;
            else
                notice.state = (int)NoticeStateType.RUN;
            notice.Update();
            return notice.state == (int)NoticeStateType.RUN;
        }

        #endregion

        /// <summary>
        /// 系统推送玩家跑商收益
        /// </summary>
        private static void PushSystemNews(TGGSession session, decimal total, int id)
        {
            var aso = new List<ASObject>
            {
                (new Chat()).BuildData((int) ChatsASObjectType.PLAYERS, null, session.Player.User.id,
                    session.Player.User.player_name, null),
                (new Chat()).BuildData((int) ChatsASObjectType.NUMBER, null, null, null, Convert.ToInt32(total))
            };
            (new Chat()).SystemChatSend(session, aso, id);
        }
    }
}
