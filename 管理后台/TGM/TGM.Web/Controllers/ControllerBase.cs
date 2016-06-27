using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ControllerBase : Controller
    {
        #region 属性
        /// <summary>登陆用户信息</summary>
        private User _user;
        /// <summary>登陆用户信息</summary>
        internal User user
        {
            get
            {
                var model = Session["user"] as User;
                return model;
            }
            set { Session["user"] = value; }
        }

        /// <summary>判断用户是否登陆</summary>
        internal Boolean IsLogin
        {
            get
            {
                return user != null;
            }

        }

        /// <summary>选择服务器信息</summary>
        private Server _server;
        /// <summary>选择服务器信息</summary>
        internal Server server
        {
            get
            {
                var model = Session["server"] as Server;
                return model;
            }
            set { Session["server"] = value; }
        }

        /// <summary>是否选择服务器</summary>
        internal Boolean IsServer
        {
            get
            {
                return server != null;
            }
        }
        #endregion

        #region 扩展公共方法

        /// <summary>获取平台数据</summary>
        internal List<Platform> ApiPlatforms()
        {
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?token={0}&pid={1}&role={2}", user.token, user.pid, user.role),
            };
            var result = api.PostJsonToParameter();
            return CommonHelper.Deserialize<List<Platform>>(result);
        }

        /// <summary>获取启服服务器</summary>
        internal List<Server> ApiFindServer()
        {
            //api/System?token={token}
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/System?token={0}&flag=true", user.token),
            };
            var result = api.PostJsonToParameter();
            var list = CommonHelper.Deserialize<List<Server>>(result);

            return list;
        }

       
        #endregion
    }
}