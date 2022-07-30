using System.Web;
using System.Web.Optimization;

namespace Multigroup.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                   "~/Scripts/Charts/jquery.tipsy.js",
                   "~/Scripts/Charts/protovis.js",
                   "~/Scripts/Charts/protovis-msie.js",
                   "~/Scripts/Charts/tipsy.js",
                   "~/Scripts/Charts/def.js",
                   "~/Scripts/Charts/pvc-r2.0.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/bootstrap-datepicker.es.min.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.bootstrap.js",
                      "~/Scripts/quick-sidebar.js",
                      "~/Scripts/dataTables.tableTools.js",
                      "~/Scripts/dataTables.responsive.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/portlet-draggable.js",
                      "~/Scripts/jquery-migrate.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/dashboard.js",
                      "~/Scripts/layout.js",
                      "~/Scripts/jquery.flip.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/nprogress.js",
                      "~/Scripts/bootstro.js",
                      "~/Scripts/icheck.js",
                      "~/Scripts/moment.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/componentes.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/plugins.css",
                      "~/Content/theme.css",
                      "~/Content/Site.css",
                      "~/Content/highslide.css"));
        }
    }
}
