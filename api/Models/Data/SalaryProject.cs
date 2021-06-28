using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class SalaryProject
    {
        public int? ChangeID { get; set; }
        public int? SourceProjectID { get; set; }
        public double SourceAmount { get; set; }
        public int? SalaryProjectID { get; set; }
        public double SalaryAmount { get; set; }
    }
}
