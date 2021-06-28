using DAL.Data;
using MyOrm.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace DAL
{
    [Serializable]
    [Table("B_Group")]
    [Description("公司信息")]
    public class Groups : BusinessEntity
    {
        //public int? GroupID { get; set; }
        //public int? StoreID { get; set; }
        public int? UID { get; set; }
        public string GroupName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string Telephone { get; set; }
     
        public string Description { get; set; }
        public string LevelCode { get; set; }
    }
    [Serializable]
    [Table("B_Store")]
    [Description("门店信息")]
    public class Stores : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? UID { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
     
        public string Description { get; set; }
        public string LevelCode { get; set; }
    }
    [Serializable]
    [Table("B_Company_User")]
    [Description("公司用户信息")]
    public class CompanyUsers : EntityBase
    {
        public int? CompanyID { get; set; }
        public int? UserID { get; set; }
    }

    [Serializable]
    [Table("B_Company")]
    [Description("公司信息")]
    public class Companys : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int? UpperCompanyID { get; set; }
        public string Description { get; set; }
        public string LevelCode { get; set; }
    }

    [Serializable]
    [Table("B_Dept")]
    [Description("部门信息")]
    public class Depts : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public int? UpperDeptID { get; set; }
        public int? DeptProperty { get; set; }
        public string EmpCodePrefix { get; set; }
        public string LevelCode { get; set; }
        public string Description { get; set; }
        public int? DeptOrder { get; set; }
    }

    [Serializable]
    [Table("B_WorkStation")]
    [Description("岗位信息")]
    public class WorkStations : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Table("B_Formation")]
    [Description("编制信息")]
    public class Formations : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public int? DeptID { get; set; }
        public int? StationID { get; set; }
        public int? PersonNum { get; set; }
    }

    [Serializable]
    [Table("B_Class")]
    [Description("班次信息")]
    public class Classes : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(Depts))]
        public int? DeptID { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int? StartHour { get; set; }
        public int? StartMinute { get; set; }
        public int? OverHour { get; set; }
        public int? OverMinute { get; set; }
        public int? RestTypeID { get; set; }
        public int? RestDays { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("班次信息视图")]
    public class ClassesView : Classes
    {
        [ForeignColumn(typeof(Depts))]
        public string DeptName { get; set; }
    }

    [Serializable]
    [Table("B_SalaryProject")]
    [Description("薪资科目信息")]
    public class SalaryProjects : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectProperty { get; set; }
        public int? ProjectSource { get; set; }
        public string DesignFormulas { get; set; }
        public int? ProjectOrder { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Table("B_Period")]
    [Description("期间信息")]
    public class Periods : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string PeriodName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}