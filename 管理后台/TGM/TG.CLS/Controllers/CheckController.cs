using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Model;

namespace TG.CLS.Controllers
{
    public class CheckController : Controller
    {
        //
        // GET: /Check/

        /// <summary>账号是否存在</summary>
        /// <param name="user">所要检查的玩家账号，与登录接口一致(长度<=50)</param>
        /// <param name="domain">游戏服二级域名，例如：s1.tk2.ya247.com</param>
        /// <param name="sign">调用票据，按 md5(user_ domain _平台密钥) 算法生成的哈希值</param>
        [HttpGet]
        public Int32 Index(string user, string domain, string sign)
        {
            //XTrace.WriteLine("user:{0} domain:{1} sign:{2}", user, domain, sign);
            //查询对应域名服务器是否存在
            tgm_server.SetDbConnName(DBConnect.GetName(null));
            var server = tgm_server.GetGameServer(domain);
            if (server == null)
            {
                return 2;
                //return Json(new BaseEntity()
                //{
                //    result = 2,
                //    message = "服务器不存在，请确认游戏服域名正确并已被添加到后台",
                //}, JsonRequestBehavior.AllowGet);
            }
            //票据检查 md5(user_ domain _平台密钥)
            var encrypt = server.Platform.encrypt;
            var ck = string.Format("{0}_{1}_{2}", user, domain, encrypt);
            var md5 = UConvert.MD5(ck);
            var chksum = md5;
            if (sign != md5)
            {
                return 3;
//                return Json(new BaseEntity()
//                {
//                    result = 3,
//#if DEBUG
//                    message = "md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致" + chksum,
//#endif
//#if !DEBUG
//                    message = " md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致",
//#endif
//                }, JsonRequestBehavior.AllowGet);
            }

            //账号检查
            tg_user.SetDbConnName(DBConnect.GetName(server));
            var player = tg_user.GetEntityByCode(user);
            if (player == null)
            {
                return 4;
                //return Json(new BaseEntity()
                //{
                //    result = 4,
                //    message = "玩家账号未创建角色",
                //}, JsonRequestBehavior.AllowGet);
            }
            return 1;
            //return Json(new BaseEntity()
            //{
            //    result = 1,
            //    message = "成功，玩家账号存在",
            //}, JsonRequestBehavior.AllowGet);
        }

    }
}
