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
    public class PayslipController : Controller
    {
        //
        // GET: /Payroll/Payslip/

        [CustAuthFilter]
        [HttpGet]
        public ActionResult Index()
        {
            PayslipViewModel model = new PayslipViewModel();
            return View(model);
        }

        [CustAuthFilter]
        [HttpPost]
        public ActionResult Index(PayslipViewModel model, HttpPostedFileBase file)
        {

            try
            {
                if (file != null)
                {
                    string filename = "payslip_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string name = System.IO.Path.GetFileName(filename);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString()), name);

                    file.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }

                    ViewBag.Success = true;
                    model.Filename = filename;
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            return View(model);
        }

        [CustAuthFilter]
        [HttpGet]
        public ActionResult Preview(string p, string f)
        {
            List<PayslipObject> model = PayslipManager.Instance.GetAll(HttpContext.Server.MapPath("/Uploads/" + f));
            ViewBag.Period = Uri.UnescapeDataString(p);
            return View(model);
        }

    }
}
