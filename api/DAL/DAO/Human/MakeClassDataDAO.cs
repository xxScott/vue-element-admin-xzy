using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public partial class MakeClassDataDAO : CObjectDAO<MakeClassData> { }

    public partial class MakeClassDataViewDAO : CObjectViewDAO<MakeClassData>
    {
        public DataTable Query(int storeId, int periodId)
        {
            try
            {
                string sqlEmp = " select Emp_Base.EmployeeName,Emp_Base.ID as EmpID,makeTab.*  from dbo.Emp_Base left outer join (select * from dbo.Hum_MakeClass  where Hum_MakeClass.DeptID is null or Hum_MakeClass.DeptID=0 ";
                sqlEmp += " ) makeTab on Emp_Base.StoreID=makeTab.StoreID where Emp_Base.StoreID=@0 and Emp_Base.State=@1";

                string sqldata = " select Hum_MakeClass_Data.* from dbo.Hum_MakeClass left outer join Hum_MakeClass_Data on Hum_MakeClass.ID=Hum_MakeClass_Data.MakeClassID ";
                sqldata += " where (DeptID is null or DeptID=0) and Hum_MakeClass.PeriodID=@2";


                string sql = "select tab.ID as dataId,tab.UpperID,";
                for (int i = 0; i < 31; i++)
                {
                    sql += "tab.ClassID" + (i + 1) + ",";
                }
                sql += " empData.* from (" + sqlEmp + ") empData left outer join(" + sqldata + " ) tab on empData.EmpID=tab.EmployeeID ";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { storeId, Convert.ToInt16(Enum_EmployeeState.Pass), periodId }))
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



        public DataTable QueryData(int companyId, int periodId, int deptId)
        {
            try
            {

                string sql = " select Hum_MakeClass_Data.* from dbo.Hum_MakeClass left outer join Hum_MakeClass_Data on Hum_MakeClass.ID=Hum_MakeClass_Data.MakeClassID ";
                sql += "  where Hum_MakeClass.PeriodID=@0 and Hum_MakeClass.CompanyID=@1 and Hum_MakeClass.DeptID=@2 and  State=@3   ";
                using (IDbCommand command = MakeParamCommand(sql, new object[] { periodId, companyId, deptId, Convert.ToInt16(Enum_EmployeeState.Pass) }))
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
        public DataTable QueryData(int storeId, int periodId)
        {
            try
            {

                string sql = " select Hum_MakeClass_Data.* from dbo.Hum_MakeClass left outer join Hum_MakeClass_Data on Hum_MakeClass.ID=Hum_MakeClass_Data.MakeClassID ";
                sql += "  where Hum_MakeClass.PeriodID=@0 and Hum_MakeClass.StoreID=@1 and  State=@2   ";
                using (IDbCommand command = MakeParamCommand(sql, new object[] { periodId, storeId, Convert.ToInt16(Enum_EmployeeState.Pass) }))
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

        public DataTable QueryAll(string keyword)
        {
            try
            {
                string sql = "select Hum_MakeClass_Data.*,Emp_Base.EmployeeName,MakeClassTab.DeptName,MakeClassTab.State,MakeClassTab.PeriodName,MakeClassTab.Description from";
                sql += " dbo.Hum_MakeClass_Data left outer  join  (select Hum_MakeClass.*,B_Period.PeriodName,B_Dept.DeptName from Hum_MakeClass left outer join ";
                sql += " B_Period on Hum_MakeClass.PeriodID=B_Period.ID left outer join  dbo.B_Dept on Hum_MakeClass.DeptID=B_Dept.ID) MakeClassTab on ";
                sql += " Hum_MakeClass_Data.MakeClassID=MakeClassTab.ID left outer join Emp_Base on Hum_MakeClass_Data.EmployeeID=Emp_Base.ID";

                sql += "  where MakeClassTab.State=" + Convert.ToInt16(Enum_MakeClassState.Pass) + " and  (EmployeeName like '%" + keyword + "%' or DeptName like '%" + keyword + "%' or PeriodName like '%" + keyword + "%' or MakeClassTab.Description like '%" + keyword + "%')";
                using (IDbCommand command = MakeParamCommand(sql))
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
