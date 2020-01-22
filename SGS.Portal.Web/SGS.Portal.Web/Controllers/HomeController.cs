using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [CustAuthFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
