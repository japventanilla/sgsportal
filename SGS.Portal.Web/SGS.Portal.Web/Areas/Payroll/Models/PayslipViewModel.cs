using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Areas.Payroll.Models
{
    public class PayslipViewModel
    {
        public string Period { get; set; }
        public string Filename { get; set; }
    }
}