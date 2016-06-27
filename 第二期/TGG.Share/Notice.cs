using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary> 公告共享类 </summary>
    public class Notice
    {
        #region 公共方法

        /// <summary> 重新起服时 初始化公告线程（重新加入公告） </summary>
        public void InitNoticeTask()
        {
            var list = tg_system_notice.GetEntitys((int)NoticeStateType.OBSOLETE);
            if (!list.Any()) return;
            NoticeSend(list);
        }

        /// <summary>系统公告推送</summary>
        public void NoticeSend(List<tg_system_notice> list)
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
                            TrainingPlayer(notice.base_Id, notice.content);
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

        /// <summary> 公告推送在线玩家 </summary>
        /// <param name="baseid">要推送的公告基表Id，无就填0</param>
        /// <param name="content">要推送的内容</param>
        public void TrainingPlayer(int baseid, string content)
        {
#if DEBUG
            XTrace.WriteLine("{0}_{1}_{2}", "向在线玩家推送系统公告！",baseid,content);
#endif

            var keys = Variable.OnlinePlayer.Keys;
            foreach (var item in keys)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var userid = Convert.ToInt64(n);
                    SendNotice(userid, baseid, content);
                }, item, token.Token);
            }
        }

        /// <summary> 公告推送玩家 </summary>
        /// <param name="userid">要推送的用户Id</param>
        /// <param name="baseid">要推送的公告基表Id，无就填0</param>
        /// <param name="content">要推送的内容</param>
        public void SendNotice(Int64 userid, int baseid, string content)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid];
            var pv = session.InitProtocol((int)ModuleNumber.NOTICE, (int)NoticeCommand.SYSTEM_NOTICE, (int)ResponseType.TYPE_SUCCESS, new ASObject(BuildData(baseid, content)));
            session.SendData(pv);
        }

        /// <summary> 增加新公告 </summary>
        public void AddNewNotice()
        {
            tg_system_notice.Delete(" base_Id in(100001,100002,100003,100004) ");
            InsertBuildNotice();
            InsertSiegeNotice();
            //NewNoticeAddTask();
        }

        /// <summary> 检索新公告，加入线程 </summary>
        public void NewNoticeAddTask()
        {
            var list = tg_system_notice.GetEntityByState((int)NoticeStateType.WAIT);
            if (!list.Any()) return;
            NoticeSend(list);
        }

        #endregion

        #region 私有方法

        /// <summary> 插入一夜墨俣相关公告 </summary>
        private void InsertBuildNotice()
        {
            var stimeA = Variable.Activity.BuildActivity.StartTime.AddMinutes(-5).Ticks;
            var etimeA = Variable.Activity.BuildActivity.StartTime.AddMinutes(-4).Ticks;
            InsertNotice(100003, "", stimeA, etimeA, 2, 100);
            var stimeB = Variable.Activity.BuildActivity.StartTime.Ticks;
            var etimeB = Variable.Activity.BuildActivity.StartTime.AddMinutes(1).Ticks;
            InsertNotice(100004, "", stimeB, etimeB, 2, 100);
        }

        /// <summary> 插入美浓攻略相关公告 </summary>
        private void InsertSiegeNotice()
        {
            var stimeA = Variable.Activity.Siege.BaseData.ActivityTime.AddMinutes(-5).Ticks;
            var etimeA = Variable.Activity.Siege.BaseData.ActivityTime.AddMinutes(-4).Ticks;
            InsertNotice(100001, "", stimeA, etimeA, 2, 100);
            var stimeB = Variable.Activity.Siege.BaseData.ActivityTime.Ticks;
            var etimeB = Variable.Activity.Siege.BaseData.ActivityTime.AddMinutes(1).Ticks;
            InsertNotice(100002, "", stimeB, etimeB, 2, 100);
        }

        /// <summary> 插入公告 </summary>
        /// <param name="baseid">系统公告的基表id 无就填0</param>
        /// <param name="content">内容</param>
        /// <param name="statime">公告开始时间</param>
        /// <param name="endtime">公告结束时间</param>
        /// <param name="level">公告级别</param>
        /// <param name="interval">公告的推送间隔</param>
        private void InsertNotice(int baseid, string content, Int64 statime, Int64 endtime, int level, int interval)
        {
            var model = new tg_system_notice
            {
                level = level,
                base_Id = baseid,
                content = content,
                end_time = endtime,
                start_time = statime,
                time_interval = interval,
            };
            model.Insert();
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

        /// <summary>玩家组装公告数据</summary>
        private Dictionary<String, Object> BuildData(int id, string content)
        {
            var dic = new Dictionary<string, object>
            {
                {"id", id},
                {"content",content }
            };
            return dic;
        }

        #endregion
    }
}
