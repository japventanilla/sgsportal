using SGS.Business.Controllers;
using SGS.Business.Objects;
using SGS.Portal.Web.Areas.Payroll.Models;
using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Areas.Payroll.Controllers
{
    public class PayslipsController : Controller
    {
        //
        // GET: /Payroll/Payslips/

        [CustAuthFilter]
        [HttpGet]
        public ActionResult Index()
        {
            List<SelectListItem> selectClients = new List<SelectListItem>();
            var clients = ClientManager.Instance.GetClients().OrderBy(x => x.Name);
            foreach (ClientObject x in clients)
            {
                selectClients.Add(new SelectListItem { Text = x.Name, Value = x.Code });
            }
            ViewBag.SelectClients = selectClients;

            return View();
        }

        [CustAuthFilter]
        [HttpGet]
        public ActionResult Preview(string co, string c)
        {
            List<PayslipsObject> model = PayslipManager.Instance.GetPayslip(co, c);
            ClientObject client = ClientManager.Instance.GetClient(c);
            ViewBag.ClientCode = client != null ? client.Code : "";
            ViewBag.Client = client != null ? client.Name : "";
            ViewBag.CutOff = co;

            return View(model);
        }
    }
}
