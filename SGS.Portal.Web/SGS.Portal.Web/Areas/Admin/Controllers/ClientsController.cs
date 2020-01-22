using SGS.Business.Controllers;
using SGS.Business.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Areas.Admin.Controllers
{
    public class ClientsController : Controller
    {
        //
        // GET: /Admin/Clients/

        public ActionResult Index()
        {
            List<ClientObject> model = ClientManager.Instance.GetClients().OrderBy(x => x.Name).ToList();
            return View(model);
        }

    }
}
