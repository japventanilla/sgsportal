using SGS.Business.Controllers;
using SGS.Business.Objects;
using SGS.Portal.Web.Areas.Service.Models;
using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using obj = SGS.Data.EntityFrameworks;

namespace SGS.Portal.Web.Areas.Service.Controllers
{
    public class RequestController : Controller
    {
        //
        // GET: /Service/Request/

        [CustAuthFilter]
        public ActionResult Create()
        {
            return View();
        }

        [CustAuthFilter]
        public ActionResult ParkedItems()
        {
            IEnumerable<RequestViewModel> model = RequestList(0, RequestStatusEnum.Parked.ToString(), 0);
            return View(model);
        }

        [CustAuthFilter]
        public ActionResult ViewRequest()
        {
            IEnumerable<RequestViewModel> model = RequestList(0, "", 0);
            return View(model);
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
                                                     TransactionDate = x.ModifiedDate == null? x.CreatedDate : x.ModifiedDate.Value,
                                                 });

            return list;
        } 

        [CustAuthFilter]
        public ActionResult Reimbursement(string id)
        {
            RequestViewModel model = new RequestViewModel();
            int requestId = 0;

            if (int.TryParse(id, out requestId))
            {
                model = RequestList(requestId, "", 0).FirstOrDefault();
            }

            return View(model);
        }

        [HttpPost]
        [CustAuthFilter]
        public ActionResult Reimbursement(RequestViewModel model)
        {
            obj.Request request = new obj.Request();

            request.RequestId = model.RequestId;
            request.RequestedBy = UserHelper.CurrentUser.UserId;
            request.RequestTypeId = RequestManager.Instance.GetReimbursmentType.RequestTypeId;
            request.Reimb_Desc = model.Description;
            request.Reimb_Amt = model.Amount;

            if (model.RequestId > 0)
                request.ModifiedBy = UserHelper.CurrentUser.EmployeeId;
            else
                request.CreatedBy = UserHelper.CurrentUser.EmployeeId;

            if (model.SubmitType == "park")
            {
                request.Status = RequestStatusEnum.Parked.ToString();
                RequestManager.Instance.SubmitRequest(request);
                return RedirectToAction("ParkedItems", "Request");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    request.Status = RequestStatusEnum.ForApproval.ToString();   
                    int requestId = RequestManager.Instance.SubmitRequest(request);
                    model = RequestList(requestId, "", 0).FirstOrDefault();
                }

                return View(model);
            }
            
        }

    }
}
