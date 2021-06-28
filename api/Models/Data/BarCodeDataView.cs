using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class BarCodeDataView
    {
     
        public string StoreUID { get; set; }
        public string DeptCode { get; set; }
        public int? StartOrOver { get; set; }
        public string RandomCode { get; set; }
    }
}
