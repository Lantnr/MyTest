using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Base;
using TGM.API.Entity.Model;
using TGM.Web.Helper;
using GoodsType = TGG.Core.Enum.Type.GoodsType;

namespace TGM.Web.Controllers
{
    public class ResourceController : ControllerBase
    {
        // GET: /Resource/
        //

        public ActionResult Index()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            ViewBag.Platform = ApiPlatforms(); ;
            ViewBag.Error = 1;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");

            var platform = collection["platform"];
            var sid = collection["server"];
            var playname = collection["playname"];
            var gift = collection["gift"]; //礼包名称
            var gifttype = collection["gifttype"]; //礼包类型 1：全服礼包 2：个人礼包
            var m1 = collection["m_1"];
            var m2 = collection["m_2"];
            var m3 = collection["m_3"];
            var m4 = collection["m_4"];
            var m5 = collection["m_5"];

            var n1 = collection["name_1"];
            var n2 = collection["name_2"];
            var n3 = collection["name_3"];
            var n4 = collection["name_4"];
            var n5 = collection["name_5"];

            var c1 = collection["count_1"];
            var c2 = collection["count_2"];
            var c3 = collection["count_3"];
            var c4 = collection["count_4"];
            var c5 = collection["count_5"];

            var p1 = collection["pin_1"];
            var p2 = collection["pin_2"];
            var p3 = collection["pin_3"];
            var p4 = collection["pin_4"];
            var p5 = collection["pin_5"];

            var t11 = collection["att_1_1"];
            var t12 = collection["att_1_2"];
            var t13 = collection["att_1_3"];
            var av11 = collection["att_v_1_1"];
            var av12 = collection["att_v_1_2"];
            var av13 = collection["att_v_1_3"];

            var t21 = collection["att_2_1"];
            var t22 = collection["att_2_2"];
            var t23 = collection["att_2_3"];
            var av21 = collection["att_v_2_1"];
            var av22 = collection["att_v_2_2"];
            var av23 = collection["att_v_2_3"];

            var t31 = collection["att_3_1"];
            var t32 = collection["att_3_2"];
            var t33 = collection["att_3_3"];
            var av31 = collection["att_v_3_1"];
            var av32 = collection["att_v_3_2"];
            var av33 = collection["att_v_3_3"];

            var t41 = collection["att_4_1"];
            var t42 = collection["att_4_2"];
            var t43 = collection["att_4_3"];
            var av41 = collection["att_v_4_1"];
            var av42 = collection["att_v_4_2"];
            var av43 = collection["att_v_4_3"];

            var t51 = collection["att_3_1"];
            var t52 = collection["att_3_2"];
            var t53 = collection["att_3_3"];
            var av51 = collection["att_v_3_1"];
            var av52 = collection["att_v_3_2"];
            var av53 = collection["att_v_3_3"];

            var reason = collection["reason"];
            var type = collection["type"];
            var message = collection["message"];
            var list = ApiPlatforms();

            if (Convert.ToInt32(m1) == 0 && Convert.ToInt32(m2) == 0 && Convert.ToInt32(m3) == 0 &&
                Convert.ToInt32(m4) == 0 && Convert.ToInt32(m5) == 0)
            {
                ViewBag.Error = -1;
                ViewBag.Message = "没选择资源";
                ViewBag.Platform = list;
                return View();
            }

