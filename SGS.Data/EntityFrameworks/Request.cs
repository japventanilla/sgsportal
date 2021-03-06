//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGS.Data.EntityFrameworks
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public Request()
        {
            this.Approvals = new HashSet<Approval>();
        }
    
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestNo { get; set; }
        public Nullable<int> RequestedBy { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<int> ProcessedBy { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
        public string Status { get; set; }
        public Nullable<bool> HasAttachment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string RecState { get; set; }
        public string Reimb_Desc { get; set; }
        public Nullable<decimal> Reimb_Amt { get; set; }
    
        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual User User { get; set; }
    }
}
