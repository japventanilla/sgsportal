using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Areas.Payroll
{
    public class PayrollAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Payroll";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Payroll_default",
                "Payroll/{controller}/{action}/{id}",
                new { controller = "Payroll", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}