using SGS.Business.Controllers;
using SGS.Portal.Web.Areas.Employee.Models;
using SGS.Portal.Web.Areas.Service.Models;
using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGS.Portal.Web.Areas.Employee.Controllers
{
    public class PersonalController : Controller
    {
        //
        // GET: /Employee/Personal/

        [CustAuthFilter]
        public ActionResult MyProfile()
        {
            return View(EmployeeHelper.Get(UserHelper.CurrentUser.UserId));
        }

        [CustAuthFilter]
        [HttpPost]
        public ActionResult MyProfile(EmployeeViewModel model, HttpPostedFileBase file)
        {
            EmployeeViewModel updModel = model;
            ViewBag.IsSuccess = false;

            if (ModelState.IsValid)
            {
                SGS.Data.EntityFrameworks.User employee = UserManager.Instance.Get(model.UserId);
               
                employee.MaritalStatus = model.MaritalStatus;
                employee.PresentAddress = model.PresentAddress;
                employee.ProvincialAddress = model.ProvincialAddress;
                employee.LandlineNo = model.LandLine;
                employee.MobileNo = model.MobileNo;
                employee.Email = model.Email;
                employee.Ref_Name = model.ReferenceName;
                employee.Ref_Relationship = model.ReferenceRelationship;
                employee.Ref_Address = model.ReferenceAddress;
                employee.Ref_ContactNo = model.ReferenceContactNo;
                employee.TaxIdNo = model.TaxIdNo;
                employee.SSSNo = model.SSSNo;
                employee.HDMFNo = model.HDMFNo;
                employee.PhilHealthNo = model.PhilHealthNo;
                employee.ModifiedBy = UserHelper.CurrentUser.EmployeeId;


                if (file != null)
                {
                    string filename = "p_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string pic = System.IO.Path.GetFileName(filename);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString()), pic);

                    file.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }

                    employee.Picture = filename;
                }

                try
                {
                    updModel = EmployeeHelper.Get(UserManager.Instance.SaveUser(employee));
                    ViewBag.Success = true;
                }
                catch (Exception ex)
                {
                    ViewBag.SysError = ex.Message;
                }
            }
            else
            {
                ViewBag.Error = true;
            }

            return View(updModel);
        }

        [CustAuthFilter]
        public ActionResult MyApprovals()
        {
            IEnumerable<ApprovalViewModel> model = ApprovalList(0, UserHelper.CurrentUser.UserId, 0);
            return View(model);
        }

        [CustAuthFilter]
        public ActionResult Approval(string id)
        {
            ApprovalViewModel model = new ApprovalViewModel();
            int approvalId = 0;

            if (int.TryParse(id, out approvalId))
            {
                model = ApprovalList(approvalId, 0, 0).FirstOrDefault();
                ViewBag.Request = RequestList(model.TransactionId, "", 0).FirstOrDefault();
            }

            return View(model);
        }

        public static IEnumerable<ApprovalViewModel> ApprovalList(int approvalId, int approverId, int transactionId)
        {

            IEnumerable<ApprovalViewModel> list = (from x in ApprovalManager.Instance.Select(approvalId, approverId, transactionId)
                                                   select new ApprovalViewModel
                                                  {
                                                      ApprovalId = x.ApprovalId,
                                                      TransactionId = x.TransactionId.Value,
                                                      TransactionNo = x.No,
                                                      ApprovalType = x.Type,
                                                      ApproverId = x.ApproverId.Value,
                                                      ApproverName = x.ApproverName,
                                                      SubmittedBy = x.SubmittedBy,
                                                      TransactionDate = x.TransactionDate,
                                                      ApprovedDate = x.ApprovedDate,
                                                      ApprovalStatus = x.ApprovedStatus,
                                                      Comments = x.Comments
                                                  });

            return list;
        }

        private static IEnumerable<RequestViewModel> RequestList(int requestId, string status, int typeId)
        {

            IEnumerable<RequestViewModel> list = (from x in RequestManager.Instance.GetAll(requestId, status, typeId, UserHelper.CurrentUser.UserId)
                                                  select new RequestViewModel
                                                  {
                                                      RequestId = x.RequestId,
                                                      RequestNo = x.RequestNo,
                                                      RequestTypeCode = x.RequestTypeCode,
                                                      RequestTypeDisplay = x.RequestTypeDisplay,
                                                      RequestedBy = x.RequestedBy,
                                                      RequestedDate = x.RequestedDate,
                                                      Description = x.Reimb_Desc,
                                                      Amount = x.Reimb_Amt == null ? 0 : x.Reimb_Amt.Value,
                                                      Status = x.Status,
                                                      TransactionDate = x.ModifiedDate == null ? x.CreatedDate : x.ModifiedDate.Value,
                                                  });

            return list;
        } 

    }
}
