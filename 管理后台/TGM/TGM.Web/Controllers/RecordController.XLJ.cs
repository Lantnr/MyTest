using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NewLife;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public partial class RecordController
    {
        #region 游戏记录列表
        //
        // GET: /Record/

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        [HttpGet]
        public JsonResult PagerServerList(Int32 pid, Int32 sid, Int32 index, Int32 size)
        {
            //POST api/Record?token={token}&role={role}&pid={pid}&sid={sid}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&role={1}&pid={2}&sid={3}&index={4}&size={5}",
                user.token, user.role, pid, sid, index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerRecord>(result);
            return Json(list, JsonRequestBehavior.AllowGet);

        }



        #endregion

        #region 服务器每日详细信息

        /// <summary>元宝充值 消费记录</summary>
        public ActionResult ServerDetail(Int32 sid)
        {
            if (!IsLogin) return Redirect("Home/Login");

            return View(sid);
        }
        [HttpGet]
        public JsonResult PagerServerDetail(Int32 sid, String begin, String end, Int32 index, Int32 size)
        {
            //POST api/Record?token={token}&sid={sid}&begin={begin}&end={end}&index={index}&size={size}
            var b = Convert.ToDateTime(begin);
            var e = Convert.ToDateTime(end);
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&sid={1}&begin={2}&end={3}&index={4}&size={5}",
                user.token, sid, b, e, index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerRecord>(result);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region 服务器每时信息

        [HttpGet]
        public JsonResult ServerHoursDetail(Int32 sid, string time)
        {
            var t = Convert.ToDateTime(time);

            //POST api/Record?token={token}&sid={sid}&time={time}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&sid={1}&time={2}", user.token, sid, t),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var chart = CommonHelper.Deserialize<ChartHours>(result);



            return Json(chart, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region 平台服务器存留

        /// <summary>元宝充值 消费记录</summary>
        public ActionResult ServerKeep()
        {
            if (!IsLogin) return Redirect("Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        [HttpGet]
        public JsonResult PagerServerKeep(Int32 pid, Int32 index, Int32 size)
        {
            //POST api/Record?token={token}&pid={pid}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&pid={1}&index={2}&size={3}", user.token, pid, index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerKeep>(result);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}