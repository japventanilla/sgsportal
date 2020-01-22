using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Areas.Service.Models
{
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public string RequestNo { get; set; }
        public string RequestTypeCode { get; set; }
        public string RequestTypeDisplay { get; set; }
        public string RequestedBy { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? RequestedDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public string SubmitType { get; set; }
        public string Status { get; set; }
        
    }
}