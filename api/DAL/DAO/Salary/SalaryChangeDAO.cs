using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class SalaryChangeDAO : CObjectDAO<SalaryChange> { }

    public partial class SalaryChangeViewDAO : CObjectViewDAO<SalaryChange>
    {
        public DataTable QueryIncrease(int empId, int property)
        {
            try
            {
                string sql = " select * from (select B_SalaryProject.*,tab.ID as salaryId,tab.SalaryAmount,tab.Description as salaryDescription,tab.EmployeeID  from dbo.B_SalaryProject left outer ";
                sql += " join (select * from Salary_Base where EmployeeID=@0) tab on B_SalaryProject.ID =tab.SalaryProjectID) sourceTab left outer join (  ";
                sql += " select Salary_Change_Project.ID as changeDataID, Salary_Change_Project.SourceProjectID,Salary_Change_Project.SourceAmount, ";
                sql += " Salary_Change_Project.SalaryProjectID as ChangeProjectID,Salary_Change_Project.SalaryAmount as ChangeAmount,ctab.ID as ";
                sql += "  ChangeID,ctab.EffectDate,ctab.EmployeeID,ctab.Reason,ctab.Description from (select * from Salary_Change where dbo.Salary_Change.EmployeeID=@0) ctab  left outer join ";
                sql += "  Salary_Change_Project on ctab.ID=Salary_Change_Project.ChangeID) targetTab on sourceTab.ID=targetTab.SourceProjectID ";
                sql += "  where ProjectProperty=@1";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { empId, property }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable QueryEvaluation(int empId, int property, int periodId)
        {
            try
            {
                string sql = " select * from (   select proTab.ID ,proTab.ProjectName,tab.* from ( select B_SalaryProject.ID ,B_SalaryProject.ProjectName,B_SalaryProject.ProjectProperty, ";
                sql += " case  when  GroupID=-1  then StoreID else GroupID end as GroupID from B_SalaryProject  ) proTab inner join(  select Emp_Base.EmployeeName,asstab.ID as AssessID,";
                sql += "   asstab.PeroidID,case when Emp_Base.GroupID=-1 then Emp_Base.StoreID else Emp_Base.GroupID end as GroupID,asstab.State as AssessState";
                sql += " from Emp_Base left outer join (select * from Salary_Assess where PeroidID=@1) asstab ";
                sql += " on Emp_Base.ID=asstab.EmployeeID where Emp_Base.ID=@0)  tab on  proTab.GroupID=tab.GroupID ";
                sql += " where proTab.ProjectProperty=@2) empTab left outer join (select Salary_AssessData.SalaryAmount,Salary_AssessData.SalaryProjectID, ";
                sql += " Salary_AssessData.ID as DataID from Salary_AssessData left outer join Salary_Assess on Salary_AssessData.AssessID=Salary_Assess.ID ";
                sql += "   where Salary_Assess.EmployeeID=@0 and PeroidID=@1) dataTab on empTab.ID=dataTab.SalaryProjectID  ";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { empId, periodId, property }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public DataTable Query(int groupId, int storeId)
        {
            try
            {
                string sql = " select * from dbo.B_SalaryProject where GroupID=@0 and StoreID=@1";
                using (IDbCommand command = MakeParamCommand(sql, new object[] { groupId, storeId }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable QueryChange(int empId)
        {
            try
            {
                string sql = "select * from (select B_SalaryProject.*,tab.ID as salaryId,tab.SalaryAmount,tab.Description as salaryDescription  from dbo.B_SalaryProject left outer ";
                sql += " join (select * from Salary_Base where EmployeeID=@0) tab on B_SalaryProject.ID =tab.SalaryProjectID ) tab left outer join (";
                sql += " select b.EmployeeID,Salary_Change_Project.SourceAmount,Salary_Change_Project.SourceProjectID,Salary_Change_Project.SalaryAmount from ";
                sql += "( select top 1 * from dbo.Salary_Change where EmployeeID=@0 order by EffectDate desc) b ";
                sql += " inner join dbo.Salary_Change_Project on b.ID=Salary_Change_Project.ChangeID) tab1 on tab.ID=tab1.SourceProjectID   where ProjectProperty=@1";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { empId, Convert.ToInt16(Enum_SalaryProjectProperty.Increase) }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
