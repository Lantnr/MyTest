using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGM.API.Entity;

namespace TGM.Tools
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            DisplayGlobal.log.LogEvent += log_LogEvent;
            Util.ReadBaseEntity();
        }

        private void SetButton(bool isrun)
        {
            DisplayGlobal.log.Write(isrun ? "停止..." : "运行中...");
            btn_Run.Enabled = isrun;
            btn_Stop.Enabled = !isrun;
        }

        void log_LogEvent(object sender, LogEventArgs e)
        {
            rtb_msg.BeginInvoke((MethodInvoker)(() =>
            {
                rtb_msg.AppendText(String.Format("[{0}]: {1} \r\n", DateTime.Now, e.Message));
                XTrace.WriteLine(e.Message);
            }));
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb_msg.Clear();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Interval = 1000 * 60 * 10;//5分钟
            EXEC_HH = DateTime.Now.AddHours(-1);
            EXEC_DD = DateTime.Now.AddDays(-1);
            DisplayGlobal.log.Write("启动程序");
            SetButton(false);
            RunWork();
        }


        private void btn_Run_Click(object sender, EventArgs e)
        {
            SetButton(false);
            RunWork();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            StopWork();
            SetButton(true);

        }


        #region

        private System.Timers.Timer WorkTimer { get; set; }

        /// <summary>停止工作</summary>
        private void StopWork()
        {
            WorkTimer.Enabled = false;
            DisplayGlobal.log.Write("停止工作...");
        }

        private Int32 Interval { get; set; }

        /// <summary>运行工作</summary>
        private void RunWork()
        {
            DisplayGlobal.log.Write("开始工作...");
            TimerTask(null, null);//程序启动即立刻运行一次
#if DEBUG
            Interval = 1000 * 60;//1分钟
#endif
            WorkTimer = new System.Timers.Timer { Interval = Interval };
            DisplayGlobal.log.Write(string.Format("时间间隔为: {0}min  {1}ms...", Interval / 60000, Interval));
            WorkTimer.Elapsed += TimerTask;  //到达时间的时候执行事件;   
            WorkTimer.AutoReset = true;     //设置是执行一次（false）还是一直执行(true); 
            WorkTimer.Enabled = true;       //是否执行System.Timers.Timer.Elapsed事件;  

        }

        private void TimerTask(object sender, System.Timers.ElapsedEventArgs e)
        {
#if !DEBUG
            var current = DateTime.Now;
            var hh = (current - EXEC_HH).Hours;
            if (hh == 1)
            {
                DisplayGlobal.log.Write("定时小时任务...");
                EXEC_HH = current;
                TaskHours();
            }
            var dd = (current - EXEC_DD).Days;
            if (dd == 1)
            {
                DisplayGlobal.log.Write("定时每日任务...");
                EXEC_DD = current;
                TaskDay();
            }
#endif
#if DEBUG
            //var current = Convert.ToDateTime("2015-1-13 0:5:14"); ;
            //EXEC_HH =Convert.ToDateTime("2015-1-12 23:5:15");
            //var hh = (current - EXEC_HH).TotalHours;
            //DisplayGlobal.log.Write("TotalHours:" + hh);
            //var hh1 = (current - EXEC_HH).Hours;
            //DisplayGlobal.log.Write("Hours:" + hh1);
            //var hh2 = (current - EXEC_HH);
            //DisplayGlobal.log.Write("sp:" + hh2);
            TaskHours();
            TaskDay();
#endif
        }

        #region 小时任务

        /// <summary>按小时执行</summary>
        private DateTime EXEC_HH { get; set; }

        /// <summary>小时任务</summary>
        private void TaskHours()
        {
            DisplayGlobal.log.Write("任务执行中...");
            try
            {
                var sw = Stopwatch.StartNew();
                tgm_server.SetDbConnName(DBConnect.GetName(null));
                DisplayGlobal.log.Write("拉取游戏服务器列表...");
                var list_server = tgm_server.FindAll("server_state in (2,3)", null, null, 0, 0);
                foreach (var item in list_server)
                {
                    var server = item.CloneEntity();
                    SingleServer(server);
                }
                sw.Stop();
                DisplayGlobal.log.Write(String.Format("任务总耗时: {0} 毫秒 ≈ {1} 秒", sw.Elapsed.Milliseconds, sw.Elapsed.Seconds));
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("任务执行中...");
            }
        }

        private void SingleServer(tgm_server model)
        {
            try
            {
                DisplayGlobal.log.Write("游戏服务器数据统计中...");
                report_day.SetDbConnName(DBConnect.GetName(model));
                var tgg = report_day.GetFindByTime();
                if (tgg == null) tgg = new report_day();

                tgm_record_pay.SetDbConnName(DBConnect.GetName(null));
                var pay = tgm_record_hours.Proc_sp_pay(model.id);
                if (pay == null) pay = new tgm_record_hours();

                tg_log_operate.SetDbConnName(DBConnect.GetName(model));
                var cost_gold = tg_log_operate.GetTodayCost((int)GoodsType.TYPE_GOLD, (int)LogType.Use);
                if (cost_gold == null) cost_gold = 0;


                tgm_record_hours.SetDbConnName(DBConnect.GetName(null));
                var entity = tgm_record_hours.GetFindBySidTime(model.id);
                if (entity == null) entity = new tgm_record_hours();

                entity.pid = model.pid;
                entity.sid = model.id;
                entity.server_name = model.name;
                entity.offline = tgg.offline;
                entity.online = tgg.online;
                entity.history_online = tgg.history_online;
                entity.register = tgg.register;
                entity.register_total = tgg.register_total;
                entity.taday_login = tgg.taday_login;
                entity.taday_online = tgg.taday_online;
                entity.pay_count = pay.pay_count;
                entity.pay_number = pay.pay_number;
                entity.pay_taday = pay.pay_taday;
                entity.pay_total = pay.pay_total;
                entity.pay_month = pay.pay_month;
                entity.createtime = DateTime.Now.Ticks;
                entity.taday_cost = cost_gold;
                entity.Save();

                tgm_record_day.SetDbConnName(DBConnect.GetName(null));
                var r_day = tgm_record_day.GetFindBySidTime(entity.sid);
                //是否没有数据
                if (r_day == null) r_day = new tgm_record_day();
                r_day.pid = entity.pid;
                r_day.sid = entity.sid;
                r_day.server_name = entity.server_name;
                r_day.offline = entity.offline;
                r_day.online = entity.online;
                r_day.history_online = entity.history_online;
                r_day.register = entity.register;
                r_day.register_total = entity.register_total;
                r_day.taday_login = entity.taday_login;
                r_day.taday_online = entity.taday_online;
                r_day.pay_count = entity.pay_count;
                r_day.pay_number = entity.pay_number;
                r_day.pay_taday = entity.pay_taday;
                r_day.pay_total = entity.pay_total;
                r_day.pay_month = entity.pay_month;
                r_day.createtime = DateTime.Now.Ticks;
                r_day.taday_cost = cost_gold;
                r_day.Save();

                tgm_record_server.SetDbConnName(DBConnect.GetName(null));
                var server = tgm_record_server.GetFindBySid(model.id);
                if (server == null) server = new tgm_record_server();
                server.pid = entity.pid;
                server.sid = entity.sid;
                server.server_name = entity.server_name;
                server.offline = entity.offline;
                server.online = entity.online;
                server.history_online = entity.history_online;
                server.register = entity.register;
                server.register_total = entity.register_total;
                server.taday_login = entity.taday_login;
                server.taday_online = entity.taday_online;
                server.pay_count = entity.pay_count;
                server.pay_number = entity.pay_number;
                server.pay_taday = entity.pay_taday;
                server.pay_total = entity.pay_total;
                server.pay_month = entity.pay_month;
                server.createtime = model.createtime;
                server.taday_cost = cost_gold;
                server.Save();

            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("单服作业失败");
            }
        }
        #endregion

        #region 每天任务

        /// <summary>按天执行</summary>
        private DateTime EXEC_DD { get; set; }

        /// <summary>按天任务</summary>
        private void TaskDay()
        {
            DisplayGlobal.log.Write("任务执行中...");
            try
            {
                var sw = Stopwatch.StartNew();
                tgm_server.SetDbConnName(DBConnect.GetName(null));
                DisplayGlobal.log.Write("拉取游戏服务器列表...");
                var list_server = tgm_server.FindAll("server_state in (2,3)", null, null, 0, 0);
                foreach (var item in list_server)
                {
                    var server = item.CloneEntity();
                    SingleServerKeep(server);
                    SingleServerIdentityAndLevel(server);
                }
                sw.Stop();
                DisplayGlobal.log.Write(String.Format("任务总耗时: {0} 毫秒 ≈ {1} 秒", sw.Elapsed.Milliseconds, sw.Elapsed.Seconds));
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("任务执行中...");
            }
        }


        private void SingleServerKeep(tgm_server model)
        {
            try
            {
                DisplayGlobal.log.Write("游戏服务器留存数据统计中...");
                var tg_connect = DBConnect.GetName(model);
                tgm_record_keep.SetDbConnName(DBConnect.GetName(null));
                var keep = tgm_record_keep.sp_user_keep(tg_connect, model.createtime);
                if (keep == null)
                {
                    DisplayGlobal.log.Write("留存统计作业失败");
                    return;
                }

                var entity = tgm_record_keep.GetFindEntityBySid(model.id);
                if (entity == null)
                {
                    entity = new tgm_record_keep
                        {
                            sid = model.id,
                            pid = model.pid,
                            server_name = model.name,
                        };
                }
                keep.id = entity.id;
                keep.sid = entity.sid;
                keep.pid = entity.pid;
                keep.server_name = entity.server_name;

                keep.Save();

            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("单服作业失败");
            }
        }

        /// <summary>统计每日身份，等级信息</summary>
        private void SingleServerIdentityAndLevel(tgm_server model)
        {
            try
            {
                DisplayGlobal.log.Write("游戏服务器身份信息统计数据中...");
                report_identity_day.SetDbConnName(DBConnect.GetName(model));
                var identity = report_identity_day.GetFindByTime();

                if (identity == null)
                {
                    identity = new report_identity_day();
                }
                else
                {
                    identity.identity1_count = 0;
                    identity.identity2_count = 0;
                    identity.identity3_count = 0;
                    identity.identity4_count = 0;
                    identity.identity5_count = 0;
                    identity.identity6_count = 0;
                    identity.identity7_count = 0;
                }

                report_level_day.SetDbConnName(DBConnect.GetName(model));
                var levelRecord = report_level_day.GetFindByTime();
                if (levelRecord == null)
                {
                    levelRecord = new report_level_day();
                }
                else
                {
                    levelRecord.stage1_count = 0;
                    levelRecord.stage2_count = 0;
                    levelRecord.stage3_count = 0;
                    levelRecord.stage4_count = 0;
                    levelRecord.stage5_count = 0;
                    levelRecord.stage6_count = 0;
                    levelRecord.stage7_count = 0;
                    levelRecord.stage8_count = 0;
                }

                tg_role.SetDbConnName(DBConnect.GetName(model));
                var list = tg_role.GetAllMainRoles().ToList();
                if (!list.Any()) return;

                var count = list.Count;
                foreach (var item in list)
                {
                    var stage = FixedResources.BASE_IDENTITY.FirstOrDefault(m => m.id == item.role_identity);
                    if (stage == null) continue;

                    switch (stage.value)
                    {
                        case 1: identity.identity1_count++; break;
                        case 2: identity.identity2_count++; break;
                        case 3: identity.identity3_count++; break;
                        case 4: identity.identity4_count++; break;
                        case 5: identity.identity5_count++; break;
                        case 6: identity.identity6_count++; break;
                        case 7: identity.identity7_count++; break;
                    }
                    var level = item.role_level;
                    if (level >= 1 && level <= 20) { levelRecord.stage1_count++; }
                    if (level >= 21 && level <= 30) { levelRecord.stage2_count++; }
                    if (level >= 31 && level <= 35) { levelRecord.stage3_count++; }
                    if (level >= 36 && level <= 40) { levelRecord.stage4_count++; }
                    if (level >= 41 && level <= 45) { levelRecord.stage5_count++; }
                    if (level >= 46 && level <= 50) { levelRecord.stage6_count++; }
                    if (level >= 51 && level <= 55) { levelRecord.stage7_count++; }
                    if (level >= 56) { levelRecord.stage8_count++; }
                }
                identity.createtime = DateTime.Now.Ticks;
                identity.Save();

                levelRecord.total_count = count;
                levelRecord.createtime = DateTime.Now.Ticks;
                levelRecord.Save();
            }
            catch (Exception ex)
            {
                DisplayGlobal.log.Write("单服作业失败");
            }
        }

        #endregion












        #endregion

    }
}
