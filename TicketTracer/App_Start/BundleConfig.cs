using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TicketTracer
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/json2.min.js",
                        "~/Scripts/jquery-ui-1.10.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/KnockOut").Include(
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/Kendo").Include(
                        "~/Scripts/kendo.web.min.js",
                        "~/Scripts/kendo.culture.it-IT.min.js",
                        "~/Scripts/Custom/Common.js"));


            


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendoStyle").Include(
                        "~/Content/kendo/2013.3.1119/kendo.silver.min.css",
                        "~/Content/kendo/2013.3.1119/kendo.common.min.css",
                        "~/Style/reveal.css"));
        }
    }
}