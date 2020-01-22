using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Admin/Default/

        [CustAuthFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
