using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Objects
{
    public class InvoiceObject
    {
        public string Description { get; set; }        
        public double VatableSale { get; set; }
        public double VatAmount { get; set; }
        public double Total { get; set; }
    }
}
