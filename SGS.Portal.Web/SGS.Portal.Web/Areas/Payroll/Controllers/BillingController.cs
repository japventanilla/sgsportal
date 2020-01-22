using SGS.Business.Controllers;
using SGS.Business.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SGS.Portal.Web.Areas.Payroll.Controllers
{
    public class BillingController : Controller
    {
        //
        // GET: /Payroll/Billing/

        [HttpGet]
        public string GetCutOffs(string c)
        {
            string cutoffs = ClientManager.Instance.GetClient(c).CutOffs;

            List<string> result = ClientManager.Instance.SetCutOffFilters(cutoffs, int.Parse(ConfigurationManager.AppSettings["NumOfCutOffs"].ToString()));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result);
        }

        public ActionResult Index()
        {

            List<SelectListItem> selectClients = new List<SelectListItem>();
            var clients = ClientManager.Instance.GetClients().OrderBy(x => x.Name);
            foreach (ClientObject x in clients)
            {
                selectClients.Add(new SelectListItem { Text = x.Name, Value = x.Code });
            }
            ViewBag.SelectClients = selectClients;


            //string cutoffs = ConfigurationManager.AppSettings["CutOffs"].ToString();
            //int numOfCutOffs = int.Parse(ConfigurationManager.AppSettings["NumOfCutOffs"].ToString());

            //List<SelectListItem> selectCutoffs = new List<SelectListItem>();
            //foreach (string x in TimeSheetManager.Instance.SetCutOffFilters(cutoffs, numOfCutOffs))
            //{
            //    selectCutoffs.Add(new SelectListItem { Text = x, Value = x });
            //}
            //ViewBag.SelectCutoffs = selectCutoffs;

            //List<SelectListItem> selectClients = new List<SelectListItem>();

            //var clients = ClientManager.Instance.GetClients().OrderBy(x => x.Name);
            //foreach (ClientObject x in clients)
            //{
            //    selectClients.Add(new SelectListItem { Text = x.Name, Value = x.Code });
            //}
            //ViewBag.SelectClients = selectClients;

            return View();
        }

        public ActionResult DownloadPayroll(string co, string c)
        {
            List<PayrollObject> model = BillingManager.Instance.GetPayroll(co, c);
            ClientObject client = ClientManager.Instance.GetClient(c);
            ViewBag.ClientCode = client != null ? client.Code : "";
            ViewBag.Client = client != null ? client.Name : "";
            ViewBag.CutOff = co;

            return View(model);
        }

        public ActionResult DownloadBilling(string co, string c)
        {
            List<BillingObject> model = BillingManager.Instance.GetBilling(co, c);
            ClientObject client = ClientManager.Instance.GetClient(c);
            ViewBag.ClientCode = client != null ? client.Code : "";
            ViewBag.Client = client != null ? client.Name : "";
            ViewBag.CutOff = co;

            return View(model);
        }

        public ActionResult DownloadInvoice(string co, string c)
        {
            InvoiceObject model = BillingManager.Instance.GetInvoice(co, c);
            ClientObject client = ClientManager.Instance.GetClient(c);
            ViewBag.ClientCode = client != null ? client.Code : "";
            ViewBag.Client = client != null ? client.Name : "";
            ViewBag.CutOff = co;

            return View(model);
        }


    }
}
