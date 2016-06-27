using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;


namespace TGM.Web.Controllers
{
    public partial class SystemController : ControllerBase
    {
        #region 平台管理
        //
        // GET: /System/

        public ActionResult Index(Int32 index = 1, Int32 size = 10)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            //POST api/System?token={token}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&index={1}&size={2}", user.token, index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerPlatform>(result);
            var query = new PagerQuery<PagerInfo, List<Platform>>(list.Pager, list.Platforms);
            ViewBag.user = user;
            return View(query);
        }

        /// <summary>注册平台</summary>
        public ActionResult Platform()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            return View();
        }


        /// <summary>注册平台</summary>
        [HttpPost]
        public ActionResult Platform(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            try
            {
                var platform = collection["platform_name"];
                var name = collection["role_name"];
                var pwd = collection["role_password"];
                var cpwd = collection["role_confirmpassword"];

                if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(cpwd))
                {
                    ViewBag.Error = -2;
                    ViewBag.Message = "用户名或密码为空";
                    return View();
                }
                if (pwd != cpwd)
                {
                    ViewBag.Error = -3;
                    ViewBag.Message = "两次密码不一致";
                    return View();
                }
                //POST api/System?token={token}&pname={pname}&name={name}&pwd={pwd}&b={b}
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&pname={1}&name={2}&pwd={3}&b={4}", user.token, platform, name, pwd, true),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<Platform>(result);

                if (rm.result == 1) return Redirect("/System/Index");

                ViewBag.Error = -1;
                ViewBag.Message = rm.message;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary> 删除</summary>
        public ActionResult DeletePlatform(Int32 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");
            if (id == 0) return Redirect("/System/Index");

            //POST api/System?token={token}&pid={pid}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/System?token={0}&pid={1}", user.token, id),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Platform>(result);

            if (rm.result == 1) return Redirect("/System/Index");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/System/Index");
        }

        #endregion

        #region 账号管理

        /// <summary>管理</summary>
        public ActionResult Account(Int32 index = 1, Int32 size = 10)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&role={1}&pid={2}&index={3}&size={4}"
                , user.token, user.role, user.pid, index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerUser>(result);
            var query = new PagerQuery<PagerInfo, List<User>>(list.Pager, list.Users);
            return View(query);
        }

        /// <summary>注册账号</summary>
        public ActionResult Register()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            return View(user);
        }

        /// <summary>注册账号</summary>
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            try
            {
                var pid = collection["pid"];
                var role = collection["role_type"];
                var name = collection["role_name"];
                var pwd = collection["role_password"];
                var cpwd = collection["role_confirmpassword"];

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(cpwd))
                {
                    ViewBag.Error = -2;
                    ViewBag.Message = "用户名或密码为空";
                    return View(user);
                }
                if (pwd != cpwd)
                {
                    ViewBag.Error = -3;
                    ViewBag.Message = "两次密码不一致";
                    return View(user);
                }
                //POST api/System?token={token}&name={name}&pid={pid}&role={role}&pwd={pwd}
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&name={1}&pid={2}&role={3}&pwd={4}", user.token, name, pid, role, cpwd),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<User>(result);

                if (rm.result == 1) return Redirect("/System/Account");

                ViewBag.Error = -1;
                ViewBag.Message = rm.message;
                return View(user);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        /// <summary> 删除</summary>
        public ActionResult Delete(int id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");
            if (id == 0) return Redirect("/System/Account");

            //POST api/System/{id}?token={token}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/System/{0}?token={1}", id, user.token),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<User>(result);

            if (rm.result == 1) return Redirect("/System/Account");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/System/Account");
        }

        #endregion

        #region 密码重置
        public ActionResult Reset()
        {
            return View();
        }

        /// <summary>密码重置</summary>
        [HttpPost]
        public ActionResult Reset(FormCollection collection)
        {
            try
            {
                if (!IsLogin) return Redirect("/Home/Login");
                var name = collection["Name"];
                var oldpwd = collection["OldPassWord"];
                var newpwd = collection["NewPassWord"];
                var cpwd = collection["ConfirmPassWord"];

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(oldpwd) || string.IsNullOrEmpty(newpwd)) return View();
                if (newpwd != cpwd)
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "新密码与确认密码不一致";
                    return View();
                }

                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource =
                        String.Format("api/System?token={0}&name={1}&old={2}&newpwd={3}", user.token, name, oldpwd,
                            newpwd),
                };

                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<User>(result);
                if (rm.result == 1)
                {
                    ViewBag.Error = 0;
                    return View();
                }
                ViewBag.Error = -1;
                ViewBag.Message = rm.message;
                return View();
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        #endregion

        #region 退出

        /// <summary>退出</summary>
        public ActionResult LignOut()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            Session.Add("user", null);
            Session.Add("server", null);
            return Redirect("/Home/Login");
        }

        #endregion

        #region 启服管理

        // GET: /System/

        public ActionResult Server(Int32 index = 1, Int32 size = 10)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            //POST api/System?token={token}&role={role}&pid={pid}&type={type}&index={index}&size={size}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&role={1}&pid={2}&type={3}&index={4}&size={5}"
                , user.token, user.role, user.pid, "", index, size),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<PagerServer>(result);
            var query = new PagerQuery<PagerInfo, List<Server>>(list.Pager, list.Servers);
            ViewBag.user = user;
            return View(query);
        }

        /// <summary>注册服务器</summary>
        public ActionResult RegisterServer()
        {
            if (!IsLogin) return Redirect("/Home/Login");

            ViewBag.Platform = ApiPlatforms();
            return View(user);
        }

        /// <summary>注册服务器提交</summary>
        [HttpPost]
        public ActionResult RegisterServer(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            try
            {
                var server_pid = collection["server_pid"];
                var server_name = collection["server_name"];
                var server_ip = collection["server_ip"];
                var server_port = collection["server_port"];
                var server_port_policy = collection["server_port_policy"];
                var server_connect_string = collection["server_connect_string"];
                var server_tg_route = collection["server_tg_route"];
                var server_tg_pay = collection["server_tg_pay"];
                var server_game_domain = collection["server_game_domain"];
                var server_game_pay = collection["server_game_pay"];

                if (string.IsNullOrEmpty(server_pid) || string.IsNullOrEmpty(server_name) || string.IsNullOrEmpty(server_ip)
                    || string.IsNullOrEmpty(server_port) || string.IsNullOrEmpty(server_port_policy) || string.IsNullOrEmpty(server_connect_string))
                {
                    ViewBag.Error = -2;
                    ViewBag.Message = "信息内容不完整";
                    ViewBag.Platform = ApiPlatforms();
                    return View();
                }
                //POST api/System?token={token}&name={name}&ip={ip}&pid={pid}&port={port}&policy={policy}&connect={connect}&tg_route={tg_route}&tg_pay={tg_pay}&game_domain={game_domain}&game_pay={game_pay}

                var sc = HttpUtility.UrlEncode(server_connect_string);
                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&name={1}&ip={2}&pid={3}&port={4}&policy={5}&connect={6}&tg_route={7}&tg_pay={8}&game_domain={9}&game_pay={10}"
                    , user.token, server_name, server_ip, server_pid, server_port, server_port_policy, sc,
                    server_tg_route, server_tg_pay, server_game_domain, server_game_pay),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<Server>(result);

                if (rm.result == 1) return Redirect("/System/Server");

                ViewBag.Error = -1;
                ViewBag.Message = rm.message;
                ViewBag.Platform = ApiPlatforms();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult ServerEdit(Int32 id)
        {
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

        [HttpPost]
        public ActionResult ServerEdit(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            try
            {
                var serverid = collection["serverid"];
                var server_name = collection["server_name"];
                var server_ip = collection["server_ip"];
                var server_port = collection["server_port"];
                var server_port_policy = collection["server_port_policy"];
                var server_connect_string = collection["server_connect_string"];
                var server_tg_route = collection["server_tg_route"];
                var server_tg_pay = collection["server_tg_pay"];
                var server_game_domain = collection["server_game_domain"];
                var server_game_pay = collection["server_game_pay"];

                var server = new Server
                {
                    id = Convert.ToInt32(serverid),
                    name = server_name,
                    ip = server_ip,
                    port_server = Convert.ToInt32(server_port),
                    port_policy = Convert.ToInt32(server_port_policy),
                    connect_string = server_connect_string,
                    tg_route = server_tg_route,
                    tg_pay = server_tg_pay,
                    game_domain = server_game_domain,
                    game_pay = server_game_pay,
                };

                if (string.IsNullOrEmpty(serverid) || string.IsNullOrEmpty(server_name) || string.IsNullOrEmpty(server_ip)
                    || string.IsNullOrEmpty(server_port) || string.IsNullOrEmpty(server_port_policy) || string.IsNullOrEmpty(server_connect_string))
                {
                    ViewBag.Error = -1;
                    ViewBag.Message = "信息内容不完整";
                    return View(server);
                }
                //  POST api/System?token={token}&id={id}&name={name}&ip={ip}&port={port}&policy={policy}&connect={connect}
                //  &tgroute={tgroute}&tgpay={tgpay}&gamedomain={gamedomain}&gamepay={gamepay}

                var sc = HttpUtility.UrlEncode(server_connect_string);

                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?token={0}&id={1}&name={2}&ip={3}&port={4}&policy={5}&connect={6}&tgroute={7}&tgpay={8}&gamedomain={9}&gamepay={10}",
                    user.token, server.id, server_name, server_ip, server_port, server_port_policy, sc,
                    server_tg_route, server_tg_pay, server_game_domain, server_game_pay),
                };
                var result = api.PostJsonToFromBody();
                api.Dispose();
                var rm = CommonHelper.Deserialize<BaseEntity>(result);

                if (rm.result == 1) return Redirect("/System/Server");

                ViewBag.Error = -2;
                ViewBag.Message = "修改服务器信息失败";
                return View(server);
            }
            catch
            {
                return Redirect("/System/Server");
            }
        }

        #endregion
    }
}
