using SGS.Business.Objects;
using SGS.Common.Controllers;
using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class RequestManager
    {
        private static RequestManager instance;
        public static RequestManager Instance
        {
            get
            {
                if (instance == null) instance = new RequestManager();
                return instance;
            }
        }

        private RequestManager()
        {

        }

        public List<GetRequests_Result> GetAll(int requestId, string status, int typeId, int requestedBy)
        {
            List<GetRequests_Result> result = new List<GetRequests_Result>();
            using (SGSDBEntities db = new SGSDBEntities())
            {
                var requests = db.GetRequests(requestId, status, typeId, requestedBy);
                if (requests != null)
                {
                    result.AddRange(requests);
                }
            }
            return result;
        }

        public IEnumerable<Request> GetParkedItems(int userId)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Requests
                        where x.RecState == "A" && x.RequestedBy == userId && x.Status == RequestStatusEnum.Parked.ToString()
                        select x).ToList();
            }
        }

        public IEnumerable<Request> GetRequest(int requestId, int userId)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Requests
                        where x.RecState == "A" && x.RequestedBy == userId && x.RequestId == requestId
                        select x).ToList();
            }
        }

        public int SubmitRequest(Request request)
        {
            int requestId = request.RequestId;

            using (SGSDBEntities db = new SGSDBEntities())
            {
                Request item = db.Requests.SingleOrDefault(x => x.RequestId == requestId);

                if (item == null)
                    item = new Request();

                item.RequestTypeId = request.RequestTypeId;
                item.Status = request.Status;
                item.Reimb_Desc = request.Reimb_Desc;
                item.Reimb_Amt = request.Reimb_Amt;
                item.RequestedBy = request.RequestedBy;

                if (request.Status == RequestStatusEnum.ForApproval.ToString())
                {                    
                    item.RequestedDate = DateHelper.DateTimeNow;
                }

                if (item.RequestId == 0)
                {
                    item.RequestNo = "TMP";
                    item.CreatedDate = DateHelper.DateTimeNow;
                    item.CreatedBy = request.CreatedBy;
                    item.RecState = "A";
                    db.Requests.Add(item);
                }
                else
                {
                    item.ModifiedBy = request.ModifiedBy;
                    item.ModifiedDate = DateHelper.DateTimeNow;
                }

                db.SaveChanges();
                requestId = item.RequestId;
                item.RequestNo = String.Format("R-{0:D7}", requestId);
                db.SaveChanges();

                request = item;
                
            }

            if (request.Status == RequestStatusEnum.ForApproval.ToString())
            {
                ApprovalManager.Instance.AddRequestApprovals(request);
            }

            return requestId;
        }

        public RequestType GetReimbursmentType
        {
            get { return GetRequestType("Reimbursement"); }
        }
        public RequestType GetPaymentRequestType
        {
            get { return GetRequestType("PaymentRequest"); }
        }

        public RequestType GetRequestType(string requestType)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.RequestTypes
                        where x.Code == requestType
                        select x).FirstOrDefault();
            }
        }

        public RequestType GetRequestType(int typeId)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.RequestTypes
                        where x.RequestTypeId == typeId
                        select x).FirstOrDefault();
            }
        }

        
    }
}
