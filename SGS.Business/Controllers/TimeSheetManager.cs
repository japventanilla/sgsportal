using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class TimeSheetManager
    {
        private static TimeSheetManager instance;
        public static TimeSheetManager Instance
        {
            get
            {
                if (instance == null) instance = new TimeSheetManager();
                return instance;
            }
        }

        public List<TimeSheet> Search(string client, string cutoff)
        {
            DateTime dt1 = DateTime.Parse(cutoff.Split('-')[0]);
            DateTime dt2 = DateTime.Parse(cutoff.Split('-')[1]);

            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.TimeSheets
                        where x.WorkDate >= dt1 && x.WorkDate <= dt2 && x.Client == client
                        select x).ToList();
            }
        }

        public List<string> SetCutOffFilters(string cutoffs, int NumOfCutoffs)
        {
            string[] cutoff = cutoffs.Split(',');
            List<string> result = new List<string>();

            DateTime date1 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + int.Parse(cutoff[0]));
            DateTime date2 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + int.Parse(cutoff[1]));
            DateTime dt = DateTime.UtcNow.AddHours(8).AddDays(-15);

            if (dt > date1 && dt < date2)
                dt = date2;
            else
                dt = date1;

            while (result.Count() < NumOfCutoffs)
            {
                if (dt.Day == int.Parse(cutoff[1]))
                {
                    string dt1 = dt.AddMonths(-1).ToString("MMM " + cutoff[1] + ", yyyy");
                    string lastdayInMonth = DateTime.DaysInMonth(dt.AddMonths(-1).Year, dt.AddMonths(-1).Month).ToString();
                    string dt2 = int.Parse(cutoff[0]) != 1 ? dt.ToString("MMM " + (int.Parse(cutoff[0]) - 1) + ", yyyy") : dt.AddMonths(-1).ToString("MMM " + lastdayInMonth + ", yyyy");
                    string filter = string.Format("{0} - {1}", dt1, dt2);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.AddMonths(-1).Year + "-" + dt.AddMonths(-1).Month + "-" + cutoff[0]);
                }
                else
                {
                    string dt1 = dt.ToString("MMM " + cutoff[0] + ", yyyy");
                    string dt2 = dt.ToString("MMM " + (int.Parse(cutoff[1]) - 1) + ", yyyy");
                    string filter = string.Format("{0} - {1}", dt1, dt2);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.Year + "-" + dt.Month + "-" + cutoff[1]);
                }
            }

            return result;
        }

        public void Save(string fileName, string UploadedBy)
        {
            string con =
                  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
                  @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [DTS$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        using (SGSDBEntities db = new SGSDBEntities())
                        {

                            string empId = GetValueString(dr["ID"]);
                            DateTime? workDate = GetValueDate(dr["Date"]);

                            if(workDate != null && empId != "")
                            {
                                TimeSheet item = db.TimeSheets.SingleOrDefault(x => x.EmpId == empId && x.WorkDate == workDate);

                                if (item == null)
                                    item = new TimeSheet();

                                item.EmpId = empId;
                                item.Name = GetValueString(dr["Name"]);
                                item.Position = GetValueString(dr["Position"]);
                                item.Client = GetValueString(dr["Client"]);
                                item.BasicRate = GetValueNum(dr["BasicRate"]);
                                item.WorkDate = workDate;
                                item.TimeIn = GetValueTime(dr["TimeIn"]);
                                item.TimeOut = GetValueTime(dr["TimeOut"]);
                                item.RegularHours = GetValueNum(dr["RegularHours"]);
                                item.Late = GetValueNum(dr["Late"]);
                                item.LegalHolidayNotWorked = GetValueNum(dr["LegalHolidayNotWorked"]);
                                item.LegalHolidayOT = GetValueNum(dr["LegalHolidayOT"]);
                                item.ExcessOfLegalHolidayOT = GetValueNum(dr["ExcessOfLegalHolidayOT"]);
                                item.SpecialHolidayNotWorked = GetValueNum(dr["SpecialHolidayNotWorked"]);
                                item.SpecialHolidayOT = GetValueNum(dr["SpecialHolidayOT"]);
                                item.ExcessOfSpecialHolidayOT = GetValueNum(dr["ExcessOfSpecialHolidayOT"]);
                                item.AuthorizedOT = GetValueNum(dr["AuthorizedOT"]);
                                item.RestDayOT = GetValueNum(dr["RestDayOT"]);
                                item.ExcessOfRestDayOT = GetValueNum(dr["ExcessOfRestDayOT"]);
                                item.NightDiff = GetValueNum(dr["NightDiff"]);
                                item.SSSLoan = GetValueNum(dr["SSSLoan"]);
                                item.PagibigLoan = GetValueNum(dr["PagibigLoan"]);
                                item.Allowance = GetValueNum(dr["Allowance"]);
                                item.Adjustment = GetValueNum(dr["Adjustment"]);

                                if(item.LegalHolidayOT > 0 || item.SpecialHolidayOT > 0 || item.RestDayOT > 0)
                                {
                                    item.RegularHours = 0;
                                }

                                if (item.Id == 0)
                                {
                                    item.CreatedBy = UploadedBy;
                                    item.CreatedDate = DateTime.UtcNow;
                                    db.TimeSheets.Add(item);
                                }
                                else
                                {
                                    item.ModifiedBy = UploadedBy;
                                    item.ModifiedDate = DateTime.UtcNow;
                                }

                                db.SaveChanges();
                            }   

                        }
                    }
                }
            }
        }

        private string GetValueString(object row)
        {
            try
            {
                return DBNull.Value.Equals(row) ? "" : Convert.ToString(row);
            }
            catch
            {
                return "";
            }
        }

        private double GetValueNum(object row)
        {
            try
            {
                return DBNull.Value.Equals(row) ? 0 : Convert.ToDouble(row);
            }
            catch
            {
                return 0;
            }
        }

        private DateTime? GetValueDate(object row)
        {
            try
            {
                if (DBNull.Value.Equals(row))
                    return null;
                else
                    return Convert.ToDateTime(row);
            }
            catch
            {
                return null;
            }
        }

        private string GetValueTime(object row)
        {
            try
            {
                if (DBNull.Value.Equals(row))
                    return null;
                else
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(row);
                        return dt.ToString("hh:mm tt");
                    }
                    catch
                    {
                        return Convert.ToString(row);
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
