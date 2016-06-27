using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class PlayerController : ControllerBase
    {
        //
        // GET: /Player/

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("Home/Login");

            ViewBag.Platform = ApiPlatforms();
            return View(new PlayerDetailed());
        }

        /// <summary>详细信息</summary>
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (!IsLogin) return Redirect("Home/Login");

            var pid = collection["platform"];
            var sid = collection["server"];
            var type = collection["search_type"];
            var value = collection["value"];
            ViewBag.Platform = ApiPlatforms();

            if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请提交完整的信息";
                return View(new PlayerDetailed());
            }
            if (Convert.ToInt32(type) != 1 && Convert.ToInt32(type) != 2)
            {
                ViewBag.Error = -1;
                ViewBag.Message = "查询类型错误";
                return View(new PlayerDetailed());
            }
            if (string.IsNullOrEmpty(value))
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请输入查询内容";
                return View(new PlayerDetailed());
            }
            //Post api/Player?token={token}&name={name}&pid={pid}&sid={sid}&type={type}&value={value}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&name={1}&pid={2}&sid={3}&type={4}&value={5}",
                user.token, user.name, pid, sid, type, value),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var entity = CommonHelper.Deserialize<PlayerDetailed>(result);
            if (entity.result == -1)
            {
                ViewBag.Error = -1;
                ViewBag.Message = entity.message;
                return View(new PlayerDetailed());
            }
            return View(entity);
        }

        /// <summary>查看技能</summary>
        [HttpGet]
        public JsonResult AjaxSkills(Int32 sid, Int64 rid)
        {
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&rid={2}", user.token, sid, rid),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var list = CommonHelper.Deserialize<RoleSkill>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看装备</summary>
        [HttpGet]
        public JsonResult AjaxEquips(Int32 sid, Int64 rid)
        {
            //api/Player?token={token}&sid={sid}&rid={rid}&name={name}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&rid={2}&name={3}", user.token, sid, rid, user.name),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<PlayerBag>>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>游戏日志</summary>
        [HttpGet]
        public JsonResult AjaxLogs(Int32 sid, Int64 playerId, Int32 type, Int32 page, Int32 size)
        {
            //Post api/Player?token={token}&sid={sid}&playerId={playerId}&type={type}&name={name}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&playerId={2}&type={3}&name={4}&index={5}&size={6}", user.token, sid, playerId, type, user.name, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PageLog>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>元宝消耗</summary>
        [HttpGet]
        public JsonResult AjaxGolds(Int32 sid, Int64 playerId, Int32 page, Int32 size)
        {
            //Post api/Player?token={token}&sid={sid}&playerId={playerId}&type={type}&name={name}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&playerId={2}&type={3}&name={4}&index={5}&size={6}", user.token, sid, playerId, 3, user.name, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PageLog>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看属性</summary>
        [HttpGet]
        public JsonResult AjaxRole(Int32 sid, Int64 rid)
        {
            //Post api/Player?token={token}&sid={sid}&rid={rid}&name={name}&role={role}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&rid={2}&name={3}&role={4}", user.token, sid, rid, user.name, user.role),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PlayerRoles>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>元宝消耗明细</summary>
        [HttpGet]
        public JsonResult AjaxPercent(Int32 sid, Int64 playerId)
        {
            //Post api/Player?token={token}&sid={sid}&playerId={playerId}&role={role}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Player?token={0}&sid={1}&playerId={2}&role={3}", user.token, sid, playerId, user.role),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PlayerGoldPercent>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
