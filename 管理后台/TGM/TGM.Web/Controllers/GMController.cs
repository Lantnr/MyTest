using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class GMController : ControllerBase
    {
        //
        // GET: /GM/

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");

            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        /// <summary>封号 冻结</summary>
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");

                var platform = collection["run_platform"];
                var sserver = collection["server_select"];
                var search = collection["search_type"];
                var value = collection["search"];
                var type = collection["silent_type"];
                var time = collection["time"];
                var reason = collection["reason"];
                ViewBag.Platform = ApiPlatforms();

                if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(sserver) || string.IsNullOrEmpty(search) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(reason))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "请将信息填写完整";
                    return View();
                }
                if (Convert.ToInt32(type) == 1)
                {
                    if (string.IsNullOrEmpty(time))
                    {
                        ViewBag.Error = -1;
                        ViewBag.Message = "冻结玩家请输入冻结时间";
                        return View();
                    }
                    if (!Regex.IsMatch(time, @"^\d+$"))
                    {
                        ViewBag.Error = -1;
                        ViewBag.Message = "时间控制输入格式错误，请重新输入";
                        return View();
                    }
                    if (Convert.ToInt32(time) < 0 || Convert.ToInt32(time) > 10000)
                    {
                        ViewBag.Error = -1;
                        ViewBag.Message = "时间设置错误或时间过大，请重新设置";
                        return View();
                    }
                }
                else { time = "0"; }
                //POST api/GM?token={token}&name={name}&platform={platform}&sid={sid}&
                //search={search}&value={value}&type={type}&time={time}&reason={reason}
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = string.Format("api/GM?token={0}&name={1}&pid={2}&sid={3}&search={4}&value={5}&type={6}&time={7}&reason={8}",
                    user.token, user.name, platform, sserver, search, value, type, time, reason),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var gm = CommonHelper.Deserialize<GmManage>(result);
                if (gm.result == -1)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = gm.message;
                    return View();
                }
                return Redirect("/GM/RecordList");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>GM操作记录</summary>
        public ActionResult RecordList(Int32 index = 1, Int32 size = 10)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms();
            ViewBag.Role = user.role;

            //api/GM?token={token}&role={role}&pid={pid}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/GM?token={0}&role={1}&pid={2}&index={3}&size={4}", user.token, user.role, user.pid, index, size),
            };

            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PageGmManage>(result);
            var query = new PagerQuery<PagerInfo, List<GmManage>>(list.Pager, list.Gms);

            if (query.result < 0)
            {
                ViewBag.Error = -1;
                ViewBag.Message = query.message;
                return View();
            }
            return View(query);
        }

        /// <summary>GM查询</summary>
        [HttpPost]
        public ActionResult RecordList(FormCollection collection)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");

                var pid = collection["platform"];
                var sid = collection["server"];
                var state = collection["state_type"];
                var type = collection["user_type"];
                var value = collection["value"];
                ViewBag.Platform = ApiPlatforms();
                ViewBag.Role = user.role;

                if (!string.IsNullOrEmpty(value))
                {
                    if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(type))
                    {
                        ViewBag.Error = -1;
                        ViewBag.Message = "查询玩家信息请选择完整依据";
                        return View(new PagerQuery<PagerInfo, List<GmManage>>(new PagerInfo(), new List<GmManage>()));
                    }
                }

                if (string.IsNullOrEmpty(sid)) { sid = "0"; }

                //api?GM/token={token}&name={name}&pid={pid}&sid={sid}&state={state}&type={type}&value={value}
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = string.Format("api/GM?token={0}&name={1}&pid={2}&sid={3}&state={4}&type={5}&value={6}", user.token, user.name, pid, sid, state, type, value),
                };

                var result = api.PostJsonToFromBody();
                var list = CommonHelper.Deserialize<PageGmManage>(result);
                var query = new PagerQuery<PagerInfo, List<GmManage>>(list.Pager, list.Gms);

                if (query.result < 0)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = query.message;
                    return View();
                }
                return View(query);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>解冻 解封</summary>
        [HttpGet]
        public ActionResult RescueGm(Int32 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (id == 0) return Redirect("/GM/RecordList");

            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/GM?token={0}&gid={1}", user.token, id),
            };
            var result = api.PostJsonToFromBody();
            var gm = CommonHelper.Deserialize<GmManage>(result);

            if (gm.result == 1) return Redirect("/GM/RecordList");
            ViewBag.Error = -1;
            ViewBag.Message = gm.message;
            return Redirect("/GM/RecordList");
        }
    }
}
