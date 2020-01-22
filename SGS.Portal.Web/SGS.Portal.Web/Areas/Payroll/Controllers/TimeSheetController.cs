using SGS.Portal.Web.Areas.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGS.Portal.Web.Classes;
using System.IO;
using System.Configuration;
using SGS.Business.Controllers;
using SGS.Business.Objects;
using System.Web.Script.Serialization;

namespace SGS.Portal.Web.Areas.Payroll.Controllers
{
    public class TimeSheetController : Controller
    {
        //
        // GET: /Payroll/TimeSheet/

        public ActionResult GetTimeSheets(string c, string co)
        {
            var model = TimeSheetHelper.Search(c, co);
            return Json(model, JsonRequestBehavior.AllowGet);
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

            return View();
        }

        public ActionResult Entry()
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

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Entry(FormCollection form)
        {

            return View();
        }

        [CustAuthFilter]
        public ActionResult Upload()
        {
            TimeSheetViewModel model = new TimeSheetViewModel();
            return View(model);
        }

        [CustAuthFilter]
        [HttpPost]
        public ActionResult Upload(TimeSheetViewModel model, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    string filename = "dts_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string name = System.IO.Path.GetFileName(filename);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString()), name);

                    file.SaveAs(path);

                    TimeSheetManager.Instance.Save(path,UserHelper.CurrentUser.EmployeeId);

                    System.IO.File.Delete(path);
                    ViewBag.Success = true;
                    model.IsUploaded = true;
                    

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            return View(model);
        }

    }
}
