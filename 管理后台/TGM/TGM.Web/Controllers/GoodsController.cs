using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class GoodsController : ControllerBase
    {
        //
        // GET: /Goods/

        public ActionResult GoodsManage(Int32 index = 1, Int32 size = 10)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            //api/Goods?token={token}&role={role}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Goods?token={0}&role={1}&index={2}&size={3}", user.token, user.role, index, size),
            };

            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerGoodsType>(result);

            var query = new PagerQuery<PagerInfo, List<GoodsType>>(list.Pager, list.GoodsType);

            if (query.result >= 0) return View(query);

            ViewBag.Error = -1;
            ViewBag.Message = query.message;
            return View();
        }

        /// <summary>添加福利卡类型</summary>
        public ActionResult AddGoodsType()
        {
            if (!IsLogin) return Redirect("/Home/Login");

            return View();
        }

        [HttpPost]
        public ActionResult AddGoodsType(FormCollection collection)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");

                var type = collection["type"];
                var name = collection["name"];

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "添加失败，信息不完整";
                    return View();
                }
                if (!Regex.IsMatch(type, @"^\d+$"))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "福利卡枚举类型ID格式错误，请重新输入";
                    return View();
                }

                //api/Goods?token={token}&type={type}&name={name}         
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/Goods?token={0}&type={1}&name={2}", user.token, type, name),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<BaseEntity>(result);

                if (rm.result > 0) return Redirect("/Goods/GoodsManage");

                ViewBag.Error = rm.result;
                ViewBag.Message = rm.message;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>编辑福利卡类型信息</summary>
        public ActionResult GoodsTypeEdit(Int32 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            //POST api/Goods?token={token}&gid={gid}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Goods?token={0}&gid={1}&b={2}", user.token, id, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var entity = CommonHelper.Deserialize<GoodsType>(result);
            return View(entity);
        }

        [HttpPost]
        public ActionResult GoodsTypeEdit(FormCollection collection)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");

                var gid = collection["gid"];
                var type = collection["typeId"];
                var name = collection["name"];

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "请编辑完整信息";
                    return Redirect("/Goods/GoodsManage");
                }

                if (!Regex.IsMatch(type, @"^\d+$"))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "福利卡枚举类型ID格式错误，请重新输入";
                    return View();
                }

                var goodtype = new GoodsType()
                {
                    id = Convert.ToInt32(gid),
                    type_id = Convert.ToInt32(type),
                    name = name,
                };
                //POST api/System?token={token}&gid={gid}&typeId={typeId}&name={name}&b={b}
                var api = new ApiReceive()
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource =
                        string.Format("api/Goods?token={0}&gid={1}&typeId={2}&name={3}&b={4}", user.token, gid, type,
                            name, 0),
                };
                var result = api.PostJsonToParameter();
                api.Dispose();
                var entity = CommonHelper.Deserialize<BaseEntity>(result);

                if (entity.result == 1) return Redirect("/Goods/GoodsManage");
                ViewBag.Error = entity.result;
                ViewBag.Message = entity.message;
                return View(goodtype);
            }
            catch (Exception ex)
            {
                return Redirect("/Goods/GoodsManage");
            }
        }

        /// <summary>删除福利卡类型</summary>
        public ActionResult DeleteType(Int32 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            //Post api/Goods?token={token}&gid={gid}&role={role}&b={b}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Goods?token={0}&gid={1}&&role={2}&b={3}", user.token, id, user.role, 0),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var entity = CommonHelper.Deserialize<BaseEntity>(result);

            if (entity.result == 1) return Redirect("/Goods/GoodsManage");
            ViewBag.Error = -1;
            ViewBag.Message = entity.message;
            return Redirect("/Goods/GoodsManage");
        }
    }
}
