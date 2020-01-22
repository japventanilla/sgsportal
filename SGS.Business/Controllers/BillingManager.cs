using SGS.Business.Objects;
using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class BillingManager
    {
        private static BillingManager instance;
        public static BillingManager Instance
        {
            get
            {
                if (instance == null) instance = new BillingManager();
                return instance;
            }
        }

        public List<PayrollObject> GetPayroll(string cutoff, string client)
        {
            List<PayrollObject> result = new List<PayrollObject>();

            DateTime dt1 = DateTime.Parse(cutoff.Split('-')[0]);
            DateTime dt2 = DateTime.Parse(cutoff.Split('-')[1]);
            
            _colaRate = SettingManager.ColaRate;
            _reliever = SettingManager.RelieverCheck;
            _pagibig = SettingManager.Pagibig;
            _philHealth = SettingManager.PhilHealth;
            _workingDays = SettingManager.WorkingDays(DateTime.Now.Year);

            using (SGSDBEntities db = new SGSDBEntities())
            {
                Client currClient = db.Clients.Where(x => x.ClientCode == client).FirstOrDefault();

                List<TimeSheet> ts = (from x in db.TimeSheets
                                      where x.Client == client && (x.WorkDate >= dt1 && x.WorkDate <= dt2)
                                      select x).ToList();

                List<TimeSheet> ts_prev = null;

                bool hasBenefitsDeductions = dt1.Day == int.Parse(currClient.GovtRemitDeductCutOff);

                if (hasBenefitsDeductions)
                {
                    string[] clientCutoffs = currClient.CutOffs.Split(',');
                    string prev_day1 = currClient.GovtRemitDeductCutOff != clientCutoffs[0] ? clientCutoffs[0] : clientCutoffs[1];

                    DateTime prev_dt1 = dt1.Day < 16 ? new DateTime(dt1.AddMonths(-1).Year, dt1.AddMonths(-1).Month, int.Parse(prev_day1)) 
                                        : new DateTime(dt1.Year, dt1.Month, int.Parse(prev_day1));
                    DateTime prev_dt2 = dt1.AddDays(-1);

                    ts_prev = (from x in db.TimeSheets
                                  where x.Client == client && (x.WorkDate >= prev_dt1 && x.WorkDate <= prev_dt2)
                                  select x).ToList();
                }

                List<string> empIds = ts.Select(x => x.EmpId).Distinct().ToList();

                foreach (string empId in empIds)
                {
                    List<TimeSheet> empTS = ts.Where(x => x.EmpId == empId).ToList();
                    if(empTS.Count() > 0)
                    {
                        bool isReliever = empTS.First().Position.ToLower() == _reliever;

                        List<TimeSheet> empTS_prev = null;
                        double emp_prev_total = 0;

                        if (hasBenefitsDeductions && !isReliever)
                        {
                            empTS_prev = ts_prev.Where(x => x.EmpId == empId).ToList();
                            if(empTS_prev.Count > 0)
                            {
                                PayrollObject t = ComputePayroll(empTS_prev);

                                if (currClient.GovtRemitType == "BasicPayOnly")
                                    emp_prev_total = t.BasicPay;
                                else
                                    emp_prev_total = t.Total;
                            }
                            else
                            {
                                emp_prev_total = 0;
                            }
                            
                        }

                        if (empTS != null)
                        {
                            PayrollObject t = ComputePayroll(empTS);
                            t.HasBenefitsDeductions = hasBenefitsDeductions;

                            if (hasBenefitsDeductions && !isReliever)
                            {
                                double total = 0;

                                if (currClient.GovtRemitType == "BasicPayOnly")
                                    total = t.BasicPay + emp_prev_total;
                                else
                                    total = t.Total + emp_prev_total;

                                SSSContribution sss = SSSCompute(total);
                                t.SSSER = sss.ER;
                                t.SSSEE = sss.EE;
                                t.SSSTotal = sss.ER + sss.EE;

                                t.PagibigER = _pagibig;
                                t.PagibigEE = _pagibig;
                                t.PagibigTotal = t.PagibigER + t.PagibigEE;

                                double phContri = PhilHealthContribution(total);
                                t.PhilHealthER = phContri;
                                t.PhilHealthEE = phContri;
                                t.PhilHealthTotal = phContri * 2;

                                t.TotalDeduction = t.SSSEE + t.PagibigEE + t.PhilHealthEE;
                            }
                            else
                            {
                                t.SSSLoan = empTS.First().SSSLoan.Value;
                                t.PagibigLoan = empTS.First().PagibigLoan.Value;

                                t.TotalDeduction = t.SSSLoan + t.PagibigLoan;
                            }

                            if (t.Adjustment < 0)
                                t.TotalDeduction += t.Adjustment;
                            else
                                t.Total += t.Adjustment;

                            t.Total += t.Allowance;

                            t.NetPay = t.Total - t.TotalDeduction;

                            result.Add(t);
                        }
                    }
                }
            }

            return result.Count() > 0? result : null;
        }

        public List<BillingObject> GetBilling(string cutoff, string client)
        {
            List<BillingObject> result = new List<BillingObject>();

            DateTime dt1 = DateTime.Parse(cutoff.Split('-')[0]);
            DateTime dt2 = DateTime.Parse(cutoff.Split('-')[1]);
            _colaRate = SettingManager.ColaRate;
            _reliever = SettingManager.RelieverCheck;
            _pagibig = SettingManager.Pagibig;
            _philHealth = SettingManager.PhilHealth;
            _workingDays = SettingManager.WorkingDays(DateTime.Now.Year);

            using (SGSDBEntities db = new SGSDBEntities())
            {
                Client currClient = db.Clients.Where(x => x.ClientCode == client).FirstOrDefault();
                _separationPayRate = currClient.SeparationPay == null? 0 : currClient.SeparationPay.Value;

                double agencyFee = currClient.AgencyFee / 100;

                List<TimeSheet> ts = (from x in db.TimeSheets
                                      where x.Client == client && (x.WorkDate >= dt1 && x.WorkDate <= dt2)
                                      select x).ToList();

                List<TimeSheet> ts_prev = null;

                bool hasBenefitsDeductions = dt1.Day == int.Parse(currClient.GovtRemitDeductCutOff);

                if (hasBenefitsDeductions)
                {

                    string[] clientCutoffs = currClient.CutOffs.Split(',');
                    string prev_day1 = currClient.GovtRemitDeductCutOff != clientCutoffs[0] ? clientCutoffs[0] : clientCutoffs[1];

                    DateTime prev_dt1 = dt1.Day < 16 ? new DateTime(dt1.AddMonths(-1).Year, dt1.AddMonths(-1).Month, int.Parse(prev_day1))
                                        : new DateTime(dt1.Year, dt1.Month, int.Parse(prev_day1));
                    DateTime prev_dt2 = dt1.AddDays(-1);

                    ts_prev = (from x in db.TimeSheets
                               where x.Client == client && (x.WorkDate >= prev_dt1 && x.WorkDate <= prev_dt2)
                               select x).ToList();
                }

                List<string> empIds = ts.Where(x => x.Client == client).Select(x => x.EmpId).Distinct().ToList();

                foreach (string empId in empIds)
                {
                    List<TimeSheet> empTS = ts.Where(x => x.EmpId == empId).ToList();
                    if (empTS.Count() > 0)
                    {
                        bool isReliever = empTS.First().Position.ToLower() == _reliever;

                        List<TimeSheet> empTS_prev = null;
                        double emp_prev_total = 0;

                        if (hasBenefitsDeductions && !isReliever)
                        {
                            empTS_prev = ts_prev.Where(x => x.EmpId == empId).ToList();                           

                            if(empTS_prev.Count > 0)
                            {
                                BillingObject t = ComputeBilling(empTS_prev);

                                if (currClient.GovtRemitType == "BasicPayOnly")
                                    emp_prev_total = t.BasicPay;
                                else
                                    emp_prev_total = t.Total - (t.IncentiveLeavePay + t.ThirteenthMonthPay + t.SeparationPay);
                            }
                            else
                            {
                                emp_prev_total = 0;
                            }
                            
                        }

                        if (empTS != null)
                        {
                            BillingObject t = ComputeBilling(empTS);
                            t.HasSeparationPay = currClient.SeparationPay.Value > 0;
                            t.HasBenefitsDeductions = hasBenefitsDeductions;

                            if (hasBenefitsDeductions && !isReliever)
                            {
                                double total = 0;

                                if (currClient.GovtRemitType == "BasicPayOnly")
                                    total = t.BasicPay + emp_prev_total;
                                else
                                    total = (t.Total + emp_prev_total) - (t.IncentiveLeavePay + t.ThirteenthMonthPay + t.SeparationPay);

                                SSSContribution sss = SSSCompute(total);
                                t.SSS = sss.ER;

                                t.Pagibig = _pagibig;

                                double phContri = PhilHealthContribution(total);
                                t.PhilHealth = phContri;

                                t.TotalGovRemitance = t.SSS + t.Pagibig + t.PhilHealth;
                            }

                            t.TotalReimbursableCost = t.Total + t.TotalGovRemitance;
                            t.AgencyFee = t.TotalReimbursableCost * agencyFee;
                            t.TotalWithFee = t.TotalReimbursableCost + t.AgencyFee;
                            t.VAT = t.TotalWithFee * 0.12;
                            t.TotalAmount = t.TotalWithFee + t.VAT + t.Allowance + t.Adjustment;

                            result.Add(t);
                        }
                    }
                }
            }

            return result.Count() > 0 ? result : null;
        }

        public InvoiceObject GetInvoice(string cutoff, string client)
        {
            InvoiceObject result = null;

            List<BillingObject> billing = GetBilling(cutoff, client);
            if(billing != null)
            {
                result = new InvoiceObject();
                result.Total = billing.Sum(x => x.TotalAmount);
                result.VatableSale = billing.Sum(x => x.TotalWithFee);
                result.VatAmount = billing.Sum(x => x.VAT);
            }

            return result;
        }

        public double _colaRate { get; set; }
        public string _reliever { get; set; }
        public double _pagibig { get; set; }
        public double _philHealth { get; set; }
        public double _separationPayRate { get; set; }
        public double _workingDays { get; set; }

        private PayrollObject ComputePayroll(List<TimeSheet> empTS)
        {
            PayrollObject t = new PayrollObject();

            t.Employee = empTS.First().Name;
            t.Position = empTS.First().Position;

            double basicRate = empTS.First().BasicRate.Value;
            if(basicRate < 5000)
            {
                t.BasicRate = basicRate;
                t.NumOfDays = empTS.Sum(x => x.RegularHours).Value / 8;
                t.BasicPay = t.BasicRate * t.NumOfDays;
            }
            else
            {
                t.BasicRate = (basicRate * 12) / _workingDays;
                t.NumOfDays = 0;
                t.BasicPay = basicRate;
            }

            t.ColaRate = _colaRate;
            t.ColaDays = (empTS.Sum(x => x.RegularHours).Value / 8) + (empTS.Sum(x => x.LegalHolidayOT).Value / 8) + (empTS.Sum(x => x.SpecialHolidayOT).Value / 8) + (empTS.Sum(x => x.RestDayOT).Value / 8);
            t.ColaPay = t.ColaRate * t.ColaDays;

            t.OvertimeRate = (basicRate / 8) * 1.25;
            t.OvertimeHours = empTS.Sum(x => x.AuthorizedOT).Value;
            t.OvertimePay = t.OvertimeRate * t.OvertimeHours;

            t.RDOTRate = (basicRate / 8) * 1.3;
            t.RDOTHours = empTS.Sum(x => x.RestDayOT).Value;
            t.RDOTPay = t.RDOTRate * t.RDOTHours;

            t.ExcessRDOTRate = (basicRate / 8) * 1.69;
            t.ExcessRDOTHours = empTS.Sum(x => x.ExcessOfRestDayOT).Value;
            t.ExcessRDOTPay = t.ExcessRDOTRate * t.ExcessRDOTHours;

            t.NightDiffRate = (basicRate / 8) * 0.1;
            t.NightDiffHours = empTS.Sum(x => x.NightDiff).Value;
            t.NightDiffPay = t.NightDiffRate * t.NightDiffHours;

            t.LegalHolidayRate = ((basicRate + t.ColaRate) / 8) * 2;
            t.LegalHolidayHours = empTS.Sum(x => x.LegalHolidayOT).Value;
            t.LegalHolidayPay = (t.LegalHolidayRate * t.LegalHolidayHours);

            t.LegalHolidayNotWorkedRate = (((basicRate + t.ColaRate) / 8) * 2) / 2;
            t.LegalHolidayNotWorkedHours = empTS.Sum(x => x.LegalHolidayNotWorked).Value;
            t.LegalHolidayNotWorkedPay = t.LegalHolidayNotWorkedRate * t.LegalHolidayNotWorkedHours;

            t.ExcessLegalHolidayRate = ((basicRate + t.ColaRate) / 8) * 2.6;
            t.ExcessLegalHolidayHours = empTS.Sum(x => x.ExcessOfLegalHolidayOT).Value;
            t.ExcessLegalHolidayPay = t.ExcessLegalHolidayRate * t.ExcessLegalHolidayHours;

            t.SpecialHolidayRate = ((basicRate + t.ColaRate) / 8) * 1.3;
            t.SpecialHolidayHours = empTS.Sum(x => x.SpecialHolidayOT).Value;
            t.SpecialHolidayPay = (t.SpecialHolidayRate * t.SpecialHolidayHours);

            t.SpecialHolidayNotWorkedRate = (((basicRate + t.ColaRate) / 8) * 2) / 2;
            t.SpecialHolidayNotWorkedHours = empTS.Sum(x => x.SpecialHolidayNotWorked).Value;
            t.SpecialHolidayNotWorkedPay = t.SpecialHolidayNotWorkedRate * t.SpecialHolidayNotWorkedHours;

            t.ExcessSpecialHolidayRate = ((basicRate + t.ColaRate) / 8) * 1.69;
            t.ExcessSpecialHolidayHours = empTS.Sum(x => x.ExcessOfSpecialHolidayOT).Value;
            t.ExcessSpecialHolidayPay = t.ExcessSpecialHolidayRate * t.ExcessSpecialHolidayHours;

            t.Adjustment = empTS.First().Adjustment.Value;
            t.Allowance = empTS.First().Allowance.Value;

            t.LateRatePerMin = (basicRate / 8) / 60;
            t.NumOfLates = empTS.Sum(x => x.Late).Value;
            t.LateDeduction = t.LateRatePerMin * t.NumOfLates;

            //t.Total = t.BasicPay + t.ColaPay + t.OvertimePay + t.RDOTPay + t.ExcessRDOTPay + t.NightDiffPay + t.LegalHolidayPay
            //          + t.ExcessLegalHolidayPay + t.SpecialHolidayPay + t.ExcessSpecialHolidayPay + t.Allowance + t.Adjustment 
            //          - t.LateDeduction;

            t.Total = t.BasicPay + t.ColaPay + t.OvertimePay + t.RDOTPay + t.ExcessRDOTPay + t.NightDiffPay + t.LegalHolidayPay
                      + t.ExcessLegalHolidayPay + t.SpecialHolidayPay + t.ExcessSpecialHolidayPay
                      - t.LateDeduction;

            return t;
        }

        private BillingObject ComputeBilling(List<TimeSheet> empTS)
        {
            BillingObject t = new BillingObject();

            t.Employee = empTS.First().Name;
            t.Position = empTS.First().Position;

            double basicRate = empTS.First().BasicRate.Value;
            if (basicRate < 5000)
            {
                t.BasicRate = basicRate;
                t.NumOfDays = empTS.Sum(x => x.RegularHours).Value / 8;
                t.BasicPay = t.BasicRate * t.NumOfDays;

                t.ColaRate = _colaRate;
                t.ColaDays = (empTS.Sum(x => x.RegularHours).Value / 8) + (empTS.Sum(x => x.LegalHolidayOT).Value / 8) + (empTS.Sum(x => x.SpecialHolidayOT).Value / 8) + (empTS.Sum(x => x.RestDayOT).Value / 8);
                t.ColaPay = t.ColaRate * t.ColaDays;

                t.IncentiveLeaveRate = ((basicRate * 5) / 12) / 26;
                t.IncentiveLeaveDays = empTS.Count();
                t.IncentiveLeavePay = t.IncentiveLeaveRate * t.IncentiveLeaveDays;

                t.ThirteenthMonthPayRate = ((basicRate * 26) / 12) / 26;
                t.ThirteenthMonthPayDays = empTS.Count();
                t.ThirteenthMonthPay = t.ThirteenthMonthPayRate * t.ThirteenthMonthPayDays;

                t.SeparationPayRate = _separationPayRate;
                t.SeparationPayDays = empTS.Count();
                t.SeparationPay = t.SeparationPayRate * t.SeparationPayDays;
            }
            else
            {
                t.BasicRate = basicRate;
                t.NumOfDays = 0;
                t.BasicPay = basicRate;

                basicRate = (basicRate * 12) / _workingDays;

                t.IncentiveLeaveRate = ((basicRate * 5) / 12) / 26;
                t.IncentiveLeavePay = t.IncentiveLeaveRate;

                t.ThirteenthMonthPayRate = ((basicRate * 26) / 12) / 26;
                t.ThirteenthMonthPay = t.ThirteenthMonthPayRate ;

                t.SeparationPayRate = _separationPayRate;
                t.SeparationPay = t.SeparationPayRate;
            }

            t.OvertimeRate = (basicRate / 8) * 1.25;
            t.OvertimeHours = empTS.Sum(x => x.AuthorizedOT).Value;
            t.OvertimePay = t.OvertimeRate * t.OvertimeHours;

            t.RDOTRate = (basicRate / 8) * 1.3;
            t.RDOTHours = empTS.Sum(x => x.RestDayOT).Value;
            t.RDOTPay = t.RDOTRate * t.RDOTHours;

            t.ExcessRDOTRate = (basicRate / 8) * 1.69;
            t.ExcessRDOTHours = empTS.Sum(x => x.ExcessOfRestDayOT).Value;
            t.ExcessRDOTPay = t.ExcessRDOTRate * t.ExcessRDOTHours;

            t.NightDiffRate = (basicRate / 8) * 0.1;
            t.NightDiffHours = empTS.Sum(x => x.NightDiff).Value;
            t.NightDiffPay = t.NightDiffRate * t.NightDiffHours;

            t.LegalHolidayRate = ((basicRate + t.ColaRate) / 8) * 2;
            t.LegalHolidayHours = empTS.Sum(x => x.LegalHolidayOT).Value;
            t.LegalHolidayPay = t.LegalHolidayRate * t.LegalHolidayHours;

            t.LegalHolidayNotWorkedRate = (((basicRate + t.ColaRate) / 8) * 2) / 2;
            t.LegalHolidayNotWorkedHours = empTS.Sum(x => x.LegalHolidayNotWorked).Value;
            t.LegalHolidayNotWorkedPay = t.LegalHolidayNotWorkedRate * t.LegalHolidayNotWorkedHours;

            t.ExcessLegalHolidayRate = ((basicRate + t.ColaRate) / 8) * 2.6;
            t.ExcessLegalHolidayHours = empTS.Sum(x => x.ExcessOfLegalHolidayOT).Value;
            t.ExcessLegalHolidayPay = t.ExcessLegalHolidayRate * t.ExcessLegalHolidayHours;

            t.SpecialHolidayRate = ((basicRate + t.ColaRate) / 8) * 1.3;
            t.SpecialHolidayHours = empTS.Sum(x => x.SpecialHolidayOT).Value;
            t.SpecialHolidayPay = t.SpecialHolidayRate * t.SpecialHolidayHours;

            t.SpecialHolidayNotWorkedRate = (((basicRate + t.ColaRate) / 8) * 2) / 2;
            t.SpecialHolidayNotWorkedHours = empTS.Sum(x => x.SpecialHolidayNotWorked).Value;
            t.SpecialHolidayNotWorkedPay = t.SpecialHolidayNotWorkedRate * t.SpecialHolidayNotWorkedHours;

            t.ExcessSpecialHolidayRate = ((basicRate + t.ColaRate) / 8) * 1.69;
            t.ExcessSpecialHolidayHours = empTS.Sum(x => x.ExcessOfSpecialHolidayOT).Value;
            t.ExcessSpecialHolidayPay = t.ExcessSpecialHolidayRate * t.ExcessSpecialHolidayHours;

            t.Adjustment = empTS.First().Adjustment.Value;
            t.Allowance = empTS.First().Allowance.Value;

            t.LateRatePerMin = (basicRate / 8) / 60;
            t.NumOfLates = empTS.Sum(x => x.Late).Value;
            t.LateDeduction = t.LateRatePerMin * t.NumOfLates;

            //t.Total = (t.BasicPay + t.ColaPay + t.IncentiveLeavePay + t.ThirteenthMonthPay + t.SeparationPay
            //          + t.OvertimePay + t.RDOTPay + t.ExcessRDOTPay + t.NightDiffPay + t.LegalHolidayPay
            //          + t.ExcessLegalHolidayPay + t.SpecialHolidayPay + t.ExcessSpecialHolidayPay + t.Allowance + t.Adjustment)
            //          - t.LateDeduction;

            t.Total = (t.BasicPay + t.ColaPay + t.IncentiveLeavePay + t.ThirteenthMonthPay + t.SeparationPay
                      + t.OvertimePay + t.RDOTPay + t.ExcessRDOTPay + t.NightDiffPay + t.LegalHolidayPay
                      + t.ExcessLegalHolidayPay + t.SpecialHolidayPay + t.ExcessSpecialHolidayPay)
                      - t.LateDeduction;

            return t;
        }

        private class SSSContribution
        {
            public double ER { get; set; }
            public double EE { get; set; }
        }

        private SSSContribution SSSCompute(double total)
        {
            SSSContribution sss = new SSSContribution();
            double r = 2250;
            sss.ER = 160;
            sss.EE = 80;
            double ec = 10;
            double gap = 500;

            if (total < r)
            {
                sss.ER += ec;
                return sss;
            }
            else
            {
                while (total >= r && total < (total + gap))
                {
                    r += gap;
                    sss.ER += 40;
                    sss.EE += 20;
                    ec = sss.ER >= 1500 ? 30 : 10;

                    //if (r > 16250) break;
                    //if (r > 30000) break;
                }

                sss.ER += ec;
            }

            return sss;
        }

        static double PhilHealthContribution(double total)
        {
            if (total <= 10000)
            {
                return 137.5;
            }
            else if (total > 10000 && total < 40000)
            {
                return (total * (2.75 / 100)) / 2;
            }
            else
            {
                return 550;
            }
        }
    }
}
