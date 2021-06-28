using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using MyOrm.Common;

namespace DAL
{
    [Serializable]
    [Table("Emp_Base")]
    [Description("员工信息")]
    public class EmployeeBase : BusinessEntity
    {
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
        [ForeignType(typeof(Depts))]
        public int? DeptID { get; set; }
        [ForeignType(typeof(WorkStations))]
        public int? WorkStationID { get; set; }
        public string SalaryBankName { get; set; }
        public string SalaryCardNo { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyRelation { get; set; }
        public string Description { get; set; }
    }


    [Serializable]
    [Description("员工信息视图")]
    public class EmployeeBaseView : EmployeeBase
    {
        [ForeignColumn(typeof(Depts))]
        public string DeptName { get; set; }
        [ForeignColumn(typeof(WorkStations))]
        public string StationName { get; set; }
    }


    [Serializable]
    [Table("Emp_Relation")]
    [Description("员工关系表")]
    public class EmployeeRalation : BusinessEntity
    {
        [ForeignType(typeof(EmployeeBase))]
        public int? EmployeeID { get; set; }
        public string RelationType { get; set; }
        public string RelationName { get; set; }
        public int? RelationSex { get; set; }
        public string RelationPhone { get; set; }
        public string RelationWorkCompany { get; set; }
        public string RelationAddress { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("员工关系视图")]
    public class EmployeeRalationView : EmployeeRalation
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeCode { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeName { get; set; }
    }
    [Serializable]
    [Table("Emp_Contract")]
    [Description("员工合同表")]
    public class EmployeeContract : BusinessEntity
    {
     
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(EmployeeBase))]
        public int? EmployeeID { get; set; }
        public string ContractNo { get; set; }
        public int? ContractType { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("员工合同视图")]
    public class EmployeeContractView : EmployeeContract
    {
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeCode { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeName { get; set; }
    }

    [Serializable]
    [Table("Emp_Change_Station")]
    [Description("员工岗位调整表")]
    public class EmployeeChangeStation : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(EmployeeBase))]
        public int? EmployeeID { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? SourceDeptID { get; set; }
        public int? SourceWorkStationID { get; set; }
        public int? DeptID { get; set; }
        public int? WorkStationID { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("员工岗位调整视图")]
    public class EmployeeChangeStationView : EmployeeChangeStation
    {
        [ForeignColumn(typeof(EmployeeBase))]
        public string GroupID { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string StoreID { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeCode { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeName { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string IDNumber { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public int? Sex { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string MobilePhone { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public int? State { get; set; }
    }
    [Serializable]
    [Table("Emp_Award")]
    [Description("员工奖惩信息表")]
    public class EmployeeAward : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(EmployeeBase))]
        public int? EmployeeID { get; set; }
        public int? AwardType { get; set; }
        public DateTime? AwardDate { get; set; }
        public double AwardAmount { get; set; }
        public string Reason { get; set; }
        public string Decision { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("员工奖惩信息视图")]
    public class EmployeeAwardView : EmployeeAward
    {
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeCode { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeName { get; set; }
    }

    [Serializable]
    [Table("Emp_Change_Duty")]
    [Description("员工离职信息表")]
    public class EmployeeLeave : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(EmployeeBase))]
        public int? EmployeeID { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public int? LevelStatus { get; set; }
        public string LevelReason { get; set; }
        public int? State { get; set; }
        public string Description { get; set; }
    }
    [Serializable]
    [Description("员工离职申请视图")]
    public class EmployeeLeaveView : EmployeeLeave
    {
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeCode { get; set; }
        [ForeignColumn(typeof(EmployeeBase))]
        public string EmployeeName { get; set; }
        //[ForeignColumn(typeof(EmployeeBase))]
        //public string GroupID { get; set; }
        //[ForeignColumn(typeof(EmployeeBase))]
        //public string StoreID { get; set; }
    }
}
