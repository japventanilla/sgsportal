using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Objects
{
    public class PayslipsObject
    {
        public string Period { get; set; }
        public string Employee { get; set; }
        public string Position { get; set; }
        public double NoOfDays { get; set; }
        public double BasicPay { get; set; }
        public double ECOLA { get; set; }
        public double NightDiff { get; set; }
        public double OTPay { get; set; }
        public double Allowances { get; set; }
        public double HolidayPay { get; set; }
        public double Adjustments { get; set; }
        public double GrossPay { get; set; }
        public double Late { get; set; }
        public double Undertime { get; set; }
        public double SSS { get; set; }
        public double PhilHealth { get; set; }
        public double Pagibig { get; set; }
        public double Uniform { get; set; }
        public double CashAdvance { get; set; }
        public double CashBond { get; set; }
        public double TotalDeduction { get; set; }
        public double NetPay { get; set; }
    }
}
