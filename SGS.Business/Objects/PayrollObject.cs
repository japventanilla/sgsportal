using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Objects
{
    public class PayrollObject
    {
        public string Employee { get; set; }
        public string Position { get; set; }

        public double BasicPay { get; set; }
        public double BasicRate { get; set; }
        public double NumOfDays { get; set; }        

        public double ColaRate { get; set; }
        public double ColaDays { get; set; }
        public double ColaPay { get; set; }

        public double OvertimeRate { get; set; }
        public double OvertimeHours { get; set; }
        public double OvertimePay { get; set; }

        public double RDOTRate { get; set; }
        public double RDOTHours { get; set; }
        public double RDOTPay { get; set; }

        public double ExcessRDOTRate { get; set; }
        public double ExcessRDOTHours { get; set; }
        public double ExcessRDOTPay { get; set; }

        public double NightDiffRate { get; set; }
        public double NightDiffHours { get; set; }
        public double NightDiffPay { get; set; }

        public double LegalHolidayRate { get; set; }
        public double LegalHolidayHours { get; set; }
        public double LegalHolidayPay { get; set; }

        public double LegalHolidayNotWorkedRate { get; set; }
        public double LegalHolidayNotWorkedHours { get; set; }
        public double LegalHolidayNotWorkedPay { get; set; }

        public double ExcessLegalHolidayRate { get; set; }
        public double ExcessLegalHolidayHours { get; set; }
        public double ExcessLegalHolidayPay { get; set; }

        public double SpecialHolidayRate { get; set; }
        public double SpecialHolidayHours { get; set; }
        public double SpecialHolidayPay { get; set; }

        public double SpecialHolidayNotWorkedRate { get; set; }
        public double SpecialHolidayNotWorkedHours { get; set; }
        public double SpecialHolidayNotWorkedPay { get; set; }

        public double ExcessSpecialHolidayRate { get; set; }
        public double ExcessSpecialHolidayHours { get; set; }
        public double ExcessSpecialHolidayPay { get; set; }

        public double Adjustment { get; set; }
        public double Allowance { get; set; }

        public double LateRatePerMin { get; set; }
        public double NumOfLates { get; set; }
        public double LateDeduction { get; set; }

        public double Total { get; set; }

        public double SSSER { get; set; }
        public double SSSEE { get; set; }
        public double SSSTotal { get; set; }

        public double PagibigER { get; set; } 
        public double PagibigEE { get; set; }
        public double PagibigTotal { get; set; }

        public double PhilHealthER { get; set; }
        public double PhilHealthEE { get; set; }
        public double PhilHealthTotal { get; set; }

        public double SSSLoan { get; set; }
        public double PagibigLoan { get; set; }

        public double TotalDeduction { get; set; }
        public double NetPay { get; set; }

        public bool HasBenefitsDeductions { get; set; }
    }
}
