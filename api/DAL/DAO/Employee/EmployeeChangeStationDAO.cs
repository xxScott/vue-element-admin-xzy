using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class EmployeeChangeStationDAO : CObjectDAO<EmployeeChangeStation> { }

    public partial class EmployeeChangeStationViewDAO : CObjectViewDAO<EmployeeChangeStationView>
    {

        public DataTable Query(int state, string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            string sql = "select * from ( select changeTab.changeId as ID,changeTab.SourceDeptName,changeTab.[Description],changeTab.SourceStationName,changeTab.UpdateTime,";
            sql += " changeTab.ChangeDate,empTab.empid as EmployeeID,empTab.EmployeeName,empTab.EmployeeCode,empTab.IDNumber,empTab.MobilePhone,";
            sql += " empTab.Sex,empTab.DeptName,empTab.StationName,empTab.[State],empTab.StoreID from (select * from (select Emp_Base.[State],";
            sql += " Emp_Base.StoreID, Emp_Base.ID as eid,Emp_Base.EmployeeName,Emp_Base.MobilePhone,Emp_Base.IDNumber,Emp_Base.Sex,";
            sql += " Emp_Base.EmployeeCode,B_Dept.DeptName as DeptName from dbo.Emp_Base left outer join B_Dept on dbo.Emp_Base.DeptID=B_Dept.ID) tab1";
            sql += " left outer join (select Emp_Base.ID as empid, B_WorkStation.StationName as StationName";
            sql += " from Emp_Base left outer join B_WorkStation on Emp_Base.WorkStationID=B_WorkStation.ID) tab2 on tab1.eid=tab2.empid) empTab left outer join ";
            sql += " (select tab1.ID as changeId, tab1.EmployeeID,tab2.SourceDeptName,tab1.SourceStationName,tab1.UpdateTime,tab1.ChangeDate,tab1.Description as [Description]";
            sql += " from (select Emp_Change_Station.*,dbo.B_WorkStation.StationName as SourceStationName from dbo.Emp_Change_Station left outer join dbo.B_WorkStation";
            sql += " on Emp_Change_Station.DeptID=dbo.B_WorkStation.ID) tab1 left outer join (select Emp_Change_Station.ID as cid, B_Dept.DeptName as SourceDeptName";
            sql += " from dbo.Emp_Change_Station left outer join B_Dept on Emp_Change_Station.SourceDeptID=B_Dept.ID) tab2 on tab1.ID=tab2.cid ) changeTab on empTab.eid ";
            sql += " =changeTab.EmployeeID) dataTab   where dataTab.State=@0 and StoreID=@1 ";

            if (keyword != null & keyword != "")
            {
                sql += "and (SourceDeptName like '%" + keyword + "%' or SourceStationName like '%" + keyword + "%' or ChangeDate like '%" + keyword + "%' or EmployeeName like '%" + keyword + "%' ";
                sql += " or EmployeeCode like '%" + keyword + "%' or IDNumber like '%" + keyword + "%' or MobilePhone like '%" + keyword + "%' or DeptName like '%" + keyword + "%' or StationName like '%" + keyword + "%')";
            }
            //查询总条数
            totalCount = this.GetCount(sql, new object[] { state, storeId });
            sql = Util.GetSQL(pageNumber, pageSize, sql, "*", "UpdateTime DESC");
            try
            {
                using (IDbCommand command = MakeParamCommand(sql, new object[] { state, storeId }))
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
