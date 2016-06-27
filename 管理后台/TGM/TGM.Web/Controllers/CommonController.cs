using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class CommonController : ControllerBase
    {
        //  Get Common/AjaxServer/1  或 Common/AjaxServer?id={id}
        /// <summary>动态加载服务器</summary>
        /// <param name="id">平台编号</param>
        /// <returns>服务器Json集合</returns>
        [HttpGet]
        public JsonResult AjaxServer(Int32 id)
        {
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?token={0}&pid={1}&flag={2}", user.token, id, true),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<Server>>(result);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }

        //  Get Common/AjaxCodes/1  或 Common/AjaxServer?id={id}&sid={sid}&type={type}
        /// <summary>动态加载平台福利卡激活码集合</summary>
        /// <param name="id">平台编号</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="type">福利卡类型</param>
        /// <returns>激活码Json集合</returns>
        [HttpGet]
        public JsonResult AjaxCodes(Int32 id, Int32 sid, Int32 type)
        {
            //POST api/Common?token={token}&pid={pid}&sid={sid}&type={type}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?token={0}&pid={1}&sid={2}&type={3}", user.token, id, sid, type),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<String>>(result);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        //  Get Common/AjaxCodeTypes/1  或 Common/AjaxServer?id={id}
        /// <summary>动态加载平台福利卡类型集合</summary>
        /// <param name="id">平台编号</param>
        /// <returns>类型Json集合</returns>
        [HttpGet]
        public JsonResult AjaxCodeTypes(Int32 id)
        {
            //POST api/Common?token={token}&name={name}&pid={pid}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?token={0}&name={1}&pid={2}&b={3}", user.token, user.name, id, 0),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<GoodsType>>(result);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}