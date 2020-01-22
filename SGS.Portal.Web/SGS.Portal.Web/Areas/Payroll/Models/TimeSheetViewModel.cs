using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Areas.Payroll.Models
{
    public class TimeSheetViewModel
    {
        public bool IsUploaded { get; set; }
        public string EmpId { get; set; }
        public string Client { get; set; }
        public string EmpName { get; set; }
        public string WorkDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public double RegularHours { get; set; }
        public double LegalHolidayOT { get; set; }
        public double LegalHolidayOTExcess { get; set; }
        public double SpecialHolidayOT { get; set; }
        public double SpecialHolidayOTExcess { get; set; }
        public double AuthorizedOT { get; set; }
        public double RestDayOT { get; set; }
        public double RestDayOTExcess { get; set; }
        public double NightDiff { get; set; }
        public double Late { get; set; }
    }
}