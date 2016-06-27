using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Model;

namespace TG.CLS.Controllers
{
    public class PayController : Controller
    {
        //
        // GET: /Pay/

        public ActionResult Recharge(String domain)
        {
            //var domain = String.Format("{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);
            if (String.IsNullOrEmpty(domain))
            {
                return View(new BaseEntity
                {
                    result = 1,
                    message = "充值请求错误",
                });
            }
            tgm_server.SetDbConnName(DBConnect.GetName(null));
            var server = tgm_server.GetGameServer(domain);
            if (server == null)
            {
                return View(new BaseEntity
                {
                    result = 1,
                    message = "服务器不存在，请确认游戏服域名正确并已被添加到后台",
                });
            }
            return Redirect(server.game_pay);
        }

        /// <summary>充值接口</summary>
        /// <param name="user">玩家的唯一标识，字符串类型，只要可以唯一标识一个玩家即可，要于登录接口一致(长度<=50)</param>
        /// <param name="money">充值的金额，一个正整数</param>
        /// <param name="order">本次充值在平台上的订单号，字符串类型，需要可以唯一标识这次充值</param>
        /// <param name="domain">所要充值的游戏服所对应的二级域名，例如：s1.tk2.ya247.com</param>
        /// <param name="sign">充值票据，字符串类型，按 md5(user_money_order_domain_平台密钥) 算法生成的哈希值(小写)，user请先urlencode后再放入加密串</param>
        [HttpGet]
        public Int32 Index(String user, Int32 money, String order, string domain, String sign)
        {
            //查询对应域名服务器是否存在
            tgm_server.SetDbConnName(DBConnect.GetName(null));
            var server = tgm_server.GetGameServer(domain);
            if (server == null)
            {
                return 2;
                //return Json(new BaseEntity()
                //{
                //    result = 2,
                //    message = "充值的服务器不存在，请确认游戏服域名正确并已被添加到后台",
                //}, JsonRequestBehavior.AllowGet);
            }
            //票据检查 md5(user_money_order_domain_平台密钥)
            var encrypt = server.Platform.encrypt;
            var ck = string.Format("{0}_{1}_{2}_{3}_{4}", user, money, order, domain, encrypt);
            var md5 = UConvert.MD5(ck);
            var chksum = md5;
            if (sign != md5)
            {
                return 5;
                //                return Json(new BaseEntity()
                //                {
                //                    result = 5,
                //#if DEBUG
                //                    message = "md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致" + chksum,
                //#endif
                //#if !DEBUG
                //                    message = "md5错误，请确认密钥正确，充值票据算法跟文档描述一致，参与票据计算的参数于传递给接口的参数一致",
                //#endif
                //                }, JsonRequestBehavior.AllowGet);
            }

            //订单号验证
            tgm_record_pay.SetDbConnName(DBConnect.GetName(null));
            var _pay = tgm_record_pay.FindCount(string.Format("order_id='{0}'", order), null, null, 0, 0);
            if (_pay > 0) return 9;
            //账号检查
            tg_user.SetDbConnName(DBConnect.GetName(server));
            var player = tg_user.GetEntityByCode(user);
            if (player == null)
            {
                return 7;
                //return Json(new BaseEntity()
                //{
                //    result = 7,
                //    message = "不存在此账号，请确认用户名和登录接口传递的是一致的",
                //}, JsonRequestBehavior.AllowGet);
            }

            //令牌|游戏服编号|玩家账号|订单号|渠道|充值类型|充值数值|平台加密字符串
            var param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
               server.Platform.token, server.id, user, order, 1, (int)PayType.RMB, money);
            var chk = string.Format("{0}|{1}", param, encrypt);
            //加密字符

            var api_chksum = UConvert.MD5(chk);
            //POST api/Common?param={param}&checksum={checksum}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?param={0}&checksum={1}", param, api_chksum),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var be = CommonHelper.Deserialize<BaseEntity>(result);

            if (be.result == (int)ApiType.OK)
            {
                return 1;
                //return Json(new BaseEntity()
                //{
                //    result = 1,
                //    message = "充值成功",
                //}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return be.result;
                //return Json(be, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
