using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TGM.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional }        
            //);
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "TGM.API.Areas.HelpPage" }
          ).DataTokens.Add("Area", "HelpPage");
        }
    }
}