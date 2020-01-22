using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class SettingManager
    {
        private static SettingManager instance;
        public static SettingManager Instance
        {
            get
            {
                if (instance == null) instance = new SettingManager();
                return instance;
            }
        }

        public static double BasicRate
        {
            get
            {
                return GetValueNum("BasicRate");
            }
        }

        public static double PhilHealth
        {
            get
            {
                return GetValueNum("PhilHealth");
            }
        }

        public static double Pagibig
        {
            get
            {
                return GetValueNum("Pagibig");
            }
        }

        public static double ColaRate
        {
            get
            {
                return GetValueNum("ColaRate");
            }
        }

        public static string RelieverCheck
        {
            get
            {
                return GetValueStr("RelieverCheck");
            }
        }

        public static double WorkingDays(int year)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                string code = "WorkingDays_" + year;
                var val = db.Settings.Where(x => x.Code == code).First().Value;
                return double.Parse(val.ToString());
            }
        }

        private static double GetValueNum(string code)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                var val = db.Settings.Where(x => x.Code == code).First().Value;
                return double.Parse(val.ToString());
            }
        }

        private static string GetValueStr(string code)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                var val = db.Settings.Where(x => x.Code == code).First().Value;
                return val.ToString();
            }
        }
    }
}
