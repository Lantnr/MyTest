using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Common.Util;

namespace TG.CLS.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tools/

        public ActionResult Index()
        {
            return View(new ToolsEnitiy());
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var user = collection["user"];
            var domain = collection["domain"];
            var encrypt = collection["encrypt"];

            var ck = string.Format("{0}_{1}_{2}", user, domain, encrypt);
            var md5 = UConvert.MD5(ck);
            var chksum = md5;

            var url1 = string.Format("login?adult=1&time={0}&user={1}&sign={2}", domain, user, chksum);
            var url2 = string.Format("check?domain={0}&user={1}&sign={2}", domain, user, chksum);
            return View(new ToolsEnitiy
            {
                user = user,
                domain = domain,
                encrypt = encrypt,
                chksum = chksum,
                url1 = url1,
                url2 = url2,
            });
        }

    }

    public class ToolsEnitiy
    {
        public string user { get; set; }
        public string domain { get; set; }
        public string encrypt { get; set; }
        public string chksum { get; set; }

        public string url1 { get; set; }
        public string url2 { get; set; }
    }
}
