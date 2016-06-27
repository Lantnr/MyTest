using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using NewLife.Log;
using TGG.Core.Entity;
using TGM.API.Entity;

namespace TGM.API.Helper
{
    /// <summary>
    /// 定时作业
    /// </summary>
    public static class TimerWork
    {
        public static void HoursWorks(object sender, ElapsedEventArgs e)
        {
            HoursRecord();
        }

        /// <summary>每小时作业</summary>
        private static void HoursRecord()
        {
            tgm_server.SetDbConnName(DBConnect.GetName(null));
            var list_server = tgm_server.FindAll();
            foreach (var item in list_server)
            {
                var server = item.CloneEntity();
                GetSingleServer(server);
            }
        }

        /// <summary>单服作业</summary>
        /// <param name="model"></param>
        private static void GetSingleServer(tgm_server model)
        {
            try
            {
                report_day.SetDbConnName(DBConnect.GetName(model));
                var tgg = report_day.GetFindByTime();
                if (tgg == null) tgg = new report_day();

                tgm_record_pay.SetDbConnName(DBConnect.GetName(null));
                var pay = tgm_record_hours.Proc_sp_pay(model.id);
                if (pay == null) pay = new tgm_record_hours();


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
                server.Save();

            }
            catch (Exception)
            {

                XTrace.WriteLine("单服作业失败");
            }
        }

        #region 应用池回收

        private static string content = "";
        /// <summary>输出信息存储的地方</summary>
        public static string Content
        {
            get { return content; }
            set { content += "<div>" + value + "</div>"; }
        }

        /// <summary>应用池回收的时候调用的方法</summary>
        public static void SetContent()
        {
            Content = "END: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

    }
}