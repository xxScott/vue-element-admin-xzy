using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public partial class MakeClassPlanDAO : CObjectDAO<MakeClassPlan> { }

    public partial class MakeClassPlanViewDAO : CObjectViewDAO<MakeClassPlanView>
    {
        public DataTable Query(int makeId)
        {
            try
            {
                string sqldata = " select Hum_MakeClass_Data.* from dbo.Hum_MakeClass left outer join  dbo.Hum_MakeClass_Data on Hum_MakeClass.ID=Hum_MakeClass_Data.MakeClassID";
                sqldata += " where Hum_MakeClass.ID=@2 ";

                string sql = "select tab.ID as dataId,tab.UpperID,";
                for (int i = 0; i < 31; i++)
                {
                    sql += "tab.ClassID" + (i + 1) + ",";
                }
                sql += " empData.* from (select Emp_Base.EmployeeName,Emp_Base.ID as EmpID,Hum_MakeClass.* from dbo.Emp_Base left outer join dbo.Hum_MakeClass on Emp_Base.DeptID=Hum_MakeClass.DeptID";
                sql += " where Hum_MakeClass.ID=@0 and Emp_Base.State=@1) empData left outer join(" + sqldata + " ) tab on empData.EmpID=tab.EmployeeID ";



                using (IDbCommand command = MakeParamCommand(sql, new object[] { makeId, Convert.ToInt16(Enum_EmployeeState.Pass), makeId }))
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
