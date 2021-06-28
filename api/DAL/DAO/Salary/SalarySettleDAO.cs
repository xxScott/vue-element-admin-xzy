using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{



    public partial class SalarySettleDAO : CObjectDAO<SalarySettle> { }

    public partial class SalarySettleViewDAO : CObjectViewDAO<SalarySettle>
    {
        public DataTable Query(int storeId, int State)
        {
            try
            {
                string sql = " select Salary_Assess.ID,Salary_Assess.TotalAmount,Salary_Assess.UpdateTime,Salary_Assess.DeductAmount,Salary_Assess.RealAmount,Salary_Assess.State, ";
                sql += " empTab.*,perTab.PeriodName,perTab.ID as PID,perTab.StartDate,perTab.EndDate,perTab.HasSettled from dbo.Salary_Assess left outer join (select Emp_Base.ID as EmployeeID,Emp_Base.EmployeeName, ";
                sql += " DeptName,StationName from Emp_Base left outer join B_Dept on Emp_Base.DeptID=B_Dept.ID left outer join B_WorkStation on Emp_Base.WorkStationID=B_WorkStation.ID ";
                sql += " )empTab on Salary_Assess.EmployeeID=empTab.EmployeeID left outer join (select B_Period.ID,B_Period.PeriodName, B_Period.StartDate,B_Period.EndDate,Salary_Settle.HasSettled from  B_Period left outer join ";
                sql += " Salary_Settle on B_Period.ID=Salary_Settle.PeroidID)  perTab on Salary_Assess.PeroidID =perTab.ID ";
                sql += " where Salary_Assess.StoreID=@0 and State=@1 ";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { storeId, State }))
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
