using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class SalaryChangeData
    {
        public int? ID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? EffectDate { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public List<SalaryProject> changes { get; set; }
    }
}
