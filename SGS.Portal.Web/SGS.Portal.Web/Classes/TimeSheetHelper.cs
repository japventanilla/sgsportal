using SGS.Business.Controllers;
using SGS.Data.EntityFrameworks;
using SGS.Portal.Web.Areas.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Classes
{
    public static class TimeSheetHelper
    {
        public static List<TimeSheetViewModel> Search(string client, string cutoff)
        {
            List<TimeSheetViewModel> timesheets = null;

            timesheets = (from x in TimeSheetManager.Instance.Search(client, cutoff)
                         select new TimeSheetViewModel
                         {
                            EmpId = x.EmpId,
                            Client = x.Client,
                            EmpName = x.Name,
                            WorkDate = x.WorkDate.Value.ToString("M/d/yyyy"),
                            TimeIn = x.TimeIn,
                            TimeOut = x.TimeOut,
                            RegularHours = x.RegularHours.Value,
                            LegalHolidayOT = x.LegalHolidayOT.Value,
                            LegalHolidayOTExcess = x.ExcessOfLegalHolidayOT.Value,
                            SpecialHolidayOT = x.SpecialHolidayOT.Value,
                            SpecialHolidayOTExcess = x.ExcessOfSpecialHolidayOT.Value,
                            AuthorizedOT = x.AuthorizedOT.Value,
                            RestDayOT = x.RestDayOT.Value,
                            RestDayOTExcess = x.ExcessOfRestDayOT.Value,
                            NightDiff = x.NightDiff.Value,
                            Late = x.Late.Value
                        }).ToList();

            return timesheets;
        }
    }
}