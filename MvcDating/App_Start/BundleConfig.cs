using System.Web.Optimization;

namespace MvcDating.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles) {
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Content/Scripts/jquery-{version}.js"
                        "~/Content/Scripts/jquery-1.10.2.min.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/ie").Include(
                        "~/Content/Scripts/modernizr-*",
                        "~/Content/Scripts/respond.src.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.unobtrusive*",
                        "~/Content/Scripts/jquery.validate*"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/main.less", 
                "~/Content/mobile.less",
                "~/Content/bootstrap.less"
            ));

            bundles.Add(new ScriptBundle("~/bundles/extra").Include(
                "~/Content/Scripts/bootstrap.js"
            ));

        }
    }
}