using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Objects
{
    //public class PayslipObject_old
    //{
    //    public string Period { get; set; }
    //    public string Employee { get; set; }
    //    public string Position { get; set; }
    //    public decimal NoOfDays { get; set; }
    //    public decimal BasicPay { get; set; }
    //    public decimal ECOLA { get; set; }
    //    public decimal NightDiff { get; set; }
    //    public decimal OTPay { get; set; }
    //    public decimal Allowances { get; set; }
    //    public decimal HolidayPay { get; set; }
    //    public decimal Adjustments { get; set; }
    //    public decimal GrossPay { get; set; }
    //    public decimal Late { get; set; }
    //    public decimal Undertime { get; set; }
    //    public decimal SSS { get; set; }
    //    public decimal PhilHealth { get; set; }
    //    public decimal Pagibig { get; set; }
    //    public decimal Uniform { get; set; }
    //    public decimal CashAdvance { get; set; }
    //    public decimal CashBond { get; set; }
    //    public decimal TotalDeduction { get; set; }
    //    public decimal NetPay { get; set; }
    //}

    public class PayslipObject
    {
        public string EmpName { get; set; }
        public string EmpPosition { get; set; }

        public decimal BasicRate { get; set; }
        public decimal BasicDays { get; set; }
        public decimal BasicPay { get; set; }

        public decimal ColaRate { get; set; }
        public decimal ColaDays { get; set; }
        public decimal ColaPay { get; set; }

        public decimal OTRate { get; set; }
        public decimal OTHours { get; set; }
        public decimal OTPay { get; set; }

        public decimal RDOTRate { get; set; }
        public decimal RDOTHours { get; set; }
        public decimal RDOTPay { get; set; }

        public decimal RDOTExcessRate { get; set; }
        public decimal RDOTExcessHours { get; set; }
        public decimal RDOTExcessPay { get; set; }

        public decimal NDRate { get; set; }
        public decimal NDHours { get; set; }
        public decimal NDPay { get; set; }

        public decimal LHRate { get; set; }
        public decimal LHHours { get; set; }
        public decimal LHPay { get; set; }

        public decimal LHExcessRate { get; set; }
        public decimal LHExcessHours { get; set; }
        public decimal LHExcessPay { get; set; }

        public decimal SHRate { get; set; }
        public decimal SHHours { get; set; }
        public decimal SHPay { get; set; }

        public decimal SHExcessRate { get; set; }
        public decimal SHExcessHours { get; set; }
        public decimal SHExcessPay { get; set; }

        public decimal LateRate { get; set; }
        public decimal LatePerMin { get; set; }
        public decimal LateDeductions { get; set; }

        public decimal SSS_EE { get; set; }
        public decimal PAGIBIG_EE { get; set; }
        public decimal PHILHEALTH_EE { get; set; }

        public decimal SSS_Loan { get; set; }
        public decimal PAGIBIG_Loan { get; set; }
        public decimal Adjustment { get; set; }
        public decimal Allowance { get; set; }
        public decimal CashAdvance { get; set; }
    }
}
