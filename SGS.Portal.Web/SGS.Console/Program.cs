using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            SetCutOffFilters2("1,16",8);
            System.Console.Read();
        }

        static List<string> SetCutOffFilters2(string cutoffs, int NumOfCutoffs)
        {
            string[] cutoff = cutoffs.Split(',');
            List<string> result = new List<string>();

            DateTime date1 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + cutoff[0]);
            DateTime date2 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + cutoff[1]);

            DateTime dt = DateTime.UtcNow.AddHours(8);

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
                    System.Console.WriteLine(filter);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.AddMonths(-1).Year + "-" + dt.AddMonths(-1).Month + "-" + cutoff[0]);
                }
                else
                {
                    string dt1 = dt.ToString("MMM " + cutoff[0] + ", yyyy");
                    string dt2 = dt.ToString("MMM " + (int.Parse(cutoff[1]) - 1) + ", yyyy");
                    string filter = string.Format("{0} - {1}", dt1, dt2);
                    System.Console.WriteLine(filter);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.Year + "-" + dt.Month + "-" + cutoff[1]);
                }
            }

            return result;
        }

        static void PhilHealthContribution(double total)
        {
            double er = 160;
            double ee = 80;

            if(total <= 10000)
            {
                er = 137.5;
                ee = 137.5;
            }
            else if (total > 10000 && total < 40000)
            {
                er = (total * (2.75 / 100)) / 2;
                ee = (total * (2.75 / 100)) / 2;
            }
            else
            {
                er = 550;
                ee = 550;
            }

            System.Console.WriteLine("Total:" + total + " - ER:" + er + " - EE:" + ee);
        }

        static void SSSContribution(double total)
        {
            double r = 2250;
            double er = 160;
            double ee = 80;
            double ec = 10;
            double gap = 500;

            if(total < r)
                System.Console.WriteLine("ER:" + er + " - EE:" + ee);
            else
            {
                while(total >= r && total < (total + gap))
                {
                    r += gap;
                    er += 40;
                    ee += 20;
                    ec = er >= 1200 ? 30 : 10;

                    if (r > 15750) break;                 
                }

                System.Console.WriteLine("Total:" + total + " - ER:" + (er + ec) + " - EE:" + ee);
            }
        }

        static List<string> SetCutOffFilters()
        {
            string cutoffs = "6,21";
            string[] cutoff = cutoffs.Split(',');
            int fCounts = 18;
            List<string> result = new List<string>();

            DateTime date1 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + int.Parse(cutoff[0]));
            DateTime date2 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + int.Parse(cutoff[1]));
            DateTime dt = DateTime.UtcNow.AddHours(8);

            if (dt > date1 && dt < date2)
                dt = date2;
            else
                dt = date1;

            while (result.Count() < fCounts)
            {
                if(dt.Day == int.Parse(cutoff[1]))
                {
                    string dt1 = dt.AddMonths(-1).ToString("MMM " + cutoff[1] + ", yyyy");
                    string lastdayInMonth = DateTime.DaysInMonth(dt.AddMonths(-1).Year, dt.AddMonths(-1).Month).ToString();
                    string dt2 = int.Parse(cutoff[0]) != 1 ? dt.ToString("MMM " + (int.Parse(cutoff[0]) - 1) + ", yyyy") : dt.AddMonths(-1).ToString("MMM " + lastdayInMonth + ", yyyy");
                    string filter = string.Format("{0} - {1}", dt1, dt2);
                    System.Console.WriteLine(filter);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.AddMonths(-1).Year + "-" + dt.AddMonths(-1).Month + "-" + cutoff[0]);
                }
                else
                {
                    string dt1 = dt.ToString("MMM " + cutoff[0] + ", yyyy");
                    string dt2 = dt.ToString("MMM " + (int.Parse(cutoff[1]) - 1) + ", yyyy");
                    string filter = string.Format("{0} - {1}", dt1, dt2);
                    System.Console.WriteLine(filter);
                    result.Add(filter);
                    dt = DateTime.Parse(dt.Year + "-" + dt.Month + "-" + cutoff[1]);
                }
            }

            return result;
        }
    }
}
