using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class EntryDataView
    {
        public int ID { get; set; }
        public int? State { get; set; }
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string EmployeePic { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string IDNumber { get; set; }
        public int? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Nation { get; set; }
        public string NativePlace { get; set; }
        public int? PoliticalStatus { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public int? EducationDegree { get; set; }
        public string MajorName { get; set; }
        public int? MarriageStatus { get; set; }
        public bool HasDisease { get; set; }
        public string DiseaseNotes { get; set; }
        public bool HasDefect { get; set; }
        public string DefectNotes { get; set; }
        public int? DeptID { get; set; }
        public int? WorkStationID { get; set; }
        public string SalaryBankName { get; set; }
        public string SalaryCardNo { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyRelation { get; set; }
        public string Description { get; set; }
        public List<SalaryDataView> Salarys { get; set; }
    }
}
