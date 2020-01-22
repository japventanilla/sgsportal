using SGS.Business.Objects;
using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class ClientManager
    {
        private static ClientManager instance;
        public static ClientManager Instance
        {
            get
            {
                if (instance == null) instance = new ClientManager();
                return instance;
            }
        }

        public List<ClientObject> GetClients()
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Clients
                        where x.RecState == "A"
                        select new ClientObject
                        {
                            Id = x.ClientId,
                            Code = x.ClientCode,
                            Name = x.ClientName,
                            Address = x.Address,
                            AgencyFee = x.AgencyFee,
                            GovtRemitType = x.GovtRemitType,
                            SeparationPay = x.SeparationPay == null ? 0 : x.SeparationPay.Value,
                            CutOffs = x.CutOffs,
                            GovtRemitDeductCutOff = x.GovtRemitDeductCutOff
                        }).ToList();
            }
        }

        public ClientObject GetClient(string code)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Clients
                        where x.RecState == "A" && x.ClientCode == code
                        select new ClientObject
                        {
                            Id = x.ClientId,
                            Code = x.ClientCode,
                            Name = x.ClientName,
                            Address = x.Address,
                            AgencyFee = x.AgencyFee,
                            GovtRemitType = x.GovtRemitType,
                            CutOffs = x.CutOffs,
                            GovtRemitDeductCutOff = x.GovtRemitDeductCutOff
                        }).FirstOrDefault();
            }
        }

        public List<string> GetClientsFromTimeSheet()
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return  db.TimeSheets.Select(x => x.Client).Distinct().ToList();
            }
        }

        public List<string> SetCutOffFilters(string cutoffs, int NumOfCutoffs)
        {
            string[] cutoff = cutoffs.Split(',');
            List<string> result = new List<string>();

            DateTime date1 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + cutoff[0]);
            DateTime date2 = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + cutoff[1]);

            DateTime dt = DateTime.UtcNow.AddHours(8);

            if (dt <= date1)
                dt = date2;
            else
                dt = date1;

            //if (dt > date1 && dt < date2)
            //    dt = date2;
            //else
            //    dt = date1;

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
    }

    
}
