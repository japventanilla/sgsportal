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
    
    public partial class RequestType
    {
        public RequestType()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int RequestTypeId { get; set; }
        public string Code { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }
        public string Approvers { get; set; }
    
        public virtual ICollection<Request> Requests { get; set; }
    }
}