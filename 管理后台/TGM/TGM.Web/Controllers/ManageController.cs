using System;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class ManageController : ControllerBase
    {
        public ActionResult Email()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            return View();
        }

        /// <summary>发送邮件</summary>
        [HttpPost]
        public ActionResult Email(FormCollection form)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");

                var title = form["title"];
                var content = form["content"];
                if (string.IsNullOrEmpty(title))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "邮件标题为空";
                    return View();
                }
                if (string.IsNullOrEmpty(content))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "邮件内容为空";
                    return View();
                }
                if (user == null) return View();
                var s = server;
                if (s == null)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "请选择服务器";
                    return Redirect("/Home/Login");
                }
                var sn = s.ToString();
                var email = ApiFindMsg(user.token, sn, title, content);
                if (email != null)
                {
                    if (email.result != -1) return Redirect("/Manage/Index"); 
                    ViewBag.Error = -1;
                    ViewBag.Message = email.message;
                    return View();
                }
                ViewBag.Error = -1;
                ViewBag.Message = "数据错误";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            return View();
        }

        public ActionResult Notice()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            return View();
        }

        [HttpPost]
        public ActionResult Notice(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            var starttime = collection["starttime"];
            var endtime = collection["endtime"];
            var interval = collection["interval"];
            var content = collection["notice_content"];

            if (string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(endtime) || string.IsNullOrEmpty(interval))
                return View();
            if (Convert.ToInt32(interval) == 0 || starttime == "0" || endtime == "0")
            {
                ViewBag.error = -1;
                ViewBag.Message = "时间设置有误";
                return View();
            }
            if (content == "")
            {
                ViewBag.error = -1;
                ViewBag.Message = "公告内容为空";
                return View();
            }
            var s = server;
            if (s == null)
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请选择服务器";
                return Redirect("/Home/Login");
            }
            var sn = s.ToString();
            var stick = DateTime.Parse(starttime).Ticks;
            var etick = DateTime.Parse(endtime).Ticks;
            if (etick < stick)
            {
                ViewBag.Error = -1;
                ViewBag.Message = "结束时间小于开始时间";
                return View();
            }

            var notice = ApiFindNotice(user.token, sn, stick, etick, Convert.ToInt32(interval), content);
            if (notice.result != 1)
            {
                ViewBag.Error = -1;
                ViewBag.Message = notice.message;
                return View();
            }
            return Redirect("/Manage/Notice");
        }

        /// <summary>请求邮件API</summary>
        private Email ApiFindMsg(Guid token, string sn, string title, string content)
        {
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Manage?token={0}&sn={1}&title={2}&content={3}", token, sn, title, content),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var entity = CommonHelper.Deserialize<Email>(result);
            return entity;
        }

        private Notice ApiFindNotice(Guid token, string sn, Int64 stime, Int64 etime, int interval, string content)
        {
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Manage?token={0}&sn={1}&start={2}&end={3}&space={4}&content={5}", token, sn, stime, etime, interval, content),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Notice>(result);
            return rm;
        }
    }
}
