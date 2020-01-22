using SGS.Business.Objects;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class PayslipManager
    {
        private static PayslipManager instance;
        public static PayslipManager Instance
        {
            get
            {
                if (instance == null) instance = new PayslipManager();
                return instance;
            }
        }

        public List<PayslipsObject> GetPayslip(string cutoff, string client)
        {
            List<PayslipsObject> result = new List<PayslipsObject>();

            List<PayrollObject> payroll = BillingManager.Instance.GetPayroll(cutoff, client);
            if(payroll != null)
            {
                result = (from p in payroll
                          select new PayslipsObject
                          {
                              Employee = p.Employee,
                              Position = p.Position,
                              NoOfDays = p.NumOfDays,
                              BasicPay = p.BasicPay,
                              ECOLA = p.ColaPay,
                              NightDiff = p.NightDiffPay,
                              OTPay = p.OvertimePay + p.RDOTPay + p.ExcessRDOTPay + p.ExcessLegalHolidayPay + p.ExcessSpecialHolidayPay,
                              Allowances = p.Allowance,
                              HolidayPay = p.LegalHolidayPay + p.SpecialHolidayPay + p.LegalHolidayNotWorkedPay + p.SpecialHolidayNotWorkedPay,
                              Adjustments = p.Adjustment,
                              GrossPay = p.BasicPay + p.ColaPay + p.NightDiffPay +
                                         p.OvertimePay + p.RDOTPay + p.ExcessRDOTPay + p.ExcessLegalHolidayPay + p.ExcessSpecialHolidayPay +
                                         p.Allowance + p.LegalHolidayPay + p.SpecialHolidayPay + p.LegalHolidayNotWorkedPay + p.SpecialHolidayNotWorkedPay
                                         + (p.Adjustment > 0 ? p.Adjustment : 0),
                              Late = p.LateDeduction,
                              SSS = p.SSSEE + p.SSSLoan,
                              PhilHealth = p.PhilHealthEE,
                              Pagibig = p.PagibigEE + p.PagibigLoan,
                              TotalDeduction = p.LateDeduction + p.SSSEE + p.SSSLoan + p.PhilHealthEE + p.PagibigEE + p.PagibigLoan + (p.Adjustment < 0 ? p.Adjustment : 0),
                              NetPay = (p.BasicPay + p.ColaPay + p.NightDiffPay +
                                        p.OvertimePay + p.RDOTPay + p.ExcessRDOTPay + p.ExcessLegalHolidayPay + p.ExcessSpecialHolidayPay +
                                        p.Allowance + p.LegalHolidayPay + p.SpecialHolidayPay + p.LegalHolidayNotWorkedPay + p.SpecialHolidayNotWorkedPay)
                                        - (p.LateDeduction + p.SSSEE + p.SSSLoan + p.PhilHealthEE + p.PagibigEE + p.PagibigLoan)
                                        + p.Adjustment

                          }).ToList();
            }

            return result.Count() > 0 ? result : null;
        }

        public List<PayslipObject> GetAll(string fileName)
        {
            List<PayslipObject> result = new List<PayslipObject>();
            string con =
                  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
                  @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        PayslipObject item = new PayslipObject();

                        item.EmpName = GetValueString(dr["EmpName"]);
                        item.EmpPosition = GetValueString(dr["EmpPosition"]);

                        item.BasicRate = GetValueNum(dr["BasicRate"]);
                        item.BasicDays = GetValueNum(dr["BasicDays"]);
                        item.BasicPay = GetValueNum(dr["BasicPay"]);

                        item.ColaRate = GetValueNum(dr["ColaRate"]);
                        item.ColaDays = GetValueNum(dr["ColaDays"]);
                        item.ColaPay = GetValueNum(dr["ColaPay"]);

                        item.OTRate = GetValueNum(dr["OTRate"]);
                        item.OTHours = GetValueNum(dr["OTHours"]);
                        item.OTPay = GetValueNum(dr["OTPay"]);

                        item.RDOTRate = GetValueNum(dr["RDOTRate"]);
                        item.RDOTHours = GetValueNum(dr["RDOTHours"]);
                        item.RDOTPay = GetValueNum(dr["RDOTPay"]);

                        item.RDOTExcessRate = GetValueNum(dr["RDOTExcessRate"]);
                        item.RDOTExcessHours = GetValueNum(dr["RDOTExcessHours"]);
                        item.RDOTExcessPay = GetValueNum(dr["RDOTExcessPay"]);

                        item.NDRate = GetValueNum(dr["NDRate"]);
                        item.NDHours = GetValueNum(dr["NDHours"]);
                        item.NDPay = GetValueNum(dr["NDPay"]);

                        item.LHRate = GetValueNum(dr["LHRate"]);
                        item.LHHours = GetValueNum(dr["LHHours"]);
                        item.LHPay = GetValueNum(dr["LHPay"]);

                        item.LHExcessRate = GetValueNum(dr["LHExcessRate"]);
                        item.LHExcessHours = GetValueNum(dr["LHExcessHours"]);
                        item.LHExcessPay = GetValueNum(dr["LHExcessPay"]);

                        item.SHRate = GetValueNum(dr["SHRate"]);
                        item.SHHours = GetValueNum(dr["SHHours"]);
                        item.SHPay = GetValueNum(dr["SHPay"]);

                        item.SHExcessRate = GetValueNum(dr["SHExcessRate"]);
                        item.SHExcessHours = GetValueNum(dr["SHExcessHours"]);
                        item.SHExcessPay = GetValueNum(dr["SHExcessPay"]);

                        item.LateRate = GetValueNum(dr["LateRate"]);
                        item.LatePerMin = GetValueNum(dr["LatePerMin"]);
                        item.LateDeductions = GetValueNum(dr["LateDeductions"]);

                        item.SSS_EE = GetValueNum(dr["SSS_EE"]);
                        item.PAGIBIG_EE = GetValueNum(dr["PAGIBIG_EE"]);
                        item.PHILHEALTH_EE = GetValueNum(dr["PHILHEALTH_EE"]);

                        item.SSS_Loan = GetValueNum(dr["SSS_Loan"]);
                        item.PAGIBIG_Loan = GetValueNum(dr["PAGIBIG_Loan"]);
                        item.Adjustment = GetValueNum(dr["Adjustment"]);
                        item.Allowance = GetValueNum(dr["Allowance"]);
                        item.CashAdvance = GetValueNum(dr["CashAdvance"]);

                        result.Add(item);
                    }
                }
            }

            return result;
        }

        //public List<PayslipObject> GetAll(string fileName)
        //{
        //    List<PayslipObject> result = new List<PayslipObject>();            
        //    string con =
        //          @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
        //          @"Extended Properties='Excel 8.0;HDR=Yes;'";
        //    using (OleDbConnection connection = new OleDbConnection(con))
        //    {
        //        connection.Open();
        //        OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
        //        using (OleDbDataReader dr = command.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                PayslipObject item = new PayslipObject();
        //                item.Period = "";
        //                item.Employee = GetValueString(dr["Employee Name"]); 
        //                item.Position = GetValueString(dr["Position"]);
        //                item.NoOfDays = GetValueNum(dr[4]);
        //                item.BasicPay = GetValueNum(dr["Basic Pay - Amount"]);
        //                item.ECOLA = GetValueNum(dr["ECOLA - Amount"]);

        //                item.NightDiff = GetValueNum(dr["ND OT - Amount"]) + GetValueNum(dr["Reg ND (hrs) - Amount"]) + GetValueNum(dr["SHND OT - Amount"]) + 
        //                    GetValueNum(dr["SHND - Amount"]) + GetValueNum(dr["RDND OT - Amount"]) + 
        //                    GetValueNum(dr["RDND - Amount"]) + GetValueNum(dr["LHND OT - Amount"]) + 
        //                    GetValueNum(dr["LHND - Amount"]);

        //                item.OTPay = GetValueNum(dr["Regular OT - Amount"]) + GetValueNum(dr["RESTDAY OT - Amount"]) +
        //                    GetValueNum(dr[" SH OT - Amount"]) + GetValueNum(dr["LH OT - Amount"]) + 
        //                    GetValueNum(dr["SH RD OT - Amount"]) + GetValueNum(dr["LH RD OT - Amount"]);

        //                item.Allowances = GetValueNum(dr["Allowances"]);
        //                item.Adjustments = GetValueNum(dr["Adjustments"]);

        //                item.HolidayPay = GetValueNum(dr["LH - Amount"]) + GetValueNum(dr["SH - Amount"]) +
        //                    GetValueNum(dr["RD SP - Amount"]) + GetValueNum(dr["RD LH - Amount"]);

        //                item.GrossPay = item.BasicPay + item.ECOLA + item.NightDiff + item.OTPay + item.Allowances + item.Adjustments + item.HolidayPay;

        //                item.Late = GetValueNum(dr["Late - Amount"]);
        //                item.Undertime = GetValueNum(dr["Undertime - Amount"]) + GetValueNum(dr["Overbreak - Amount"]);
        //                item.SSS = GetValueNum(dr["SSS"]);
        //                item.PhilHealth = GetValueNum(dr["PHILHEALTH"]);
        //                item.Pagibig = GetValueNum(dr["HDMF"]);
        //                item.Uniform = GetValueNum(dr["UNIFORMS"]);
        //                item.CashAdvance = GetValueNum(dr["CASH ADVANCE"]);
        //                item.TotalDeduction = item.Late + item.Undertime + item.SSS + item.PhilHealth + item.Pagibig + item.Uniform + item.CashAdvance;

        //                item.NetPay = item.GrossPay - item.TotalDeduction;

        //                result.Add(item);
        //            }
        //        }
        //    }

        //    return result;
        //}


        private string GetValueString(object row)
        {
            return DBNull.Value.Equals(row) ? "" : Convert.ToString(row);
        }

        private decimal GetValueNum(object row)
        {
            return DBNull.Value.Equals(row) ? 0 : Convert.ToDecimal(row);
        }
    }
}
