using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TG.CLS.Models;
using TGG.Core.Common.Util;
using TGM.API.Entity;
using TGM.API.Entity.Model;

namespace TG.CLS.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        /// <summary>登录接口</summary>
        /// <param name="user">玩家的唯一标识，字符串类型，只要可以唯一标识一个玩家即可，可以是平台的用户ID、用户名、Email等(长度<=50)</param>
        /// <param name="time">登录时间，时间戳类型，一个正整数，对应php中的time()函数的返回值，北京时间，如果登录服务器时间不是北京时间，请算好时差，登录时间用于登录票据的过期验证，目前票据过期时间为前后10分钟</param>
        /// <param name="adult">用户是否成年，数字类型，1表示成年，0表示未成年</param>
        /// <param name="sign">登录票据，字符串类型，按 md5(user_time_平台密钥) 算法生成的哈希值(小写)</param>
        [HttpGet]
        public ActionResult Index(String user, String sign, Int64 time = 0, Int32 adult = 1)
        {
            var info = new BaseEntity();
            if (Request.Url == null)
            {
                info.result = 404;
                info.message = "请求路径错误";
                return View(info);
            }

            //参数验证           
            if (String.IsNullOrEmpty(user) ||
                String.IsNullOrEmpty(sign)
                )
            {
                info.result = 1;
                info.message = string.Format("请求格式错误:?adult=1&time=0&user=***&sign=***");
                return View(info);
            }

            //时间戳验证
            TimeSpan delta = TimeSpan.FromMilliseconds(time*1000);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime curTime = startTime.Add(delta);
            DateTime dt = curTime;
            DateTime nowTime = DateTime.Now;
            var sub = nowTime.Subtract(dt).TotalSeconds;
            //有效时间10分钟 600秒
            if (sub < 0 || sub > 600) {
                info.result = 1;
                info.message = string.Format("登陆超时");
                return View(info);
            }
            
            //
            var domain = String.Format("{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);

            tgm_server.SetDbConnName(DBConnect.GetName(null));
            var server = tgm_server.GetGameServer(domain);
            if (server == null)
            {
                info.result = 1;
                info.message = string.Format("服务器不存在，请确认游戏服域名正确并已被添加到后台,请求格式参数:?adult=1&time=0&user=***&sign=***");
                return View(info);
            }

            if (server.server_state == 0 || server.server_state == 1)
            {
                info.result = 501;
                info.message = string.Format("服务器当前状态:{0}", server.server_state == 0 ? "未启服" : "停服");
                return View(info);
            }

            //票据检查  md5(user_time_平台密钥) 
            var encrypt = server.Platform.encrypt;
            var ck = string.Format("{0}_{1}_{2}", user, time, encrypt);
            var md5 = UConvert.MD5(ck);
            var chksum = md5;
            if (sign != md5)
            {
                info.result = 2;
#if DEBUG
                info.message = "md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致 " + chksum;
#endif
#if !DEBUG
                info.message = "md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致";
#endif
                return View(info);
            }

            //用户名加密 md5(user_game密钥) 
            var game_key = ConfigHelper.GetAppSettings("gamekey");
            var game_user = UConvert.CryptoString(CryptoHelper.Encrypt(user, game_key));

            var data = String.Format("userName={0}&isAdult={1}&serverIp={2}&port={3}&portPolicy={4}"
                 , game_user, adult, server.ip, server.port_server, server.port_policy);
            //var data = "userName=arlen0101&isAdult=1&connectServer=1&server=1&serverIp=192.168.1.254&port=10086&portPolicy=10087";

            //ViewBag.js = string.Format("\"{0}\"", data);

            //ViewBag.languageType = string.Format("{0}", ConfigHelper.GetAppSettings("languageType"));
            //ViewBag.resourceRootPath = string.Format("\"{0}\"", ConfigHelper.GetAppSettings("resourceRootPath"));

            //var _domain = String.Format("{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);

            //ViewBag.rechargetUrl = string.Format("\"{0}?domain={1}\"", ConfigHelper.GetAppSettings("rechargetUrl"), _domain);
            //ViewBag.homeUrl = string.Format("\"{0}\"", ConfigHelper.GetAppSettings("homeUrl"));

            var _data=String.Format("\"{0}\"", data);
            var _state = server.server_state;
            //TempData["data"] = 
            //TempData["state"] = server.server_state;

            Session.Clear();
            Session.Add("login", new LoginEntity { data = _data, state = _state });
            Session.Timeout = 60;
            return RedirectToAction("Index", "Home");

        }

       


    }
}
