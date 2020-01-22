using System.Web;
using System.Web.Optimization;

namespace SGS.Portal.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* CSS */
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/App_Themes/css/bootstrap.css")
                .Include("~/App_Themes/css/font-awesome.min.css")
                .Include("~/App_Themes/css/bootstrap-datetimepicker.css")      
                .Include("~/App_Themes/css/styles.css")
                );

            bundles.Add(new StyleBundle("~/bundles/payroll_css")
                .Include("~/App_Themes/css/bootstrap.css")
                .Include("~/App_Themes/lib/fontawesome/css/all.css")
                .Include("~/App_Themes/css/bootstrap-datetimepicker.css")
                .Include("~/App_Themes/css/styles.css")
                );

            /* JS */
            bundles.Add(new ScriptBundle("~/bundles/js")
                //.Include("~/App_Themes/js/jquery-1.11.1.min.js")
                .Include("~/App_Themes/js/jquery-3.3.1.min.js")
                //.Include("~/App_Themes/js/popper.min.js")
                .Include("~/App_Themes/js/datatables.min.js")
                .Include("~/App_Themes/js/bootstrap.min.js")
                //.Include("~/App_Themes/js/chart.min.js")
                //.Include("~/App_Themes/js/easypiechart.js")
                //.Include("~/App_Themes/js/easypiechart-data.js")
                .Include("~/App_Themes/js/moment.js")
                .Include("~/App_Themes/js/bootstrap-datetimepicker.min.js")
                .Include("~/App_Themes/js/jquery.maskedinput.min.js")
                .Include("~/App_Themes/js/custom.js")
                );

            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;
        }
    }
}