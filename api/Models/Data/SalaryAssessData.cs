using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class SalaryAssessData
    {
        public int ID { get; set; }
        public int? AssessID { get; set; }
        public int? SalaryProjectID { get; set; }
        public string SalaryProjectName { get; set; }
        public int? SalaryProjectProperty { get; set; }
        public int? SalaryProjectSource { get; set; }
        public string SalaryDesignFormulas { get; set; }
        public int? SalaryProjectOrder { get; set; }
        public double SalaryAmount { get; set; }
    }
}
