using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SGS.Portal.Web.Classes
{
    public class CustomAuthorize : AuthorizeAttribute
    {

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary(new
            {
                controller = "Account",
                action = "Login",
                area = "",
                redirect = HttpContext.Current.Request.Url.AbsolutePath.ToString()
            });
            filterContext.Result = new RedirectToRouteResult(routeValues);
        }
    }
}