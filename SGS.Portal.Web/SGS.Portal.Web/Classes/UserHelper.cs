using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Classes
{
    public static class UserHelper
    {
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
                    return (User)HttpContext.Current.Session["CurrentUser"];
                else
                {
                    return null;
                }
            }
        }
    }
}