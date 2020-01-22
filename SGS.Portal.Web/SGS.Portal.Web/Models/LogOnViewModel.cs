using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Models
{
    public class LogOnViewModel
    {
        [Required(ErrorMessage = "Employee ID field is required")]
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [StringLength(20)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}