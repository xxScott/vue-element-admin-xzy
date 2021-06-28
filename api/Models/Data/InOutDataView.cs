using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class InOutDataView
    {
        public int ID { get; set; }
        public DateTime InOutDate { get; set; }
        public int OrgID{get;set;}
        public int ToOrgID { get; set; }
        public string Description { get; set; }
        public string SourceNo { get; set; }
        public List<InOutProductDataView> Products { get; set; }
    }
}
