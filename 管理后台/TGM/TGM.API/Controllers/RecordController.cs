using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NewLife;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public partial class RecordController : ControllerBase
    {
        #region Arlen

        //  POST api/Record?token={token}&role={role}&pid={pid}&sid={sid}&index={index}&size={size}
        /// <summary>服务器列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="role">角色</param>
        /// <param name="pid">平台编号</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerRecord PostServerList(String token, Int32 role, Int32 pid, Int32 sid, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) { return new PagerRecord() { result = -1, message = "令牌不存在" }; }
            if (pid == 0)
            {
                if (role != 10000) { return new PagerRecord() { result = -2, message = "平台编号错误" }; }
            }
            tgm_record_server.SetDbConnName(tgm_connection);
            var total = tgm_record_server.GetServerTotalPay(pid, sid);
            var month = tgm_record_server.GetServerMonthPay(pid, sid);
            var count = 0;
            var entitys = tgm_record_server.GetPageEntity(pid, sid, index, size, out count).ToList();
            var list = new List<RecordServer>();
            list.AddRange(entitys.Select(ToEntity.ToRecordServer));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };


            return new PagerRecord { Pager = pager, month_total = month, total = total, RecordServers = list };
        }

        //  POST api/Record?token={token}&sid={sid}&begin={begin}&end={end}&index={index}&size={size}
        /// <summary>服务器每天分页列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerRecord PostRecordDayList(String token, Int32 sid, DateTime begin, DateTime end, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) { return new PagerRecord() { result = -1, message = "令牌不存在" }; }

            tgm_record_day.SetDbConnName(tgm_connection);
            var count = 0;
            var entitys = tgm_record_day.GetPageEntity(sid, begin, end, index, size, out count).ToList();
            var list = new List<RecordServer>();
            list.AddRange(entitys.Select(ToEntity.ToRecordServer));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerRecord { Pager = pager, RecordServers = list };
        }

        //  POST api/Record?token={token}&sid={sid}&time={time}
        /// <summary>服务器每天图表</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="time">时间</param>
        public ChartHours PostRecordHoursList(String token, Int32 sid, DateTime time)
        {
            if (!IsToken(token)) { return new ChartHours() { result = -1, message = "令牌不存在" }; }

            tgm_record_hours.SetDbConnName(tgm_connection);
            var entitys = tgm_record_hours.GetFind24(sid, time).ToList();

            var data_online = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var data_login = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var data_register = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var item in entitys)
            {
                var m = item.CloneEntity();
                var hour = new DateTime(m.createtime).Hour;
                data_online[hour] = m.online;
                data_login[hour] = m.taday_login;
                data_register[hour] = m.register;
            }

            var sum_online = entitys.Any() ? entitys.Sum(m => m.online) : 0;
            var sum_login = entitys.Any() ? entitys.Sum(m => m.taday_login) : 0;
            var sum_register = entitys.Any() ? entitys.Sum(m => m.register) : 0;

            var avg_online = entitys.Any() ? Math.Round(entitys.Average(m => m.online), 1) : 0;
            var avg_login = entitys.Any() ? Math.Round(entitys.Average(m => m.taday_login), 1) : 0;
            var avg_register = entitys.Any() ? Math.Round(entitys.Average(m => m.register), 1) : 0;

            var best_online = entitys.Any() ? entitys.Max(m => m.online) : 0;
            var best_login = entitys.Any() ? entitys.Max(m => m.taday_login) : 0;
            var best_register = entitys.Any() ? entitys.Max(m => m.register) : 0;

            var online = new RecordHours { data = data_online, best = best_online, average = avg_online, total = sum_online };
            var login = new RecordHours { data = data_login, best = best_login, average = avg_login, total = sum_login };
            var register = new RecordHours { data = data_register, best = best_register, average = avg_register, total = sum_register };
            //list.AddRange(entitys.Select(ToEntity.ToRecordServer));
            //var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new ChartHours { result = 1, online = online, login = login, register = register };
        }

        //  POST api/Record?token={token}&pid={pid}&index={index}&size={size}
        /// <summary>获取平台服务器存留数据</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台编号</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerKeep PostServerKeepList(String token, Int32 pid, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) { return new PagerKeep() { result = -1, message = "令牌不存在" }; }
            tgm_record_keep.SetDbConnName(tgm_connection);
            var count = 0;
            var entitys = tgm_record_keep.GetPageEntity(pid, index, size, out count).ToList();
            var list = new List<RecordKeep>();
            list.AddRange(entitys.Select(ToEntity.ToRecordKeep));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerKeep { Pager = pager, Keeps = list };
        }

        #endregion
    }
}
