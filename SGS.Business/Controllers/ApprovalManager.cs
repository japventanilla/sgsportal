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
    public class ApprovalManager
    {
        private static ApprovalManager instance;
        public static ApprovalManager Instance
        {
            get
            {
                if (instance == null) instance = new ApprovalManager();
                return instance;
            }
        }

        private ApprovalManager()
        {

        }

        public List<GetApprovals_Result> Select(int approvalId, int approverId, int transactionId)
        {
            List<GetApprovals_Result> result = new List<GetApprovals_Result>();
            using (SGSDBEntities db = new SGSDBEntities())
            {
                var approvals = db.GetApprovals(approvalId, approverId, transactionId);
                if (approvals != null)
                {
                    result.AddRange(approvals);
                }
            }
            return result;
        }

        public void AddRequestApprovals(Request request)
        {
            RequestType requestType = RequestManager.Instance.GetRequestType(request.RequestTypeId);
            string[] approvers = requestType.Approvers.Split('|');

            using (SGSDBEntities db = new SGSDBEntities())
            {
                foreach (string approver in approvers)
                {
                    Approval item = new Approval();
                    item.ApprovalType = "Request";
                    item.RequestId = request.RequestId;
                    item.ApprovalStatus = RequestStatusEnum.ForApproval.ToString();
                    item.ApproverId = Convert.ToInt32(approver);
                    item.RequestedDate = request.RequestedDate;
                    item.RequestedBy = request.RequestedBy.Value;
                    item.CreatedDate = DateHelper.DateTimeNow;
                    item.CreatedBy = "system";
                    db.Approvals.Add(item);
                }

                db.SaveChanges();
            }            

        }


    }
}
