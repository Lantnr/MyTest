using System;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public partial class RecordController : ControllerBase
    {
        /// <summary>元宝充值 消费记录</summary>
        public ActionResult GoldRecord()
        {
            if (!IsLogin) return Redirect("Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        [HttpGet]
        public JsonResult AjaxGold(Int32 sid, Int32 page, Int32 size)
        {
            //Post api/Record?token={token}&sid={sid}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&sid={1}&index={2}&size={3}", user.token, sid, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerServerGold>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>身份分布记录</summary>
        public ActionResult StruIdentity()
        {
            if (!IsLogin) return Redirect("Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        [HttpGet]
        public JsonResult AjaxIndetity(Int32 sid, Int32 page, Int32 size)
        {
            //Post api/Record?token={token}&sid={sid}&role={role}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&sid={1}&role={2}&index={3}&size={4}", user.token, sid, user.role, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerServerIdentity>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>等级分布记录</summary>
        public ActionResult StruLevel()
        {
            if (!IsLogin) return Redirect("Home/Login");
            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        [HttpGet]
        public ActionResult AjaxLevel(Int32 sid, Int32 page, Int32 size)
        {
            //Post api/Record?token={token}&pid={pid}&sid={sid}&name={name}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&pid={1}&sid={2}&name={3}&index={4}&size={5}", user.token, user.pid, sid, user.name, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerServerLevel>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>玩家查询</summary>
        public ActionResult QueryPlayer(Int32 sid)
        {
            if (!IsLogin) return Redirect("Home/Login");
            ViewBag.Sid = sid;
            return View();
        }

        [HttpGet]
        public JsonResult AjaxPlayer(Int32 sid, Int32 type, String value, Int32 page, Int32 size)
        {
            //Post api/Record?token={token}&name={name}&sid={sid}&type={type}&value={value}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Record?token={0}&name={1}&sid={2}&type={3}&value={4}&index={5}&size={6}", user.token, user.name, sid, type, value, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PageServerPlayer>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
