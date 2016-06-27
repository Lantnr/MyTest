using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TGM.API.Entity;
using TGM.API.Entity.Model;
using TGM.Web.Helper;

namespace TGM.Web.Controllers
{
    public class TestController : ControllerBase
    {
        #region Arlen Test
        //
        // GET: /Test/

        public ActionResult Index()
        {

            ViewBag.Platform = new List<Platform>(); //GetPlatformAllList();
            return View();
        }

        /// <summary>获取所有平台数据</summary>
        private List<Platform> GetPlatformAllList()
        {
            //POST api/Common?token={token}&pid={pid}&role={role}
            var api = new ApiReceive()
            {
                URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                Resource = string.Format("api/Common?token={0}&pid={1}&role={2}", user.token, user.pid, user.role),
            };
            var result = api.PostJsonToParameter();
            api.Dispose();
            var list = CommonHelper.Deserialize<List<Platform>>(result);
            return list;
        }

        /// <summary>注册账号</summary>
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                var name = collection["Name"];
                var pwd = collection["PassWord"];
                var cpwd = collection["ConfirmPassWord"];

                if (pwd != cpwd) return View();

                var api = new ApiReceive
                {
                    URL = string.Format("{0}", ConfigHelper.GetApiUrl()),
                    Resource = String.Format("api/System?name={0}&pwd={1}&confirm={2}", name, pwd, cpwd),
                };

                var result = api.GetJsonToFromBody();
                api.Dispose();
                return View();
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }


        #endregion

        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult UserInfoQuery()
        {
            return View();
        }

        public ActionResult GmManage()
        {
            return View();
        }

        public FileResult Excel()
        {
            var list = new List<ReportCode>();
            for (var i = 0; i < 10; i++)
            {
                var sg = new ReportCode
                {
                   
                    激活码 = "DY" + DateTime.Now.Ticks.ToString(),
                    生成序号 = "12345",
                    平台名称 = "DY",
                    //服务器名称 = "S0",
                    福利卡类型 = "新手卡"
                };
                list.Add(sg);
            }


            var html = ExcelHelper.ToHtmlTable(list);

            var name =String.Format("{0}.xls",DateTime.Now.ToString("yyyyMMddHHmmss"));
            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.UTF8.GetBytes(html);
            return File(fileContents, "application/ms-excel", name);

            //第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "excel.xls");
        }
    }
}
