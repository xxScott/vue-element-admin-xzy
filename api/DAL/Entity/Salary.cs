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
    [Table("Salary_Base")]
    [Description("工资基础表")]
    public class SalaryBase : BusinessEntity
    {
        public int? CompanyID { get; set; }
        public int? EmployeeID { get; set; }
        public int? SalaryProjectID { get; set; }
        public double SalaryAmount { get; set; }
        public string Description { get; set; }
        public int? State { get; set; }
    }



    [Serializable]
    [Table("Salary_Change")]
    [Description("工资调整表")]
    public class SalaryChange : BusinessEntity
    {
        public int? EmployeeID { get; set; }
        public DateTime? EffectDate { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Table("Salary_Change_Project")]
    [Description("工资调整内容表")]
    public class SalaryChangeProject : BusinessEntity
    {
        public int? ChangeID { get; set; }
        public int? SourceProjectID { get; set; }
        public double SourceAmount { get; set; }
        public int? SalaryProjectID { get; set; }
        public double SalaryAmount { get; set; }
    }
    [Serializable]
    [Table("Salary_Assess")]
    [Description("工资考评表")]
    public class SalaryAssess : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public int? EmployeeID { get; set; }
        public int? PeroidID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? State { get; set; }
        public double? TotalAmount { get; set; }
        public double? DeductAmount { get; set; }
        public double? RealAmount { get; set; }

    }

    [Serializable]
    [Table("Salary_AssessData")]
    [Description("工资考评内容表")]
    public class SalaryAssessData : BusinessEntity
    {
        public int? AssessID { get; set; }
        public int? SalaryProjectID { get; set; }
        public string SalaryProjectName { get; set; }
        public int? SalaryProjectProperty { get; set; }
        public int? SalaryProjectSource { get; set; }
        public string SalaryDesignFormulas { get; set; }
        public int? SalaryProjectOrder { get; set; }
        public double SalaryAmount { get; set; }
    }



    [Serializable]
    [Table("Salary_Settle")]
    [Description("工资结算表")]
    public class SalarySettle : BusinessEntity
    {
        public int? CompanyID { get; set; }
        public int? PeroidID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? TotalAmout { get; set; }
        public double? DeductAmount { get; set; }
        public double? SettleAmount { get; set; }
        public int? State { get; set; }
        public bool HasSettled { get; set; }
    }
}
