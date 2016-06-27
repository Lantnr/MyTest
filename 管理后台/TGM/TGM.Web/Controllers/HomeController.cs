using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TGM.API.Entity;
using TGM.API.Entity.Model;
using TGM.Web.Helper;
using TGM.Web.Models;
using User = TGM.API.Entity.Model.User;

namespace TGM.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        #region Login
        public ActionResult Login()
        {
            ViewBag.error = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {
                var name = collection["Name"];
                var cpwd = collection["PassWord"];

                //api/System?name={name}&password={password}
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = string.Format("api/System?name={0}&password={1}", name, cpwd),
                };
                var result = api.PostJsonToParameter();
                var entity = CommonHelper.Deserialize<User>(result);
                api.Dispose();
                if (entity.result == 1)
                {
                    Session.Add("user", entity);
                    Session.Timeout = 60;
                    return RedirectToAction("Index");
                }
                ViewBag.msg = entity.message;
                ViewBag.error = 1;
                return View();
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Index
        public ActionResult Index(Int32 id = 1)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }


        public ActionResult HeaderResult()
        {
            if (!IsLogin) return PartialView("_Header", null); //return Redirect("/Home/Login");
            var header = new HeaderData { User = user };

            return PartialView("_Header", header);
        }

        public ActionResult SelectServer(Int32 id)
        {

            return Redirect("~/Home/Index/" + id);
        }

        #endregion

        #region Json

        [HttpGet]
        public JsonResult RealTimeGame(Int32 sid)
        {
            //var data = new ChartHome
            //{
            //    server = new List<int> { 612, 458, 235, 567 },
            //    pay = new List<int> { 10, 15, 255, 5405 },
            //};

            // POST api/Home?token={token}&sid={sid}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Home?token={0}&sid={1}", user.token, sid),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var data = CommonHelper.Deserialize<ChartHome>(result);

            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }

        #endregion

    }
}
