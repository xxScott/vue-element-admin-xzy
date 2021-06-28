using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    

    public partial class EmployeeLeaveDAO : CObjectDAO<EmployeeLeave> { }

    public partial class EmployeeLeaveViewDAO : CObjectViewDAO<EmployeeLeaveView>
    {
        public DataTable GetAll()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from dbo.Emp_Change_Duty ", null))
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
        public DataTable Query(int state, string companyId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            string sql = "select * from ( select changeTab.changeId as ID,changeTab.SourceDeptName,changeTab.[Description],changeTab.SourceStationName,changeTab.UpdateTime,";
           

            if (keyword != null & keyword != "")
            {
                sql += "and (SourceDeptName like '%" + keyword + "%' or SourceStationName like '%" + keyword + "%' or ChangeDate like '%" + keyword + "%' or EmployeeName like '%" + keyword + "%' ";
                sql += " or EmployeeCode like '%" + keyword + "%' or IDNumber like '%" + keyword + "%' or MobilePhone like '%" + keyword + "%' or DeptName like '%" + keyword + "%' or StationName like '%" + keyword + "%')";
            }
            //查询总条数
            totalCount = this.GetCount(sql, new object[] { state, companyId });
            sql = Util.GetSQL(pageNumber, pageSize, sql, "*", "UpdateTime DESC");
            try
            {
                using (IDbCommand command = MakeParamCommand(sql, new object[] { state, companyId }))
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