            if (c1 != "" && !Regex.IsMatch(c1, @"^\d+$") || c2 != "" && !Regex.IsMatch(c2, @"^\d+$") || c3 != "" && !Regex.IsMatch(c3, @"^\d+$") ||
                c4 != "" && !Regex.IsMatch(c4, @"^\d+$") || c5 != "" && !Regex.IsMatch(c5, @"^\d+$") ||
                av11 != "" && !Regex.IsMatch(av11, @"^\d+$") || av12 != "" && !Regex.IsMatch(av12, @"^\d+$") || av13 != "" && !Regex.IsMatch(av13, @"^\d+$")
                || av21 != "" && !Regex.IsMatch(av21, @"^\d+$") || av22 != "" && !Regex.IsMatch(av22, @"^\d+$") || av23 != "" && !Regex.IsMatch(av23, @"^\d+$") ||
                av31 != "" && !Regex.IsMatch(av31, @"^\d+$") || av32 != "" && !Regex.IsMatch(av32, @"^\d+$") || av33 != "" && !Regex.IsMatch(av33, @"^\d+$") ||
                av41 != "" && !Regex.IsMatch(av41, @"^\d+$") || av42 != "" && !Regex.IsMatch(av42, @"^\d+$") || av43 != "" && !Regex.IsMatch(av43, @"^\d+$") ||
                av51 != "" && !Regex.IsMatch(av51, @"^\d+$") || av52 != "" && !Regex.IsMatch(av52, @"^\d+$") || av53 != "" && !Regex.IsMatch(av53, @"^\d+$"))
            {
                ViewBag.Error = -1;
                ViewBag.Message = "数据格式错误";
                ViewBag.Platform = list;
                return View();
            }
            if(Convert.ToInt32(m1)>0&&c1==""||Convert.ToInt32(m2)>0&&c2==""||Convert.ToInt32(m3)>0&&c3==""||Convert.ToInt32(m4)>0&&c4==""||Convert.ToInt32(m5)>0&&c5=="")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "某物品没填数量";
                ViewBag.Platform = list;
                return View();
            }
            var b = new BaseEquip();
            if (!CheckEquip(Convert.ToInt32(m1), n1, p1, ref b))
            {
                ViewBag.Error = -1;
                ViewBag.Message = b.name + "没有该品质";
                ViewBag.Platform = list;
                return View();
            }
            if (!CheckEquip(Convert.ToInt32(m2), n2, p2, ref b))
            {
                ViewBag.Error = -1;
                ViewBag.Message = b.name + "没有该品质";
                ViewBag.Platform = list;
                return View();
            }
            if (!CheckEquip(Convert.ToInt32(m3), n3, p3, ref b))
            {
                ViewBag.Error = -1;
                ViewBag.Message = b.name + "没有该品质";
                ViewBag.Platform = list;
                return View();
            }
            if (!CheckEquip(Convert.ToInt32(m4), n4, p4, ref b))
            {
                ViewBag.Error = -1;
                ViewBag.Message = b.name + "没有该品质";
                ViewBag.Platform = list;
                return View();
            }
            if (!CheckEquip(Convert.ToInt32(m5), n5, p5, ref b))
            {
                ViewBag.Error = -1;
                ViewBag.Message = b.name + "没有该品质";
                ViewBag.Platform = list;
                return View();
            }
            string s = "";
            var s1 = GetString(Convert.ToInt32(m1), n1, c1, p1, t11, t12, t13, av11, av12, av13);
            var s2 = GetString(Convert.ToInt32(m2), n2, c2, p2, t21, t22, t23, av21, av22, av23);
            var s3 = GetString(Convert.ToInt32(m3), n3, c3, p3, t31, t32, t33, av31, av32, av33);
            var s4 = GetString(Convert.ToInt32(m4), n4, c4, p4, t41, t42, t43, av41, av42, av43);
            var s5 = GetString(Convert.ToInt32(m5), n5, c5, p5, t51, t52, t53, av51, av52, av53);
            if (platform == "" || sid == "")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请选择平台和服务器";
                ViewBag.Platform = list;
                return View();
            }
            if (gifttype == "0")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "没有选择申请类型";
                ViewBag.Platform = list;
                return View();
            }
            if (playname != "" && gifttype == "1")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "选择了玩家不能选择全服礼包类型";
                ViewBag.Platform = list;
                return View();
            }

            var resource = ApiFindResource(user.token, Convert.ToInt64(platform), Convert.ToInt64(sid), user.id,
                playname, gift, Convert.ToInt32(gifttype), s1, s2, s3, s4, s5, reason, Convert.ToInt32(type),message);
            if (resource.result != 1)
            {
                ViewBag.Error = -1;
                ViewBag.Message = resource.message;
                ViewBag.Platform = list;
                return View();
            }

            ViewBag.Platform = list;
            return View();
        }

        public string GetString(int type, string name, string count, string pin, string at1, string at2, string at3, string atv1, string atv2, string atv3)
        {
            string s = "";
            if (type != 0)
            {
                if (count == "")
                    return s;
                switch (type)
                {
                    case (int)GoodsType.TYPE_GOLD:
                    case (int)GoodsType.TYPE_COIN:
                    case (int)GoodsType.TYPE_EXP:
                    case (int)GoodsType.TYPE_HONOR:
                    case (int)GoodsType.TYPE_FAME:
                    case (int)GoodsType.TYPE_SPIRIT:
                    case (int)GoodsType.TYPE_MERIT:
                    case (int)GoodsType.TYPE_DONATE:
                        s = string.Format("{0}_{1}", type, count);
                        break;
                    case (int)GoodsType.TYPE_PROP:
                        {
                            var baseprop = FixedResources.BASE_PROP.FirstOrDefault(m => m.id == Convert.ToInt32(name));
                            if (baseprop != null)
                            {
                                s = string.Format("{0}_{1}_{2}", type, baseprop.id, count);
                            }
                        }
                        break;
                    case (int)GoodsType.TYPE_FUSION:
                        {
                            var basefusion = FixedResources.BASE_FUSION.FirstOrDefault(m => m.id == Convert.ToInt32(name));
                            if (basefusion != null)
                                s = string.Format("{0}_{1}_{2}", type, basefusion.id, count);
                        }
                        break;
                    case (int)GoodsType.TYPE_EQUIP:
                        {
                            BaseEquip baseequip;
                            if (pin.Length != 0)
                            {
                                var baseequip1 =FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == Convert.ToInt32(name));
                                if (baseequip1 == null) return s;
                                baseequip = FixedResources.BASE_EQUIP.FirstOrDefault(m => m.name == baseequip1.name && m.grade == Convert.ToInt32(pin));
                                if (baseequip == null)
                                {
                                    return s;
                                }
                            }
                            else
                            {
                                baseequip = FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == Convert.ToInt32(name));
                            }
                            if (baseequip == null) return null;

                            var v = CheckAtt(at1, at2, at3, atv1, atv2, atv3);
                            s = string.Format("{0}_{1}_{2}_{3}", type, baseequip.id, 0, count);   //7_6010056_0_1_6-180,10-400_1
                            if (v != "")
                                s += "_" + v;
                        }
                        break;
                }
            }
            return s;
        }

        public string CheckAtt(string at1, string at2, string at3, string atv1, string atv2, string atv3)
        {
            string s = "";
            if (at1 != "0")
            {
                if (atv1 != "")
                    s += at1 + "-" + atv1;
            }
            if (s.Contains("-"))
            {
                if (at2 != "0")
                    if (atv2 != "")
                        s += "," + at2 + "-" + atv2;
            }
            else
            {
                if (atv2 != "")
                    s += at2 + "-" + atv2;
            }
            if (s.Contains("-"))
            {
                if (at3 != "0")
                    if (atv3 != "")
                        s += "," + at3 + "-" + atv3;
            }
            else
            {
                if (atv3 != "")
                    s += at3 + "-" + atv3;
            }
            return s;
        }

        public bool CheckEquip(int type, string name, string pin, ref BaseEquip b)
        {
            if (type == (int)GoodsType.TYPE_EQUIP)
            {
                if (pin.Length > 0)
                {
                    var baseequip1 =
                        FixedResources.BASE_EQUIP.FirstOrDefault(
                            m => m.id == Convert.ToInt32(name));
                    if (baseequip1 == null) return false;
                    var baseequip =
                        FixedResources.BASE_EQUIP.FirstOrDefault(
                            m => m.name == baseequip1.name && m.grade == Convert.ToInt32(pin));
                    if (baseequip == null)
                    {
                        baseequip = FixedResources.BASE_EQUIP.FirstOrDefault(m => m.name == name);
                        b = baseequip;
                        return false;
                    }
                }
            }
            return true;
        }

        public Resource ApiFindResource(Guid token, Int64 pid, Int64 sid, Int32 roleid, string playername, string gift, int gifttype, string g1, string g2, string g3, string g4, string g5, string reason, int type,string message)
        {
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Resource?token={0}&pid={1}&sid={2}&roleid={3}&playername={4}&gift={5}&gifttype={6}&g1={7}&g2={8}&g3={9}&g4={10}&g5={11}&reason={12}&type={13}&message={14}", token, pid, sid, roleid, playername, gift, gifttype, g1, g2, g3, g4, g5, reason, type, message),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            return CommonHelper.Deserialize<Resource>(result);
        }

        public ActionResult Approval()
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Resource?token={0}&roleid={1}", user.token, user.id),
            };
            var result1 = api.PostJsonToParameter();
            api.Dispose();
            var list1 = CommonHelper.Deserialize<List<Resource>>(result1);
            ViewBag.Resource = list1;

            var list2 = ApiPlatforms();
            ViewBag.Platform = list2;
            return View();
        }


        [HttpPost]
        public ActionResult Approval(FormCollection collection)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            var platform = collection["platform"];
            var sid = collection["server"];
            var type = collection["type"];
            if (platform == "" || sid == "")
            {
                ViewBag.Error = -1;
                ViewBag.Message = "请选择平台和服务器";
                ViewBag.Platform = new List<Platform>();
                ViewBag.Resource = new List<Resource>();
                return View();
            }
            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Resource?token={0}&pid={1}&sid={2}&roleid={3}&state={4}", user.token, Convert.ToInt64(platform), Convert.ToInt64(sid), user.id, type),
            };
            var result = api.PostJsonToFromBody();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<Resource>>(result);
            ViewBag.Resource = list;
            var list2 = ApiPlatforms();
            ViewBag.Platform = list2;
            return View();
        }

        public ActionResult Update1(Int64 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");

            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/Resource?token={0}&id={1}&name={2}", user.token, id, user.name),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Resource>(result);
            ViewBag.operation = user.name;
            if (rm.result == 1) return Redirect("/Resource/Approval");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/Resource/Approval");
        }

        public ActionResult Update2(Int64 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");

            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/Resource?token={0}&id={1}&name={2}&flag={3}", user.token, id, user.name, true),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Resource>(result);

            if (rm.result == 1) return Redirect("/Resource/Approval");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/Resource/Approval");
        }

        public ActionResult Delete(Int64 id)
        {
            if (!IsLogin) return Redirect("/Home/Login");
            if (user == null) return Redirect("/Home/Login");

            var api = new ApiReceive
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = String.Format("api/Resource?token={0}&id={1}&flag={2}", user.token, id, true),
            };

            var result = api.PostJsonToFromBody();
            api.Dispose();
            var rm = CommonHelper.Deserialize<Resource>(result);

            if (rm.result == 1) return Redirect("/Resource/Approval");
            ViewBag.Error = -1;
            ViewBag.Message = rm.message;
            return Redirect("/Resource/Approval");
        }

        /// <summary>查看道具</summary>
        [HttpGet]
        public JsonResult AjaxProps(Int32 id)
        {
            var list = FixedResources.BASE_PROP;
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看装备</summary>
        [HttpGet]
        public JsonResult AjaxEquips(Int32 id)
        {
            var list = FixedResources.BASE_EQUIP;
            //list = GetList(list);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看熔炼道具</summary>
        [HttpGet]
        public JsonResult AjaxFusions(Int32 id)
        {
            var list = FixedResources.BASE_FUSION;
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>查看熔炼道具</summary>
        [HttpGet]
        public JsonResult AjaxPin1(Int32 id)
        {
            var list = FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == id);
            if (list != null)
            {
                var lists = FixedResources.BASE_EQUIP.Where(m => m.name == list.name).ToList();
                var jsonResult = Json(lists, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            return null;
        }

        public List<BaseEquip> GetList(List<BaseEquip> list)
        {
            string b = "";
            foreach (var item in list)
            {
                if (b == item.name)
                {
                    list.Remove(item);
                }
                else
                {
                    b = item.name;
                }

            }
            return list;
        }
    }
}
