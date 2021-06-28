using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class AttendData
    {
        public int StoreID { get; set; }
        public int DeptID { get; set; }
        public int EmployeeID { get; set; }
        public int ClassID { get; set; }
        public string StartTime { get; set; }
        public string OverTime { get; set; }
        public string FactStartTime { get; set; }
        public string FactOverTime { get; set; }
        public int BeLate { get; set; }
        public int LeaveEarlier { get; set; }
        public bool StayAway { get; set; }
    }
}
