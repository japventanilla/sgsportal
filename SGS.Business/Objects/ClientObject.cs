using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Objects
{
    public class ClientObject : BaseObject
    {
        public string Address { get; set; }
        public double AgencyFee { get; set; }
        public string GovtRemitType { get; set; }
        public double SeparationPay { get; set; }
        public string CutOffs { get; set; }
        public string GovtRemitDeductCutOff { get; set; }
    }
}
