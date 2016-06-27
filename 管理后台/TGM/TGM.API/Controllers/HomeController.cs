using System;
using System.Web.Http;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class HomeController : ControllerBase
    {

        //  POST api/Home?token={token}&sid={sid}
        /// <summary>获取首页图表数据</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器id</param>
        public ChartHome PostChartHome(String token, Int32 sid)
        {
            if (!IsToken(token)) { return new ChartHome() { result = -1, message = "令牌不存在" }; }

            tgm_server.SetDbConnName(tgm_connection);
            var server=tgm_server.FindByid(sid);
            if (server == null) { return new ChartHome() { result = -2, message = "服务器不存在" }; }

            var entity = new ChartHome();

            SN = server.name;
            report_day.SetDbConnName(db_connection);
            var player = report_day.Find("");
            if (player == null) player = new report_day();
            entity.server.Add(player.online);//在线人数
            entity.server.Add(player.taday_online);//今日最高在线人数
            entity.server.Add(player.register);//今日注册人数
            entity.server.Add(player.taday_login);//今日登陆人数

            tgm_record_hours.SetDbConnName(tgm_connection);
            var pay = tgm_record_hours.Proc_sp_pay(sid);
            if (pay == null) pay = new tgm_record_hours();
            entity.pay.Add(pay.pay_number);//今日充值人数
            entity.pay.Add(pay.pay_count);//今日充值次数
            entity.pay.Add(pay.pay_taday);//今日充值
            entity.pay.Add(pay.pay_month);//月充值
            return entity;
        }
    }
}
