using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public partial class SystemController
    {
        /// <summary>平台编辑</summary>
        /// <param name="id">平台id</param>
        public ActionResult PlatformEdit(Int32 id)
        {
            //POST api/System?token={token}&pid={pid}&name={name}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&pid={1}&name={2}&b={3}", user.token, id, user.name, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var entity = CommonHelper.Deserialize<Platform>(result);
            return View(entity);
        }

        [HttpPost]
        public ActionResult PlatformEdit(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            try
            {
                var pid = collection["platform_id"];
                var pname = collection["platfrom_name"];
                var token = collection["platform_token"];
                var enctypy = collection["platform_encrtpy"];

                var platform = new Platform
                {
                    id = Convert.ToInt32(pid),
                    name = pname,
                    token = new Guid(token),
                    encrypt = enctypy,
                };

                if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pname) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(enctypy))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "信息内容不完整";
                    return View(platform);
                }
                //POST api/System?token={token}&pid={pid}&pname={pname}&encrypt={encrypt}&b={b}              
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&pid={1}&pname={2}&encrypt={3}&b={4}", user.token, pid, pname, enctypy, 0),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<BaseEntity>(result);

                if (rm.result == 1) return Redirect("/System/Index");

                ViewBag.Error = -2;
                ViewBag.Message = "修改平台信息失败";
                return View(platform);
            }
            catch (Exception ex)
            {
                return Redirect("/System/Index");
            }
        }

        /// <summary>福利卡信息</summary>
        public ActionResult GameGoods()
        {
            if (!IsLogin) return Redirect("/Home/Login");

            ViewBag.Platform = ApiPlatforms();
            ViewBag.User = user;
            ViewBag.GoodsType = ApiFindGoodsType();
            return View();
        }

        /// <summary>获取福利卡类型信息</summary>
        private List<GoodsType> ApiFindGoodsType()
        {
            //  POST api/Goods?token={token}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Goods?token={0}&flag=true", user.token),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<GoodsType>>(result);
            return list;
        }

        /// <summary>查询福利卡激活码信息</summary>
        [HttpGet]
        public JsonResult AjaxGoodsCode(Int32 pid, String order, Int32 type, Int32 page, Int32 size)
        {
            //Post api/System?token={token}&pid={pid}&order={order}&type={type}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&pid={1}&order={2}&type={3}&index={4}&size={5}", user.token, pid, order, type, page, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerServerGoodsCode>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>发放福利卡激活码</summary>
        public ActionResult GoodsCodeProvide()
        {
            if (!IsLogin) return Redirect("/Home/Login");

            ViewBag.Platform = ApiPlatforms();
            return View();
        }

        /// <summary>发放激活码信息</summary>
        [HttpPost]
        public ActionResult GoodsCodeProvide(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            try
            {
                var pid = collection["platform"];
                var type = collection["give_type"];
                var sid = collection["server"];
                var order = collection["order_number"];
                ViewBag.Platform = ApiPlatforms();

                if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(order) || string.IsNullOrEmpty(type))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "信息不完整，请填写完整信息";
                    return View();
                }

                //POST api/System?token={token}&pid={pid}&sid={sid}&order={order}&type={type}&b={b}             
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&pid={1}&sid={2}&order={3}&type={4}&b={5}", user.token, pid, sid, order, type, 0),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<BaseEntity>(result);

                ViewBag.Error = rm.result;
                ViewBag.Message = rm.message;
                return View();
            }
            catch (Exception ex)
            {
                return Redirect("/System/GameGoods");
            }
        }

        /// <summary>
        /// 生成福利卡激活码
        /// </summary>
        public JsonResult AjaxCreateCode(Int32 pid, String data, Int32 type, String number)
        {
            //POST api/System?token={token}&pid={pid}&data={data}&type={type}&number={number}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&pid={1}&data={2}&type={3}&number={4}&b={5}", user.token, pid, data, type, number, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<BaseEntity>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看平台福利卡发放记录</summary>
        [HttpGet]
        public JsonResult AjaxCodeLog(Int32 pid)
        {
            //api/System?token={token}&pid={pid}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&pid={1}&b={2}", user.token, pid, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<ServerCodeLog>>(result);

            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public FileResult AjaxExcel(FormCollection collection)
        {
            var pid = Convert.ToInt32(collection["pid"]);
            var excel_type = Convert.ToInt32(collection["excel_type"]);
            var excel_kind = collection["excel_kind"];

            //api/Common?pid={pid}&type={type}&order={order}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource =
                    string.Format("api/Common?pid={0}&type={1}&order={2}", pid, excel_type, excel_kind),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<ReportCode>>(result);

            if (!list.Any()) { list.Add(new ReportCode()); }

            var html = ExcelHelper.ToHtmlTable(list);

            var name = String.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.UTF8.GetBytes(html);
            return File(fileContents, "application/ms-excel", name);

            //第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "excel.xls");
        }

        /// <summary>管理服务器状态</summary>
        public ActionResult ManageServerState(int id)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            //POST api/System?token={token}&sid={sid}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&sid={1}&b={2}", user.token, id, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var entity = CommonHelper.Deserialize<Server>(result);
            return View(entity);
        }

        /// <summary>更改服务器状态</summary>
        [HttpPost]
        public ActionResult ManageServerState(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            var sid = collection["serverid"];
            var serverName = collection["server_name"];
            var stateName = collection["state_name"];
            var state = collection["server_state"];
            var startTime = collection["start_time"];
            var stateType = collection["state_type"];

            var nowServer = new Server()
            {
                name = serverName,
                id = Convert.ToInt32(sid),
                state_name = stateName,
                server_state = Convert.ToInt32(state),
            };
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(stateType) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(stateName))
            {
                ViewBag.Error = -1;
                ViewBag.Message = "信息内容不完整！";
                return View(nowServer);
            }
            if (Convert.ToInt32(state) == Convert.ToInt32(stateType))
            {
                ViewBag.Error = 1;
                ViewBag.Message = "服务器状态未更改！";
                return View(nowServer);
            }
            if (Convert.ToInt32(stateType) == (int)ServerOpenState.启服)
            {
                if (string.IsNullOrEmpty(startTime))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "服务器启服，请选择启服时间！";
                    return View(nowServer);
                }
                //var date = DateTime.Now;
                //var start = Convert.ToDateTime(startTime);
                //if (start <= date)
                //{
                //    ViewBag.Error = -1;
                //    ViewBag.Message = "启服时间应大于或略大于当前时间！";
                //    return View(nowServer);
                //}
            }
            //api/System?token={token}&sid={sid}&stateType={stateType}&startTime={startTime}&b={b}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/System?token={0}&sid={1}&stateType={2}&startTime={3}&b={4}", user.token, sid, stateType, startTime, 0),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<BaseEntity>(result);

            if (rm.result == 1) return Redirect("/System/Server");

            ViewBag.Error = rm.result;
            ViewBag.Message = rm.message;
            return View(nowServer);
        }
    }
}
