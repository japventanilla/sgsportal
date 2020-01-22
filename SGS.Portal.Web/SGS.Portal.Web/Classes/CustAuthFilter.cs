using SGS.Business.Controllers;
using SGS.Portal.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SGS.Portal.Web.Classes
{
    public class CustAuthFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            if (HttpContext.Current.Session["CurrentUser"] == null)
            {
               //HttpContext.Current.Session["CurrentUser"] = UserManager.Instance.Get(1);
               filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "", ReturnUrl = HttpContext.Current.Request.RawUrl }));
            }
        }
    }
}