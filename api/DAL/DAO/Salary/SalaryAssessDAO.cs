using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public partial class SalaryAssessDAO : CObjectDAO<SalaryAssess> { }

    public partial class SalaryAssessViewDAO : CObjectViewDAO<SalaryAssess>
    {

        public DataTable QueryEmp(int storeId, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            try
            {
                string sql = "select * from (select * from (select Emp_Base.*,DeptName,StationName from dbo.Emp_Base left outer join dbo.B_Dept on Emp_Base.DeptID=B_Dept.ID  ";
                sql += " left outer join B_WorkStation on Emp_Base.WorkStationID=B_WorkStation.ID where State=@0) empTab left outer join ";
                sql += " (select Salary_Assess.ID as AssessID,Salary_Assess.State as AssessState,Salary_Assess.EmployeeID from dbo.Salary_Assess where PeroidID=@1) assessTab on ";
                sql += "  empTab.ID=assessTab.EmployeeID where empTab.StoreID=@2) tab where 1=1 ";
                if (keyword != null & keyword != "")
                {
                    sql += "and (EmployeeCode like '%" + keyword + "%' or EmployeeName like '%" + keyword + "%' or IDNumber like '%" + keyword + "%' or MobilePhone like '%" + keyword + "%' ";
                    sql += " or DeptName like '%" + keyword + "%' or StationName like '%" + keyword + "%' or Descripiton like '%" + keyword + "%')";
                }


                totalCount = this.GetCount(sql, new object[] { Convert.ToInt16(Enum_EmployeeState.Pass), peroidId, storeId });
                sql = Util.GetSQL(pageNumber, pageSize, sql, "*", "UpdateTime DESC");

                using (IDbCommand command = MakeParamCommand(sql, new object[] { Convert.ToInt16(Enum_EmployeeState.Pass), peroidId, storeId }))
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

        public DataTable QueryAssess(int storeId, int state, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            try
            {
                string sql = "select * from (select Salary_Assess.ID as AssessID,Salary_Assess.UpdateTime,Salary_Assess.StoreID as StoreID,Salary_Assess.State as ";
                sql += " AssessState,Salary_Assess.EmployeeID from dbo.Salary_Assess where PeroidID=@1) assessTab left outer join (select Emp_Base.ID, Emp_Base.EmployeeCode,";
                sql += " Emp_Base.EmployeeName,Emp_Base.IDNumber,Emp_Base.Sex,Emp_Base.MobilePhone,DeptName,StationName from dbo.Emp_Base left outer join dbo.B_Dept on";
                sql += " Emp_Base.DeptID=B_Dept.ID left outer join B_WorkStation on Emp_Base.WorkStationID=B_WorkStation.ID ) empTab   on assessTab.EmployeeID= empTab.ID";
                sql += " where assessTab.StoreID=@2 and assessTab.AssessState=@0 ";


                if (keyword != null & keyword != "")
                {
                    sql += "and (EmployeeCode like '%" + keyword + "%' or EmployeeName like '%" + keyword + "%' or IDNumber like '%" + keyword + "%' or MobilePhone like '%" + keyword + "%' ";
                    sql += " or DeptName like '%" + keyword + "%' or StationName like '%" + keyword + "%' )";
                }


                totalCount = this.GetCount(sql, new object[] { state, peroidId, storeId });
                sql = Util.GetSQL(pageNumber, pageSize, sql, "*", "UpdateTime DESC");

                using (IDbCommand command = MakeParamCommand(sql, new object[] { state, peroidId, storeId }))
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

        public int GetCount(string sql, object[] param)
        {
            using (IDbCommand command = MakeParamCommand(sql, param))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt.Rows.Count;
                }
            }
        }

    }
}
