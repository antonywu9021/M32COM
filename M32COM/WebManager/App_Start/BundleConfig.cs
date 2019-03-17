using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
namespace WebManager
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/js/jquery-1.9.1.min.js", "~/js/msgmanger.js", "~/js/select-ui.min.js"));
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/css/style.css", "~/css/select.css", "~/css/weui-min.css"));
            bundles.Add(new StyleBundle("~/iconfont/css").Include("~/iconfont/iconfont.css"));
            bundles.Add(new StyleBundle("~/fonts/treecss").Include("~/css/tree.css","~/css/font-awesome.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/treejs").Include("~/js/tree.js"));
        }
    }
}