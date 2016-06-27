using System.Web.Optimization;

namespace TGM.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/js/jquery-1.11.1.js",
                        "~/js/jquery.nicescroll.js",
                        "~/assets/bootstrap/js/bootstrap.js",
                        "~/js/jquery.scrollTo.js",
                        "~/js/common-scripts.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                "~/assets/bootstrap/css/bootstrap.css",
                "~/assets/bootstrap/css/bootstrap-responsive.css",
                "~/assets/font-awesome/css/font-awesome.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/css/style.css",
                         "~/css/style-responsive.css",
                        "~/css/style-default.css"
                       ));
        }
    }
}