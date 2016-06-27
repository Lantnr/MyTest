using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TG.CLS.Models;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Model;

namespace TG.CLS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            try
            {
                var session = Session["login"] as LoginEntity;
                var data = string.Empty;
                var state = 0;
                if (session != null)
                {
                    data = session.data;
                    state = session.state;
                }
                return Index(data, state);
            }
            catch {
                return Redirect("http://tk2.ya247.com");
            }
        }


        /// <summary>访问路径</summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(String data, Int32 state)
        {
            if (String.IsNullOrEmpty(data))
            {
                var domain = String.Format("{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);
                tgm_server.SetDbConnName(DBConnect.GetName(null));
                var server = tgm_server.GetGameServer(domain);
                if (server == null)
                {
                    return View(new BaseEntity
                    {
                        result = 1,
                        message = "服务器不存在，请确认游戏服域名正确",
                    });
                }
                return Redirect(server.tg_route);
            }

            var url = ConfigHelper.GetAppSettings("resourceRootPath");

            if (state == 2)
                url = ConfigHelper.GetAppSettings("testurl");

            ViewBag.js = string.Format("{0}", data);

            ViewBag.languageType = string.Format("{0}", ConfigHelper.GetAppSettings("languageType"));
            ViewBag.resourceRootPath = string.Format("{0}", url);
            ViewBag.index = string.Format("{0}index.swf", url);
            ViewBag.playerProductInstall = string.Format("{0}playerProductInstall.swf", url);
            var _domain = String.Format("{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);

            ViewBag.rechargetUrl = string.Format("{0}?domain={1}", ConfigHelper.GetAppSettings("rechargetUrl"), _domain);
            ViewBag.homeUrl = string.Format("{0}", ConfigHelper.GetAppSettings("homeUrl"));

            ViewBag.version = string.Format("{0}", ConfigHelper.GetAppSettings("gameversion"));
            

            return View(new BaseEntity());
        }
    }
}
