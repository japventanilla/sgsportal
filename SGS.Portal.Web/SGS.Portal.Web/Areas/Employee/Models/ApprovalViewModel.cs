using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Areas.Employee.Models
{
    public class ApprovalViewModel
    {
        public int ApprovalId { get; set; }
        public int TransactionId { get; set; }
        public string ApprovalType { get; set; }
        public string ApproverName { get; set; }
        public int ApproverId { get; set; }
        public string SubmittedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string TransactionNo { get; set; }

        public DateTime? TransactionDate { get; set; }
        public string TransactionDateString
        {
            get
            {
                if (this.TransactionDate != null)
                {
                    return this.TransactionDate.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DateTime? ApprovedDate { get; set; }
        public string ApprovedDateString
        {
            get
            {
                if (this.ApprovedDate != null)
                {
                    return this.ApprovedDate.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [Required(ErrorMessage = "Comments field is required")]
        public string Comments { get; set; }
    }
}