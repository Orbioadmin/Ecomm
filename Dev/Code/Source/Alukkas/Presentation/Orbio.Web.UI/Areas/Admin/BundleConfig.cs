using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Orbio.Web.UI.Areas.Admin
{
    internal static class BundleConfig
    {
        internal static void RegisterBundlesAdministration(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/jqueryadmin").Include("~/Areas/Admin/Scripts/jquery.js",
                        "~/Areas/Admin/Scripts/jquery-{version}.js", "~/Areas/Admin/Scripts/bootstrap.min.js", "~/Areas/Admin/Scripts/bootstrap-multiselect.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryuiadmin").Include("~/Areas/Admin/Scripts/jquery.js", "~/Areas/Admin/Scripts/bootstrap-multiselect.js",
                        "~/Areas/Admin/Scripts/less.js", "~/Areas/Admin/Scripts/ie-emulation-modes-warning.js", "~/Areas/Admin/Scripts/ie10-viewport-bug-workaround.js", "~/Areas/Admin/Scripts/highcharts.js", "~/Areas/Admin/Scripts/kendo.all.min.js", "~/Areas/Admin/Scripts/jquery-ui.js", "~/Areas/Admin/Scripts/tabMenu.js", "~/Areas/Admin/Scripts/tinymce/tinymce.min.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizradmin").Include(
                        "~/Areas/Admin/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/cssadmin").Include("~/Areas/Admin/Content/bootstrap.min.css", "~/Areas/Admin/Content/style.css", "~/Areas/Admin/Content/*.css",
                "~/Areas/Admin/Content/kendo.common.min.css", "~/Areas/Admin/Content/kendo.default.min.css", "~/Areas/Admin/Content/kendo.dataviz.min.css", "~/Areas/Admin/Content/kendo.dataviz.default.min.css", "~/Areas/Admin/Content/jquery-ui.css", "~/Areas/Admin/Content/tabMenu.css"));
            new CssRewriteUrlTransform();
        }
    }
}