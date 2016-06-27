using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Common.Util;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class PayController : ControllerBase
    {
        //
        // GET: /Pay/

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms();
            ViewBag.Error = 1;
            return View();
        }

        #region

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var pid = collection["platform"];
            var sid = collection["server"];
            var start = collection["starttime"];
            var end = collection["endtime"];

            if (pid == "")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请选择平台";
                ViewBag.Platform = ApiPlatforms(); ;
                return View();
            }
            if (sid == "")
                sid = "0";
            Int64 stick = 0;
            Int64 etick = 0;
            if (start != "" && start != "0" && end != "" && end != "0")
            {
                stick = DateTime.Parse(start).Ticks;
                etick = DateTime.Parse(end).Ticks;
            }

            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Pay?token={0}&pid={1}&sid={2}&start={3}&end={4}", user.token, Convert.ToInt32(pid), Convert.ToInt32(sid), stick, etick),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var totalpay = CommonHelper.Deserialize<List<TotalRecordPay>>(result);
            ViewBag.Platform = ApiPlatforms();
            return View(totalpay);
        }
        #endregion

        public ActionResult Paylist()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms();
            ViewBag.Recordpay = new List<SingleRecordPay>();
            ViewBag.Error = 1;
            return View();
        }

        #region
        [HttpPost]
        public ActionResult Paylist(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var pid = collection["platform"];
            var sid = collection["server"];
            var start = collection["starttime"];
            var end = collection["endtime"];
            var type = collection["type"];
            var playername = collection["playname"];


            if (sid == "" || sid == "0" || pid == "" || pid == "0")
            {
                return View();
            }
            if (playername == "")
            {
                return View();
            }

            Int64 stick = 0;
            Int64 etick = 0;
            if (start != "" && start != "0" && end != "" && end != "0")
            {
                stick = DateTime.Parse(start).Ticks;
                etick = DateTime.Parse(end).Ticks;
            }

            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Pay?token={0}&pid={1}&sid={2}&start={3}&end={4}&type={5}&playname={6}&index={7}&size={8}", user.token, Convert.ToInt32(pid), Convert.ToInt32(sid), stick, etick, type, playername, 0, 0),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var totalpay = CommonHelper.Deserialize<List<SingleRecordPay>>(result);
            ViewBag.Platform = ApiPlatforms();
            ViewBag.Recordpay = totalpay;
            return View();
        }
        #endregion

        [HttpGet]
        public JsonResult TotalPagerHandler(Int32 page, Int32 size, string pid, string sid, string start, string end)
        {
            
            if (pid == "0" && sid == "0")
            {
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource =
                        string.Format("api/Pay?token={0}&role={1}&pid={2}&sign={3}&index={4}&size={5}", user.token, user.role,
                            user.pid, true, page, size),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var totalpay = CommonHelper.Deserialize<PagerTotalPay>(result);
                return Json(totalpay, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var pager = new PagerTotalPay();
                if (start != "" && start != "0" && end == "" || end == "0")
                {
                    pager.result = -1;
                    pager.message = "没有选结束时间";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                if (end != "" && end != "0" && start == "" || start == "0")
                {
                    pager.result = -1;
                    pager.message = "没有选开始时间";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                Int64 stick = 0;
                Int64 etick = 0;
                if (start != "" && start != "0" && end != "" && end != "0")
                {
                    stick = DateTime.Parse(start).Ticks;
                    etick = DateTime.Parse(end).Ticks;
                }
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource =
                        string.Format(
                            "api/Pay?token={0}&pid={1}&sid={2}&start={3}&end={4}&index={5}&size={6}",
                            user.token, Convert.ToInt32(pid), Convert.ToInt32(sid), stick, etick, page, size),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var totalpay = CommonHelper.Deserialize<PagerTotalPay>(result);
                return Json(totalpay, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult PagerHandler(Int32 page, Int32 size, string pid, string sid, string start, string end, string type, string playername)
        {
            if (pid == "0" && sid == "0")
            {
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = string.Format("api/Pay?token={0}&role={1}&pid={2}&index={3}&size={4}", user.token, user.role, user.pid, page, size),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var totalpay = CommonHelper.Deserialize<PagerPay>(result);
                return Json(totalpay, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var pager = new PagerTotalPay();
                if (start != "" && start != "0" && end == "" || end == "0")
                {
                    pager.result = -1;
                    pager.message = "没有选结束时间";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                if (end != "" && end != "0" && start == "" || start == "0")
                {
                    pager.result = -1;
                    pager.message = "没有选开始时间";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                if (sid == "0")
                {
                     pager.result = -1;
                    pager.message = "没有选服务器";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                if (playername == "")
                {
                    pager.result = -1;
                    pager.message = "没有选玩家";
                    return Json(pager, JsonRequestBehavior.AllowGet);
                }
                Int64 stick = 0;
                Int64 etick = 0;
                if (start != "" && start != "0" && end != "" && end != "0")
                {
                    stick = DateTime.Parse(start).Ticks;
                    etick = DateTime.Parse(end).Ticks;
                }
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = string.Format("api/Pay?token={0}&pid={1}&sid={2}&start={3}&end={4}&type={5}&playname={6}&userpid={7}&role={8}&index={9}&size={10}", user.token, Convert.ToInt32(pid), Convert.ToInt32(sid), stick, etick, Convert.ToInt32(type), playername, user.pid, user.role, page, size),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var totalpay = CommonHelper.Deserialize<PagerPay>(result);
                return Json(totalpay, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult Recharge()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Error = 0;
            return View(user);
        }

        [HttpPost]
        public ActionResult Recharge(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            //token|sid|user_code|player_id|player_name|order_id|channel|type|amount|key
            var pay_token = collection["pay_token"];
            var pay_sid = collection["pay_sid"];
            var pay_user_code = collection["pay_user_code"];
            //var pay_player_id = collection["pay_player_id"];
            //var pay_player_name = collection["pay_player_name"];
            var pay_order_id = collection["pay_order_id"];
            var pay_channel = collection["pay_channel"];
            var pay_type = collection["pay_type"];
            var pay_amount = collection["pay_amount"];

            var param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                pay_token, pay_sid, pay_user_code, pay_order_id, pay_channel, pay_type, pay_amount);

            var chk = string.Format("{0}|{1}", param, "15B5EACDB05E66D");
            //加密字符

            var chksum = UConvert.MD5(chk);
            //POST api/Common?param={param}&checksum={checksum}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?param={0}&checksum={1}", param,chksum),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var be = CommonHelper.Deserialize<BaseEntity>(result);
            ViewBag.Error = be.result;
            ViewBag.Message = be.message;
            return View(user);
        }

    }
}
