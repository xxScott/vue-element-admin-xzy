using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class SalaryDataView
    {
        public int ID { get; set; }
        public int? CompanyID { get; set; }
        public int? EmployeeID { get; set; }
        public int? SalaryProjectID { get; set; }
        public double SalaryAmount { get; set; }
        public string Description { get; set; }
    
      
    }
}
