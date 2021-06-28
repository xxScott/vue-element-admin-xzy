using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class SalaryAssessView
    {
        public int ID { get; set; }
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public int? EmployeeID { get; set; }
        public int? PeroidID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? State { get; set; }
        public double TotalAmount { get; set; }
        public double DeductAmount { get; set; }
        public double RealAmount { get; set; }
        public SalaryAssessData AssessData { get; set; }
    }
}
