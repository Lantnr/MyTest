using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class MessageController : ControllerBase
    {
        //
        // GET: /Message/

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var list = ApiFindALLNotice(user.token, user.id);
            ViewBag.Platform = ApiPlatforms();
            return View(list);
        }

        /// <summary>发送公告</summary>
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var notices = ApiFindALLNotice(user.token, user.id);
            ViewBag.Platform = ApiPlatforms();

            var starttime = collection["starttime"];
            var endtime = collection["endtime"];
            var interval = collection["interval"];
            var content = collection["notice_content"];
            var sid = collection["server"];
            var pid = collection["platform"];

            if (string.IsNullOrEmpty(endtime) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(interval) || string.IsNullOrEmpty(pid))
            {
                ViewBag.error = -1;
                ViewBag.Message = "信息设置不完整";
                return View(notices);
            }
            if (string.IsNullOrEmpty(content))
            {
                ViewBag.error = -1;
                ViewBag.Message = "公告内容为空";
                return View(notices);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                if (starttime == "0" || endtime == "0")
                {
                    ViewBag.error = -1;
                    ViewBag.Message = "时间设置有误";
                    return View(notices);
                }
                var dt1 = DateTime.Now;
                var datetime = Convert.ToDateTime(starttime);
                var ts = datetime.Subtract(dt1);
                var minute = ts.TotalMinutes;
                if (minute < 30)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "预设时间小于30分钟";
                    return View(notices);
                }
                var stick = DateTime.Parse(starttime).Ticks;
                var etick = DateTime.Parse(endtime).Ticks;
                if (etick < stick)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "结束时间小于开始时间";
                    return View(notices);
                }
                var dateend = Convert.ToDateTime(endtime);
                var t = dateend.Subtract(datetime);
                var m = t.TotalSeconds;
                if (Convert.ToInt32(interval) > m)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "间隔时间大于开始与结束时间的时间差";
                    return View(notices);
                }
            }
            else
            {
                var dt1 = DateTime.Now;
                var datetime = Convert.ToDateTime(endtime);
                var ts = datetime.Subtract(dt1);
                var ms = ts.TotalSeconds;
                if(ms<=Convert.ToInt32(interval))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "间隔时间大于开始与结束时间的时间差";
                    return View(notices);
                }
            }

            var notice = ApiFindNotice(user.token, starttime, endtime, user.id, user.role, Convert.ToInt32(pid), Convert.ToInt64(sid), Convert.ToInt32(interval), content);
            if (notice.result != 1)
            {
                ViewBag.Error = -1;
                ViewBag.Message = notice.message;
                ViewBag.Platform = ApiPlatforms();
                return View(notices);
            }
            var noticess = ApiFindALLNotice(user.token, user.id);
            return View(noticess);
        }

        private Notice ApiFindNotice(Guid token, string stime, string etime, Int64 roleid, int role, Int64 pid, Int64 sid, int interval, string content)
        {
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Message?token={0}&roleid={1}&role={2}&&pid={3}&sid={4}&start={5}&end={6}&space={7}&content={8}", token, roleid, role, pid, sid, stime, etime, interval, content),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Notice>(result);
            return rm;
        }

        private List<Notice> ApiFindALLNotice(Guid token, Int64 roleid)
        {
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Message?token={0}&roleid={1}&flag={2}", token, roleid, true),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            return CommonHelper.Deserialize<List<Notice>>(result);
        }

        /// <summary> 删除</summary>
        public ActionResult Delete(int id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");
            if (id == 0) return Redirect("/Message/Index");

            //POST api/System/{id}?token={token}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/Message?id={0}&token={1}", id, user.token),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Notice>(result);

            if (rm.result == 1) return Redirect("/Message/Index");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/Message/Index");
        }
    }
}
